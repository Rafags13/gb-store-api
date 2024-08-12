namespace GbStoreApi.Domain.Dto.Users.Dashboard
{
    public class CellValueLineChart
    {
        public int MonthIndex { get; set; }
        public required string AbbreviatedMonthName { get; set; }
        public required string FullMonthName { get; set; }
        public decimal Earns { get; set; }
    }
}
