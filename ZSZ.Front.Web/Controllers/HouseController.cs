using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.Common;
using ZSZ.CommonMVC;
using ZSZ.DTO;
using ZSZ.Front.Web.Modals;
using ZSZ.IService;
using ZSZIService;

namespace ZSZ.Front.Web.Controllers
{
    public class HouseController : Controller
    {
        public IHouseService houseService { get; set; }      
        public IAttachmentService attachmentService { get; set; } 
        public IRegionService regionService { get; set; }
        public IHouseAppointmentService appService { get; set; }
        public ActionResult Index(long id)
        {
            /*  var house = houseService.GetById(id);
              if (house==null)
              {
                  return View("Error",(object)"不存在的房源Id");
              }
              var pics= houseService.GetPics(id);
              var attachments = attachmentService.GetAttachments(id);
              string cacheKey = "HouseIndex_" + id;

                    HouseIndexViewModel model = new HouseIndexViewModel();
                    model.House = house;
                    model.Pics = pics;
                    model.Attachments = attachments;
                    */
            string cacheKey = "HouseIndex_" + id;
            //使用asp.net缓存
            HouseIndexViewModel model =(HouseIndexViewModel)HttpContext.Cache[cacheKey];//cacheKey不能重复
            if (model==null)
            {
                var house = houseService.GetById(id);
                if (house == null)
                {
                    return View("Error", (object)"不存在的房源Id");
                }
                var pics = houseService.GetPics(id);
                var attachments = attachmentService.GetAttachments(id);
                model = new HouseIndexViewModel();
                model.House = house;
                model.Pics = pics;
                model.Attachments = attachments;
                HttpContext.Cache.Insert(cacheKey, model,null,DateTime.Now.AddMinutes(1),TimeSpan.Zero);
            }        
            return View(model);
        }
        /// <summary>
        /// 分析"200-300"、"300-*"这样的价格区间
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startMonthRent">解析出来的起始租金</param>
        /// <param name="endMonthRent">解析出来的结束租金</param>
        private void ParseMonthRent(string value,out int?startMonthRent,out int?endMonthRent)
        {           
            //如果没有传递MonthRent参数，说明“不限制”房租
            if (string.IsNullOrEmpty(value))
            {
                startMonthRent = null;
                endMonthRent = null;
                return;          
            }
            string[] values = value.Split('-');
            string strStart = values[0];
            string strEnd = values[1];
            if (strStart=="*")
            {
                startMonthRent = null;//不舍下限，100以下
            }
            else
            {
                startMonthRent =Convert.ToInt32(strStart);
            }
            if (strEnd == "*")
            {
                endMonthRent = null;//不舍上限，1000以上
            }
            else
            {
                endMonthRent = Convert.ToInt32(strEnd);
            }
        }

        public ActionResult Search(long typeId,string keyWords ,string monthRent,string orderByType ,long? regionId)
        {  //获得当前城市的Id
            long cityId = FrontUtils.GetCityId(HttpContext);
            HouseSearchViewModel model = new Modals.HouseSearchViewModel();
            //获取这个cityId城市下所有的区域
            model.regions= regionService.GetAll(cityId);
            HouseSearchOptions searchOpt = new HouseSearchOptions();            
            searchOpt.CityId = cityId;
            searchOpt.CurrentIndex = 1;
          
            //根据月租金
            int?startMonthRent;
            int?endMonthRent;
            ParseMonthRent(monthRent,out startMonthRent,out endMonthRent);
            searchOpt.EndMonthRent = endMonthRent;
            searchOpt.StartMonthRent = startMonthRent;
            //搜索
            searchOpt.Keywords = keyWords;
            //根据orderByType排序规则搜索
            switch (orderByType)
            {
                case "MonthRentAsc":
                    searchOpt.OrderByType = HouseSearchOrderByType.MonthRentAsc;
                    break;
                case "MonthRentDesc":
                    searchOpt.OrderByType = HouseSearchOrderByType.MonthRentDesc;
                    break;
                case "AreaAsc":
                    searchOpt.OrderByType = HouseSearchOrderByType.AreaAsc;
                    break;
                default:
                    searchOpt.OrderByType = HouseSearchOrderByType.AreaDesc;
                    break;
            }
            searchOpt.PageSize = 10;
            searchOpt.RegionId = regionId;
            searchOpt.TypeId = typeId;
            //开始搜索
            var searchResult= houseService.Search(searchOpt);//搜索结果
            model.Houses = searchResult.result;
            return View(model);
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="keyWords"></param>
        /// <param name="monthRent"></param>
        /// <param name="orderByType"></param>
        /// <param name="regionId"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult LoadMore(long typeId, string keyWords, string monthRent, string orderByType, long? regionId,int pageIndex)
        {
            //获取城市Id
            long cityId = FrontUtils.GetCityId(HttpContext);          
           
            HouseSearchOptions searchOpt = new HouseSearchOptions();
            searchOpt.CityId = cityId;
            searchOpt.CurrentIndex = pageIndex;

            //根据月租金
            int? startMonthRent;
            int? endMonthRent;
            ParseMonthRent(monthRent, out startMonthRent, out endMonthRent);
            searchOpt.EndMonthRent = endMonthRent;
            searchOpt.StartMonthRent = startMonthRent;
            //搜索
            searchOpt.Keywords = keyWords;
            //根据orderByType排序规则搜索
            switch (orderByType)
            {
                case "MonthRentAsc":
                    searchOpt.OrderByType = HouseSearchOrderByType.MonthRentAsc;
                    break;
                case "MonthRentDesc":
                    searchOpt.OrderByType = HouseSearchOrderByType.MonthRentDesc;
                    break;
                case "AreaAsc":
                    searchOpt.OrderByType = HouseSearchOrderByType.AreaAsc;
                    break;
                default:
                    searchOpt.OrderByType = HouseSearchOrderByType.AreaDesc;
                    break;
            }
            searchOpt.PageSize = 10;
            searchOpt.RegionId = regionId;
            searchOpt.TypeId = typeId;
            //开始搜索
            var searchResult = houseService.Search(searchOpt);//搜索结果
            var houses= searchResult.result;
            return Json(new AjaxResult(){Status="ok",Data=houses });
        }
        //手机搜索
        public ActionResult Search2(long typeId, string keyWords, string monthRent, string orderByType, long? regionId)
        {
            long cityId = FrontUtils.GetCityId(HttpContext);         
          
            var regions = regionService.GetAll(cityId);
            return View(regions);
        }
        public ActionResult MakeAppointment(HouseMakeAppointMentModel model)
        {
            if (!ModelState.IsValid)
            {
                string msg = MVCHelper.GetValidMsg(ModelState);
                return Json(new AjaxResult() {
                    Status="Error",ErrorMsg=msg
               });
            }
            long? userId = FrontUtils.GetUserId(HttpContext);
            appService.AddNew(userId,model.Name,model.PhoneNum,model.HouseId,model.VisitDate);
           return Json(new AjaxResult { Status="ok"});
        }
        public ActionResult Test()
        {
            return View();
        }
    }
}