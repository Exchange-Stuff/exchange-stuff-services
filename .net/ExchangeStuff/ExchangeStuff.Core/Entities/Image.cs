using ExchangeStuff.Core.Common;

namespace ExchangeStuff.Core.Entities
{
    public class Image:Auditable<Guid>
    {
        public string Url { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
