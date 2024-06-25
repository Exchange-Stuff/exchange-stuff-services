using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ExchangeStuff.Service.Models.Users
{
    public class UserUpdateModel : BaseEntity<Guid>
    {
        [MaxLength(50)]
        public string? Name { get; set; }

        [MaxLength(30)]
        public string? Password { get; set; }

        [MaxLength(200)]
        public string? Thumbnail { get; set; }

        [MaxLength(10)]
        public string? StudentId { get; set; }

        [MaxLength(100)]
        public string? Address { get; set; }

        [MaxLength(12)]
        public string? Phone { get; set; }

        public GenderType? Gender { get; set; }
    }
}
