using Microsoft.EntityFrameworkCore;
using Sparrow.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparrow.Web.Managers
{
    public class AuthenticationManager : ManagerBase
    {
        public AuthenticationManager(DataContext db) : base(db)
        {
        }

        public async Task<Authentication> Get(int id)
        {
            return await Db.Authentications.FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<Authentication> Get(AuthenticationType type, string nameIdentified)
        {
            return await Db.Authentications.FirstOrDefaultAsync(e => e.NameIdentified == nameIdentified && e.Type == type);
        }

        private async Task If_NameIdentified_Existed_ThrowException(Authentication model)
        {
            var entity = await Db.Authentications.FirstOrDefaultAsync(e => e.NameIdentified == model.NameIdentified && e.Type == model.Type && e.UserID != model.UserID);
            if (entity != null)
            {
                throw new Exception($"{model.NameIdentified}已被占用");
            }
        }

        public async Task<int> Save(Authentication model)
        {
            await If_NameIdentified_Existed_ThrowException(model);
            Db.Authentications.Add(model);
            return await Db.SaveChangesAsync();
        }

        public async Task<int> Update(Authentication model)
        {
            await If_NameIdentified_Existed_ThrowException(model);

            var entity = await Get(model.ID);
            entity.NameIdentified = model.NameIdentified;
            entity.AccessToken = model.AccessToken;
            entity.UserID = model.UserID;

            return await Db.SaveChangesAsync();
        }

        public async Task<int> Delete(int id)
        {
            var model = await Get(id);
            Db.Authentications.Remove(model);
            return await Db.SaveChangesAsync();
        }
    }
}
