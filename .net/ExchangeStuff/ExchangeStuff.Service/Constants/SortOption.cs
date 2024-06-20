using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeStuff.Service.Constants;

public static class SortOption
{
    public static readonly string[] SortFinancialTicket =  {
        "", 
        "Amount ASC","Amount DESC",
        "Status ASC", "Status DESC",
        "CreatedOn ASC", "CreatedOn DESC",
        "ModifiedOn ASC, ModifiedOn DESC"
    };
}
