using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.Service.Entities;
using ZSZIService;
using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace ZSZ.Service
{
    public class HouseService : IHouseService
    {
        /// <summary>
        /// 新增房源
        /// </summary>
        /// <param name="house"></param>
        /// <returns></returns>
        public long AddNew(HouseAddNewDTO house)
        {
            HouseEntity houseEntity = new HouseEntity();
            houseEntity.Address = house.Address;
            houseEntity.Area = house.Area;
            using (ZSZDbContext ctx = new Service.ZSZDbContext())
            {
                BaseService<AttachmentEntity> attBs = new Service.BaseService<Entities.AttachmentEntity>(ctx);
                //获取house.AttachmentIds为主键的房屋配套设施
                var atts = attBs.GetAll().Where(a => house.AttachmentIds.Contains(a.Id));
                foreach (var att in atts)
                {
                    houseEntity.Attachments.Add(att);
                }
                houseEntity.CheckInDateTime = house.CheckInDateTime;
                houseEntity.CommunityId = house.CommunityId;
                houseEntity.DecorateStatusId = house.DecorateStatusId;
                houseEntity.Description = house.Description;
                houseEntity.Direction = house.Direction;
                houseEntity.FloorIndex = house.FloorIndex;
                //houseEntity.HousePics 新增后再单独添加
                houseEntity.LookableDateTime = house.LookableDateTime;
                houseEntity.MonthRent = house.MonthRent;
                houseEntity.OwnerName = house.OwnerName;
                houseEntity.OwnerPhoneNum = house.OwnerPhoneNum;
                houseEntity.RoomTypeId = house.RoomTypeId;
                houseEntity.StatusId = house.StatusId;
                houseEntity.TotalFloorCount = house.TotalFloorCount;
                houseEntity.TypeId = house.TypeId;
                ctx.Houses.Add(houseEntity);
                ctx.SaveChanges();
                return houseEntity.Id;
            }
        }
        /// <summary>
        /// 新增房源图片
        /// </summary>
        /// <param name="housePic"></param>
        /// <returns></returns>
        public long AddNewHousePic(HousePicDTO housePic)
        {
            HousePicEntity entity = new HousePicEntity();
            entity.HouseId = housePic.HouseId;
            entity.Url = housePic.Url;
            entity.ThumbUrl = housePic.ThumbUrl;
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                ctx.HousePics.Add(entity);
                ctx.SaveChanges();
                return entity.Id;
            }
        }
        /// <summary>
        /// 删除房源图片
        /// </summary>
        /// <param name="housePicId"></param>
        public void DeleteHousePic(long housePicId)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                //复习EF状态转换
                /*
                HousePicEntity entity = new HousePicEntity();
                entity.Id = housePicId;
                ctx.Entry(entity).State = EntityState.Deleted;
                ctx.SaveChanges();                */
                /* BaseService<HousePicEntity> bs = new BaseService<HousePicEntity>(ctx);
                 bs.MarkDeleted(housePicId);
                 */
                var entity = ctx.HousePics.SingleOrDefault(p => p.IsDeleted == false && p.Id == housePicId);
                if (entity != null)
                {
                    ctx.HousePics.Remove(entity);
                    ctx.SaveChanges();
                }
            }
        }

        /// <summary>
        /// 根据Id 得到房源信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HouseDTO GetById(long id)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<HouseEntity> bs = new BaseService<HouseEntity>(ctx);
                var houses = bs.GetAll()
                    .Include(h => h.Attachments)
                    //Include("Community.Region.City");
                    .Include(h => h.Community)                    
                    .Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region) + "." + nameof(RegionEntity.City))
                    .Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region))
                    .Include(h => h.DecorateStatus)
                    .Include(h => h.HousePics)
                    .Include(h => h.RoomType)
                    .Include(h => h.Status)
                    .Include(h => h.Type)
                    .SingleOrDefault(h => h.Id == id);
                if (houses == null)
                {
                    return null;
                }
                return ToDTO(houses);
            }
        }
        /// <summary>
        /// ToDTO方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private HouseDTO ToDTO(HouseEntity entity)
        {
            HouseDTO dto = new HouseDTO();
            dto.Address = entity.Address;
            dto.Area = entity.Area;
            dto.AttachmentIds = entity.Attachments.Select(a => a.Id).ToArray();
            dto.CheckInDateTime = entity.CheckInDateTime;
            dto.CityId = entity.Community.Region.CityId;
            dto.CityName = entity.Community.Region.City.Name;
            dto.CommunityBuiltYear = entity.Community.BuildYear;
            dto.CommunityId = entity.CommunityId;
            dto.CommunityLocation = entity.Community.Location;
            dto.CommunityName = entity.Community.Name;
            dto.CreateDateTime = entity.CreateDateTime;
            dto.DecorateStatusId = entity.DecorateStatusId;
            dto.DecorateStatusName = entity.DecorateStatus.Name;
            dto.Description = entity.Description;
            dto.Direction = entity.Direction;
            var firstPic = entity.HousePics.FirstOrDefault();
            if (firstPic != null)
            {
                dto.FirstThumbUrl = firstPic.ThumbUrl;
            }
            dto.FloorIndex = entity.FloorIndex;
            dto.Id = entity.Id;
            dto.LookableDateTime = entity.LookableDateTime;
            dto.MonthRent = entity.MonthRent;
            dto.OwnerName = entity.OwnerName;
            dto.OwnerPhoneNum = entity.OwnerPhoneNum;
            dto.RegionId = entity.Community.RegionId;
            dto.RegionName = entity.Community.Region.Name;
            dto.RoomTypeId = entity.RoomTypeId;
            dto.RoomTypeName = entity.RoomType.Name;
            dto.StatusId = entity.StatusId;
            dto.StatusName = entity.Status.Name;
            dto.TotalFloorCount = entity.TotalFloorCount;
            dto.TypeId = entity.TypeId;
            dto.TypeName = entity.Type.Name;
            dto.CommunityTraffic = entity.Community.Traffic;
            return dto;

        }

        /// <summary>
        /// 得到CityId这个城市下，从是startDateTime~endDateTime这个事件段新增的房源数量
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <returns></returns>
        public long GetCount(long cityId, DateTime startDateTime, DateTime endDateTime)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<HouseEntity> bs = new BaseService<HouseEntity>(ctx);
                return bs.GetAll().LongCount(h => h.Community.Region.CityId == cityId && h.CreateDateTime >= startDateTime && h.CreateDateTime <= endDateTime);
            }
        }
        /// <summary>
        /// 得到cityId这个城市下，类别为typeId的房屋的分页数据
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="typeId"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        public HouseDTO[] GetPagedData(long cityId, long typeId, int pageSize, int currentIndex)
        {
            using (ZSZDbContext ctx=new ZSZDbContext())
            {
                BaseService<HouseEntity> houseBs = new BaseService<HouseEntity>(ctx);
                var houses = houseBs.GetAll().Include(h => h.Attachments)
                    .Include(h => h.Community)
                    //Include(Community.Region.City)
                    .Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region) + "." + nameof(RegionEntity.City))
                    .Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region))
                    .Include(h => h.DecorateStatus)
                    .Include(h => h.HousePics)
                    .Include(h => h.RoomType)
                    .Include(h => h.Status)
                    .Include(h => h.Type)
                    //注意where的位置，要房到Skip前
                    .Where(h => h.Community.Region.CityId == cityId && h.TypeId == typeId)
                    .OrderByDescending(h => h.CreateDateTime)//分页前一定先排序
                    .Skip(currentIndex).Take(pageSize);
                return houses.ToList().Select(h => ToDTO(h)).ToArray();
            }
            
        }

        public HousePicDTO[] GetPics(long houseId)
        {
            using (ZSZDbContext ctx=new ZSZDbContext())
            {
                BaseService<HousePicEntity> bs = new Service.BaseService<Entities.HousePicEntity>(ctx);
                var housePics = bs.GetAll().Where(h => h.HouseId == houseId);
                List<HousePicDTO> list = new List<HousePicDTO>();
                foreach (var housePic in housePics)
                {
                    HousePicDTO dto = new HousePicDTO();
                    dto.Url = housePic.Url;
                    dto.ThumbUrl = housePic.Url;
                    dto.Id = housePic.Id;
                    list.Add(dto);                
                }
                return list.ToArray();
                
            }
        }
        /// <summary>
        /// 得到cityId这个城市下，类别为typeId的房屋数量
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public long GetTotalCount(long cityId, long typeId)
        {
            using (ZSZDbContext ctx=new ZSZDbContext())
            {
                BaseService<HouseEntity> houseBs = new BaseService<HouseEntity>(ctx);
                return houseBs.GetAll().LongCount(h=>h.Community.Region.CityId==cityId&&h.TypeId==typeId);                
            }
        }

        public void MarkDeleted(long id)
        {
            using (ZSZDbContext ctx=new ZSZDbContext())
            {
                BaseService<HouseEntity> bs = new BaseService<HouseEntity>(ctx);
                bs.MarkDeleted(id);
            }
        }

        public HouseSearchResult Search(HouseSearchOptions options)
        {
            using (ZSZDbContext ctx=new ZSZDbContext())
            {
                BaseService<HouseEntity> bs = new BaseService<Entities.HouseEntity>(ctx);
                var items = bs.GetAll().Where(h=>h.Community.Region.CityId==options.CityId&&h.TypeId==options.TypeId);
                //指定过滤条件区域
                if (options.RegionId!=null)
                {
                    items = items.Where(t => t.Community.RegionId == options.RegionId);
                }
                if (options.StartMonthRent != null)
                {
                    items = items.Where(t => t.MonthRent >= options.StartMonthRent);
                }
                if (options.EndMonthRent != null)
                {
                    items = items.Where(t =>t.MonthRent <= options.EndMonthRent);
                }
                if (!string.IsNullOrEmpty(options.Keywords))
                {
                    items = items.Where(t => t.Address.Contains(options.Keywords)
                                     ||t.Description.Contains(options.Keywords)
                                     ||t.Community.Name.Contains(options.Keywords)
                                     || t.Community.Location.Contains(options.Keywords)
                                     || t.Community.Traffic.Contains(options.Keywords));
                }
                //获取总条数
                long totalCount= items.LongCount();
                items = items.Include(h => h.Community)                  
                    .Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region) + "." + nameof(RegionEntity.City))
                    .Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region))
                    .Include(h => h.DecorateStatus)
                    .Include(h => h.HousePics)
                    .Include(h => h.RoomType)
                    .Include(h => h.Status)
                    .Include(h => h.Type)
                    .Include(h=>h.Attachments);
                //排序
                switch (options.OrderByType)
                {
                    case HouseSearchOrderByType.MonthRentDesc:
                        items = items.OrderByDescending(t => t.MonthRent);
                        break;
                    case HouseSearchOrderByType.MonthRentAsc:
                        items = items.OrderBy(t => t.MonthRent);
                        break;
                    case HouseSearchOrderByType.AreaDesc:
                        items = items.OrderByDescending(t => t.Area);
                        break;
                    case HouseSearchOrderByType.AreaAsc:
                        items = items.OrderBy(t => t.Area);
                        break;
                    case HouseSearchOrderByType.CreateDateDesc:
                        items = items.OrderByDescending(t => t.CreateDateTime);
                        break;
                    default:
                        items = items.OrderBy(t => t.CreateDateTime);
                        break;
                }
                //分页,一定要先排序在分页，给用户看的页码从1开始，程序中从0开始
                items = items.Skip((options.CurrentIndex - 1)* options.PageSize).Take(options.PageSize);
                HouseSearchResult searchResult = new HouseSearchResult();
                searchResult.totalCount = totalCount;
                List<HouseDTO> dto = new List<HouseDTO>();
                foreach (var item in items)
                {
                    //把数据转存到DTO中
                    dto.Add(ToDTO(item));
                }
                searchResult.result = dto.ToArray();
                return searchResult;
                
            }
        }

        public void Update(HouseDTO house)
        {
            using (ZSZDbContext ctx=new ZSZDbContext())
            {
                BaseService<HouseEntity> bS = new BaseService<HouseEntity>(ctx);
                HouseEntity entity = bS.GetById(house.Id);//去掉软删除
                entity.Address = house.Address;
                entity.Area = house.Area;
                entity.Attachments.Clear();//先删再加
                var atts = ctx.Attachments.Where(a => a.IsDeleted == false && house.AttachmentIds.Contains(a.Id));
                foreach (var att in atts)
                {
                    entity.Attachments.Add(att);
                }
                entity.CheckInDateTime = house.CheckInDateTime;
                entity.CommunityId = house.CommunityId;
                entity.DecorateStatusId = house.DecorateStatusId;
                entity.Description = house.Description;
                entity.Direction = house.Direction;
                entity.FloorIndex = house.FloorIndex;
                entity.LookableDateTime = house.LookableDateTime;
                entity.MonthRent = house.MonthRent;
                entity.OwnerName = house.OwnerName;
                entity.RoomTypeId = house.RoomTypeId;
                entity.TotalFloorCount = house.TotalFloorCount;
                entity.TypeId = house.TypeId;
                ctx.SaveChanges();
            }
        }
        /// <summary>
        /// 查看今天新增的房源数量
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public int GetTodayNewHouseCount(long cityId)
        {
            using (ZSZDbContext ctx=new Service.ZSZDbContext())
            {
                //房子创建的时间是在当前时间内的24个小时，就认为是“今天的房源”
                //日期相减结果是TimeSpan类型
                /*
                return bs.GetAll().Count(h => h.Community.Region.CityId==cityId
                            && (DateTime.Now - h.CreateDateTime).TotalHours <= 24);*/
                /*
    DateTime date24HourAgo = DateTime.Now.AddHours(-24);//算出来一个24小时之前的时间
    return bs.GetAll().Count(h => h.Community.Region.CityId == cityId
               && h.CreateDateTime<=date24HourAgo);*/
                BaseService<HouseEntity> bs = new BaseService<HouseEntity>(ctx);
                return bs.GetAll().Count(h=>h.Community.Region.CityId==cityId&&SqlFunctions.DateDiff("hh",h.CreateDateTime,DateTime.Now)<=24);
            }
        }
    }
}
