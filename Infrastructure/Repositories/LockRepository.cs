using Domain.Constant;
using Domain.Models.AuditLog;
using Domain.Models.Lock;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Infrastructure.Repositories
{
    public class LockRepository : RepositoryBase, ILockRepository
    {
        private readonly IAuditLogRepository _auditLogRepository;

        public LockRepository(ITenantProvider tenantProvider, IAuditLogRepository auditLogRepository) : base(tenantProvider)
        {
            _auditLogRepository = auditLogRepository;
        }

        public bool AddLock(int hpId, string functionCd, long ptId, int sinDate, long raiinNo, int userId, string tabKey, string loginKey)
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
            "INSERT INTO \"LOCK_INF\" (\"FUNCTION_CD\", \"HP_ID\", \"OYA_RAIIN_NO\", \"PT_ID\", \"RAIIN_NO\", \"SIN_DATE\", \"LOCK_DATE\", \"MACHINE\", \"USER_ID\", \"LOGINKEY\")\r\n      " +
            $"VALUES ('{functionCd}', {hpId}, {oyaRaiinNo}, {ptId}, {raiinNo}, {sinDate}, '{lockDate}', '{tabKey}', {userId}, '{loginKey}') ON CONFLICT DO NOTHING;";

            return TrackingDataContext.Database.ExecuteSqlRaw(rawSql) > 0;
        }

        public LockModel CheckOpenSpecialNote(int hpId, string functionCd, long ptId)
        {
            var lockInf = NoTrackingDataContext.LockInfs.FirstOrDefault(item => item.HpId == hpId
                                                                                && item.PtId == ptId
                                                                                && item.FunctionCd == functionCd);
            if (lockInf == null)
            {
                return new();
            }

            var userInf = NoTrackingDataContext.UserMsts.FirstOrDefault(item => item.HpId == hpId
                                                                                && item.UserId == lockInf.UserId
                                                                                && item.IsDeleted == 0);

            var functionInf = NoTrackingDataContext.FunctionMsts.FirstOrDefault(item => item.FunctionCd == lockInf.FunctionCd);

            return new LockModel(
                       lockInf.UserId,
                       userInf?.Name ?? string.Empty,
                       lockInf.LockDate,
                       functionInf?.FunctionName ?? string.Empty,
                       lockInf.FunctionCd,
                       0,
                       0,
                       lockInf.Machine ?? string.Empty
                );
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
                    join userMst in NoTrackingDataContext.UserMsts.Where(u => u.HpId == hpId)
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
                else if (functionCd == FunctionCode.Accounting && checkedResult.lockInf.OyaRaiinNo != oyaRaiinNo)
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
                    checkedResult.lockMst.LockRange,
                    checkedResult.lockInf.Machine ?? string.Empty));
            }

            return result;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        public (List<long> raiinList, int removedCount) RemoveLock(int hpId, string functionCd, long ptId, int sinDate, long raiinNo, int userId, string tabKey)
        {
            var lockInf = TrackingDataContext.LockInfs.FirstOrDefault(r => r.HpId == hpId && r.PtId == ptId && r.FunctionCd == functionCd && r.RaiinNo == raiinNo && r.SinDate == sinDate && r.UserId == userId && r.Machine == tabKey);
            if (lockInf == null)
            {
                // if the key does not exist in the DB, wait 500ms and then try again to remove the key.
                Thread.Sleep(500);
                lockInf = TrackingDataContext.LockInfs.FirstOrDefault(r => r.HpId == hpId && r.PtId == ptId && r.FunctionCd == functionCd && r.RaiinNo == raiinNo && r.SinDate == sinDate && r.UserId == userId && r.Machine == tabKey);
                if (lockInf == null)
                {
                    return (new() { raiinNo }, 0);
                }
            }
            TrackingDataContext.LockInfs.Remove(lockInf);
            var removedCount = TrackingDataContext.SaveChanges();
            return (new() { raiinNo }, removedCount);
        }

        public List<long> RemoveAllLock(int hpId, int userId)
        {
            var lockInfList = TrackingDataContext.LockInfs.Where(r => r.HpId == hpId && r.UserId == userId).ToList();
            if (!lockInfList.Any())
            {
                return new();
            }
            var raiinNoList = lockInfList.Select(item => item.RaiinNo).Distinct().ToList();
            TrackingDataContext.LockInfs.RemoveRange(lockInfList);
            TrackingDataContext.SaveChanges();
            return raiinNoList;
        }

        public (List<long> raiinNoList, int removedCount) RemoveAllLock(int hpId, int userId, long ptId, int sinDate, string functionCd, string tabKey)
        {
            var stopwatch = Stopwatch.StartNew();
            Console.WriteLine("Start Remove All Lock Follow PtId");

            List<string> functionCdList = new()
            {
                functionCd
            };
            if (functionCd == FunctionCode.MedicalExaminationCode)
            {
                functionCdList.Add(FunctionCode.SwitchOrderCode);
            }
            var lockInfList = TrackingDataContext.LockInfs.Where(item => item.HpId == hpId
                                                                         && item.UserId == userId
                                                                         && item.PtId == ptId
                                                                         && functionCdList.Contains(item.FunctionCd)
                                                                         && item.SinDate == sinDate
                                                                         && item.Machine == tabKey
                                                          ).ToList();
            if (!lockInfList.Any())
            {
                return new();
            }
            var raiinNoList = lockInfList.Select(item => item.RaiinNo).Distinct().ToList();
            TrackingDataContext.LockInfs.RemoveRange(lockInfList);
            var removedCount = TrackingDataContext.SaveChanges();
            Console.WriteLine("Stop Remove Lock: " + stopwatch.ElapsedMilliseconds);

            return (raiinNoList, removedCount);
        }

        public List<long> RemoveAllLock(int hpId, int userId, string loginKey)
        {
            var lockInfList = TrackingDataContext.LockInfs.Where(item => item.HpId == hpId
                                                                         && item.UserId == userId
                                                                         && item.LoginKey == loginKey)
                                                          .ToList();
            if (!lockInfList.Any())
            {
                return new();
            }
            var raiinNoList = lockInfList.Select(item => item.RaiinNo).Distinct().ToList();
            TrackingDataContext.LockInfs.RemoveRange(lockInfList);
            TrackingDataContext.SaveChanges();
            return raiinNoList;
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
                                                        x.lockMst.LockRange,
                                                        x.lockInf.Machine ?? string.Empty)).ToList();
            return result;
        }

        public List<LockModel> GetVisitingLockStatus(int hpId, int userId, long ptId, string functionCode)
        {
            List<LockInf> lockInfList;
            if (functionCode == FunctionCode.MedicalExaminationCode)
            {
                lockInfList = NoTrackingDataContext.LockInfs.Where(item => item.HpId == hpId
                                                                           && item.PtId == ptId
                                                                           && (item.FunctionCd == functionCode
                                                                               || item.FunctionCd == FunctionCode.SwitchOrderCode)
                                                                           && item.UserId == userId)
                                                            .ToList();
            }
            else
            {
                lockInfList = NoTrackingDataContext.LockInfs.Where(item => item.HpId == hpId
                                                                           && item.PtId == ptId
                                                                           && item.FunctionCd == functionCode
                                                                           && item.UserId == userId)
                                                            .ToList();
            }
            if (!lockInfList.Any())
            {
                return new();
            }
            List<LockModel> result = new();
            var functionCdList = lockInfList.Select(item => item.FunctionCd).Distinct().ToList();
            var lockMstList = NoTrackingDataContext.LockMsts.Where(item => functionCdList.Contains(item.FunctionCdB) && item.IsInvalid == 0).ToList();
            var functionMstList = NoTrackingDataContext.FunctionMsts.Where(item => functionCdList.Contains(item.FunctionCd)).ToList();
            var userMst = NoTrackingDataContext.UserMsts.FirstOrDefault(item => item.HpId == hpId && item.IsDeleted != 1 && item.UserId == userId);
            foreach (var lockInf in lockInfList)
            {
                var lockMst = lockMstList.FirstOrDefault(item => lockInf.FunctionCd == item.FunctionCdA);
                var functionMst = functionMstList.FirstOrDefault(item => item.FunctionCd == lockInf.FunctionCd);
                var lockModel = new LockModel(
                                    lockInf.UserId,
                                    userMst?.Name ?? string.Empty,
                                    lockInf.LockDate,
                                    functionMst?.FunctionName ?? string.Empty,
                                    lockInf.FunctionCd,
                                    lockMst?.LockLevel ?? 0,
                                    lockMst?.LockRange ?? 0,
                                    lockInf.Machine ?? string.Empty);
                result.Add(lockModel);
            }
            return result;
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

        public List<ResponseLockModel> GetResponseLockModel(int hpId, long ptId, int sinDate, long raiinNo)
        {
            List<ResponseLockModel> result = new();
            List<long> raiinNoList = new();
            List<RaiinInf> raiinInfList = new();

            // Raiin
            if (raiinNo == 0)
            {
                raiinInfList = NoTrackingDataContext.RaiinInfs.Where(item => item.IsDeleted == DeleteTypes.None
                                                                             && item.SinDate == sinDate
                                                                             && item.PtId == ptId)
                                                              .ToList();

                raiinNoList = raiinInfList.Select(item => item.RaiinNo).Distinct().ToList();
            }
            else
            {
                raiinNoList.Add(raiinNo);
            }

            // Lock 
            var lockInfList = NoTrackingDataContext.LockInfs.Where(item => raiinNoList.Contains(item.RaiinNo)
                                                                           && (item.FunctionCd == FunctionCode.MedicalExaminationCode
                                                                               || item.FunctionCd == FunctionCode.TeamKarte
                                                                               || item.FunctionCd == FunctionCode.SwitchOrderCode))
                                                            .ToList();
            foreach (var raiinItem in raiinInfList)
            {
                int status = raiinItem.Status;
                if (lockInfList.Any(item => item.RaiinNo == raiinItem.RaiinNo))
                {
                    status = RaiinState.Examining;
                }
                result.Add(new ResponseLockModel(
                               raiinItem.SinDate,
                               raiinItem.PtId,
                               raiinItem.RaiinNo,
                               status
                           ));
            }

            if (raiinNo != 0 && lockInfList.Any(item => item.RaiinNo == raiinNo))
            {
                result.Add(new ResponseLockModel(
                               sinDate,
                               ptId,
                               raiinNo,
                               RaiinState.Examining
                           ));
            }
            return result;
        }

        public List<ResponseLockModel> GetResponseLockModel(int hpId, List<long> raiinNoList)
        {
            List<ResponseLockModel> result = new();
            List<RaiinInf> raiinInfList;

            // Raiin
            if (raiinNoList.Any())
            {
                raiinInfList = NoTrackingDataContext.RaiinInfs.Where(item => item.HpId == hpId
                                                                             && item.IsDeleted == DeleteTypes.None
                                                                             && raiinNoList.Contains(item.RaiinNo))
                                                              .ToList();

                raiinNoList = raiinInfList.Select(item => item.RaiinNo).Distinct().ToList();
            }
            else
            {
                return result;
            }

            // Lock 
            var lockInfList = NoTrackingDataContext.LockInfs.Where(item => raiinNoList.Contains(item.RaiinNo)
                                                                           && (item.FunctionCd == FunctionCode.MedicalExaminationCode
                                                                               || item.FunctionCd == FunctionCode.TeamKarte
                                                                               || item.FunctionCd == FunctionCode.SwitchOrderCode))
                                                            .ToList();
            foreach (var raiinItem in raiinInfList)
            {
                int status = raiinItem.Status;
                if (lockInfList.Any(item => item.RaiinNo == raiinItem.RaiinNo))
                {
                    status = RaiinState.Examining;
                }
                result.Add(new ResponseLockModel(
                               raiinItem.SinDate,
                               raiinItem.PtId,
                               raiinItem.RaiinNo,
                               status
                           ));
            }
            return result;
        }

        public List<LockModel> CheckLockOpenAccounting(int hpId, long ptId, long raiinNo, int userId)
        {
            var raiinInf = NoTrackingDataContext.RaiinInfs.FirstOrDefault(item => item.HpId == hpId
                                                                                  && item.RaiinNo == raiinNo
                                                                                  && item.IsDeleted == 0);
            if (raiinInf == null)
            {
                return new();
            }
            else if (raiinInf.Status == 9)
            {
                return new();
            }
            long oyaRaiinNo = raiinInf.OyaRaiinNo;
            var lockInfList = NoTrackingDataContext.LockInfs.Where(item => item.HpId == hpId
                                                                           && item.PtId == ptId
                                                                           && item.UserId == userId
                                                                           && item.OyaRaiinNo == oyaRaiinNo)
                                                            .ToList();

            List<LockModel> result = new();
            var functionCdList = lockInfList.Select(item => item.FunctionCd).Distinct().ToList();
            var lockMstList = NoTrackingDataContext.LockMsts.Where(item => functionCdList.Contains(item.FunctionCdB) && item.IsInvalid == 0).ToList();
            var functionMstList = NoTrackingDataContext.FunctionMsts.Where(item => functionCdList.Contains(item.FunctionCd)).ToList();
            var userMst = NoTrackingDataContext.UserMsts.FirstOrDefault(item => item.HpId == hpId && item.IsDeleted != 1 && item.UserId == userId);
            foreach (var lockInf in lockInfList)
            {
                var lockMst = lockMstList.FirstOrDefault(item => lockInf.FunctionCd == item.FunctionCdA);
                var functionMst = functionMstList.FirstOrDefault(item => item.FunctionCd == lockInf.FunctionCd);
                var lockModel = new LockModel(
                                    lockInf.UserId,
                                    userMst?.Name ?? string.Empty,
                                    lockInf.LockDate,
                                    functionMst?.FunctionName ?? string.Empty,
                                    lockInf.FunctionCd,
                                    lockMst?.LockLevel ?? 0,
                                    lockMst?.LockRange ?? 0,
                                    lockInf.Machine ?? string.Empty);
                result.Add(lockModel);
            }
            return result;
        }

        public List<LockInfModel> GetLockInfModels(int hpId, int userId, int managerKbn)
        {
            List<LockInfModel> result = new List<LockInfModel>();
            var listLock = NoTrackingDataContext.LockInfs.Where(u => u.HpId == hpId && !string.IsNullOrEmpty(u.Machine));

            if (managerKbn == 0)
            {
                listLock = listLock.Where(x => x.UserId == userId);
            }

            var listFunctMst = NoTrackingDataContext.FunctionMsts.Select(f => new { f.FunctionCd, f.FunctionName });
            var listPtInf = NoTrackingDataContext.PtInfs.Where(p => p.HpId == hpId && p.IsDelete == DeleteStatus.None).Select(pt => new { pt.PtId, pt.PtNum });
            var listCalcStatus = NoTrackingDataContext.CalcStatus.Where(cal => cal.HpId == hpId && !string.IsNullOrEmpty(cal.CreateMachine) && (cal.Status == 0 || cal.Status == 1));
            var listDocInf = NoTrackingDataContext.DocInfs.Where(d => d.HpId == hpId && d.IsLocked == 1 && !string.IsNullOrEmpty(d.LockMachine) && d.IsDeleted == DeleteStatus.None);

            var lockInfQuerry = (from lockInf in listLock
                                 join functMst in listFunctMst on lockInf.FunctionCd equals functMst.FunctionCd
                                 join ptInf in listPtInf on lockInf.PtId equals ptInf.PtId
                                 select new
                                 {
                                     LockInf = lockInf,
                                     FunctName = functMst.FunctionName,
                                     PtNum = ptInf.PtNum,
                                 }).ToList();

            var docInfQuerry = (from docInf in listDocInf
                                join ptInf in listPtInf on docInf.PtId equals ptInf.PtId
                                select new
                                {
                                    DocInf = docInf,
                                    PtNum = ptInf.PtNum,
                                }).ToList();

            var calcStatusQuerry = (from calcStatus in listCalcStatus
                                    join ptInf in listPtInf on calcStatus.PtId equals ptInf.PtId
                                    select new
                                    {
                                        CalcStatus = calcStatus,
                                        PtNum = ptInf.PtNum,
                                    }).ToList();

            result.AddRange(lockInfQuerry.AsEnumerable().Select(l => new LockInfModel(
                new LockPtInfModel(l.LockInf.PtId, l.FunctName, l.PtNum, l.LockInf.SinDate, l.LockInf.LockDate, l.LockInf.Machine ?? string.Empty, l.LockInf.FunctionCd, l.LockInf.RaiinNo, l.LockInf.OyaRaiinNo, l.LockInf.UserId)
            )).ToList());

            result.AddRange(docInfQuerry.AsEnumerable().Select(doc => new LockInfModel(
                new LockDocInfModel(doc.DocInf.PtId, doc.PtNum, doc.DocInf.SinDate, doc.DocInf.RaiinNo, doc.DocInf.SeqNo, doc.DocInf.CategoryCd, doc.DocInf.FileName ?? string.Empty, doc.DocInf.DspFileName ?? string.Empty, doc.DocInf.IsLocked, (DateTime)doc.DocInf.LockDate, doc.DocInf.LockId, doc.DocInf.LockMachine ?? string.Empty, doc.DocInf.IsDeleted)
            )).ToList());

            result.AddRange(calcStatusQuerry.AsEnumerable().Select(cal => new LockInfModel(
                new LockCalcStatusModel(cal.CalcStatus.CalcId, cal.CalcStatus.PtId, cal.PtNum, cal.CalcStatus.SinDate, cal.CalcStatus.CreateDate, cal.CalcStatus.CreateMachine ?? string.Empty, cal.CalcStatus.CreateId)
            )));

            return result;

        }

        public Dictionary<int, Dictionary<int, string>> GetLockInf(int hpId)
        {
            Dictionary<int, Dictionary<int, string>> result = new();
            var userIdLockInfs = NoTrackingDataContext.LockInfs.Where(x => x.HpId == hpId).Select(x => x.UserId).Distinct().ToList();
            var userMsts = NoTrackingDataContext.UserMsts.Where(x => x.HpId == hpId).ToList();
            var index = 0;
            var query = (from userIdLockInf in userIdLockInfs
                         join userMst in userMsts
                         on userIdLockInf equals userMst.UserId
                         select new
                         {
                             userIdLockInf,
                             userMst.Name
                         }).OrderBy(x => x.userIdLockInf);

            foreach (var item in query.AsEnumerable().ToList())
            {
                Dictionary<int, string> data = new()
                {
                    { item.userIdLockInf, item.Name }
                };
                result.Add(index, data);
                index++;
            }

            return result;
        }

        public bool Unlock(int hpId, int userId, List<LockInfModel> lockInfModels, int managerKbn)
        {
            bool result = true;

            try
            {
                List<string> listMachineLock = lockInfModels.Where(u => !string.IsNullOrEmpty(u.Machine)).Select(u => u.Machine).GroupBy(u => u).Select(u => u.First()).ToList();
                List<LockPtInfModel> listLockPtInfModel = lockInfModels.Where(u => u.PatientInfoModels != null && !u.CheckDefaultValue()).Select(u => u.PatientInfoModels).ToList();
                List<LockCalcStatusModel> listLockCalcStatusModel = lockInfModels.Where(u => u.CalcStatusModels != null && !u.CheckDefaultValue()).Select(u => u.CalcStatusModels).ToList();
                List<LockDocInfModel> listLockDocInfModel = lockInfModels.Where(u => u.DocInfModels != null && !u.CheckDefaultValue()).Select(u => u.DocInfModels).ToList();
                UnlockSessionInf(hpId, listMachineLock);
                UnlockPtInf(hpId, userId, listLockPtInfModel);
                UnlockCalcStatusInf(hpId, userId, listLockCalcStatusModel);
                UnlockDocInf(hpId, userId, listLockDocInfModel);

                if (TrackingDataContext.SaveChanges() >= 1)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public void UnlockSessionInf(int hpId, List<string> listMachineToUnlock)
        {
            var listSessionToUnlock = TrackingDataContext.SessionInfs.Where(u => u.HpId == hpId && listMachineToUnlock.Contains(u.Machine)).ToList();
            TrackingDataContext.SessionInfs.RemoveRange(listSessionToUnlock);
        }

        private void UnlockPtInf(int hpId, int userId, List<LockPtInfModel> listLockPtInfModel)
        {
            foreach (var item in listLockPtInfModel)
            {


                _auditLogRepository.SaveAuditLog(hpId, userId, new AuditTrailLogModel(0, CIUtil.GetJapanDateTimeNow(), hpId, userId, "99999000001", item.PtId, item.SinDateInt, item.RaiinNo, "", "LOCK_INF:" + item.FunctionName));

                var lockInf = TrackingDataContext.LockInfs.Where(x =>
                                                                   x.HpId == hpId &&
                                                                   x.PtId == item.PtId &&
                                                                   x.FunctionCd == item.FunctionCd &&
                                                                   x.SinDate == item.SinDate &&
                                                                   x.RaiinNo == item.RaiinNo &&
                                                                   x.OyaRaiinNo == item.OyaRaiinNo
                                                                   ).ToList();

                TrackingDataContext.LockInfs.RemoveRange(lockInf);
            }
        }

        private void UnlockCalcStatusInf(int hpId, int userId, List<LockCalcStatusModel> listLockCalcStatusModels)
        {
            foreach (var listLockCalcStatusModel in listLockCalcStatusModels)
            {
                var calcStatus = NoTrackingDataContext.CalcStatus.FirstOrDefault(x => x.HpId == hpId && x.PtId == listLockCalcStatusModel.PtId);
                if (calcStatus != null)
                {
                    calcStatus.CreateMachine = string.Empty;
                    calcStatus.Status = 8;
                    calcStatus.UpdateId = userId;
                    calcStatus.UpdateDate = CIUtil.GetJapanDateTimeNow();
                }

                _auditLogRepository.SaveAuditLog(hpId, userId, new AuditTrailLogModel(0, CIUtil.GetJapanDateTimeNow(), hpId, userId, "99999000001", listLockCalcStatusModel.PtId, listLockCalcStatusModel.SinDate, 0, "", "CALC_STATUS:" + listLockCalcStatusModel.CalcId));
            }
        }

        private void UnlockDocInf(int hpId, int userId, List<LockDocInfModel> listLockDocInfModel)
        {
            foreach (var docInfModel in listLockDocInfModel)
            {
                var docInf = TrackingDataContext.DocInfs.FirstOrDefault(x => x.HpId == hpId && x.PtId == docInfModel.PtId && x.SinDate == docInfModel.SinDate && x.RaiinNo == docInfModel.RaiinNo && x.SeqNo == docInfModel.SeqNo);
                if (docInf != null)
                {
                    docInf.LockMachine = string.Empty;
                    docInf.IsLocked = 0;
                    docInf.UpdateId = userId;
                    docInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                }

                _auditLogRepository.SaveAuditLog(hpId, userId, new AuditTrailLogModel(0, CIUtil.GetJapanDateTimeNow(), hpId, userId, "99999000001", docInfModel.PtId, docInfModel.SinDate, 0, "", "DOC_INF:" + docInfModel.DspFileName));
            }
        }
    }
}
