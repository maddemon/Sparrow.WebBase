using Microsoft.EntityFrameworkCore;
using Sparrow.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparrow.Web.Managers
{
    public abstract class ManagerBase
    {
        public ManagerBase(DataContext db)
        {
            Db = db;
        }

        public DataContext Db { get; set; }
    }

    public abstract class ManagerBase<T> : ManagerBase where T : class
    {
        public ManagerBase(DataContext db) : base(db)
        {
        }

        public virtual async Task<T> GetAsync(params object[] keyValues)
        {
            return await Db.Set<T>().FindAsync(keyValues);
        }

        public virtual async Task AddAsync(T model)
        {
            await Db.Set<T>().AddAsync(model);
            await Db.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(T model)
        {
            Db.Attach(model);
            Db.Entry(model).State = EntityState.Modified;
            await Db.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(params object[] keyValues)
        {
            var entity = await GetAsync(keyValues);
            Db.Set<T>().Remove(entity);
            await Db.SaveChangesAsync();
        }
    }

}
