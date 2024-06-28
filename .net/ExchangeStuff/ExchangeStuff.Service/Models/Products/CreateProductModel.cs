using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Entities;

namespace ExchangeStuff.Service.Models.Products
{
    public class CreateProductModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Thumbnail { get; set; }
        public List<string> ImageUrls {  get; set; }
        public List<Guid> CategoryId { get; set; }


    }
}
