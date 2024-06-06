namespace ExchangeStuff.Responses
{
    public class ResponseResult<T> where T : class
    {
        public T Value { get; set; }
        public bool IsSuccess { get; set; }
        public ErrorViewModel Error { get; set; }
    }
}
