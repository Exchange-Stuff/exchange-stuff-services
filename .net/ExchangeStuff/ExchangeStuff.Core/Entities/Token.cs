using ExchangeStuff.Core.Common;

namespace ExchangeStuff.Core.Entities
{
    public class Token:Auditable<Guid>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
