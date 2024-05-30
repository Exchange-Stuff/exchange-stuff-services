using ExchangeStuff.Core.Common;
using System.ComponentModel.DataAnnotations;

namespace ExchangeStuff.Core.Entities
{
    public class Campus:Auditable<Guid>
    {
        [MaxLength(30)]
        public string Name { get; set; }
        public bool IsActived { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
