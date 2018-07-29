using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.Service.Entities
{
   public class AttachmentEntity:BaseEntity
    {
        public string Name { get; set; }
        /// <summary>
        /// 房屋图标名称：床、沙发，冰箱等
        /// </summary>
        public string IconName { get; set; }
        public virtual ICollection<HouseEntity> Houses { get; set; } = new List<HouseEntity>();
    }
}
