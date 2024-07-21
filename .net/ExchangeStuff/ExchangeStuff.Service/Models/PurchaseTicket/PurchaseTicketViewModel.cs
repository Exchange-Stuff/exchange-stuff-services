using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Enums;
using ExchangeStuff.Service.Models.Products;
using System;
using System.ComponentModel.DataAnnotations;

namespace ExchangeStuff.Service.Models.PurchaseTicket
{
    public class PurchaseTicketViewModel : BaseEntity<Guid>
    {
        public double Amount { get; set; }
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public int Quantity { get; set; }
        public ProductViewModel Product { get; set; }
        public PurchaseTicketStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
