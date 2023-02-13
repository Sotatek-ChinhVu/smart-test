using Domain.Models.User;
using Entity.Tenant;
using Helper.Common;
using Helper.Constant;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Helper.Constants.UserConst;

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
            var listKaMsts = NoTrackingDataContext.KaMsts.Where(item =>
                                                                    query.Select(item => item.KaId).ToList()
                                                                    .Contains(item.KaId)
                                                                    && item.IsDeleted == 0
                                                              ).ToList();

            return query.OrderBy(u => u.SortNo).AsEnumerable().Select(u => ToModel(u, listKaMsts)).ToList();
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
                        userMst.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    }
                    else
                    {
                        TrackingDataContext.UserMsts.Add(ConvertUserList(inputData));
                    }
                }
            }
            TrackingDataContext.SaveChanges();
        }

        private static UserMstModel ToModel(UserMst u, List<KaMst> listKaMsts)
        {
            return new UserMstModel(
                u.HpId,
                u.Id,
                u.UserId,
                u.JobCd,
                u.ManagerKbn,
                u.KaId,
                listKaMsts.FirstOrDefault(item => item.KaId == u.KaId)?.KaSname ?? string.Empty,
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

        private static UserMstModel ToModel(UserMst u)
        {
            return new UserMstModel(
                u.HpId,
                u.Id,
                u.UserId,
                u.JobCd,
                u.ManagerKbn,
                u.KaId,
                string.Empty,
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

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        public bool CheckLockMedicalExamination(int hpId, long ptId, long raiinNo, int sinDate, string token, int userId)
        {
            // Check lockMedicalExamination
            var raiinInfo = NoTrackingDataContext.RaiinInfs.FirstOrDefault(p => p.HpId == hpId && p.PtId == ptId && p.SinDate == sinDate && p.RaiinNo == raiinNo && p.IsDeleted == DeleteTypes.None);
            long oyaRaiinNo = raiinInfo != null ? raiinInfo.OyaRaiinNo : 0;
            var result = CheckLockInfo(hpId, ptId, FunctionCode.MedicalExaminationCode, raiinNo, oyaRaiinNo, sinDate, token, userId);
            return GetPermissionByScreenCode(hpId, userId, FunctionCode.MedicalExaminationCode) == PermissionType.Unlimited && !(result != null && result.Item2?.LockLevel == 0);
        }

        private Tuple<LockInf, LockMst, UserMst, FunctionMst>? CheckLockInfo(int hpID, long ptID_B, string functionCD_B, long raiinNo_B, long oyaRaiinNo_B, int sinDate_B, string token, int currentUserID)
        {
            var listCheckedResult =
                (
                    from lockInf in NoTrackingDataContext.LockInfs.Where(i => i.HpId == hpID && i.PtId == ptID_B && i.Machine != token)
                    join raiinInf in NoTrackingDataContext.RaiinInfs.Where(r => r.HpId == hpID)
                    on lockInf.RaiinNo equals raiinInf.RaiinNo into rfg
                    from lockedRaiinInf in rfg.DefaultIfEmpty()
                    join lockMst in NoTrackingDataContext.LockMsts.Where(m => m.FunctionCdB == functionCD_B && m.IsInvalid == 0)
                    on lockInf.FunctionCd equals lockMst.FunctionCdA
                    join userMst in NoTrackingDataContext.UserMsts.Where(u => u.HpId == hpID && u.IsDeleted != 1 && u.StartDate <= sinDate_B && sinDate_B <= u.EndDate)
                    on lockInf.UserId equals userMst.UserId into gj
                    from lockedUserInf in gj.DefaultIfEmpty()
                    join functionMst in NoTrackingDataContext.FunctionMsts
                    on lockInf.FunctionCd equals functionMst.FunctionCd
                    where (lockMst.FunctionCdA != lockMst.FunctionCdB) || (lockMst.FunctionCdA == lockMst.FunctionCdB && (lockInf.Machine != token || lockInf.UserId != currentUserID))
                    orderby lockMst.LockLevel, lockMst.LockRange
                    select new
                    {
                        lockInf,
                        lockMst,
                        lockedUserInf,
                        functionMst,
                        lockedRaiinInf
                    }
                ).ToList();

            if (listCheckedResult == null) return null;

            for (int i = 0; i < listCheckedResult.Count; i++)
            {
                var checkedResult = listCheckedResult[i];
                if (checkedResult.lockMst.LockRange == 1 && checkedResult.lockInf.RaiinNo != raiinNo_B)
                {
                    continue;
                }
                else if (checkedResult.lockMst.LockRange == 2 && checkedResult.lockedRaiinInf != null && checkedResult.lockedRaiinInf.OyaRaiinNo != oyaRaiinNo_B)
                {
                    continue;
                }
                else if (checkedResult.lockMst.LockRange == 3 && checkedResult.lockInf.SinDate != sinDate_B)
                {
                    continue;
                }
                return new Tuple<LockInf, LockMst, UserMst, FunctionMst>(checkedResult.lockInf, checkedResult.lockMst, checkedResult.lockedUserInf, checkedResult.functionMst);
            }

            return null;
        }


        public PermissionType GetPermissionByScreenCode(int hpId, int userId, string permisionCode)
        {
            var listUserPermission = NoTrackingDataContext.UserPermissions.Where(u => u.HpId == hpId && u.UserId == userId).ToList();
            var listUserPermissionOfUserDefault = NoTrackingDataContext.UserPermissions.Where(u => u.HpId == hpId && u.UserId == 0).ToList();
            var isDoctor = NoTrackingDataContext.UserMsts.FirstOrDefault(u => u.UserId == userId && u.HpId == hpId && u.IsDeleted == DeleteTypes.None)?.JobCd == 1;
            if (string.IsNullOrEmpty(permisionCode))
            {
                return PermissionType.NotAvailable;
            }
            UserPermission? userPermission = null;
            if (listUserPermission != null)
            {
                userPermission = listUserPermission.FirstOrDefault(u => u.FunctionCd == permisionCode);
            }
            if (userPermission == null && listUserPermissionOfUserDefault != null)
            {
                // get permission of userID = 0
                userPermission = listUserPermissionOfUserDefault.FirstOrDefault(u => u.FunctionCd == permisionCode);
            }
            if (userPermission != null)
            {
                return GetPermissionTypeByCode(userPermission.Permission);
            }
            return GetDefaultPermission(permisionCode, isDoctor);
        }

        private PermissionType GetPermissionTypeByCode(int code)
        {
            switch (code)
            {
                case 0:
                    return PermissionType.Unlimited;
                case 1:
                    return PermissionType.ReadOnly;
                case 99:
                    return PermissionType.NotAvailable;
                default:
                    return PermissionType.NotAvailable;
            }
        }

        private PermissionType GetDefaultPermission(string permissionCode, bool isDoctor)
        {
            switch (permissionCode)
            {
                case FunctionCode.MedicalExaminationCode:
                    return PermissionType.Unlimited;
                case FunctionCode.SuperSetCode:
                    return PermissionType.Unlimited;
                case FunctionCode.ApprovalInfo:
                    if (isDoctor)
                    {
                        return PermissionType.Unlimited;
                    }
                    return PermissionType.NotAvailable;
                case FunctionCode.PatientInfo:
                    return PermissionType.Unlimited;
                case FunctionCode.CheckDrugInfo:
                case FunctionCode.CheckSpecificHealth:
                    return PermissionType.Unlimited;
                case FunctionCode.Accounting:
                    return PermissionType.Unlimited;
                case FunctionCode.EditSummary:
                case FunctionCode.MasterMaintenanceCode:
                case FunctionCode.HolidaySettingCode:
                    return PermissionType.Unlimited;
                case FunctionCode.Sta1001:
                case FunctionCode.Sta1002:
                case FunctionCode.Sta1010:
                case FunctionCode.Sta2001:
                case FunctionCode.Sta2002:
                case FunctionCode.Sta2003:
                case FunctionCode.Sta2010:
                case FunctionCode.Sta2011:
                case FunctionCode.Sta2020:
                case FunctionCode.Sta2021:
                case FunctionCode.Sta3001:
                case FunctionCode.Sta3010:
                case FunctionCode.Sta3020:
                case FunctionCode.Sta3030:
                case FunctionCode.Sta3040:
                case FunctionCode.Sta3041:
                case FunctionCode.Sta3050:
                case FunctionCode.Sta3060:
                case FunctionCode.Sta3061:
                case FunctionCode.Sta3070:
                case FunctionCode.Sta3071:
                case FunctionCode.Sta3080:
                case FunctionCode.PatientManagement:
                case FunctionCode.LockInf:
                    return PermissionType.Unlimited;
                default:
                    throw new NotSupportedException("Not supported for code : " + permissionCode);
            }
        }
    }
}
