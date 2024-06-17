namespace GbStoreApi.Domain.Dto.Generic
{
    public class PaginatedResponseDto<T> : ResponseDto<T> where T : class
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public PaginatedResponseDto(T value, int statusCode, string message, int page, int pageSize, int total): base(value, statusCode, message) 
        {
            Page = page;
            PageSize = pageSize;
            Total = total;
        }
        public PaginatedResponseDto(T value, int page, int pageSize, int total): base(value) 
        {
            Page = page;
            PageSize = pageSize;
            Total = total;
        }

        public PaginatedResponseDto(int statusCode, string message, int page, int pageSize): base(statusCode, message)
        {
            Page = page;
            PageSize = pageSize;
        }
    }
}
