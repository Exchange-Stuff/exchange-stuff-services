﻿using ExchangeStuff.Core.Common;
using System.ComponentModel.DataAnnotations;

namespace ExchangeStuff.Core.Entities
{
    public class Account : Auditable<Guid>
    {
        [MaxLength(50)]
        public string? Username { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? Thumbnail { get; set; }
        public ICollection<PermissionGroup> PermissionGroups { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
        public ICollection<Token>? Tokens { get; set; }
        public bool IsActived { get; set; }
    }
}
