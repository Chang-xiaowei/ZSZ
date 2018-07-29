using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;
using System.Data.Entity;

namespace ZSZ.Service
{
    public class RoleService : IRoleService
    {
        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public long AddNew(string roleName)
        {
            using (ZSZDbContext ctx=new Service.ZSZDbContext())
            {
                BaseService<RoleEntity> bs = new BaseService<RoleEntity>(ctx);
                bool exsits = bs.GetAll().Any(r => r.Name == roleName);
                if (exsits)
                {
                    throw new ArgumentException("角色已存在" + roleName);
                }
                RoleEntity role = new RoleEntity();
                role.Name = roleName;
                ctx.Roles.Add(role);
                ctx.SaveChanges();
                return role.Id;
            }
        }
        /// <summary>
        /// 给adminUserId这个用户添加角色
        /// </summary>
        /// <param name="adminUserId"></param>
        /// <param name="roleIds"></param>
        public void AddRoleIds(long adminUserId, long[] roleIds)
        {
            using (ZSZDbContext ctx=new ZSZDbContext())
            {
                BaseService<AdminUserEntity> AdminService = new BaseService<AdminUserEntity>(ctx);
                var adminUser = AdminService.GetById(adminUserId);
                if (adminUser!=null)
                {
                    BaseService<RoleEntity> roleBS = new BaseService<RoleEntity>(ctx);
                    var roles = roleBS.GetAll().Where(r => roleIds.Contains(r.Id)).ToArray();
                    foreach (var role in roles)
                    {
                        adminUser.Roles.Add(role);
                    }
                    ctx.SaveChanges();
                }
                else
                {
                    throw new ArgumentException("不存在Id为" + adminUserId + "的用户");
                }

            }
        }
        /// <summary>
        /// 得到Role的所有信息
        /// </summary>
        /// <returns></returns>
        public RoleDTO[] GetAll()
        {
            using (ZSZDbContext ctx=new ZSZDbContext())
            {
                BaseService<RoleEntity> bs = new BaseService<RoleEntity>(ctx);
                return bs.GetAll().ToList().Select(r => ToDTO(r)).ToArray();
            }
        }
        /// <summary>
        /// RoleEntity转存到dto的方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private RoleDTO ToDTO(RoleEntity entity)
        {
            RoleDTO dto = new RoleDTO();
            dto.Name = entity.Name;
            dto.Id = entity.Id;
            dto.CreateDateTime = entity.CreateDateTime;
            return dto;
        }
        /// <summary>
        /// 得到adminUserId这个用户所有的角色
        /// </summary>
        /// <param name="adminUserId"></param>
        /// <returns></returns>
        public RoleDTO[] GetByAdminUserId(long adminUserId)
        {
            using (ZSZDbContext ctx=new ZSZDbContext())
            {
                BaseService<AdminUserEntity> userBs = new BaseService<AdminUserEntity>(ctx);
                var user = userBs.GetById(adminUserId);
                if (user==null)
                {
                    throw new ArgumentException("不存在Id为" + adminUserId + "的管理员");
                }
                return user.Roles.ToList().Select(r => ToDTO(r)).ToArray();               
            }
        }
        /// <summary>
        /// 根据Id求角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RoleDTO GetById(long id)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<RoleEntity> userBs = new BaseService<RoleEntity>(ctx);
                var user = userBs.GetById(id);
                if (user == null)
                {
                    return null;
                   // throw new ArgumentException("不存在Id为" + id + "的管理员");
                }
                return ToDTO(user);
            }
        }

        public RoleDTO GetByName(string name)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<RoleEntity> userBs = new BaseService<RoleEntity>(ctx);
                var user = userBs.GetAll().SingleOrDefault(r=>r.Name==name);
                if (user == null)
                {
                    return null;
                }
                return ToDTO(user);
            }
        }
        public void MarkDeleted(long roleId)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<RoleEntity> userBs = new BaseService<RoleEntity>(ctx);
                userBs.MarkDeleted(roleId);
            }
        }
        /// <summary>
        /// 根据roleId更新角色名称
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="roleName"></param>
        public void Update(long roleId, string roleName)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<RoleEntity> bs = new BaseService<RoleEntity>(ctx);
                var role = bs.GetAll().ToList().SingleOrDefault(r=>r.Name==roleName && r.Id==roleId);
                //正常情况不应该执行这个异常，因为UI层应该把这些情况处理好
                //这里只是“把好最后一关”
                if (role==null)
                {
                    throw new ArgumentException("不存在" + roleId + "角色");
                }
                /*role.Name = roleName;
                ctx.Roles.Add(role);
                ctx.SaveChanges();*/
                RoleEntity roleentity = new Entities.RoleEntity();
                roleentity.Id = roleId;
                ctx.Entry(role).State = EntityState.Unchanged;
                role.Name = roleName;
                ctx.SaveChanges();
                
            }
        }
        /// <summary>
        ///  为某个用户更新角色
        /// </summary>
        /// <param name="adminUserId"></param>
        /// <param name="roleIds"></param>
        public void UpdateRoleIds(long adminUserId, long[] roleIds)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<AdminUserEntity> bs = new BaseService<AdminUserEntity>(ctx);
                var adminUser = bs.GetById(adminUserId);
                if (adminUser==null)
                {
                    throw new ArgumentException("不存在Id为"+ adminUserId + "的管理员");
                }
                adminUser.Roles.Clear();
                BaseService<RoleEntity> roleBs = new BaseService<RoleEntity>(ctx);
                var roles=roleBs.GetAll().Where(r => roleIds.Contains(r.Id)).ToArray();
                foreach (var role in roles)
                {
                    adminUser.Roles.Add(role);
                }
                ctx.SaveChanges();
            }
        }
    }
}
