using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExchangeStuff.Core.Entities
{
    public class UserBalance
    {
        [Key, ForeignKey("User")]
        public Guid UserId { get; set; }
        public double Balance { get; set; }
        public User User { get; set; }
    }
}
