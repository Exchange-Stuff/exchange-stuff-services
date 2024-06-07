using ExchangeStuff.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeStuff.Service.Services.Interfaces;

public interface IRatingSerivce
{
    Task<ActionResult> GetRatingByUserId(Guid userId); 
}
