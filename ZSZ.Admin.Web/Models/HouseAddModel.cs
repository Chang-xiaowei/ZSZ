using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZSZ.Admin.Web.Models
{
    public class HouseAddModel
    {
        [Required]
        public long CommunityId { get; set; }
        [Required]
        public long  RoomTypeId { get; set; }
        [Required]
        public string Address { get; set; }
        /// <summary>
        /// 月租金
        /// </summary>
        [Required]
        public int MonthRent { get; set; }
        /// <summary>
        /// 房屋状态
        /// </summary>
        [Required]
        public long StatusId { get; set; }
        /// <summary>
        /// 房屋朝向
        /// </summary>
        [Required]
        public string Direction { get; set; }
        /// <summary>
        /// 房屋面积
        /// </summary>
        [Required]
        public decimal Area { get; set; }
        /// <summary>
        /// 装修状态
        /// </summary>
        [Required]
        public long DecorateStatusId { get; set; }
        [Required]
        public int FloorIndex { get; set; }
        [Required]
        public int TotalFloorCount { get; set; }
        /// <summary>
        /// 可看房时间
        /// </summary>
        [Required]
        public DateTime LookableDateTime { get; set; }
        /// <summary>
        /// 可入住时间
        /// </summary>
        [Required]
        public DateTime CheckInDateTime { get; set; }

        /// <summary>
        /// 房源描述
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// 房屋配置
        /// </summary>
        [Required]
        public long[] AttachmentIds { get; set; }

        /// <summary>
        /// 户型
        /// </summary>
        [Required]
        public long TypeId { get; set; }
        /// <summary>
        /// 业主姓名
        /// </summary>
        public string OwnerName { get; set; }
        /// <summary>
        /// 业主手机号
        /// </summary>
        public string OwnerPhoneNum { get; set; }



    }
}