namespace GbStoreApi.Domain.Dto.Users.Dashboard
{
    public class DashboardSummaryDto
    {
        public List<CardCountDto> SystemAmountSpecificationCard { get; set; } = null!;
        public List<CellValueLineChart> SellingProgressionLastSixMonths { get; set; } = null!;
        public List<GenderPercentageCounting> PercentageOfSellingByGender { get; set; } = null!;

        public DashboardSummaryDto(
            List<CardCountDto> cardCount,
            List<CellValueLineChart> lineChartByMonths,
            List<GenderPercentageCounting> circleChartByGender
            )
        {
            SystemAmountSpecificationCard = cardCount;
            SellingProgressionLastSixMonths = lineChartByMonths;
            PercentageOfSellingByGender = circleChartByGender;
        }

        public DashboardSummaryDto()
        {
            
        }
    }
}
