using ExchangeStuff.Core.Common;

namespace ExchangeStuff.Service.Models.Products
{
    public class CreateProductModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Thumbnail { get; set; }
        public List<Guid> CategoryId { get; set; }


    }
}
