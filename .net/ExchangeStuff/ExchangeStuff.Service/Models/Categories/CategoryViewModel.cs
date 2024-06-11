
using ExchangeStuff.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeStuff.Service.Models.Categories
{
    public class CategoryViewModel : BaseEntity<Guid>
    {
        public string Name { get; set; }
    }
}
