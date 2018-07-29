using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZSZ.DTO;

namespace ZSZ.Front.Web.Modals
{
    public class HouseSearchViewModel
    {
        public RegionDTO[] regions { get; set; }
        public HouseDTO[] Houses { get; set; }
    }
}