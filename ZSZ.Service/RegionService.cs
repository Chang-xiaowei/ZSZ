using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;
using System.Data.Entity;

namespace ZSZ.Service
{
    public class RegionService : IRegionService
    {
        /// <summary>
        /// 求CityId这个所在城市下区域信息
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public RegionDTO[] GetAll(long cityId)
        {
            using (ZSZDbContext ctx=new Service.ZSZDbContext())
            {
                BaseService<RegionEntity> bs = new Service.BaseService<Entities.RegionEntity>(ctx);
                return bs.GetAll().Include(r => r.City).Where(r => r.CityId == cityId).ToList().Select(r => ToDTO(r)).ToArray();
            }
        }

        private RegionDTO ToDTO(RegionEntity entity)
        {
            RegionDTO dto = new RegionDTO();
            dto.CityName = entity.City.Name;
            dto.Id = entity.Id;
            dto.CityId = entity.Id;
            dto.CreateDateTime = entity.CreateDateTime;
            dto.Name = entity.Name;
            return dto;
        }
        /// <summary>
        ///  根据区域id,求这个区域下的所有信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public RegionDTO GetById(long id)
        {
            using (ZSZDbContext ctx=new ZSZDbContext())
            {
                BaseService<RegionEntity> bs = new BaseService<RegionEntity>(ctx);
               var region= bs.GetAll().Include(r => r.City).SingleOrDefault(r =>r.Id == id);
                return region == null ? null : ToDTO(region);
            }
        }
    }
}
