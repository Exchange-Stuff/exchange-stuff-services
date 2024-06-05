using ExchangeStuff.Core.Common;

namespace ExchangeStuff.Core.Entities
{
    public class Token:Auditable<Guid>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public Guid AccountId { get; set; }
        public Account Account { get; set; }
    }
}
