using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeStuff.Service.Models.Products
{
    public class ProductUserViewModel : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Thumbnail { get; set; }
        public ProductStatus ProductStatus { get; set; }
    }
}
