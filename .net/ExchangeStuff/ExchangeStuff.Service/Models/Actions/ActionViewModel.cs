using ExchangeStuff.Core.Common;

namespace ExchangeStuff.Service.Models.Actions
{
    public class ActionViewModel : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public int Index { get; set; }
    }
}
