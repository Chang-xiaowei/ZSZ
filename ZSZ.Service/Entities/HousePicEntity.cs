using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.Service.Entities
{
   public  class HousePicEntity:BaseEntity
    {
        public long HouseId { get; set; }
        public virtual  HouseEntity House { get; set; }
        /// <summary>
        /// 房源图片地址
        /// </summary>
        public  string Url { get; set; }
        /// <summary>
        /// 缩略图地址
        /// </summary>
        public string ThumbUrl { get; set; }
    }
}
