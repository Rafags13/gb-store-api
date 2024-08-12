namespace GbStoreApi.Domain.Dto.Users.Dashboard
{
    public class CardCountDto
    {
        public required string Label { get; set; }
        public decimal Value { get; set; } 
        public float PercentageOfGainOrLose { get; set; }
    }
}
