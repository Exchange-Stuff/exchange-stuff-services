﻿using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ExchangeStuff.Service.Models.PurchaseTicket
{
    public class PurchaseTicketViewModel : Auditable<Guid>
    {
        public double Amount { get; set; }
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public int Quantity { get; set; }
        public PurchaseTicketStatus Status { get; set; }
    }
}