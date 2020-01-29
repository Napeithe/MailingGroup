namespace Model.Dto
{
    public class ApiResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public static ApiResult Failed(string message)
        {
            return new ApiResult
            {
                IsSuccess = false,
                Message = message
            };
        }
        public static ApiResult Success()
        {
            return new ApiResult
            {
                IsSuccess = true
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
                IsSuccess = true
            };
        }

        public static ApiResult<T> Failed(string message)
        {
            return new ApiResult<T>
            {
                Data = default,
                Message = message,
                IsSuccess = false
            };
        }
    }
}
