using CaptchaGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.Common;
using ZSZ.CommonMVC;
using ZSZIService;

namespace ZSZ.Front.Web.Controllers
{
    public class MainController : Controller
    {
        public IUserService userService { get; set; }
        public ICityService cityService { get; set; }
        public ActionResult Index( )
        {
            long cityId= FrontUtils.GetCityId(HttpContext);
            string cityName = cityService.GetById(cityId).Name;
            ViewBag.CityName = cityName;
            var cities = cityService.GetAll();
            return View(cities);
        }
        /// <summary>
        /// 切换当前用户的城市的Id
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SwitchCityId(long? cityId)
        {
            //判断session里是否有用户不登录
            long? userId = FrontUtils.GetUserId(HttpContext);
            if (userId==null)//无人登录
            {
                Session["CityId"] = cityId;
            }
            else
            {
                //切换当前用户的城市Id
                userService.SetUserCityId((long)userId,(long)cityId);
            }
            return Json(new AjaxResult() { Status="ok"}); ;
        }      
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(string phoneNum, string password)
        {
            userService.AddNew(phoneNum,password);
            return Json(new AjaxResult() {Status="ok"});
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string phoneNum, string password)
        {
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = MVCHelper.GetValidMsg(ModelState) });
            }
            //根据手机号得到用户信息
            var user = userService.GetByPhoneNum(phoneNum);
            if (user != null)
            {
                if (userService.IsLocked(user.Id))
                {
                    //TimeSpan代表时间段，时间相减就是相差的时间段
                    TimeSpan? leftTimeSpan = TimeSpan.FromMinutes(30) - (DateTime.Now - user.LastLoginErrorDateTime);
                    return Json(new AjaxResult
                    {
                        Status = "Error",
                        ErrorMsg = "账号已被锁定,请"
                        + (int)leftTimeSpan.Value.TotalMinutes + "分钟后再试"
                    });
                }
            }
            bool isOk = userService.CheckLogin(phoneNum, password);
            //登录成功           
            if (isOk)
            {
                //在规定次数登录成功，归零,重置错误登录信息               
                userService.ResetLoginError(user.Id);
                //登录成功时候把当前用户信息存在session
                Session["UserId"] = user.Id;
                Session["CityId"] = user.CityId;
                return Json(new AjaxResult() { Status = "ok" });
            }
            //登录失败
            else
            {
                if (user != null)//存在手机号
                {
                    var userErrorTimes = userService.IncrLoginError(user.Id);
                    long errorTime = 5 - userErrorTimes.Value;
                    return Json(new AjaxResult() { Status = "error", ErrorMsg = "用户名或者密码错误" + "你好剩下" + errorTime + "次机会" });
                }
                //手机号不存在，返回用户名或者密码错误
                else
                {
                    return Json(new AjaxResult() { Status = "error", ErrorMsg = "用户名或者密码错误" });
                }

            }
        }

           /// <summary>
           /// 创建验证码
           /// </summary>
           /// <returns></returns>
        public ActionResult CreateVerifyCode()
        {
            string verifyCode = Common.CommonHelper.GenerateCaptchaCode(4);
            TempData["verifyCode"] = verifyCode;
            MemoryStream ms = ImageFactory.GenerateImage(verifyCode,60,100,30,5);          
            return File(ms, "image/jpeg");           
        }
        [HttpGet]
        public ActionResult ForgetPassword()
        {
            return View();
        }
        /// <summary>
        /// 找回密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ForgetPassword(string phoneNum ,string verifyCode,string newPassword)
        {
            if (verifyCode != (string)TempData["verifyCode"])
            {
                return Json(new AjaxResult() {Status="error",ErrorMsg="验证码错误" });
            }
            var checkIsPhoneNum = userService.GetByPhoneNum(phoneNum);
            var userId = FrontUtils.GetUserId(HttpContext);
            if (checkIsPhoneNum!=null)
            {
                userService.UpdatePwd(userId.Value, newPassword);
                return Json(new AjaxResult { Status = "ok" });
            }
            else
            {
                return Json(new AjaxResult { Status = "error",ErrorMsg="修改密码错误" });
            }
            
        }
    }
}