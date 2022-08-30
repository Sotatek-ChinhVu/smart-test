using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EmrCalculateApi.Extensions
{
    public static class DatabaseExtension
    {
        public static void RemoveRange<T>(this DbSet<T> data, Expression<Func<T, bool>> predicate) where T : class
        {
            data.RemoveRange(data.Where(predicate).ToList());
        }

        public static IQueryable<T> FindListQueryableNoTrack<T>(this DbSet<T> data, Expression<Func<T, bool>>? predicate = null) where T : class
        {
            if (predicate == null)
            {
                return data.AsNoTracking();
            }
            else
            {
                return data.Where(predicate).AsNoTracking();
            }
        }

        public static List<T> FindListNoTrack<T>(this DbSet<T> data, Expression<Func<T, bool>>? predicate = null) where T : class
        {
            var query = data.FindListQueryableNoTrack(predicate);

            if (query == null)
            {
                return new List<T>();
            }

            return query.ToList();
        }

        public static List<T> FindList<T>(this DbSet<T> data, Expression<Func<T, bool>>? predicate = null) where T : class
        {
            var query = data.FindListQueryable(predicate);

            if (query == null)
            {
                return new List<T>();
            }

            return query.ToList();
        }

        /// <summary>
        /// Find list entities with queryable
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IQueryable<T> FindListQueryable<T>(this DbSet<T> data, Expression<Func<T, bool>>? predicate = null) where T : class
        {
            if (predicate == null)
            {
                return data;
            }
            else
            {
                return data.Where(predicate);
            }
        }
    }
}
