using Domain.Models.User;
using Entity.Tenant;
using Infrastructure.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;

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

        public void Delete(int userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserMstModel> GetAll()
        {
            return _tenantDataContext.UserMsts.Select(u => ConvertToModel(u)).ToList();
        }
        public IEnumerable<UserMstModel> GetAllDoctors()
        {
            return _tenantDataContext.UserMsts.Where(d => d.IsDeleted == 0 && d.JobCd == JobCdConstant.Doctor).Select(u => ConvertToModel(u)).OrderBy(i => i.SortNo).ToList();
        }

        public int MaxUserId()
        {
            return _tenantDataContext.UserMsts.Max(u => u.UserId);
        }

        public UserMstModel Read(int userId)
        {
            throw new NotImplementedException();
        }

        public void Update(UserMstModel user)
        {
            throw new NotImplementedException();
        }

        private UserMstModel ConvertToModel(UserMst itemData)
        {
            return new UserMstModel(
                itemData.HpId,
                itemData.Id,
                itemData.UserId,
                itemData.JobCd,
                itemData.ManagerKbn,
                itemData.KaId,
                itemData.KanaName,
                itemData.Name,
                itemData.Sname,
                itemData.LoginId,
                itemData.LoginPass,
                itemData.MayakuLicenseNo,
                itemData.StartDate,
                itemData.EndDate,
                itemData.SortNo,
                itemData.IsDeleted,
                itemData.RenkeiCd1,
                itemData.DrName
              );
        }

    }
}
