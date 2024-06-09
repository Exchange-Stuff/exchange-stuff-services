namespace ExchangeStuff.Service.DTOs
{
    public class JwtDTO
    {
        public string JwtSecret { get; set; }
        public int ExpireMinute { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
    }
}
