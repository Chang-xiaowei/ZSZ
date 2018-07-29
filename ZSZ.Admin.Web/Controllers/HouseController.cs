using CodeCarvings.Piczard;
using CodeCarvings.Piczard.Filters.Watermarks;
using System;
using System.Collections.Generic;
using System.IO;
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
    public class HouseController : Controller
    {
        public IHouseService houseService { get; set; }
        public IAdminUserService userService { get; set; }
        public ICityService cityService { get; set; }
        public IAttachmentService attService { get; set;}
        public IRegionService regionService { get; set; }
        public IIdNameService idNameService { get; set; }
        public ICommunityService communityService { get; set; }
        /// <summary>
        /// </summary>
        /// <param name="typeId">房源类型(整租、合租)</param>
        /// <returns></returns>
        [CheckPermisson("House.List")]
        public ActionResult List(long typeId,int pageIndex=1) 
        {
            //因为AuthorizeFilter做了是否登录的检查，因此这里不会取不到Id
            long  userId =(long)AdminHelper.GetUserId(HttpContext);
            //userId.Value 等价于(long)userId（userId为可空类型)
            long? cityId = userService.GetById(userId).CityId;
            if (cityId==null)
            {
                return View("Error",(object)"总部不能进行房源管理");
            }
            var houses = houseService.GetPagedData(cityId.Value,typeId,10,(pageIndex-1)*10);//typeId代表房屋类别，整租合租
            //cityId这个城市下typeId这种类型的房子共有多少个
            long totalCount = houseService.GetTotalCount(cityId.Value, typeId);
            ViewBag.pageIndex = pageIndex;
            ViewBag.totalCount = totalCount;
            ViewBag.TypeId = typeId;
            return View(houses);
        }
        [CheckPermisson("House.Delete")]
        public ActionResult Delete(long id )
        {
            houseService.MarkDeleted(id);
            return Json(new AjaxResult { Status="ok"});
        }
        [CheckPermisson("House.Add")]
        [ValidateInput(false)]
        [HttpGet]
        public ActionResult Add()
        {
            long?userId = AdminHelper.GetUserId(HttpContext);
            long? cityId = userService.GetById(userId.Value).CityId;
            if (cityId==null)
            {
                return View("Error",(object)"总部不能进行房源管理");
            }
            var regions = regionService.GetAll(cityId.Value);
            var roomTypes = idNameService.GetAll("户型");
            var statuses = idNameService.GetAll("房屋状态");
            var decorateStatus = idNameService.GetAll("装修状态");
            var attachments = attService.GetAll();
            var types = idNameService.GetAll("房屋类别");
            HouseAddViewModel model = new HouseAddViewModel();
            model.regions = regions;
            model.roomTypes = roomTypes;
            model.statuses = statuses;
            model.decorateStatuses = decorateStatus;
            model.attachments = attachments;
            model.types = types;
            return View(model);
        }
        public ActionResult LoadCommunity(long regionId)
        {
            var communities = communityService.GetByRegionId(regionId);
            return Json(new AjaxResult { Status = "ok",Data=communities});
        }
        [HttpPost]
        [CheckPermisson("House.Add")]
        [ValidateInput(false)]
        public ActionResult Add(HouseAddModel model)
        {
            long? userId = AdminHelper.GetUserId(HttpContext);//获得登录用户Id
            long? cityId = userService.GetById(userId.Value).CityId;//得到该用户所在的城市
            if (cityId==null)
            {
                return View("Error", (object)"总部不能进行房源管理");
            }
            HouseAddNewDTO dto = new HouseAddNewDTO();
            dto.Address = model.Address;
            dto.Area = model.Area;
            dto.AttachmentIds = model.AttachmentIds;
            dto.CheckInDateTime = model.CheckInDateTime;
            dto.CommunityId = model.CommunityId;
            dto.DecorateStatusId = model.DecorateStatusId;
            dto.Description = model.Description;
            dto.Direction = model.Direction;
            dto.FloorIndex = model.FloorIndex;
            dto.LookableDateTime = model.LookableDateTime;
            dto.MonthRent = model.MonthRent;
            dto.OwnerName = model.OwnerName;
            dto.OwnerPhoneNum = model.OwnerPhoneNum;
            dto.RoomTypeId = model.RoomTypeId;
            dto.StatusId = model.StatusId;
            dto.TotalFloorCount = model.TotalFloorCount;
            dto.TypeId = model.TypeId;
            long houseId=houseService.AddNew(dto);
             //根据新增的房源houseId，生成静态页面
            CreateStaticPage(houseId);            
            return Json(new AjaxResult { Status = "ok" ,ErrorMsg="添加房源失败"});
        }
        private void CreateStaticPage(long houseId)
        {
            var house = houseService.GetById(houseId);
            var pics = houseService.GetPics(houseId);
            var attachments = attService.GetAttachments(houseId);
            HouseIndexViewModel model = new HouseIndexViewModel();
            model.House = house;
            model.Pics = pics;
            model.Attachments = attachments;
            string html= MVCHelper.RenderViewToString(this.ControllerContext, @"~/Views/House/StaticIndex.cshtml", model);
            System.IO.File.WriteAllText("d:/1.txt",html);
        }

        [CheckPermisson("House.Edit")]
        [HttpGet]
        public ActionResult Edit(long id)
        {
            long? userId = AdminHelper.GetUserId(HttpContext);//获得登录用户Id
            long? cityId = userService.GetById(userId.Value).CityId;//得到该用户所在的城市
            if (cityId==null)
            {
                return View("Error", (object)"总部不能进行房源管理");
            }
            var house = houseService.GetById(id);
            HouseEditViewModel model = new HouseEditViewModel();
            model.House = house;
            var regions = regionService.GetAll(cityId.Value);
            var roomTypes = idNameService.GetAll("户型");           
            var statuses = idNameService.GetAll("房屋状态");
            var decorateStatus = idNameService.GetAll("装修状态");
            var attachments = attService.GetAll();
            var types = idNameService.GetAll("房屋类别");
            model.Regions = regions;
            model.RoomTypes = roomTypes;
            model.Statuses = statuses;
            model.DecorateStatuses = decorateStatus;
            model.Attachments = attachments;
            model.Types = types;            
            return View(model);
        }
        [HttpPost]
       [CheckPermisson("House.Edit")]
        public ActionResult Edit(HouseEditModel model)
        {
            HouseDTO dto = new HouseDTO();
            dto.Address = model.Address;
            dto.Area = model.Area;
            dto.AttachmentIds = model.AttachmentIds;
            dto.CheckInDateTime = model.CheckInDateTime;
            //有没有感觉强硬用一些不适合的DTO，有一些没用的属性时候的迷茫？
            dto.CommunityId = model.CommunityId;
            dto.DecorateStatusId = model.DecorateStatusId;
            dto.Description = model.Description;
            dto.Direction = model.Direction;
            dto.FloorIndex = model.FloorIndex;
            dto.Id = model.Id;
            dto.LookableDateTime = model.LookableDateTime;
            dto.MonthRent = model.MonthRent;
            dto.OwnerName = model.OwnerName;
            dto.OwnerPhoneNum = model.OwnerPhoneNum;
            dto.RoomTypeId = model.RoomTypeId;
            dto.StatusId = model.StatusId;
            dto.TotalFloorCount = model.TotalFloorCount;
            dto.TypeId = model.TypeId;
            houseService.Update(dto);
            return Json(new AjaxResult { Status="ok"});
        }
        [CheckPermisson("House.Delete")]
        public ActionResult BatchDelete(long[] selectIds)
        {
            foreach (var id in selectIds)
            {
                houseService.MarkDeleted(id);
            }
            return Json(new AjaxResult {Status="ok" });
        }
       /// <summary>
        /// 图片上传，为那个房子上传图片
        /// </summary>
        /// <returns></returns>
        public ActionResult PicUpload(int houseId)
        {
            return View(houseId);
        }
        //处理图片的上传请求
        public ActionResult UpLoadPic(int houseId,HttpPostedFileBase file)
        {
            //HttpContext.Server.MapPath()表示保存到网站的根目录
            //保存的文件名：houseId.扩展名；Path.GetExtension：获取文件的扩展名                       
            //file.SaveAs(HttpContext.Server.MapPath("~/"+houseId+Path.GetExtension(file.FileName)));
            string mds = CommonHelper.CalMD5(file.InputStream);
            string ext = Path.GetExtension(file.FileName);//得到文件的后缀名
            string path = "/upload/"+ DateTime.Now.ToString("yyyy/MM/dd") + "/" + mds + ext;           
            string fullPath = HttpContext.Server.MapPath("~" + path);
            string thumbPath = "/upload/" + DateTime.Now.ToString("yyyy/MM/dd") + "/" + mds + "_thumb" + ext;
            string thumbFullPath= HttpContext.Server.MapPath("~" + thumbPath);
            //判断文件夹upload是否存在
            new FileInfo(fullPath).Directory.Create();//尝试创建可能不存在的文件夹
            file.InputStream.Position = 0;//指针复位，如果Md5生成的文件夹名为0的情况
            //file.SaveAs(fullPath);
            //缩列图
            ImageProcessingJob jobThumb = new ImageProcessingJob();
            jobThumb.Filters.Add(new FixedResizeConstraint(200, 200));//缩略图尺寸 200*200 调用 ImageProcessingJob 的 
            jobThumb.SaveProcessedImageToFileSystem(file.InputStream,thumbFullPath);
            file.InputStream.Position = 0;
            //水印
            ImageWatermark imgWatermark = new ImageWatermark("~/images/watermark.jpg");
            imgWatermark.ContentAlignment = System.Drawing.ContentAlignment.BottomRight;//水印位置 imgWatermark.Alpha = 50;//透明度，需要水印图片是背景透明的 png 图片 
            ImageProcessingJob jobNormal = new ImageProcessingJob();
            jobNormal.Filters.Add(imgWatermark);//添加水印 
            jobNormal.Filters.Add(new FixedResizeConstraint(600, 600));//限制图片的大小，避免生成大图。如果想原图大小处理，就不用加这个 Filter
            jobNormal.SaveProcessedImageToFileSystem(file.InputStream,fullPath);
            //上传成功后把文件加入到数据库中
            houseService.AddNewHousePic(new HousePicDTO {HouseId=houseId,Url=path,ThumbUrl=thumbPath });
            return Json(new AjaxResult { Status = "ok" });        
        }
        /// <summary>
        /// 获取id这个房子下的所有图片信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult PicList(long id)
        {
            var pics = houseService.GetPics(id);
            return View(pics);
        }
        /// <summary>
        /// 批量删除图片
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <returns></returns>
        public ActionResult DeletePics(long[]selectedIds)
        {
            foreach (var id in selectedIds)
            {
                houseService.DeleteHousePic(id);
            }
            //不建议删除图片
            return Json(new AjaxResult() {Status="ok" });
        }
    }
}