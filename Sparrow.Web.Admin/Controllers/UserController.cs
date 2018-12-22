using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sparrow.Web.Common;
using Sparrow.Web.Managers;
using Sparrow.Web.Models;

namespace Sparrow.Web.Admin.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager UserManager;
        private readonly AuthenticationManager AuthenticationManager;
        private readonly OrganizationManager OrganizationManager;
        public UserController(UserManager userManager, AuthenticationManager authenticationManager, OrganizationManager organizationManager)
        {
            UserManager = userManager;
            AuthenticationManager = authenticationManager;
            OrganizationManager = organizationManager;
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task Login(string username, string password)
        {
            var authentication = await AuthenticationManager.Get(AuthenticationType.Password, username);
            if (authentication == null)
            {
                throw new Exception("用户名不存在");
            }
            if (authentication.AccessToken != password.MD5())
            {
                throw new Exception("密码错误");
            }
             
            var user = await UserManager.Get(authentication.UserID);
            var identity = user.ToIdentity("cookies");
            await HttpContext.SignInAsync(new ClaimsPrincipal(identity));
        }

        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        public async Task<ActionResult> List(int organizationId = 0, UserRole? role = null, string searchKey = null, int page = 0, int rows = 20)
        {
            var isAjaxRequest = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            var paramter = new UserParameter
            {
                OrganizationId = organizationId,
                SearchKey = searchKey,
                Page = isAjaxRequest ? null : new PageParameter(page, rows)
            };
            var list = await UserManager.GetList(paramter);
            if (isAjaxRequest)
            {
                return Json(list.Select(e => new { e.ID, e.Name }).ToList());
            }
            else
            {
                ViewBag.List = list.ToList();
                ViewBag.Page = paramter.Page;
                return View();
            }
        }

        public async Task<ActionResult> Edit(int id = 0)
        {
            if (id > 0)
            {
                ViewBag.Model = UserManager.Get(id);
            }
            ViewBag.Organizations = await OrganizationManager.GetList();
            return View();
        }

        [HttpPost]
        public async Task Save(User data)
        {
            if (data.ID == 0)
            {
                await UserManager.Add(data);
            }
            else
            {
                await UserManager.Update(data);
            }
        }

        public async void Delete(int id)
        {
            await UserManager.Delete(id);
        }
    }
}