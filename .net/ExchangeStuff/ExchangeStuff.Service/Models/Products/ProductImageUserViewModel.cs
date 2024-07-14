using ExchangeStuff.Core.Common;
using ExchangeStuff.Service.Models.Images;
using ExchangeStuff.Service.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeStuff.Service.Models.Products
{
    public class ProductImageUserViewModel : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Thumbnail { get; set; }
        public string Description { get; set; }
        public List<ImageViewModel> Images { get; set; }
        public UserViewModel User { get; set; }
        // Add on more properties
    }
}
