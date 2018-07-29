using CaptchaGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.Common;
using ZSZ.Front.Web.Modals;
using ZSZIService;
namespace ZSZ.Front.Web.Controllers
{
    public class UserController : Controller
    {
        public IUserService userService { get; set; }
        /// <summary>
        /// 创建验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateVerifyCode()
        {
            string verifyCode = Common.CommonHelper.GenerateCaptchaCode(4);
            TempData["verifyCode"] = verifyCode;
            MemoryStream ms = ImageFactory.GenerateImage(verifyCode, 50, 100,20, 6);
            return File(ms, "/Image/jpeg");
        }
        [HttpGet]
        public ActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgetPassword(UserFogetPwdModel model)
        {
            if (model.VerifyCode!= (string)TempData["verifyCode"])
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = "验证码错误" });
            }
            var user = userService.GetByPhoneNum(model.PhoneNum);
            if (user!=null)//手机号存在
            {
                userService.UpdatePwd(user.Id, model.NewPassword);
                return Json(new AjaxResult() {Status="ok" });
            }
            else
            {
                return Json(new AjaxResult() { Status = "error" ,ErrorMsg="手机不存在"});
            }
        }
    }
}