namespace ExchangeStuff.Core.Common
{
    public interface IAuditable< T> : IBaseEntity<T>
    {
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public T CreatedBy { get; set; }
        public T ModifiedBy { get; set; }
    }
}
