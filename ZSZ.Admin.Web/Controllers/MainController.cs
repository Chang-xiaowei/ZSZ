using CaptchaGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.Admin.Web.Models;
using ZSZ.Common;
using ZSZ.CommonMVC;
using ZSZ.IService;
using ZSZIService;

namespace ZSZ.Admin.Web.Controllers
{
    public class MainController : Controller
    {
        public IAdminUserService userService { get; set; } 
        public IRoleService roleService { get; set; }
        public ActionResult Index()
        {
            long? userId = AdminHelper.GetUserId(HttpContext);
            if (userId==null)
            {
                return Redirect("~/Main/Login");//没有登录请重新登录
            }
            var user= userService.GetById(userId.Value);
            ViewBag.AdminLoginName = user.Name;
            var role= roleService.GetByAdminUserId(userId.Value)[0];
            ViewBag.RoleName = role.Name;
            return View();
        }     
        public ActionResult CreateVerifyCode()
        {
            string verifyCode = CommonHelper.GenerateCaptchaCode(4);
            // Session["verifyCode"] = verifyCode;//验证码保存在Session中,不安全，容易被暴力破解，浏览器上的东西是不可信的
            TempData["verifyCode"] = verifyCode;
            //using (MemoryStream ms = ImageFactory.GenerateImage(verifyCode, 60, 100, 20, 6))
            //{
            //    return File(ms,"image/jpeg");
            //}
            MemoryStream ms = ImageFactory.GenerateImage(verifyCode,60, 100, 20, 6);
            return File(ms,"image/jpeg");
        }
        [HttpGet]
        public ActionResult Login()
        {
            ViewBag.DateTime = DateTime.Now.ToString();
            return View();
        }
        [HttpPost]           
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult {Status="error",ErrorMsg=MVCHelper.GetValidMsg(ModelState) });
            }
            //ToDo:有漏洞跟验证码有关
            string verifyCode = model.VerifyCode;
            //验证码错误
            if (verifyCode != (string)TempData["verifyCode"])
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "验证码错误" });
            }
            bool result = userService.CheckLogin(model.PhoneNum, model.Password);
            if (result)
            {
                //把当前登录用户的Id存到Session,给后面检查“当前Session登录的这个用户有没有***的权限”
                Session["LoginUserId"] = userService.GetByPhoneNum(model.PhoneNum).Id;
                return Json(new AjaxResult { Status = "ok" });
            }
            else
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "用户名或密码错误" });
            }
        }
        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            Session.Abandon();//销毁Session
            return RedirectToAction("Login");
        }    
        
    }
}
