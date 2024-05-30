﻿using ExchangeStuff.Core.Common;

namespace ExchangeStuff.Application.DTOs
{
    public class ClaimDTO:BaseEntity<Guid>
    {
        public string Email { get; set; }
        public string Role { get; set; }
        public Guid RoleId { get; set; }
    }
}
