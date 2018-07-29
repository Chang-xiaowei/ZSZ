using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZIService
{
   public interface ISettingService: IServiceSupport
    {

        /// <summary>
        /// 设置配置项Name的值为Value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void SetValue(string name,string value);
        /// <summary>
        /// 获取配置项name的值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        String GetValue(string name);
        void SetIntValue(string name, int value);//("秒数",5)
        int? GetIntValue(string name);
        void SetBoolValue(string name, bool value);
        bool? GetBoolValue(string name);
        SettingDTO[] GetAll();
    }
}
