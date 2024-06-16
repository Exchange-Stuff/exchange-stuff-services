using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Repository.Data;
using ExchangeStuff.Repository.Repositories.Base;

namespace ExchangeStuff.Repository.Repositories
{
    public class ProductBanReportRepository : Repository<ProductBanReport>, IProductBanReportRepository
    {
        public ProductBanReportRepository(ExchangeStuffContext context) : base(context)
        {
        }
    }
}
