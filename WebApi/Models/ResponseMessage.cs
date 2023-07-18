namespace WebApi.Models
{
    public class ResponseMessage<T>
    {
        public string Message { get; set; }
        public IEnumerable<T> ListData { get; set; }
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
    }
}
