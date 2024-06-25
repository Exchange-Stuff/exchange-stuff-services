using ExchangeStuff.Core.Common;
using System.ComponentModel.DataAnnotations;

namespace ExchangeStuff.Service.Models.Moderators
{
    public class ModeratorUpdateModel : BaseEntity<Guid>
    {
        [MaxLength(50)]
        public string? Name { get; set; }

        [MaxLength(50)]
        public string? Email { get; set; }

        [MaxLength(50)]
        public string? Password { get; set; }

        [MaxLength(500)]
        public string? Thumbnail { get; set; }
    }
}
