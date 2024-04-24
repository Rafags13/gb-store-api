namespace GbStoreApi.Domain.Dto.Generic
{
    public class ResponseDto<T> where T : class
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

        public ResponseDto(T value, int statusCode)
        {
            Value = value;
            StatusCode = statusCode;
        }

        public ResponseDto() 
        {
        }
    }
}
