using Domain.Models.User;
using Entity.Tenant;
using Helper.Constant;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TenantNoTrackingDataContext _tenantNoTrackingDataContext;
        private readonly TenantDataContext _tenantTrackingDataContext;
        public UserRepository(ITenantProvider tenantProvider)
        {
            _tenantNoTrackingDataContext = tenantProvider.GetNoTrackingDataContext();
            _tenantTrackingDataContext = tenantProvider.GetTrackingTenantDataContext();
        }

        public bool CheckExistedId(List<long> idList)
        {
            return _tenantNoTrackingDataContext.UserMsts.Any(u => idList.Contains(u.Id));
        }

        public bool CheckExistedUserId(int userId)
        {
            return _tenantNoTrackingDataContext.UserMsts.Any(u => u.UserId == userId && u.IsDeleted == 0);
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
            return _tenantNoTrackingDataContext.UserMsts.AsEnumerable().Select(u => ToModel(u)).ToList();
        }

        public List<UserMstModel> GetAll(int sinDate, bool isDoctorOnly)
        {
            var query = _tenantNoTrackingDataContext.UserMsts.Where(u =>
                u.StartDate <= sinDate
                && u.EndDate >= sinDate
                && u.IsDeleted == DeleteTypes.None);
            if (isDoctorOnly)
            {
                query = query.Where(u => u.JobCd == JobCodes.Doctor);
            }

            return query.OrderBy(u => u.SortNo).AsEnumerable().Select(u => ToModel(u)).ToList();
        }

        public IEnumerable<UserMstModel> GetDoctorsList(int userId)
        {
            var result = _tenantNoTrackingDataContext.UserMsts.Where(d => d.IsDeleted == 0 && d.JobCd == JobCdConstant.Doctor && d.UserId == userId).AsEnumerable();
            return result.Select(u => ToModel(u)).OrderBy(i => i.SortNo);
        }

        public IEnumerable<UserMstModel> GetDoctorsList(List<int> userIds)
        {
            var result = _tenantNoTrackingDataContext.UserMsts.Where(d => d.IsDeleted == 0 && d.JobCd == JobCdConstant.Doctor && userIds.Contains(d.UserId)).AsEnumerable();
            return result.Select(u => ToModel(u)).OrderBy(i => i.SortNo);
        }

        public UserMstModel? GetByUserId(int userId)
        {
            var entity = _tenantNoTrackingDataContext.UserMsts
                .Where(u => u.UserId == userId && u.IsDeleted == DeleteTypes.None).FirstOrDefault();
            return entity is null ? null : ToModel(entity);
        }

        public UserMstModel? GetByLoginId(string loginId)
        {
            var entity = _tenantNoTrackingDataContext.UserMsts
                .Where(u => u.LoginId == loginId && u.IsDeleted == DeleteTypes.None).FirstOrDefault();
            return entity is null ? null : ToModel(entity);
        }

        public int MaxUserId()
        {
            return _tenantNoTrackingDataContext.UserMsts.Max(u => u.UserId);
        }

        public UserMstModel Read(int userId)
        {
            throw new NotImplementedException();
        }

        public void Update(UserMstModel user)
        {
            throw new NotImplementedException();
        }
        public void Upsert(List<UserMstModel> upsertUserList)
        {
            foreach(var inputData in upsertUserList)
            {
                if(inputData.IsDeleted == DeleteTypes.Deleted)
                {
                    var userMsts = _tenantTrackingDataContext.UserMsts.FirstOrDefault(u => u.Id == inputData.Id);
                    if(userMsts != null)
                    {
                        userMsts.IsDeleted = DeleteTypes.Deleted;
                    }
                }
                else
                {
                    var userMsts = _tenantNoTrackingDataContext.UserMsts.FirstOrDefault(u => u.Id == inputData.Id && u.IsDeleted == inputData.IsDeleted);
                    if( userMsts != null)
                    {
                        var updateUser = ConvertUserList(inputData);
                        updateUser.UpdateId = TempIdentity.UserId;
                        updateUser.UpdateDate = DateTime.UtcNow;
                        updateUser.UpdateMachine = TempIdentity.ComputerName;
                        _tenantTrackingDataContext.Update(updateUser);
                    }
                    else
                    {
                        _tenantTrackingDataContext.UserMsts.Add(ConvertUserList(inputData));
                    }
                }
            }
            _tenantTrackingDataContext.SaveChanges();
        }

        private static UserMstModel ToModel(UserMst u)
        {
            return new UserMstModel(
                u.Id,
                u.UserId,
                u.JobCd,
                u.ManagerKbn,
                u.KaId,
                u.KanaName ?? string.Empty,
                u.Name ?? string.Empty,
                u.Sname ?? string.Empty,
                u.DrName ?? string.Empty,
                u.LoginId ?? string.Empty,
                u.LoginPass ?? string.Empty,
                u.MayakuLicenseNo ?? string.Empty,
                u.StartDate,
                u.EndDate,
                u.SortNo,
                u.RenkeiCd1 ?? string.Empty,
                u.IsDeleted);
        }
        private static UserMst ConvertUserList(UserMstModel u)
        {
            return new UserMst{
                Id = u.Id,
                UserId = u.UserId,
                JobCd = u.JobCd,
                ManagerKbn = u.ManagerKbn,
                KaId = u.KaId,
                KanaName = u.KanaName ?? string.Empty,
                Name = u.Name ?? string.Empty,
                Sname = u.Sname ?? string.Empty,
                DrName = u.DrName ?? string.Empty,
                LoginId = u.LoginId ?? string.Empty,
                LoginPass = u.LoginPass ?? string.Empty,
                MayakuLicenseNo = u.MayakuLicenseNo ?? string.Empty,
                StartDate = u.StartDate,
                EndDate = u.EndDate,
                SortNo = u.SortNo,
                RenkeiCd1 = u.RenkeiCd1 ?? string.Empty,
                IsDeleted = u.IsDeleted,
            };
                
        }
    }
}
