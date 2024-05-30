using ExchangeStuff.Core.Common;
using System.ComponentModel.DataAnnotations;

namespace ExchangeStuff.Core.Entities
{
    public class Comment:Auditable<Guid>
    {
        [MaxLength(500)]
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public User User { get; set; }
        public Product Product { get; set; }
    }
}
