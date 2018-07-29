using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.Service.Entities;
using ZSZIService;

namespace ZSZ.Service
{
    public class CityService : ICityService
    {
        /// <summary>
        /// 新增城市
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns>新增Id</returns>
        public long AddNew(string cityName)
        {
            using (ZSZDbContext ctx=new Service.ZSZDbContext())
            {
                BaseService<CityEntity> bs = new BaseService<CityEntity>(ctx);
                bool exsits=bs.GetAll().Any(c => c.Name == cityName);
                //var exsit=bs.GetAll().Where(c => c.Name == cityName).Count()>0;
                //如果只是判断是否存在，用Any比用where().Count()的效率高
                if (exsits)
                {
                    throw new ArgumentException("城市已经存在");
                }
                CityEntity city = new Entities.CityEntity();
                city.Name = cityName;
                ctx.Cities.Add(city);
                ctx.SaveChanges();
                return city.Id;
            }           
        }

        public CityDTO[] GetAll()
        {
            using (ZSZDbContext ctx=new ZSZDbContext())
            {
                BaseService<CityEntity> bs = new BaseService<CityEntity>(ctx);
                return  bs.GetAll().AsNoTracking().ToList().Select(c=>ToDTO(c)).ToArray();
            }
        }
        public CityDTO GetById(long id)
        {
            using (ZSZDbContext ctx=new Service.ZSZDbContext())
            {
                BaseService<CityEntity> bs = new BaseService<CityEntity>(ctx);
                var city = bs.GetById(id);
                if (city == null)
                {
                    return null;
                }                
                return ToDTO(city);
            }
        }

        private  CityDTO ToDTO(CityEntity city)
        {
            CityDTO dto = new CityDTO();
            dto.Name = city.Name;
            dto.CreateDateTime = city.CreateDateTime;
            dto.Id = city.Id;
            return dto;
        }
    }
}
