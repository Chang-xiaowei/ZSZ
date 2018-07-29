
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.Service.Entities
{
   public class IdNameEntity:BaseEntity
    {
        /// <summary>
        /// 类型名：户型、房屋状态，房屋类别、装修状态
        /// </summary>
        public string  TypeName { get; set; }
        /// <summary>
        /// 开间、一室两厅、别墅、其他；租房中，已出租，锁定中；短租，写字楼，合租，整租；装修状态：精装修、简装、毛坯
        /// </summary>
        public string Name { get; set; }
    }
}
