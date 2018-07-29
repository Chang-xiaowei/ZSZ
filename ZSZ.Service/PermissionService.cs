using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;

namespace ZSZ.Service
{
    public class PermissionService : IPermissionService
    {
        /// <summary>
        /// 为roleId增加权限，新增权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="permIds"></param>
        public void AddPermIds(long roleId, long[] permIds)
        {
            using (ZSZDbContext ctx=new Service.ZSZDbContext())
            {
                BaseService<RoleEntity> roleBs = new Service.BaseService<Entities.RoleEntity>(ctx);
                var role = roleBs.GetById(roleId);
                if (role==null)
                {
                    throw new ArgumentException("角色不存在" + roleId);
                }
                BaseService<PermissionEntity> perm = new Service.BaseService<Entities.PermissionEntity>(ctx);
               var perms= perm.GetAll().ToList().Where(p => permIds.Contains(p.Id));
                foreach (var p in perms)
                {
                    role.Permissions.Add(p);
                }
                ctx.SaveChanges();
            }
        }
        /// <summary>
        /// 新增一个权限
        /// </summary>
        /// <param name="permName"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public long AddPermission(string permName, string description)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<PermissionEntity> bs = new BaseService<PermissionEntity>(ctx);
                bool exsits = bs.GetAll().Any(p => p.Name == permName);
                if (exsits)
                {
                    throw new ArgumentException("权限项已存在" + permName);
                }
                PermissionEntity perm = new PermissionEntity();
                perm.Description = description;
                perm.Name = permName;
                ctx.Permissions.Add(perm);
                ctx.SaveChanges();
                return perm.Id;

            }
        }

        public PermissionDTO[] GetAll()
        {
            using (ZSZDbContext ctx=new Service.ZSZDbContext())
            {
                BaseService<PermissionEntity> perms = new Service.BaseService<Entities.PermissionEntity>(ctx);
               return  perms.GetAll().ToList().Select(p => ToDTO(p)).ToArray();
            }
        }

        private PermissionDTO ToDTO(PermissionEntity entity)
        {
            PermissionDTO dto = new PermissionDTO();
            dto.Name = entity.Name;
            dto.CreateDateTime = entity.CreateDateTime;
            dto.Description = entity.Description;
            dto.Id = entity.Id;
            return dto;
        }

        public PermissionDTO GetById(long id)
        {
            using (ZSZDbContext ctx=new Service.ZSZDbContext())
            {
                BaseService<PermissionEntity> bs = new BaseService<PermissionEntity>(ctx);
                var perm = bs.GetById(id);
                if (perm==null)
                {
                    return null;
                }
                return ToDTO(perm);
            }
        }

        public PermissionDTO GetByName(string name)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<PermissionEntity> permBs = new BaseService<PermissionEntity>(ctx);
                var perm = permBs.GetAll().SingleOrDefault(p => p.Name == name);
                return perm == null ? null : ToDTO(perm);
            }
        }

        public PermissionDTO[] GetByRoleId(long roleId)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<RoleEntity> roleBs = new BaseService<RoleEntity>(ctx);
                return roleBs.GetById(roleId).Permissions.ToList().Select(p => ToDTO(p)).ToArray();
            }
        }

        public void MarkDeleted(long id)
        {
            using (ZSZDbContext ctx = new Service.ZSZDbContext())
            {
                BaseService<PermissionEntity> bs = new Service.BaseService<Entities.PermissionEntity>(ctx);
                bs.MarkDeleted(id);
            }
        }
        /// <summary>
        /// 修改某个角色的权限，思想，先把原来的权限清空，给它重新添加权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="permIds"></param>
        public void UpdatePermIds(long roleId, long[] permIds)
        {
            using (ZSZDbContext ctx=new Service.ZSZDbContext() )
            {
                BaseService<RoleEntity> roleBs = new BaseService<RoleEntity>(ctx);
                var role = roleBs.GetById(roleId);
                if (role==null)
                {
                    throw new ArgumentException("角色不存在" + roleId);
                }
                //把原来的权限清空
                role.Permissions.Clear();
                BaseService<PermissionEntity> permBs = new BaseService<PermissionEntity>(ctx);
                var perms = permBs.GetAll().Where(p => permIds.Contains(p.Id)).ToList();
                foreach (var p in perms)
                {
                    role.Permissions.Add(p);
                }
                ctx.SaveChanges();
            }
        }
        /// <summary>
        /// 根据权限Id更新权限名和描述信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="permName"></param>
        /// <param name="description"></param>
       
        public void UpdatePermission(long id, string permName, string description)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<PermissionEntity> permBS = new BaseService<PermissionEntity>(ctx);
                var perm = permBS.GetById(id);
                if (perm == null)
                {
                    throw new ArgumentException("权限id不存在" + id);
                }
                perm.Name = permName;
                perm.Description = description;
                ctx.SaveChanges();
            }
        }
    }
}
