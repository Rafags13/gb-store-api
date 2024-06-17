namespace GbStoreApi.Domain.Dto.Generic
{
    public class ResponseDto<T>
    {
        public T? Value { get; set; }
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ResponseDto(T value, int statusCode, string message)
        {
            Value = value;
            StatusCode = statusCode;
            Message = message;
        }

        public ResponseDto(T value)
        {
            Value = value;
            StatusCode = 200;
        }

        public ResponseDto(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public ResponseDto(int statusCode)
        {
            StatusCode = statusCode;
            Value = default;
        }

        public ResponseDto() 
        {
        }
    }
}
