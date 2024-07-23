using ExchangeStuff.Core.Enums;
using ExchangeStuff.Service.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeStuff.Service.Models.FinancialTickets
{
    public class FinancialTicketViewModel
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        public Guid UserId { get; set; }
        public UserViewModel User { get; set; }
        public string ImageQRCode { get; set; }
        public FinancialTicketStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
