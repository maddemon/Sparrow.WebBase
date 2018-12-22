using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sparrow.Web.Models;

namespace Sparrow.Web.Managers
{
    public class OrganizationManager : ManagerBase
    {
        public OrganizationManager(DataContext db) : base(db)
        {
        }

        public async Task<IEnumerable<Organization>> GetList()
        {
            throw new NotImplementedException();
        }
    }
}
