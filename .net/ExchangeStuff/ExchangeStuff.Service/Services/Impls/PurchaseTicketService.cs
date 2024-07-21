using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Enums;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.CurrentUser.Users;
using ExchangeStuff.Service.Maps;
using ExchangeStuff.Service.Models.PurchaseTicket;
using ExchangeStuff.Service.Models.Users;
using ExchangeStuff.Service.Paginations;
using ExchangeStuff.Service.Services.Interfaces;
using System.Collections.Generic;

namespace ExchangeStuff.Service.Services.Impls
{
    public class PurchaseTicketService : IPurchaseTicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPurchaseTicketRepository _purchaseTicketRepository;
        private readonly IIdentityUser<Guid> _identityUser;
        private readonly ITransactionHistoryRepository _transactionHistoryRepository;
        private readonly IUserBalanceRepository _userBalanceRepository;
        private readonly IPostTicketRepository _postTicketRepository;
        private readonly IProductRepository _productRepository;

        public PurchaseTicketService(IIdentityUser<Guid> identityUser, IUnitOfWork unitOfWork)
        {
            _identityUser = identityUser;
            _unitOfWork = unitOfWork;
            _purchaseTicketRepository = _unitOfWork.PurchaseTicketRepository;
            _accountRepository = _unitOfWork.AccountRepository;
            _userRepository = _unitOfWork.UserRepository;
            _transactionHistoryRepository = _unitOfWork.TransactionHistoryRepository;
            _userBalanceRepository = _unitOfWork.UserBalanceRepository;
            _postTicketRepository = _unitOfWork.PostTicketRepository;
            _productRepository = _unitOfWork.ProductRepository;
        }

        public async Task<bool> CreatePurchaseTicket(CreatePurchaseTicketModel request)
        {
            User user = await _userRepository.GetOneAsync(predicate: u => u.Id.Equals(_identityUser.AccountId));
            if (user == null!) throw new UnauthorizedAccessException("Not found user");
            PurchaseTicket purchaseTicket = new PurchaseTicket
            {
                Amount = request.Amount,
                ProductId = request.ProductId,
                StudentId = user.StudentId + "",
                Email = user.Email + "",
                UserId = _identityUser.AccountId,
                Quantity = request.Quantity,
                Status = PurchaseTicketStatus.Processing
            };
            await _purchaseTicketRepository.AddAsync(purchaseTicket);

            TransactionHistory transactionHistory = new TransactionHistory
            {
                UserId = _identityUser.AccountId,
                Amount = request.Amount,
                IsCredit = false,
                TransactionType = TransactionType.Purchase
            };
            await _transactionHistoryRepository.AddAsync(transactionHistory);

            UserBalance balance = await _userBalanceRepository.GetOneAsync(predicate: b => b.UserId.Equals(_identityUser.AccountId), forUpdate: true);

            if (balance.Balance <= 0) throw new Exception("Not enough money");
            balance.Balance = balance.Balance - request.Amount;

            if (balance.Balance < 0) throw new Exception("Not enough money");
            _userBalanceRepository.Update(balance);

            var product = await _productRepository.GetOneAsync(x => x.Id == request.ProductId && x.IsActived, forUpdate:true);
            if (product.Quantity <= 0) throw new Exception("Out of stock");

            product.Quantity = product.Quantity - request.Quantity;
            if (product.Quantity < 0) throw new Exception("Out of stock");
            if (request.Quantity * product.Price != request.Amount) throw new Exception("Check sum invalid");
            _productRepository.Update(product);

            var result = await _unitOfWork.SaveChangeAsync();
            return result > 0;
        }

        public async Task<PaginationItem<PurchaseTicketViewModel>> GetAllPurchaseTicket(int pageSize, int pageIndex, PurchaseTicketStatus? status = null!)
        {
            try
            {
                List<PurchaseTicket> listTicket = new List<PurchaseTicket>();
                if (status.HasValue)
                {
                    listTicket = await _purchaseTicketRepository.GetManyAsync(
                        predicate: p => p.Status.Equals(status),
                        orderBy: p => p.OrderBy(p => p.CreatedOn));
                }
                else
                {
                    listTicket = await _purchaseTicketRepository.GetManyAsync(
                        orderBy: p => p.OrderBy(p => p.CreatedOn));
                }

                var result = AutoMapperConfig.Mapper.Map<List<PurchaseTicketViewModel>>(listTicket);
                return PaginationItem<PurchaseTicketViewModel>.ToPagedList(result, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PaginationItem<PurchaseTicketViewModel>> GetListPurchaseTicketByUserId(int pageSize, int pageIndex, PurchaseTicketStatus? status = null!)
        {
            try
            {
                List<PurchaseTicket> listTicket;
                if (status.HasValue)
                {
                    listTicket = await _purchaseTicketRepository.GetManyAsync(
                        predicate: p => p.Status.Equals(status) && p.UserId.Equals(_identityUser.AccountId),
                        orderBy: p => p.OrderBy(p => p.CreatedOn),
                        include: "Product");
                }
                else
                {
                    listTicket = await _purchaseTicketRepository.GetManyAsync(
                        predicate: p => p.UserId.Equals(_identityUser.AccountId),
                        orderBy: p => p.OrderBy(p => p.CreatedOn),
                        include: "Product");
                }

                var result = AutoMapperConfig.Mapper.Map<List<PurchaseTicketViewModel>>(listTicket);
                return PaginationItem<PurchaseTicketViewModel>.ToPagedList(result, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PurchaseTicketViewModel> GetPurchaseTicketDetail(Guid purchaseTicketId)
        {
            try
            {
                PurchaseTicket ticket = new PurchaseTicket();
                ticket = await _purchaseTicketRepository.GetOneAsync(predicate: p => p.Id.Equals(purchaseTicketId));
                var result = AutoMapperConfig.Mapper.Map<PurchaseTicketViewModel>(ticket);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdatePurchaseTicket(UpdatePurchaseTicketModel request)
        {
            try
            {
                PurchaseTicket ticket = await _purchaseTicketRepository.GetOneAsync(predicate: p => p.Id.Equals(request.Id));
                if (ticket == null)
                    throw new Exception("Not found ticket!");

                ticket.Status = request.Status;
                _purchaseTicketRepository.Update(ticket);

                if (request.Status.Equals(PurchaseTicketStatus.Cancelled))
                {
                    var product = await _productRepository.GetOneAsync(predicate: p => p.Id == ticket.ProductId);
                    if (product == null) throw new Exception("Not found product!");
                    product.Quantity += ticket.Quantity;
                    _productRepository.Update(product);
                    TransactionHistory transactionHistory = new TransactionHistory
                    {
                        Id = Guid.NewGuid(),
                        UserId = ticket.UserId,
                        Amount = ticket.Amount,
                        IsCredit = true,
                        TransactionType = TransactionType.Purchase
                    };
                    await _transactionHistoryRepository.AddAsync(transactionHistory);

                    UserBalance balance = await _userBalanceRepository.GetOneAsync(predicate: b => b.UserId.Equals(_identityUser.AccountId));
                    balance.Balance = balance.Balance + ticket.Amount;
                    _userBalanceRepository.Update(balance);
                }

                else if (request.Status.Equals(PurchaseTicketStatus.Confirmed))
                {
                    PostTicket postTicket = await _postTicketRepository.GetOneAsync(predicate: p => p.ProductId.Equals(ticket.ProductId));
                    TransactionHistory transactionHistory = new TransactionHistory
                    {
                        Id = Guid.NewGuid(),
                        UserId = postTicket.UserId,
                        Amount = ticket.Amount,
                        IsCredit = true,
                        TransactionType = TransactionType.Purchase
                    };
                    await _transactionHistoryRepository.AddAsync(transactionHistory);

                    UserBalance balance = await _userBalanceRepository.GetOneAsync(predicate: b => b.UserId.Equals(postTicket.UserId));
                    balance.Balance = balance.Balance + ticket.Amount;
                    _userBalanceRepository.Update(balance);
                }

                var result = await _unitOfWork.SaveChangeAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
