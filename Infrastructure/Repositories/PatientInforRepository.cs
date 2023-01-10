using Domain.Models.CalculationInf;
using Domain.Models.GroupInf;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Domain.Models.InsuranceMst;
using Domain.Models.MaxMoney;
using Domain.Models.PatientInfor;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Helper.Mapping;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using PostgreDataContext;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using HokenInfModel = Domain.Models.Insurance.HokenInfModel;

namespace Infrastructure.Repositories
{
    public class PatientInforRepository : RepositoryBase, IPatientInforRepository
    {
        public PatientInforRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        (PatientInforModel, bool) IPatientInforRepository.SearchExactlyPtNum(int ptNum, int hpId)
        {
            var ptInf = NoTrackingDataContext.PtInfs.Where(x => x.PtNum == ptNum).FirstOrDefault();
            if (ptInf == null)
            {
                return (new PatientInforModel(), false);
            }

            long ptId = ptInf.PtId;

            //Get ptMemo
            string memo = string.Empty;
            PtMemo? ptMemo = NoTrackingDataContext.PtMemos.Where(x => x.PtId == ptId).FirstOrDefault();
            if (ptMemo != null)
            {
                memo = ptMemo.Memo ?? string.Empty;
            }

            int lastVisitDate = NoTrackingDataContext.RaiinInfs
                .Where(r => r.HpId == hpId && r.PtId == ptId && r.Status >= RaiinState.TempSave && r.IsDeleted == DeleteTypes.None)
                .OrderByDescending(r => r.SinDate)
                .Select(r => r.SinDate)
                .FirstOrDefault();
            PatientInforModel ptInfModel = ToModel(ptInf, memo, lastVisitDate);

            return new(ptInfModel, true);
        }

        public List<PatientInforModel> SearchContainPtNum(int ptNum, string keyword, int hpId, int pageIndex, int pageSize)
        {
            var ptInfWithLastVisitDate =
                from p in NoTrackingDataContext.PtInfs
                where p.IsDelete == 0 && (p.PtNum == ptNum || (p.KanaName != null && p.KanaName.Contains(keyword)) || (p.Name != null && p.Name.Contains(keyword)))
                select new
                {
                    ptInf = p,
                    lastVisitDate = (
                        from r in NoTrackingDataContext.RaiinInfs
                        where r.HpId == hpId
                            && r.PtId == p.PtId
                            && r.Status >= RaiinState.TempSave
                            && r.IsDeleted == DeleteTypes.None
                        orderby r.SinDate descending
                        select r.SinDate
                    ).FirstOrDefault()
                };

            return ptInfWithLastVisitDate
                         .AsEnumerable()
                         .Skip((pageIndex - 1) * pageSize)
                         .Take(pageSize)
                         .Select(p => ToModel(p.ptInf, string.Empty, p.lastVisitDate))
                         .ToList();
        }

        public PatientInforModel? GetById(int hpId, long ptId, int sinDate, int raiinNo)
        {

            var itemData = NoTrackingDataContext.PtInfs.Where(x => x.HpId == hpId && x.PtId == ptId).FirstOrDefault();


            // Raiin Count
            string raiinCountString = "";

            // status = RaiinState Receptionist
            var GetCountraiinInf = NoTrackingDataContext.RaiinInfs.Where(u => u.HpId == hpId &&
                                                                         u.SinDate == sinDate &&
                                                                         u.RaiinNo != raiinNo &&
                                                                         u.IsDeleted == DeleteTypes.None &&
                                                                         u.Status == 1).ToList();
            if (GetCountraiinInf != null && GetCountraiinInf.Count > 0)
            {
                raiinCountString = GetCountraiinInf.Count.ToString() + "人";
            }

            if (itemData == null)
            {
                return new PatientInforModel(
                    0,
                    0,
                    0,
                    0,
                    0,
                    "",
                    "",
                    0,
                    0,
                    0,
                    0,
                    0,
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    0,
                    0,
                    0,
                    0,
                    "",
                    0,
                    0,
                    raiinCountString);
            }
            else
            {

                //Get ptMemo
                string memo = string.Empty;
                PtMemo? ptMemo = NoTrackingDataContext.PtMemos.Where(x => x.PtId == itemData.PtId).FirstOrDefault();
                if (ptMemo != null)
                {
                    memo = ptMemo.Memo ?? string.Empty;
                }


                //Get lastVisitDate
                int lastVisitDate = 0;
                RaiinInf? raiinInf = NoTrackingDataContext.RaiinInfs.Where(p => p.HpId == hpId &&
                                                           p.PtId == ptId &&
                                                           p.IsDeleted == DeleteTypes.None &&
                                                           p.Status >= RaiinState.TempSave &&
                                                           (sinDate <= 0 || p.SinDate < sinDate))
                                                            .OrderByDescending(p => p.SinDate)
                                                            .ThenByDescending(p => p.RaiinNo)
                                                            .FirstOrDefault();
                if (raiinInf != null)
                {
                    lastVisitDate = raiinInf.SinDate;
                }

                //Get First Visit Date
                int firstDate = 0;
                RaiinInf? raiinInfFirstDate = NoTrackingDataContext.RaiinInfs.Where(x => x.HpId == hpId
                                                                               && x.PtId == itemData.PtId
                                                                               && x.SyosaisinKbn == SyosaiConst.Syosin
                                                                               && x.Status >= RaiinState.TempSave
                                                                               && x.IsDeleted == DeleteTypes.None
                    )
                    .OrderByDescending(x => x.SinDate)
                    .FirstOrDefault();
                if (raiinInfFirstDate != null)
                {
                    firstDate = raiinInfFirstDate.SinDate;
                }

                return new PatientInforModel(
                    itemData.HpId,
                    itemData.PtId,
                    itemData.ReferenceNo,
                    itemData.SeqNo,
                    itemData.PtNum,
                    itemData.KanaName ?? string.Empty,
                    itemData.Name ?? string.Empty,
                    itemData.Sex,
                    itemData.Birthday,
                    itemData.LimitConsFlg,
                    itemData.IsDead,
                    itemData.DeathDate,
                    itemData.HomePost ?? string.Empty,
                    itemData.HomeAddress1 ?? string.Empty,
                    itemData.HomeAddress2 ?? string.Empty,
                    itemData.Tel1 ?? string.Empty,
                    itemData.Tel2 ?? string.Empty,
                    itemData.Mail ?? string.Empty,
                    itemData.Setanusi ?? string.Empty,
                    itemData.Zokugara ?? string.Empty,
                    itemData.Job ?? string.Empty,
                    itemData.RenrakuName ?? string.Empty,
                    itemData.RenrakuPost ?? string.Empty,
                    itemData.RenrakuAddress1 ?? string.Empty,
                    itemData.RenrakuAddress2 ?? string.Empty,
                    itemData.RenrakuTel ?? string.Empty,
                    itemData.RenrakuMemo ?? string.Empty,
                    itemData.OfficeName ?? string.Empty,
                    itemData.OfficePost ?? string.Empty,
                    itemData.OfficeAddress1 ?? string.Empty,
                    itemData.OfficeAddress2 ?? string.Empty,
                    itemData.OfficeTel ?? string.Empty,
                    itemData.OfficeMemo ?? string.Empty,
                    itemData.IsRyosyoDetail,
                    itemData.PrimaryDoctor,
                    itemData.IsTester,
                    itemData.MainHokenPid,
                    memo,
                    lastVisitDate,
                    firstDate,
                    raiinCountString);
            }
        }

        public bool CheckExistListId(List<long> ptIds)
        {
            var countPtInfs = NoTrackingDataContext.PtInfs.Count(x => ptIds.Contains(x.PtId) && x.IsDelete != 1);
            return ptIds.Count == countPtInfs;
        }

        public List<PatientInforModel> SearchSimple(string keyword, bool isContainMode, int hpId)
        {
            long ptNum = keyword.AsLong();
            var ptInfWithLastVisitDate =
                from p in NoTrackingDataContext.PtInfs
                where p.IsDelete == 0 && (p.PtNum == ptNum || isContainMode && ((p.KanaName != null && p.KanaName.Contains(keyword)) || (p.Name != null && p.Name.Contains(keyword))))
                select new
                {
                    ptInf = p,
                    lastVisitDate = (
                        from r in NoTrackingDataContext.RaiinInfs
                        where r.HpId == hpId
                            && r.PtId == p.PtId
                            && r.Status >= RaiinState.TempSave
                            && r.IsDeleted == DeleteTypes.None
                        orderby r.SinDate descending
                        select r.SinDate
                    ).FirstOrDefault()
                };

            return ptInfWithLastVisitDate.AsEnumerable().Select(p => ToModel(p.ptInf, string.Empty, p.lastVisitDate)).ToList();
        }

        public List<PatientInforModel> GetAdvancedSearchResults(PatientAdvancedSearchInput input, int hpId, int pageIndex, int pageSize)
        {
            var ptInfQuery = NoTrackingDataContext.PtInfs.Where(p => p.HpId == hpId && p.IsDelete == DeleteTypes.None);
            // PtNum
            if (input.FromPtNum > 0)
            {
                ptInfQuery = ptInfQuery.Where(p => p.PtNum >= input.FromPtNum);
            }
            if (input.ToPtNum > 0)
            {
                ptInfQuery = ptInfQuery.Where(p => p.PtNum <= input.ToPtNum);
            }
            // Name
            if (!string.IsNullOrEmpty(input.Name))
            {
                ptInfQuery = ptInfQuery.Where(p =>
                    (p.Name != null && p.Name.Contains(input.Name))
                    || (p.KanaName != null && p.KanaName.Contains(input.Name))
                    || (p.Name != null && p.Name.Replace(" ", string.Empty).Replace("\u3000", string.Empty).Contains(input.Name))
                    || (p.KanaName != null && p.KanaName.Replace(" ", string.Empty).Replace("\u3000", string.Empty).Contains(input.Name)));
            }
            // Sex
            if (input.Sex > 0)
            {
                ptInfQuery = ptInfQuery.Where(p => p.Sex == input.Sex);
            }
            // BirthDay
            if (input.FromBirthDay > 0)
            {
                ptInfQuery = ptInfQuery.Where(p => p.Birthday >= input.FromBirthDay);

            }
            if (input.ToBirthDay > 0)
            {
                ptInfQuery = ptInfQuery.Where(p => p.Birthday <= input.ToBirthDay);

            }
            // PhoneNum
            if (!string.IsNullOrEmpty(input.PhoneNum))
            {
                ptInfQuery = ptInfQuery.Where(p => p.Tel1!.Contains(input.PhoneNum) || p.Tel2!.Contains(input.PhoneNum));
            }
            // Age
            if (input.FromAge > 0)
            {
                int fromBirthDay = GetBirthDayFromAge(input.FromAge);
                ptInfQuery = ptInfQuery.Where(p => p.Birthday <= fromBirthDay);
            }
            if (input.ToAge > 0)
            {
                int toBirthDay = GetBirthDayFromAge(input.ToAge);
                ptInfQuery = ptInfQuery.Where(p => p.Birthday >= toBirthDay);
            }
            // PostalCode
            if (!string.IsNullOrEmpty(input.PostalCode1)
                && string.IsNullOrEmpty(input.PostalCode2))
            {
                ptInfQuery = ptInfQuery.Where(p => p.HomePost!.StartsWith(input.PostalCode1));
            }
            else if (!string.IsNullOrEmpty(input.PostalCode2)
                && string.IsNullOrEmpty(input.PostalCode1))
            {
                ptInfQuery = ptInfQuery.Where(p => p.HomePost!.EndsWith(input.PostalCode2));
            }
            else if (!string.IsNullOrEmpty(input.PostalCode1)
                && !string.IsNullOrEmpty(input.PostalCode2))
            {
                var postalCode = input.PostalCode1 + input.PostalCode2;
                ptInfQuery = ptInfQuery.Where(p => p.HomePost!.StartsWith(postalCode) || p.HomePost!.EndsWith(postalCode));
            }
            // Address
            if (input.Address != string.Empty)
            {
                ptInfQuery = ptInfQuery.Where(p => p.HomeAddress1!.Contains(input.Address) || p.HomeAddress2!.Contains(input.Address));
            }

            // End simple search
            // Check if we can end the search here
            if (!ptInfQuery.Any()) return new();

            // Continue the search in the related tables. This is the slowest part.
            // VisitDate
            var raiinInfQuery = NoTrackingDataContext.RaiinInfs.Where(r => r.HpId == hpId && r.IsDeleted == DeleteTypes.None);
            if (input.FromVisitDate > 0 || input.ToVisitDate > 0)
            {
                var ptIdsBySinDateQuery = raiinInfQuery;
                if (input.FromVisitDate > 0)
                {
                    ptIdsBySinDateQuery = ptIdsBySinDateQuery.Where(r => r.SinDate >= input.FromVisitDate);
                }
                if (input.ToVisitDate > 0)
                {
                    ptIdsBySinDateQuery = ptIdsBySinDateQuery.Where(r => r.SinDate <= input.ToVisitDate);
                }

                var ptIds = ptIdsBySinDateQuery.Select(r => r.PtId).Distinct().ToList();
                if (ptIds.Count == 0) return new();
                ptInfQuery = ptInfQuery.Where(p => ptIds.Contains(p.PtId));
            }

            // LastVisitDate
            if (input.FromLastVisitDate > 0 || input.ToLastVisitDate > 0)
            {
                var lastVisitDateByPtIdQuery =
                    from raiinInf in raiinInfQuery
                    group raiinInf by raiinInf.PtId into raiinInfGroup
                    select new
                    {
                        ptId = raiinInfGroup.Key,
                        lastVisitDate = (
                            from r in raiinInfQuery
                            where r.PtId == raiinInfGroup.Key
                                && r.Status >= RaiinState.TempSave
                            orderby r.SinDate descending
                            select r.SinDate
                        ).FirstOrDefault()
                    };

                if (input.FromLastVisitDate > 0)
                {
                    lastVisitDateByPtIdQuery = lastVisitDateByPtIdQuery.Where(x => x.lastVisitDate >= input.FromLastVisitDate);
                }
                if (input.ToLastVisitDate > 0)
                {
                    lastVisitDateByPtIdQuery = lastVisitDateByPtIdQuery.Where(x => x.lastVisitDate <= input.ToLastVisitDate);
                }

                var ptIds = lastVisitDateByPtIdQuery.Select(x => x.ptId).ToList();
                if (ptIds.Count == 0) return new();
                ptInfQuery = ptInfQuery.Where(p => ptIds.Contains(p.PtId));
            }

            // InsuranceNum
            // Declare variable here to be able to reuse it in another queries
            IEnumerable<PtHokenInf>? ptHokenInfs = null;
            if (input.FromInsuranceNum > 0 || input.ToInsuranceNum > 0)
            {
                ptHokenInfs = GetPtHokenInfs();
                var ptIdsByInsuranceNumQuery = ptHokenInfs.Where(p => !string.IsNullOrEmpty(p.HokensyaNo));
                if (input.FromInsuranceNum > 0)
                {
                    ptIdsByInsuranceNumQuery = ptIdsByInsuranceNumQuery.Where(p => long.Parse(p.HokensyaNo!) >= input.FromInsuranceNum);
                }
                if (input.ToInsuranceNum > 0)
                {
                    ptIdsByInsuranceNumQuery = ptIdsByInsuranceNumQuery.Where(p => long.Parse(p.HokensyaNo!) <= input.ToInsuranceNum);
                }

                var ptIds = ptIdsByInsuranceNumQuery.Select(p => p.PtId).Distinct().ToList();
                if (ptIds.Count == 0) return new();
                ptInfQuery = ptInfQuery.Where(p => ptIds.Contains(p.PtId));
            }
            // PublicExpensesNum
            // Declare variable here to be able to reuse it in another queries
            IEnumerable<PtKohi>? ptKohis = null;
            if (input.FromPublicExpensesNum > 0 || input.ToPublicExpensesNum > 0)
            {
                ptKohis = GetPtKohis();
                var ptIdsByPublicExpensesNumQuery = ptKohis.Where(p => !string.IsNullOrEmpty(p.FutansyaNo));
                if (input.FromPublicExpensesNum > 0)
                {
                    ptIdsByPublicExpensesNumQuery = ptIdsByPublicExpensesNumQuery.Where(p => long.Parse(p.FutansyaNo!) >= input.FromPublicExpensesNum);
                }
                if (input.ToPublicExpensesNum > 0)
                {
                    ptIdsByPublicExpensesNumQuery = ptIdsByPublicExpensesNumQuery.Where(p => long.Parse(p.FutansyaNo!) <= input.ToPublicExpensesNum);
                }

                var ptIds = ptIdsByPublicExpensesNumQuery.Select(p => p.PtId).Distinct().ToList();
                if (ptIds.Count == 0) return new();
                ptInfQuery = ptInfQuery.Where(p => ptIds.Contains(p.PtId));
            }
            // SpecialPublicExpensesNum
            if (!string.IsNullOrEmpty(input.FromSpecialPublicExpensesNum) || !string.IsNullOrEmpty(input.ToSpecialPublicExpensesNum))
            {
                if (ptKohis is null)
                {
                    ptKohis = GetPtKohis();
                }

                var ptIdsBySpecialPublicExpensesNumQuery = ptKohis.Where(p => !string.IsNullOrEmpty(p.TokusyuNo));
                if (!string.IsNullOrEmpty(input.FromSpecialPublicExpensesNum))
                {
                    ptIdsBySpecialPublicExpensesNumQuery = ptIdsBySpecialPublicExpensesNumQuery.Where(p => p.TokusyuNo!.PadLeft(20, '0').CompareTo(input.FromSpecialPublicExpensesNum.PadLeft(20, '0')) >= 0);
                }
                if (!string.IsNullOrEmpty(input.ToSpecialPublicExpensesNum))
                {
                    ptIdsBySpecialPublicExpensesNumQuery = ptIdsBySpecialPublicExpensesNumQuery.Where(p => p.TokusyuNo!.PadLeft(20, '0').CompareTo(input.ToSpecialPublicExpensesNum.PadLeft(20, '0')) <= 0);
                }

                var ptIds = ptIdsBySpecialPublicExpensesNumQuery.Select(p => p.PtId).Distinct().ToList();
                if (ptIds.Count == 0) return new();
                ptInfQuery = ptInfQuery.Where(p => ptIds.Contains(p.PtId));
            }
            // HokenNum
            if (input.HokenNum > 0)
            {
                if (ptHokenInfs is null)
                {
                    ptHokenInfs = GetPtHokenInfs();
                }

                var ptIds = ptHokenInfs.Where(p => p.HokenNo == input.HokenNum).Select(p => p.PtId).Distinct().ToList();
                if (ptIds.Count == 0) return new();
                ptInfQuery = ptInfQuery.Where(p => ptIds.Contains(p.PtId));
            }
            // Kohi
            if (input.Kohi1Num > 0)
            {
                var ptIds = GetPtIdsByKohi(input.Kohi1Num, input.Kohi1EdaNo);
                if (ptIds.Count == 0) return new();
                ptInfQuery = ptInfQuery.Where(p => ptIds.Contains(p.PtId));
            }
            if (input.Kohi2Num > 0)
            {
                var ptIds = GetPtIdsByKohi(input.Kohi2Num, input.Kohi2EdaNo);
                if (ptIds.Count == 0) return new();
                ptInfQuery = ptInfQuery.Where(p => ptIds.Contains(p.PtId));
            }
            if (input.Kohi3Num > 0)
            {
                var ptIds = GetPtIdsByKohi(input.Kohi3Num, input.Kohi3EdaNo);
                if (ptIds.Count == 0) return new();
                ptInfQuery = ptInfQuery.Where(p => ptIds.Contains(p.PtId));
            }
            if (input.Kohi4Num > 0)
            {
                var ptIds = GetPtIdsByKohi(input.Kohi4Num, input.Kohi4EdaNo);
                if (ptIds.Count == 0) return new();
                ptInfQuery = ptInfQuery.Where(p => ptIds.Contains(p.PtId));
            }
            // PatientGroups
            var validPatientGroups = input.PatientGroups.Where(p => !string.IsNullOrEmpty(p.GroupCode)).ToList();
            if (validPatientGroups.Any())
            {
                var ptGrpInfQuery = NoTrackingDataContext.PtGrpInfs.Where(p => p.IsDeleted == DeleteTypes.None);
                var firstGrp = validPatientGroups.First();
                var ptIdsByPtGroupsQuery = ptGrpInfQuery.Where(p => p.GroupId == firstGrp.GroupId && p.GroupCode == firstGrp.GroupCode).Select(p => p.PtId);
                // Inner join with another groups
                for (int i = 1; i < validPatientGroups.Count; i++)
                {
                    var anotherGrp = validPatientGroups[i];
                    ptIdsByPtGroupsQuery =
                        from ptId in ptIdsByPtGroupsQuery
                        join anotherPtGrpInf in ptGrpInfQuery on ptId equals anotherPtGrpInf.PtId
                        where anotherPtGrpInf.GroupId == anotherGrp.GroupId && anotherPtGrpInf.GroupCode == anotherGrp.GroupCode
                        select ptId;
                }

                var ptIds = ptIdsByPtGroupsQuery.ToList();
                if (ptIds.Count == 0) return new();
                ptInfQuery = ptInfQuery.Where(p => ptIds.Contains(p.PtId));
            }
            // Orders
            if (input.OrderItemCodes.Any())
            {
                var ptIds = new List<long>();
                var odrInfDetailQuery = NoTrackingDataContext.OdrInfDetails;
                var trimmedItemCodes = input.OrderItemCodes.Select(code => code.Trim()).ToList();
                if (input.OrderLogicalOperator == LogicalOperator.Or)
                {
                    ptIds = odrInfDetailQuery.Where(o => trimmedItemCodes.Contains(o.ItemCd!.Trim())).Select(o => o.PtId).Distinct().ToList();
                }
                else if (input.OrderLogicalOperator == LogicalOperator.And)
                {
                    var firstItemCode = trimmedItemCodes.First();
                    var ptIdsByOrdersQuery = odrInfDetailQuery.Where(o => o.ItemCd!.Trim() == firstItemCode.Trim()).Select(p => p.PtId).Distinct();
                    // Inner join with another groups
                    for (int i = 1; i < trimmedItemCodes.Count; i++)
                    {
                        var anotherItemCode = trimmedItemCodes[i];
                        ptIdsByOrdersQuery = (
                            from ptId in ptIdsByOrdersQuery
                            join anotherOdrInfDetail in odrInfDetailQuery on ptId equals anotherOdrInfDetail.PtId
                            where anotherOdrInfDetail.ItemCd!.Trim() == anotherItemCode
                            select ptId
                        ).Distinct();
                    }
                    ptIds = ptIdsByOrdersQuery.ToList();
                }

                if (ptIds.Count == 0) return new();
                ptInfQuery = ptInfQuery.Where(p => ptIds.Contains(p.PtId));
            }
            // Department
            if (input.DepartmentId > 0)
            {
                var ptIds = raiinInfQuery.Where(r => r.KaId == input.DepartmentId).Select(r => r.PtId).Distinct().ToList();
                if (ptIds.Count == 0) return new();
                ptInfQuery = ptInfQuery.Where(p => ptIds.Contains(p.PtId));
            }
            // Doctor
            if (input.DoctorId > 0)
            {
                var ptIds = raiinInfQuery.Where(r => r.TantoId == input.DoctorId).Select(r => r.PtId).Distinct().ToList();
                if (ptIds.Count == 0) return new();
                ptInfQuery = ptInfQuery.Where(p => ptIds.Contains(p.PtId));
            }
            // Byomeis
            var ptByomeiQuery = NoTrackingDataContext.PtByomeis.Where(b => b.IsDeleted == DeleteTypes.None);
            if (input.Byomeis.Any())
            {
                var trimmedByomeis = input.Byomeis.Select(b => new ByomeiSearchInput(b.Code.Trim(), b.Name.Trim(), b.IsFreeWord)).ToList();
                IQueryable<long> ptIdsByByomeisQuery = null!;
                for (int i = 0; i < trimmedByomeis.Count; i++)
                {
                    var byomei = trimmedByomeis[i];
                    var ptIdsByByomeiItemQuery = ptByomeiQuery.Where(p =>
                        p.HpId == hpId
                        && (byomei.IsFreeWord ? p.ByomeiCd!.Trim() == ByomeiConstant.FreeWordCode : p.ByomeiCd!.Trim() == byomei.Code)
                        && (!byomei.IsFreeWord || p.Byomei!.Trim().Contains(byomei.Name))
                        && (input.ResultKbn == -1 || p.TenkiKbn == input.ResultKbn)
                        && (input.ByomeiStartDate <= 0 || p.StartDate >= input.ByomeiStartDate)
                        && (input.ByomeiEndDate <= 0 || p.StartDate <= input.ByomeiEndDate)
                        && (!input.IsSuspectedDisease
                            || p.SyusyokuCd1!.Trim() == ByomeiConstant.SuspectedCode
                            || p.SyusyokuCd2!.Trim() == ByomeiConstant.SuspectedCode
                            || p.SyusyokuCd3!.Trim() == ByomeiConstant.SuspectedCode
                            || p.SyusyokuCd4!.Trim() == ByomeiConstant.SuspectedCode
                            || p.SyusyokuCd5!.Trim() == ByomeiConstant.SuspectedCode
                            || p.SyusyokuCd6!.Trim() == ByomeiConstant.SuspectedCode
                            || p.SyusyokuCd7!.Trim() == ByomeiConstant.SuspectedCode
                            || p.SyusyokuCd8!.Trim() == ByomeiConstant.SuspectedCode
                            || p.SyusyokuCd9!.Trim() == ByomeiConstant.SuspectedCode
                            || p.SyusyokuCd10!.Trim() == ByomeiConstant.SuspectedCode
                            || p.SyusyokuCd11!.Trim() == ByomeiConstant.SuspectedCode
                            || p.SyusyokuCd12!.Trim() == ByomeiConstant.SuspectedCode
                            || p.SyusyokuCd13!.Trim() == ByomeiConstant.SuspectedCode
                            || p.SyusyokuCd14!.Trim() == ByomeiConstant.SuspectedCode
                            || p.SyusyokuCd15!.Trim() == ByomeiConstant.SuspectedCode
                            || p.SyusyokuCd16!.Trim() == ByomeiConstant.SuspectedCode
                            || p.SyusyokuCd11!.Trim() == ByomeiConstant.SuspectedCode
                            || p.SyusyokuCd18!.Trim() == ByomeiConstant.SuspectedCode
                            || p.SyusyokuCd19!.Trim() == ByomeiConstant.SuspectedCode
                            || p.SyusyokuCd20!.Trim() == ByomeiConstant.SuspectedCode
                            || p.SyusyokuCd21!.Trim() == ByomeiConstant.SuspectedCode))
                        .Select(p => p.PtId).Distinct();

                    if (i == 0)
                    {
                        // Initialize
                        ptIdsByByomeisQuery = ptIdsByByomeiItemQuery;
                    }
                    else
                    {
                        if (input.ByomeiLogicalOperator == LogicalOperator.Or)
                        {
                            ptIdsByByomeisQuery = ptIdsByByomeisQuery!.Union(ptIdsByByomeiItemQuery);
                        }
                        else if (input.ByomeiLogicalOperator == LogicalOperator.And)
                        {
                            ptIdsByByomeisQuery = ptIdsByByomeisQuery!.Intersect(ptIdsByByomeiItemQuery);
                        }
                    }
                }

                var ptIds = ptIdsByByomeisQuery.ToList();
                if (ptIds.Count == 0) return new();
                ptInfQuery = ptInfQuery.Where(p => ptIds.Contains(p.PtId));
            }
            // ResultKbn
            if (input.ResultKbn != -1 && input.Byomeis.Count == 0)
            {
                var ptIds = ptByomeiQuery.Where(p => p.TenkiKbn == input.ResultKbn).Select(p => p.PtId).Distinct().ToList();
                if (ptIds.Count == 0) return new();
                ptInfQuery = ptInfQuery.Where(p => ptIds.Contains(p.PtId));
            }

            // Add LastVisitDate to patient info
            var ptInfWithLastVisitDateQuery =
                from ptInf in ptInfQuery
                select new
                {
                    ptInf,
                    lastVisitDate = (
                        from r in raiinInfQuery
                        where r.PtId == ptInf.PtId
                            && r.Status >= RaiinState.TempSave
                        orderby r.SinDate descending
                        select r.SinDate
                    ).FirstOrDefault()
                };

            return ptInfWithLastVisitDateQuery
                                            .AsEnumerable()
                                            .Skip((pageIndex - 1) * pageSize)
                                            .Take(pageSize)
                                            .Select(p => ToModel(p.ptInf, string.Empty, p.lastVisitDate))
                                            .ToList();

            #region Helper methods

            List<long> GetPtIdsByKohi(int kohiNum, int kohiEdaNo)
            {
                if (ptKohis is null)
                {
                    ptKohis = GetPtKohis();
                }

                return ptKohis.Where(p => p.HokenNo == kohiNum && p.HokenEdaNo == kohiEdaNo).Select(p => p.PtId).Distinct().ToList();
            }

            int GetBirthDayFromAge(int age)
            {
                var bithDay = DateTime.Now.AddYears(-age);
                return CIUtil.ShowSDateToSDate(bithDay.ToString("yyyyMMdd"));
            }

            IEnumerable<PtHokenInf> GetPtHokenInfs()
            {
                return NoTrackingDataContext.PtHokenInfs.Where(p => p.IsDeleted == DeleteTypes.None).AsEnumerable();
            }

            IEnumerable<PtKohi> GetPtKohis()
            {
                return NoTrackingDataContext.PtKohis.Where(p => p.IsDeleted == DeleteTypes.None).AsEnumerable();
            }

            #endregion
        }

        public List<TokkiMstModel> GetListTokki(int hpId, int sinDate)
        {
            return NoTrackingDataContext.TokkiMsts
                    .Where(entity => entity.HpId == hpId && entity.StartDate <= sinDate && entity.EndDate >= sinDate)
                    .OrderBy(entity => entity.HpId)
                    .ThenBy(entity => entity.TokkiCd)
                    .Select(x => new TokkiMstModel(x.TokkiCd, x.TokkiName ?? string.Empty))
                    .ToList();
        }

        private PatientInforModel ToModel(PtInf p, string memo, int lastVisitDate)
        {
            return new PatientInforModel(
                p.HpId,
                p.PtId,
                p.ReferenceNo,
                p.SeqNo,
                p.PtNum,
                p.KanaName ?? string.Empty,
                p.Name ?? string.Empty,
                p.Sex,
                p.Birthday,
                p.LimitConsFlg,
                p.IsDead,
                p.DeathDate,
                p.HomePost ?? string.Empty,
                p.HomeAddress1 ?? string.Empty,
                p.HomeAddress2 ?? string.Empty,
                p.Tel1 ?? string.Empty,
                p.Tel2 ?? string.Empty,
                p.Mail ?? string.Empty,
                p.Setanusi ?? string.Empty,
                p.Zokugara ?? string.Empty,
                p.Job ?? string.Empty,
                p.RenrakuName ?? string.Empty,
                p.RenrakuPost ?? string.Empty,
                p.RenrakuAddress1 ?? string.Empty,
                p.RenrakuAddress2 ?? string.Empty,
                p.RenrakuTel ?? string.Empty,
                p.RenrakuMemo ?? string.Empty,
                p.OfficeName ?? string.Empty,
                p.OfficePost ?? string.Empty,
                p.OfficeAddress1 ?? string.Empty,
                p.OfficeAddress2 ?? string.Empty,
                p.OfficeTel ?? string.Empty,
                p.OfficeMemo ?? string.Empty,
                p.IsRyosyoDetail,
                p.PrimaryDoctor,
                p.IsTester,
                p.MainHokenPid,
                memo,
                lastVisitDate,
                0,
                "");
        }

        public PatientInforModel PatientCommentModels(int hpId, long ptId)
        {
            var data = NoTrackingDataContext.PtCmtInfs
                .FirstOrDefault(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == 0);
            if (data is null)
                return new PatientInforModel();

            return new PatientInforModel(
                data.HpId,
                data.PtId,
                data.Text ?? string.Empty
                );
        }

        public List<PatientInforModel> SearchBySindate(int sindate, int hpId, int pageIndex, int pageSize)
        {
            var ptIdList = NoTrackingDataContext.RaiinInfs.Where(r => r.SinDate == sindate).GroupBy(r => r.PtId).Select(gr => gr.Key).ToList();
            var ptInfWithLastVisitDate =
                (from p in NoTrackingDataContext.PtInfs
                 where p.IsDelete == 0 && ptIdList.Contains(p.PtId)
                 select new
                 {
                     ptInf = p,
                     lastVisitDate = (
                         from r in NoTrackingDataContext.RaiinInfs
                         where r.HpId == hpId
                             && r.PtId == p.PtId
                             && r.Status >= RaiinState.TempSave
                             && r.IsDeleted == DeleteTypes.None
                         orderby r.SinDate descending
                         select r.SinDate
                     ).FirstOrDefault()
                 }).ToList();

            return ptInfWithLastVisitDate
                                         .Skip((pageIndex - 1) * pageSize)
                                         .Take(pageSize)
                                         .Select(p => ToModel(p.ptInf, string.Empty, p.lastVisitDate))
                                         .ToList();
        }

        public List<PatientInforModel> SearchPhone(string keyword, bool isContainMode, int hpId, int pageIndex, int pageSize)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return new List<PatientInforModel>();
            }

            var ptInfWithLastVisitDate =
            from p in NoTrackingDataContext.PtInfs
            where p.IsDelete == 0 && (p.Tel1 != null && (isContainMode && p.Tel1.Contains(keyword) || p.Tel1.StartsWith(keyword)) ||
                                      p.Tel2 != null && (isContainMode && p.Tel2.Contains(keyword) || p.Tel2.StartsWith(keyword)) ||
                                      p.Name == keyword)
            select new
            {
                ptInf = p,
                lastVisitDate = (
                        from r in NoTrackingDataContext.RaiinInfs
                        where r.HpId == hpId
                            && r.PtId == p.PtId
                            && r.Status >= RaiinState.TempSave
                            && r.IsDeleted == DeleteTypes.None
                        orderby r.SinDate descending
                        select r.SinDate
                    ).FirstOrDefault()
            };

            return ptInfWithLastVisitDate
                                         .AsEnumerable()
                                         .Skip((pageIndex - 1) * pageSize)
                                         .Take(pageSize)
                                         .Select(p => ToModel(p.ptInf, string.Empty, p.lastVisitDate))
                                         .ToList();
        }

        public List<PatientInforModel> SearchName(string keyword, bool isContainMode, int hpId, int pageIndex, int pageSize)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return new List<PatientInforModel>();
            }

            var ptInfWithLastVisitDate =
            from p in NoTrackingDataContext.PtInfs
            where p.IsDelete == 0 && (p.Name != null && (isContainMode && p.Name.Contains(keyword) || p.Name.StartsWith(keyword)) ||
                                      p.KanaName != null && (isContainMode && p.KanaName.Contains(keyword) || p.KanaName.StartsWith(keyword)))
            select new
            {
                ptInf = p,
                lastVisitDate = (
                        from r in NoTrackingDataContext.RaiinInfs
                        where r.HpId == hpId
                            && r.PtId == p.PtId
                            && r.Status >= RaiinState.TempSave
                            && r.IsDeleted == DeleteTypes.None
                        orderby r.SinDate descending
                        select r.SinDate
                    ).FirstOrDefault()
            };

            return ptInfWithLastVisitDate
                                         .AsEnumerable()
                                         .Skip((pageIndex - 1) * pageSize)
                                         .Take(pageSize)
                                         .Select(p => ToModel(p.ptInf, string.Empty, p.lastVisitDate))
                                         .ToList();
        }

        public List<PatientInforModel> SearchEmptyId(int hpId, long ptNum, int pageIndex, int pageSize)
        {
            long endIndex = (pageIndex - 1) * pageSize + ptNum + pageSize;
            long startIndex = (pageIndex - 1) * pageSize + ptNum;
            var result = new List<PatientInforModel>();

            var existPtNum = NoTrackingDataContext.PtInfs.Where(p => p.HpId == hpId && p.IsDelete == 0 && p.PtNum >= startIndex && p.PtNum <= endIndex).ToList();

            for (long i = startIndex; i < endIndex; i++)
            {
                var checkExistPtNum = existPtNum.FirstOrDefault(x => x.HpId == hpId && x.PtNum == i && x.IsDelete == 0);
                if (checkExistPtNum == null)
                {
                    result.Add(new PatientInforModel(hpId, 0, i, string.Concat(i, " (空き)")));
                }
                else
                {
                    result.Add(new PatientInforModel(checkExistPtNum.HpId, checkExistPtNum.PtId, checkExistPtNum.PtNum, string.Concat(checkExistPtNum.PtNum, " ", checkExistPtNum.Name)));
                }
            }

            return result;
        }

        public List<DefHokenNoModel> GetDefHokenNoModels(int hpId, string futansyaNo)
        {
            try
            {
                int hokenNo = Int32.Parse(futansyaNo.Substring(0, 2));
                string digit1 = futansyaNo.Substring(0, 1);
                string digit2 = futansyaNo.Substring(1, 1);
                var listDefHoken = NoTrackingDataContext.DefHokenNos
                .Where(x => x.HpId == hpId
                        && (x.HokenNo == hokenNo || x.HokenNo == 0)
                        && x.Digit1.Equals(digit1)
                        && x.Digit2.Equals(digit2)
                        && x.IsDeleted == 0)
                .OrderBy(x => x.SortNo)
                .Select(x => new DefHokenNoModel(
                    x.Digit1,
                    x.Digit2,
                    x.Digit3 ?? string.Empty,
                    x.Digit4 ?? string.Empty,
                    x.Digit5 ?? string.Empty,
                    x.Digit6 ?? string.Empty,
                    x.Digit7 ?? string.Empty,
                    x.Digit8 ?? string.Empty,
                    x.SeqNo,
                    x.HokenNo,
                    x.HokenEdaNo,
                    x.SortNo,
                    x.IsDeleted
                    ))
                .ToList();

                return listDefHoken;
            }
            catch (Exception)
            {
                return new List<DefHokenNoModel>();
            }
        }

        public List<PtKyuseiInfModel> PtKyuseiInfModels(int hpId, long ptId, bool isDeleted)
        {
            var listPtKyusei = NoTrackingDataContext.PtKyuseis
                .Where(x => x.HpId == hpId && x.PtId == ptId && (isDeleted || x.IsDeleted == 0))
                .OrderByDescending(x => x.CreateDate)
                .Select(x => new PtKyuseiInfModel(
                    x.HpId,
                    x.PtId,
                    x.SeqNo,
                    x.KanaName ?? string.Empty,
                    x.Name ?? string.Empty,
                    x.EndDate,
                    x.IsDeleted))
                .ToList();
            return listPtKyusei;
        }

        public bool SaveInsuranceMasterLinkage(List<DefHokenNoModel> defHokenNoModels, int hpId, int userId)
        {
            try
            {
                int sortNo = 1;
                foreach (var item in defHokenNoModels)
                {
                    var checkExistDefHoken = NoTrackingDataContext.DefHokenNos
                        .FirstOrDefault(x => x.SeqNo == item.SeqNo && x.IsDeleted == 0);

                    //Add new if data does not exist
                    if (checkExistDefHoken == null)
                    {
                        TrackingDataContext.DefHokenNos.Add(new DefHokenNo()
                        {
                            HpId = hpId,
                            Digit1 = item.Digit1,
                            Digit2 = item.Digit2,
                            Digit3 = item.Digit3,
                            Digit4 = item.Digit4,
                            Digit5 = item.Digit5,
                            Digit6 = item.Digit6,
                            Digit7 = item.Digit7,
                            Digit8 = item.Digit8,
                            HokenNo = item.HokenNo,
                            HokenEdaNo = item.HokenEdaNo,
                            IsDeleted = 0,
                            CreateDate = DateTime.UtcNow,
                            CreateId = userId,
                            UpdateDate = DateTime.UtcNow,
                            UpdateId = userId,
                            SortNo = sortNo
                        });
                    }
                    else if (checkExistDefHoken.Digit1 == item.Digit1 && checkExistDefHoken.Digit2 == item.Digit2
                        && (checkExistDefHoken.Digit3 != item.Digit3 || checkExistDefHoken.Digit4 != item.Digit4 || checkExistDefHoken.Digit5 != item.Digit5
                        || checkExistDefHoken.Digit6 != item.Digit6 || checkExistDefHoken.Digit7 != item.Digit7 || checkExistDefHoken.Digit8 != item.Digit8
                        || checkExistDefHoken.SortNo != item.SortNo || item.IsDeleted == 1))
                    {
                        TrackingDataContext.DefHokenNos.Update(new DefHokenNo()
                        {
                            HpId = hpId,
                            Digit1 = checkExistDefHoken.Digit1,
                            Digit2 = checkExistDefHoken.Digit2,
                            Digit3 = item.Digit3,
                            Digit4 = item.Digit4,
                            Digit5 = item.Digit5,
                            Digit6 = item.Digit6,
                            Digit7 = item.Digit7,
                            Digit8 = item.Digit8,
                            SeqNo = checkExistDefHoken.SeqNo,
                            HokenNo = item.HokenNo,
                            HokenEdaNo = item.HokenEdaNo,
                            IsDeleted = item.IsDeleted,
                            CreateDate = DateTime.SpecifyKind(checkExistDefHoken.CreateDate, DateTimeKind.Utc),
                            CreateId = checkExistDefHoken.CreateId,
                            CreateMachine = checkExistDefHoken.CreateMachine,
                            UpdateDate = DateTime.UtcNow,
                            UpdateId = userId,
                            SortNo = sortNo
                        });
                    }

                    sortNo++;
                }

                TrackingDataContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public (bool, long) CreatePatientInfo(PatientInforSaveModel ptInf, List<PtKyuseiModel> ptKyuseis, List<CalculationInfModel> ptSanteis, List<InsuranceModel> insurances, List<HokenInfModel> hokenInfs, List<KohiInfModel> hokenKohis, List<GroupInfModel> ptGrps, List<LimitListModel> limitLists, int userId)
        {
            int defaultMaxDate = 99999999;
            int hpId = ptInf.HpId;

            PtInf patientInsert = Mapper.Map(ptInf, new PtInf(), (source, dest) => { return dest; });
            if (patientInsert.PtNum == 0)
            {
                patientInsert.PtNum = GetAutoPtNum(hpId);
            }
            else
            {
                var ptExists = NoTrackingDataContext.PtInfs.FirstOrDefault(x => x.PtNum == patientInsert.PtNum && x.HpId == hpId);
                if (ptExists != null)
                    patientInsert.PtNum = GetAutoPtNum(hpId);
            }
            patientInsert.CreateDate = DateTime.UtcNow;
            patientInsert.CreateId = userId;
            patientInsert.UpdateId = userId;
            patientInsert.UpdateDate = DateTime.UtcNow;
            patientInsert.HpId = hpId;
            TrackingDataContext.PtInfs.Add(patientInsert);
            bool resultCreatePatient = TrackingDataContext.SaveChanges() > 0;

            if (!resultCreatePatient)
                return (false, 0);

            if (ptSanteis != null && ptSanteis.Any())
            {
                var ptSanteiInserts = Mapper.Map<CalculationInfModel, PtSanteiConf>(ptSanteis, (src, dest) =>
                {
                    dest.CreateId = userId;
                    dest.PtId = patientInsert.PtId;
                    dest.HpId = hpId;
                    dest.CreateDate = DateTime.UtcNow;
                    dest.UpdateId = userId;
                    dest.UpdateDate = DateTime.UtcNow;
                    return dest;
                });
                TrackingDataContext.PtSanteiConfs.AddRange(ptSanteiInserts);
            }

            if (!string.IsNullOrEmpty(ptInf.Memo))
            {
                TrackingDataContext.PtMemos.Add(new PtMemo()
                {
                    HpId = hpId,
                    PtId = patientInsert.PtId,
                    Memo = ptInf.Memo,
                    CreateId = userId,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateId = userId
                });
            }

            if (ptGrps != null && ptGrps.Any())
            {
                var listPtGrpInf = Mapper.Map<GroupInfModel, PtGrpInf>(ptGrps, (src, dest) =>
                {
                    dest.CreateDate = DateTime.UtcNow;
                    dest.CreateId = userId;
                    dest.HpId = hpId;
                    dest.PtId = patientInsert.PtId;
                    dest.UpdateDate = DateTime.UtcNow;
                    dest.UpdateId = userId;
                    return dest;
                });
                TrackingDataContext.PtGrpInfs.AddRange(listPtGrpInf);
            }

            if (ptKyuseis != null && ptKyuseis.Any())
            {
                var ptKyuseiList = Mapper.Map<PtKyuseiModel, PtKyusei>(ptKyuseis, (src, dest) =>
                {
                    dest.CreateDate = DateTime.UtcNow;
                    dest.CreateId = userId;
                    dest.UpdateId = userId;
                    dest.HpId = hpId;
                    dest.PtId = patientInsert.PtId;
                    dest.UpdateDate = DateTime.UtcNow;
                    return dest;
                });
                TrackingDataContext.PtKyuseis.AddRange(ptKyuseiList);
            }

            #region Hoken parterrn
            List<PtHokenPattern> pthokenPartterns = Mapper.Map<InsuranceModel, PtHokenPattern>(insurances.Where(x => x.IsAddNew), (src, dest) =>
            {
                dest.CreateId = userId;
                dest.UpdateId = userId;
                dest.CreateDate = DateTime.UtcNow;
                dest.UpdateDate = DateTime.UtcNow;
                dest.PtId = patientInsert.PtId;
                dest.HpId = hpId;
                dest.EndDate = src.EndDate == 0 ? defaultMaxDate : src.EndDate;
                return dest;
            });
            TrackingDataContext.PtHokenPatterns.AddRange(pthokenPartterns);
            #endregion Hoken parterrn

            #region HokenInf
            List<PtHokenInf> ptHokenInfs = Mapper.Map<HokenInfModel, PtHokenInf>(hokenInfs.Where(x => x.IsAddNew), (src, dest) =>
            {
                dest.CreateId = userId;
                dest.CreateDate = DateTime.UtcNow;
                dest.UpdateId = userId;
                dest.UpdateDate = DateTime.UtcNow;
                dest.PtId = patientInsert.PtId;
                dest.HpId = hpId;
                dest.EndDate = src.EndDate == 0 ? defaultMaxDate : src.EndDate;

                #region PtRousaiTenki
                TrackingDataContext.PtRousaiTenkis.AddRange(Mapper.Map<RousaiTenkiModel, PtRousaiTenki>(src.ListRousaiTenki, (srcR, destR) =>
                {
                    destR.CreateId = userId;
                    destR.UpdateId = userId;
                    destR.PtId = patientInsert.PtId;
                    destR.HpId = hpId;
                    destR.Tenki = srcR.RousaiTenkiTenki;
                    destR.Sinkei = srcR.RousaiTenkiSinkei;
                    destR.EndDate = srcR.RousaiTenkiEndDate;
                    destR.HokenId = dest.HokenId;
                    destR.CreateDate = DateTime.UtcNow;
                    destR.UpdateDate = DateTime.UtcNow;
                    return destR;
                }));
                #endregion

                #region PtHokenCheck
                TrackingDataContext.PtHokenChecks.AddRange(Mapper.Map<ConfirmDateModel, PtHokenCheck>(src.ConfirmDateList, (srcCf, destCf) =>
                {
                    destCf.CreateId = userId;
                    destCf.CreateDate = DateTime.UtcNow;
                    destCf.CheckDate = DateTime.SpecifyKind(CIUtil.IntToDate(srcCf.ConfirmDate), DateTimeKind.Utc);
                    destCf.CheckCmt = srcCf.CheckComment;
                    destCf.HokenId = dest.HokenId;
                    destCf.CheckId = userId;
                    destCf.PtID = patientInsert.PtId;
                    destCf.HokenGrp = 1;
                    destCf.HpId = hpId;
                    return destCf;
                }));
                #endregion
                return dest;
            });
            TrackingDataContext.PtHokenInfs.AddRange(ptHokenInfs);
            #endregion HokenInf

            #region PtKohiInf
            List<PtKohi> ptKohiInfs = Mapper.Map<KohiInfModel, PtKohi>(hokenKohis.Where(x => x.IsAddNew), (src, dest) =>
            {
                dest.UpdateId = userId;
                dest.UpdateDate = DateTime.UtcNow;
                dest.CreateId = userId;
                dest.CreateDate = DateTime.UtcNow;
                dest.PtId = patientInsert.PtId;
                dest.HpId = hpId;
                dest.EndDate = src.EndDate == 0 ? defaultMaxDate : src.EndDate;
                #region PtHokenCheck
                TrackingDataContext.PtHokenChecks.AddRange(Mapper.Map<ConfirmDateModel, PtHokenCheck>(src.ConfirmDateList, (srcCf, destCf) =>
                {
                    destCf.CreateId = userId;
                    destCf.CreateDate = DateTime.UtcNow;
                    destCf.CheckDate = DateTime.SpecifyKind(CIUtil.IntToDate(srcCf.ConfirmDate), DateTimeKind.Utc);
                    destCf.CheckCmt = srcCf.CheckComment;
                    destCf.HokenId = dest.HokenId;
                    destCf.CheckId = userId;
                    destCf.PtID = patientInsert.PtId;
                    destCf.HokenGrp = 1;
                    destCf.HpId = hpId;
                    return destCf;
                }));
                #endregion
                return dest;
            });
            TrackingDataContext.PtKohis.AddRange(ptKohiInfs);
            #endregion PtKohiInf

            #region Maxmoney
            if(limitLists != null && limitLists.Any())
            {
                TrackingDataContext.LimitListInfs.AddRange(Mapper.Map<LimitListModel, LimitListInf>(limitLists, (src, dest) =>
                {
                    dest.UpdateDate = DateTime.UtcNow;
                    dest.CreateDate = DateTime.UtcNow;
                    dest.PtId = patientInsert.PtId;
                    dest.HpId = hpId;
                    dest.SinDate = src.SinDateY * 10000 + src.SinDateM * 100 + src.SinDateD;
                    dest.UpdateId = userId;
                    dest.CreateId = userId;
                    return dest;
                }));
            }
            #endregion Maxmoney

            int changeDatas = TrackingDataContext.ChangeTracker.Entries().Count(x => x.State == EntityState.Modified || x.State == EntityState.Added);
            if (changeDatas == 0 && resultCreatePatient)
                return (true, patientInsert.PtId);

            return (TrackingDataContext.SaveChanges() > 0, patientInsert.PtId);
        }

        public (bool, long) UpdatePatientInfo(PatientInforSaveModel ptInf, List<PtKyuseiModel> ptKyuseis, List<CalculationInfModel> ptSanteis, List<InsuranceModel> insurances, List<HokenInfModel> hokenInfs, List<KohiInfModel> hokenKohis, List<GroupInfModel> ptGrps, List<LimitListModel> limitLists, int userId)
        {
            int defaultMaxDate = 99999999;
            int hpId = ptInf.HpId;

            #region Patient-info
            PtInf? patientInfo = TrackingDataContext.PtInfs.FirstOrDefault(x => x.PtId == ptInf.PtId);
            if (patientInfo is null)
                return (false, ptInf.PtId);

            Mapper.Map(ptInf, patientInfo, (source, dest) =>
            {
                dest.UpdateDate = DateTime.UtcNow;
                dest.UpdateId = userId;
                return dest;
            });
            #endregion

            #region Patient-memo
            PtMemo? memoCurrent = TrackingDataContext.PtMemos.FirstOrDefault(x => x.PtId == patientInfo.PtId && x.HpId == patientInfo.HpId && x.IsDeleted == 0);
            if (memoCurrent != null)
            {
                if (string.IsNullOrEmpty(ptInf.Memo))
                {
                    memoCurrent.IsDeleted = 1;
                    memoCurrent.UpdateDate = DateTime.UtcNow;
                    memoCurrent.UpdateId = userId;
                }
                else
                {
                    if (memoCurrent.Memo != null && !memoCurrent.Memo.Equals(ptInf.Memo))
                    {
                        memoCurrent.IsDeleted = 1;
                        memoCurrent.UpdateDate = DateTime.UtcNow;
                        memoCurrent.UpdateId = userId;
                        TrackingDataContext.PtMemos.Add(new PtMemo()
                        {
                            HpId = patientInfo.HpId,
                            PtId = patientInfo.PtId,
                            Memo = ptInf.Memo,
                            CreateId = userId,
                            UpdateDate = DateTime.UtcNow,
                            UpdateId = userId,
                            CreateDate = DateTime.UtcNow
                        });
                    }
                }

            }
            else
            {
                if (!string.IsNullOrEmpty(ptInf.Memo))
                {
                    TrackingDataContext.PtMemos.Add(new PtMemo()
                    {
                        HpId = patientInfo.HpId,
                        PtId = patientInfo.PtId,
                        Memo = ptInf.Memo,
                        CreateId = userId,
                        UpdateDate = DateTime.UtcNow,
                        UpdateId = userId,
                        CreateDate = DateTime.UtcNow,
                    });
                }
            }
            #endregion

            #region PtSantei
            var ptSanteiConfDb = TrackingDataContext.PtSanteiConfs.Where(x => x.PtId == patientInfo.PtId && x.IsDeleted == 0 && x.HpId == patientInfo.HpId).ToList();
            var ptSanteiConfRemoves = ptSanteiConfDb.Where(c => !ptSanteis.Any(_ => _.SeqNo == c.SeqNo));

            foreach (var item in ptSanteiConfRemoves)
            {
                item.UpdateId = userId;
                item.UpdateDate = DateTime.UtcNow;
                item.IsDeleted = DeleteTypes.Deleted;
            }

            var ptSanteiConfListAdd = Mapper.Map<CalculationInfModel, PtSanteiConf>(ptSanteis.Where(x => x.SeqNo == 0), (src, dest) =>
            {
                dest.CreateDate = DateTime.UtcNow;
                dest.CreateId = userId;
                dest.HpId = hpId;
                dest.PtId = patientInfo.PtId;
                dest.UpdateDate = DateTime.UtcNow;
                dest.UpdateId = userId;
                return dest;
            });
            TrackingDataContext.PtSanteiConfs.AddRange(ptSanteiConfListAdd);

            foreach (var item in ptSanteis.Where(x => x.SeqNo != 0))
            {
                var ptSanteiUpdate = ptSanteiConfDb.FirstOrDefault(x => x.SeqNo == item.SeqNo);
                if (ptSanteiUpdate != null)
                {
                    ptSanteiUpdate.KbnNo = item.KbnNo;
                    ptSanteiUpdate.EdaNo = item.EdaNo;
                    ptSanteiUpdate.KbnVal = item.KbnVal;
                    ptSanteiUpdate.StartDate = item.StartDate;
                    ptSanteiUpdate.EndDate = item.EndDate;
                    ptSanteiUpdate.UpdateId = userId;
                    ptSanteiUpdate.UpdateDate = DateTime.UtcNow;
                }
            }
            #endregion

            #region PtKyusei

            var databaseKyuseis = TrackingDataContext.PtKyuseis.Where(x => x.PtId == patientInfo.PtId && x.HpId == hpId && x.IsDeleted == DeleteTypes.None).ToList();
            var KyuseiRemoves = databaseKyuseis.Where(c => !ptKyuseis.Any(_ => _.SeqNo == c.SeqNo));

            foreach (var item in KyuseiRemoves)
            {
                item.UpdateId = userId;
                item.UpdateDate = DateTime.UtcNow;
                item.IsDeleted = DeleteTypes.Deleted;
            }

            var ptKyuseiListAdd = Mapper.Map<PtKyuseiModel, PtKyusei>(ptKyuseis.Where(x => x.SeqNo == 0), (src, dest) =>
            {
                dest.CreateDate = DateTime.UtcNow;
                dest.CreateId = userId;
                dest.UpdateId = userId;
                dest.HpId = hpId;
                dest.PtId = patientInfo.PtId;
                dest.UpdateDate = DateTime.UtcNow;
                return dest;
            });
            TrackingDataContext.PtKyuseis.AddRange(ptKyuseiListAdd);

            foreach (var item in ptKyuseis.Where(x => x.SeqNo != 0))
            {
                var kyuseiUpdate = databaseKyuseis.FirstOrDefault(x => x.SeqNo == item.SeqNo);
                if (kyuseiUpdate != null)
                {
                    kyuseiUpdate.UpdateDate = DateTime.UtcNow;
                    kyuseiUpdate.Name = item.Name;
                    kyuseiUpdate.KanaName = item.KanaName;
                    kyuseiUpdate.EndDate = item.EndDate;
                    kyuseiUpdate.UpdateId = userId;
                }
            }
            #endregion

            #region GrpInf
            var databaseGrpInfs = TrackingDataContext.PtGrpInfs.Where(x => x.PtId == patientInfo.PtId && x.IsDeleted == DeleteTypes.None).ToList();

            var GrpInRemoves = databaseGrpInfs.Where(c => !ptGrps.Any(_ => _.GroupId == c.GroupId)
                                        || ptGrps.Any(_ => _.GroupId == c.GroupId && string.IsNullOrEmpty(_.GroupCode)));
            foreach (var item in GrpInRemoves)
            {
                item.UpdateId = userId;
                item.UpdateDate = DateTime.UtcNow;
                item.IsDeleted = DeleteTypes.Deleted;
            }

            foreach (var item in ptGrps)
            {
                var info = databaseGrpInfs.FirstOrDefault(pt => pt.HpId == hpId && pt.PtId == patientInfo.PtId && pt.GroupId == item.GroupId);

                if (info != null && !string.IsNullOrEmpty(item.GroupCode))
                {
                    //Remove record old
                    info.UpdateId = userId;
                    info.UpdateDate = DateTime.UtcNow;
                    info.IsDeleted = DeleteTypes.Deleted;

                    //clone new record
                    PtGrpInf model = Mapper.Map(item, new PtGrpInf(), (source, dest) =>
                    {
                        dest.CreateId = userId;
                        dest.CreateDate = DateTime.UtcNow;
                        dest.UpdateId = userId;
                        dest.UpdateDate = DateTime.UtcNow;
                        dest.PtId = patientInfo.PtId;
                        dest.HpId = hpId;
                        return dest;
                    });
                    TrackingDataContext.PtGrpInfs.Add(model);
                }
                else if (info == null && !string.IsNullOrEmpty(item.GroupCode))
                {
                    PtGrpInf model = Mapper.Map(item, new PtGrpInf(), (source, dest) =>
                    {
                        dest.CreateId = userId;
                        dest.UpdateId = userId;
                        dest.CreateDate = DateTime.UtcNow;
                        dest.UpdateDate = DateTime.UtcNow;
                        dest.PtId = patientInfo.PtId;
                        dest.HpId = hpId;
                        return dest;
                    });
                    TrackingDataContext.PtGrpInfs.Add(model);
                }
                else if (info != null && string.IsNullOrEmpty(item.GroupCode))
                {
                    //delete it 
                    info.UpdateId = userId;
                    info.UpdateDate = DateTime.UtcNow;
                    info.IsDeleted = DeleteTypes.Deleted;
                }
            }
            #endregion


            var databaseHokenPartterns = TrackingDataContext.PtHokenPatterns.Where(x => x.PtId == patientInfo.PtId && x.HpId == patientInfo.HpId && x.IsDeleted == DeleteTypes.None).ToList();
            var databaseHoKentInfs = TrackingDataContext.PtHokenInfs.Where(x => x.PtId == patientInfo.PtId && x.HpId == patientInfo.HpId && x.IsDeleted == DeleteTypes.None).ToList();
            var databasePtKohis = TrackingDataContext.PtKohis.Where(x => x.PtId == patientInfo.PtId && x.HpId == patientInfo.HpId && x.IsDeleted == DeleteTypes.None).ToList();
            var databaseHokenChecks = TrackingDataContext.PtHokenChecks.Where(c => c.PtID == patientInfo.PtId && c.HpId == patientInfo.HpId && c.IsDeleted == DeleteTypes.None).ToList();
            var databasePtRousaiTenkis = TrackingDataContext.PtRousaiTenkis.Where(c => c.PtId == patientInfo.PtId && c.HpId == patientInfo.HpId && c.IsDeleted == DeleteTypes.None).ToList();

            #region Hoken parterrn
            List<PtHokenPattern> deleteHokenPartterns = databaseHokenPartterns.Where(c => !insurances.Any(_ => _.SeqNo == c.SeqNo) && c.IsDeleted == 0).ToList();
            deleteHokenPartterns.ForEach(x =>
            {
                x.IsDeleted = DeleteTypes.Deleted;
                x.UpdateDate = DateTime.UtcNow;
                x.UpdateId = userId;
            });

            List<PtHokenPattern> pthokenPartterns = Mapper.Map<InsuranceModel, PtHokenPattern>(insurances.Where(x => x.SeqNo == 0 && x.IsAddNew), (src, dest) =>
            {
                dest.CreateId = userId;
                dest.CreateDate = DateTime.UtcNow;
                dest.UpdateId = userId;
                dest.UpdateDate = DateTime.UtcNow;
                dest.PtId = patientInfo.PtId;
                dest.HpId = hpId;
                dest.EndDate = src.EndDate == 0 ? defaultMaxDate : src.EndDate;
                return dest;
            });
            TrackingDataContext.PtHokenPatterns.AddRange(pthokenPartterns);

            foreach (var item in insurances.Where(x => x.SeqNo != 0))
            {
                PtHokenPattern? modelUpdate = databaseHokenPartterns.FirstOrDefault(x => x.SeqNo == item.SeqNo);
                if (modelUpdate != null)
                    Mapper.Map(item, modelUpdate, (src, dest) =>
                    {
                        dest.EndDate = src.EndDate == 0 ? defaultMaxDate : src.EndDate;
                        dest.UpdateDate = DateTime.UtcNow;
                        dest.UpdateId = userId;
                        return dest;
                    });
            }
            #endregion Hoken parterrn

            #region HokenInf
            //Add New
            List<PtHokenInf> ptHokenInfs = Mapper.Map<HokenInfModel, PtHokenInf>(hokenInfs.Where(x => x.SeqNo == 0 && x.IsAddNew), (src, dest) =>
            {
                dest.CreateId = userId;
                dest.CreateDate = DateTime.UtcNow;
                dest.UpdateId = userId;
                dest.UpdateDate = DateTime.UtcNow;
                dest.PtId = patientInfo.PtId;
                dest.HpId = hpId;
                dest.EndDate = src.EndDate == 0 ? defaultMaxDate : src.EndDate;

                #region PtRousaiTenki
                TrackingDataContext.PtRousaiTenkis.AddRange(Mapper.Map<RousaiTenkiModel, PtRousaiTenki>(src.ListRousaiTenki, (srcR, destR) =>
                {
                    destR.CreateId = userId;
                    destR.PtId = patientInfo.PtId;
                    destR.HpId = hpId;
                    destR.Tenki = srcR.RousaiTenkiTenki;
                    destR.Sinkei = srcR.RousaiTenkiSinkei;
                    destR.EndDate = srcR.RousaiTenkiEndDate;
                    destR.HokenId = dest.HokenId;
                    destR.UpdateId = userId;
                    destR.CreateDate = DateTime.UtcNow;
                    destR.UpdateDate = DateTime.UtcNow;
                    return destR;
                }));
                #endregion

                #region PtHokenCheck
                TrackingDataContext.PtHokenChecks.AddRange(Mapper.Map<ConfirmDateModel, PtHokenCheck>(src.ConfirmDateList, (srcCf, destCf) =>
                {
                    destCf.CreateId = userId;
                    destCf.UpdateId = userId;
                    destCf.CreateDate = DateTime.UtcNow;
                    destCf.UpdateDate = DateTime.UtcNow;
                    destCf.CheckDate = DateTime.SpecifyKind(CIUtil.IntToDate(srcCf.ConfirmDate), DateTimeKind.Utc);
                    destCf.CheckCmt = srcCf.CheckComment;
                    destCf.HokenId = dest.HokenId;
                    destCf.CheckId = userId;
                    destCf.PtID = patientInfo.PtId;
                    destCf.HpId = hpId;
                    destCf.HokenGrp = 1;
                    return destCf;
                }));
                #endregion
                return dest;
            });
            TrackingDataContext.PtHokenInfs.AddRange(ptHokenInfs);

            //Update
            foreach (var item in hokenInfs.Where(x => x.SeqNo != 0))
            {
                PtHokenInf? updateHokenInf = databaseHoKentInfs.FirstOrDefault(x => x.SeqNo == item.SeqNo);
                if (updateHokenInf != null)
                {
                    //Info inf
                    Mapper.Map(item, updateHokenInf, (src, dest) =>
                    {
                        dest.EndDate = src.EndDate == 0 ? defaultMaxDate : src.EndDate;
                        dest.UpdateDate = DateTime.UtcNow;
                        dest.UpdateId = userId;
                        return dest;
                    });

                    //ConfirmDate
                    UpdateHokenCheck(databaseHokenChecks, item.ConfirmDateList, patientInfo.HpId, patientInfo.PtId, updateHokenInf.HokenId, userId, false);

                    //RousaiTenki
                    var listAddTenki = Mapper.Map<RousaiTenkiModel, PtRousaiTenki>(item.ListRousaiTenki.Where(x => x.SeqNo == 0), (src, dest) =>
                    {
                        dest.Sinkei = src.RousaiTenkiSinkei;
                        dest.Tenki = src.RousaiTenkiTenki;
                        dest.EndDate = src.RousaiTenkiEndDate;
                        dest.PtId = patientInfo.PtId;
                        dest.HpId = hpId;
                        dest.HokenId = updateHokenInf.HokenId;
                        dest.CreateId = userId;
                        dest.CreateDate = DateTime.UtcNow;
                        dest.UpdateId = userId;
                        dest.UpdateDate = DateTime.UtcNow;
                        return dest;
                    });
                    TrackingDataContext.PtRousaiTenkis.AddRange(listAddTenki);

                    foreach (var rsTkUpdate in item.ListRousaiTenki.Where(x => x.SeqNo != 0))
                    {
                        var updateItem = databasePtRousaiTenkis.FirstOrDefault(x => x.HokenId == updateHokenInf.HokenId && x.SeqNo == rsTkUpdate.SeqNo);
                        if (updateItem != null)
                        {
                            updateItem.Sinkei = rsTkUpdate.RousaiTenkiSinkei;
                            updateItem.Tenki = rsTkUpdate.RousaiTenkiTenki;
                            updateItem.EndDate = rsTkUpdate.RousaiTenkiEndDate;
                            updateItem.UpdateDate = DateTime.UtcNow;
                        }
                    }

                    var listDatabaseByHokenInf = databasePtRousaiTenkis.Where(x => x.HokenId == updateHokenInf.HokenId);
                    var listRemoves = listDatabaseByHokenInf.Where(x => !item.ListRousaiTenki.Any(m => m.SeqNo == x.SeqNo)).ToList();

                    listRemoves.ForEach(x =>
                    {
                        x.IsDeleted = 1;
                        x.UpdateId = userId;
                        x.UpdateDate = DateTime.UtcNow;
                    });
                }
            }
            #endregion HokenInf

            #region HokenKohi
            //Add new
            List<PtKohi> ptKohiInfs = Mapper.Map<KohiInfModel, PtKohi>(hokenKohis.Where(x => x.IsAddNew && x.SeqNo == 0), (src, dest) =>
            {
                dest.CreateId = userId;
                dest.CreateDate = DateTime.UtcNow;
                dest.UpdateDate = DateTime.UtcNow;
                dest.UpdateId = userId;
                dest.PtId = patientInfo.PtId;
                dest.HpId = hpId;
                dest.EndDate = src.EndDate == 0 ? defaultMaxDate : src.EndDate;
                #region PtHokenCheck
                TrackingDataContext.PtHokenChecks.AddRange(Mapper.Map<ConfirmDateModel, PtHokenCheck>(src.ConfirmDateList, (srcCf, destCf) =>
                {
                    destCf.CreateId = userId;
                    destCf.CreateDate = DateTime.UtcNow;
                    destCf.UpdateDate = DateTime.UtcNow;
                    destCf.UpdateId = userId;
                    destCf.CheckDate = DateTime.SpecifyKind(CIUtil.IntToDate(srcCf.ConfirmDate), DateTimeKind.Utc);
                    destCf.CheckCmt = srcCf.CheckComment;
                    destCf.HokenId = dest.HokenId;
                    destCf.CheckId = userId;
                    destCf.PtID = patientInfo.PtId;
                    destCf.HokenGrp = 2;
                    destCf.HpId = hpId;
                    return destCf;
                }));
                #endregion
                return dest;
            });
            TrackingDataContext.PtKohis.AddRange(ptKohiInfs);

            //Update
            foreach (var item in hokenKohis.Where(x => !x.IsAddNew && x.SeqNo != 0))
            {
                PtKohi? updateKohi = databasePtKohis.FirstOrDefault(c => c.HokenId == item.HokenId && c.SeqNo == item.SeqNo);
                if (updateKohi != null)
                {
                    //Info Kohi
                    Mapper.Map(item, updateKohi, (src, dest) =>
                    {
                        dest.EndDate = src.EndDate == 0 ? defaultMaxDate : src.EndDate;
                        dest.UpdateDate = DateTime.UtcNow;
                        dest.UpdateId = userId;
                        return dest;
                    });

                    //ConfirmDate
                    UpdateHokenCheck(databaseHokenChecks, item.ConfirmDateList, patientInfo.HpId, patientInfo.PtId, updateKohi.HokenId, userId, true);
                }
            }
            #endregion HokenKohi

            #region Maxmoney
            List<LimitListInf> maxMoneyDatabases = TrackingDataContext.LimitListInfs.Where(x => x.HpId == hpId
                                                                   && x.PtId == patientInfo.PtId
                                                                   && x.IsDeleted == 0).ToList();

            foreach (var item in maxMoneyDatabases)
            {
                var exist = limitLists.FirstOrDefault(x => x.SeqNo == item.SeqNo && x.Id == item.Id);
                if (exist == null)
                {
                    item.IsDeleted = DeleteTypes.Deleted;
                    item.UpdateDate = DateTime.UtcNow;
                    item.UpdateId = userId;
                }
                else
                {
                    item.SortKey = exist.SortKey;
                    item.FutanGaku = exist.FutanGaku;
                    item.TotalGaku = exist.TotalGaku;
                    item.Biko = exist.Biko;
                    item.SinDate = exist.SinDateY * 10000 + exist.SinDateM * 100 + exist.SinDateD;
                    item.UpdateDate = DateTime.UtcNow;
                    item.UpdateId = userId;
                }
            }

            TrackingDataContext.LimitListInfs.AddRange(Mapper.Map<LimitListModel, LimitListInf>(limitLists.Where(x=>x.SeqNo == 0 && x.Id == 0), (src, dest) =>
            {
                dest.UpdateDate = DateTime.UtcNow;
                dest.CreateDate = DateTime.UtcNow;
                dest.PtId = patientInfo.PtId;
                dest.HpId = hpId;
                dest.SinDate = src.SinDateY * 10000 + src.SinDateM * 100 + src.SinDateD;
                dest.UpdateId = userId;
                dest.CreateId = userId;
                return dest;
            }));
            #endregion 

            return (TrackingDataContext.SaveChanges() > 0, patientInfo.PtId);
        }

        private long GetAutoPtNum(int HpId)
        {
            long startPtNum = 1;
            long startPtNumSetting = (long)GetSettingValue(1014, HpId, 1);
            if (startPtNumSetting > 0)
            {
                startPtNum = startPtNumSetting;
            }
            return GetAutoPtNum(startPtNum, HpId);
        }

        private double GetSettingValue(int groupCd, int hpId, int grpEdaNo = 0, int defaultValue = 0, bool fromLastestDb = false)
        {
            SystemConf? systemConf = new SystemConf();
            systemConf = NoTrackingDataContext.SystemConfs.FirstOrDefault(p =>
                    p.HpId == hpId && p.GrpCd == groupCd && p.GrpEdaNo == grpEdaNo);
            return systemConf != null ? systemConf.Val : defaultValue;
        }


        private long GetAutoPtNum(long startValue, int hpId)
        {
            int autoSetting = (int)GetSettingValue(1014, hpId, 0);
            var ptNumExisting = NoTrackingDataContext.PtInfs.FirstOrDefault
                (ptInf => (autoSetting != 1 ? true : ptInf.IsDelete == 0) && ptInf.PtNum == startValue);
            if (ptNumExisting == null)
            {
                return startValue;
            }

            var ptList = NoTrackingDataContext.PtInfs.Where(ptInf => (autoSetting != 1 ? true : ptInf.IsDelete == 0) && ptInf.PtNum >= startValue)
               .OrderBy(ptInf => ptInf.PtNum);

            long minPtNum = 0;
            if (ptList != null && ptList.Any())
            {
                var queryNotExistPtNum =
                    from ptInf in ptList
                    where !(from ptInfDistinct in ptList
                            select ptInfDistinct.PtNum)
                           .Contains(ptInf.PtNum + 1)
                    orderby ptInf.PtNum
                    select ptInf.PtNum;
                if (queryNotExistPtNum != null)
                {
                    minPtNum = queryNotExistPtNum.FirstOrDefault();
                }
            }
            return minPtNum + 1;
        }

        private void UpdateHokenCheck(List<PtHokenCheck> databaseList, List<ConfirmDateModel> savingList, int hpId, long ptId, int hokenId, int actUserId, bool hokenKohi = false)
        {
            int hokenGrp = 1;
            if (hokenKohi)
            {
                hokenGrp = 2;
            }
            var checkDatabaseData = databaseList.Where(c => c.HokenId == hokenId && c.HokenGrp == hokenGrp);
            var deleteList = checkDatabaseData.Where(c => !savingList.Any(_ => _.SeqNo == c.SeqNo) && c.IsDeleted == 0);
            foreach (var deleteItem in deleteList) //Removes
            {
                deleteItem.IsDeleted = 1;
            }

            foreach (var createItem in savingList.Where(c => c.SeqNo == 0)) // Add new
            {
                PtHokenCheck addedHokenCheck = new PtHokenCheck();
                addedHokenCheck.HpId = hpId;
                addedHokenCheck.PtID = ptId;
                addedHokenCheck.HokenGrp = hokenGrp;
                addedHokenCheck.HokenId = hokenId;
                addedHokenCheck.CheckDate = DateTime.SpecifyKind(CIUtil.IntToDate(createItem.ConfirmDate), DateTimeKind.Utc);
                addedHokenCheck.CheckId = actUserId;
                addedHokenCheck.CheckCmt = createItem.CheckComment;
                addedHokenCheck.CreateId = actUserId;
                addedHokenCheck.CreateDate = DateTime.UtcNow;
                addedHokenCheck.UpdateDate = DateTime.UtcNow;
                addedHokenCheck.UpdateId = actUserId;
                TrackingDataContext.PtHokenChecks.Add(addedHokenCheck);
            }

            //Updates
            foreach (var updateItem in savingList.Where(c => c.SeqNo != 0))
            {
                var modelUpdate = checkDatabaseData.FirstOrDefault(c => c.SeqNo == updateItem.SeqNo);
                if (modelUpdate != null)
                {
                    modelUpdate.CheckDate = DateTime.SpecifyKind(CIUtil.IntToDate(updateItem.ConfirmDate), DateTimeKind.Utc);
                    modelUpdate.CheckId = actUserId;
                    modelUpdate.CheckCmt = updateItem.CheckComment;
                    modelUpdate.CreateId = actUserId;
                    modelUpdate.UpdateDate = DateTime.UtcNow;
                }
            }
        }

        public bool DeletePatientInfo(long ptId, int hpId, int userId)
        {
            var patientInf = TrackingDataContext.PtInfs.FirstOrDefault(x => x.PtId == ptId && x.HpId == hpId && x.IsDelete == DeleteTypes.None);
            if (patientInf != null)
            {
                patientInf.IsDelete = DeleteTypes.Deleted;
                patientInf.UpdateDate = DateTime.UtcNow;
                patientInf.UpdateId = userId;
                #region PtMemo
                var ptMemos = TrackingDataContext.PtMemos.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == DeleteTypes.None).ToList();
                foreach (var item in ptMemos)
                {
                    item.IsDeleted = DeleteTypes.Deleted;
                    item.UpdateDate = DateTime.UtcNow;
                    item.UpdateId = userId;
                }
                #endregion

                #region ptKyuseis
                var ptKyuseis = TrackingDataContext.PtKyuseis.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == DeleteTypes.None).ToList();
                ptKyuseis.ForEach(x =>
                {
                    x.IsDeleted = DeleteTypes.Deleted;
                    x.UpdateId = userId;
                    x.UpdateDate = DateTime.UtcNow;
                });
                #endregion

                #region ptSanteis
                var ptSanteis = TrackingDataContext.PtSanteiConfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == DeleteTypes.None).ToList();
                ptSanteis.ForEach(x =>
                {
                    x.IsDeleted = DeleteTypes.Deleted;
                    x.UpdateId = userId;
                    x.UpdateDate = DateTime.UtcNow;
                });
                #endregion

                #region HokenParttern
                var ptHokenParterns = TrackingDataContext.PtHokenPatterns.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == DeleteTypes.None).ToList();
                ptHokenParterns.ForEach(x =>
                {
                    x.IsDeleted = DeleteTypes.Deleted;
                    x.UpdateId = userId;
                    x.UpdateDate = DateTime.UtcNow;
                });
                #endregion

                #region HokenInf
                var ptHokenInfs = TrackingDataContext.PtHokenInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == DeleteTypes.None).ToList();
                ptHokenInfs.ForEach(x =>
                {
                    x.IsDeleted = DeleteTypes.Deleted;
                    x.UpdateId = userId;
                    x.UpdateDate = DateTime.UtcNow;
                });
                #endregion

                #region HokenKohi
                var ptHokenKohis = TrackingDataContext.PtKohis.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == DeleteTypes.None).ToList();
                ptHokenKohis.ForEach(x =>
                {
                    x.IsDeleted = DeleteTypes.Deleted;
                    x.UpdateDate = DateTime.UtcNow;
                    x.UpdateId = userId;
                });
                #endregion

                #region HokenCheck
                var ptHokenChecks = TrackingDataContext.PtHokenChecks.Where(x => x.HpId == hpId && x.PtID == ptId && x.IsDeleted == DeleteTypes.None).ToList();
                ptHokenChecks.ForEach(x =>
                {
                    x.IsDeleted = DeleteTypes.Deleted;
                    x.UpdateId = userId;
                    x.UpdateDate = DateTime.UtcNow;
                });
                #endregion

                #region RousaiTenki
                var ptRousaiTenkies = TrackingDataContext.PtRousaiTenkis.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == DeleteTypes.None).ToList();
                ptRousaiTenkies.ForEach(x =>
                {
                    x.IsDeleted = DeleteTypes.Deleted;
                    x.UpdateId = userId;
                    x.UpdateDate = DateTime.UtcNow;
                });
                #endregion
            }
            return TrackingDataContext.SaveChanges() > 0;
        }

        public bool IsAllowDeletePatient(int hpId, long ptId)
        {
            var raiinInfCount = NoTrackingDataContext.RaiinInfs
                .Count(p => p.HpId == hpId && p.PtId == ptId && p.Status >= RaiinState.TempSave);

            if (raiinInfCount > 0)
                return false;
            return true;
        }

        public HokenMstModel GetHokenMstByInfor(int hokenNo, int hokenEdaNo)
        {
            var hokenMst = TrackingDataContext.HokenMsts.FirstOrDefault(x => x.HokenNo == hokenNo && x.HokenEdaNo == hokenEdaNo);
            if (hokenMst is null)
                return new HokenMstModel();

            return Mapper.Map(hokenMst, new HokenMstModel(), (src, dest) =>
            {
                return dest;
            });
        }

        public HokensyaMstModel GetHokenSyaMstByInfor(int hpId, string houbetu, string hokensya)
        {
            var hokensyaMst = TrackingDataContext.HokensyaMsts.Where(x => x.HpId == hpId && x.HokensyaNo == hokensya && x.Houbetu == houbetu).Select(x => new HokensyaMstModel(x.IsKigoNa)).FirstOrDefault();
            return hokensyaMst ?? new HokensyaMstModel();
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}