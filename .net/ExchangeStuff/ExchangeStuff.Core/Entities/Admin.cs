using ExchangeStuff.Core.Common;
using System.ComponentModel.DataAnnotations;

namespace ExchangeStuff.Core.Entities
{
    public class Admin : Auditable<Guid>
    {
        [MaxLength(30)]
        public string Username { get; set; }
        public string Password { get; set; }
        [MaxLength(30)]
        public string Email { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
    }
}
