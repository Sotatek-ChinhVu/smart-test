using Domain.Constant;
using Domain.Models.CalculationInf;
using Domain.Models.GroupInf;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Domain.Models.InsuranceMst;
using Domain.Models.MaxMoney;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Enum;
using Helper.Extension;
using Helper.Mapping;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using HokenInfModel = Domain.Models.Insurance.HokenInfModel;

namespace Infrastructure.Repositories
{
    public class PatientInforRepository : RepositoryBase, IPatientInforRepository
    {
        private const string startGroupOrderKey = "group_";
        private readonly IReceptionRepository _receptionRepository;
        public PatientInforRepository(ITenantProvider tenantProvider, IReceptionRepository receptionRepository) : base(tenantProvider)
        {
            _receptionRepository = receptionRepository;
        }

        (PatientInforModel ptInfModel, bool isFound) IPatientInforRepository.SearchExactlyPtNum(long ptNum, int hpId, int sinDate)
        {
            var ptInf = NoTrackingDataContext.PtInfs.Where(x => x.PtNum == ptNum && x.IsDelete == 0).FirstOrDefault();
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
            PatientInforModel ptInfModel = ToModel(ptInf, memo, lastVisitDate, sinDate);

            return new(ptInfModel, true);
        }

        public List<PatientInforModel> SearchContainPtNum(int ptNum, string keyword, int hpId, int pageIndex, int pageSize, Dictionary<string, string> sortData)
        {
            List<PatientInforModel> result = new();
            var ptInfWithLastVisitDate =
                from p in NoTrackingDataContext.PtInfs
                where p.IsDelete == 0 && (p.PtNum == ptNum || (p.KanaName != null && p.KanaName.Contains(keyword)) || (p.Name != null && p.Name.Contains(keyword)))
                orderby p.PtNum descending
                select new PatientInfQueryModel
                {
                    PtInf = p,
                    LastVisitDate = (
                        from r in NoTrackingDataContext.RaiinInfs
                        where r.HpId == hpId
                            && r.PtId == p.PtId
                            && r.Status >= RaiinState.TempSave
                            && r.IsDeleted == DeleteTypes.None
                        orderby r.SinDate descending
                        select r.SinDate
                    ).FirstOrDefault()
                };
            bool sortGroup = sortData.Select(item => item.Key).ToList().Exists(item => item.StartsWith(startGroupOrderKey));
            result = sortGroup
                         ?
                         ptInfWithLastVisitDate
                         .AsEnumerable()
                         .Select(p => ToModel(p.PtInf, string.Empty, p.LastVisitDate))
                         .ToList()
                         :
                         SortData(ptInfWithLastVisitDate, sortData, pageIndex, pageSize);
            return result;
        }

        public PatientInforModel? GetById(int hpId, long ptId, int sinDate, long raiinNo , bool isShowKyuSeiName = false, List<int>? listStatus = null)
        {
            var itemData = NoTrackingDataContext.PtInfs.FirstOrDefault(x => x.HpId == hpId && x.PtId == ptId);


            // Raiin Count
            // status = RaiinState Receptionist
            int raiinCount = NoTrackingDataContext.RaiinInfs.Count(u => u.HpId == hpId &&
                                                                        u.SinDate == sinDate &&
                                                                        u.RaiinNo != raiinNo &&
                                                                        u.IsDeleted == DeleteTypes.None &&
                                                                        (listStatus != null ? listStatus.Contains(u.Status) : u.Status == 1));
            if (itemData == null)
            {
                return new PatientInforModel(raiinCount);
            }

            //Get ptMemo
            string memo = string.Empty;
            PtMemo? ptMemo = NoTrackingDataContext.PtMemos.FirstOrDefault(x => x.PtId == itemData.PtId);
            if (ptMemo != null)
            {
                memo = ptMemo.Memo ?? string.Empty;
            }

            //Get lastVisitDate
            int lastVisitDate = _receptionRepository.GetLastVisit(hpId, ptId, sinDate)?.SinDate ?? 0;

            //Get First Visit Date
            int firstDate = _receptionRepository.GetFirstVisitWithSyosin(hpId, ptId, sinDate);
            string comment = NoTrackingDataContext.PtCmtInfs.FirstOrDefault(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == 0)?.Text ?? string.Empty;

            var name = itemData.Name ?? string.Empty;
            var kanaName = itemData.KanaName ?? string.Empty;
            bool isKyuSeiName = false;
            if (isShowKyuSeiName)
            {
                var ptKyusei = NoTrackingDataContext.PtKyuseis.Where(item => item.HpId == hpId
                                                                            && item.PtId == ptId
                                                                            && item.EndDate >= sinDate
                                                                            && item.IsDeleted != 1)
                                                              .OrderBy(x => x.EndDate)
                                                              .FirstOrDefault();
                if (ptKyusei != null)
                {
                    name = ptKyusei.Name;
                    kanaName = ptKyusei.KanaName;
                    isKyuSeiName = true;
                }
            }
            return new PatientInforModel(
                itemData.HpId,
                itemData.PtId,
                itemData.ReferenceNo,
                itemData.SeqNo,
                itemData.PtNum,
                kanaName ?? string.Empty,
                name ?? string.Empty,
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
                raiinCount,
                comment,
                sinDate,
                isKyuSeiName);
        }

        public bool CheckExistIdList(List<long> ptIds)
        {
            ptIds = ptIds.Distinct().ToList();
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

        public List<PatientInforModel> GetAdvancedSearchResults(PatientAdvancedSearchInput input, int hpId, int pageIndex, int pageSize, Dictionary<string, string> sortData)
        {
            bool sortGroup = sortData.Select(item => item.Key).ToList().Exists(item => item.StartsWith(startGroupOrderKey));

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
            var groupKeyList = input.PatientGroups.Where(p => !string.IsNullOrEmpty(p.GroupCode)).Select(p => new { p.GroupId, p.GroupCode });
            if (groupKeyList.Any())
            {
                var groupIdList = groupKeyList.Select(g => g.GroupId).Distinct().ToList();
                var groupPtByIdList = NoTrackingDataContext.PtGrpInfs
                    .Where(p => p.IsDeleted == DeleteTypes.None && groupIdList.Contains(p.GroupId) && p.GroupCode != null)
                    .Select(p => new { p.PtId, p.GroupId, p.GroupCode })
                    .ToList();

                if (groupPtByIdList == null)
                {
                    return new();
                }

                string firstGroupCode = groupKeyList.First(g => g.GroupId == groupIdList.First()).GroupCode;
                var ptIds = groupPtByIdList.Where(g => g.GroupId == groupIdList.First() && g.GroupCode == firstGroupCode).Select(g => g.PtId).ToList();
                foreach (var groupId in groupIdList.Skip(1))
                {
                    string groupCode = groupKeyList.First(g => g.GroupId == groupId).GroupCode;
                    var ptIdItems = groupPtByIdList.Where(g => g.GroupId == groupId && g.GroupCode == groupCode).Select(g => g.PtId).ToList();
                    ptIds = ptIds.Where(item => ptIdItems.Contains(item)).ToList();
                }

                if (ptIds.Count == 0) return new();
                ptInfQuery = ptInfQuery.Where(p => ptIds.Distinct().Contains(p.PtId));
            }

            // Search with TenMst
            var listTenMstSearch = input.TenMsts;
            if (listTenMstSearch.Count > 0)
            {
                var odrInf = NoTrackingDataContext.OdrInfs.Where(x => x.IsDeleted == 0);
                int index = 0;
                IQueryable<long> ptOdrDetailTemp = Enumerable.Empty<long>().AsQueryable();
                while (index < listTenMstSearch.Count)
                {
                    var item = listTenMstSearch[index];
                    bool isComment = item.IsComment;
                    var ptOdrDetailNexts = NoTrackingDataContext.OdrInfDetails.Where(odr => odrInf.Any(x => x.HpId == odr.HpId
                                                                                                             && x.PtId == odr.PtId
                                                                                                             && x.SinDate == odr.SinDate
                                                                                                             && x.RaiinNo == odr.RaiinNo
                                                                                                             && x.RpNo == odr.RpNo
                                                                                                             && x.RpEdaNo == odr.RpEdaNo
                                                                                                             && (isComment ? odr.ItemCd != null && odr.ItemName != null && odr.ItemName.Contains(item.InputName)
                                                                                                                            : odr.ItemCd != null && odr.ItemCd == item.ItemCd)))
                                                                           .Select(x => x.PtId).Distinct();
                    if (index == 0)
                    {
                        ptOdrDetailTemp = NoTrackingDataContext.OdrInfDetails.Where(odr => odrInf.Any(x => x.HpId == odr.HpId
                                                                                                             && x.PtId == odr.PtId
                                                                                                             && x.SinDate == odr.SinDate
                                                                                                             && x.RaiinNo == odr.RaiinNo
                                                                                                             && x.RpNo == odr.RpNo
                                                                                                             && x.RpEdaNo == odr.RpEdaNo
                                                                                                             && (isComment ? odr.ItemCd != null && odr.ItemName != null && odr.ItemName.Contains(item.InputName)
                                                                                                                            : odr.ItemCd != null && odr.ItemCd == item.ItemCd)))
                                                                          .Select(x => x.PtId).Distinct();
                    }
                    else
                    {
                        if (!input.IsOrderOr)
                        {
                            ptOdrDetailTemp = ptOdrDetailTemp.Where(odr => ptOdrDetailNexts.Any(x => x == odr));
                        }
                        else
                        {
                            ptOdrDetailTemp = ptOdrDetailTemp.Union(ptOdrDetailNexts).Distinct();
                        }
                    }

                    index++;
                }
                ptInfQuery = ptInfQuery.Where(pt => ptOdrDetailTemp.Any(x => x == pt.PtId));
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
                select new PatientInfQueryModel
                {
                    PtInf = ptInf,
                    LastVisitDate = (
                        from r in raiinInfQuery
                        where r.PtId == ptInf.PtId
                            && r.Status >= RaiinState.TempSave
                        orderby r.SinDate descending
                        select r.SinDate
                    ).FirstOrDefault()
                };

            var result = sortGroup
                         ?
                         ptInfWithLastVisitDateQuery
                         .AsEnumerable()
                         .Select(p => ToModel(p.PtInf, string.Empty, p.LastVisitDate))
                         .ToList()
                         :
                         SortData(ptInfWithLastVisitDateQuery, sortData, pageIndex, pageSize);
            return result;

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
                var bithDay = CIUtil.GetJapanDateTimeNow().AddYears(-age);
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

        private PatientInforModel ToModel(PtInf p, string memo, int lastVisitDate, int sinDate)
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
                0,
                string.Empty,
                sinDate);
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
                0,
                string.Empty,
                0);
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

        public PatientInforModel GetPtInfByRefNo(int hpId, long refNo)
        {
            var ptInfWithRefNo = NoTrackingDataContext.PtInfs.FirstOrDefault(item => item.HpId == hpId
                                                                                     && item.ReferenceNo == refNo
                                                                                     && item.ReferenceNo != 0
                                                                                     && item.IsDelete == 0
                                                                                     && item.IsTester == 0);
            if (ptInfWithRefNo == null)
            {
                return new();
            }
            return ToModel(ptInfWithRefNo, string.Empty, 0);
        }

        public List<PatientInforModel> GetPtInfModelsByName(int hpId, string kanaName, string name, int birthDate, int sex1, int sex2)
        {
            var ptInfs = NoTrackingDataContext.PtInfs.Where(item => item.HpId == hpId
                                                                    && (item.KanaName == kanaName || item.Name == name)
                                                                    && item.Birthday == birthDate
                                                                    && (item.Sex == sex1 || item.Sex == sex2)
                                                                    && item.IsDelete == 0
                                                                    && item.IsTester == 0)
                                                    .ToList();
            return ptInfs.Select(item => ToModel(item, string.Empty, 0)).ToList();
        }

        public List<PatientInforModel> GetPtInfModels(int hpId, long refNo)
        {
            var ptInfs = NoTrackingDataContext.PtInfs.Where(item => item.HpId == hpId
                                                                    && item.ReferenceNo == refNo)
                                                    .ToList();
            return ptInfs.Select(item => ToModel(item, string.Empty, 0)).ToList();
        }

        public List<PatientInforModel> SearchBySindate(int sindate, int hpId, int pageIndex, int pageSize, Dictionary<string, string> sortData)
        {
            var ptIdList = NoTrackingDataContext.RaiinInfs.Where(r => r.SinDate == sindate).GroupBy(r => r.PtId).Select(gr => gr.Key).ToList();
            var ptInfWithLastVisitDate =
                (from p in NoTrackingDataContext.PtInfs
                 where p.IsDelete == 0 && ptIdList.Contains(p.PtId)
                 orderby p.PtNum descending
                 select new PatientInfQueryModel
                 {
                     PtInf = p,
                     LastVisitDate = (
                         from r in NoTrackingDataContext.RaiinInfs
                         where r.HpId == hpId
                             && r.PtId == p.PtId
                             && r.Status >= RaiinState.TempSave
                             && r.IsDeleted == DeleteTypes.None
                         orderby r.SinDate descending
                         select r.SinDate
                     ).FirstOrDefault()
                 });

            bool sortGroup = sortData.Select(item => item.Key).ToList().Exists(item => item.StartsWith(startGroupOrderKey));
            var result = sortGroup
                         ?
                         ptInfWithLastVisitDate
                         .AsEnumerable()
                         .Select(p => ToModel(p.PtInf, string.Empty, p.LastVisitDate))
                         .ToList()
                         :
                         SortData(ptInfWithLastVisitDate, sortData, pageIndex, pageSize);
            return result;
        }

        public List<PatientInforModel> SearchPhone(string keyword, bool isContainMode, int hpId, int pageIndex, int pageSize, Dictionary<string, string> sortData)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return new();
            }

            var ptInfWithLastVisitDate =
            from p in NoTrackingDataContext.PtInfs
            where p.IsDelete == 0 && (p.Tel1 != null && (isContainMode && p.Tel1.Contains(keyword) || p.Tel1.StartsWith(keyword)) ||
                                      p.Tel2 != null && (isContainMode && p.Tel2.Contains(keyword) || p.Tel2.StartsWith(keyword)) ||
                                      p.Name == keyword)
            orderby p.PtNum descending
            select new PatientInfQueryModel
            {
                PtInf = p,
                LastVisitDate = (
                        from r in NoTrackingDataContext.RaiinInfs
                        where r.HpId == hpId
                            && r.PtId == p.PtId
                            && r.Status >= RaiinState.TempSave
                            && r.IsDeleted == DeleteTypes.None
                        orderby r.SinDate descending
                        select r.SinDate
                    ).FirstOrDefault()
            };

            bool sortGroup = sortData.Select(item => item.Key).ToList().Exists(item => item.StartsWith(startGroupOrderKey));
            var result = sortGroup
                         ?
                         ptInfWithLastVisitDate
                         .AsEnumerable()
                         .Select(p => ToModel(p.PtInf, string.Empty, p.LastVisitDate))
                         .ToList()
                         :
                         SortData(ptInfWithLastVisitDate, sortData, pageIndex, pageSize);
            return result;
        }

        public List<PatientInforModel> SearchName(string originKeyword, string halfsizeKeyword, bool isContainMode, int hpId, int pageIndex, int pageSize, Dictionary<string, string> sortData)
        {
            if (string.IsNullOrWhiteSpace(originKeyword) ||
                string.IsNullOrWhiteSpace(halfsizeKeyword))
            {
                return new();
            }

            IQueryable<PatientInfQueryModel> ptInfWithLastVisitDate;
            if (isContainMode)
            {
                ptInfWithLastVisitDate = from p in NoTrackingDataContext.PtInfs
                                         where p.IsDelete == 0
                                         && ((p.Name != null && p.Name.Contains(originKeyword))
                                            || (p.KanaName != null && p.KanaName.Contains(originKeyword))
                                            || (p.Name != null && p.Name.Replace(" ", string.Empty).Replace("　", string.Empty).Contains(originKeyword))
                                            || (p.KanaName != null && p.KanaName.Replace(" ", string.Empty).Replace("　", string.Empty).Contains(originKeyword)))
                                         orderby p.PtNum descending
                                         select new PatientInfQueryModel
                                         {
                                             PtInf = p,
                                             LastVisitDate = (
                                                     from r in NoTrackingDataContext.RaiinInfs
                                                     where r.HpId == hpId
                                                         && r.PtId == p.PtId
                                                         && r.Status >= RaiinState.TempSave
                                                         && r.IsDeleted == DeleteTypes.None
                                                     orderby r.SinDate descending
                                                     select r.SinDate
                                                 ).FirstOrDefault()
                                         };
            }
            else
            {
                ptInfWithLastVisitDate = from p in NoTrackingDataContext.PtInfs
                                         where p.IsDelete == 0
                                         && ((p.Name != null && p.Name.StartsWith(originKeyword))
                                            || (p.KanaName != null && p.KanaName.StartsWith(originKeyword))
                                            || (p.Name != null && p.Name.Replace(" ", string.Empty).Replace("　", string.Empty).Contains(originKeyword))
                                            || (p.KanaName != null && p.KanaName.Replace(" ", string.Empty).Replace("　", string.Empty).Contains(originKeyword)))
                                         orderby p.PtNum descending
                                         select new PatientInfQueryModel
                                         {
                                             PtInf = p,
                                             LastVisitDate = (
                                                     from r in NoTrackingDataContext.RaiinInfs
                                                     where r.HpId == hpId
                                                         && r.PtId == p.PtId
                                                         && r.Status >= RaiinState.TempSave
                                                         && r.IsDeleted == DeleteTypes.None
                                                     orderby r.SinDate descending
                                                     select r.SinDate
                                                 ).FirstOrDefault()
                                         };
            }

            bool sortGroup = sortData.Select(item => item.Key).ToList().Exists(item => item.StartsWith(startGroupOrderKey));
            var result = sortGroup
                         ?
                         ptInfWithLastVisitDate
                         .AsEnumerable()
                         .Select(p => ToModel(p.PtInf, string.Empty, p.LastVisitDate))
                         .ToList()
                         :
                         SortData(ptInfWithLastVisitDate, sortData, pageIndex, pageSize);
            return result;
        }

        public List<PatientInforModel> SearchEmptyId(int hpId, long ptNum, int pageIndex, int pageSize, bool isPtNumCheckDigit, int autoSetting)
        {
            if (ptNum > 9999999999)
            {
                return new();
            }
            int originPageSize = pageSize;
            if (isPtNumCheckDigit)
            {
                pageSize = pageSize * 10;
            }
            long endIndex = (pageIndex - 1) * pageSize + ptNum + pageSize;
            long startIndex = (pageIndex - 1) * pageSize + ptNum;
            List<PatientInforModel> result = new();

            var existPtNum = NoTrackingDataContext.PtInfs.Where(p => p.HpId == hpId && p.PtNum >= startIndex && p.PtNum <= endIndex).ToList();
            for (long i = startIndex; i < endIndex; i++)
            {
                if (result.Count > originPageSize || i > 9999999999)
                {
                    break;
                }
                if (isPtNumCheckDigit && !CIUtil.PtNumCheckDigits(i))
                {
                    continue;
                }
                var checkExistPtNum = existPtNum.FirstOrDefault(p => p.PtNum == i && (autoSetting != 1 || p.IsDelete == 0));
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
                string digit1 = futansyaNo.Substring(0, 1);
                string digit2 = futansyaNo.Substring(1, 1);
                var listDefHoken = NoTrackingDataContext.DefHokenNos
                .Where(x => x.HpId == hpId
                            && x.Digit1.Equals(digit1)
                            && x.Digit2.Equals(digit2)
                            && x.IsDeleted == 0)
                .OrderBy(entity => entity.HpId)
                .ThenBy(entity => entity.HokenNo)
                .ThenBy(entity => entity.HokenEdaNo)
                .ThenBy(entity => entity.SortNo)
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
                throw;
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

        public PtKyuseiInfModel GetDocumentKyuSeiInf(int hpId, long ptId, int sinDay)
        {
            var ptKyusei = NoTrackingDataContext.PtKyuseis.Where(item => item.HpId == hpId
                                                                         && item.PtId == ptId
                                                                         && item.EndDate < sinDay
                                                                         && item.IsDeleted != 1
                                                           ).OrderByDescending(item => item.EndDate)
                                                           .FirstOrDefault() ?? new PtKyusei();

            return new PtKyuseiInfModel(
                       ptKyusei.HpId,
                       ptKyusei.PtId,
                       ptKyusei.SeqNo,
                       ptKyusei.KanaName ?? string.Empty,
                       ptKyusei.Name ?? string.Empty,
                       ptKyusei.EndDate,
                       ptKyusei.IsDeleted);
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
                            CreateDate = CIUtil.GetJapanDateTimeNow(),
                            CreateId = userId,
                            UpdateDate = CIUtil.GetJapanDateTimeNow(),
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
                            UpdateDate = CIUtil.GetJapanDateTimeNow(),
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
                throw;
            }
        }

        public (bool resultSave, long ptId) CreatePatientInfo(PatientInforSaveModel ptInf, List<PtKyuseiModel> ptKyuseis, List<CalculationInfModel> ptSanteis, List<InsuranceModel> insurances, List<HokenInfModel> hokenInfs, List<KohiInfModel> hokenKohis, List<GroupInfModel> ptGrps, List<LimitListModel> maxMoneys, Func<int, long, long, IEnumerable<InsuranceScanModel>> handlerInsuranceScans, int userId)
        {
            try
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
                if (patientInsert.DeathDate > 0)
                {
                    patientInsert.IsDead = 1;
                }
                else
                {
                    patientInsert.IsDead = 0;
                }
                patientInsert.CreateDate = CIUtil.GetJapanDateTimeNow();
                patientInsert.CreateId = userId;
                patientInsert.UpdateId = userId;
                patientInsert.UpdateDate = CIUtil.GetJapanDateTimeNow();
                patientInsert.HpId = hpId;

                // string querySql = $"INSERT INTO public.\"PT_INF\"\r\n(\"HP_ID\", \"PT_NUM\", \"KANA_NAME\", \"NAME\", \"SEX\", \"BIRTHDAY\", \"IS_DEAD\", \"DEATH_DATE\", \"HOME_POST\", \"HOME_ADDRESS1\", \"HOME_ADDRESS2\", \"TEL1\", \"TEL2\", \"MAIL\", \"SETAINUSI\", \"ZOKUGARA\", \"JOB\", \"RENRAKU_NAME\", \"RENRAKU_POST\", \"RENRAKU_ADDRESS1\", \"RENRAKU_ADDRESS2\", \"RENRAKU_TEL\", \"RENRAKU_MEMO\", \"OFFICE_NAME\", \"OFFICE_POST\", \"OFFICE_ADDRESS1\", \"OFFICE_ADDRESS2\", \"OFFICE_TEL\", \"OFFICE_MEMO\", \"IS_RYOSYO_DETAIL\", \"PRIMARY_DOCTOR\", \"IS_TESTER\", \"IS_DELETE\", \"CREATE_DATE\", \"CREATE_ID\", \"CREATE_MACHINE\", \"UPDATE_DATE\", \"UPDATE_ID\", \"UPDATE_MACHINE\", \"MAIN_HOKEN_PID\", \"LIMIT_CONS_FLG\") VALUES({patientInsert.HpId}, {patientInsert.PtNum}, '{patientInsert.KanaName}', '{patientInsert.Name}', {patientInsert.Sex}, {patientInsert.Birthday}, {patientInsert.IsDead}, {patientInsert.DeathDate}, '{patientInsert.HomePost}', '{patientInsert.HomeAddress1}', '{patientInsert.HomeAddress2}', '{patientInsert.Tel1}', '{patientInsert.Tel2}', '{patientInsert.Mail}', '{patientInsert.Setanusi}', '{patientInsert.Zokugara}', '{patientInsert.Job}', '{patientInsert.RenrakuName}', '{patientInsert.RenrakuPost}', '{patientInsert.RenrakuAddress1}', '{patientInsert.RenrakuAddress2}', '{patientInsert.RenrakuTel}', '{patientInsert.RenrakuMemo}', '{patientInsert.OfficeName}', '{patientInsert.OfficePost}', '{patientInsert.OfficeAddress1}', '{patientInsert.OfficeAddress2}', '{patientInsert.OfficeTel}', '{patientInsert.OfficeMemo}', {patientInsert.IsRyosyoDetail}, {patientInsert.PrimaryDoctor}, {patientInsert.IsTester}, {patientInsert.IsDelete}, '{patientInsert.CreateDate.ToString("yyyy-MM-dd HH:mm:ss.fff")}', {patientInsert.CreateId}, '', '{patientInsert.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss.fff")}', {patientInsert.UpdateId}, '', {patientInsert.MainHokenPid}, {patientInsert.LimitConsFlg}) ON CONFLICT DO NOTHING;";
                TrackingDataContext.PtInfs.Add(patientInsert);
                //TrackingDataContext.Database.SetCommandTimeout(1200);
                //bool resultCreatePatient = TrackingDataContext.Database.ExecuteSqlRaw(querySql) > 0;
                bool resultCreatePatient = TrackingDataContext.SaveChanges() > 0;

                if (!resultCreatePatient)
                {
                    return (false, 0);
                }
                else
                {
                    patientInsert.PtId = NoTrackingDataContext.PtInfs.FirstOrDefault(p => p.HpId == hpId && p.PtNum == patientInsert.PtNum)?.PtId ?? 0;
                }

                if (ptSanteis != null && ptSanteis.Any())
                {
                    var ptSanteiInserts = Mapper.Map<CalculationInfModel, PtSanteiConf>(ptSanteis, (src, dest) =>
                    {
                        dest.CreateId = userId;
                        dest.PtId = patientInsert.PtId;
                        dest.HpId = hpId;
                        dest.CreateDate = CIUtil.GetJapanDateTimeNow();
                        dest.UpdateId = userId;
                        dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
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
                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                        UpdateDate = CIUtil.GetJapanDateTimeNow(),
                        UpdateId = userId
                    });
                }

                if (ptGrps != null && ptGrps.Any())
                {
                    var listPtGrpInf = Mapper.Map<GroupInfModel, PtGrpInf>(ptGrps, (src, dest) =>
                    {
                        dest.CreateDate = CIUtil.GetJapanDateTimeNow();
                        dest.CreateId = userId;
                        dest.HpId = hpId;
                        dest.PtId = patientInsert.PtId;
                        dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        dest.UpdateId = userId;
                        return dest;
                    });
                    TrackingDataContext.PtGrpInfs.AddRange(listPtGrpInf);
                }

                if (ptKyuseis != null && ptKyuseis.Any())
                {
                    var ptKyuseiList = Mapper.Map<PtKyuseiModel, PtKyusei>(ptKyuseis, (src, dest) =>
                    {
                        dest.CreateDate = CIUtil.GetJapanDateTimeNow();
                        dest.CreateId = userId;
                        dest.UpdateId = userId;
                        dest.HpId = hpId;
                        dest.PtId = patientInsert.PtId;
                        dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        return dest;
                    });
                    TrackingDataContext.PtKyuseis.AddRange(ptKyuseiList);
                }

                #region Hoken parterrn
                List<PtHokenPattern> pthokenPartterns = Mapper.Map<InsuranceModel, PtHokenPattern>(insurances.Where(x => x.IsAddNew), (src, dest) =>
                {
                    dest.CreateId = userId;
                    dest.UpdateId = userId;
                    dest.CreateDate = CIUtil.GetJapanDateTimeNow();
                    dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
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
                    dest.CreateDate = CIUtil.GetJapanDateTimeNow();
                    dest.UpdateId = userId;
                    dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
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
                        destR.CreateDate = CIUtil.GetJapanDateTimeNow();
                        destR.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        return destR;
                    }));
                    #endregion

                    #region PtHokenCheck
                    TrackingDataContext.PtHokenChecks.AddRange(Mapper.Map<ConfirmDateModel, PtHokenCheck>(src.ConfirmDateList, (srcCf, destCf) =>
                    {
                        destCf.CreateId = userId;
                        destCf.CreateDate = CIUtil.GetJapanDateTimeNow();
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
                    dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    dest.CreateId = userId;
                    dest.CreateDate = CIUtil.GetJapanDateTimeNow();
                    dest.PtId = patientInsert.PtId;
                    dest.HpId = hpId;
                    dest.EndDate = src.EndDate == 0 ? defaultMaxDate : src.EndDate;
                    #region PtHokenCheck
                    TrackingDataContext.PtHokenChecks.AddRange(Mapper.Map<ConfirmDateModel, PtHokenCheck>(src.ConfirmDateList, (srcCf, destCf) =>
                    {
                        destCf.CreateId = userId;
                        destCf.CreateDate = CIUtil.GetJapanDateTimeNow();
                        destCf.CheckDate = DateTime.SpecifyKind(CIUtil.IntToDate(srcCf.ConfirmDate), DateTimeKind.Utc);
                        destCf.CheckCmt = srcCf.CheckComment;
                        destCf.HokenId = dest.HokenId;
                        destCf.CheckId = userId;
                        destCf.PtID = patientInsert.PtId;
                        destCf.HokenGrp = 2;
                        destCf.HpId = hpId;
                        return destCf;
                    }));
                    #endregion
                    return dest;
                });
                TrackingDataContext.PtKohis.AddRange(ptKohiInfs);
                #endregion PtKohiInf

                #region Maxmoney
                if (maxMoneys != null && maxMoneys.Any())
                {
                    TrackingDataContext.LimitListInfs.AddRange(Mapper.Map<LimitListModel, LimitListInf>(maxMoneys, (src, dest) =>
                    {
                        dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        dest.CreateDate = CIUtil.GetJapanDateTimeNow();
                        dest.PtId = patientInsert.PtId;
                        dest.HpId = hpId;
                        dest.SinDate = src.SinDateY * 10000 + src.SinDateM * 100 + src.SinDateD;
                        dest.UpdateId = userId;
                        dest.CreateId = userId;
                        return dest;
                    }));
                }
                #endregion Maxmoney

                #region insurancesCan
                var insuranceScanDatas = handlerInsuranceScans(hpId, patientInsert.PtNum, patientInsert.PtId);
                if (insuranceScanDatas != null && insuranceScanDatas.Any())
                {
                    TrackingDataContext.PtHokenScans.AddRange(Mapper.Map<InsuranceScanModel, PtHokenScan>(insuranceScanDatas, (src, dest) =>
                    {
                        dest.CreateDate = CIUtil.GetJapanDateTimeNow();
                        dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        dest.CreateId = userId;
                        dest.UpdateId = userId;
                        return dest;
                    }));
                }
                #endregion

                int changeDatas = TrackingDataContext.ChangeTracker.Entries().Count(x => x.State == EntityState.Modified || x.State == EntityState.Added);
                if (changeDatas == 0 && resultCreatePatient)
                    return (true, patientInsert.PtId);

                return (TrackingDataContext.SaveChanges() > 0, patientInsert.PtId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private long GetAutoPtNum(int hpId)
        {
            long startPtNum = 1;
            long startPtNumSetting = (long)GetSettingValue(1014, hpId, 1);
            if (startPtNumSetting > 0)
            {
                startPtNum = startPtNumSetting;
            }
            return GetAutoPtNumAction(startPtNum, hpId);
        }

        private long GetAutoPtNumAction(long startValue, int hpId)
        {
            int ptNumCheckDigit = (int)GetSettingValue(1001, hpId, 0);
            int autoSetting = (int)GetSettingValue(1014, hpId, 0);
            var ptList = NoTrackingDataContext.PtInfs.Where(ptInf => (autoSetting != 1 || ptInf.IsDelete == 0) && ptInf.PtNum >= startValue).Select(pt => pt.PtNum);
            long minPtNum = 0;

            if (ptNumCheckDigit == 1)
            {
                if (ptList != null && ptList.Any())
                {
                    var ptListDropNumberUnit = ptList.Select(pt => (long)(pt / 10));
                    var ptInfNoNext = ptList?.Where(pt => !ptListDropNumberUnit.Distinct().Contains(((pt / 10) + 1))).Select(pt => pt / 10).OrderBy(pt => pt).ToList();

                    if (ptInfNoNext != null && ptInfNoNext.Any())
                    {
                        minPtNum = ptInfNoNext.FirstOrDefault();
                    }
                }
                return CIUtil.PtIDChkDgtMakeM10W31(minPtNum + 1);
            }
            else
            {
                var ptNumExisting = NoTrackingDataContext.PtInfs.FirstOrDefault
                    (ptInf => (autoSetting != 1 || ptInf.IsDelete == 0) && ptInf.PtNum == startValue);
                if (ptNumExisting == null)
                {
                    return startValue;
                }

                var ptInfNoNext = ptList?.Where(pt => !ptList.Distinct().Contains(pt + 1)).OrderBy(pt => pt).ToList();

                if (ptInfNoNext != null && ptInfNoNext.Any())
                {
                    minPtNum = ptInfNoNext.FirstOrDefault();
                }

                return minPtNum + 1;
            }
        }

        public (bool resultSave, long ptId) UpdatePatientInfo(PatientInforSaveModel ptInf, List<PtKyuseiModel> ptKyuseis, List<CalculationInfModel> ptSanteis, List<InsuranceModel> insurances, List<HokenInfModel> hokenInfs, List<KohiInfModel> hokenKohis, List<GroupInfModel> ptGrps, List<LimitListModel> maxMoneys, Func<int, long, long, IEnumerable<InsuranceScanModel>> handlerInsuranceScans, int userId, List<int> hokenIdList)
        {
            int defaultMaxDate = 99999999;
            int hpId = ptInf.HpId;

            #region CloneByomeiWithNewHokenId
            if (hokenIdList.Any())
            {
                //if add new hoken => confirm clone byomei
                var hokenInf = hokenInfs.OrderByDescending(p => p.EndDateSort)
                                        .ThenByDescending(p => p.HokenId)
                                        .FirstOrDefault(p => p.IsDeleted == DeleteTypes.None && !p.IsAddNew);
                if (hokenInf != null)
                {
                    var ptByomeis = TrackingDataContext.PtByomeis.Where(item => item.PtId == ptInf.PtId
                                                                                && item.HokenPid == hokenInf.HokenId
                                                                                && item.IsDeleted == DeleteTypes.None
                                                                                && item.TenkiKbn == TenkiKbnConst.Continued)
                                                                 .ToList();
                    if (ptByomeis.Count > 0)
                    {
                        foreach (var hokenId in hokenIdList)
                        {
                            CloneByomeiWithNewHokenId(ptByomeis, hokenId, userId);
                        }
                    }
                }
            }
            #endregion

            #region Patient-info
            PtInf? patientInfo = TrackingDataContext.PtInfs.FirstOrDefault(x => x.PtId == ptInf.PtId);
            if (patientInfo is null)
                return (false, ptInf.PtId);

            Mapper.Map(ptInf, patientInfo, (source, dest) =>
            {
                if (dest.DeathDate > 0)
                {
                    dest.IsDead = 1;
                }
                else
                {
                    dest.IsDead = 0;
                }
                dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
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
                    memoCurrent.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    memoCurrent.UpdateId = userId;
                }
                else
                {
                    if (memoCurrent.Memo != null && !memoCurrent.Memo.Equals(ptInf.Memo))
                    {
                        memoCurrent.IsDeleted = 1;
                        memoCurrent.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        memoCurrent.UpdateId = userId;
                        TrackingDataContext.PtMemos.Add(new PtMemo()
                        {
                            HpId = patientInfo.HpId,
                            PtId = patientInfo.PtId,
                            Memo = ptInf.Memo,
                            CreateId = userId,
                            UpdateDate = CIUtil.GetJapanDateTimeNow(),
                            UpdateId = userId,
                            CreateDate = CIUtil.GetJapanDateTimeNow()
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
                        UpdateDate = CIUtil.GetJapanDateTimeNow(),
                        UpdateId = userId,
                        CreateDate = CIUtil.GetJapanDateTimeNow(),
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
                item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                item.IsDeleted = DeleteTypes.Deleted;
            }

            var ptSanteiConfListAdd = Mapper.Map<CalculationInfModel, PtSanteiConf>(ptSanteis.Where(x => x.SeqNo == 0), (src, dest) =>
            {
                dest.CreateDate = CIUtil.GetJapanDateTimeNow();
                dest.CreateId = userId;
                dest.HpId = hpId;
                dest.PtId = patientInfo.PtId;
                dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
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
                    ptSanteiUpdate.UpdateDate = CIUtil.GetJapanDateTimeNow();
                }
            }
            #endregion

            #region PtKyusei

            var databaseKyuseis = TrackingDataContext.PtKyuseis.Where(x => x.PtId == patientInfo.PtId && x.HpId == hpId && x.IsDeleted == DeleteTypes.None).ToList();
            var KyuseiRemoves = databaseKyuseis.Where(c => !ptKyuseis.Any(_ => _.SeqNo == c.SeqNo));

            foreach (var item in KyuseiRemoves)
            {
                item.UpdateId = userId;
                item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                item.IsDeleted = DeleteTypes.Deleted;
            }

            var ptKyuseiListAdd = Mapper.Map<PtKyuseiModel, PtKyusei>(ptKyuseis.Where(x => x.SeqNo == 0), (src, dest) =>
            {
                dest.CreateDate = CIUtil.GetJapanDateTimeNow();
                dest.CreateId = userId;
                dest.UpdateId = userId;
                dest.HpId = hpId;
                dest.PtId = patientInfo.PtId;
                dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
                return dest;
            });
            TrackingDataContext.PtKyuseis.AddRange(ptKyuseiListAdd);

            foreach (var item in ptKyuseis.Where(x => x.SeqNo != 0))
            {
                var kyuseiUpdate = databaseKyuseis.FirstOrDefault(x => x.SeqNo == item.SeqNo);
                if (kyuseiUpdate != null)
                {
                    kyuseiUpdate.UpdateDate = CIUtil.GetJapanDateTimeNow();
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
                item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                item.IsDeleted = DeleteTypes.Deleted;
            }

            foreach (var item in ptGrps)
            {
                var info = databaseGrpInfs.FirstOrDefault(pt => pt.HpId == hpId && pt.PtId == patientInfo.PtId && pt.GroupId == item.GroupId);

                if (info != null && !string.IsNullOrEmpty(item.GroupCode))
                {
                    //Remove record old
                    info.UpdateId = userId;
                    info.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    info.IsDeleted = DeleteTypes.Deleted;

                    //clone new record
                    PtGrpInf model = Mapper.Map(item, new PtGrpInf(), (source, dest) =>
                    {
                        dest.CreateId = userId;
                        dest.CreateDate = CIUtil.GetJapanDateTimeNow();
                        dest.UpdateId = userId;
                        dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
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
                        dest.CreateDate = CIUtil.GetJapanDateTimeNow();
                        dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
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
                    info.UpdateDate = CIUtil.GetJapanDateTimeNow();
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
                x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                x.UpdateId = userId;
            });

            List<PtHokenPattern> pthokenPartterns = Mapper.Map<InsuranceModel, PtHokenPattern>(insurances.Where(x => x.SeqNo == 0 && x.IsAddNew), (src, dest) =>
            {
                dest.CreateId = userId;
                dest.CreateDate = CIUtil.GetJapanDateTimeNow();
                dest.UpdateId = userId;
                dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
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
                        dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
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
                dest.CreateDate = CIUtil.GetJapanDateTimeNow();
                dest.UpdateId = userId;
                dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
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
                    destR.CreateDate = CIUtil.GetJapanDateTimeNow();
                    destR.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    return destR;
                }));
                #endregion

                #region PtHokenCheck
                TrackingDataContext.PtHokenChecks.AddRange(Mapper.Map<ConfirmDateModel, PtHokenCheck>(src.ConfirmDateList, (srcCf, destCf) =>
                {
                    destCf.CreateId = userId;
                    destCf.UpdateId = userId;
                    destCf.CreateDate = CIUtil.GetJapanDateTimeNow();
                    destCf.UpdateDate = CIUtil.GetJapanDateTimeNow();
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
                        dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
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
                        dest.CreateDate = CIUtil.GetJapanDateTimeNow();
                        dest.UpdateId = userId;
                        dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
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
                            updateItem.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        }
                    }

                    var listDatabaseByHokenInf = databasePtRousaiTenkis.Where(x => x.HokenId == updateHokenInf.HokenId);
                    var listRemoves = listDatabaseByHokenInf.Where(x => !item.ListRousaiTenki.Any(m => m.SeqNo == x.SeqNo)).ToList();

                    listRemoves.ForEach(x =>
                    {
                        x.IsDeleted = 1;
                        x.UpdateId = userId;
                        x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    });
                }
            }
            #endregion HokenInf

            #region HokenKohi
            //Add new
            List<PtKohi> ptKohiInfs = Mapper.Map<KohiInfModel, PtKohi>(hokenKohis.Where(x => x.IsAddNew && x.SeqNo == 0), (src, dest) =>
            {
                dest.CreateId = userId;
                dest.CreateDate = CIUtil.GetJapanDateTimeNow();
                dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
                dest.UpdateId = userId;
                dest.PtId = patientInfo.PtId;
                dest.HpId = hpId;
                dest.EndDate = src.EndDate == 0 ? defaultMaxDate : src.EndDate;
                #region PtHokenCheck
                TrackingDataContext.PtHokenChecks.AddRange(Mapper.Map<ConfirmDateModel, PtHokenCheck>(src.ConfirmDateList, (srcCf, destCf) =>
                {
                    destCf.CreateId = userId;
                    destCf.CreateDate = CIUtil.GetJapanDateTimeNow();
                    destCf.UpdateDate = CIUtil.GetJapanDateTimeNow();
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
                        dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
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
                var exist = maxMoneys.FirstOrDefault(x => x.SeqNo == item.SeqNo && x.Id == item.Id);
                if (exist == null)
                {
                    item.IsDeleted = DeleteTypes.Deleted;
                    item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    item.UpdateId = userId;
                }
                else
                {
                    item.SortKey = exist.SortKey;
                    item.FutanGaku = exist.FutanGaku;
                    item.TotalGaku = exist.TotalGaku;
                    item.Biko = exist.Biko;
                    item.SinDate = exist.SinDateY * 10000 + exist.SinDateM * 100 + exist.SinDateD;
                    item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    item.UpdateId = userId;
                }
            }

            TrackingDataContext.LimitListInfs.AddRange(Mapper.Map<LimitListModel, LimitListInf>(maxMoneys.Where(x => x.SeqNo == 0 && x.Id == 0), (src, dest) =>
            {
                dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
                dest.CreateDate = CIUtil.GetJapanDateTimeNow();
                dest.PtId = patientInfo.PtId;
                dest.HpId = hpId;
                dest.SinDate = src.SinDateY * 10000 + src.SinDateM * 100 + src.SinDateD;
                dest.UpdateId = userId;
                dest.CreateId = userId;
                return dest;
            }));
            #endregion

            #region insuranceScan
            var insuranceScanDatabases = TrackingDataContext.PtHokenScans.Where(x => x.HpId == hpId && x.PtId == patientInfo.PtId && x.IsDeleted == DeleteTypes.None).ToList();
            var insuranceScanDatas = handlerInsuranceScans(hpId, patientInfo.PtNum, patientInfo.PtId);
            if (insuranceScanDatas != null && insuranceScanDatas.Any())
            {
                foreach (var scan in insuranceScanDatas)
                {
                    if (scan.IsDeleted == DeleteTypes.Deleted)
                    {
                        var deleteItem = insuranceScanDatabases.FirstOrDefault(x => x.SeqNo == scan.SeqNo);
                        if (deleteItem is not null)
                        {
                            deleteItem.IsDeleted = DeleteTypes.Deleted;
                            deleteItem.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            deleteItem.UpdateId = userId;
                        }
                    }
                    else
                    {
                        if (scan.SeqNo == 0) //Create
                        {
                            TrackingDataContext.PtHokenScans.Add(Mapper.Map(scan, new PtHokenScan(), (src, dest) =>
                            {
                                dest.CreateDate = CIUtil.GetJapanDateTimeNow();
                                dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                dest.CreateId = userId;
                                return dest;
                            }));
                        }
                        else
                        {
                            var updateItem = insuranceScanDatabases.FirstOrDefault(x => x.SeqNo == scan.SeqNo);
                            if (updateItem is not null)
                            {
                                updateItem.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                updateItem.UpdateId = userId;
                                updateItem.FileName = scan.FileName;
                            }
                        }
                    }
                }
            }
            #endregion

            return (TrackingDataContext.SaveChanges() > 0, patientInfo.PtId);
        }

        private double GetSettingValue(int groupCd, int hpId, int grpEdaNo = 0, int defaultValue = 0)
        {
            SystemConf? systemConf;
            systemConf = NoTrackingDataContext.SystemConfs.FirstOrDefault(p =>
                    p.HpId == hpId && p.GrpCd == groupCd && p.GrpEdaNo == grpEdaNo);
            return systemConf != null ? systemConf.Val : defaultValue;
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
                addedHokenCheck.CreateDate = CIUtil.GetJapanDateTimeNow();
                addedHokenCheck.UpdateDate = CIUtil.GetJapanDateTimeNow();
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
                    modelUpdate.UpdateDate = CIUtil.GetJapanDateTimeNow();
                }
            }
        }

        public bool DeletePatientInfo(long ptId, int hpId, int userId)
        {
            var patientInf = TrackingDataContext.PtInfs.FirstOrDefault(x => x.PtId == ptId && x.HpId == hpId && x.IsDelete == DeleteTypes.None);

            if (patientInf != null)
            {
                patientInf.IsDelete = DeleteTypes.Deleted;
                patientInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                patientInf.UpdateId = userId;
                #region PtMemo
                var ptMemos = TrackingDataContext.PtMemos.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == DeleteTypes.None).ToList();
                foreach (var item in ptMemos)
                {
                    item.IsDeleted = DeleteTypes.Deleted;
                    item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    item.UpdateId = userId;
                }
                #endregion

                #region ptKyuseis
                var ptKyuseis = TrackingDataContext.PtKyuseis.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == DeleteTypes.None).ToList();
                ptKyuseis.ForEach(x =>
                {
                    x.IsDeleted = DeleteTypes.Deleted;
                    x.UpdateId = userId;
                    x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                });
                #endregion

                #region ptSanteis
                var ptSanteis = TrackingDataContext.PtSanteiConfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == DeleteTypes.None).ToList();
                ptSanteis.ForEach(x =>
                {
                    x.IsDeleted = DeleteTypes.Deleted;
                    x.UpdateId = userId;
                    x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                });
                #endregion

                #region HokenParttern
                var ptHokenParterns = TrackingDataContext.PtHokenPatterns.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == DeleteTypes.None).ToList();
                ptHokenParterns.ForEach(x =>
                {
                    x.IsDeleted = DeleteTypes.Deleted;
                    x.UpdateId = userId;
                    x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                });
                #endregion

                #region HokenInf
                var ptHokenInfs = TrackingDataContext.PtHokenInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == DeleteTypes.None).ToList();
                ptHokenInfs.ForEach(x =>
                {
                    x.IsDeleted = DeleteTypes.Deleted;
                    x.UpdateId = userId;
                    x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                });
                #endregion

                #region HokenKohi
                var ptHokenKohis = TrackingDataContext.PtKohis.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == DeleteTypes.None).ToList();
                ptHokenKohis.ForEach(x =>
                {
                    x.IsDeleted = DeleteTypes.Deleted;
                    x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    x.UpdateId = userId;
                });
                #endregion

                #region HokenCheck
                var ptHokenChecks = TrackingDataContext.PtHokenChecks.Where(x => x.HpId == hpId && x.PtID == ptId && x.IsDeleted == DeleteTypes.None).ToList();
                ptHokenChecks.ForEach(x =>
                {
                    x.IsDeleted = DeleteTypes.Deleted;
                    x.UpdateId = userId;
                    x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                });
                #endregion

                #region RousaiTenki
                var ptRousaiTenkies = TrackingDataContext.PtRousaiTenkis.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == DeleteTypes.None).ToList();
                ptRousaiTenkies.ForEach(x =>
                {
                    x.IsDeleted = DeleteTypes.Deleted;
                    x.UpdateId = userId;
                    x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                });
                #endregion

                #region RaiinInf
                var raiinInfList = TrackingDataContext.RaiinInfs.Where(item => item.PtId == ptId
                                                                               && item.IsDeleted != DeleteTypes.Deleted)
                                                                .ToList();
                raiinInfList.ForEach(x =>
                {
                    x.IsDeleted = DeleteTypes.Deleted;
                    x.UpdateId = userId;
                    x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                });
                #endregion

            }
            return TrackingDataContext.SaveChanges() > 0;
        }

        public bool IsAllowDeletePatient(int hpId, long ptId)
        {
            var raiinInf = NoTrackingDataContext.RaiinInfs.FirstOrDefault(item => item.HpId == hpId
                                                                                  && item.PtId == ptId
                                                                                  && item.SinStartTime != null
                                                                                  && item.SinStartTime != string.Empty
                                                                                  && item.SinEndTime != null
                                                                                  && item.SinEndTime != string.Empty
                                                                                  && item.Status > 2
                                                                                  && item.IsDeleted != DeleteTypes.Deleted);

            if (raiinInf != null)
            {
                return false;
            }
            return true;
        }

        public HokenMstModel GetHokenMstByInfor(int hokenNo, int hokenEdaNo, int sinDate)
        {
            var hokenMst = TrackingDataContext.HokenMsts.FirstOrDefault(x => x.HokenNo == hokenNo
                                                                        && x.HokenEdaNo == hokenEdaNo
                                                                        && x.StartDate <= sinDate
                                                                        && sinDate <= x.EndDate);
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

        public PatientInforModel GetPtInf(int hpId, long ptId)
        {
            var ptInf = NoTrackingDataContext.PtInfs.FirstOrDefault(pt => pt.HpId == hpId && pt.PtId == ptId && pt.IsDelete != 1) ?? new PtInf();
            return new PatientInforModel(
                        ptInf.HpId,
                        ptInf.PtId,
                        ptInf.ReferenceNo,
                        ptInf.SeqNo,
                        ptInf.PtNum,
                        ptInf.KanaName ?? string.Empty,
                        ptInf.Name ?? string.Empty,
                        ptInf.Sex,
                        ptInf.Birthday,
                        ptInf.LimitConsFlg,
                        ptInf.IsDead,
                        ptInf.DeathDate,
                        ptInf.HomePost ?? string.Empty,
                        ptInf.HomeAddress1 ?? string.Empty,
                        ptInf.HomeAddress2 ?? string.Empty,
                        ptInf.Tel1 ?? string.Empty,
                        ptInf.Tel2 ?? string.Empty,
                        ptInf.Mail ?? string.Empty,
                        ptInf.Setanusi ?? string.Empty,
                        ptInf.Zokugara ?? string.Empty,
                        ptInf.Job ?? string.Empty,
                        ptInf.RenrakuName ?? string.Empty,
                        ptInf.RenrakuPost ?? string.Empty,
                        ptInf.RenrakuAddress1 ?? string.Empty,
                        ptInf.RenrakuAddress2 ?? string.Empty,
                        ptInf.RenrakuTel ?? string.Empty,
                        ptInf.RenrakuMemo ?? string.Empty,
                        ptInf.OfficeName ?? string.Empty,
                        ptInf.OfficePost ?? string.Empty,
                        ptInf.OfficeAddress1 ?? string.Empty,
                        ptInf.OfficeAddress2 ?? string.Empty,
                        ptInf.OfficeTel ?? string.Empty,
                        ptInf.OfficeMemo ?? string.Empty,
                        ptInf.IsRyosyoDetail,
                        ptInf.PrimaryDoctor,
                        ptInf.IsTester,
                        ptInf.MainHokenPid,
                        string.Empty,
                        0,
                        0,
                        0,
                        string.Empty,
                        0
                    );
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        public List<PatientInforModel> SearchPatient(int hpId, long ptId, int pageIndex, int pageSize)
        {
            string keyword = ptId.ToString();

            List<PatientInforModel> result;
            var ptInfs = NoTrackingDataContext.PtInfs
                .Where(x => x.HpId == hpId && x.IsDelete == 0 && x.PtId.ToString().Contains(keyword))
                .OrderBy(x => x.PtNum)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var ptIdList = ptInfs.Select(p => p.PtId).ToList();

            var raiinInfs = NoTrackingDataContext.RaiinInfs.Where(x =>
                    x.HpId == hpId &&
                    x.Status >= RaiinState.TempSave &&
                    x.IsDeleted == 0 &&
                    ptIdList.Contains(x.PtId))
                .GroupBy(raiinInf => new { raiinInf.HpId, raiinInf.PtId })
                .Select(grp => new
                {
                    grp.Key.PtId,
                    SinDate = grp.OrderByDescending(x => x.SinDate).Select(x => x.SinDate).FirstOrDefault()
                })
                .ToList();

            result = ptInfs.Select((x) => new PatientInforModel(
                            x.HpId,
                            x.PtId,
                            x.PtNum,
                            x.KanaName ?? string.Empty,
                            x.Name ?? string.Empty,
                            x.Birthday,
                            raiinInfs.Any(s => s.PtId == x.PtId) ? raiinInfs.First(s => s.PtId == x.PtId).SinDate : 0
                            ))
                            .ToList();
            return result;
        }

        public List<PatientInforModel> SearchPatient(int hpId, int startDate, string startTime, int endDate, string endTime)
        {
            var startTimeFormat = (startTime + "00").PadLeft(6, '0');
            var endTimeFormat = (endTime + "60").PadLeft(6, '0');
            var ptIdList = NoTrackingDataContext.RaiinInfs.Where(item => item.HpId == hpId
                                                                         && item.Status >= RaiinState.Calculate
                                                                         && item.IsDeleted == DeleteTypes.None
                                                                         && (item.SinDate > startDate || (item.SinDate == startDate && string.Compare(item.UketukeTime, startTimeFormat) >= 0))
                                                                         && (item.SinDate < endDate || (item.SinDate == endDate && string.Compare(item.UketukeTime, endTimeFormat) <= 0)))
                                                           .Select(item => item.PtId)
                                                           .Distinct()
                                                           .ToList();

            var result = NoTrackingDataContext.PtInfs.Where(item => item.HpId == hpId
                                                                    && item.IsDelete != 1
                                                                    && ptIdList.Contains(item.PtId))
                                                     .Select(item => new PatientInforModel(
                                                                            item.PtId,
                                                                            item.PtNum,
                                                                            item.Name ?? string.Empty,
                                                                            item.KanaName ?? string.Empty,
                                                                            item.Sex,
                                                                            item.Birthday))
                                                     .ToList();
            return result;
        }

        public List<PatientInforModel> SearchPatient(int hpId, List<long> ptIdList)
        {
            ptIdList = ptIdList.Distinct().ToList();
            var result = NoTrackingDataContext.PtInfs.Where(item => item.HpId == hpId
                                                                    && item.IsDelete != 1
                                                                    && ptIdList.Contains(item.PtId))
                                                     .Select(item => new PatientInforModel(
                                                                            item.PtId,
                                                                            item.PtNum,
                                                                            item.Name ?? string.Empty,
                                                                            item.KanaName ?? string.Empty,
                                                                            item.Sex,
                                                                            item.Birthday))
                                                     .ToList();
            return result;
        }

        public bool IsRyosyoFuyou(int hpId, long ptId)
        {
            var ptInf = NoTrackingDataContext.PtInfs.FirstOrDefault(item => item.HpId == hpId && item.PtId == ptId);
            if (ptInf != null)
            {
                if (ptInf.IsRyosyoDetail == 0)
                {
                    return true;
                }
            }
            return false;
        }

        private class PatientInfQueryModel
        {
            public PtInf PtInf { get; set; } = new();

            public int LastVisitDate { get; set; }
        }

        private List<PatientInforModel> SortData(IQueryable<PatientInfQueryModel> ptInfWithLastVisitDate, Dictionary<string, string> sortData, int pageIndex, int pageSize)
        {
            if (!sortData.Any())
            {
                return ptInfWithLastVisitDate
                       .Skip((pageIndex - 1) * pageSize)
                       .Take(pageSize)
                       .AsEnumerable()
                       .Select(p => ToModel(p.PtInf, string.Empty, p.LastVisitDate))
                       .ToList();
            }
            int index = 1;
            var sortQuery = ptInfWithLastVisitDate.OrderBy(item => item.PtInf.PtId);
            foreach (var item in sortData)
            {
                int field = int.Parse(item.Key);
                string typeSort = item.Value.Replace(" ", string.Empty).ToLower();
                if (index == 1)
                {
                    sortQuery = OrderByAction((FieldSortPatientEnum)field, typeSort, sortQuery);
                    index++;
                    continue;
                }
                sortQuery = ThenOrderByAction((FieldSortPatientEnum)field, typeSort, sortQuery);
            }

            var result = sortQuery
                         .Skip((pageIndex - 1) * pageSize)
                         .Take(pageSize)
                         .AsEnumerable()
                         .Select(p => ToModel(p.PtInf, string.Empty, p.LastVisitDate))
                         .ToList();
            return result;
        }

        private IOrderedQueryable<PatientInfQueryModel> OrderByAction(FieldSortPatientEnum field, string typeSort, IOrderedQueryable<PatientInfQueryModel> sortQuery)
        {
            switch (field)
            {
                case FieldSortPatientEnum.PtId:
                    if (typeSort.Equals("desc"))
                    {
                        sortQuery = sortQuery.OrderByDescending(item => item.PtInf.PtId);
                    }
                    else
                    {
                        sortQuery = sortQuery.OrderBy(item => item.PtInf.PtId);
                    }
                    break;
                case FieldSortPatientEnum.PtNum:
                    if (typeSort.Equals("desc"))
                    {
                        sortQuery = sortQuery.OrderByDescending(item => item.PtInf.PtNum);
                    }
                    else
                    {
                        sortQuery = sortQuery.OrderBy(item => item.PtInf.PtNum);
                    }
                    break;
                case FieldSortPatientEnum.KanaName:
                    if (typeSort.Equals("desc"))
                    {
                        sortQuery = sortQuery.OrderByDescending(item => item.PtInf.KanaName);
                    }
                    else
                    {
                        sortQuery = sortQuery.OrderBy(item => item.PtInf.KanaName);
                    }
                    break;
                case FieldSortPatientEnum.Name:
                    if (typeSort.Equals("desc"))
                    {
                        sortQuery = sortQuery.OrderByDescending(item => item.PtInf.Name);
                    }
                    else
                    {
                        sortQuery = sortQuery.OrderBy(item => item.PtInf.Name);
                    }
                    break;
                case FieldSortPatientEnum.Birthday:
                    if (typeSort.Equals("desc"))
                    {
                        sortQuery = sortQuery.OrderByDescending(item => item.PtInf.Birthday);
                    }
                    else
                    {
                        sortQuery = sortQuery.OrderBy(item => item.PtInf.Birthday);
                    }
                    break;
                case FieldSortPatientEnum.Sex:
                    if (typeSort.Equals("desc"))
                    {
                        sortQuery = sortQuery.OrderByDescending(item => item.PtInf.Sex);
                    }
                    else
                    {
                        sortQuery = sortQuery.OrderBy(item => item.PtInf.Sex);
                    }
                    break;
                case FieldSortPatientEnum.Age:
                    if (typeSort.Equals("desc"))
                    {
                        sortQuery = sortQuery.OrderBy(item => item.PtInf.Birthday);
                    }
                    else
                    {
                        sortQuery = sortQuery.OrderByDescending(item => item.PtInf.Birthday);
                    }
                    break;
                case FieldSortPatientEnum.Tel1:
                    if (typeSort.Equals("desc"))
                    {
                        sortQuery = sortQuery.OrderByDescending(item => item.PtInf.Tel1);
                    }
                    else
                    {
                        sortQuery = sortQuery.OrderBy(item => item.PtInf.Tel1);
                    }
                    break;
                case FieldSortPatientEnum.Tel2:
                    if (typeSort.Equals("desc"))
                    {
                        sortQuery = sortQuery.OrderByDescending(item => item.PtInf.Tel2);
                    }
                    else
                    {
                        sortQuery = sortQuery.OrderBy(item => item.PtInf.Tel2);
                    }
                    break;
                case FieldSortPatientEnum.RenrakuTel:
                    if (typeSort.Equals("desc"))
                    {
                        sortQuery = sortQuery.OrderByDescending(item => item.PtInf.RenrakuTel);
                    }
                    else
                    {
                        sortQuery = sortQuery.OrderBy(item => item.PtInf.RenrakuTel);
                    }
                    break;
                case FieldSortPatientEnum.HomePost:
                    if (typeSort.Equals("desc"))
                    {
                        sortQuery = sortQuery.OrderByDescending(item => item.PtInf.HomePost);
                    }
                    else
                    {
                        sortQuery = sortQuery.OrderBy(item => item.PtInf.HomePost);
                    }
                    break;
                case FieldSortPatientEnum.HomeAddress:
                    if (typeSort.Equals("desc"))
                    {
                        sortQuery = sortQuery.OrderByDescending(item => item.PtInf.HomeAddress1 + '\u3000' + item.PtInf.HomeAddress2);
                    }
                    else
                    {
                        sortQuery = sortQuery.OrderBy(item => item.PtInf.HomeAddress1 + '\u3000' + item.PtInf.HomeAddress2);
                    }
                    break;
                case FieldSortPatientEnum.LastVisitDate:
                    if (typeSort.Equals("desc"))
                    {
                        sortQuery = sortQuery.OrderByDescending(item => item.LastVisitDate);
                    }
                    else
                    {
                        sortQuery = sortQuery.OrderBy(item => item.LastVisitDate);
                    }
                    break;
            }
            return sortQuery;
        }

        private IOrderedQueryable<PatientInfQueryModel> ThenOrderByAction(FieldSortPatientEnum field, string typeSort, IOrderedQueryable<PatientInfQueryModel> sortQuery)
        {
            switch (field)
            {
                case FieldSortPatientEnum.PtId:
                    if (typeSort.Equals("desc"))
                    {
                        sortQuery = sortQuery.ThenByDescending(item => item.PtInf.PtId);
                    }
                    else
                    {
                        sortQuery = sortQuery.ThenBy(item => item.PtInf.PtId);
                    }
                    break;
                case FieldSortPatientEnum.PtNum:
                    if (typeSort.Equals("desc"))
                    {
                        sortQuery = sortQuery.ThenByDescending(item => item.PtInf.PtNum);
                    }
                    else
                    {
                        sortQuery = sortQuery.ThenBy(item => item.PtInf.PtNum);
                    }
                    break;
                case FieldSortPatientEnum.KanaName:
                    if (typeSort.Equals("desc"))
                    {
                        sortQuery = sortQuery.ThenByDescending(item => item.PtInf.KanaName);
                    }
                    else
                    {
                        sortQuery = sortQuery.ThenBy(item => item.PtInf.KanaName);
                    }
                    break;
                case FieldSortPatientEnum.Name:
                    if (typeSort.Equals("desc"))
                    {
                        sortQuery = sortQuery.ThenByDescending(item => item.PtInf.Name);
                    }
                    else
                    {
                        sortQuery = sortQuery.ThenBy(item => item.PtInf.Name);
                    }
                    break;
                case FieldSortPatientEnum.Birthday:
                    if (typeSort.Equals("desc"))
                    {
                        sortQuery = sortQuery.ThenByDescending(item => item.PtInf.Birthday);
                    }
                    else
                    {
                        sortQuery = sortQuery.ThenBy(item => item.PtInf.Birthday);
                    }
                    break;
                case FieldSortPatientEnum.Sex:
                    if (typeSort.Equals("desc"))
                    {
                        sortQuery = sortQuery.ThenByDescending(item => item.PtInf.Sex);
                    }
                    else
                    {
                        sortQuery = sortQuery.ThenBy(item => item.PtInf.Sex);
                    }
                    break;
                case FieldSortPatientEnum.Age:
                    if (typeSort.Equals("desc"))
                    {
                        sortQuery = sortQuery.ThenBy(item => item.PtInf.Birthday);
                    }
                    else
                    {
                        sortQuery = sortQuery.ThenByDescending(item => item.PtInf.Birthday);
                    }
                    break;
                case FieldSortPatientEnum.Tel1:
                    if (typeSort.Equals("desc"))
                    {
                        sortQuery = sortQuery.ThenByDescending(item => item.PtInf.Tel1);
                    }
                    else
                    {
                        sortQuery = sortQuery.ThenBy(item => item.PtInf.Tel1);
                    }
                    break;
                case FieldSortPatientEnum.Tel2:
                    if (typeSort.Equals("desc"))
                    {
                        sortQuery = sortQuery.ThenByDescending(item => item.PtInf.Tel2);
                    }
                    else
                    {
                        sortQuery = sortQuery.ThenBy(item => item.PtInf.Tel2);
                    }
                    break;
                case FieldSortPatientEnum.RenrakuTel:
                    if (typeSort.Equals("desc"))
                    {
                        sortQuery = sortQuery.ThenByDescending(item => item.PtInf.RenrakuTel);
                    }
                    else
                    {
                        sortQuery = sortQuery.ThenBy(item => item.PtInf.RenrakuTel);
                    }
                    break;
                case FieldSortPatientEnum.HomePost:
                    if (typeSort.Equals("desc"))
                    {
                        sortQuery = sortQuery.ThenByDescending(item => item.PtInf.HomePost);
                    }
                    else
                    {
                        sortQuery = sortQuery.ThenBy(item => item.PtInf.HomePost);
                    }
                    break;
                case FieldSortPatientEnum.HomeAddress:
                    if (typeSort.Equals("desc"))
                    {
                        sortQuery = sortQuery.ThenByDescending(item => item.PtInf.HomeAddress1 + '\u3000' + item.PtInf.HomeAddress2);
                    }
                    else
                    {
                        sortQuery = sortQuery.ThenBy(item => item.PtInf.HomeAddress1 + '\u3000' + item.PtInf.HomeAddress2);
                    }
                    break;
                case FieldSortPatientEnum.LastVisitDate:
                    if (typeSort.Equals("desc"))
                    {
                        sortQuery = sortQuery.ThenByDescending(item => item.LastVisitDate);
                    }
                    else
                    {
                        sortQuery = sortQuery.ThenBy(item => item.LastVisitDate);
                    }
                    break;
            }
            return sortQuery;
        }

        public long GetPtIdFromPtNum(int hpId, long ptNum)
        {
            var ptInf = NoTrackingDataContext.PtInfs.FirstOrDefault(item => item.HpId == hpId
                                                                                            && item.PtNum == ptNum);
            if (ptInf != null)
            {
                return ptInf.PtId;
            }
            return 0;
        }

        public int GetCountRaiinAlreadyPaidOfPatientByDate(int fromDate, int toDate, long ptId, int raiintStatus)
        {
            return NoTrackingDataContext.RaiinInfs.Count(u => u.PtId == ptId &&
                                                                              u.SinDate >= fromDate &&
                                                                              u.SinDate <= toDate &&
                                                                              u.Status >= raiintStatus &&
                                                                              u.IsDeleted == DeleteTypes.None);
        }

        public List<PatientInforModel> FindSamePatient(int hpId, string kanjiName, int sex, int birthDay)
        {
            kanjiName = kanjiName.Replace("　", " ");
            return NoTrackingDataContext.PtInfs.Where(p => p.HpId == hpId
                                                        && p.Name != null && p.Name.Replace("　", " ") == kanjiName
                                                        && p.Sex == sex
                                                        && p.Birthday == birthDay
                                                        && p.IsDelete != DeleteTypes.Deleted)
                                               .Select(x => new PatientInforModel(x.HpId,
                                                                                  x.PtId,
                                                                                  x.ReferenceNo,
                                                                                  x.SeqNo,
                                                                                  x.PtNum,
                                                                                  x.KanaName ?? string.Empty,
                                                                                  x.Name ?? string.Empty,
                                                                                  x.Sex,
                                                                                  x.Birthday,
                                                                                  x.LimitConsFlg,
                                                                                  x.IsDead,
                                                                                  x.DeathDate,
                                                                                  x.HomePost ?? string.Empty,
                                                                                  x.HomeAddress1 ?? string.Empty,
                                                                                  x.HomeAddress2 ?? string.Empty,
                                                                                  x.Tel1 ?? string.Empty,
                                                                                  x.Tel2 ?? string.Empty,
                                                                                  x.Mail ?? string.Empty,
                                                                                  x.Setanusi ?? string.Empty,
                                                                                  x.Zokugara ?? string.Empty,
                                                                                  x.Job ?? string.Empty,
                                                                                  x.RenrakuName ?? string.Empty,
                                                                                  x.RenrakuPost ?? string.Empty,
                                                                                  x.RenrakuAddress1 ?? string.Empty,
                                                                                  x.RenrakuAddress2 ?? string.Empty,
                                                                                  x.RenrakuTel ?? string.Empty,
                                                                                  x.RenrakuMemo ?? string.Empty,
                                                                                  x.OfficeName ?? string.Empty,
                                                                                  x.OfficePost ?? string.Empty,
                                                                                  x.OfficeAddress1 ?? string.Empty,
                                                                                  x.OfficeAddress2 ?? string.Empty,
                                                                                  x.OfficeTel ?? string.Empty,
                                                                                  x.OfficeMemo ?? string.Empty,
                                                                                  x.IsRyosyoDetail,
                                                                                  x.PrimaryDoctor,
                                                                                  x.IsTester,
                                                                                  x.MainHokenPid,
                                                                                  string.Empty,
                                                                                  0,
                                                                                  0,
                                                                                  0,
                                                                                  string.Empty,
                                                                                  0,
                                                                                  false)).ToList();
        }

        public bool SavePtKyusei(int hpId, int userId, List<PtKyuseiModel> ptKyuseiList)
        {
            var seqNoList = ptKyuseiList.Select(item => item.SeqNo).Distinct().ToList();
            var ptKyuseiDBList = TrackingDataContext.PtKyuseis.Where(item => item.HpId == hpId && seqNoList.Contains(item.SeqNo)).ToList();
            foreach (var model in ptKyuseiList)
            {
                var entity = ptKyuseiDBList.FirstOrDefault(entity => entity.SeqNo == model.SeqNo && entity.PtId == model.PtId);
                if (entity == null)
                {
                    entity = new PtKyusei();
                    entity.HpId = hpId;
                    entity.SeqNo = 0;
                    entity.IsDeleted = 0;
                    entity.CreateDate = CIUtil.GetJapanDateTimeNow();
                    entity.CreateId = userId;
                    entity.PtId = model.PtId;
                }
                entity.Name = model.Name;
                entity.KanaName = model.KanaName;
                entity.EndDate = model.EndDate;
                entity.UpdateDate = CIUtil.GetJapanDateTimeNow();
                entity.UpdateId = userId;
                if (model.IsDeleted)
                {
                    entity.IsDeleted = 1;
                }
                if (entity.SeqNo == 0)
                {
                    TrackingDataContext.PtKyuseis.Add(entity);
                }
            }
            return TrackingDataContext.SaveChanges() > 0;
        }

        private void CloneByomeiWithNewHokenId(List<PtByomei> ptByomeis, int hokenId, int userId)
        {
            List<PtByomei> newCloneByomeis = new();
            foreach (var ptByomei in ptByomeis)
            {
                var cloneByomei = ptByomei.Clone();
                cloneByomei.CreateId = userId;
                cloneByomei.UpdateId = userId;
                cloneByomei.CreateDate = CIUtil.GetJapanDateTimeNow();
                cloneByomei.UpdateDate = CIUtil.GetJapanDateTimeNow();
                cloneByomei.HokenPid = hokenId;
                cloneByomei.Id = 0;
                newCloneByomeis.Add(cloneByomei);
            }
            TrackingDataContext.PtByomeis.AddRange(newCloneByomeis);

            foreach (var newCloneByomei in newCloneByomeis)
            {
                newCloneByomei.SeqNo = newCloneByomei.Id;
            }
        }

        public List<VisitTimesManagementModel> GetVisitTimesManagementModels(int hpId, int sinYm, long ptId, int kohiId)
        {
            var limitCntListInfList = NoTrackingDataContext.LimitCntListInfs.Where(item => item.HpId == hpId
                                                                                           && item.SinDate / 100 == sinYm
                                                                                           && item.PtId == ptId
                                                                                           && item.KohiId == kohiId
                                                                                           && item.IsDeleted == DeleteTypes.None)
                                                                            .ToList();
            return limitCntListInfList.Select(item => new VisitTimesManagementModel(
                                                          item.PtId,
                                                          item.SinDate,
                                                          item.HokenPid,
                                                          item.KohiId,
                                                          item.SeqNo,
                                                          item.SortKey ?? string.Empty))
                                      .OrderBy(item => item.SortKey)
                                      .ToList();
        }

        public bool UpdateVisitTimesManagement(int hpId, int userId, long ptId, int kohiId, int sinYm, List<VisitTimesManagementModel> visitTimesManagementList)
        {
            var limitCntListInfDBList = TrackingDataContext.LimitCntListInfs.Where(item => item.HpId == hpId
                                                                                           && item.PtId == ptId
                                                                                           && item.KohiId == kohiId)
                                                                            .ToList();
            var maxSeqNo = limitCntListInfDBList.Any() ? limitCntListInfDBList.Max(item => item.SeqNo) : 0;
            limitCntListInfDBList = limitCntListInfDBList.Where(item => item.IsDeleted == 0
                                                                        && item.SinDate / 100 == sinYm)
                                                         .ToList();

            var seqNoList = visitTimesManagementList.Where(item => item.SeqNo >= 0).Select(item => item.SeqNo).Distinct().ToList();
            var deletedVisitTimeList = limitCntListInfDBList.Where(item => item.HokenPid == 0 && !seqNoList.Contains(item.SeqNo)).ToList();
            foreach (var item in deletedVisitTimeList)
            {
                item.IsDeleted = 1;
                item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                item.UpdateId = userId;
            }

            foreach (var model in visitTimesManagementList)
            {
                bool isAddNew = false;
                var entity = limitCntListInfDBList.FirstOrDefault(item => model.SeqNo > 0 && item.SeqNo == model.SeqNo);
                if (entity == null)
                {
                    if (model.SeqNo == 0 && model.IsOutHospital)
                    {
                        entity = new LimitCntListInf();
                        entity.HpId = hpId;
                        entity.PtId = ptId;
                        entity.KohiId = kohiId;
                        entity.SinDate = model.SinDate;
                        entity.SeqNo = maxSeqNo + 1;
                        entity.IsDeleted = 0;
                        entity.CreateDate = CIUtil.GetJapanDateTimeNow();
                        entity.CreateId = userId;
                        entity.SortKey = model.SortKey;
                        maxSeqNo = entity.SeqNo;
                        isAddNew = true;
                    }
                    else
                    {
                        continue;
                    }
                }
                entity.UpdateDate = CIUtil.GetJapanDateTimeNow();
                entity.UpdateId = userId;
                if (model.IsDeleted)
                {
                    entity.IsDeleted = 1;
                }
                if (isAddNew)
                {
                    TrackingDataContext.LimitCntListInfs.Add(entity);
                }
            }
            return TrackingDataContext.SaveChanges() > 0;
        }

        public bool UpdateVisitTimesManagementNeedSave(int hpId, int userId, long ptId, List<VisitTimesManagementModel> visitTimesManagementList)
        {
            var kohiIdList = visitTimesManagementList.Select(item => item.KohiId).Distinct().ToList();
            var limitCntListInfDBList = TrackingDataContext.LimitCntListInfs.Where(item => item.HpId == hpId
                                                                                           && item.PtId == ptId
                                                                                           && kohiIdList.Contains(item.KohiId))
                                                                            .ToList();
            foreach (var kohiId in kohiIdList)
            {
                var visitTimesModelList = visitTimesManagementList.Where(item => item.KohiId == kohiId).ToList();
                var limitCntListInfByKohiDBList = limitCntListInfDBList.Where(item => item.KohiId == kohiId).ToList();
                var maxSeqNo = limitCntListInfByKohiDBList.Any() ? limitCntListInfByKohiDBList.Max(item => item.SeqNo) : 0;

                foreach (var model in visitTimesModelList)
                {
                    var limitCntListInf = new LimitCntListInf()
                    {
                        HpId = hpId,
                        CreateId = userId,
                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                        UpdateId = userId,
                        UpdateDate = CIUtil.GetJapanDateTimeNow(),
                        PtId = ptId,
                        SinDate = model.SinDate,
                        KohiId = kohiId,
                        SeqNo = maxSeqNo + 1,
                        SortKey = model.SortKey,
                    };
                    TrackingDataContext.LimitCntListInfs.Add(limitCntListInf);
                    maxSeqNo = limitCntListInf.SeqNo;
                }
            }
            return TrackingDataContext.SaveChanges() > 0;
        }
    }
}