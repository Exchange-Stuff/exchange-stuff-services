﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeStuff.Service.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> createPaymentAsync(Guid userId, int amount);
    }
}
