using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZIService;

namespace ZSZ.Front.Web
{
    public static  class FrontUtils
    {
        /// <summary>
        /// 获取当前用户在Session保存的userId,如果没有登录则返回null
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static long? GetUserId(HttpContextBase ctx)
        {
            return (long?)ctx.Session["UserId"];
        }
        /// <summary>
        /// 获取当前用户在Session保存的cityId,如果没有登录则返回null
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static long GetCityId(HttpContextBase ctx)
        {
            //判断是否有用户登录
            long ?userId = GetUserId(ctx);          
            if (userId==null)  //没有用户登录
            {
                //如果Session中存在CityId，则以此为当前Session的城市Id
                long? cityId =(long?)ctx.Session["CityId"];
                if (cityId!=null)
                {
                    return cityId.Value;
                }
                else//如果Session没有，则以第一个城市为CityId
                {
                    var citySvc = DependencyResolver.Current.GetService<ICityService>();
                    return citySvc.GetAll()[0].Id;
                }
            }
            else //有用户登录
            {
                var userService = DependencyResolver.Current.GetService<IUserService>();
                long ?cityId=userService.GetById(userId.Value).CityId;
                if (cityId==null) //如果没有CityId则以第一个城市为CityId
                {
                    var citySvc = DependencyResolver.Current.GetService<ICityService>();
                    return citySvc.GetAll()[0].Id;
                }
                else
                {
                    return cityId.Value;
                }            
            }          
        }
    }
}