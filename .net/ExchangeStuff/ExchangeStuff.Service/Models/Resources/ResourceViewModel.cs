using ExchangeStuff.Core.Common;

namespace ExchangeStuff.Service.Models.Resources
{
    public class ResourceViewModel : BaseEntity<Guid>
    {
        public string Name { get; set; }
    }
}
