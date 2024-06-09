namespace ExchangeStuff.Responses
{
    /// <summary>
    /// If value is bool => Convert to string "true"/"false"
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseResult<T> where T : class
    {
        public T Value { get; set; }
        public bool IsSuccess { get; set; }
        public ErrorViewModel Error { get; set; }
    }
}
