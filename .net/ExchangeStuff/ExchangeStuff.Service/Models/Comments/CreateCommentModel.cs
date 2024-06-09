using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeStuff.Service.Models.Comments;

public class CreateCommentModel
{
    [MaxLength(500)]
    public string Content { get; set; }
    public Guid AccountId { get; set; }
    public Guid ProductId { get; set; }
}
