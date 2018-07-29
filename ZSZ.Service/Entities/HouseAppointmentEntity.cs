using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.Service.Entities
{
   public  class HouseAppointmentEntity:BaseEntity
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long? UserId { get; set; }
        public virtual UserEntity User { get; set; }
        public string Name { get; set; }
        public string  PhoneNum { get; set; }
        public DateTime VisitDate { get; set; }
        public long HouseId { get; set; }
        public virtual  HouseEntity House { get; set; }
        public string Status { get; set; }
        public long? FollowAdminUserId { get; set; }
        public virtual  AdminUserEntity FollowAdminUser { get; set; }
        /// <summary>
        /// 这个属性是用来作并发控制的
        /// </summary>
        public byte[] RowVersion { get; set; }
        public DateTime? FollowDateTime { get; set; }
    }
}
