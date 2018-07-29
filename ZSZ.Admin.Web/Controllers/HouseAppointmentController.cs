using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.Common;
using ZSZIService;

namespace ZSZ.Admin.Web.Controllers
{
    public class HouseAppointmentController : Controller
    {
        public IHouseAppointmentService appSercice { get; set; }
        public IAdminUserService userService { get; set; }
        public ActionResult List()
        {
            long? userId = AdminHelper.GetUserId(HttpContext);
            long?cityId = userService.GetById(userId.Value).CityId;
            if (cityId==null)
            {
                return View("Error",(object)"总部的人不能进行房源抢单");
            }
            var apps= appSercice.GetPagedData(cityId.Value,"未处理",10,1);
            return View(apps);
        }
        public ActionResult Follow(long appId)
        {
            long? userId = AdminHelper.GetUserId(HttpContext);
            bool isOk= appSercice.Follow(userId.Value, appId);
           if (isOk)//抢单成功
            {
                return Json(new AjaxResult() {Status="ok" });
            }
            else 
            {
                return Json(new AjaxResult() { Status = "fail" });
            }            
           
        }
    }
}