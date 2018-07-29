using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.Admin.Web.App_Start;
using ZSZ.Admin.Web.Models;
using ZSZ.Common;
using ZSZ.IService;

namespace ZSZ.Admin.Web.Controllers
{
    public class PermissionController : Controller
    {
        // GET: Permission
        public IPermissionService PermSvc { get; set; }
        [CheckPermisson("Permission.List")]
        public ActionResult List()
        {
            var perms = PermSvc.GetAll();
            return View(perms);
        }
        [CheckPermisson("Permission.Delete")]
        public ActionResult Delete(long id)
        {
            PermSvc.MarkDeleted(id);
            //return RedirectToAction("List");//删除之后刷新
            return RedirectToAction(nameof(List));
        }
        [CheckPermisson("Permission.Delete")]
        public ActionResult Delete2(long id)
        {
            PermSvc.MarkDeleted(id);
            return Json(new AjaxResult {Status="ok"});
        }
        [CheckPermisson("Permission.Delete")]
        public ActionResult GetDelete(long id)
        {
            PermSvc.MarkDeleted(id);
            //return RedirectToAction("List");//删除之后刷新
            return RedirectToAction(nameof(List));
        }
        [CheckPermisson("Permission.Add")]
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        [CheckPermisson("Permission.Add")]
        [HttpPost]
        //public ActionResult Add(string name,string description)
         public ActionResult Add(PermissionEditModel model)
        {            
            PermSvc.AddPermission(model.Name,model.Description);
            return Json(new AjaxResult { Status="ok"});
        }
        [CheckPermisson("Permission.Edit")]
        [HttpGet]
        public ActionResult Edit(long id)
        {
           var perm= PermSvc.GetById(id);
           return View(perm);
        }
        [CheckPermisson("Permission.Edit")]
        [HttpPost]
        public ActionResult Edit(PermissionEditModel model)
        {
            PermSvc.UpdatePermission(model.Id,model.Name, model.Description);
            //检查name的重复性
            return Json(new AjaxResult { Status = "ok" });
        }
        public ActionResult batchDel(long[] selecteds)
        {
            foreach (var id in selecteds)
            {
                PermSvc.MarkDeleted(id);
            }
            return Json(new AjaxResult() {Status="ok",ErrorMsg="网络错误"});
        }

    }
}