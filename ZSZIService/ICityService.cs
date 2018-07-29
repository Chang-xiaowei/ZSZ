using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZIService
{
   public  interface ICityService: IServiceSupport
    {
        long AddNew(string cityName);
        //根据 id 获取城市 DTO 
        CityDTO GetById(long id);
        //获取所有城市 
        CityDTO[] GetAll();
    }
}
