using Domain.Models.User;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        public UserRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public void Create(UserMstModel user)
        {
            throw new NotImplementedException();
        }

        public void Delete(UserId userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserMstModel> GetAll()
        {
            return _tenantDataContext.UserMsts.ToList().Select(u => ToModel(u));
        }

        public UserMstModel? GetByUserId(int userId)
        {
            var entity = _tenantDataContext.UserMsts
                .Where(u => u.UserId == userId && u.IsDeleted == DeleteTypes.None).FirstOrDefault();
            return entity is null ? null : ToModel(entity);
        }

        public int MaxUserId()
        {
            return _tenantDataContext.UserMsts.Max(u => u.UserId);
        }

        public UserMstModel Read(UserId userId)
        {
            throw new NotImplementedException();
        }

        public void Update(UserMstModel user)
        {
            throw new NotImplementedException();
        }

        private UserMstModel ToModel(UserMst u)
        {
            return new UserMstModel(
                u.UserId,
                u.JobCd,
                u.ManagerKbn,
                u.KaId,
                u.KanaName,
                u.Name,
                u.Sname,
                u.DrName,
                u.LoginId,
                u.LoginPass,
                u.MayakuLicenseNo ?? string.Empty,
                u.StartDate,
                u.EndDate,
                u.SortNo,
                u.RenkeiCd1 ?? string.Empty,
                u.IsDeleted);
        }
    }
}
