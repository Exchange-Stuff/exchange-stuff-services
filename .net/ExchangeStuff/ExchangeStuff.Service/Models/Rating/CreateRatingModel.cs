using ExchangeStuff.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ExchangeStuff.Service.Models.Rating;

public class CreateRatingModel
{
    public Guid PurchaseTicketId { get; set; }

    [MaxLength(100)]
    public string Content { get; set; }

    public EvaluateType EvaluateType { get; set; }
}
