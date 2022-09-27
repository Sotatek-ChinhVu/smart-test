using Domain.Models.Insurance;
using Domain.Models.Ka;
using Domain.Models.KarteFilterMst;
using Domain.Models.KarteInfs;
using Domain.Models.KarteKbnMst;
using Domain.Models.OrdInfs;
using Domain.Models.RainListTag;
using Domain.Models.Reception;
using Domain.Models.User;
using UseCase.MedicalExamination.GetHistory;
using UseCase.OrdInfs.GetListTrees;

namespace Interactor.MedicalExamination
{
    public class GetMedicalExaminationHistoryInteractor : IGetMedicalExaminationHistoryInputPort
    {
        private readonly IOrdInfRepository _ordInfRepository;
        private readonly IKarteInfRepository _karteInfRepository;
        private readonly IKarteKbnMstRepository _karteKbnRepository;
        private readonly IReceptionRepository _receptionRepository;
        private readonly IInsuranceRepository _insuranceRepository;
        private readonly IUserRepository _userRepository;
        private readonly IKaRepository _kaRepository;
        private readonly IKarteFilterMstRepository _karteFilterMstRepository;
        private readonly IRaiinListTagRepository _rainListTagRepository;
        public GetMedicalExaminationHistoryInteractor(IOrdInfRepository ordInfRepository, IKarteInfRepository karteInfRepository, IKarteKbnMstRepository karteKbnRepository, IReceptionRepository receptionRepository, IInsuranceRepository insuranceRepository, IUserRepository userRepository, IKaRepository kaRepository, IKarteFilterMstRepository karteFilterMstRepository, IRaiinListTagRepository rainListTagRepository)
        {
            _ordInfRepository = ordInfRepository;
            _karteInfRepository = karteInfRepository;
            _karteKbnRepository = karteKbnRepository;
            _receptionRepository = receptionRepository;
            _insuranceRepository = insuranceRepository;
            _userRepository = userRepository;
            _kaRepository = kaRepository;
            _karteFilterMstRepository = karteFilterMstRepository;
            _rainListTagRepository = rainListTagRepository;
        }

        public GetMedicalExaminationHistoryOutputData Handle(GetMedicalExaminationHistoryInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new GetMedicalExaminationHistoryOutputData(0, new List<HistoryKarteOdrRaiinItem>(), GetMedicalExaminationHistoryStatus.InvalidHpId, 0);
                }
                if (inputData.StartPage < 0)
                {
                    return new GetMedicalExaminationHistoryOutputData(0, new List<HistoryKarteOdrRaiinItem>(), GetMedicalExaminationHistoryStatus.InvalidStartPage, 0);
                }
                if (inputData.PtId <= 0)
                {
                    return new GetMedicalExaminationHistoryOutputData(0, new List<HistoryKarteOdrRaiinItem>(), GetMedicalExaminationHistoryStatus.InvalidPtId, 0);
                }
                if (inputData.SinDate <= 0)
                {
                    return new GetMedicalExaminationHistoryOutputData(0, new List<HistoryKarteOdrRaiinItem>(), GetMedicalExaminationHistoryStatus.InvalidSinDate, 0);
                }
                if (inputData.PageSize <= 0)
                {
                    return new GetMedicalExaminationHistoryOutputData(0, new List<HistoryKarteOdrRaiinItem>(), GetMedicalExaminationHistoryStatus.InvalidPageSize, 0);
                }
                if (!(inputData.DeleteConditon >= 0 && inputData.DeleteConditon <= 2))
                {
                    return new GetMedicalExaminationHistoryOutputData(0, new List<HistoryKarteOdrRaiinItem>(), GetMedicalExaminationHistoryStatus.InvalidDeleteCondition, 0);
                }
                if (inputData.UserId <= 0)
                {
                    return new GetMedicalExaminationHistoryOutputData(0, new List<HistoryKarteOdrRaiinItem>(), GetMedicalExaminationHistoryStatus.InvalidUserId, 0);
                }
                if (inputData.FilterId < 0)
                {
                    return new GetMedicalExaminationHistoryOutputData(0, new List<HistoryKarteOdrRaiinItem>(), GetMedicalExaminationHistoryStatus.InvalidFilterId, 0);
                }
                if (!(inputData.SearchType >= 0 && inputData.SearchType <= 2))
                {
                    return new GetMedicalExaminationHistoryOutputData(0, new List<HistoryKarteOdrRaiinItem>(), GetMedicalExaminationHistoryStatus.InvalidSearchType, 0);
                }
                if ((inputData.SearchType != 0 && !(inputData.SearchCategory >= 1 && inputData.SearchType <= 3)) || (inputData.SearchType == 0 && inputData.SearchCategory != 0))
                {
                    return new GetMedicalExaminationHistoryOutputData(0, new List<HistoryKarteOdrRaiinItem>(), GetMedicalExaminationHistoryStatus.InvalidSearchCategory, 0);
                }
                if (string.IsNullOrEmpty(inputData.SearchText.Trim()) && inputData.SearchType != 0)
                {
                    return new GetMedicalExaminationHistoryOutputData(0, new List<HistoryKarteOdrRaiinItem>(), GetMedicalExaminationHistoryStatus.InvalidSearchText, 0);
                }

                bool allowDisplayDeleted = inputData.KarteDeleteHistory > 0;

                var karteFilter = inputData.FilterId == 0 ? null : _karteFilterMstRepository.Get(inputData.HpId, inputData.UserId, inputData.FilterId);
                IEnumerable<ReceptionModel>? query;

                if (karteFilter?.OnlyBookmark == true)
                {
                    query = from raiinInf in _receptionRepository.GetList(inputData.HpId, inputData.PtId, inputData.KarteDeleteHistory).Where(r => (karteFilter.IsAllDepartment || karteFilter.ListDepartmentCode.Contains(r.KaId)) &&
                                (karteFilter.IsAllDoctor || karteFilter.ListDoctorCode.Contains(r.TantoId)))
                            join raiinListTag in _rainListTagRepository.GetList(inputData.HpId, inputData.PtId, true)
                            on raiinInf.RaiinNo equals raiinListTag.RaiinNo
                            join ptHokenPattern in _insuranceRepository.GetListHokenPattern(inputData.HpId, inputData.PtId, allowDisplayDeleted, karteFilter.IsAllHoken, karteFilter.IsHoken, karteFilter.IsJihi, karteFilter.IsRosai, karteFilter.IsJibai)
                            on raiinInf.HokenPid equals ptHokenPattern.HokenPid
                            select raiinInf;
                }
                else
                {
                    query = from raiinInf in _receptionRepository.GetList(inputData.HpId, inputData.PtId, inputData.KarteDeleteHistory)
                            join ptHokenPattern in _insuranceRepository.GetListHokenPattern(inputData.HpId, inputData.PtId, allowDisplayDeleted)
                            on raiinInf.HokenPid equals ptHokenPattern.HokenPid
                            select raiinInf;
                }

                var allRaiinInf = query?.ToList();

                var pageTotal = allRaiinInf?.Count ?? 0;
                if (pageTotal == 0)
                    return new GetMedicalExaminationHistoryOutputData(0, new List<HistoryKarteOdrRaiinItem>(), GetMedicalExaminationHistoryStatus.NoData, 0);
                if (inputData.StartPage >= pageTotal)
                {
                    return new GetMedicalExaminationHistoryOutputData(0, new List<HistoryKarteOdrRaiinItem>(), GetMedicalExaminationHistoryStatus.InvalidStartPage, 0);
                }

                var allRaiinNos = allRaiinInf?.Select(q => q.RaiinNo)?.ToList();
                var raiinNoStartPage = !(allRaiinNos?.Count() > 0) ? 0 : allRaiinNos[inputData.StartPage];

                long raiinNoMark = -1;
                if (inputData.SearchType != 0)
                {
                    var rainNoMarkKarte = _karteInfRepository.GetRaiinNo(inputData.PtId, inputData.HpId, inputData.SearchType, raiinNoStartPage, inputData.SearchText);
                    var rainNoMarkOdr = _ordInfRepository.GetRaiinNo(inputData.PtId, inputData.HpId, inputData.SearchType, raiinNoStartPage, inputData.SearchText);

                    if (inputData.SearchCategory == 1)
                    {
                        raiinNoMark = rainNoMarkKarte;
                    }
                    else if (inputData.SearchCategory == 2)
                    {
                        raiinNoMark = rainNoMarkOdr;
                    }
                    else
                    {
                        if (inputData.SearchType == 1)
                        {
                            if (rainNoMarkKarte >= 0 && rainNoMarkOdr >= 0)
                                raiinNoMark = Math.Min(rainNoMarkKarte, rainNoMarkOdr);
                            else if (rainNoMarkKarte >= 0 || rainNoMarkOdr >= 0)
                                raiinNoMark = Math.Max(rainNoMarkKarte, rainNoMarkOdr);
                        }
                        else
                        {
                            raiinNoMark = Math.Max(rainNoMarkKarte, rainNoMarkOdr);

                        }
                    }
                }

                List<ReceptionModel>? rainInfs;
                var startPageSearch = 0;

                if (inputData.SearchType == 0)
                {
                    rainInfs = allRaiinInf?.Skip(inputData.StartPage).Take(inputData.PageSize).ToList();
                }
                else
                {
                    if (raiinNoMark == -1) rainInfs = null;
                    else
                    {
                        var rainMarkObj = allRaiinInf?.FirstOrDefault(r => r.RaiinNo == raiinNoMark);
                        var index = rainMarkObj == null ? 0 : allRaiinInf?.IndexOf(rainMarkObj) ?? 0;

                        if (inputData.SearchType == 1)
                        {
                            rainInfs = allRaiinInf?.Where(r => r.RaiinNo <= raiinNoMark).Take(inputData.PageSize).ToList();
                        }
                        else
                        {
                            if (index < inputData.PageSize)
                            {
                                rainInfs = allRaiinInf?.Take(inputData.PageSize).ToList();
                                index = 0;
                            }
                            else
                            {
                                rainInfs = allRaiinInf?.Skip(index + 1 - inputData.PageSize).Take(inputData.PageSize).ToList();
                                index = index - inputData.PageSize + 1;
                            }
                        }
                        startPageSearch = index;
                    }
                }

                var raiinNos = rainInfs?.Select(q => q.RaiinNo)?.ToList();
                var tantoIds = rainInfs?.Select(r => r.TantoId).ToList();
                var kaIds = rainInfs?.Select(r => r.TantoId).ToList();
                var sinDates = rainInfs?.Select(r => r.SinDate).ToList();
                var historyKarteOdrRaiins = new List<HistoryKarteOdrRaiinItem>();

                #region karte
                var allkarteKbns = _karteKbnRepository.GetList(inputData.HpId, true);
                var allkarteInfs = raiinNos == null ? new List<KarteInfModel>() : _karteInfRepository.GetList(inputData.PtId, inputData.HpId, inputData.DeleteConditon, raiinNos).OrderBy(c => c.KarteKbn).ToList();
                #endregion

                #region Odr
                var allOdrInfs = raiinNos == null ? new List<OrdInfModel>() : _ordInfRepository
               .GetList(inputData.PtId, inputData.HpId, inputData.UserId, inputData.DeleteConditon, raiinNos)
                                                     .OrderBy(odr => odr.OdrKouiKbn)
                                                     .ThenBy(odr => odr.RpNo)
                                                     .ThenBy(odr => odr.RpEdaNo)
                                                     .ThenBy(odr => odr.SortNo)
                                                     .ToList();

                var insuranceData = _insuranceRepository.GetInsuranceListById(inputData.HpId, inputData.PtId, inputData.SinDate);
                var hokenFirst = insuranceData?.ListInsurance.FirstOrDefault();
                var doctors = tantoIds == null ? new List<UserMstModel>() : _userRepository.GetDoctorsList(tantoIds)?.ToList();
                var kaMsts = kaIds == null ? new List<KaMstModel>() : _kaRepository.GetByKaIds(kaIds)?.ToList();
                var raiinListTags = (sinDates == null || raiinNos == null) ? new List<RaiinListTagModel>() : _rainListTagRepository.GetList(inputData.HpId, inputData.PtId, false, sinDates, raiinNos)?.ToList();
                IEnumerable<ApproveInfModel>? approveInfs = null;

                if (!(rainInfs?.Count > 0))
                    return new GetMedicalExaminationHistoryOutputData(0, new List<HistoryKarteOdrRaiinItem>(), GetMedicalExaminationHistoryStatus.NoData, 0);

                if (inputData.IsShowApproval == 1 || inputData.IsShowApproval == 2)
                {
                    approveInfs = raiinNos == null ? new List<ApproveInfModel>() : _ordInfRepository.GetApproveInf(inputData.HpId, inputData.PtId, inputData.IsShowApproval == 2, raiinNos);
                }

                if (!(rainInfs?.Count > 0))
                    return new GetMedicalExaminationHistoryOutputData(0, new List<HistoryKarteOdrRaiinItem>(), GetMedicalExaminationHistoryStatus.NoData, 0);

                Parallel.ForEach(rainInfs, raiinInf =>
                {
                    var doctorFirst = doctors?.FirstOrDefault(d => d.UserId == raiinInf.TantoId);
                    var kaMst = kaMsts?.FirstOrDefault(k => k.KaId == raiinInf.KaId);
                    var raiinTag = raiinListTags?.FirstOrDefault(r => r.RaiinNo == raiinInf.RaiinNo && r.SinDate == raiinInf.SinDate);
                    var approveInf = approveInfs?.FirstOrDefault(a => a.RaiinNo == raiinInf.RaiinNo);

                    var historyKarteOdrRaiin = new HistoryKarteOdrRaiinItem(raiinInf.RaiinNo, raiinInf.SinDate, raiinInf.HokenPid, hokenFirst == null ? string.Empty : hokenFirst.HokenName, hokenFirst == null ? string.Empty : hokenFirst.DisplayRateOnly, raiinInf.SyosaisinKbn, raiinInf.JikanKbn, raiinInf.KaId, kaMst == null ? String.Empty : kaMst.KaName, raiinInf.TantoId, doctorFirst == null ? String.Empty : doctorFirst.Sname, raiinInf.SanteiKbn, raiinTag?.TagNo ?? 0, approveInf?.DisplayApprovalInfo ?? string.Empty, GetHokenPatternType(hokenFirst?.HokenKbn ?? 0), new List<HokenGroupHistoryItem>(), new List<GrpKarteHistoryItem>());

                    List<KarteInfModel> karteInfByRaiinNo = allkarteInfs.Where(odr => odr.RaiinNo == historyKarteOdrRaiin.RaiinNo).OrderBy(c => c.KarteKbn).ThenByDescending(c => c.IsDeleted).ThenBy(c => c.CreateDate).ThenBy(c => c.UpdateDate).ToList();

                    historyKarteOdrRaiin.KarteHistories.AddRange(from karteKbn in allkarteKbns
                                                                 where karteInfByRaiinNo.Any(c => c.KarteKbn == karteKbn.KarteKbn)
                                                                 let karteGrp = new GrpKarteHistoryItem(karteKbn == null ? 0 : karteKbn.KarteKbn, string.IsNullOrEmpty(karteKbn?.KbnName) ? String.Empty : karteKbn.KbnName, string.IsNullOrEmpty(karteKbn?.KbnShortName) ? String.Empty : karteKbn.KbnShortName, karteKbn == null ? 0 : karteKbn.CanImg, karteKbn == null ? 0 : karteKbn.SortNo, karteInfByRaiinNo.Where(c => c.KarteKbn == karteKbn?.KarteKbn).OrderByDescending(c => c.IsDeleted)
                    .Select(c => new KarteInfHistoryItem(
                                        c.HpId,
                                        c.RaiinNo,
                                        c.KarteKbn,
                                        c.SeqNo,
                                        c.PtId,
                                        c.SinDate,
                                        c.Text,
                                        c.CreateDate,
                                        c.UpdateDate,
                                        c.IsDeleted,
                                        c.RichText,
                                        c.CreateName
                                        )
                    ).ToList())
                                                                 select karteGrp);

                    var odrInfListByRaiinNo = allOdrInfs
                        .Where(o => o.RaiinNo == historyKarteOdrRaiin.RaiinNo).Select(
                               o => o.ChangeOdrDetail(o.OrdInfDetails.Where(od => od.RaiinNo == historyKarteOdrRaiin.RaiinNo).
                        ToList()));
                    odrInfListByRaiinNo = odrInfListByRaiinNo.OrderBy(odr => odr.OdrKouiKbn)
                                              .ThenBy(odr => odr.RpNo)
                                              .ThenBy(odr => odr.RpEdaNo)
                                              .ThenBy(odr => odr.SortNo)
                                              .ToList();

                    // Find By Hoken
                    List<int> hokenPidList = odrInfListByRaiinNo.GroupBy(odr => odr.HokenPid).Select(grp => grp.Key).ToList();

                    Parallel.ForEach(hokenPidList, hokenPid =>
                    {
                        var hoken = insuranceData?.ListInsurance.FirstOrDefault(c => c.HokenId == hokenPid);
                        var hokenGrp = new HokenGroupHistoryItem(hokenPid, hoken == null ? string.Empty : hoken.HokenName, new List<GroupOdrGHistoryItem>());

                        var groupOdrInfList = odrInfListByRaiinNo.Where(odr => odr.HokenPid == hokenPid)
                            .GroupBy(odr => new
                            {
                                odr.HokenPid,
                                odr.GroupKoui,
                                odr.InoutKbn,
                                odr.SyohoSbt,
                                odr.SikyuKbn,
                                odr.TosekiKbn,
                                odr.SanteiKbn
                            })
                            .Select(grp => grp.FirstOrDefault())
                            .ToList();

                        Parallel.ForEach(groupOdrInfList, groupOdrInf =>
                        {
                            var group = new GroupOdrGHistoryItem(hokenPid, string.Empty, new List<OdrInfHistoryItem>());

                            var rpOdrInfs = odrInfListByRaiinNo.Where(odrInf => odrInf.HokenPid == hokenPid
                                                        && odrInf.GroupKoui.Value == groupOdrInf?.GroupKoui.Value
                                                        && odrInf.InoutKbn == groupOdrInf?.InoutKbn
                                                        && odrInf.SyohoSbt == groupOdrInf?.SyohoSbt
                                                        && odrInf.SikyuKbn == groupOdrInf?.SikyuKbn
                                                        && odrInf.TosekiKbn == groupOdrInf?.TosekiKbn
                                                        && odrInf.SanteiKbn == groupOdrInf?.SanteiKbn)
                                                    .ToList();

                            //_mapper.Map<OdrInfModel>(c)
                            Parallel.ForEach(rpOdrInfs.OrderBy(c => c.IsDeleted), rpOdrInf =>
                            {

                                var odrModel = new OdrInfHistoryItem(
                                                                rpOdrInf.HpId,
                                                                rpOdrInf.RaiinNo,
                                                                rpOdrInf.RpNo,
                                                                rpOdrInf.RpEdaNo,
                                                                rpOdrInf.PtId,
                                                                rpOdrInf.SinDate,
                                                                rpOdrInf.HokenPid,
                                                                rpOdrInf.OdrKouiKbn,
                                                                rpOdrInf.RpName,
                                                                rpOdrInf.InoutKbn,
                                                                rpOdrInf.SikyuKbn,
                                                                rpOdrInf.SyohoSbt,
                                                                rpOdrInf.SanteiKbn,
                                                                rpOdrInf.TosekiKbn,
                                                                rpOdrInf.DaysCnt,
                                                                rpOdrInf.SortNo,
                                                                rpOdrInf.Id,
                                                                rpOdrInf.GroupKoui.Value,
                                                                rpOdrInf.OrdInfDetails.Select(od =>
                                                                    new OdrInfDetailItem(
                                                                        od.HpId,
                                                                        od.RaiinNo,
                                                                        od.RpNo,
                                                                        od.RpEdaNo,
                                                                        od.RowNo,
                                                                        od.PtId,
                                                                        od.SinDate,
                                                                        od.SinKouiKbn,
                                                                        od.ItemCd,
                                                                        od.ItemName,
                                                                        od.Suryo,
                                                                        od.UnitName,
                                                                        od.UnitSbt,
                                                                        od.TermVal,
                                                                        od.KohatuKbn,
                                                                        od.SyohoKbn,
                                                                        od.SyohoLimitKbn,
                                                                        od.DrugKbn,
                                                                        od.YohoKbn,
                                                                        od.Kokuji1,
                                                                        od.Kokuji2,
                                                                        od.IsNodspRece,
                                                                        od.IpnCd,
                                                                        od.IpnName,
                                                                        od.JissiKbn,
                                                                        od.JissiDate,
                                                                        od.JissiId,
                                                                        od.JissiMachine,
                                                                        od.ReqCd,
                                                                        od.Bunkatu,
                                                                        od.CmtName,
                                                                        od.CmtName,
                                                                        od.FontColor,
                                                                        od.CommentNewline,
                                                                        od.Yakka,
                                                                        od.IsGetPriceInYakka,
                                                                        od.Ten,
                                                                        od.BunkatuKoui,
                                                                        od.AlternationIndex,
                                                                        od.KensaGaichu,
                                                                        od.OdrTermVal,
                                                                        od.CnvTermVal,
                                                                        od.YjCd,
                                                                        od.MasterSbt,
                                                                        od.YohoSets,
                                                                        od.Kasan1,
                                                                        od.Kasan2
                                                                )
                                                                ).ToList(),
                                                                rpOdrInf.CreateDate,
                                                                rpOdrInf.CreateId,
                                                                rpOdrInf.CreateName,
                                                                rpOdrInf.UpdateDate,
                                                                rpOdrInf.IsDeleted
                                                             );

                                group.OdrInfs.Add(odrModel);
                            });
                            hokenGrp.GroupOdrItems.Add(group);
                        });

                        historyKarteOdrRaiin.HokenGroups.Add(hokenGrp);
                    });
                    historyKarteOdrRaiins.Add(historyKarteOdrRaiin);
                });

                var result = new GetMedicalExaminationHistoryOutputData(pageTotal, historyKarteOdrRaiins.OrderByDescending(x => x.SinDate).ToList(), GetMedicalExaminationHistoryStatus.Successed, startPageSearch);

                #endregion
                if (historyKarteOdrRaiins?.Count > 0)
                    return result;
                else
                    return new GetMedicalExaminationHistoryOutputData(0, new List<HistoryKarteOdrRaiinItem>(), GetMedicalExaminationHistoryStatus.NoData, 0);
            }
            catch
            {
                return new GetMedicalExaminationHistoryOutputData(0, new List<HistoryKarteOdrRaiinItem>(), GetMedicalExaminationHistoryStatus.Failed, 0);
            }
        }

        private int GetHokenPatternType(int hokenKbn)
        {
            switch (hokenKbn)
            {
                case 0:
                    return 3;
                case 1:
                case 2:
                    return 1;
                case 11:
                case 12:
                case 13:
                case 14:
                    return 2;
                default:
                    return 0;
            }
        }
    }
}
