namespace ExchangeStuff.Service.Models.Tokens
{
    public class TokenViewModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public Guid AccountId { get; set; }
    }
}
