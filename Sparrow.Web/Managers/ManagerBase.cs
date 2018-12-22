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
}
