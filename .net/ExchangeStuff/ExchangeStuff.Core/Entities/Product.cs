using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ExchangeStuff.Core.Entities
{
    public class Product : Auditable<Guid>
    {
        [MaxLength(30)]
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Thumbnail { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        public bool IsActived { get; set; }
        public ProductStatus ProductStatus { get; set; }
        public ICollection<Image> Images { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
