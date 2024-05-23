namespace GbStoreApi.Data.Extensions
{
    public static class GenericExtensions
    {
        public static IQueryable<T> OrderByDifferent<T>(this IQueryable<T> value)
        {
            return value;
        }
    }
}
