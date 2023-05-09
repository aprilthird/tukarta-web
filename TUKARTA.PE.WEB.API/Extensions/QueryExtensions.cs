using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TUKARTA.PE.DATA.Extensions;
using TUKARTA.PE.WEB.API.Requests;

namespace TUKARTA.PE.WEB.API.Extensions
{
    public static class QueryExtensions
    {
        public static IQueryable<TSource> Filter<TSource, TKey>(this IQueryable<TSource> query, DataSetRequest request, Expression<Func<TSource, TKey>> filter)
            => query.Filter(request.Search, filter);
        
        public static IQueryable<TSource> FilterAndPage<TSource, TKey>(this IQueryable<TSource> query, DataSetRequest request, Expression<Func<TSource, TKey>> filter)
            => query.Filter(request, filter).Page(request);
    }
}
