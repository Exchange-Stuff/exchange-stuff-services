﻿namespace ExchangeStuff.CurrentUser.Users
{
    public interface IIdentityUser<T>
    {
        public T AccountId { get; }
    }
}
