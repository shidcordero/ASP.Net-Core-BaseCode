using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Utilities
{
    public static class Extensions
    {
        /// <summary>
        /// Extension for IQueryable to order by using property name
        /// </summary>
        /// <typeparam name="T">Any Type</typeparam>
        /// <param name="query">Holds the queryable data</param>
        /// <param name="attribute">Holds the string attribute</param>
        /// <param name="direction">Holds the sort direction</param>
        /// <returns>Queryable data</returns>
        public static IQueryable<T> OrderByPropertyName<T>(this IQueryable<T> query, string attribute, string direction)
        {
            return ApplyOrdering(query, attribute, direction, Constants.Common.OrderBy);
        }

        /// <summary>
        /// Extension for IQueryable to sort data after sorting
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="query">Holds the queryable data</param>
        /// <param name="attribute">Holds the string attribute</param>
        /// <param name="direction">Holds the sort direction</param>
        /// <returns>Queryable data</returns>
        public static IQueryable<T> ThenBy<T>(this IQueryable<T> query, string attribute, string direction)
        {
            return ApplyOrdering(query, attribute, direction, Constants.Common.ThenBy);
        }

        /// <summary>
        /// Used to apply sort order on Queryable object
        /// </summary>
        /// <typeparam name="T">Any Type</typeparam>
        /// <param name="query">Holds the queryable data</param>
        /// <param name="attribute">Holds the string attribute</param>
        /// <param name="direction">Holds the sort direction</param>
        /// <param name="orderMethodName">Holds the order method name</param>
        /// <returns>Queryable data</returns>
        private static IQueryable<T> ApplyOrdering<T>(IQueryable<T> query, string attribute, string direction, string orderMethodName)
        {
            try
            {
                if (string.Equals(direction, Constants.SortDirection.Descending)) orderMethodName += Constants.SortDirection.Descending;

                var t = typeof(T);

                var param = Expression.Parameter(t);
                var property = t.GetProperty(attribute);

                if (property != null)
                    return query.Provider.CreateQuery<T>(
                        Expression.Call(
                            typeof(Queryable),
                            orderMethodName,
                            new[] { t, property.PropertyType },
                            query.Expression,
                            Expression.Quote(
                                Expression.Lambda(
                                    Expression.Property(param, property),
                                    param))
                        ));
                return query;
            }
            catch (Exception) // Probably invalid input, you can catch specifics if you want
            {
                return query; // Return unsorted query
            }
        }

        /// <summary>
        /// Extension for object checking if null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool ObjectIsNull<T>(this T obj) where T : class
        {
            return obj == null;
        }

        /// <summary>
        /// Extension for object reflective equal
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static T ReflectiveEquals<T>(this T obj, T obj2) where T : class
        {
            var returnObject = obj;
            if (obj == null && obj2 == null)
            {
                return null;
            }
            if (obj == null || obj2 == null)
            {
                return null;
            }
            var firstType = obj.GetType();
            if (obj2.GetType() != firstType)
            {
                return null; // Or throw an exception
            }
            // This will only use public properties
            foreach (var propertyInfo in firstType.GetProperties())
            {
                if (!propertyInfo.CanRead) continue;

                var firstValue = propertyInfo.GetValue(obj, null);
                var secondValue = propertyInfo.GetValue(obj2, null);
                if (!Equals(firstValue, secondValue))
                {
                    propertyInfo.SetValue(obj, secondValue);
                }
            }
            return returnObject;
        }

        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }

        public class PaginatedList<T> : List<T>
        {
            public int PageIndex { get; private set; }
            public int TotalPages { get; private set; }

            public PaginatedList(IEnumerable<T> items, int count, int pageIndex, int pageSize)
            {
                PageIndex = pageIndex;
                TotalPages = (int)Math.Ceiling(count / (double)pageSize);

                AddRange(items);
            }

            public bool HasPreviousPage => (PageIndex > 1);

            public bool HasNextPage => (PageIndex < TotalPages);

            public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
            {
                var count = await source.CountAsync();
                var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                return new PaginatedList<T>(items, count, pageIndex, pageSize);
            }
        }
    }
}