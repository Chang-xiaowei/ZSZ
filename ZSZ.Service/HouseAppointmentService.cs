using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.Service.Entities;
using ZSZIService;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace ZSZ.Service
{
    public class HouseAppointmentService : IHouseAppointmentService
    {
        public long AddNew(long? userId, string name, string phoneNum, long houseId, DateTime visitDate)
        {
            using (ZSZDbContext ctx=new Service.ZSZDbContext())
            {
                HouseAppointmentEntity houseApp = new HouseAppointmentEntity();
                houseApp.HouseId = houseId;
                houseApp.VisitDate = visitDate;
                houseApp.Name = name;
                houseApp.PhoneNum = phoneNum;
                houseApp.Status = "未处理";
                houseApp.UserId = userId;
                ctx.HouseAppointments.Add(houseApp);
                ctx.SaveChanges();
                return houseApp.Id;
            }
        }
        public HouseAppointmentDTO GetById(long id)
        {
            using (ZSZDbContext ctx = new Service.ZSZDbContext())
            {
                BaseService<HouseAppointmentEntity> bs = new Service.BaseService<Entities.HouseAppointmentEntity>(ctx);
                var houseApp = bs.GetAll().Include(a => a.House).Include(nameof(HouseAppointmentEntity.House) + "." + nameof(HouseEntity.Community)).Include(a => a.FollowAdminUser).Include
                    //Include("House.Community.Region")
                    (nameof(HouseAppointmentEntity.House) + "." + nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region)).AsNoTracking().SingleOrDefault(a => a.Id == id);
                if (houseApp == null)
                {
                    return null;
                }
                return ToDTO(houseApp);
            }
        }
        /// <summary>
        /// 抢单
        /// </summary>
        /// <param name="adminUserId"></param>
        /// <param name="houseAppointmentId"></param>
        /// <returns></returns>
        public bool Follow(long adminUserId, long houseAppointmentId)
        {
            using (ZSZDbContext ctx=new Service.ZSZDbContext())
            {
                BaseService<HouseAppointmentEntity> bs = new Service.BaseService<HouseAppointmentEntity>(ctx);
                var app = bs.GetById(houseAppointmentId);
                if (app == null)
                {
                    throw new ArgumentException("不存在的订单Id");
                }
                //FollowAdminUserId不为null,要么已经自己抢过，要么早早被别人抢了
                if (app.FollowAdminUserId!=null)
                {
                    return app.FollowAdminUserId == adminUserId;
                    /* if (app.FollowAdminUserId==adminUserId)
                     {
                         return true;
                     }
                     else
                     {
                         return false;
                     }
                     */
                }
                //如果为null，说明有抢的机会
                app.FollowAdminUserId = adminUserId;
                try
                {
                    ctx.SaveChanges();
                    return true;
                }
                //如果抛出DbUpdateConcurrencyException这个异常说明抢单失败
                catch (DbUpdateConcurrencyException)
                {
                    return false;
                }
            }
        }

        public HouseAppointmentDTO[] GetPagedData(long cityId, string status, int pageSize, int currentIndex)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<HouseAppointmentEntity> bs = new BaseService<HouseAppointmentEntity>(ctx);
                var apps=bs.GetAll().Include(a=>a.House).Include(nameof(HouseAppointmentEntity.House)+"."+nameof(HouseEntity.Community))
                    .Include(a=>a.FollowAdminUser).
                    Include(a=>a.FollowAdminUser).Include(nameof(HouseAppointmentEntity.House)+"."+nameof(HouseEntity.Community)+"."+nameof(CommunityEntity.Region))
                    .AsNoTracking().Where(a=>a.House.Community.Region.CityId==cityId&&a.Status==status)
                    .OrderByDescending(a=>a.CreateDateTime).Skip(currentIndex).Take(pageSize);//skip之前要先排序
                return apps.ToList().Select(a => ToDTO(a)).ToArray();
            }
        }

        private HouseAppointmentDTO ToDTO(HouseAppointmentEntity houseApp)
        {
            HouseAppointmentDTO dto = new HouseAppointmentDTO();
            dto.CommunityName = houseApp.House.Community.Name;
            dto.CreateDateTime = houseApp.CreateDateTime;
            dto.FollowAdminUserId = houseApp.FollowAdminUserId;
            if (houseApp.FollowAdminUser != null)
            {
                dto.FollowAdminUserName = houseApp.FollowAdminUser.Name;
            }
            dto.FollowDateTime = houseApp.FollowDateTime;
            dto.HouseId = houseApp.HouseId;
            dto.Id = houseApp.Id;
            dto.Name = houseApp.Name;
            dto.PhoneNum = houseApp.PhoneNum;
            dto.RegionName = houseApp.House.Community.Region.Name;
            dto.Status = houseApp.Status;
            dto.UserId = houseApp.UserId;
            dto.VisitDate = houseApp.VisitDate;
            dto.HouseAddress = houseApp.House.Address;
            return dto;
        }

        public long GetTotalCount(long cityId, string status)
        {
            using (ZSZDbContext ctx=new ZSZDbContext())
            {
                BaseService<HouseAppointmentEntity> houseapp = new BaseService<HouseAppointmentEntity>(ctx);
                //houseapp.GetAll().Where(a=>a.House.Community.Region.CityId==cityId&&a.Status==status).LongCount()
                var count = houseapp.GetAll().LongCount(a => a.House.Community.Region.CityId == cityId && a.Status == status);
                return count;
            }
        }
    }
}
