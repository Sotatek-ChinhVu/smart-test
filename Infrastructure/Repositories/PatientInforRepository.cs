using Domain.Models.PatientInfor;
using Domain.Models.PatientInfor.Domain.Models.PatientInfor;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class PatientInforRepository : IPatientInforRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        public PatientInforRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
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
                .FirstOrDefault(x => x.HpId == hpId & x.PtId == ptId & x.IsDeleted == 0);
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
            var result = new List<PatientInforModel>();
            for (long i = ptNum; i < (ptNum + pageSize); i++)
            {
                var CheckExistPtNum = _tenantDataContext.PtInfs.FirstOrDefault(p => p.PtNum == ptNum);

                if (CheckExistPtNum == null)

                    result.Add(new PatientInforModel(hpId, i, "(空き) " + i, "(空き) " + i));
                else
                    result.Add(new PatientInforModel(hpId, i, CheckExistPtNum.KanaName, CheckExistPtNum.Name));

                ptNum += 1;
            }

            return result;
        }
    }
}