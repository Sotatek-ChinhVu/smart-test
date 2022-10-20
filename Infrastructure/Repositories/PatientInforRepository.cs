﻿using Domain.Models.CalculationInf;
using Domain.Models.GroupInf;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Domain.Models.InsuranceMst;
using Domain.Models.PatientInfor;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Helper.Mapping;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class PatientInforRepository : IPatientInforRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        private readonly TenantDataContext _tenantTrackingDataContext;

        public PatientInforRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
            _tenantTrackingDataContext = tenantProvider.GetTrackingTenantDataContext();
        }

        (PatientInforModel, bool) IPatientInforRepository.SearchExactlyPtNum(int ptNum)
        {
            var ptInf = _tenantDataContext.PtInfs.Where(x => x.PtNum == ptNum).FirstOrDefault();
            if (ptInf == null)
            {
                return (new PatientInforModel(), false);
            }

            long ptId = ptInf.PtId;

            //Get ptMemo
            string memo = string.Empty;
            PtMemo? ptMemo = _tenantDataContext.PtMemos.Where(x => x.PtId == ptId).FirstOrDefault();
            if (ptMemo != null)
            {
                memo = ptMemo.Memo ?? string.Empty;
            }

            int lastVisitDate = _tenantDataContext.RaiinInfs
                .Where(r => r.HpId == TempIdentity.HpId && r.PtId == ptId && r.Status >= RaiinState.TempSave && r.IsDeleted == DeleteTypes.None)
                .OrderByDescending(r => r.SinDate)
                .Select(r => r.SinDate)
                .FirstOrDefault();
            PatientInforModel ptInfModel = ToModel(ptInf, memo, lastVisitDate);

            return new(ptInfModel, true);
        }

        public List<PatientInforModel> SearchContainPtNum(int ptNum, string keyword)
        {
            var ptInfWithLastVisitDate =
                from p in _tenantDataContext.PtInfs
                where p.IsDelete == 0 && (p.PtNum == ptNum || p.KanaName.Contains(keyword) || p.Name.Contains(keyword))
                select new
                {
                    ptInf = p,
                    lastVisitDate = (
                        from r in _tenantDataContext.RaiinInfs
                        where r.HpId == TempIdentity.HpId
                            && r.PtId == p.PtId
                            && r.Status >= RaiinState.TempSave
                            && r.IsDeleted == DeleteTypes.None
                        orderby r.SinDate descending
                        select r.SinDate
                    ).FirstOrDefault()
                };

            return ptInfWithLastVisitDate.AsEnumerable().Select(p => ToModel(p.ptInf, string.Empty, p.lastVisitDate)).ToList();
        }

        public PatientInforModel? GetById(int hpId, long ptId, int sinDate, int raiinNo)
        {
            var itemData = _tenantDataContext.PtInfs.Where(x => x.HpId == hpId && x.PtId == ptId).FirstOrDefault();


            // Raiin Count
            string raiinCountString = "";

            // status = RaiinState Receptionist
            var GetCountraiinInf = _tenantDataContext.RaiinInfs.Where(u => u.HpId == hpId &&
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
                PtMemo? ptMemo = _tenantDataContext.PtMemos.Where(x => x.PtId == itemData.PtId).FirstOrDefault();
                if (ptMemo != null)
                {
                    memo = ptMemo.Memo ?? string.Empty;
                }


                //Get lastVisitDate
                int lastVisitDate = 0;
                RaiinInf? raiinInf = _tenantDataContext.RaiinInfs.Where(p => p.HpId == hpId &&
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
                RaiinInf? raiinInfFirstDate = _tenantDataContext.RaiinInfs.Where(x => x.HpId == hpId
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
                    itemData.KanaName,
                    itemData.Name,
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

        public bool CheckListId(List<long> ptIds)
        {
            var countPtInfs = _tenantDataContext.PtInfs.Count(x => ptIds.Contains(x.PtId) && x.IsDelete != 1);
            return ptIds.Count <= countPtInfs;
        }

        public List<PatientInforModel> SearchSimple(string keyword, bool isContainMode)
        {
            long ptNum = keyword.AsLong();
            var ptInfWithLastVisitDate =
                from p in _tenantDataContext.PtInfs
                where p.IsDelete == 0 && (p.PtNum == ptNum || isContainMode && (p.KanaName.Contains(keyword) || p.Name.Contains(keyword)))
                select new
                {
                    ptInf = p,
                    lastVisitDate = (
                        from r in _tenantDataContext.RaiinInfs
                        where r.HpId == TempIdentity.HpId
                            && r.PtId == p.PtId
                            && r.Status >= RaiinState.TempSave
                            && r.IsDeleted == DeleteTypes.None
                        orderby r.SinDate descending
                        select r.SinDate
                    ).FirstOrDefault()
                };

            return ptInfWithLastVisitDate.AsEnumerable().Select(p => ToModel(p.ptInf, string.Empty, p.lastVisitDate)).ToList();
        }

        public List<PatientInforModel> GetAdvancedSearchResults(PatientAdvancedSearchInput input)
        {
            var ptInfQuery = _tenantDataContext.PtInfs.Where(p => p.HpId == TempIdentity.HpId && p.IsDelete == DeleteTypes.None);
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
                    p.Name.Contains(input.Name)
                    || p.KanaName.Contains(input.Name)
                    || p.Name.Replace(" ", string.Empty).Replace("\u3000", string.Empty).Contains(input.Name)
                    || p.KanaName.Replace(" ", string.Empty).Replace("\u3000", string.Empty).Contains(input.Name));
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
                || string.IsNullOrEmpty(input.PostalCode2))
            {
                ptInfQuery = ptInfQuery.Where(p => p.HomePost!.StartsWith(input.PostalCode1));
            }
            else if (!string.IsNullOrEmpty(input.PostalCode2)
                || string.IsNullOrEmpty(input.PostalCode1))
            {
                ptInfQuery = ptInfQuery.Where(p => p.HomePost!.EndsWith(input.PostalCode2));
            }
            else if (!string.IsNullOrEmpty(input.PostalCode1)
                || !string.IsNullOrEmpty(input.PostalCode2))
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
            var raiinInfQuery = _tenantDataContext.RaiinInfs.Where(r => r.HpId == TempIdentity.HpId && r.IsDeleted == DeleteTypes.None);
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
                var ptGrpInfQuery = _tenantDataContext.PtGrpInfs.Where(p => p.IsDeleted == DeleteTypes.None);
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
                var odrInfDetailQuery = _tenantDataContext.OdrInfDetails;
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
            var ptByomeiQuery = _tenantDataContext.PtByomeis.Where(b => b.IsDeleted == DeleteTypes.None);
            if (input.Byomeis.Any())
            {
                var trimmedByomeis = input.Byomeis.Select(b => new ByomeiSearchInput(b.Code.Trim(), b.Name.Trim(), b.IsFreeWord)).ToList();
                IQueryable<long> ptIdsByByomeisQuery = null!;
                for (int i = 0; i < trimmedByomeis.Count; i++)
                {
                    var byomei = trimmedByomeis[i];
                    var ptIdsByByomeiItemQuery = ptByomeiQuery.Where(p =>
                        p.HpId == TempIdentity.HpId
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

            return ptInfWithLastVisitDateQuery.AsEnumerable().Select(p => ToModel(p.ptInf, string.Empty, p.lastVisitDate)).ToList();

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
                return _tenantDataContext.PtHokenInfs.Where(p => p.IsDeleted == DeleteTypes.None).AsEnumerable();
            }

            IEnumerable<PtKohi> GetPtKohis()
            {
                return _tenantDataContext.PtKohis.Where(p => p.IsDeleted == DeleteTypes.None).AsEnumerable();
            }

            #endregion
        }

        public List<TokkiMstModel> GetListTokki(int hpId, int sinDate)
        {
            return _tenantDataContext.TokkiMsts
                    .Where(entity => entity.HpId == hpId && entity.StartDate <= sinDate && entity.EndDate >= sinDate)
                    .OrderBy(entity => entity.HpId)
                    .ThenBy(entity => entity.TokkiCd)
                    .Select(x => new TokkiMstModel(x.TokkiCd, x.TokkiName))
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
                p.KanaName,
                p.Name,
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
            var data = _tenantDataContext.PtCmtInfs
                .FirstOrDefault(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == 0);
            if (data is null)
                return new PatientInforModel();

            return new PatientInforModel(
                data.HpId,
                data.PtId,
                data.Text ?? string.Empty
                );
        }

        public List<PatientInforModel> SearchBySindate(int sindate)
        {
            var ptIdList = _tenantDataContext.RaiinInfs.Where(r => r.SinDate == sindate).GroupBy(r => r.PtId).Select(gr => gr.Key).ToList();
            var ptInfWithLastVisitDate =
                (from p in _tenantDataContext.PtInfs
                 where p.IsDelete == 0 && ptIdList.Contains(p.PtId)
                 select new
                 {
                     ptInf = p,
                     lastVisitDate = (
                         from r in _tenantDataContext.RaiinInfs
                         where r.HpId == TempIdentity.HpId
                             && r.PtId == p.PtId
                             && r.Status >= RaiinState.TempSave
                             && r.IsDeleted == DeleteTypes.None
                         orderby r.SinDate descending
                         select r.SinDate
                     ).FirstOrDefault()
                 }).ToList();

            return ptInfWithLastVisitDate.Select(p => ToModel(p.ptInf, string.Empty, p.lastVisitDate)).ToList();
        }

        public List<PatientInforModel> SearchPhone(string keyword, bool isContainMode)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return new List<PatientInforModel>();
            }

            var ptInfWithLastVisitDate =
            from p in _tenantDataContext.PtInfs
            where p.IsDelete == 0 && (p.Tel1 != null && (isContainMode && p.Tel1.Contains(keyword) || p.Tel1.StartsWith(keyword)) ||
                                      p.Tel2 != null && (isContainMode && p.Tel2.Contains(keyword) || p.Tel2.StartsWith(keyword)) ||
                                      p.Name == keyword)
            select new
            {
                ptInf = p,
                lastVisitDate = (
                        from r in _tenantDataContext.RaiinInfs
                        where r.HpId == TempIdentity.HpId
                            && r.PtId == p.PtId
                            && r.Status >= RaiinState.TempSave
                            && r.IsDeleted == DeleteTypes.None
                        orderby r.SinDate descending
                        select r.SinDate
                    ).FirstOrDefault()
            };

            return ptInfWithLastVisitDate.AsEnumerable().Select(p => ToModel(p.ptInf, string.Empty, p.lastVisitDate)).ToList();
        }

        public List<PatientInforModel> SearchName(string keyword, bool isContainMode)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return new List<PatientInforModel>();
            }

            var ptInfWithLastVisitDate =
            from p in _tenantDataContext.PtInfs
            where p.IsDelete == 0 && (p.Name != null && (isContainMode && p.Name.Contains(keyword) || p.Name.StartsWith(keyword)) ||
                                      p.KanaName != null && (isContainMode && p.KanaName.Contains(keyword) || p.KanaName.StartsWith(keyword)))
            select new
            {
                ptInf = p,
                lastVisitDate = (
                        from r in _tenantDataContext.RaiinInfs
                        where r.HpId == TempIdentity.HpId
                            && r.PtId == p.PtId
                            && r.Status >= RaiinState.TempSave
                            && r.IsDeleted == DeleteTypes.None
                        orderby r.SinDate descending
                        select r.SinDate
                    ).FirstOrDefault()
            };

            return ptInfWithLastVisitDate.AsEnumerable().Select(p => ToModel(p.ptInf, string.Empty, p.lastVisitDate)).ToList();
        }

        public List<PatientInforModel> SearchEmptyId(int hpId, long ptNum, int pageIndex, int pageSize)
        {
            long endIndex = (pageIndex - 1) * pageSize + ptNum + pageSize;
            long startIndex = (pageIndex - 1) * pageSize + ptNum;
            var result = new List<PatientInforModel>();

            var existPtNum = _tenantDataContext.PtInfs.Where(p => p.HpId == hpId && p.IsDelete == 0 && p.PtNum >= startIndex && p.PtNum <= endIndex).ToList();

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
                var listDefHoken = _tenantDataContext.DefHokenNos
                .Where(x => x.HpId == hpId && x.HokenNo == hokenNo && x.IsDeleted == 0)
                .OrderBy(x => x.SortNo)
                .Select(x => new DefHokenNoModel(
                    x.HpId,
                    x.Digit1,
                    x.Digit2,
                    x.Digit3,
                    x.Digit4,
                    x.Digit5,
                    x.Digit6,
                    x.Digit7,
                    x.Digit8,
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
            var listPtKyusei = _tenantDataContext.PtKyuseis
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

        public bool SaveInsuranceMasterLinkage(List<DefHokenNoModel> defHokenNoModels)
        {
            try
            {
                long countDefHokenNo = _tenantDataContext.DefHokenNos.Count(x => x.IsDeleted == 0 && x.HokenNo == defHokenNoModels[0].HokenNo);
                if (defHokenNoModels.Count != countDefHokenNo)
                    return false;

                int sortNo = 1;
                foreach (var item in defHokenNoModels)
                {
                    var checkExistDefHoken = _tenantDataContext.DefHokenNos
                        .FirstOrDefault(x => x.SeqNo == item.SeqNo && x.IsDeleted == 0);

                    //Add new if data does not exist
                    if (checkExistDefHoken == null)
                    {
                        _tenantTrackingDataContext.DefHokenNos.Add(new DefHokenNo()
                        {
                            HpId = TempIdentity.HpId,
                            Digit1 = item.Digit1,
                            Digit2 = item.Digit2,
                            Digit3 = item.Digit3,
                            Digit4 = item.Digit4,
                            Digit5 = item.Digit5,
                            Digit6 = item.Digit6,
                            Digit7 = item.Digit7,
                            Digit8 = item.Digit8,
                            HokenNo = Int32.Parse(string.Concat(item.Digit1, item.Digit2)),
                            HokenEdaNo = item.HokenEdaNo,
                            IsDeleted = 0,
                            CreateDate = DateTime.UtcNow,
                            CreateId = TempIdentity.UserId,
                            CreateMachine = TempIdentity.ComputerName,
                            UpdateDate = DateTime.UtcNow,
                            UpdateId = TempIdentity.UserId,
                            UpdateMachine = TempIdentity.ComputerName,
                            SortNo = sortNo
                        });
                    }
                    else if (checkExistDefHoken.HpId == item.HpId && checkExistDefHoken.Digit1 == item.Digit1 && checkExistDefHoken.Digit2 == item.Digit2
                        && (checkExistDefHoken.Digit3 != item.Digit3 || checkExistDefHoken.Digit4 != item.Digit4 || checkExistDefHoken.Digit5 != item.Digit5
                        || checkExistDefHoken.Digit6 != item.Digit6 || checkExistDefHoken.Digit7 != item.Digit7 || checkExistDefHoken.Digit8 != item.Digit8
                        || checkExistDefHoken.SortNo != item.SortNo || item.IsDeleted == 1))
                    {
                        _tenantTrackingDataContext.DefHokenNos.Update(new DefHokenNo()
                        {
                            HpId = TempIdentity.HpId,
                            Digit1 = checkExistDefHoken.Digit1,
                            Digit2 = checkExistDefHoken.Digit2,
                            Digit3 = item.Digit3,
                            Digit4 = item.Digit4,
                            Digit5 = item.Digit5,
                            Digit6 = item.Digit6,
                            Digit7 = item.Digit7,
                            Digit8 = item.Digit8,
                            SeqNo = checkExistDefHoken.SeqNo,
                            HokenNo = checkExistDefHoken.HokenNo,
                            HokenEdaNo = item.HokenEdaNo,
                            IsDeleted = item.IsDeleted,
                            CreateDate = DateTime.SpecifyKind(checkExistDefHoken.CreateDate, DateTimeKind.Utc),
                            CreateId = checkExistDefHoken.CreateId,
                            CreateMachine = checkExistDefHoken.CreateMachine,
                            UpdateDate = DateTime.UtcNow,
                            UpdateId = TempIdentity.UserId,
                            UpdateMachine = TempIdentity.ComputerName,
                            SortNo = sortNo
                        });
                    }

                    sortNo++;
                }

                _tenantTrackingDataContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool CreatePatientInfo(PatientInforSaveModel ptInf, List<PtKyuseiModel> ptKyuseis, List<CalculationInfModel> ptSanteis, List<InsuranceModel> insurances, List<GroupInfModel> ptGrps)
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
                var ptExists = _tenantDataContext.PtInfs.FirstOrDefault(x => x.PtNum == patientInsert.PtNum && x.HpId == hpId);
                if (ptExists != null)
                    patientInsert.PtNum = GetAutoPtNum(hpId);
            }
            patientInsert.CreateDate = DateTime.UtcNow;
            patientInsert.CreateId = TempIdentity.UserId;
            patientInsert.UpdateDate = DateTime.UtcNow;
            patientInsert.HpId = hpId;
            _tenantTrackingDataContext.PtInfs.Add(patientInsert);
            bool resultCreatePatient = _tenantTrackingDataContext.SaveChanges() > 0;

            if (!resultCreatePatient)
                return false;

            if (ptSanteis != null && ptSanteis.Any())
            {
                var ptSanteiInserts = Mapper.Map<CalculationInfModel, PtSanteiConf>(ptSanteis, (src, dest) =>
                {
                    dest.CreateId = TempIdentity.UserId;
                    dest.PtId = patientInsert.PtId;
                    dest.HpId = hpId;
                    dest.UpdateMachine = TempIdentity.ComputerName;
                    dest.CreateId = TempIdentity.UserId;
                    dest.CreateDate = DateTime.UtcNow;
                    dest.UpdateDate = DateTime.UtcNow;
                    return dest;
                });
                _tenantTrackingDataContext.PtSanteiConfs.AddRange(ptSanteiInserts);
            }

            if (!string.IsNullOrEmpty(ptInf.Memo))
            {
                _tenantTrackingDataContext.PtMemos.Add(new PtMemo()
                {
                    HpId = hpId,
                    PtId = patientInsert.PtId,
                    Memo = ptInf.Memo,
                    CreateId = TempIdentity.UserId,
                    UpdateMachine = TempIdentity.ComputerName,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                });
            }

            if (ptGrps != null && ptGrps.Any())
            {
                var listPtGrpInf = Mapper.Map<GroupInfModel, PtGrpInf>(ptGrps, (src, dest) =>
                {
                    dest.CreateDate = DateTime.UtcNow;
                    dest.CreateId = TempIdentity.UserId;
                    dest.UpdateMachine = TempIdentity.ComputerName;
                    dest.HpId = hpId;
                    dest.PtId = patientInsert.PtId;
                    dest.UpdateDate = DateTime.UtcNow;
                    return dest;
                });
                _tenantTrackingDataContext.PtGrpInfs.AddRange(listPtGrpInf);
            }

            if (ptKyuseis != null && ptKyuseis.Any())
            {
                var ptKyuseiList = Mapper.Map<PtKyuseiModel, PtKyusei>(ptKyuseis, (src, dest) =>
                {
                    dest.CreateDate = DateTime.UtcNow;
                    dest.CreateId = TempIdentity.UserId;
                    dest.UpdateMachine = TempIdentity.ComputerName;
                    dest.HpId = hpId;
                    dest.PtId = patientInsert.PtId;
                    dest.UpdateDate = DateTime.UtcNow;
                    return dest;
                });
                _tenantTrackingDataContext.PtKyuseis.AddRange(ptKyuseiList);
            }

            //Hoken
            int hoKenIndex = 1;
            int hokenKohiIndex = 1;
            if (insurances != null && insurances.Any())
            {
                foreach (var hokenParttern in insurances)
                {
                    var hokenModel = Mapper.Map(hokenParttern, new PtHokenPattern(), (source, dest) =>
                    {
                        dest.CreateId = TempIdentity.UserId;
                        dest.CreateDate = DateTime.UtcNow;
                        dest.UpdateDate = DateTime.UtcNow;
                        dest.UpdateMachine = TempIdentity.ComputerName;
                        dest.PtId = patientInsert.PtId;
                        dest.HpId = hpId;
                        return dest;
                    });

                    hokenModel.HokenPid = hoKenIndex;
                    hokenModel.HokenId = hoKenIndex;
                    hokenModel.EndDate = hokenModel.EndDate == 0 ? defaultMaxDate : hokenModel.EndDate;

                    var hokenInfModel = Mapper.Map(hokenParttern.HokenInf, new PtHokenInf(), (source, dest) =>
                    {
                        dest.CreateId = TempIdentity.UserId;
                        dest.CreateDate = DateTime.UtcNow;
                        dest.UpdateDate = DateTime.UtcNow;
                        dest.UpdateMachine = TempIdentity.ComputerName;
                        dest.PtId = patientInsert.PtId;
                        dest.HpId = hpId;
                        return dest;
                    });

                    hokenInfModel.HokenId = hoKenIndex;
                    hokenInfModel.EndDate = hokenInfModel.EndDate == 0 ? defaultMaxDate : hokenInfModel.EndDate;
                    _tenantTrackingDataContext.PtHokenInfs.Add(hokenInfModel);

                    if (hokenParttern.HokenInf.ListRousaiTenki.Any())
                    {
                        var listAddTenki = Mapper.Map<RousaiTenkiModel, PtRousaiTenki>(hokenParttern.HokenInf.ListRousaiTenki, (src, dest) =>
                        {
                            dest.CreateId = TempIdentity.UserId;
                            dest.PtId = patientInsert.PtId;
                            dest.HpId = hpId;
                            dest.Tenki = src.RousaiTenkiTenki;
                            dest.Sinkei = src.RousaiTenkiSinkei;
                            dest.EndDate = src.RousaiTenkiEndDate;
                            dest.HokenId = hokenInfModel.HokenId;
                            dest.UpdateMachine = TempIdentity.ComputerName;
                            dest.CreateId = TempIdentity.UserId;
                            dest.CreateDate = DateTime.UtcNow;
                            dest.UpdateDate = DateTime.UtcNow;
                            return dest;
                        });
                        _tenantTrackingDataContext.PtRousaiTenkis.AddRange(listAddTenki);
                    }

                    if (hokenParttern.HokenInf.ConfirmDateList != null && hokenParttern.HokenInf.ConfirmDateList.Any())
                    {
                        foreach (var item in hokenParttern.HokenInf.ConfirmDateList)
                        {
                            PtHokenCheck addPtHokenCheck = Mapper.Map(item, new PtHokenCheck(), (source, dest) =>
                            {
                                dest.CreateId = TempIdentity.UserId;
                                dest.CreateDate = DateTime.UtcNow;
                                dest.UpdateDate = DateTime.UtcNow;
                                dest.UpdateMachine = TempIdentity.ComputerName;
                                dest.CheckDate = DateTime.SpecifyKind(CIUtil.IntToDate(source.ConfirmDate), DateTimeKind.Utc);
                                dest.CheckCmt = source.CheckComment;
                                dest.HokenId = hokenModel.HokenId;
                                dest.CheckId = TempIdentity.UserId;
                                dest.PtID = patientInsert.PtId;
                                dest.HokenGrp = 1;
                                dest.HpId = hpId;
                                return dest;
                            });
                            _tenantTrackingDataContext.PtHokenChecks.Add(addPtHokenCheck);
                        }
                    }

                    if (hokenParttern.Kohi1 != null && !hokenParttern.Kohi1.IsEmptyModel)
                    {
                        var kohi = Mapper.Map(hokenParttern.Kohi1, new PtKohi(), (source, dest) =>
                        {
                            dest.CreateId = TempIdentity.UserId;
                            dest.CreateDate = DateTime.UtcNow;
                            dest.UpdateDate = DateTime.UtcNow;
                            dest.UpdateMachine = TempIdentity.ComputerName;
                            dest.PtId = patientInsert.PtId;
                            dest.HpId = hpId;
                            dest.HokenId = hokenKohiIndex;
                            dest.EndDate = source.EndDate == 0 ? defaultMaxDate : source.EndDate;
                            return dest;
                        });

                        _tenantTrackingDataContext.PtKohis.Add(kohi);
                        hokenModel.Kohi1Id = hokenKohiIndex;
                        hokenKohiIndex++;

                        if (hokenParttern.Kohi1.ConfirmDateList != null && hokenParttern.Kohi1.ConfirmDateList.Any())
                        {
                            var listAddHokenCheck = Mapper.Map<ConfirmDateModel, PtHokenCheck>(hokenParttern.Kohi1.ConfirmDateList, (src, dest) =>
                            {
                                dest.CreateDate = DateTime.UtcNow;
                                dest.CreateId = TempIdentity.UserId;
                                dest.UpdateDate = DateTime.UtcNow;
                                dest.UpdateMachine = TempIdentity.ComputerName;
                                dest.CheckDate = DateTime.SpecifyKind(CIUtil.IntToDate(src.ConfirmDate), DateTimeKind.Utc);
                                dest.CheckCmt = src.CheckComment;
                                dest.HpId = hpId;
                                dest.PtID = patientInsert.PtId;
                                dest.HokenGrp = 2;
                                dest.HokenId = kohi.HokenId;
                                dest.CheckId = TempIdentity.UserId;
                                return dest;
                            });
                            _tenantTrackingDataContext.PtHokenChecks.AddRange(listAddHokenCheck);
                        }

                    }

                    if (hokenParttern.Kohi2 != null && !hokenParttern.Kohi2.IsEmptyModel)
                    {
                        var kohi = Mapper.Map(hokenParttern.Kohi2, new PtKohi(), (source, dest) =>
                        {
                            dest.CreateId = TempIdentity.UserId;
                            dest.CreateDate = DateTime.UtcNow;
                            dest.UpdateDate = DateTime.UtcNow;
                            dest.UpdateMachine = TempIdentity.ComputerName;
                            dest.PtId = patientInsert.PtId;
                            dest.HpId = hpId;
                            dest.HokenId = hokenKohiIndex;
                            dest.EndDate = source.EndDate == 0 ? defaultMaxDate : source.EndDate;
                            return dest;
                        });

                        _tenantTrackingDataContext.PtKohis.Add(kohi);
                        hokenModel.Kohi2Id = hokenKohiIndex;
                        hokenKohiIndex++;

                        if (hokenParttern.Kohi2.ConfirmDateList != null && hokenParttern.Kohi2.ConfirmDateList.Any())
                        {
                            var listAddHokenCheck = Mapper.Map<ConfirmDateModel, PtHokenCheck>(hokenParttern.Kohi2.ConfirmDateList, (src, dest) =>
                            {
                                dest.CreateDate = DateTime.UtcNow;
                                dest.CreateId = TempIdentity.UserId;
                                dest.UpdateDate = DateTime.UtcNow;
                                dest.UpdateMachine = TempIdentity.ComputerName;
                                dest.CheckDate = DateTime.SpecifyKind(CIUtil.IntToDate(src.ConfirmDate), DateTimeKind.Utc);
                                dest.CheckCmt = src.CheckComment;
                                dest.HpId = hpId;
                                dest.PtID = patientInsert.PtId;
                                dest.HokenGrp = 2;
                                dest.HokenId = kohi.HokenId;
                                dest.CheckId = TempIdentity.UserId;
                                return dest;
                            });
                            _tenantTrackingDataContext.PtHokenChecks.AddRange(listAddHokenCheck);
                        }
                    }

                    if (hokenParttern.Kohi3 != null && !hokenParttern.Kohi3.IsEmptyModel)
                    {
                        var kohi = Mapper.Map(hokenParttern.Kohi3, new PtKohi(), (source, dest) =>
                        {
                            dest.CreateId = TempIdentity.UserId;
                            dest.CreateDate = DateTime.UtcNow;
                            dest.UpdateDate = DateTime.UtcNow;
                            dest.UpdateMachine = TempIdentity.ComputerName;
                            dest.PtId = patientInsert.PtId;
                            dest.HpId = hpId;
                            dest.HokenId = hokenKohiIndex;
                            dest.EndDate = source.EndDate == 0 ? defaultMaxDate : source.EndDate;
                            return dest;
                        });

                        _tenantTrackingDataContext.PtKohis.Add(kohi);
                        hokenModel.Kohi3Id = hokenKohiIndex;
                        hokenKohiIndex++;

                        if (hokenParttern.Kohi3.ConfirmDateList != null && hokenParttern.Kohi3.ConfirmDateList.Any())
                        {
                            var listAddHokenCheck = Mapper.Map<ConfirmDateModel, PtHokenCheck>(hokenParttern.Kohi3.ConfirmDateList, (src, dest) =>
                            {
                                dest.CreateDate = DateTime.UtcNow;
                                dest.CreateId = TempIdentity.UserId;
                                dest.UpdateDate = DateTime.UtcNow;
                                dest.UpdateMachine = TempIdentity.ComputerName;
                                dest.CheckDate = DateTime.SpecifyKind(CIUtil.IntToDate(src.ConfirmDate), DateTimeKind.Utc);
                                dest.CheckCmt = src.CheckComment;
                                dest.HpId = hpId;
                                dest.PtID = patientInsert.PtId;
                                dest.HokenGrp = 2;
                                dest.HokenId = kohi.HokenId;
                                dest.CheckId = TempIdentity.UserId;
                                return dest;
                            });
                            _tenantTrackingDataContext.PtHokenChecks.AddRange(listAddHokenCheck);
                        }
                    }

                    if (hokenParttern.Kohi4 != null && !hokenParttern.Kohi4.IsEmptyModel)
                    {
                        var kohi = Mapper.Map(hokenParttern.Kohi4, new PtKohi(), (source, dest) =>
                        {
                            dest.CreateId = TempIdentity.UserId;
                            dest.CreateDate = DateTime.UtcNow;
                            dest.UpdateDate = DateTime.UtcNow;
                            dest.PtId = patientInsert.PtId;
                            dest.HpId = hpId;
                            dest.HokenId = hokenKohiIndex;
                            dest.EndDate = source.EndDate == 0 ? defaultMaxDate : source.EndDate;
                            dest.UpdateMachine = TempIdentity.ComputerName;
                            return dest;
                        });

                        _tenantTrackingDataContext.PtKohis.Add(kohi);
                        hokenModel.Kohi4Id = hokenKohiIndex;
                        hokenKohiIndex++;

                        if (hokenParttern.Kohi4.ConfirmDateList != null && hokenParttern.Kohi4.ConfirmDateList.Any())
                        {
                            var listAddHokenCheck = Mapper.Map<ConfirmDateModel, PtHokenCheck>(hokenParttern.Kohi4.ConfirmDateList, (src, dest) =>
                            {
                                dest.CreateDate = DateTime.UtcNow;
                                dest.CreateId = TempIdentity.UserId;
                                dest.UpdateDate = DateTime.UtcNow;
                                dest.HpId = hpId;
                                dest.PtID = patientInsert.PtId;
                                dest.CheckDate = DateTime.SpecifyKind(CIUtil.IntToDate(src.ConfirmDate), DateTimeKind.Utc);
                                dest.CheckCmt = src.CheckComment;
                                dest.HokenGrp = 2;
                                dest.HokenId = kohi.HokenId;
                                dest.CheckId = TempIdentity.UserId;
                                dest.UpdateMachine = TempIdentity.ComputerName;
                                return dest;
                            });
                            _tenantTrackingDataContext.PtHokenChecks.AddRange(listAddHokenCheck);
                        }
                    }

                    _tenantTrackingDataContext.PtHokenPatterns.Add(hokenModel);
                    hoKenIndex++;
                }
            }
            return _tenantTrackingDataContext.SaveChanges() > 0;
        }

        public bool UpdatePatientInfo(PatientInforSaveModel ptInf, List<PtKyuseiModel> ptKyuseis, List<CalculationInfModel> ptSanteis, List<InsuranceModel> insurances, List<GroupInfModel> ptGrps)
        {
            int defaultMaxDate = 99999999;
            int hpId = ptInf.HpId;

            #region Patient-info
            PtInf? patientInfo = _tenantTrackingDataContext.PtInfs.FirstOrDefault(x => x.PtId == ptInf.PtId);
            if (patientInfo is null)
                return false;

            Mapper.Map(ptInf, patientInfo, (source, dest) =>
            {
                dest.UpdateDate = DateTime.UtcNow;
                dest.UpdateId = TempIdentity.UserId;
                dest.UpdateMachine = TempIdentity.ComputerName;
                return dest;
            });
            #endregion

            #region Patient-memo
            PtMemo? memoCurrent = _tenantTrackingDataContext.PtMemos.FirstOrDefault(x => x.PtId == patientInfo.PtId && x.HpId == patientInfo.HpId && x.IsDeleted == 0);
            if (memoCurrent != null)
            {
                if (string.IsNullOrEmpty(ptInf.Memo))
                {
                    memoCurrent.IsDeleted = 1;
                    memoCurrent.UpdateDate = DateTime.UtcNow;
                    memoCurrent.UpdateMachine = TempIdentity.ComputerName;
                    memoCurrent.UpdateId = TempIdentity.UserId;
                }
                else
                {
                    if (memoCurrent.Memo != null && !memoCurrent.Memo.Equals(ptInf.Memo))
                    {
                        memoCurrent.IsDeleted = 1;
                        memoCurrent.UpdateDate = DateTime.UtcNow;
                        memoCurrent.UpdateMachine = TempIdentity.ComputerName;
                        memoCurrent.UpdateId = TempIdentity.UserId;
                        _tenantTrackingDataContext.PtMemos.Add(new PtMemo()
                        {
                            HpId = patientInfo.HpId,
                            PtId = patientInfo.PtId,
                            Memo = ptInf.Memo,
                            CreateId = TempIdentity.UserId,
                            CreateDate = DateTime.UtcNow,
                            CreateMachine = TempIdentity.ComputerName
                        });
                    }
                }

            }
            else
            {
                if (!string.IsNullOrEmpty(ptInf.Memo))
                {
                    _tenantTrackingDataContext.PtMemos.Add(new PtMemo()
                    {
                        HpId = patientInfo.HpId,
                        PtId = patientInfo.PtId,
                        Memo = ptInf.Memo,
                        CreateId = TempIdentity.UserId,
                        CreateDate = DateTime.UtcNow,
                        CreateMachine = TempIdentity.ComputerName
                    });
                }
            }
            #endregion

            #region PtSantei
            var ptSanteiConfDb = _tenantTrackingDataContext.PtSanteiConfs.Where(x => x.PtId == patientInfo.PtId && x.IsDeleted == 0 && x.HpId == patientInfo.HpId).ToList();
            var ptSanteiConfRemoves = ptSanteiConfDb.Where(c => !ptSanteis.Any(_ => _.SeqNo == c.SeqNo));

            foreach (var item in ptSanteiConfRemoves)
            {
                item.UpdateId = TempIdentity.UserId;
                item.UpdateDate = DateTime.UtcNow;
                item.IsDeleted = DeleteTypes.Deleted;
            }

            var ptSanteiConfListAdd = Mapper.Map<CalculationInfModel, PtSanteiConf>(ptSanteis.Where(x => x.SeqNo == 0), (src, dest) =>
            {
                dest.CreateDate = DateTime.UtcNow;
                dest.CreateId = TempIdentity.UserId;
                dest.UpdateMachine = TempIdentity.ComputerName;
                dest.HpId = hpId;
                dest.PtId = patientInfo.PtId;
                dest.UpdateDate = DateTime.UtcNow;
                return dest;
            });
            _tenantTrackingDataContext.PtSanteiConfs.AddRange(ptSanteiConfListAdd);

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
                    ptSanteiUpdate.UpdateId = TempIdentity.UserId;
                    ptSanteiUpdate.UpdateDate = DateTime.UtcNow;
                }
            }
            #endregion

            #region PtKyusei

            var databaseKyuseis = _tenantTrackingDataContext.PtKyuseis.Where(x => x.PtId == patientInfo.PtId && x.HpId == hpId && x.IsDeleted == DeleteTypes.None).ToList();
            var KyuseiRemoves = databaseKyuseis.Where(c => !ptKyuseis.Any(_ => _.SeqNo == c.SeqNo));

            foreach (var item in KyuseiRemoves)
            {
                item.UpdateId = TempIdentity.UserId;
                item.UpdateDate = DateTime.UtcNow;
                item.IsDeleted = DeleteTypes.Deleted;
            }

            var ptKyuseiListAdd = Mapper.Map<PtKyuseiModel, PtKyusei>(ptKyuseis.Where(x => x.SeqNo == 0), (src, dest) =>
            {
                dest.CreateDate = DateTime.UtcNow;
                dest.CreateId = TempIdentity.UserId;
                dest.UpdateMachine = TempIdentity.ComputerName;
                dest.HpId = hpId;
                dest.PtId = patientInfo.PtId;
                dest.UpdateDate = DateTime.UtcNow;
                return dest;
            });
            _tenantTrackingDataContext.PtKyuseis.AddRange(ptKyuseiListAdd);

            foreach (var item in ptKyuseis.Where(x => x.SeqNo != 0))
            {
                var kyuseiUpdate = databaseKyuseis.FirstOrDefault(x => x.SeqNo == item.SeqNo);
                if (kyuseiUpdate != null)
                {
                    kyuseiUpdate.UpdateDate = DateTime.UtcNow;
                    kyuseiUpdate.UpdateMachine = TempIdentity.ComputerName;
                    kyuseiUpdate.Name = item.Name;
                    kyuseiUpdate.KanaName = item.KanaName;
                    kyuseiUpdate.EndDate = item.EndDate;
                }
            }
            #endregion

            #region GrpInf
            var databaseGrpInfs = _tenantTrackingDataContext.PtGrpInfs.Where(x => x.PtId == patientInfo.PtId && x.IsDeleted == DeleteTypes.None).ToList();

            var GrpInRemoves = databaseGrpInfs.Where(c => !ptGrps.Any(_ => _.GroupId == c.GroupId)
                                        || ptGrps.Any(_ => _.GroupId == c.GroupId && string.IsNullOrEmpty(_.GroupCode)));
            foreach (var item in GrpInRemoves)
            {
                item.UpdateId = TempIdentity.UserId;
                item.UpdateDate = DateTime.UtcNow;
                item.IsDeleted = DeleteTypes.Deleted;
            }

            foreach (var item in ptGrps)
            {
                var info = databaseGrpInfs
                   .Where(pt => pt.HpId == hpId && pt.PtId == patientInfo.PtId && pt.GroupId == item.GroupId)
                   .FirstOrDefault();

                if (info != null && !string.IsNullOrEmpty(item.GroupCode))
                {
                    //Remove record old
                    info.UpdateId = TempIdentity.UserId;
                    info.UpdateDate = DateTime.UtcNow;
                    info.IsDeleted = DeleteTypes.Deleted;

                    //clone new record
                    PtGrpInf model = Mapper.Map(item, new PtGrpInf(), (source, dest) =>
                    {
                        dest.CreateId = TempIdentity.UserId;
                        dest.CreateDate = DateTime.UtcNow;
                        dest.PtId = patientInfo.PtId;
                        dest.HpId = hpId;
                        dest.CreateMachine = TempIdentity.ComputerName;
                        return dest;
                    });
                    _tenantTrackingDataContext.PtGrpInfs.Add(model);
                }
                else if (info == null && !string.IsNullOrEmpty(item.GroupCode))
                {
                    PtGrpInf model = Mapper.Map(item, new PtGrpInf(), (source, dest) =>
                    {
                        dest.CreateId = TempIdentity.UserId;
                        dest.CreateDate = DateTime.UtcNow;
                        dest.PtId = patientInfo.PtId;
                        dest.HpId = hpId;
                        dest.CreateMachine = TempIdentity.ComputerName;
                        return dest;
                    });
                    _tenantTrackingDataContext.PtGrpInfs.Add(model);
                }
                else if (info != null && string.IsNullOrEmpty(item.GroupCode))
                {
                    //delete it 
                    info.UpdateId = TempIdentity.UserId;
                    info.UpdateDate = DateTime.UtcNow;
                    info.IsDeleted = DeleteTypes.Deleted;
                }
            }
            #endregion

            //Hoken
            var databaseHokenPartterns = _tenantTrackingDataContext.PtHokenPatterns.Where(x => x.PtId == patientInfo.PtId && x.HpId == patientInfo.HpId && x.IsDeleted == DeleteTypes.None).ToList();
            var databaseHoKentInfs = _tenantTrackingDataContext.PtHokenInfs.Where(x => x.PtId == patientInfo.PtId && x.HpId == patientInfo.HpId && x.IsDeleted == DeleteTypes.None).ToList();
            var databasePtKohis = _tenantTrackingDataContext.PtKohis.Where(x => x.PtId == patientInfo.PtId && x.HpId == patientInfo.HpId && x.IsDeleted == DeleteTypes.None).ToList();
            var databaseHokenChecks = _tenantTrackingDataContext.PtHokenChecks.Where(c => c.PtID == patientInfo.PtId && c.HpId == patientInfo.HpId && c.IsDeleted == DeleteTypes.None).ToList();
            var databasePtRousaiTenkis = _tenantTrackingDataContext.PtRousaiTenkis.Where(c => c.PtId == patientInfo.PtId && c.HpId == patientInfo.HpId && c.IsDeleted == DeleteTypes.None).ToList();

            int hoKenIndex = databaseHokenPartterns.Any() ? databaseHokenPartterns.Max(c => c.HokenId) + 1 : 1;
            int hokenKohiIndex = databasePtKohis.Any() ? databasePtKohis.Max(c => c.HokenId) + 1 : 1;

            #region Delete data not in request
            var deleteHokenPartterns = databaseHokenPartterns.Where(c => !insurances.Any(_ => _.SeqNo == c.SeqNo) && c.IsDeleted == 0);

            foreach (var hokenPartternDelete in deleteHokenPartterns)
            {
                if (hokenPartternDelete.Kohi1Id != 0)
                {
                    var hokenHokiDelete = databasePtKohis.FirstOrDefault(c => c.HpId == hokenPartternDelete.HpId && c.PtId == hokenPartternDelete.PtId
                                                                        && c.HokenId == hokenPartternDelete.Kohi1Id);
                    if (hokenHokiDelete != null)
                    {
                        hokenHokiDelete.IsDeleted = 1;
                        hokenHokiDelete.UpdateDate = DateTime.UtcNow;
                        hokenHokiDelete.UpdateId = TempIdentity.UserId;

                        var hokenChecks = databaseHokenChecks.Where(c => c.CheckId == hokenHokiDelete.HokenId && c.HokenGrp == 2);
                        if (hokenChecks != null && hokenChecks.Any())
                        {
                            foreach (var check in hokenChecks)
                            {
                                check.IsDeleted = 1;
                                check.UpdateId = TempIdentity.UserId;
                                check.UpdateDate = DateTime.UtcNow;
                            }
                        }
                    }
                }

                if (hokenPartternDelete.Kohi2Id != 0)
                {
                    var hokenHokiDelete = databasePtKohis.FirstOrDefault(c => c.HpId == hokenPartternDelete.HpId && c.PtId == hokenPartternDelete.PtId
                                                                        && c.HokenId == hokenPartternDelete.Kohi2Id);
                    if (hokenHokiDelete != null)
                    {
                        hokenHokiDelete.IsDeleted = 1;
                        hokenHokiDelete.UpdateDate = DateTime.UtcNow;
                        hokenHokiDelete.UpdateId = TempIdentity.UserId;

                        var hokenChecks = databaseHokenChecks.Where(c => c.CheckId == hokenHokiDelete.HokenId && c.HokenGrp == 2);
                        if (hokenChecks != null && hokenChecks.Any())
                        {
                            foreach (var check in hokenChecks)
                            {
                                check.IsDeleted = 1;
                                check.UpdateId = TempIdentity.UserId;
                                check.UpdateDate = DateTime.UtcNow;
                            }
                        }
                    }
                }

                if (hokenPartternDelete.Kohi3Id != 0)
                {
                    var hokenHokiDelete = databasePtKohis.FirstOrDefault(c => c.HpId == hokenPartternDelete.HpId && c.PtId == hokenPartternDelete.PtId
                                                                        && c.HokenId == hokenPartternDelete.Kohi3Id);
                    if (hokenHokiDelete != null)
                    {
                        hokenHokiDelete.IsDeleted = 1;
                        hokenHokiDelete.UpdateDate = DateTime.UtcNow;
                        hokenHokiDelete.UpdateId = TempIdentity.UserId;

                        var hokenChecks = databaseHokenChecks.Where(c => c.CheckId == hokenHokiDelete.HokenId && c.HokenGrp == 2);
                        if (hokenChecks != null && hokenChecks.Any())
                        {
                            foreach (var check in hokenChecks)
                            {
                                check.IsDeleted = 1;
                                check.UpdateId = TempIdentity.UserId;
                                check.UpdateDate = DateTime.UtcNow;
                            }
                        }
                    }
                }

                if (hokenPartternDelete.Kohi4Id != 0)
                {
                    var hokenHokiDelete = databasePtKohis.FirstOrDefault(c => c.HpId == hokenPartternDelete.HpId && c.PtId == hokenPartternDelete.PtId
                                                                        && c.HokenId == hokenPartternDelete.Kohi4Id);
                    if (hokenHokiDelete != null)
                    {
                        hokenHokiDelete.IsDeleted = 1;
                        hokenHokiDelete.UpdateDate = DateTime.UtcNow;
                        hokenHokiDelete.UpdateId = TempIdentity.UserId;

                        var hokenChecks = databaseHokenChecks.Where(c => c.CheckId == hokenHokiDelete.HokenId && c.HokenGrp == 2);
                        if (hokenChecks != null && hokenChecks.Any())
                        {
                            foreach (var check in hokenChecks)
                            {
                                check.IsDeleted = 1;
                                check.UpdateId = TempIdentity.UserId;
                                check.UpdateDate = DateTime.UtcNow;
                            }
                        }
                    }
                }

                var hokenInfDelete = databaseHoKentInfs.FirstOrDefault(c => c.HpId == hokenPartternDelete.HpId && c.PtId == hokenPartternDelete.PtId
                                                                        && c.HokenId == hokenPartternDelete.HokenId);

                if (hokenInfDelete != null)
                {
                    hokenInfDelete.IsDeleted = 1;
                    hokenInfDelete.UpdateDate = DateTime.UtcNow;
                    hokenInfDelete.UpdateId = TempIdentity.UserId;

                    foreach (var itemRsTk in databasePtRousaiTenkis.Where(x => x.HokenId == hokenInfDelete.HokenId))
                    {
                        itemRsTk.IsDeleted = 1;
                        itemRsTk.UpdateDate = DateTime.UtcNow;
                        itemRsTk.UpdateId = TempIdentity.UserId;
                    }
                }

                hokenPartternDelete.IsDeleted = 1;
                hokenPartternDelete.UpdateDate = DateTime.UtcNow;
                hokenPartternDelete.UpdateId = TempIdentity.UserId;

                var hokenPatternChecks = databaseHokenChecks.Where(c => c.CheckId == hokenPartternDelete.HokenId && c.HokenGrp == 1);
                if (hokenPatternChecks != null && hokenPatternChecks.Any())
                {
                    foreach (var check in hokenPatternChecks)
                    {
                        check.IsDeleted = 1;
                        check.UpdateId = TempIdentity.UserId;
                        check.UpdateDate = DateTime.UtcNow;
                    }
                }
            }
            #endregion

            #region Add new & update current
            foreach (var hokenParttern in insurances)
            {
                if (hokenParttern.SeqNo != 0) //update entity
                {
                    var hokenPartternUpdate = databaseHokenPartterns.FirstOrDefault(c => c.SeqNo == hokenParttern.SeqNo);
                    if (hokenPartternUpdate != null)
                    {
                        hokenPartternUpdate.HokenSbtCd = hokenParttern.HokenSbtCd;

                        #region KohiId1
                        if (hokenPartternUpdate.Kohi1Id == 0 && hokenParttern.Kohi1 != null && !hokenParttern.Kohi1.IsEmptyModel) // add new
                        {


                            var kohi = Mapper.Map(hokenParttern.Kohi1, new PtKohi(), (source, dest) =>
                            {
                                dest.CreateId = TempIdentity.UserId;
                                dest.CreateDate = DateTime.UtcNow;
                                dest.UpdateMachine = TempIdentity.ComputerName;
                                dest.PtId = patientInfo.PtId;
                                dest.HpId = hpId;
                                dest.EndDate = source.EndDate == 0 ? defaultMaxDate : source.EndDate;
                                return dest;
                            });
                            _tenantTrackingDataContext.PtKohis.Add(kohi);

                            hokenPartternUpdate.Kohi1Id = kohi.HokenId;
                            hokenKohiIndex++;
                        }
                        else if (hokenPartternUpdate.Kohi1Id != 0 && hokenParttern.Kohi1 != null && hokenParttern.Kohi1.IsEmptyModel) //Case remove
                        {
                            var kohiRemove = databasePtKohis.FirstOrDefault(c => c.HpId == hokenPartternUpdate.HpId && c.PtId == hokenPartternUpdate.PtId
                                                                            && c.HokenId == hokenPartternUpdate.Kohi1Id);
                            if (kohiRemove != null)
                            {
                                kohiRemove.IsDeleted = 1;
                                kohiRemove.UpdateDate = DateTime.UtcNow;
                                kohiRemove.UpdateId = TempIdentity.UserId;
                            }
                            hokenPartternUpdate.Kohi1Id = 0;
                        }
                        else if (hokenPartternUpdate.Kohi1Id != 0 && hokenParttern.Kohi1 != null && !hokenParttern.Kohi1.IsEmptyModel)
                        {
                            hokenPartternUpdate.HokenSbtCd = hokenParttern.HokenSbtCd;

                            var kohiUpdate = databasePtKohis.FirstOrDefault(c => c.HpId == hokenPartternUpdate.HpId && c.PtId == hokenPartternUpdate.PtId
                                                                            && c.HokenId == hokenPartternUpdate.Kohi1Id);
                            if (kohiUpdate != null)
                            {
                                kohiUpdate.FutansyaNo = hokenParttern.Kohi1.FutansyaNo;
                                kohiUpdate.JyukyusyaNo = hokenParttern.Kohi1.JyukyusyaNo;
                                kohiUpdate.TokusyuNo = hokenParttern.Kohi1.TokusyuNo;
                                kohiUpdate.SikakuDate = hokenParttern.Kohi1.SikakuDate;
                                kohiUpdate.KofuDate = hokenParttern.Kohi1.KofuDate;
                                kohiUpdate.Rate = hokenParttern.Kohi1.Rate;
                                kohiUpdate.StartDate = hokenParttern.Kohi1.StartDate;
                                kohiUpdate.EndDate = hokenParttern.Kohi1.EndDate == 0 ? defaultMaxDate : hokenParttern.Kohi1.EndDate;
                                kohiUpdate.GendoGaku = hokenParttern.Kohi1.GendoGaku;
                                kohiUpdate.HokenEdaNo = hokenParttern.Kohi1.HokenEdaNo;
                                kohiUpdate.HokenSbtKbn = hokenParttern.Kohi1.HokenSbtKbn;
                                kohiUpdate.Houbetu = hokenParttern.Kohi1.Houbetu;
                                kohiUpdate.UpdateId = TempIdentity.UserId;
                                kohiUpdate.UpdateDate = DateTime.UtcNow;
                            }
                        }

                        //HokenCheck
                        if (hokenParttern.Kohi1 != null)
                        {
                            if (hokenParttern.Kohi1.ConfirmDateList == null)
                                UpdateHokenCheck(databaseHokenChecks, new List<ConfirmDateModel>(), patientInfo.HpId, patientInfo.PtId, hokenPartternUpdate.Kohi1Id, TempIdentity.UserId, true).Wait();
                            else
                                UpdateHokenCheck(databaseHokenChecks, hokenParttern.Kohi1.ConfirmDateList, patientInfo.HpId, patientInfo.PtId, hokenPartternUpdate.Kohi1Id, TempIdentity.UserId, true).Wait();
                        }

                        #endregion

                        #region KohiId2
                        if (hokenPartternUpdate.Kohi2Id == 0 && hokenParttern.Kohi2 != null && !hokenParttern.Kohi2.IsEmptyModel) // add new
                        {
                            var kohi = Mapper.Map(hokenParttern.Kohi2, new PtKohi(), (source, dest) =>
                            {
                                dest.CreateId = TempIdentity.UserId;
                                dest.CreateDate = DateTime.UtcNow;
                                dest.UpdateMachine = TempIdentity.ComputerName;
                                dest.PtId = patientInfo.PtId;
                                dest.HpId = hpId;
                                dest.EndDate = source.EndDate == 0 ? defaultMaxDate : source.EndDate;
                                return dest;
                            });
                            _tenantTrackingDataContext.PtKohis.Add(kohi);
                            hokenPartternUpdate.Kohi2Id = kohi.HokenId;
                            hokenKohiIndex++;
                        }
                        else if (hokenPartternUpdate.Kohi2Id != 0 && hokenParttern.Kohi2 != null && hokenParttern.Kohi2.IsEmptyModel) //Case remove
                        {
                            var kohiRemove = databasePtKohis.FirstOrDefault(c => c.HpId == hokenPartternUpdate.HpId && c.PtId == hokenPartternUpdate.PtId
                                                                             && c.HokenId == hokenPartternUpdate.Kohi2Id);
                            if (kohiRemove != null)
                            {
                                kohiRemove.IsDeleted = 1;
                                kohiRemove.UpdateDate = DateTime.UtcNow;
                                kohiRemove.UpdateId = TempIdentity.UserId;
                            }
                            hokenPartternUpdate.Kohi2Id = 0;
                        }
                        else if (hokenPartternUpdate.Kohi2Id != 0 && hokenParttern.Kohi2 != null && !hokenParttern.Kohi2.IsEmptyModel)
                        {
                            hokenPartternUpdate.HokenSbtCd = hokenParttern.HokenSbtCd;

                            var kohiUpdate = databasePtKohis.FirstOrDefault(c => c.HpId == hokenPartternUpdate.HpId && c.PtId == hokenPartternUpdate.PtId
                                                                            && c.HokenId == hokenPartternUpdate.Kohi2Id);
                            if (kohiUpdate != null)
                            {
                                kohiUpdate.FutansyaNo = hokenParttern.Kohi2.FutansyaNo;
                                kohiUpdate.JyukyusyaNo = hokenParttern.Kohi2.JyukyusyaNo;
                                kohiUpdate.TokusyuNo = hokenParttern.Kohi2.TokusyuNo;
                                kohiUpdate.SikakuDate = hokenParttern.Kohi2.SikakuDate;
                                kohiUpdate.KofuDate = hokenParttern.Kohi2.KofuDate;
                                kohiUpdate.Rate = hokenParttern.Kohi2.Rate;
                                kohiUpdate.StartDate = hokenParttern.Kohi2.StartDate;
                                kohiUpdate.EndDate = hokenParttern.Kohi2.EndDate == 0 ? defaultMaxDate : hokenParttern.Kohi2.EndDate;
                                kohiUpdate.GendoGaku = hokenParttern.Kohi2.GendoGaku;
                                kohiUpdate.HokenEdaNo = hokenParttern.Kohi2.HokenEdaNo;
                                kohiUpdate.HokenSbtKbn = hokenParttern.Kohi2.HokenSbtKbn;
                                kohiUpdate.Houbetu = hokenParttern.Kohi2.Houbetu;
                                kohiUpdate.UpdateId = TempIdentity.UserId;
                                kohiUpdate.UpdateDate = DateTime.UtcNow;
                            }
                        }

                        //HokenCheck
                        if (hokenParttern.Kohi2 != null)
                        {
                            if (hokenParttern.Kohi2.ConfirmDateList == null)
                                UpdateHokenCheck(databaseHokenChecks, new List<ConfirmDateModel>(), patientInfo.HpId, patientInfo.PtId, hokenPartternUpdate.Kohi2Id, TempIdentity.UserId, true).Wait();
                            else
                                UpdateHokenCheck(databaseHokenChecks, hokenParttern.Kohi2.ConfirmDateList, patientInfo.HpId, patientInfo.PtId, hokenPartternUpdate.Kohi2Id, TempIdentity.UserId, true).Wait();
                        }

                        #endregion

                        #region KohiId3
                        if (hokenPartternUpdate.Kohi3Id == 0 && hokenParttern.Kohi3 != null && !hokenParttern.Kohi3.IsEmptyModel) // add new
                        {
                            var kohi = Mapper.Map(hokenParttern.Kohi3, new PtKohi(), (source, dest) =>
                            {
                                dest.CreateId = TempIdentity.UserId;
                                dest.CreateDate = DateTime.UtcNow;
                                dest.UpdateMachine = TempIdentity.ComputerName;
                                dest.PtId = patientInfo.PtId;
                                dest.HpId = hpId;
                                dest.EndDate = source.EndDate == 0 ? defaultMaxDate : source.EndDate;
                                return dest;
                            });
                            _tenantTrackingDataContext.PtKohis.Add(kohi);
                            hokenPartternUpdate.Kohi3Id = kohi.HokenId;
                            hokenKohiIndex++;
                        }
                        else if (hokenPartternUpdate.Kohi3Id != 0 && hokenParttern.Kohi3 != null && hokenParttern.Kohi3.IsEmptyModel) //Case remove
                        {
                            var kohiRemove = databasePtKohis.FirstOrDefault(c => c.HpId == hokenPartternUpdate.HpId && c.PtId == hokenPartternUpdate.PtId
                                                                            && c.HokenId == hokenPartternUpdate.Kohi3Id);
                            if (kohiRemove != null)
                            {
                                kohiRemove.IsDeleted = 1;
                                kohiRemove.UpdateDate = DateTime.UtcNow;
                                kohiRemove.UpdateId = TempIdentity.UserId;
                            }
                            hokenPartternUpdate.Kohi3Id = 0;
                        }
                        else if (hokenPartternUpdate.Kohi3Id != 0 && hokenParttern.Kohi3 != null && !hokenParttern.Kohi3.IsEmptyModel)
                        {
                            hokenPartternUpdate.HokenSbtCd = hokenParttern.HokenSbtCd;

                            var kohiUpdate = databasePtKohis.FirstOrDefault(c => c.HpId == hokenPartternUpdate.HpId && c.PtId == hokenPartternUpdate.PtId
                                                                            && c.HokenId == hokenPartternUpdate.Kohi3Id);
                            if (kohiUpdate != null)
                            {
                                kohiUpdate.FutansyaNo = hokenParttern.Kohi3.FutansyaNo;
                                kohiUpdate.JyukyusyaNo = hokenParttern.Kohi3.JyukyusyaNo;
                                kohiUpdate.TokusyuNo = hokenParttern.Kohi3.TokusyuNo;
                                kohiUpdate.SikakuDate = hokenParttern.Kohi3.SikakuDate;
                                kohiUpdate.KofuDate = hokenParttern.Kohi3.KofuDate;
                                kohiUpdate.Rate = hokenParttern.Kohi3.Rate;
                                kohiUpdate.StartDate = hokenParttern.Kohi3.StartDate;
                                kohiUpdate.EndDate = hokenParttern.Kohi3.EndDate == 0 ? defaultMaxDate : hokenParttern.Kohi3.EndDate;
                                kohiUpdate.GendoGaku = hokenParttern.Kohi3.GendoGaku;
                                kohiUpdate.HokenEdaNo = hokenParttern.Kohi3.HokenEdaNo;
                                kohiUpdate.HokenSbtKbn = hokenParttern.Kohi3.HokenSbtKbn;
                                kohiUpdate.Houbetu = hokenParttern.Kohi3.Houbetu;
                                kohiUpdate.UpdateId = TempIdentity.UserId;
                                kohiUpdate.UpdateDate = DateTime.UtcNow;
                            }
                        }

                        //HokenCheck

                        if (hokenParttern.Kohi3 != null)
                        {
                            if (hokenParttern.Kohi3.ConfirmDateList == null)
                                UpdateHokenCheck(databaseHokenChecks, new List<ConfirmDateModel>(), patientInfo.HpId, patientInfo.PtId, hokenPartternUpdate.Kohi3Id, TempIdentity.UserId, true).Wait();
                            else
                                UpdateHokenCheck(databaseHokenChecks, hokenParttern.Kohi3.ConfirmDateList, patientInfo.HpId, patientInfo.PtId, hokenPartternUpdate.Kohi3Id, TempIdentity.UserId, true).Wait();
                        }

                        #endregion

                        #region KohiId4
                        if (hokenPartternUpdate.Kohi4Id == 0 && hokenParttern.Kohi4 != null && !hokenParttern.Kohi4.IsEmptyModel) // add new
                        {
                            var kohi = Mapper.Map(hokenParttern.Kohi4, new PtKohi(), (source, dest) =>
                            {
                                dest.CreateId = TempIdentity.UserId;
                                dest.CreateDate = DateTime.UtcNow;
                                dest.UpdateMachine = TempIdentity.ComputerName;
                                dest.PtId = patientInfo.PtId;
                                dest.HpId = hpId;
                                dest.HokenId = hokenKohiIndex;
                                dest.EndDate = source.EndDate == 0 ? defaultMaxDate : source.EndDate;
                                return dest;
                            });
                            _tenantTrackingDataContext.PtKohis.Add(kohi);
                            hokenPartternUpdate.Kohi4Id = kohi.HokenId;
                            hokenKohiIndex++;
                        }
                        else if (hokenPartternUpdate.Kohi4Id != 0 && hokenParttern.Kohi4 != null && hokenParttern.Kohi4.IsEmptyModel) //Case remove
                        {
                            var kohiRemove = databasePtKohis.FirstOrDefault(c => c.HpId == hokenPartternUpdate.HpId && c.PtId == hokenPartternUpdate.PtId
                                                                            && c.HokenId == hokenPartternUpdate.Kohi4Id);
                            if (kohiRemove != null)
                            {
                                kohiRemove.IsDeleted = 1;
                                kohiRemove.UpdateDate = DateTime.UtcNow;
                                kohiRemove.UpdateId = TempIdentity.UserId;
                            }
                            hokenPartternUpdate.Kohi4Id = 0;
                        }
                        else if (hokenPartternUpdate.Kohi4Id != 0 && hokenParttern.Kohi4 != null && !hokenParttern.Kohi4.IsEmptyModel)
                        {
                            hokenPartternUpdate.HokenSbtCd = hokenParttern.HokenSbtCd;

                            var kohiUpdate = databasePtKohis.FirstOrDefault(c => c.HpId == hokenPartternUpdate.HpId && c.PtId == hokenPartternUpdate.PtId
                                                                            && c.HokenId == hokenPartternUpdate.Kohi4Id);
                            if (kohiUpdate != null)
                            {
                                kohiUpdate.FutansyaNo = hokenParttern.Kohi4.FutansyaNo;
                                kohiUpdate.JyukyusyaNo = hokenParttern.Kohi4.JyukyusyaNo;
                                kohiUpdate.TokusyuNo = hokenParttern.Kohi4.TokusyuNo;
                                kohiUpdate.SikakuDate = hokenParttern.Kohi4.SikakuDate;
                                kohiUpdate.KofuDate = hokenParttern.Kohi4.KofuDate;
                                kohiUpdate.Rate = hokenParttern.Kohi4.Rate;
                                kohiUpdate.StartDate = hokenParttern.Kohi4.StartDate;
                                kohiUpdate.EndDate = hokenParttern.Kohi4.EndDate == 0 ? defaultMaxDate : hokenParttern.Kohi4.EndDate;
                                kohiUpdate.GendoGaku = hokenParttern.Kohi4.GendoGaku;
                                kohiUpdate.HokenEdaNo = hokenParttern.Kohi4.HokenEdaNo;
                                kohiUpdate.HokenSbtKbn = hokenParttern.Kohi4.HokenSbtKbn;
                                kohiUpdate.Houbetu = hokenParttern.Kohi4.Houbetu;
                                kohiUpdate.UpdateId = TempIdentity.UserId;
                                kohiUpdate.UpdateDate = DateTime.UtcNow;
                            }
                        }

                        //HokenCheck

                        if (hokenParttern.Kohi4 != null)
                        {
                            if (hokenParttern.Kohi4.ConfirmDateList == null)
                                UpdateHokenCheck(databaseHokenChecks, new List<ConfirmDateModel>(), patientInfo.HpId, patientInfo.PtId, hokenPartternUpdate.Kohi4Id, TempIdentity.UserId, true).Wait();
                            else
                                UpdateHokenCheck(databaseHokenChecks, hokenParttern.Kohi4.ConfirmDateList, patientInfo.HpId, patientInfo.PtId, hokenPartternUpdate.Kohi4Id, TempIdentity.UserId, true).Wait();
                        }

                        #endregion

                        hokenPartternUpdate.StartDate = hokenParttern.StartDate;
                        hokenPartternUpdate.EndDate = hokenParttern.EndDate;
                        hokenPartternUpdate.UpdateDate = DateTime.UtcNow;
                        hokenPartternUpdate.UpdateId = TempIdentity.UserId;
                    };

                    int hokenParternHokenId = 0;
                    if (hokenPartternUpdate != null)
                        hokenParternHokenId = hokenPartternUpdate.HokenId;

                    var hokenInfUpdate = databaseHoKentInfs.FirstOrDefault(c => c.HpId == hpId && c.PtId == patientInfo.PtId
                                                                        && c.HokenId == hokenParternHokenId);
                    if (hokenInfUpdate != null)
                    {
                        if (hokenParttern.HokenInf != null)
                        {
                            hokenInfUpdate.HokenNo = hokenParttern.HokenInf.HokenNo;
                            hokenInfUpdate.HokensyaNo = hokenParttern.HokenInf.HokensyaNo;
                            hokenInfUpdate.Kigo = hokenParttern.HokenInf.Kigo;
                            hokenInfUpdate.Bango = hokenParttern.HokenInf.Bango;
                            hokenInfUpdate.HonkeKbn = hokenParttern.HokenInf.HonkeKbn;
                            hokenInfUpdate.HokenKbn = hokenParttern.HokenInf.HokenKbn;
                            hokenInfUpdate.Houbetu = hokenParttern.HokenInf.Houbetu;
                            hokenInfUpdate.HokensyaNo = hokenParttern.HokenInf.HokensyaNo;
                            hokenInfUpdate.KofuDate = hokenParttern.HokenInf.KofuDate;
                            hokenInfUpdate.StartDate = hokenParttern.HokenInf.StartDate;
                            hokenInfUpdate.EndDate = hokenParttern.HokenInf.EndDate == 0 ? defaultMaxDate : hokenParttern.HokenInf.EndDate;
                            hokenInfUpdate.RyoyoStartDate = hokenParttern.HokenInf.RyoyoStartDate;
                            hokenInfUpdate.RyoyoEndDate = hokenParttern.HokenInf.RyoyoEndDate == 0 ? defaultMaxDate : hokenParttern.HokenInf.RyoyoEndDate;
                            hokenInfUpdate.KeizokuKbn = hokenParttern.HokenInf.KeizokuKbn;
                            hokenInfUpdate.KogakuKbn = hokenParttern.HokenInf.KogakuKbn;
                            hokenInfUpdate.TokureiYm1 = hokenParttern.HokenInf.TokureiYm1;
                            hokenInfUpdate.TokureiYm2 = hokenParttern.HokenInf.TokureiYm2;
                            hokenInfUpdate.TasukaiYm = hokenParttern.HokenInf.TasukaiYm;
                            hokenInfUpdate.SyokumuKbn = hokenParttern.HokenInf.SyokumuKbn;
                            hokenInfUpdate.GenmenKbn = hokenParttern.HokenInf.GenmenKbn;
                            hokenInfUpdate.GenmenRate = hokenParttern.HokenInf.GenmenRate;
                            hokenInfUpdate.GenmenGaku = hokenParttern.HokenInf.GenmenGaku;
                            hokenInfUpdate.Tokki1 = hokenParttern.HokenInf.Tokki1;
                            hokenInfUpdate.Tokki2 = hokenParttern.HokenInf.Tokki2;
                            hokenInfUpdate.Tokki3 = hokenParttern.HokenInf.Tokki3;
                            hokenInfUpdate.Tokki4 = hokenParttern.HokenInf.Tokki4;
                            hokenInfUpdate.Tokki5 = hokenParttern.HokenInf.Tokki5;
                            hokenInfUpdate.RousaiKofuNo = hokenParttern.HokenInf.RousaiKofuNo;
                            hokenInfUpdate.RousaiSaigaiKbn = hokenParttern.HokenInf.RousaiSaigaiKbn;
                            hokenInfUpdate.RousaiJigyosyoName = hokenParttern.HokenInf.RousaiJigyosyoName;
                            hokenInfUpdate.RousaiPrefName = hokenParttern.HokenInf.RousaiPrefName;
                            hokenInfUpdate.RousaiCityName = hokenParttern.HokenInf.RousaiCityName;
                            hokenInfUpdate.RousaiSyobyoDate = hokenParttern.HokenInf.RousaiSyobyoDate;
                            hokenInfUpdate.RousaiSyobyoCd = hokenParttern.HokenInf.RousaiSyobyoCd;
                            hokenInfUpdate.RousaiRoudouCd = hokenParttern.HokenInf.RousaiRoudouCd;
                            hokenInfUpdate.RousaiKantokuCd = hokenParttern.HokenInf.RousaiKantokuCd;
                            hokenInfUpdate.RousaiReceCount = hokenParttern.HokenInf.RousaiReceCount;
                            hokenInfUpdate.JibaiHokenName = hokenParttern.HokenInf.JibaiHokenName;
                            hokenInfUpdate.JibaiHokenTanto = hokenParttern.HokenInf.JibaiHokenTanto;
                            hokenInfUpdate.JibaiHokenTel = hokenParttern.HokenInf.JibaiHokenTel;
                            hokenInfUpdate.JibaiJyusyouDate = hokenParttern.HokenInf.JibaiJyusyouDate;
                            hokenInfUpdate.SikakuDate = hokenParttern.HokenInf.SikakuDate;
                            hokenInfUpdate.EdaNo = hokenParttern.HokenInf.EdaNo;
                            hokenInfUpdate.KeizokuKbn = hokenParttern.HokenInf.KeizokuKbn;

                            if (hokenPartternUpdate != null && hokenParttern.HokenInf.ConfirmDateList != null)
                                UpdateHokenCheck(databaseHokenChecks, hokenParttern.HokenInf.ConfirmDateList, patientInfo.HpId, patientInfo.PtId, hokenPartternUpdate.HokenId, TempIdentity.UserId);



                            var listAddTenki = Mapper.Map<RousaiTenkiModel, PtRousaiTenki>(hokenParttern.HokenInf.ListRousaiTenki.Where(x => x.SeqNo == 0), (src, dest) =>
                            {
                                dest.CreateId = TempIdentity.UserId;
                                dest.PtId = patientInfo.PtId;
                                dest.HpId = hpId;
                                dest.HokenId = hokenInfUpdate.HokenId;
                                dest.UpdateMachine = TempIdentity.ComputerName;
                                dest.CreateId = TempIdentity.UserId;
                                dest.CreateDate = DateTime.UtcNow;
                                dest.UpdateDate = DateTime.UtcNow;
                                return dest;
                            });
                            _tenantTrackingDataContext.PtRousaiTenkis.AddRange(listAddTenki);


                            foreach (var rsTkUpdate in hokenParttern.HokenInf.ListRousaiTenki.Where(x => x.SeqNo != 0))
                            {
                                var updateItem = databasePtRousaiTenkis.FirstOrDefault(x => x.HokenId == hokenInfUpdate.HokenId && x.SeqNo == rsTkUpdate.SeqNo);
                                if (updateItem != null)
                                {
                                    updateItem.Sinkei = rsTkUpdate.RousaiTenkiSinkei;
                                    updateItem.Tenki = rsTkUpdate.RousaiTenkiTenki;
                                    updateItem.EndDate = rsTkUpdate.RousaiTenkiEndDate;
                                }
                            }

                            var listDatabaseByHokenInf = databasePtRousaiTenkis.Where(x => x.HokenId == hokenInfUpdate.HokenId);
                            var listRemoves = listDatabaseByHokenInf.Where(x => !hokenParttern.HokenInf.ListRousaiTenki.Any(m => m.SeqNo == x.SeqNo)).ToList();

                            listRemoves.ForEach(x =>
                            {
                                x.IsDeleted = 1;
                                x.UpdateId = TempIdentity.UserId;
                                x.UpdateDate = DateTime.UtcNow;
                            });
                        }
                    }

                    if (hokenPartternUpdate != null)
                        _tenantTrackingDataContext.PtHokenPatterns.Update(hokenPartternUpdate);
                }
                else //Add Entity
                {
                    var hokenModel = Mapper.Map(hokenParttern, new PtHokenPattern(), (source, dest) =>
                    {
                        dest.CreateId = TempIdentity.UserId;
                        dest.CreateDate = DateTime.UtcNow;
                        dest.UpdateMachine = TempIdentity.ComputerName;
                        dest.PtId = patientInfo.PtId;
                        dest.HpId = hpId;
                        return dest;
                    });

                    hokenModel.HokenPid = hoKenIndex;
                    hokenModel.HokenId = hoKenIndex;
                    hokenModel.EndDate = hokenModel.EndDate == 0 ? defaultMaxDate : hokenModel.EndDate;

                    var hokenInfModel = Mapper.Map(hokenParttern.HokenInf, new PtHokenInf(), (source, dest) =>
                    {
                        dest.CreateId = TempIdentity.UserId;
                        dest.CreateDate = DateTime.UtcNow;
                        dest.UpdateMachine = TempIdentity.ComputerName;
                        dest.PtId = patientInfo.PtId;
                        dest.HpId = hpId;
                        return dest;
                    });

                    hokenInfModel.HokenId = hoKenIndex;
                    hokenInfModel.EndDate = hokenInfModel.EndDate == 0 ? defaultMaxDate : hokenInfModel.EndDate;
                    _tenantTrackingDataContext.Add(hokenInfModel);

                    if (hokenParttern.HokenInf != null && hokenParttern.HokenInf.ListRousaiTenki.Any())
                    {
                        var listAddTenki = Mapper.Map<RousaiTenkiModel, PtRousaiTenki>(hokenParttern.HokenInf.ListRousaiTenki, (src, dest) =>
                        {
                            dest.CreateId = TempIdentity.UserId;
                            dest.PtId = patientInfo.PtId;
                            dest.Tenki = src.RousaiTenkiTenki;
                            dest.Sinkei = src.RousaiTenkiSinkei;
                            dest.EndDate = src.RousaiTenkiEndDate;
                            dest.HpId = hpId;
                            dest.HokenId = hokenInfModel.HokenId;
                            dest.UpdateMachine = TempIdentity.ComputerName;
                            dest.CreateId = TempIdentity.UserId;
                            dest.CreateDate = DateTime.UtcNow;
                            dest.UpdateDate = DateTime.UtcNow;
                            return dest;
                        });
                        _tenantTrackingDataContext.PtRousaiTenkis.AddRange(listAddTenki);
                    }

                    if (hokenParttern.HokenInf != null && hokenParttern.HokenInf.ConfirmDateList != null && hokenParttern.HokenInf.ConfirmDateList.Any())
                    {
                        foreach (var item in hokenParttern.HokenInf.ConfirmDateList)
                        {
                            PtHokenCheck addPtHokenCheck = Mapper.Map(item, new PtHokenCheck(), (source, dest) =>
                            {
                                dest.CheckCmt = source.CheckComment;
                                dest.CheckDate = DateTime.SpecifyKind(CIUtil.IntToDate(source.ConfirmDate), DateTimeKind.Utc);
                                dest.CreateId = TempIdentity.UserId;
                                dest.CreateDate = DateTime.UtcNow;
                                dest.UpdateMachine = TempIdentity.ComputerName;
                                dest.HokenId = hokenModel.HokenId;
                                dest.CheckId = TempIdentity.UserId;
                                dest.PtID = patientInfo.PtId;
                                dest.HokenGrp = 1;
                                dest.HpId = hpId;
                                return dest;
                            });
                            _tenantTrackingDataContext.Add(addPtHokenCheck);
                        }
                    }

                    if (hokenParttern.Kohi1 != null && !hokenParttern.Kohi1.IsEmptyModel)
                    {
                        var kohi = Mapper.Map(hokenParttern.Kohi1, new PtKohi(), (source, dest) =>
                        {
                            dest.CreateId = TempIdentity.UserId;
                            dest.CreateDate = DateTime.UtcNow;
                            dest.UpdateMachine = TempIdentity.ComputerName;
                            dest.PtId = patientInfo.PtId;
                            dest.HpId = hpId;
                            dest.EndDate = source.EndDate == 0 ? defaultMaxDate : source.EndDate;
                            return dest;
                        });

                        _tenantTrackingDataContext.PtKohis.Add(kohi);
                        hokenModel.Kohi1Id = kohi.HokenId;
                        hokenKohiIndex++;

                        if (hokenParttern.Kohi1.ConfirmDateList != null && hokenParttern.Kohi1.ConfirmDateList.Any())
                        {
                            var listAddHokenCheck = Mapper.Map<ConfirmDateModel, PtHokenCheck>(hokenParttern.Kohi1.ConfirmDateList, (src, dest) =>
                            {
                                dest.CheckCmt = src.CheckComment;
                                dest.CheckDate = DateTime.SpecifyKind(CIUtil.IntToDate(src.ConfirmDate), DateTimeKind.Utc);
                                dest.CreateDate = DateTime.UtcNow;
                                dest.CreateId = TempIdentity.UserId;
                                dest.UpdateMachine = TempIdentity.ComputerName;
                                dest.HpId = hpId;
                                dest.PtID = patientInfo.PtId;
                                dest.HokenGrp = 2;
                                dest.HokenId = kohi.HokenId;
                                dest.CheckId = TempIdentity.UserId;
                                return dest;
                            });
                            _tenantTrackingDataContext.PtHokenChecks.AddRange(listAddHokenCheck);
                        }

                    }

                    if (hokenParttern.Kohi2 != null && !hokenParttern.Kohi2.IsEmptyModel)
                    {
                        var kohi = Mapper.Map(hokenParttern.Kohi2, new PtKohi(), (source, dest) =>
                        {
                            dest.CreateId = TempIdentity.UserId;
                            dest.CreateDate = DateTime.UtcNow;
                            dest.UpdateMachine = TempIdentity.ComputerName;
                            dest.PtId = patientInfo.PtId;
                            dest.HpId = hpId;
                            dest.EndDate = source.EndDate == 0 ? defaultMaxDate : source.EndDate;
                            return dest;
                        });

                        _tenantTrackingDataContext.PtKohis.Add(kohi);
                        hokenModel.Kohi2Id = kohi.HokenId;
                        hokenKohiIndex++;

                        if (hokenParttern.Kohi2.ConfirmDateList != null && hokenParttern.Kohi2.ConfirmDateList.Any())
                        {
                            var listAddHokenCheck = Mapper.Map<ConfirmDateModel, PtHokenCheck>(hokenParttern.Kohi2.ConfirmDateList, (src, dest) =>
                            {
                                dest.CheckCmt = src.CheckComment;
                                dest.CheckDate = DateTime.SpecifyKind(CIUtil.IntToDate(src.ConfirmDate), DateTimeKind.Utc);
                                dest.CreateDate = DateTime.UtcNow;
                                dest.CreateId = TempIdentity.UserId;
                                dest.UpdateMachine = TempIdentity.ComputerName;
                                dest.HpId = hpId;
                                dest.PtID = patientInfo.PtId;
                                dest.HokenGrp = 2;
                                dest.HokenId = kohi.HokenId;
                                dest.CheckId = TempIdentity.UserId;
                                return dest;
                            });
                            _tenantTrackingDataContext.PtHokenChecks.AddRange(listAddHokenCheck);
                        }
                    }

                    if (hokenParttern.Kohi3 != null && !hokenParttern.Kohi3.IsEmptyModel)
                    {
                        var kohi = Mapper.Map(hokenParttern.Kohi3, new PtKohi(), (source, dest) =>
                        {
                            dest.CreateId = TempIdentity.UserId;
                            dest.CreateDate = DateTime.UtcNow;
                            dest.UpdateMachine = TempIdentity.ComputerName;
                            dest.PtId = patientInfo.PtId;
                            dest.HpId = hpId;
                            dest.EndDate = source.EndDate == 0 ? defaultMaxDate : source.EndDate;
                            return dest;
                        });

                        _tenantTrackingDataContext.PtKohis.Add(kohi);
                        hokenModel.Kohi3Id = kohi.HokenId;
                        hokenKohiIndex++;

                        if (hokenParttern.Kohi3.ConfirmDateList != null && hokenParttern.Kohi3.ConfirmDateList.Any())
                        {
                            var listAddHokenCheck = Mapper.Map<ConfirmDateModel, PtHokenCheck>(hokenParttern.Kohi3.ConfirmDateList, (src, dest) =>
                            {
                                dest.CheckCmt = src.CheckComment;
                                dest.CheckDate = DateTime.SpecifyKind(CIUtil.IntToDate(src.ConfirmDate), DateTimeKind.Utc);
                                dest.CreateDate = DateTime.UtcNow;
                                dest.CreateId = TempIdentity.UserId;
                                dest.UpdateMachine = TempIdentity.ComputerName;
                                dest.HpId = hpId;
                                dest.PtID = patientInfo.PtId;
                                dest.HokenGrp = 2;
                                dest.HokenId = kohi.HokenId;
                                dest.CheckId = TempIdentity.UserId;
                                return dest;
                            });
                            _tenantTrackingDataContext.PtHokenChecks.AddRange(listAddHokenCheck);
                        }
                    }

                    if (hokenParttern.Kohi4 != null && !hokenParttern.Kohi4.IsEmptyModel)
                    {
                        var kohi = Mapper.Map(hokenParttern.Kohi4, new PtKohi(), (source, dest) =>
                        {
                            dest.CreateId = TempIdentity.UserId;
                            dest.CreateDate = DateTime.UtcNow;
                            dest.PtId = patientInfo.PtId;
                            dest.HpId = hpId;
                            dest.EndDate = source.EndDate == 0 ? defaultMaxDate : source.EndDate;
                            dest.UpdateMachine = TempIdentity.ComputerName;
                            return dest;
                        });

                        _tenantTrackingDataContext.PtKohis.Add(kohi);
                        hokenModel.Kohi4Id = kohi.HokenId;
                        hokenKohiIndex++;

                        if (hokenParttern.Kohi4.ConfirmDateList != null && hokenParttern.Kohi4.ConfirmDateList.Any())
                        {
                            var listAddHokenCheck = Mapper.Map<ConfirmDateModel, PtHokenCheck>(hokenParttern.Kohi4.ConfirmDateList, (src, dest) =>
                            {
                                dest.CheckCmt = src.CheckComment;
                                dest.CheckDate = DateTime.SpecifyKind(CIUtil.IntToDate(src.ConfirmDate), DateTimeKind.Utc);
                                dest.CreateDate = DateTime.UtcNow;
                                dest.CreateId = TempIdentity.UserId;
                                dest.HpId = hpId;
                                dest.PtID = patientInfo.PtId;
                                dest.HokenGrp = 2;
                                dest.HokenId = kohi.HokenId;
                                dest.CheckId = TempIdentity.UserId;
                                dest.UpdateMachine = TempIdentity.ComputerName;
                                return dest;
                            });
                            _tenantTrackingDataContext.PtHokenChecks.AddRange(listAddHokenCheck);
                        }
                    }

                    _tenantTrackingDataContext.PtHokenPatterns.Add(hokenModel);
                    hoKenIndex++;
                }
            }
            #endregion
            return _tenantTrackingDataContext.SaveChanges() > 0;
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
            systemConf = _tenantDataContext.SystemConfs.FirstOrDefault(p =>
                    p.HpId == hpId && p.GrpCd == groupCd && p.GrpEdaNo == grpEdaNo);
            return systemConf != null ? systemConf.Val : defaultValue;
        }


        private long GetAutoPtNum(long startValue, int hpId)
        {
            int autoSetting = (int)GetSettingValue(1014, hpId, 0);
            var ptNumExisting = _tenantDataContext.PtInfs.FirstOrDefault
                (ptInf => (autoSetting != 1 ? true : ptInf.IsDelete == 0) && ptInf.PtNum == startValue);
            if (ptNumExisting == null)
            {
                return startValue;
            }

            var ptList = _tenantDataContext.PtInfs.Where(ptInf => (autoSetting != 1 ? true : ptInf.IsDelete == 0) && ptInf.PtNum >= startValue)
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

        private Task UpdateHokenCheck(List<PtHokenCheck> databaseList, List<ConfirmDateModel> savingList, int hpId, long ptId, int hokenId, int actUserId, bool hokenKohi = false)
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
                _tenantTrackingDataContext.PtHokenChecks.Update(deleteItem);
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
                _tenantTrackingDataContext.PtHokenChecks.Add(addedHokenCheck);
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
                    _tenantTrackingDataContext.PtHokenChecks.Update(modelUpdate);
                }
            }
            return Task.CompletedTask;
        }

        public bool DeletePatientInfo(long ptId, int hpId = TempIdentity.HpId)
        {
            var patientInf = _tenantTrackingDataContext.PtInfs.FirstOrDefault(x => x.PtId == ptId && x.HpId == hpId && x.IsDelete == DeleteTypes.None);
            if (patientInf != null)
            {
                patientInf.IsDelete = DeleteTypes.Deleted;
                patientInf.UpdateDate = DateTime.UtcNow;
                patientInf.UpdateId = TempIdentity.UserId;
                patientInf.UpdateMachine = TempIdentity.ComputerName;
                #region PtMemo
                var ptMemos = _tenantTrackingDataContext.PtMemos.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == DeleteTypes.None).ToList();
                foreach (var item in ptMemos)
                {
                    item.IsDeleted = DeleteTypes.Deleted;
                    item.UpdateDate = DateTime.UtcNow;
                    item.UpdateId = TempIdentity.UserId;
                    item.UpdateMachine = TempIdentity.ComputerName;
                }
                #endregion

                #region ptKyuseis
                var ptKyuseis = _tenantTrackingDataContext.PtKyuseis.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == DeleteTypes.None).ToList();
                ptKyuseis.ForEach(x =>
                {
                    x.IsDeleted = DeleteTypes.Deleted;
                    x.UpdateId = TempIdentity.UserId;
                    x.UpdateDate = DateTime.UtcNow;
                    x.UpdateMachine = TempIdentity.ComputerName;
                });
                #endregion

                #region ptSanteis
                var ptSanteis = _tenantTrackingDataContext.PtSanteiConfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == DeleteTypes.None).ToList();
                ptSanteis.ForEach(x =>
                {
                    x.IsDeleted = DeleteTypes.Deleted;
                    x.UpdateId = TempIdentity.UserId;
                    x.UpdateDate = DateTime.UtcNow;
                    x.UpdateMachine = TempIdentity.ComputerName;
                });
                #endregion

                #region HokenParttern
                var ptHokenParterns = _tenantTrackingDataContext.PtHokenPatterns.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == DeleteTypes.None).ToList();
                ptHokenParterns.ForEach(x =>
                {
                    x.IsDeleted = DeleteTypes.Deleted;
                    x.UpdateId = TempIdentity.UserId;
                    x.UpdateDate = DateTime.UtcNow;
                    x.UpdateMachine = TempIdentity.ComputerName;
                });
                #endregion

                #region HokenInf
                var ptHokenInfs = _tenantTrackingDataContext.PtHokenInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == DeleteTypes.None).ToList();
                ptHokenInfs.ForEach(x =>
                {
                    x.IsDeleted = DeleteTypes.Deleted;
                    x.UpdateId = TempIdentity.UserId;
                    x.UpdateDate = DateTime.UtcNow;
                    x.UpdateMachine = TempIdentity.ComputerName;
                });
                #endregion

                #region HokenKohi
                var ptHokenKohis = _tenantTrackingDataContext.PtKohis.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == DeleteTypes.None).ToList();
                ptHokenKohis.ForEach(x =>
                {
                    x.IsDeleted = DeleteTypes.Deleted;
                    x.UpdateDate = DateTime.UtcNow;
                    x.UpdateId = TempIdentity.UserId;
                    x.UpdateMachine = TempIdentity.ComputerName;
                });
                #endregion

                #region HokenCheck
                var ptHokenChecks = _tenantTrackingDataContext.PtHokenChecks.Where(x => x.HpId == hpId && x.PtID == ptId && x.IsDeleted == DeleteTypes.None).ToList();
                ptHokenChecks.ForEach(x =>
                {
                    x.IsDeleted = DeleteTypes.Deleted;
                    x.UpdateId = TempIdentity.UserId;
                    x.UpdateDate = DateTime.UtcNow;
                    x.UpdateMachine = TempIdentity.ComputerName;
                });
                #endregion

                #region RousaiTenki
                var ptRousaiTenkies = _tenantTrackingDataContext.PtRousaiTenkis.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == DeleteTypes.None).ToList();
                ptRousaiTenkies.ForEach(x =>
                {
                    x.IsDeleted = DeleteTypes.Deleted;
                    x.UpdateId = TempIdentity.UserId;
                    x.UpdateDate = DateTime.UtcNow;
                    x.UpdateMachine = TempIdentity.ComputerName;
                });
                #endregion
            }
            return _tenantTrackingDataContext.SaveChanges() > 0;
        }
    }
}