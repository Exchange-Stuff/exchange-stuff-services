using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Enums;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.CurrentUser.Users;
using ExchangeStuff.Repository.Repositories;
using ExchangeStuff.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeStuff.Service.Services.Impls
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdentityUser<Guid> _identityUser;
        private readonly ITransactionHistoryRepository _transactionHistoryRepository;
        private readonly IUserBalanceRepository _userBalanceRepository;

        public PaymentService(IUnitOfWork unitOfWork, IIdentityUser<Guid> identityUser)
        {
            _unitOfWork = unitOfWork;
            _paymentRepository = _unitOfWork.PaymentRepository;
            _identityUser = identityUser;
            _transactionHistoryRepository = _unitOfWork.TransactionHistoryRepository;
            _userBalanceRepository = _unitOfWork.UserBalanceRepository;
        }

        public async Task<bool> createPaymentAsync(Guid userId, int amount)
        {
            try
            {
                Payment payment = new Payment
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Amount = amount * 1000,
                    Status = "Đã thanh toán",
                    PaymentDate = DateTime.Now,
                    Description = "Nạp tiền"
                };
               
                await _paymentRepository.AddAsync(payment);


                TransactionHistory transactionHistory = new TransactionHistory
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Amount = amount,
                    IsCredit = true,
                    TransactionType = TransactionType.Payment
                };

                await _transactionHistoryRepository.AddAsync(transactionHistory);

                var userBl = await _userBalanceRepository.GetOneAsync(predicate: p => p.UserId.Equals(userId));

                if (userBl != null) 
                {
                    userBl.Balance = userBl.Balance + amount;
                }

                _userBalanceRepository.Update(userBl);

                var rs = await _unitOfWork.SaveChangeAsync();

                return rs > 0;

            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
