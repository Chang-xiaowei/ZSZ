using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.Admin.Web.App_Start;
using ZSZ.Admin.Web.Models;
using ZSZ.Common;
using ZSZ.CommonMVC;
using ZSZ.IService;

namespace ZSZ.Admin.Web.Controllers
{
    public class RoleController : Controller
    {
        public IRoleService roleService { get; set; }
        public IPermissionService permService { get; set; }
       [CheckPermisson("Role.List")]       
        public ActionResult List()
        {
            var role = roleService.GetAll();
            return View(role);
        }
        [CheckPermisson("Role.Delete")]
        public ActionResult Delete(long id )
        {
            roleService.MarkDeleted(id);
            return Json(new AjaxResult { Status="ok"});
        }
        [CheckPermisson("Role.Add")]
        [HttpGet]
        public ActionResult Add()
        {
            var perms = permService.GetAll();//所有可用的权限项
            return View(perms);
        }
        [CheckPermisson("Role.Add")]
        [HttpPost]
        public ActionResult Add(RoleAddModel model)
        {
            //TransactionScope
            //检查Model验证是否通过
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = MVCHelper.GetValidMsg(ModelState) });
            }
            long roleId = roleService.AddNew(model.Name);
            permService.AddPermIds(roleId,model.PermissionIds); //为roleId这个角色添加权限
            return Json(new  { status = "ok" });
        }
        [CheckPermisson("Role.Edit")]
        [HttpGet]
        public ActionResult Edit(long id)
        {
            var role = roleService.GetById(id);
            var rolePerms= permService.GetByRoleId(id);//id这个角色拥有的权限项
            var allPerms = permService.GetAll();//全部的权限
            RoleEditGetModel model = new RoleEditGetModel();
            model.Role = role;
            model.RolePerms = rolePerms;
            model.AllPerms = allPerms;
            return View(model);
        }
        [CheckPermisson("Role.Edit")]
        [HttpPost]
        public ActionResult Edit(RoleEditModel model)
        {
            roleService.Update(model.Id,model.Name);
            permService.UpdatePermIds(model.Id,model.PermissionIds);   
            return Json(new AjaxResult { Status="ok"});
        }
        [CheckPermisson("Role.Delete")]
        public ActionResult BatchDelete(long[] selectedIds)
        {
            foreach (long  id in selectedIds)
            {
                roleService.MarkDeleted(id);
            }
            return Json(new AjaxResult { Status = "ok" });
        }
    }
}