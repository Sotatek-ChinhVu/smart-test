using Domain.Models.User;
using Entity.Tenant;
using Helper.Constant;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using PostgreDataContext;
using System.Linq;
using System.Xml.Linq;

namespace Infrastructure.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public UserRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {

        }
        public bool CheckExistedId(List<long> ids)
        {
            var anyUsertMsts = NoTrackingDataContext.UserMsts.Count(u => ids.Contains(u.Id));
            return ids.Count == anyUsertMsts;
        }

        public bool CheckExistedUserId(int userId)
        {
            return NoTrackingDataContext.UserMsts.Any(u => u.UserId == userId && u.IsDeleted == 0);
        }

        public bool CheckExistedUserIdCreate(List<int> userIds)
        {
            var anyUsertMsts = NoTrackingDataContext.UserMsts.Any(u => userIds.Contains(u.UserId) && u.IsDeleted != 1);
            return anyUsertMsts;
        }

        public bool CheckExistedUserIdUpdate(List<long> ids, List<int> userIds)
        {
            var anyUsertMsts = NoTrackingDataContext.UserMsts.Any(u => userIds.Contains(u.UserId) && !ids.Contains(u.Id) && u.IsDeleted != 1);
            return anyUsertMsts;
        }

        public bool CheckExistedLoginIdCreate(List<string> loginIds)
        {
            var anyUsertMsts = NoTrackingDataContext.UserMsts.Any(u => loginIds.Contains(u.LoginId ?? string.Empty) && u.IsDeleted != 1);
            return anyUsertMsts;
        }

        public bool CheckExistedLoginIdUpdate(List<long> ids, List<string> loginIds)
        {
            var anyUsertMsts = NoTrackingDataContext.UserMsts.Any(u => loginIds.Contains(u.LoginId ?? string.Empty) && !ids.Contains(u.Id) && u.IsDeleted != 1);
            return anyUsertMsts;
        }

        public bool CheckExistedJobCd(List<int> jobCds)
        {
            var countUsertMsts = NoTrackingDataContext.JobMsts.Count(u => jobCds.Contains(u.JobCd));
            return jobCds.Count == countUsertMsts;
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
            return NoTrackingDataContext.UserMsts.AsEnumerable().Select(u => ToModel(u)).ToList();
        }

        public List<UserMstModel> GetAll(int sinDate, bool isDoctorOnly)
        {
            var query = NoTrackingDataContext.UserMsts.Where(u =>
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
            var result = NoTrackingDataContext.UserMsts.Where(d => d.IsDeleted == 0 && d.JobCd == JobCdConstant.Doctor && d.UserId == userId).AsEnumerable();
            return result.Select(u => ToModel(u)).OrderBy(i => i.SortNo);
        }

        public IEnumerable<UserMstModel> GetDoctorsList(List<int> userIds)
        {
            var result = NoTrackingDataContext.UserMsts.Where(d => d.IsDeleted == 0 && d.JobCd == JobCdConstant.Doctor && userIds.Contains(d.UserId)).AsEnumerable();
            return result.Select(u => ToModel(u)).OrderBy(i => i.SortNo);
        }

        public UserMstModel? GetByUserId(int userId)
        {
            var entity = NoTrackingDataContext.UserMsts
                .Where(u => u.UserId == userId && u.IsDeleted == DeleteTypes.None).FirstOrDefault();
            return entity is null ? null : ToModel(entity);
        }

        public UserMstModel GetByUserId(int userId, int sinDate)
        {
            var entity = NoTrackingDataContext.UserMsts
                .FirstOrDefault(u => u.UserId == userId
                                    && u.IsDeleted == DeleteTypes.None
                                    && (sinDate <= 0 || u.StartDate <= sinDate && u.EndDate >= sinDate));
            return entity is null ? new UserMstModel() : ToModel(entity);
        }

        public UserMstModel? GetByLoginId(string loginId)
        {
            var entity = NoTrackingDataContext.UserMsts
                .Where(u => u.LoginId == loginId && u.IsDeleted == DeleteTypes.None).FirstOrDefault();
            return entity is null ? null : ToModel(entity);
        }

        public int MaxUserId()
        {
            return NoTrackingDataContext.UserMsts.Max(u => u.UserId);
        }

        public UserMstModel Read(int userId)
        {
            throw new NotImplementedException();
        }

        public void Update(UserMstModel user)
        {
            throw new NotImplementedException();
        }
        public void Upsert(List<UserMstModel> upsertUserList, int userId)
        {
            foreach (var inputData in upsertUserList)
            {
                if (inputData.IsDeleted == DeleteTypes.Deleted)
                {
                    var userMsts = TrackingDataContext.UserMsts.FirstOrDefault(u => u.Id == inputData.Id);
                    if (userMsts != null)
                    {
                        userMsts.IsDeleted = DeleteTypes.Deleted;
                    }
                }
                else
                {
                    var userMst = TrackingDataContext.UserMsts.FirstOrDefault(u => u.Id == inputData.Id && u.IsDeleted == inputData.IsDeleted);
                    if (userMst != null)
                    {
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
                        userMst.UpdateId = userId;
                        userMst.UpdateDate = DateTime.UtcNow;
                    }
                    else
                    {
                        TrackingDataContext.UserMsts.Add(ConvertUserList(inputData));
                    }
                }
            }
            TrackingDataContext.SaveChanges();
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

        public bool CheckLoginInfo(string userName, string password)
        {
            return NoTrackingDataContext.UserMsts.Any(u => u.LoginId == userName && u.LoginPass == password);
        }

        public bool MigrateDatabase()
        {
            try
            {
                TrackingDataContext.Database.Migrate();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
