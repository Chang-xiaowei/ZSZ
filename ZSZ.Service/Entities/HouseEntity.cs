using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.Service.Entities
{
   public  class HouseEntity:BaseEntity
    {
        public long CommunityId { get; set; }
        public virtual CommunityEntity Community { get; set; }
        public long TypeId { get; set; }
        /// <summary>
        /// 户型
        /// </summary>
        public virtual IdNameEntity Type { get; set; }
        public long  RoomTypeId { get; set; }
        /// <summary>
        /// 房屋类别
        /// </summary>
        public virtual IdNameEntity RoomType { get; set; }
        public long StatusId { get; set; }
        /// <summary>
        /// 房屋状态
        /// </summary>
        public virtual  IdNameEntity Status { get; set; }
        public long DecorateStatusId  { get; set; }
        /// <summary>
        /// 装修状态
        /// </summary>
        public virtual IdNameEntity DecorateStatus { get; set; }
        public string Address { get; set; }
        public int MonthRent { get; set; }
        public decimal Area { get; set; }
        public int TotalFloorCount { get; set; }
        public int FloorIndex { get; set; }
        /// <summary>
        /// 房屋朝向
        /// </summary>
        public string Direction { get; set; }
        /// <summary>
        /// 可看房时间
        /// </summary>
        public DateTime  LookableDateTime { get; set; }
        /// <summary>
        /// 可入住时间
        /// </summary>
        public DateTime CheckInDateTime { get; set; }
        /// <summary>
        /// 业主姓名
        /// </summary>
        public string  OwnerName { get; set; }
        public string OwnerPhoneNum { get; set; }
        public string  Description { get; set; }
        public virtual ICollection<AttachmentEntity> Attachments { get; set; } = new List<AttachmentEntity>();
        public virtual ICollection<HousePicEntity> HousePics { get; set; } = new List<HousePicEntity>();
       

    }
}
