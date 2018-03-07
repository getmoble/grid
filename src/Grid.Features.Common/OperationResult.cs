namespace Grid.Features.Common
{
    public class OperationResult<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
    }
}
