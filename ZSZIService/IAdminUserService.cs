using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZIService
{
   public interface IAdminUserService:IServiceSupport
    {
        //加入一个用户，name 用户姓名，phoneNum 手机号，password 密码，email，cityId 城市 id（null 表示总部） 
        long AddAdminUser(string name, string phoneNum, string password, string email, long?cityId);
        void UpdateAdminUser(long id, string name, string phoneNum, string password, string email ,long? cityId);
        //获取cityId这个城市下的所有管理员
        AdminUserDTO[] GetAll(long? cityId);
        //获取所有管理员 
        AdminUserDTO[] GetAll();
        //根据 id 获取 DTO 
        AdminUserDTO GetById(long id);

        //根据手机号获取 DTO 
        AdminUserDTO GetByPhoneNum(String phoneNum);
        //检查用户名密码是否正确 
        bool CheckLogin(String phoneNum, String password);
        //软删除 
        void MarkDeleted(long adminUserId);
        //判断 adminUserId 这个用户是否有 permissionName 这个权限项（举个例子） 
        //HasPermission(3,"User.Add")
        bool HasPermission(long adminUserId, String permissionName); 

        void RecordLoginError(long id);//记录错误登录一次 
        void ResetLoginError(long id);//重置登录错误信息 



    }
}
