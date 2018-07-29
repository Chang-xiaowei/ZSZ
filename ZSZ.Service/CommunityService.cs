using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.Service.Entities;
using ZSZIService;
using System.Data.Entity;
namespace ZSZ.Service
{
    public class CommunityService:ICommunityService
    {
        public CommunityDTO[] GetByRegionId(long RegionId)
        {
            using (ZSZDbContext ctx=new Service.ZSZDbContext())
            {
                BaseService<CommunityEntity> bs = new BaseService<CommunityEntity>(ctx);
                var community=bs.GetAll().Include(c => c.Region).AsNoTracking().Where(c => c.RegionId == RegionId);
                if (community==null)
                {
                    return null;
                }
                return community.Select(c => new CommunityDTO()
                {
                    CreateDateTime = c.CreateDateTime,
                    Location = c.Location,
                    BuildYear = c.BuildYear,
                    RegionId = c.RegionId,
                    Traffic = c.Traffic,
                    Name=c.Name,
                    Id = c.Id
                }).ToArray();
            }
        }
    }
}
