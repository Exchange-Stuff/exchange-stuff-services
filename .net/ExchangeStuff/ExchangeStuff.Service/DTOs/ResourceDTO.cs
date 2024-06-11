using ExchangeStuff.Core.Common;

namespace ExchangeStuff.Service.DTOs
{
    public class ResourceDTO:BaseEntity<Guid>
    {
        public string Name { get; set; }

    }
}
