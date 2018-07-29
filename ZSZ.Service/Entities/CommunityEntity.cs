using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.Service.Entities
{
  public  class CommunityEntity:BaseEntity
    {
        public string Name { get; set; }
        public long  RegionId { get; set; }
        public virtual RegionEntity Region { get; set; }
        /// <summary>
        /// 小区位置
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 交通情况
        /// </summary>
        public string Traffic { get; set; }
        /// <summary>
        /// 建造年代
        /// </summary>
        public int? BuildYear { get; set; }
    }
}
