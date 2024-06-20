using ExchangeStuff.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeStuff.Service.Models.FinancialTickets
{
    public class CreateFinancialTicketModel
    {
        public double Amount { get; set; }
        public Guid UserId { get; set; }
        public  string ImageQRCode {  get; set; }
    }
}
