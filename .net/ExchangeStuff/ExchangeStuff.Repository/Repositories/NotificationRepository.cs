﻿using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Repository.Data;
using ExchangeStuff.Repository.Repositories.Base;

namespace ExchangeStuff.Repository.Repositories
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(ExchangeStuffContext context) : base(context)
        {
        }
    }
}
