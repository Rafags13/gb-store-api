namespace GbStoreApi.Data.Extensions
{
    public static class GenericExtensions
    {
        public static IQueryable<T> OrderByDifferent<T>(this IQueryable<T> value)
        {
            return value;
        }

        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int page = 0, int pageSize = 20)
        {
            return query.Skip(page * pageSize).Take(pageSize);
        }

        public static IQueryable<T> GetCount<T>(this IQueryable<T> query, out int count)
        {
            count = query.Count();

            return query;
        }
    }
}
