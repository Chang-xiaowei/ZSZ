using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZSZ.DTO;

namespace ZSZ.Admin.Web.Models
{
    public class HouseEditViewModel
    {
        /// <summary>
        ///区域
        /// </summary>
        public RegionDTO[] Regions { get; set; }
        /// <summary>
        /// 房屋类型
        /// </summary>
        public IdNameDTO[] RoomTypes { get; set; }
        /// <summary>
        /// 房屋状态
        /// </summary>
        public IdNameDTO[] Statuses { get; set; }
        /// <summary>
        /// 装修状态
        /// </summary>
        public IdNameDTO[] DecorateStatuses { get; set; }
        /// <summary>
        /// 户型
        /// </summary>
        public IdNameDTO[] Types { get; set; }
        /// <summary>
        /// 房屋配置
        /// </summary>
        public AttachmentDTO[] Attachments { get; set; }
        public HouseDTO House { get; set; }
    }
}