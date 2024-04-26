namespace GbStoreApi.Domain.Dto.Generic
{
    public class PaginatedResponseDto<T> : ResponseDto<T> where T : class
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public PaginatedResponseDto(T value, int statusCode, string message, int page, int pageSize): base(value, statusCode, message) 
        {
            Page = page;
            PageSize = pageSize;
        }
        public PaginatedResponseDto(T value, int statusCode, int page, int pageSize): base(value, statusCode) 
        {
            Page = page;
            PageSize = pageSize;
        }

        public PaginatedResponseDto(int statusCode, string message, int page, int pageSize): base(statusCode, message)
        {
            Page = page;
            PageSize = pageSize;
        }
    }
}
