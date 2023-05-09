using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TUKARTA.PE.CORE.Structs;

namespace TUKARTA.PE.DATA.Extensions
{
    public static class QueryExtensions
    {
        public static IQueryable<T> Page<T>(this IQueryable<T> query, int page, int recordsPerPage)
        {
            query = query
                .Skip((page - 1) * recordsPerPage)
                .Take(recordsPerPage);
            return query;
        }

        public static IQueryable<T> Page<T>(this IQueryable<T> query, PaginationParameters paginationParameters)
        {
            if (!paginationParameters.TakeAll)
                query = query.Page(paginationParameters.Page, paginationParameters.Limit);
            return query;
        }

        #region Expression Helpers
        private static Expression MakeString(Expression source)
            => source.Type == typeof(string) ? source : Expression.Call(source, "ToString", Type.EmptyTypes);

        private static Expression BuildFilterExpression(string propertyName, string value, ParameterExpression parameter)
        {
            var left = propertyName.Split('.').Aggregate((Expression)parameter, Expression.Property);
            var body = Expression.Call(MakeString(left), "Contains", Type.EmptyTypes,
                Expression.Constant(value, typeof(string)));
            return body;
        }

        private static Expression BuildFilterExpression(string propertyName, string[] values, ParameterExpression parameter)
        {
            var body = BuildFilterExpression(propertyName, values[0], parameter);
            for (var i = 1; i < values.Count(); ++i)
            {
                var newExp = BuildFilterExpression(propertyName, values[i], parameter);
                body = Expression.OrElse(body, newExp);
            }
            return body;
        }

        private static Expression BuildFilterExpression(string[] propertyNames, string value, ParameterExpression parameter)
        {
            var body = BuildFilterExpression(propertyNames[0], value, parameter);
            for (var i = 1; i < propertyNames.Count(); ++i)
            {
                var newExp = BuildFilterExpression(propertyNames[i], value, parameter);
                body = Expression.OrElse(body, newExp);
            }
            return body;
        }

        private static Expression BuildFilterExpression(string[] propertyNames, string[] values, ParameterExpression parameter)
        {
            var body = BuildFilterExpression(propertyNames[0], values, parameter);
            for (var i = 1; i < propertyNames.Count(); ++i)
            {
                var newExp = BuildFilterExpression(propertyNames[i], values, parameter);
                body = Expression.OrElse(body, newExp);
            }
            return body;
        }

        private static Expression<Func<T, bool>> BuildFilterPredicate<T>(string propertyName, string value)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var expBody = BuildFilterExpression(propertyName, value, parameter);
            return Expression.Lambda<Func<T, bool>>(expBody, parameter);
        }

        private static Expression<Func<T, bool>> BuildFilterPredicate<T>(string propertyName, string[] values)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var expBody = BuildFilterExpression(propertyName, values, parameter);
            return Expression.Lambda<Func<T, bool>>(expBody, parameter);
        }

        private static Expression<Func<T, bool>> BuildFilterPredicate<T>(string[] propertyNames, string value)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var expBody = BuildFilterExpression(propertyNames, value, parameter);
            return Expression.Lambda<Func<T, bool>>(expBody, parameter);
        }

        private static Expression<Func<T, bool>> BuildFilterPredicate<T>(string[] propertyNames, string[] values)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var expBody = BuildFilterExpression(propertyNames, values, parameter);
            return Expression.Lambda<Func<T, bool>>(expBody, parameter);
        }
        #endregion

        public static IQueryable<TSource> Filter<TSource, TKey>(this IQueryable<TSource> query, string search, Expression<Func<TSource, TKey>> filter)
        {
            if (!string.IsNullOrEmpty(search))
            {
                var propertyNames = filter.GetPropertyAccessList().Select(x => x.Name).ToArray();
                if (propertyNames.Any())
                {
                    var searchValues = search.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    var queryFilter = BuildFilterPredicate<TSource>(propertyNames, searchValues);
                    query = query.Where(queryFilter);
                }
            }
            return query;
        }

        public static IQueryable<TSource> FilterAndPage<TSource, TKey>(this IQueryable<TSource> query, string search, Expression<Func<TSource, TKey>> filter, int page, int recordsPerPage)
            => query.Filter(search, filter)
                .Page(page, recordsPerPage);

        public static IQueryable<TSource> FilterAndPage<TSource, TKey>(this IQueryable<TSource> query, string search, Expression<Func<TSource, TKey>> filter, PaginationParameters paginationParameters)
            => query.Filter(search, filter).Page(paginationParameters);
    }
}
