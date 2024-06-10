using ExchangeStuff.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeStuff.Service.Models.Comments;

public class UpdateCommentModel : BaseEntity<Guid>
{
    [MaxLength(500)]
    public string Content { get; set; }
}
