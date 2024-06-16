using ExchangeStuff.Core.Common;

namespace ExchangeStuff.Core.Entities
{
    public class ProductBanReport : Auditable<Guid>
    {
        public Guid ProductId { get; set; }
        public Guid BanReasonId { get; set; }
        public Product Product { get; set; }
        public BanReason BanReason { get; set; }
        public bool IsApproved { get; set; }
    }
}
