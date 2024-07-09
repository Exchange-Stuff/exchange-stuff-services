using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Enums;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.CurrentUser.Users;
using ExchangeStuff.Service.Constants;
using ExchangeStuff.Service.Maps;
using ExchangeStuff.Service.Models.FinancialTickets;
using ExchangeStuff.Service.Models.TransactionHistory;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace ExchangeStuff.Service.Services.Impls
{
    public class FinancialTicketService : IFinancialTicketService
    {
        private readonly IIdentityUser<Guid> _identityUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IFinancialTicketsRepository _financialTicketsRepository;
        private readonly ITransactionHistoryRepository _transactionHistoryRepository;
        private readonly IUserBalanceRepository _userBalanceRepository;
        public FinancialTicketService(IIdentityUser<Guid> identityUser, IUnitOfWork unitOfWork)
        {
            _identityUser = identityUser;
            _unitOfWork = unitOfWork;
            _financialTicketsRepository = _unitOfWork.FinancialTicketsRepository;
            _userRepository = _unitOfWork.UserRepository;
            _transactionHistoryRepository = _unitOfWork.TransactionHistoryRepository;
            _userBalanceRepository = _unitOfWork.UserBalanceRepository;
        }

        public async Task<bool> CreateFinancialTicket(CreateFinancialTicketModel request)
        {
            var user = await _userRepository.GetOneAsync(predicate: u => u.Id.Equals(request.UserId), include: "UserBalance");
            if (user == null) throw new Exception("Not found user!");

            if (user.UserBalance.Balance < request.Amount) throw new Exception("Not enough blance!");

            FinancialTicket financialTicket = new FinancialTicket
            {
                UserId = request.UserId,
                Amount = request.Amount,
                ImageQRCode = request.ImageQRCode,
                Status = FinancialTicketStatus.Pending,
            };

            await _financialTicketsRepository.AddAsync(financialTicket);
            var userBl = await _userBalanceRepository.GetOneAsync(predicate: p => p.UserId.Equals(request.UserId));

            if (userBl != null)
            {
                userBl.Balance = userBl.Balance - request.Amount;
            }
            else
            {
                throw new Exception();
            }

            _userBalanceRepository.Update(userBl);


            TransactionHistory transactionHistory = new TransactionHistory
            {
                UserId = request.UserId,
                Amount = request.Amount,
                IsCredit = false,
                TransactionType = TransactionType.Financial
            };

            await _transactionHistoryRepository.AddAsync(transactionHistory);


            var result = await _unitOfWork.SaveChangeAsync();
            return result > 0;
        }

        public async Task<List<FinancialTicketViewModel>> GetAllFilter(int pageSize, int pageIndex, DateTime? from, DateTime? to, FinancialTicketStatus? status, int? sort)
        {
            try
            {
                if (sort != null && (sort > SortOption.SortFinancialTicket.Length - 1 || sort < 0)) throw new Exception("Error sort!");
                if (from != null && to != null && from > to || (from != null && to == null) || (from == null && to != null)) throw new Exception("Invalid time!");

                var listTicket = new List<FinancialTicket>();

                if (from == null && to == null && status == null && sort == null)
                {
                    listTicket = await _financialTicketsRepository.GetManyAsync(pageSize: pageSize, pageIndex: pageIndex, include: "User");
                }
                else if (from != null && to != null && status != null && sort != null)
                {
                    Expression<Func<FinancialTicket, bool>> time = query => query.CreatedOn >= from && query.CreatedOn <= to && query.Status == status;
                    Func<IQueryable<FinancialTicket>, IOrderedQueryable<FinancialTicket>> order = query => query.OrderBy(SortOption.SortFinancialTicket[(int)sort]);
                    listTicket = await _financialTicketsRepository.GetManyAsync(predicate: time, pageSize: pageSize, pageIndex: pageIndex, orderBy: order, include: "User");
                }
                else if (status != null && sort == null && from == null && to == null)
                {
                    Expression<Func<FinancialTicket, bool>> time = query => query.Status == status;
                    listTicket = await _financialTicketsRepository.GetManyAsync(predicate: time, pageSize: pageSize, pageIndex: pageIndex, include: "User");
                }
                else if (status != null && sort != null)
                {
                    Expression<Func<FinancialTicket, bool>> time = query => query.Status == status;
                    Func<IQueryable<FinancialTicket>, IOrderedQueryable<FinancialTicket>> order = query => query.OrderBy(SortOption.SortFinancialTicket[(int)sort]);
                    listTicket = await _financialTicketsRepository.GetManyAsync(predicate: time, pageSize: pageSize, pageIndex: pageIndex, orderBy: order, include: "User");
                }
                else if (status != null && from != null && to != null)
                {
                    Expression<Func<FinancialTicket, bool>> time = query => query.CreatedOn >= from && query.CreatedOn <= to && query.Status == status;
                    listTicket = await _financialTicketsRepository.GetManyAsync(predicate: time, pageSize: pageSize, pageIndex: pageIndex, include: "User");
                }
                else if (sort != null && from == null && to == null && status == null)
                {
                    Func<IQueryable<FinancialTicket>, IOrderedQueryable<FinancialTicket>> order = query => query.OrderBy(SortOption.SortFinancialTicket[(int)sort]);
                    listTicket = await _financialTicketsRepository.GetManyAsync(pageSize: pageSize, pageIndex: pageIndex, orderBy: order, include: "User");
                }
                else if (sort != null && from != null && to != null)
                {
                    Expression<Func<FinancialTicket, bool>> time = query => query.CreatedOn >= from && query.CreatedOn <= to;
                    Func<IQueryable<FinancialTicket>, IOrderedQueryable<FinancialTicket>> order = query => query.OrderBy(SortOption.SortFinancialTicket[(int)sort]);
                    listTicket = await _financialTicketsRepository.GetManyAsync(predicate: time, pageSize: pageSize, pageIndex: pageIndex, orderBy: order, include: "User");
                }
                else if (from != null && to != null && sort == null && status == null)
                {
                    Expression<Func<FinancialTicket, bool>> time = query => query.CreatedOn >= from && query.CreatedOn <= to;
                    listTicket = await _financialTicketsRepository.GetManyAsync(predicate: time, pageSize: pageSize, pageIndex: pageIndex, include: "User");
                }
                return AutoMapperConfig.Mapper.Map<List<FinancialTicketViewModel>>(listTicket);
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<List<FinancialTicketViewModel>> GetAllFinancialTicket(int pageSize, int pageIndex, FinancialTicketStatus? status = null!)
        {

            try
            {
                List<FinancialTicket> listTicket = new List<FinancialTicket>();
                if (status.HasValue)
                {
                    listTicket = await _financialTicketsRepository.GetManyAsync(pageSize: pageSize, pageIndex: pageIndex, predicate: p => p.Status == status, orderBy: p => p.OrderBy(p => p.CreatedOn), include: "User");

                }
                else
                {
                    listTicket = await _financialTicketsRepository.GetManyAsync(pageSize: pageSize, pageIndex: pageIndex, orderBy: p => p.OrderBy(p => p.CreatedOn), include: "User");

                }
                var result = AutoMapperConfig.Mapper.Map<List<FinancialTicketViewModel>>(listTicket);
                return result;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<List<FinancialTicketViewModel>> GetFinancialTicketByUserId(int pageSize, int pageIndex, FinancialTicketStatus? status = null!)
        {
            try
            {
                List<FinancialTicket> listTicket = new List<FinancialTicket>();
                if (status.HasValue)
                {
                    listTicket = await _financialTicketsRepository.GetManyAsync(
                        pageSize: 10, pageIndex: 1, predicate: p => p.Status == status && p.UserId.Equals(_identityUser.AccountId), orderBy: p => p.OrderBy(p => p.CreatedOn));
                }
                else
                {
                    listTicket = await _financialTicketsRepository.GetManyAsync(
                        pageSize: pageSize, pageIndex: pageIndex, predicate: p => p.UserId.Equals(_identityUser.AccountId), orderBy: p => p.OrderBy(p => p.CreatedOn));
                }

                var result = AutoMapperConfig.Mapper.Map<List<FinancialTicketViewModel>>(listTicket);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<FinancialTicketViewModel> GetFinancialTicketDetail(Guid financialTicketId)
        {
            try
            {
                FinancialTicket ticket = new FinancialTicket();
                ticket = await _financialTicketsRepository.GetOneAsync(predicate: p => p.Id == financialTicketId);
                var result = AutoMapperConfig.Mapper.Map<FinancialTicketViewModel>(ticket);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateFinancialTicket(UpdateFinancialTicketModel request)
        {
            try
            {
                FinancialTicket ticket = await _financialTicketsRepository.GetOneAsync(predicate: p => p.Id.Equals(request.Id), forUpdate: true);
                if (ticket == null)
                {
                    throw new Exception("Not found ticket");
                }
                else
                {
                    ticket.Status = request.Status;
                    _financialTicketsRepository.Update(ticket);
                    if (request.Status == FinancialTicketStatus.Approve)
                    {
                        TransactionHistory transactionHistory = new TransactionHistory
                        {
                            Id = Guid.NewGuid(),
                            UserId = ticket.UserId,
                            Amount = ticket.Amount,
                            IsCredit = false,
                            TransactionType = TransactionType.Financial,

                        };
                        await _transactionHistoryRepository.AddAsync(transactionHistory);
                    }
                    UserBalance userBalance = await _userBalanceRepository.GetOneAsync(predicate: b => b.UserId.Equals(ticket.UserId), forUpdate: true);
                    if (userBalance == null)
                    {
                        throw new Exception("User balance not found");
                    }
                    else
                    {
                        if (ticket.Amount <= userBalance.Balance)
                        {
                            userBalance.Balance -= ticket.Amount;
                            _userBalanceRepository.Update(userBalance);
                        }
                        else
                        {
                            throw new Exception("Not enough blance!");
                        }
                    }
                    // Update the user's balance
                    var result = await _unitOfWork.SaveChangeAsync();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            throw new NotImplementedException();
        }
    }
}
