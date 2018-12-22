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

        public override Task Add(User model)
        {
            return base.Add(model);
        }

        public override async Task Delete(params object[] keyValues)
        {
            var model = await Get(keyValues);
            model.Deleted = true;
            await Db.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetList(UserParameter parameter)
        {
            throw new NotImplementedException();
        }
    }
}
