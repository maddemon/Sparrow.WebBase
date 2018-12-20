using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sparrow.Web.Models;

namespace Sparrow.Web.Managers
{
    public class UserManager : ManagerBase<User>
    {
        public UserManager(DataContext db) : base(db)
        {
        }

        public override Task AddAsync(User model)
        {
            return base.AddAsync(model);
        }

        public override async Task DeleteAsync(params object[] keyValues)
        {
            var model = await GetAsync(keyValues);
            model.Deleted = true;
            await Db.SaveChangesAsync();
        }
    }
}
