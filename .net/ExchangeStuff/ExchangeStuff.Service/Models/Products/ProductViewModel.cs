using ExchangeStuff.Core.Common;

namespace ExchangeStuff.Service.Models.Products
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Thumbnail { get; set; }
        public string Description { get; set; }
        
        // Add on more properties
    }
}
