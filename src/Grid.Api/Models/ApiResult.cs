using System;

namespace Grid.Api.Models
{
    public class ApiResult<T>
    {
        public T Result { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }

        public static ApiResult<T> Success(T result, string message)
        {
            return new ApiResult<T>
            {
                Result = result,
                Status = true,
                Message = message
            };
        }

        public static ApiResult<T> Failure(string message)
        {
            return new ApiResult<T>
            {
                Message = message
            };
        }

        public static ApiResult<T> Failure(Exception ex)
        {
            return new ApiResult<T>
            {
                Message = ex.Message
            };
        }
    }
}