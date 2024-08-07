﻿using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeStuff.Service.Models.PurchaseTicket
{
    public class CreatePurchaseTicketModel
    {
        public double Amount { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
