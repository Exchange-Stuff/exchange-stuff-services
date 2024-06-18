using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeStuff.Service.Models.Products
{
    public class UpdateProductViewModel : BaseEntity<Guid>
    {
        public ProductStatus ProductStatus { get; set; }
    }
}
