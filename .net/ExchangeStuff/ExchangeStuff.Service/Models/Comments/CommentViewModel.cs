﻿using ExchangeStuff.Core.Common;
using ExchangeStuff.Service.Models.Accounts;
using ExchangeStuff.Service.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeStuff.Service.Models.Comments;
public class CommentViewModel : Auditable<Guid>
{
    [MaxLength(500)]
    public string Content { get; set; }
    public AccountViewModel User { get; set; }

}
