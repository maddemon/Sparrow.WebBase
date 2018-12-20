using Sparrow.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparrow.Web.Managers
{
    public class AuthenticationManager : ManagerBase<Authentication>
    {
        public AuthenticationManager(DataContext db) : base(db)
        {
        }

        private void If_NameIdentified_Existed_ThrowException(Authentication model)
        {
            var entity = Db.Authentications.FirstOrDefault(e => e.NameIdentified == model.NameIdentified && e.Type == model.Type && e.UserID != model.UserID);
            if (entity != null)
            {
                throw new Exception($"{model.NameIdentified}已被占用");
            }
        }

        public override Task AddAsync(Authentication model)
        {
            If_NameIdentified_Existed_ThrowException(model);
            return base.AddAsync(model);
        }

        public override Task UpdateAsync(Authentication model)
        {
            If_NameIdentified_Existed_ThrowException(model);
            return base.UpdateAsync(model);
        }
    }
}
