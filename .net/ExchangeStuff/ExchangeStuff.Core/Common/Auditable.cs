namespace ExchangeStuff.Core.Common
{
    public class Auditable<T> : BaseEntity<T>, IAuditable<T>
    {
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public T CreatedBy { get; set; }
        public T ModifiedBy { get; set; }
    }
}
