using ExchangeStuff.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeStuff.Service.Models.FinancialTickets
{
    public class FinancialTicketViewModel
    {
        public double Amount { get; set; }
        public Guid UserId { get; set; }
        public  Boolean IsCredit { get; set; }
        public FinancialTicketStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Guid CreateBy { get; set; }
        public Guid ModifiedBy {  get; set; }
    }
}
