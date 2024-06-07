using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeStuff.Service.Models.PurchaseTicket
{
    public class PurchaseTicketViewModel
    {
        public double Amount { get; set; }
        public Guid ProductId { get; set; }
        [MaxLength(10)]
        public string StudentId { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        public int Quantity { get; set; }
        [MaxLength(30)]
        public PurchaseTicketStatus Status { get; set; }
    }
}
