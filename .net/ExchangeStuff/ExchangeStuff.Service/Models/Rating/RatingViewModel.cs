using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Enums;

namespace ExchangeStuff.Service.Models.Rating;

public class RatingViewModel : BaseEntity<Guid>
{
    public string Content { get; set; }
    public EvaluateType EvaluateType { get; set; }
}
