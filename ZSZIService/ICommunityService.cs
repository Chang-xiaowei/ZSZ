using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZIService
{
   public interface ICommunityService:IServiceSupport
    {
        CommunityDTO[] GetByRegionId(long RegionId);
    }
}
