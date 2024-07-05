using System.ComponentModel.DataAnnotations;

namespace ExchangeStuff.Service.Models.Notifications
{
    public class NotificationCreateModel
    {
        [Required]
        public string Message { get; set; }
        [Required]
        public Guid AccountId { get; set; }
    }
}
