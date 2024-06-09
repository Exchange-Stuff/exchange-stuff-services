using System.ComponentModel.DataAnnotations;

namespace ExchangeStuff.Service.Models.Admins
{
    public class AdminCreateModel
    {
        [StringLength(30, ErrorMessage = "30 > Username > 5", MinimumLength = 5)]
        public string Username { get; set; }

        [StringLength(30, ErrorMessage = "30 > Password > 8", MinimumLength = 8)]
        public string Password { get; set; }

        [StringLength(30, ErrorMessage = "30 > Name > 2", MinimumLength = 2)]
        public string Name { get; set; }
    }
}
