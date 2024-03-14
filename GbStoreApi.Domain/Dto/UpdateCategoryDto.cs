namespace GbStoreApi.Domain.Dto
{
    public class UpdateCategoryDto
    {
        public string OldCategoryName { get; set; } = string.Empty;
        public string NewCategoryName { get; set;} = string.Empty;
    }
}
