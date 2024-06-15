using ExchangeStuff.Core.Common;
using ExchangeStuff.Service.Models.BanReasons;
using ExchangeStuff.Service.Models.Products;

namespace ExchangeStuff.Service.Models.ProductBanReports
{
    public class ProductBanReportViewModel : Auditable<Guid>
    {
        public ProductViewModel Product { get; set; }

        public BanReasonViewModel BanReason { get; set; }

        public bool IsApproved { get; set; }
    }
}

