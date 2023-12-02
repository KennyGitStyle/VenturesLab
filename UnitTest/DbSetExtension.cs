using Microsoft.EntityFrameworkCore;

namespace UnitTest;


public static class DbSetExtensions
{
    public static IQueryable<T> AsNoTrackingExtension<T>(this IQueryable<T> query) where T : class
    {
        return query.AsNoTracking();
    }
}

