using System.ComponentModel.DataAnnotations;

namespace ExchangeStuff.Service.Models.Moderators
{
    public class ModeratorCreateModel
    {
        [MaxLength(50, ErrorMessage = "Max length 50 mate")]
        [Required(ErrorMessage = "Username required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password required")]
        public string Password { get; set; }

        [MaxLength(50, ErrorMessage = "Max length 50 mate")]
        [Required(ErrorMessage = "Name required")]
        public string Name { get; set; }
    }
}
