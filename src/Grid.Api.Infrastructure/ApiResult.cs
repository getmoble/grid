namespace Grid.Api.Infrastructure
{
    public class ApiResult<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
    }
}