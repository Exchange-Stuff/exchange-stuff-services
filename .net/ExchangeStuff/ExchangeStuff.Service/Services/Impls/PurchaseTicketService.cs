using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Enums;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.CurrentUser.Users;
using ExchangeStuff.Service.Maps;
using ExchangeStuff.Service.Models.PurchaseTicket;
using ExchangeStuff.Service.Services.Interfaces;

namespace ExchangeStuff.Service.Services.Impls
{
    public class PurchaseTicketService : IPurchaseTicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPurchaseTicketRepository _purchaseTicketRepository;
        private readonly IIdentityUser<Guid> _identityUser;

        public PurchaseTicketService(IIdentityUser<Guid> identityUser, IUnitOfWork unitOfWork)
        {
            _identityUser = identityUser;
            _unitOfWork = unitOfWork;
            _purchaseTicketRepository = _unitOfWork.PurchaseTicketRepository;
            _accountRepository = _unitOfWork.AccountRepository;
            _userRepository = _unitOfWork.UserRepository;
        }

        public async Task<PurchaseTicketViewModel> CreatePurchaseTicket(CreatePurchaseTicketModel request)
        {
            try
            {
                Account acc = await _accountRepository.GetOneAsync(predicate: a => a.Id.Equals(_identityUser.AccountId));
                User user = await _userRepository.GetOneAsync(predicate: u => u.Id.Equals(_identityUser.AccountId));
                PurchaseTicket purchaseTicket = new PurchaseTicket
                {
                    Id = Guid.NewGuid(),
                    Amount = request.Amount,
                    ProductId = request.ProductId,
                    StudentId = user.StudentId,
                    Email = acc.Email,
                    UserId = _identityUser.AccountId,
                    Quantity = request.Quantity,
                    Status = PurchaseTicketStatus.Processing
                };

                await _purchaseTicketRepository.AddAsync(purchaseTicket);
                var result = await _unitOfWork.SaveChangeAsync();
                return result > 0 ? AutoMapperConfig.Mapper.Map<PurchaseTicketViewModel>(purchaseTicket) : throw new Exception("Create purchase ticket fail");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<List<PurchaseTicketViewModel>> GetAllPurchaseTicket(int pageSize, int pageIndex, PurchaseTicketStatus? status = null!)
        {
            try
            {
                List<PurchaseTicket> listTicket = new List<PurchaseTicket>();
                if (status.HasValue)
                {
                    listTicket = await _purchaseTicketRepository.GetManyAsync(
                        pageSize: pageSize, pageIndex: pageIndex, predicate: p => p.Status.Equals(status), orderBy: p => p.OrderBy(p => p.CreatedOn));
                }
                else
                {
                    listTicket = await _purchaseTicketRepository.GetManyAsync(
                        pageSize: pageSize, pageIndex: pageIndex, orderBy: p => p.OrderBy(p => p.CreatedOn));
                }

                var result = AutoMapperConfig.Mapper.Map<List<PurchaseTicketViewModel>>(listTicket);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<PurchaseTicketViewModel>> GetListPurchaseTicketByUserId(int pageSize, int pageIndex, PurchaseTicketStatus? status = null!)
        {
            try
            {
                List<PurchaseTicket> listTicket = new List<PurchaseTicket>();
                if (status.HasValue)
                {
                    listTicket = await _purchaseTicketRepository.GetManyAsync(
                        pageSize: pageSize, pageIndex: pageIndex, predicate: p => p.Status.Equals(status) && p.UserId.Equals(_identityUser.AccountId), orderBy: p => p.OrderBy(p => p.CreatedOn));
                }
                else
                {
                    listTicket = await _purchaseTicketRepository.GetManyAsync(
                        pageSize: pageSize, pageIndex: pageIndex, predicate: p => p.UserId.Equals(_identityUser.AccountId), orderBy: p => p.OrderBy(p => p.CreatedOn));
                }

                var result = AutoMapperConfig.Mapper.Map<List<PurchaseTicketViewModel>>(listTicket);
                return result;
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

        public async Task<PurchaseTicketViewModel> UpdatePurchaseTicket(UpdatePurchaseTicketModel request)
        {
            try
            {
                PurchaseTicket ticket = await _purchaseTicketRepository.GetOneAsync(predicate: p => p.Id.Equals(request.Id));
                if (ticket == null)
                    throw new Exception("Not found ticket!");

                ticket.Status = request.Status;
                _purchaseTicketRepository.Update(ticket);
                var result = await _unitOfWork.SaveChangeAsync();
                return result > 0 ? AutoMapperConfig.Mapper.Map<PurchaseTicketViewModel>(ticket) : throw new Exception("Update purchase ticket fail");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
                
        }
    }
}
