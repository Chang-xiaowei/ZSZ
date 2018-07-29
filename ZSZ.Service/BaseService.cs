using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;
using ZSZIService;

namespace ZSZ.Service
{
    class BaseService<T> where T: BaseEntity
    {
        private ZSZDbContext ctx;
        public BaseService(ZSZDbContext ctx)
        {
            this.ctx = ctx;
        }
        /// <summary>
        /// 获取所有没有软删除的数据
        /// </summary>
        /// <returns></returns>
        public IQueryable<T>GetAll()
        {
            return ctx.Set<T>().Where(e => e.IsDeleted == false);
        }
        /// <summary>
        /// 获取总数据条数
        /// </summary>
        /// <returns></returns>
        public long GetTotalCount()
        {
            return GetAll().LongCount();
        }
        public IQueryable<T>GetPageData(int startIndex,int count)
        {
            return GetAll().OrderBy(e=>e.CreateDateTime).Skip(startIndex).Take(count);
        }
        /// <summary>
        /// 查找id=id的数据，如果找不到返回null
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById(long id)
        {
            return GetAll().Where(e => e.Id == id).SingleOrDefault();
        }
        public void MarkDeleted(long id)
        {
            var data = GetById(id);
            data.IsDeleted = true;
            ctx.SaveChanges();
        }
    }
}
