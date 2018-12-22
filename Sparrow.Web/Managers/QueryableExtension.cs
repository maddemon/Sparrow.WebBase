using Microsoft.EntityFrameworkCore;
using Sparrow.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sparrow.Web.Managers
{
    public static class QueryableExtension
    {
        public static async System.Threading.Tasks.Task<IQueryable<T>> SetPage<T>(this IQueryable<T> query, PageParameter page)
        {
            if (page == null) return query;
            if (page.RecordCount == 0)
            {
                page.RecordCount = await query.CountAsync();
            }
            return query.Skip(page.PageSize * (page.PageIndex - 1)).Take(page.PageSize);
        }
    }
}
