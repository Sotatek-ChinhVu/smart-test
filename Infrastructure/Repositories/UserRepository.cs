using Domain.Models.User;
using Entity.Tenant;
using Helper.Constant;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System.Linq;
using System.Xml.Linq;

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

        public bool CheckExistedUserIdCreate(List<int> UserIds)
        {
            var countUsertMsts = _tenantNoTrackingDataContext.UserMsts.Count(u => UserIds.Contains(u.UserId) && u.Id == 0 && u.IsDeleted != 1);
            return UserIds.Count <= countUsertMsts;
        }

        public bool CheckExistedUserIdUpdate(List<long> Ids, List<int> UserIds)
        {
            var countUsertMsts = _tenantNoTrackingDataContext.UserMsts.Count(u => UserIds.Contains(u.UserId) && !Ids.Contains(u.Id) && u.IsDeleted != 1);
            return UserIds.Count <= countUsertMsts;
        }

        public bool CheckExistedLoginIdCreate(List<string> LoginIds)
        {
            var countUsertMsts = _tenantNoTrackingDataContext.UserMsts.Count(u => LoginIds.Contains(u.LoginId) && u.Id == 0 && u.IsDeleted != 1);
            return LoginIds.Count <= countUsertMsts;
        }

        public bool CheckExistedLoginIdUpdate(List<long> Ids, List<string> LoginIds)
        {
            var countUsertMsts = _tenantNoTrackingDataContext.UserMsts.Count(u => LoginIds.Contains(u.LoginId) && !Ids.Contains(u.Id) && u.IsDeleted != 1);
            return LoginIds.Count <= countUsertMsts;
        }

        public bool CheckExistedJobCd(List<int> JobCds)
        {
            var countUsertMsts = _tenantNoTrackingDataContext.JobMsts.Count(u => JobCds.Contains(u.JobCd));
            return JobCds.Count <= countUsertMsts;
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
            foreach (var inputData in upsertUserList)
            {
                if (inputData.IsDeleted == DeleteTypes.Deleted)
                {
                    var userMsts = _tenantTrackingDataContext.UserMsts.FirstOrDefault(u => u.Id == inputData.Id);
                    if (userMsts != null)
                    {
                        userMsts.IsDeleted = DeleteTypes.Deleted;
                    }
                }
                else
                {
                    var userMst = _tenantTrackingDataContext.UserMsts.FirstOrDefault(u => u.Id == inputData.Id && u.IsDeleted == inputData.IsDeleted);
                    if (userMst != null)
                    {
                        userMst.HpId = TempIdentity.HpId;
                        userMst.UserId = inputData.UserId;
                        userMst.JobCd = inputData.JobCd;
                        userMst.ManagerKbn = inputData.ManagerKbn;
                        userMst.KaId = inputData.KaId;
                        userMst.KanaName = inputData.KanaName ?? string.Empty;
                        userMst.Name = inputData.Name ?? string.Empty;
                        userMst.Sname = inputData.Sname ?? string.Empty;
                        userMst.DrName = inputData.DrName ?? string.Empty;
                        userMst.LoginId = inputData.LoginId ?? string.Empty;
                        userMst.LoginPass = inputData.LoginPass ?? string.Empty;
                        userMst.MayakuLicenseNo = inputData.MayakuLicenseNo ?? string.Empty;
                        userMst.StartDate = inputData.StartDate;
                        userMst.EndDate = inputData.EndDate;
                        userMst.SortNo = inputData.SortNo;
                        userMst.RenkeiCd1 = inputData.RenkeiCd1 ?? string.Empty;
                        userMst.IsDeleted = inputData.IsDeleted;
                        userMst.UpdateId = TempIdentity.UserId;
                        userMst.UpdateDate = DateTime.UtcNow;
                        userMst.UpdateMachine = TempIdentity.ComputerName;
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
                u.HpId,
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
            return new UserMst
            {
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
