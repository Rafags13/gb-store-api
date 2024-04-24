namespace GbStoreApi.Domain.Dto.Categories
{
    public class UpdateCategoryDto
    {
        public string OldCategoryName { get; set; } = string.Empty;
        public string NewCategoryName { get; set; } = string.Empty;
    }
}
