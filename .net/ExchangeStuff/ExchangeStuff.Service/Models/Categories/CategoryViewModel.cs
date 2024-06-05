using ExchangeStuff.Core.Common;

namespace ExchangeStuff.Service.Models.Categories
{
    public class CategoryViewModel:BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Thumbnail { get; set; }
    }
}
