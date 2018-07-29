using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZIService
{
   public interface IHouseAppointmentService:IServiceSupport
    {
        //新增一个预约：userId用户 id（可以为 null，表示匿名用户）；
        //name姓名、phoneNum 手机号、houseId 房间 id、visiteDate 预约看房时间 
        long AddNew(long?userId,string name,string phoneNum,long houseId,DateTime visitDate);
        bool Follow(long adminUserId, long houseAppointmentId); //使用乐观锁解决并发的问题（这里先不实现，先抛个异常，后面再做） 
        long GetTotalCount(long cityId, string status);//得到CityId这个城市中状态为status的预约订单数
        //limit 后面两个数不能用计算表达式，只能用固定的值，因此只能通过参数传递，计算在 java 中完成。 
        HouseAppointmentDTO[] GetPagedData(long cityId, String status, int pageSize, int currentIndex);
        HouseAppointmentDTO GetById(long id);

    }
}
