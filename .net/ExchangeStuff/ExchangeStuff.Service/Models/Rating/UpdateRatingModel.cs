using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeStuff.Service.Models.Rating;

public class UpdateRatingModel : BaseEntity<Guid>
{
    public Guid PurchaseTicketId { get; set; }

    [MaxLength(100)]
    public string Content { get; set; }

    public EvaluateType EvaluateType { get; set; }
}
