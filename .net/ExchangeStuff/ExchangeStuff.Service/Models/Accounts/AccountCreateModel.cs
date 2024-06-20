using System.ComponentModel.DataAnnotations;

namespace ExchangeStuff.Service.Models.Accounts
{
    public class AccountCreateModel
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(30)]
        public string Password { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public List<Guid>? PermisisonGroupIds { get; set; }
    }
}
