namespace software_API.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public int? Code { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string message = "??? ??????? ?????", int code = 200)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                Code = code
            };
        }

        public static ApiResponse<T> ErrorResponse(string message = "??? ???", int code = 400)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default,
                Code = code
            };
        }

        public static ApiResponse<T> NotFoundResponse(string message = "???????? ??? ??????")
        {
            return ErrorResponse(message, 404);
        }

        public static ApiResponse<T> UnauthorizedResponse(string message = "??? ???? ?? ???????")
        {
            return ErrorResponse(message, 401);
        }
    }

    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? Code { get; set; }

        public static ApiResponse SuccessResponse(string message = "??? ??????? ?????", int code = 200)
        {
            return new ApiResponse
            {
                Success = true,
                Message = message,
                Code = code
            };
        }

        public static ApiResponse ErrorResponse(string message = "??? ???", int code = 400)
        {
            return new ApiResponse
            {
                Success = false,
                Message = message,
                Code = code
            };
        }
    }
}
