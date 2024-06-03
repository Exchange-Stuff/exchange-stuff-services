using ExchangeStuff.Core.Common;

namespace ExchangeStuff.Core.Entities
{
    public class Action : Auditable<Guid>
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public int Index { get; set; }
    }
}
