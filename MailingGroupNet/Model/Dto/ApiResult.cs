namespace Model.Dto
{
    public class ApiResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }

        public static ApiResult Failed(string message, int statusCode)
        {
            return new ApiResult
            {
                IsSuccess = false,
                Message = message,
                StatusCode = statusCode
            };
        }
        public static ApiResult Success()
        {
            return new ApiResult
            {
                IsSuccess = true,
                StatusCode = 200
            };
        }

    }

    public class ApiResult<T> : ApiResult where T: new()
    {
        public T Data { get; set; }

        public static ApiResult<T> Success(T data)
        {
            return new ApiResult<T>
            {
                Data = data,
                IsSuccess = true,
                StatusCode = 200
            };
        }

        public static ApiResult<T> Failed(string message, int statusCode)
        {
            return new ApiResult<T>
            {
                Data = default,
                Message = message,
                IsSuccess = false,
                StatusCode = statusCode
            };
        }
    }
}
