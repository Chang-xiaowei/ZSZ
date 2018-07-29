using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.Admin.Web.App_Start;
using ZSZ.Admin.Web.Models;
using ZSZ.Common;
using ZSZ.CommonMVC;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZIService;

namespace ZSZ.Admin.Web.Controllers
{
    public class AdminUserController : Controller
    {
        // GET: User
        public IAdminUserService adminUserService { get; set; }
        public ICityService cityService { get; set; }
        public IRoleService roleService { get; set; }
        //标签：访问List这个Action方法的时候，当前用户必须具有“AdminUser.List AdminUser.List”权限
        [CheckPermissonAttribute("AdminUser.List")]        
        public ActionResult List()
        {
            var adminUsers = adminUserService.GetAll() ;
            return View(adminUsers);
        }
        [CheckPermisson("AdminUser.Delete")]
        public ActionResult Delete(long id )
        {
            adminUserService.MarkDeleted(id);
            //RedirectToAction(nameof(List));
            return Json(new AjaxResult { Status = "ok" });
        }
        [CheckPermisson("AdminUser.Add")]
        [HttpGet]
        public ActionResult Add()
        {
            var roles = roleService.GetAll();//拿到所有的角色
            var citys = cityService.GetAll().ToList();//拿到所有的城市
            //在最前面加一个总部
            citys.Insert(0,new CityDTO {Id=0,Name= "总部"});
            AdminUserAddViewModel model = new AdminUserAddViewModel();
            model.Cities = citys.ToArray();
            model.Roles = roles;
            return View(model);
        }
        /// <summary>
        /// 检查手机号是否存在
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [CheckPermisson("AdminUser.SearchPhoneNum")]
        public ActionResult CheckPhoneNum(string phoneNum,long?userId)
        {
            var user = adminUserService.GetByPhoneNum(phoneNum);
            bool isOk = false;
            //如果没有userId,则说明是“插入”，只要检查是不是存在这个手机号
            if(userId==null)
            {
                isOk =(user==null);
            }
            else//如果有userId，则说明是修改，则要把自己排除在外
            {
                isOk = (user == null || user.Id == userId);
            }
            return Json(new AjaxResult {Status=isOk?"ok":"exsits" });
        }
        [CheckPermisson("AdminUser.Add")]
        [HttpPost]
        public ActionResult Add(AdminUserAddModel model)
        {
            if (!ModelState.IsValid)
            {
                string msg = MVCHelper.GetValidMsg(ModelState);
                return Json(new AjaxResult { Status = "error", ErrorMsg = msg });
            }
            //服务器端的校验必不可少
            bool exsit = adminUserService.GetByPhoneNum(model.PhoneNum)!=null;
            if (exsit)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "手机号已存在" });
            }
            long? cityId = null;
            if (model.CityId!=0)//cityId=0的时候为“总部”
            {
                cityId = model.CityId;
            }            
            long userId= adminUserService.AddAdminUser(model.Name,model.PhoneNum,model.Password,model.Email, cityId);
            //为userId这个管理员添加权限
            roleService.AddRoleIds(userId, model.RoleIds);
            return Json(new AjaxResult { Status = "ok" });
        }
        [CheckPermisson("AdminUser.Edit")]
        [HttpGet]
        public ActionResult Edit(long id)
        {
            var adminUser = adminUserService.GetById(id);
            if (adminUser==null)
            {
                //不要忘了第二个参数的(object) 
                //如果视图在当前文件夹下没有找到，则去Shared下去找
                //一个Error视图，大家共用
                //return Redirect();
                return View("Error",(object)"id指定的操作员不存在");               
            }
            long? cityId = adminUser.CityId;
            var cities = cityService.GetAll().ToList();
            cities.Insert(0,new DTO.CityDTO { Id = 0, Name = "总部" });
            var roles = roleService.GetByAdminUserId(id);//获得用户Id获取用户所有的角色信息
            AdminUserEditViewModel model = new AdminUserEditViewModel();
            model.UserRoleIds = roles.Select(r => r.Id).ToArray();//获取用户角色Id
            model.Cities = cities.ToArray();
            model.AdminRoles = roles;
            model.AllRoles = roleService.GetAll();
            //if (cityId == null)
            //{
            //    cityId = 0;
            //}
            model.CityId = cityId;
            model.AdminUser = adminUser;
            return View(model);

        }
        /// <summary>
        /// 编辑的时候手机号不能修改
        /// 两个密码都留空。如果不输入密码，表示不修改密码；
        /// 如果输入了密码，则把用户的密码设定为新输入的密码。
        /// </summary>
        /// <returns></returns>
        [CheckPermisson("AdminUser.Edit")]
        [HttpPost]
        public ActionResult Edit(AdminUserEditModel model)
        {
            long? cityId = null;
            if (model.cityId>0)
            {
                cityId = model.cityId;
            }
            adminUserService.UpdateAdminUser(model.Id,model.Name,model.PhoneNum,model.Password,model.Email,cityId);
            roleService.UpdateRoleIds(model.Id,model.RoleIds);
            return Json(new AjaxResult { Status = "ok" });   
        }    
        //批量删除 
        [CheckPermisson("AdminUser.Delete")]
        public ActionResult BatchDel(long[] selectedIds)
        {
            foreach (long  id in selectedIds)
            {
                adminUserService.MarkDeleted(id);
            }
            return Json(new AjaxResult { Status = "ok" });
        }
    }
}