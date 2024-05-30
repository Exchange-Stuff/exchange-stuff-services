using ExchangeStuff.Core.Common;
using System.ComponentModel.DataAnnotations;

namespace ExchangeStuff.Core.Entities
{
    public class Category:Auditable<Guid>
    {
        [MaxLength(30 )]
        public string Name { get; set; }
        public string Thumbnail { get; set; }
        public bool IsActived { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
