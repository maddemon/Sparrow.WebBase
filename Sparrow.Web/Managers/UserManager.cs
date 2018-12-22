using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sparrow.Web.Models;

namespace Sparrow.Web.Managers
{
    public class UserManager : ManagerBase
    {
        public UserManager(DataContext db) : base(db)
        {
        }

        public async Task<User> Get(int id)
        {
            return await Db.Users.FindAsync(id);
        }

        public async Task<int> Add(User model)
        {
            Db.Users.Add(model);
            return await Db.SaveChangesAsync();
        }

        public async Task<int> Update(User model)
        {
            Db.Attach(model);
            Db.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return await Db.SaveChangesAsync();
        }

        public async Task<int> Delete(int id)
        {
            var model = await Get(id);
            model.Deleted = true;
            return await Db.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetList(UserParameter parameter)
        {
            var query = Db.Users.Where(e => !e.Deleted);
            if (!string.IsNullOrWhiteSpace(parameter.SearchKey))
            {
                query = query.Where(e => e.Name.Contains(parameter.SearchKey));
            }

            return await query.OrderByDescending(e => e.ID).SetPage(parameter.Page);
        }
    }
}
