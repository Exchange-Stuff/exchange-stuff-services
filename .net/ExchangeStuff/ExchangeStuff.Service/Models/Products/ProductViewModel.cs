using ExchangeStuff.Core.Common;
using ExchangeStuff.Service.Models.Images;

namespace ExchangeStuff.Service.Models.Products
{
    public class ProductViewModel : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Thumbnail { get; set; }
        public string Description { get; set; }
        public List<ImageViewModel> Images { get; set; }
        // Add on more properties
    }
}
