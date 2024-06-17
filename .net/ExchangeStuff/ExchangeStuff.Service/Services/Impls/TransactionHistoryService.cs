using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Enums;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.CurrentUser.Users;
using ExchangeStuff.Service.Maps;
using ExchangeStuff.Service.Models.TransactionHistory;
using ExchangeStuff.Service.Services.Interfaces;

namespace ExchangeStuff.Service.Services.Impls
{
    public class TransactionHistoryService : ITransactionHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITransactionHistoryRepository _transactionHistoryRepository;
        private readonly IIdentityUser<Guid> _identityUser;

        public TransactionHistoryService(IIdentityUser<Guid> identityUser, IUnitOfWork unitOfWork)
        {
            _identityUser = identityUser;
            _unitOfWork = unitOfWork;
            _transactionHistoryRepository = _unitOfWork.TransactionHistoryRepository;
        }

        public async Task<bool> CreateTransactionHistory(CreateTransactionHistoryModel request)
        {
            try
            {
                TransactionHistory transactionHistory = new TransactionHistory
                {
                    Id = Guid.NewGuid(),
                    UserId = _identityUser.AccountId,
                    Amount = request.Amount,
                    IsCredit = request.IsCredit,
                    TransactionType = request.TransactionType
                };

                await _transactionHistoryRepository.AddAsync(transactionHistory);
                var result = await _unitOfWork.SaveChangeAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<TransactionHistoryViewModel>> GetAllTransactionHistory(int pageSize, int pageIndex, TransactionType? type = null)
        {
            try
            {
                List<TransactionHistory> listTransactionHistory = new List<TransactionHistory>();
                if (type.HasValue)
                {
                    listTransactionHistory = await _transactionHistoryRepository.GetManyAsync(
                        pageSize: pageSize, pageIndex: pageIndex, predicate: t => t.TransactionType.Equals(type), orderBy: t => t.OrderBy(t => t.CreatedOn));
                }
                else
                {
                    listTransactionHistory = await _transactionHistoryRepository.GetManyAsync(
                        pageSize: pageSize, pageIndex: pageIndex, orderBy: t => t.OrderBy(t => t.CreatedOn));
                }

                var result = AutoMapperConfig.Mapper.Map<List<TransactionHistoryViewModel>>(listTransactionHistory);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<TransactionHistoryViewModel>> GetListTransactionHistoryByUserId(int pageSize, int pageIndex, TransactionType? type = null)
        {
            try
            {
                List<TransactionHistory> listTransactionHistory = new List<TransactionHistory>();
                if (type.HasValue)
                {
                    listTransactionHistory = await _transactionHistoryRepository.GetManyAsync(
                        pageSize: pageSize, pageIndex: pageIndex, predicate: t => t.TransactionType.Equals(type) && t.UserId.Equals(_identityUser.AccountId), orderBy: t => t.OrderBy(t => t.CreatedOn));
                }
                else
                {
                    listTransactionHistory = await _transactionHistoryRepository.GetManyAsync(
                        pageSize: pageSize, pageIndex: pageIndex, predicate: t => t.UserId.Equals(_identityUser.AccountId), orderBy: t => t.OrderBy(t => t.CreatedOn));
                }

                var result = AutoMapperConfig.Mapper.Map<List<TransactionHistoryViewModel>>(listTransactionHistory);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TransactionHistoryViewModel> GetTransactionHistoryDetail(Guid transactionHistoryId)
        {
            try
            {
                TransactionHistory transactionHistory = new TransactionHistory();
                transactionHistory = await _transactionHistoryRepository.GetOneAsync(predicate: p => p.Id.Equals(transactionHistoryId));
                var result = AutoMapperConfig.Mapper.Map<TransactionHistoryViewModel>(transactionHistory);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
