using Domain.Core;
using Domain.Models.User;
using Entity.Tenant;
using Helper.Common;
using Helper.Constant;
using Helper.Constants;
using Helper.Redis;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Konscious.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Security.Cryptography;
using System.Text;
using static Amazon.S3.Util.S3EventNotification;
using static Helper.Constants.UserConst;

namespace Infrastructure.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        private readonly string key;
        private readonly IDatabase _cache;
        private readonly IConfiguration _configuration;
        private readonly IUserInfoService _userInfoService;

        public UserRepository(ITenantProvider tenantProvider, IConfiguration configuration, IUserInfoService userInfoService) : base(tenantProvider)
        {
            key = GetDomainKey();
            _configuration = configuration;
            GetRedis();
            _cache = RedisConnectorHelper.Connection.GetDatabase();
            _userInfoService = userInfoService;
        }

        public void GetRedis()
        {
            string connection = string.Concat(_configuration["Redis:RedisHost"], ":", _configuration["Redis:RedisPort"]);
            if (RedisConnectorHelper.RedisHost != connection)
            {
                RedisConnectorHelper.RedisHost = connection;
            }
        }

        public bool CheckExistedId(List<long> ids)
        {
            // get data from UserMstList
            var anyUsertMsts = _userInfoService.AllUserMstList().Count(u => ids.Contains(u.Id));
            return ids.Count == anyUsertMsts;
        }

        public bool CheckExistedUserId(int userId)
        {
            // get data from UserMstList
            return _userInfoService.AllUserMstList().Any(u => u.UserId == userId && u.IsDeleted == 0);
        }

        public bool CheckExistedUserIdCreate(List<int> userIds)
        {
            // get data from UserMstList
            var anyUsertMsts = _userInfoService.AllUserMstList().Any(u => userIds.Contains(u.UserId) && u.IsDeleted != 1);
            return anyUsertMsts;
        }

        public bool CheckExistedUserIdUpdate(List<long> ids, List<int> userIds)
        {
            // get data from UserMstList
            var anyUsertMsts = _userInfoService.AllUserMstList().Any(u => userIds.Contains(u.UserId) && !ids.Contains(u.Id) && u.IsDeleted != 1);
            return anyUsertMsts;
        }

        public bool CheckExistedLoginIdCreate(List<string> loginIds)
        {
            // get data from UserMstList
            var anyUsertMsts = _userInfoService.AllUserMstList().Any(u => loginIds.Contains(u.LoginId ?? string.Empty) && u.IsDeleted != 1);
            return anyUsertMsts;
        }

        public bool CheckExistedLoginIdUpdate(List<long> ids, List<string> loginIds)
        {
            // get data from UserMstList
            var anyUsertMsts = _userInfoService.AllUserMstList().Any(u => loginIds.Contains(u.LoginId ?? string.Empty) && !ids.Contains(u.Id) && u.IsDeleted != 1);
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

        public List<UserMstModel> GetAll(int sinDate, bool isDoctorOnly, bool isAll)
        {
            if (isAll)
            {
                // get data from UserMstList
                var query = _userInfoService.AllUserMstList();
                return query.OrderBy(u => u.SortNo).AsEnumerable().Select(u => ToModel(u, new())).ToList();
            }
            else
            {
                // get data from UserMstList
                var query = _userInfoService.AllUserMstList().Where(u =>
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
        }

        public IEnumerable<UserMstModel> GetDoctorsList(int userId)
        {
            // get data from UserMstList
            var result = _userInfoService.AllUserMstList().Where(d => d.IsDeleted == 0 && d.JobCd == JobCdConstant.Doctor && d.UserId == userId);
            return result.Select(u => ToModel(u)).OrderBy(i => i.SortNo);
        }

        public IEnumerable<UserMstModel> GetDoctorsList(List<int> userIds)
        {
            // get data from UserMstList
            var result = _userInfoService.AllUserMstList().Where(d => d.IsDeleted == 0 && d.JobCd == JobCdConstant.Doctor && userIds.Contains(d.UserId));
            return result.Select(u => ToModel(u)).OrderBy(i => i.SortNo);
        }

        public IEnumerable<UserMstModel> GetListAnyUser(List<int> userIds)
        {
            // get data from UserMstList
            var result = _userInfoService.AllUserMstList().Where(d => userIds.Contains(d.UserId));
            return result.Select(u => ToModel(u)).OrderBy(i => i.SortNo);
        }

        public UserMstModel GetByUserId(int userId)
        {
            // get data from UserMstList
            var entity = _userInfoService.AllUserMstList().FirstOrDefault(u => u.UserId == userId && u.IsDeleted == DeleteTypes.None);
            return entity is null ? new UserMstModel() : ToModel(entity);
        }

        public UserMstModel GetByUserId(int userId, int sinDate)
        {
            // get data from UserMstList
            var entity = _userInfoService.AllUserMstList()
                .FirstOrDefault(u => u.UserId == userId
                                    && u.IsDeleted == DeleteTypes.None
                                    && (sinDate <= 0 || u.StartDate <= sinDate && u.EndDate >= sinDate));
            return entity is null ? new UserMstModel() : ToModel(entity);
        }

        public UserMstModel? GetByLoginId(string loginId, string password)
        {
            var timeNow = CIUtil.DateTimeToInt(CIUtil.GetJapanDateTimeNow());

            var entity = NoTrackingDataContext.UserMsts
                .Where(u => u.LoginId == loginId && u.IsDeleted == DeleteTypes.None && u.StartDate <= timeNow && u.EndDate >= timeNow).FirstOrDefault();
            if (entity is null)
            {
                return null;
            }
            if (!VerifyHash(Encoding.UTF8.GetBytes(password), entity.Salt, entity.HashPassword))
            {
                return null;
            }
            return ToModel(entity);
        }

        public int MaxUserId()
        {
            // get data from UserMstList
            return _userInfoService.AllUserMstList().Max(u => u.UserId);
        }

        public bool Upsert(List<UserMstModel> upsertUserList, int userId)
        {
            try
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
                        byte[] salt = GenerateSalt();
                        byte[] hashPassword = CreateHash(Encoding.UTF8.GetBytes(inputData.LoginPass ?? string.Empty), salt);
                        var userMst = TrackingDataContext.UserMsts.FirstOrDefault(u => u.Id == inputData.Id && u.IsDeleted == inputData.IsDeleted);
                        if (userMst != null)
                        {
                            userMst.JobCd = inputData.JobCd;
                            userMst.ManagerKbn = inputData.ManagerKbn;
                            userMst.KaId = inputData.KaId;
                            userMst.KanaName = inputData.KanaName ?? string.Empty;
                            userMst.Name = inputData.Name ?? string.Empty;
                            userMst.Sname = inputData.Sname ?? string.Empty;
                            userMst.DrName = inputData.DrName ?? string.Empty;
                            //userMst.LoginPass = inputData.LoginPass ?? string.Empty;
                            userMst.MayakuLicenseNo = inputData.MayakuLicenseNo ?? string.Empty;
                            userMst.StartDate = inputData.StartDate;
                            userMst.EndDate = inputData.EndDate;
                            userMst.SortNo = inputData.SortNo;
                            userMst.RenkeiCd1 = inputData.RenkeiCd1 ?? string.Empty;
                            userMst.IsDeleted = inputData.IsDeleted;
                            userMst.UpdateId = userId;
                            userMst.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            userMst.HashPassword = hashPassword;
                            userMst.Salt = salt;
                        }
                        else
                        {
                            var user = ConvertUserList(inputData);
                            user.HashPassword = hashPassword;
                            user.Salt = salt;
                            TrackingDataContext.UserMsts.Add(user);
                        }
                    }
                }
                TrackingDataContext.SaveChanges();

                // delete cache when save userMstList
                string finalKey = key + CacheKeyConstant.UserInfoCacheService;
                if (_cache.KeyExists(finalKey))
                {
                    _cache.KeyDelete(finalKey);
                }

                return true;
            }
            catch (Exception ex)
            {
                var error = ex.InnerException?.Message ?? string.Empty;
                if (error.Contains("23505: duplicate key value violates unique constraint \"IX_USER_MST_USER_ID\""))
                {
                    return false;
                }
                throw;
            }
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
                string.Empty,
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
                string.Empty,
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
                //LoginPass = u.LoginPass ?? string.Empty,
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
            var userMsts = NoTrackingDataContext.UserMsts.Where(u => u.LoginId == userName).ToList();
            bool result = false;
            foreach (UserMst userMst in userMsts)
            {
                var bytePassword = Encoding.UTF8.GetBytes(password);
                var hashPassword = CreateHash(bytePassword, userMst.Salt ?? new byte[0]);
                if (hashPassword == userMst.HashPassword)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public bool MigrateDatabase()
        {
            TrackingDataContext.Database.Migrate();
            return true;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
            _userInfoService.DisposeSource();
        }

        public bool CheckLockMedicalExamination(int hpId, long ptId, long raiinNo, int sinDate, int userId)
        {
            // Check lockMedicalExamination
            var raiinInfo = NoTrackingDataContext.RaiinInfs.FirstOrDefault(p => p.HpId == hpId && p.PtId == ptId && p.SinDate == sinDate && p.RaiinNo == raiinNo && p.IsDeleted == DeleteTypes.None);
            long oyaRaiinNo = raiinInfo != null ? raiinInfo.OyaRaiinNo : 0;
            var result = CheckLockInfo(hpId, ptId, FunctionCode.MedicalExaminationCode, raiinNo, oyaRaiinNo, sinDate, userId);
            return GetPermissionByScreenCode(hpId, userId, FunctionCode.MedicalExaminationCode) == PermissionType.Unlimited && !(result != null && result.Item2?.LockLevel == 0);
        }

        public bool NotAllowSaveMedicalExamination(int hpId, long ptId, long raiinNo, int sinDate, int userId)
        {
            var userLock = NoTrackingDataContext.LockInfs.FirstOrDefault(item => item.HpId == hpId
                                                                                 && item.PtId == ptId
                                                                                 && (item.FunctionCd == FunctionCode.MedicalExaminationCode
                                                                                    || item.FunctionCd == FunctionCode.SwitchOrderCode)
                                                                                 && item.RaiinNo == raiinNo);
            var allow = userLock == null || userLock.UserId == userId;
            return !allow;
        }

        private Tuple<LockInf, LockMst, UserMst, FunctionMst>? CheckLockInfo(int hpID, long ptID_B, string functionCD_B, long raiinNo_B, long oyaRaiinNo_B, int sinDate_B, int currentUserID)
        {
            var listCheckedResult =
                (
                    from lockInf in NoTrackingDataContext.LockInfs.Where(i => i.HpId == hpID && i.PtId == ptID_B && i.UserId != currentUserID)
                    join raiinInf in NoTrackingDataContext.RaiinInfs.Where(r => r.HpId == hpID)
                    on lockInf.RaiinNo equals raiinInf.RaiinNo into rfg
                    from lockedRaiinInf in rfg.DefaultIfEmpty()
                    join lockMst in NoTrackingDataContext.LockMsts.Where(m => m.FunctionCdB == functionCD_B && m.IsInvalid == 0)
                    on lockInf.FunctionCd equals lockMst.FunctionCdA
                    join userMst in NoTrackingDataContext.UserMsts.Where(u => u.HpId == hpID && u.IsDeleted != 1 && u.StartDate <= sinDate_B && sinDate_B <= u.EndDate)
                    on lockInf.UserId equals userMst.UserId into gj
                    from lockedUserInf in gj.DefaultIfEmpty()
                    join functionMst in NoTrackingDataContext.FunctionMsts.Where(f => f.HpId == hpID)
                    on lockInf.FunctionCd equals functionMst.FunctionCd
                    where (lockMst.FunctionCdA != lockMst.FunctionCdB) || (lockMst.FunctionCdA == lockMst.FunctionCdB && lockInf.UserId != currentUserID)
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

        public List<UserPermissionModel> GetAllPermission(int hpId, int userId)
        {
            var listUserPermission = NoTrackingDataContext.UserPermissions.Where(u => u.HpId == hpId && u.UserId == userId).AsEnumerable().Select(u => new UserPermissionModel(u.HpId, u.UserId, u.FunctionCd, u.Permission, false));

            var listUserPermissionOfUserDefault = NoTrackingDataContext.UserPermissions.Where(u => u.HpId == hpId && u.UserId == 0).AsEnumerable().Select(u => new UserPermissionModel(u.HpId, u.UserId, u.FunctionCd, u.Permission, true));

            return listUserPermission.Union(listUserPermissionOfUserDefault).ToList();
        }

        public List<JobMstModel> GetListJob(int hpId)
        {
            return NoTrackingDataContext.JobMsts.Where(j => j.HpId == hpId && !string.IsNullOrEmpty(j.JobName)).Select(x => new JobMstModel(x.HpId, x.JobCd, x.JobName ?? string.Empty, x.SortNo)).ToList();
        }

        public PermissionType GetPermissionByScreenCode(int hpId, int userId, string permisionCode)
        {
            var listUserPermission = NoTrackingDataContext.UserPermissions.Where(u => u.HpId == hpId && u.UserId == userId).ToList();
            var listUserPermissionOfUserDefault = NoTrackingDataContext.UserPermissions.Where(u => u.HpId == hpId && u.UserId == 0).ToList();

            // get data from UserMstList
            var isDoctor = _userInfoService.AllUserMstList().FirstOrDefault(u => u.UserId == userId && u.HpId == hpId && u.IsDeleted == DeleteTypes.None)?.JobCd == 1;
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

        public List<UserMstModel> GetUsersByCurrentUser(int hpId, int currentUser)
        {
            // get data from UserMstList
            var infoCurrent = _userInfoService.AllUserMstList().FirstOrDefault(u => u.UserId == currentUser);
            if (infoCurrent is null) return new List<UserMstModel>();

            IQueryable<UserMst> listUsers = NoTrackingDataContext.UserMsts.Where(u => u.HpId == hpId &&
                                                                            u.IsDeleted != 1 &&
                                                                            u.ManagerKbn <= infoCurrent.ManagerKbn);

            IQueryable<UserPermission> listUserPermission = NoTrackingDataContext.UserPermissions.Where(u => u.HpId == hpId);

            var queryFinal = (from user in listUsers
                              join userPermission in listUserPermission on user.UserId equals userPermission.UserId into listUserPer
                              select new
                              {
                                  User = user,
                                  Permissions = listUserPer
                              }).ToList();

            return queryFinal.Select(x => new UserMstModel(x.User.HpId,
                                                          x.User.Id,
                                                          x.User.UserId,
                                                          x.User.JobCd,
                                                          x.User.ManagerKbn,
                                                          x.User.KaId,
                                                          x.User.Sname ?? string.Empty,
                                                          x.User.KanaName ?? string.Empty,
                                                          x.User.Name ?? string.Empty,
                                                          x.User.Sname ?? string.Empty,
                                                          x.User.LoginId ?? string.Empty,
                                                          string.Empty,
                                                          x.User.MayakuLicenseNo ?? string.Empty,
                                                          x.User.StartDate,
                                                          x.User.EndDate,
                                                          x.User.SortNo,
                                                          x.User.IsDeleted,
                                                          x.User.RenkeiCd1 ?? string.Empty,
                                                          x.User.DrName ?? string.Empty,
                                                          x.Permissions.Select(p => new UserPermissionModel(p.HpId, p.UserId, p.FunctionCd, p.Permission, false)).ToList()))
                                                     .OrderBy(item => item.SortNo).ToList();
        }

        public List<UserMstModel> GetUsersByPermission(int hpId, int managerKbn)
        {

            List<UserMstModel> result = new List<UserMstModel>();
            var listUsers = NoTrackingDataContext.UserMsts.Where(u => u.HpId == Session.HospitalID &&
                                                                        u.IsDeleted != 1 &&
                                                                        u.ManagerKbn <= managerKbn);
            var listUserPermission = NoTrackingDataContext.UserPermissions.Where(u => u.HpId == hpId);
            var listFuncMst = NoTrackingDataContext.FunctionMsts.Where(u => u != null && u.HpId == hpId);
            var listPerMst = NoTrackingDataContext.PermissionMsts.Where(u => u != null && u.HpId == hpId);

            var functionMstQuery = from funcMst in listFuncMst
                                   join perMst in listPerMst on funcMst.FunctionCd equals perMst.FunctionCd into listPermission
                                   select new
                                   {
                                       FuncMst = funcMst,
                                       ListPermission = listPermission,
                                   };
            var listFunction = functionMstQuery.Where(item => item.ListPermission.Any()).ToList();

            var queryFinal = from user in listUsers
                             join userPermission in listUserPermission on user.UserId equals userPermission.UserId into listUserPer
                             select new
                             {
                                 User = user,
                                 Permission = listUserPer.Select(p => new UserPermissionModel(p.HpId, p.UserId, p.FunctionCd, p.Permission, false))
                             };

            var entityList = queryFinal.OrderBy(item => item.User.SortNo).ToList();
            foreach (var entity in entityList)
            {
                var functionMsts = listFunction.Select(item => new FunctionMstModel(item.FuncMst.FunctionCd, item.FuncMst.FunctionName ?? string.Empty
                                                                                , entity.User.JobCd
                                                                                , item.ListPermission.Select(p => new PermissionMstModel(p.FunctionCd, p.Permission)).ToList()
                                                                                , entity.Permission.FirstOrDefault(i => i.FunctionCd == item.FuncMst.FunctionCd) ?? new UserPermissionModel(entity.User.UserId)
                                                                                )).ToList();
                UserMstModel newModel = new UserMstModel(entity.User.HpId,
                                                          entity.User.Id,
                                                          entity.User.UserId,
                                                          entity.User.JobCd,
                                                          entity.User.ManagerKbn,
                                                          entity.User.KaId,
                                                          entity.User.Sname ?? string.Empty,
                                                          entity.User.KanaName ?? string.Empty,
                                                          entity.User.Name ?? string.Empty,
                                                          entity.User.Sname ?? string.Empty,
                                                          entity.User.LoginId ?? string.Empty,
                                                          string.Empty,
                                                          entity.User.MayakuLicenseNo ?? string.Empty,
                                                          entity.User.StartDate,
                                                          entity.User.EndDate,
                                                          entity.User.SortNo,
                                                          entity.User.IsDeleted,
                                                          entity.User.RenkeiCd1 ?? string.Empty,
                                                          entity.User.DrName ?? string.Empty,
                                                          functionMsts);

                result.Add(newModel);
            }
            if (result.Count == 0)
            {
                return new List<UserMstModel>();
            }

            return result;
        }

        /// <summary>
        /// only pass users need save.
        /// </summary>
        /// <param name="users"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public bool SaveListUserMst(int hpId, List<UserMstModel> users, int currentUser)
        {
            IEnumerable<long> idList = users.Select(x => x.Id);
            var usersUpdate = TrackingDataContext.UserMsts.Where(x => idList.Contains(x.Id)).ToList();
            foreach (var item in users)
            {
                var update = usersUpdate.FirstOrDefault(x => x.Id == item.Id);
                byte[] salt = GenerateSalt();
                byte[] hashPassword = CreateHash(Encoding.UTF8.GetBytes(item.LoginPass ?? string.Empty), salt);
                if (update is null)
                {
                    if (item.Id == 0)
                    {
                        TrackingDataContext.UserMsts.Add(new UserMst()
                        {
                            Id = item.Id,
                            CreateDate = CIUtil.GetJapanDateTimeNow(),
                            CreateId = currentUser,
                            DrName = item.DrName,
                            EndDate = item.EndDate,
                            HpId = hpId,
                            IsDeleted = DeleteTypes.None,
                            JobCd = item.JobCd,
                            KaId = item.KaId,
                            KanaName = item.KanaName,
                            LoginId = item.LoginId,
                            //LoginPass = item.LoginPass,
                            ManagerKbn = item.ManagerKbn,
                            Name = item.Name,
                            RenkeiCd1 = item.RenkeiCd1,
                            MayakuLicenseNo = item.MayakuLicenseNo,
                            Sname = item.Sname,
                            SortNo = item.SortNo,
                            StartDate = item.StartDate,
                            UpdateDate = CIUtil.GetJapanDateTimeNow(),
                            UserId = item.UserId,
                            UpdateId = currentUser,
                            HashPassword = hashPassword,
                            Salt = salt
                        });

                        TrackingDataContext.UserPermissions.AddRange(item.Permissions.Select(x => new UserPermission()
                        {
                            HpId = hpId,
                            CreateDate = CIUtil.GetJapanDateTimeNow(),
                            CreateId = currentUser,
                            FunctionCd = x.FunctionCd,
                            Permission = x.Permission,
                            UpdateId = currentUser,
                            UserId = item.UserId,
                            UpdateDate = CIUtil.GetJapanDateTimeNow()
                        }));
                    }
                }
                else
                {
                    if (item.IsDeleted != 1)
                    {
                        update.DrName = item.DrName;
                        update.EndDate = item.EndDate;
                        update.HpId = hpId;
                        update.JobCd = item.JobCd;
                        update.KaId = item.KaId;
                        update.KanaName = item.KanaName;
                        update.LoginId = item.LoginId;
                        //update.LoginPass = item.LoginPass;
                        update.ManagerKbn = item.ManagerKbn;
                        update.Name = item.Name;
                        update.RenkeiCd1 = item.RenkeiCd1;
                        update.MayakuLicenseNo = item.MayakuLicenseNo;
                        update.Sname = item.Sname;
                        update.SortNo = item.SortNo;
                        update.StartDate = item.StartDate;
                        update.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        update.UpdateId = currentUser;
                        update.HashPassword = hashPassword;
                        update.Salt = salt;

                        var permissionByUsers = TrackingDataContext.UserPermissions.Where(x => x.HpId == hpId && x.UserId == update.UserId);
                        foreach (var permission in item.Permissions)
                        {
                            var updateP = permissionByUsers.FirstOrDefault(x => x.FunctionCd == permission.FunctionCd);
                            if (updateP is null)
                            {
                                TrackingDataContext.UserPermissions.Add(new UserPermission()
                                {
                                    HpId = hpId,
                                    CreateDate = CIUtil.GetJapanDateTimeNow(),
                                    CreateId = currentUser,
                                    FunctionCd = permission.FunctionCd,
                                    Permission = permission.Permission,
                                    UpdateId = currentUser,
                                    UserId = update.UserId,
                                    UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                });
                            }
                            else
                            {
                                updateP.Permission = permission.Permission;
                                updateP.UpdateId = currentUser;
                                updateP.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            }
                        }
                    }
                    else
                    {
                        update.IsDeleted = item.IsDeleted;
                        update.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        update.UpdateId = currentUser;
                    }

                }
            }

            // delete cache when save userMstList
            string finalKey = key + CacheKeyConstant.UserInfoCacheService;
            if (_cache.KeyExists(finalKey))
            {
                _cache.KeyDelete(finalKey);
            }

            return TrackingDataContext.SaveChanges() > 0;
        }

        public bool UserIdIsExistInDb(int userId)
        {
            // get data from UserMstList
            return _userInfoService.AllUserMstList().Any(x => x.UserId == userId);
        }

        public List<int> ListJobCdValid(int hpId)
        {
            return NoTrackingDataContext.JobMsts.Where(x => x.HpId == hpId).Select(x => x.JobCd).ToList();
        }

        public List<int> ListDepartmentValid(int hpId)
        {
            return NoTrackingDataContext.KaMsts.Where(x => x.HpId == hpId && x.IsDeleted == DeleteTypes.None).Select(x => x.KaId).ToList();
        }

        public bool GetShowRenkeiCd1ColumnSetting()
        {
            var renkeiMst = NoTrackingDataContext.RenkeiMsts.FirstOrDefault(u => u.RenkeiId == 2016);
            return renkeiMst != null && renkeiMst.IsInvalid == 0;
        }

        public List<FunctionMstModel> GetListFunctionPermission(int hpId)
        {
            IQueryable<FunctionMst> listFuncMst = NoTrackingDataContext.FunctionMsts.Where(f => f.HpId == hpId);
            IQueryable<PermissionMst> listPerMst = NoTrackingDataContext.PermissionMsts.Where(item => item.HpId == hpId);
            var functionMstQuery = (from funcMst in listFuncMst
                                    join perMst in listPerMst on funcMst.FunctionCd equals perMst.FunctionCd into listPermission
                                    select new
                                    {
                                        FuncMst = funcMst,
                                        ListPermission = listPermission,
                                    }).ToList();

            return functionMstQuery.Where(x => x.ListPermission.Any()).Select(x => new FunctionMstModel(x.FuncMst.FunctionCd,
                                                                                                        x.FuncMst.FunctionName ?? string.Empty,
                                                                                                        x.ListPermission.Select(p => new PermissionMstModel(p.FunctionCd, p.Permission)).ToList())).ToList();
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



        public UserMstModel GetUserInfo(int hpId, int userId)
        {
            // get data from UserMstList
            var user = _userInfoService.AllUserMstList()
                .FirstOrDefault(x => x.HpId == hpId &&
                                     x.UserId == userId);

            return ToModel(user ?? new());
        }

        public void UpdateHashPassword()
        {
            //var users = TrackingDataContext.UserMsts.ToList();
            //foreach (var user in users)
            //{
            //    byte[] salt = GenerateSalt();
            //    byte[] hashPassword = CreateHash(Encoding.UTF8.GetBytes(user.LoginPass ?? string.Empty), salt);
            //    user.HashPassword = hashPassword;
            //    user.Salt = salt;
            //}
            //TrackingDataContext.SaveChanges();
        }

        public byte[] CreateHash(byte[] password, byte[] salt)
        {
            using var argon2 = new Argon2id(password);
            // Todo fix get Pepper configuration on staging
            var preper = "Pepper";
            //var preper = _configuration["Pepper"] ?? string.Empty;
            salt = salt.Union(Encoding.UTF8.GetBytes(preper)).ToArray();
            argon2.Salt = salt;
            argon2.DegreeOfParallelism = 8;
            argon2.Iterations = 4;
            argon2.MemorySize = 1024 * 128;
            return argon2.GetBytes(32);
        }

        public byte[] GenerateSalt()
        {
            var buffer = new byte[32];
            using var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buffer);
            return buffer;
        }

        public bool VerifyHash(byte[] password, byte[] salt, byte[] hash)
        {
            var inputHash = CreateHash(password, salt);
            return Encoding.UTF8.GetString(inputHash) == Encoding.UTF8.GetString(hash);
        }
    }
}
