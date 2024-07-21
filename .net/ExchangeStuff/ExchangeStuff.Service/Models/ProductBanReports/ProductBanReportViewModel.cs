using ExchangeStuff.Core.Common;
using ExchangeStuff.Service.Models.BanReasons;
using ExchangeStuff.Service.Models.Products;
using ExchangeStuff.Service.Models.Users;

namespace ExchangeStuff.Service.Models.ProductBanReports
{
    public class ProductBanReportViewModel : Auditable<Guid>
    {
        public ProductViewModel Product { get; set; }

        public BanReasonViewModel BanReason { get; set; }

        public bool IsApproved { get; set; }

        public UserViewModel User  { get; set; }
    }
}

