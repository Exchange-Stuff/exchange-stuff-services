﻿using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Repository.Data;
using ExchangeStuff.Repository.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace ExchangeStuff.Repository.Repositories
{
    public class TransactionHistoryRepository : Repository<TransactionHistory>, ITransactionHistoryRepository
    {
        private readonly DbSet<TransactionHistory> _transactionHistoryRepository;

        public TransactionHistoryRepository(ExchangeStuffContext context) : base(context)
        {
            _transactionHistoryRepository = context.Set<TransactionHistory>();
        }
    }
}
