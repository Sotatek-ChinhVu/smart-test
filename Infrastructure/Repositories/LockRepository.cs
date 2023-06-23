using Domain.Models.Lock;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class LockRepository : RepositoryBase, ILockRepository
    {
        public LockRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public bool AddLock(int hpId, string functionCd, long ptId, int sinDate, long raiinNo, int userId, string token)
        {
            long oyaRaiinNo = 0;
            if (raiinNo > 0)
            {
                var raiinInf = TrackingDataContext.RaiinInfs.FirstOrDefault(r => r.HpId == hpId && r.PtId == ptId && r.RaiinNo == raiinNo && r.SinDate == sinDate);
                if (raiinInf != null)
                {
                    oyaRaiinNo = raiinInf.OyaRaiinNo;
                }
            }

            var lockInf = TrackingDataContext.LockInfs.FirstOrDefault(r => r.HpId == hpId && r.PtId == ptId && r.FunctionCd == functionCd && r.RaiinNo == raiinNo && r.OyaRaiinNo == oyaRaiinNo && r.SinDate == sinDate);

            if (lockInf != null)
            {
                return false;
            }

            string lockDate = CIUtil.GetJapanDateTimeNow().ToString("yyyy-MM-dd HH:mm:ss.fff");

            string rawSql =
            "INSERT INTO \"LOCK_INF\" (\"FUNCTION_CD\", \"HP_ID\", \"OYA_RAIIN_NO\", \"PT_ID\", \"RAIIN_NO\", \"SIN_DATE\", \"LOCK_DATE\", \"MACHINE\", \"USER_ID\")\r\n      " +
            $"VALUES ('{functionCd}', {hpId}, {oyaRaiinNo}, {ptId}, {raiinNo}, {sinDate}, '{lockDate}', '{token}', {userId}) ON CONFLICT DO NOTHING;";

            return TrackingDataContext.Database.ExecuteSqlRaw(rawSql) > 0;
        }

        public bool ExistLock(int hpId, string functionCd, long ptId, int sinDate, long raiinNo)
        {
            long oyaRaiinNo = 0;
            if (raiinNo > 0)
            {
                var raiinInf = NoTrackingDataContext.RaiinInfs.FirstOrDefault(r => r.HpId == hpId && r.PtId == ptId && r.RaiinNo == raiinNo && r.SinDate == sinDate);
                if (raiinInf != null)
                {
                    oyaRaiinNo = raiinInf.OyaRaiinNo;
                }
            }
            return NoTrackingDataContext.LockInfs.Any(r => r.HpId == hpId && r.PtId == ptId && r.FunctionCd == functionCd && r.RaiinNo == raiinNo && r.OyaRaiinNo == oyaRaiinNo && r.SinDate == sinDate);
        }

        public List<LockModel> GetLock(int hpId, string functionCd, long ptId, int sinDate, long raiinNo, int userId)
        {
            long oyaRaiinNo = 0;
            if (raiinNo > 0)
            {
                var raiinInf = NoTrackingDataContext.RaiinInfs.FirstOrDefault(r => r.HpId == hpId && r.PtId == ptId && r.RaiinNo == raiinNo && r.SinDate == sinDate);
                if (raiinInf != null)
                {
                    oyaRaiinNo = raiinInf.OyaRaiinNo;
                }
            }

            var listCheckedResult =
            (
                    from lockInf in NoTrackingDataContext.LockInfs.Where(i => i.HpId == hpId && i.PtId == ptId)
                    join raiinInf in NoTrackingDataContext.RaiinInfs.Where(r => r.HpId == hpId)
                    on lockInf.RaiinNo equals raiinInf.RaiinNo into rfg
                    from lockedRaiinInf in rfg.DefaultIfEmpty()
                    join lockMst in NoTrackingDataContext.LockMsts.Where(m => m.FunctionCdB == functionCd && m.IsInvalid == 0)
                    on lockInf.FunctionCd equals lockMst.FunctionCdA
                    join userMst in NoTrackingDataContext.UserMsts.Where(u => u.HpId == hpId && u.IsDeleted != 1 && u.StartDate <= sinDate && sinDate <= u.EndDate)
                    on lockInf.UserId equals userMst.UserId into gj
                    from lockedUserInf in gj.DefaultIfEmpty()
                    join functionMst in NoTrackingDataContext.FunctionMsts
                    on lockInf.FunctionCd equals functionMst.FunctionCd
                    where (lockMst.FunctionCdA != lockMst.FunctionCdB) || (lockMst.FunctionCdA == lockMst.FunctionCdB && lockInf.UserId != userId)
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

            if (listCheckedResult == null) return new List<LockModel>();

            var result = new List<LockModel>();
            for (int i = 0; i < listCheckedResult.Count; i++)
            {
                var checkedResult = listCheckedResult[i];
                if (checkedResult.lockMst.LockRange == 1 && checkedResult.lockInf.RaiinNo != raiinNo)
                {
                    continue;
                }
                else if (checkedResult.lockMst.LockRange == 2 && checkedResult.lockedRaiinInf != null && checkedResult.lockedRaiinInf.OyaRaiinNo != oyaRaiinNo)
                {
                    continue;
                }
                else if (checkedResult.lockMst.LockRange == 3 && checkedResult.lockInf.SinDate != sinDate)
                {
                    continue;
                }
                result.Add(new LockModel(
                    checkedResult.lockInf.UserId,
                    checkedResult.lockedUserInf?.Name ?? string.Empty,
                    checkedResult.lockInf.LockDate,
                    checkedResult.functionMst.FunctionName ?? string.Empty,
                    checkedResult.lockInf.FunctionCd,
                    checkedResult.lockMst.LockLevel,
                    checkedResult.lockMst.LockRange));
            }

            return result;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        public bool RemoveLock(int hpId, string functionCd, long ptId, int sinDate, long raiinNo, int userId)
        {
            long oyaRaiinNo = 0;
            if (raiinNo > 0)
            {
                var raiinInf = NoTrackingDataContext.RaiinInfs.FirstOrDefault(r => r.HpId == hpId && r.PtId == ptId && r.RaiinNo == raiinNo && r.SinDate == sinDate);
                if (raiinInf != null)
                {
                    oyaRaiinNo = raiinInf.OyaRaiinNo;
                }
            }
            var lockInf = TrackingDataContext.LockInfs.FirstOrDefault(r => r.HpId == hpId && r.PtId == ptId && r.FunctionCd == functionCd && r.RaiinNo == raiinNo && r.OyaRaiinNo == oyaRaiinNo && r.SinDate == sinDate && r.UserId == userId);
            if (lockInf == null)
            {
                return true;
            }
            TrackingDataContext.LockInfs.Remove(lockInf);
            TrackingDataContext.SaveChanges();
            return true;
        }

        public bool RemoveAllLock(int hpId, int userId)
        {
            var lockInfList = TrackingDataContext.LockInfs.Where(r => r.HpId == hpId && r.UserId == userId).ToList();
            if (!lockInfList.Any())
            {
                return true;
            }
            TrackingDataContext.LockInfs.RemoveRange(lockInfList);
            TrackingDataContext.SaveChanges();
            return true;
        }

        public bool ExtendTtl(int hpId, string functionCd, long ptId, int sinDate, long raiinNo, int userId)
        {
            long oyaRaiinNo = 0;
            if (raiinNo > 0)
            {
                var raiinInf = NoTrackingDataContext.RaiinInfs.FirstOrDefault(r => r.HpId == hpId && r.PtId == ptId && r.RaiinNo == raiinNo && r.SinDate == sinDate);
                if (raiinInf != null)
                {
                    oyaRaiinNo = raiinInf.OyaRaiinNo;
                }
            }
            var lockInf = TrackingDataContext.LockInfs.FirstOrDefault(r => r.HpId == hpId && r.PtId == ptId && r.FunctionCd == functionCd && r.RaiinNo == raiinNo && r.OyaRaiinNo == oyaRaiinNo && r.SinDate == sinDate && r.UserId == userId);
            if (lockInf == null)
            {
                return false;
            }
            lockInf.LockDate = CIUtil.GetJapanDateTimeNow();
            TrackingDataContext.SaveChanges();
            return true;
        }

        public List<LockModel> GetLockInfo(int hpId, long ptId, List<string> lisFunctionCD_B, int sinDate_B, long raiinNo)
        {
            var query =
            (
                    from lockInf in NoTrackingDataContext.LockInfs.Where(i => i.HpId == hpId && i.PtId == ptId)
                    join lockMst in NoTrackingDataContext.LockMsts.Where(m => lisFunctionCD_B.Contains(m.FunctionCdB) && m.IsInvalid == 0)
            on lockInf.FunctionCd equals lockMst.FunctionCdA
                    join userMst in NoTrackingDataContext.UserMsts.Where(u => u.HpId == hpId && u.IsDeleted != 1 && u.StartDate <= sinDate_B && sinDate_B <= u.EndDate)
            on lockInf.UserId equals userMst.UserId
                    join functionMst in NoTrackingDataContext.FunctionMsts.AsQueryable()
                    on lockInf.FunctionCd equals functionMst.FunctionCd
                    where lockInf.RaiinNo == 0 || lockInf.RaiinNo == raiinNo
                    orderby lockMst.LockLevel, lockMst.LockRange
                    select new
                    {
                        lockInf,
                        lockMst,
                        userMst,
                        functionMst
                    }
            )
            .AsEnumerable().ToList();

            var result = query.Select(x => new LockModel(
                                                        x.lockInf.UserId,
                                                        x.userMst.Name ?? string.Empty,
                                                        x.lockInf.LockDate,
                                                        x.functionMst.FunctionName ?? string.Empty,
                                                        x.lockInf.FunctionCd,
                                                        x.lockMst.LockLevel,
                                                        x.lockMst.LockRange)).ToList();
            return result;
        }

        public bool GetVisitingLockStatus(int hpId, int userId, long ptId, string functionCode)
        {
            LockInf? log = null;
            if (functionCode == FunctionCode.MedicalExaminationCode)
            {
                log = NoTrackingDataContext.LockInfs.FirstOrDefault(item => item.HpId == hpId
                                                                            && item.PtId == ptId
                                                                            && (item.FunctionCd == functionCode
                                                                                || item.FunctionCd == FunctionCode.SwitchOrderCode)
                                                                            && item.UserId == userId);
            }
            else
            {
                log = NoTrackingDataContext.LockInfs.FirstOrDefault(item => item.HpId == hpId
                                                                            && item.PtId == ptId
                                                                            && item.FunctionCd == functionCode
                                                                            && item.UserId == userId);
            }
            return log == null;
        }

        public string GetFunctionNameLock(string functionCode)
        {
            string result = string.Empty;
            var functionItem = NoTrackingDataContext.FunctionMsts.FirstOrDefault(item => item.FunctionCd == functionCode);
            if (functionItem != null)
            {
                result = functionItem.FunctionName ?? string.Empty;
            }
            return result;
        }
    }
}
