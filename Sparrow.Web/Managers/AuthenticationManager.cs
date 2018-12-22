﻿using Microsoft.EntityFrameworkCore;
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

        public override async Task Add(Authentication model)
        {
            await If_NameIdentified_Existed_ThrowException(model);
            await base.Add(model);
        }

        public override async Task Update(Authentication model)
        {
            await If_NameIdentified_Existed_ThrowException(model);
            await base.Update(model);
        }
    }
}
