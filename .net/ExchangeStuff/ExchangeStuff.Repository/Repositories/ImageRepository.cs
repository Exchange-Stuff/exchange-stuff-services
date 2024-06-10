using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Repository.Data;
using ExchangeStuff.Repository.Repositories.Base;

namespace ExchangeStuff.Repository.Repositories;

public class ImageRepository : Repository<Image>, IImageRepository
{
    public ImageRepository(ExchangeStuffContext context) : base(context)
    {
    }
}
