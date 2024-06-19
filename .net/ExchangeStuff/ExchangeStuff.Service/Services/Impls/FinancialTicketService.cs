using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Enums;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.CurrentUser.Users;
using ExchangeStuff.Service.Maps;
using ExchangeStuff.Service.Models.FinancialTickets;
using ExchangeStuff.Service.Models.TransactionHistory;
using ExchangeStuff.Service.Services.Interfaces;

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
            //try
            //{
            //    User user = await _userRepository.GetOneAsync(predicate: u => u.Id.Equals(_identityUser.AccountId));
            //    FinancialTicket financialTicket = new FinancialTicket
            //    {
            //        Id = Guid.NewGuid(),
            //        Amount = request.Amount,
            //        UserId = _identityUser.AccountId,
            //        IsCredit = true,
            //        Status = FinancialTicketStatus.Pending,
            //    };
            //    await _financialTicketsRepository.AddAsync(financialTicket);
            //    var result = await _unitOfWork.SaveChangeAsync();
            //    return result > 0 ? AutoMapperConfig.Mapper.Map<FinancialTicketViewModel>(financialTicket) : throw new Exception("Create financialTiket fail");

            //}
            //catch (Exception ex) {
            //    throw new Exception("Server error");
            //}

                throw new Exception("Server error");


        }

        public async Task<List<FinancialTicketViewModel>> GetAllFinancialTicket(int pageSize, int pageIndex, FinancialTicketStatus? status = null!)
        {

            try
            {
                List<FinancialTicket> listTicket = new List<FinancialTicket>();
                if (status.HasValue)
                {
                    listTicket = await _financialTicketsRepository.GetManyAsync(pageSize: pageSize, pageIndex: pageIndex, predicate: p => p.Status == status, orderBy: p => p.OrderBy(p => p.CreatedOn));

                }
                else
                {
                    listTicket = await _financialTicketsRepository.GetManyAsync(pageSize: pageSize, pageIndex: pageIndex, orderBy: p => p.OrderBy(p => p.CreatedOn));

                }
                var result = AutoMapperConfig.Mapper.Map<List<FinancialTicketViewModel>>(listTicket);
                return result;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<List<FinancialTicketViewModel>> GetFinancialTicketByUserId( int pageSize, int pageIndex, FinancialTicketStatus? status = null!)
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
                //FinancialTicket ticket = await  _financialTicketsRepository.GetOneAsync(predicate: p => p.Id.Equals(request.Id), forUpdate:true);
                //if (ticket == null)
                //{
                //    throw new Exception("Not found ticket");



                //}
                //else
                //{
                //    ticket.Status = request.Status;
                //    _financialTicketsRepository.Update(ticket);
                //    if (request.Status == FinancialTicketStatus.Approve)
                //    {
                //        TransactionHistory transactionHistory = new TransactionHistory
                //        {
                //            Id = Guid.NewGuid(),
                //            UserId = ticket.UserId,
                //            Amount = ticket.Amount,
                //            IsCredit = ticket.IsCredit,
                //            TransactionType = TransactionType.Financial,

                //        };
                //        await _transactionHistoryRepository.AddAsync(transactionHistory);
                //    }
                //    UserBalance userBalance = await _userBalanceRepository.GetOneAsync(predicate: b => b.UserId.Equals(ticket.UserId), forUpdate: true);
                //    if (userBalance == null)
                //    {
                //        throw new Exception("User balance not found");
                //    }
                //    else
                //    {
                //        if(ticket.IsCredit == true)
                //        {
                //            userBalance.Balance += ticket.Amount;

                //        }
                //        else
                //        {
                //            userBalance.Balance -= ticket.Amount;
                //        }
                //        _userBalanceRepository.Update(userBalance);
                //    }
                    // Update the user's balance
                   
                   

                    //var result = await _unitOfWork.SaveChangeAsync();
                    //return result > 0;
                //}

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            throw new NotImplementedException();
        }


    }
}
