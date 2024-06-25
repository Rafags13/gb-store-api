using GbStoreApi.Domain.Dto.Purchases;

namespace GbStoreApi.Data.Extensions
{
    public static class PurchaseExtensions
    {
        public static IQueryable<AdminPurchaseDisplay> FilterByBoughterName(this IQueryable<AdminPurchaseDisplay> query, string boughterName = "")
        {
            if (string.IsNullOrWhiteSpace(boughterName))
                return query;

            return query.Where(x => x.BoughterName.Contains(boughterName));
        }
    }
}
