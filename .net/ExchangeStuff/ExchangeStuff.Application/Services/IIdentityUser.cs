namespace ExchangeStuff.Application.Services
{
    public interface IIdentityUser<T>
    {
        public T UserId { get; }
    }
}
