using GbStoreApi.Domain.Constants;
using GbStoreApi.Domain.Dto.Products;
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

        public static IQueryable<AdminPurchaseDisplay> OrderByBoughterNameWithInformedDirection(this IQueryable<AdminPurchaseDisplay> query, string? nameOrderDirection)
        {
            if (string.IsNullOrWhiteSpace(nameOrderDirection)) return query;

            if (nameOrderDirection == OrdenationConstants.ASCENDING) return query.OrderBy(x => x.BoughterName);

            return query.OrderByDescending(x => x.BoughterName);
        }

        public static IQueryable<AdminPurchaseDisplay> OrderByPriceWithInformedDirection(this IQueryable<AdminPurchaseDisplay> query, string? priceOrderDirection)
        {
            if (string.IsNullOrWhiteSpace(priceOrderDirection)) return query;

            if (priceOrderDirection == OrdenationConstants.ASCENDING) return query.OrderBy(x => x.Price);

            return query.OrderByDescending(x => x.Price);
        }

        public static IQueryable<AdminPurchaseDisplay> OrderByPaymentTypeWithInformedDirection(
            this IQueryable<AdminPurchaseDisplay> query,
            string? paymentTypeOrderDirection)
        {
            if (string.IsNullOrWhiteSpace(paymentTypeOrderDirection )) return query;

            if (paymentTypeOrderDirection  == OrdenationConstants.ASCENDING) return query.OrderBy(x => x.TypeOfPayment);

            return query.OrderByDescending(x => x.TypeOfPayment);
        }

        public static IQueryable<AdminPurchaseDisplay> OrderByPurchaseStateWithInformedDirection(
            this IQueryable<AdminPurchaseDisplay> query,
            string? purchaseStateOrderDirection)
        {
            if (string.IsNullOrWhiteSpace(purchaseStateOrderDirection)) return query;

            if (purchaseStateOrderDirection == OrdenationConstants.ASCENDING) return query.OrderBy(x => x.PurchaseState);

            return query.OrderByDescending(x => x.PurchaseState);
        }

        public static IQueryable<AdminPurchaseDisplay> OrderByDeliveryDateWithInformedDirection(
            this IQueryable<AdminPurchaseDisplay> query,
            string? deliveryDateOrderDirection)
        {
            if (string.IsNullOrWhiteSpace(deliveryDateOrderDirection)) return query;

            if (deliveryDateOrderDirection == OrdenationConstants.ASCENDING) return query.OrderBy(x => x.OrderDate);

            return query.OrderByDescending(x => x.OrderDate);
        }
    }
}
