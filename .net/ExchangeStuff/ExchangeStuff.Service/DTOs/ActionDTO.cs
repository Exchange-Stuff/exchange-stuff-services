using ExchangeStuff.Core.Common;

namespace ExchangeStuff.Service.DTOs
{
    public class ActionDTO:BaseEntity<Guid>
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public int Index { get; set; }
    }
}
