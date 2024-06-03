using ExchangeStuff.Core.Common;

namespace ExchangeStuff.Service.Models.Products
{
    public class ProductViewModel:BaseEntity<Guid>
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Thumbnail { get; set; }
        public string Description { get; set; }
        
        // Add on more properties
    }
}
