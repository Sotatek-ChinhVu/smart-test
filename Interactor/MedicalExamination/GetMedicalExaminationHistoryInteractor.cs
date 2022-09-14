using Domain.Models.Insurance;
using Domain.Models.KaMst;
using Domain.Models.KarteInfs;
using Domain.Models.KarteKbnMst;
using Domain.Models.OrdInfs;
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
        private readonly IKaMstRepository _kaRepository;
        public GetMedicalExaminationHistoryInteractor(IOrdInfRepository ordInfRepository, IKarteInfRepository karteInfRepository, IKarteKbnMstRepository karteKbnRepository, IReceptionRepository receptionRepository, IInsuranceRepository insuranceRepository, IUserRepository userRepository, IKaMstRepository kaRepository)
        {
            _ordInfRepository = ordInfRepository;
            _karteInfRepository = karteInfRepository;
            _karteKbnRepository = karteKbnRepository;
            _receptionRepository = receptionRepository;
            _insuranceRepository = insuranceRepository;
            _userRepository = userRepository;
            _kaRepository = kaRepository;
        }

        public GetMedicalExaminationHistoryOutputData Handle(GetMedicalExaminationHistoryInputData inputData)
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
            if (inputData.EndPage <= 0)
            {
                return new GetMedicalExaminationHistoryOutputData(0, new List<HistoryKarteOdrRaiinItem>(), GetMedicalExaminationHistoryStatus.InvalidEndPage, 0);
            }

            #region hard value
            int karteDeleteHistory = 1;
            bool allowDisplayDeleted = karteDeleteHistory > 0;
            #endregion

            var query = from raiinInf in _receptionRepository.GetList(inputData.HpId, inputData.PtId, karteDeleteHistory)
                        join ptHokenPattern in _insuranceRepository.GetListHokenPattern(inputData.HpId, inputData.PtId, allowDisplayDeleted)
                        on raiinInf.HokenPid equals ptHokenPattern.HokenPid
                        select raiinInf;

            var raiinNos = query?.Select(q => q.RaiinNo)?.ToList();
            var raiinNoStartPage = raiinNos == null ? 0 : raiinNos[inputData.StartPage];

            var historyKarteOdrRaiins = new List<HistoryKarteOdrRaiinItem>();

            #region karte
            List<KarteKbnMstModel> allkarteKbns = _karteKbnRepository.GetList(inputData.HpId, true);
            List<KarteInfModel> allkarteInfs = _karteInfRepository.GetList(inputData.PtId, inputData.HpId).OrderBy(c => c.KarteKbn).ToList();
            long SearchRaiinOnKarte()
            {
                return allkarteInfs.FirstOrDefault(k => k.RichText.Contains(inputData.SearchText) && ((inputData.SearchType == 1 && k.RaiinNo < raiinNoStartPage) || (inputData.SearchType == 2 && k.RaiinNo > raiinNoStartPage)))?.RaiinNo ?? 0;
            }
            #endregion
            #region Odr
            var allOdrInfs = raiinNos == null ? new List<OrdInfModel>() : _ordInfRepository
           .GetList(inputData.PtId, inputData.HpId, inputData.UserId, raiinNos)
                                                 .OrderBy(odr => odr.OdrKouiKbn)
                                                 .ThenBy(odr => odr.RpNo)
                                                 .ThenBy(odr => odr.RpEdaNo)
                                                 .ThenBy(odr => odr.SortNo)
                                                 .ToList();
            long SearchRaiinOnOdrInf()
            {
                return allOdrInfs?.FirstOrDefault(o => o.OrdInfDetails.Any(od => od.ItemName.Contains(inputData.SearchText)) && ((inputData.SearchType == 1 && o.RaiinNo < raiinNoStartPage) || (inputData.SearchType == 2 && o.RaiinNo > raiinNoStartPage)))?.RaiinNo ?? 0;
            }

            long raiinNoMark = 0;
            if (inputData.SearchCategory == 1)
            {
                raiinNoMark = SearchRaiinOnKarte();
            }
            else if (inputData.SearchCategory == 2)
            {
                raiinNoMark = SearchRaiinOnOdrInf();
            }
            else
            {
                if (inputData.SearchType == 1)
                    raiinNoMark = Math.Min(SearchRaiinOnKarte(), SearchRaiinOnOdrInf());
                else
                    raiinNoMark = Math.Max(SearchRaiinOnKarte(), SearchRaiinOnOdrInf());
            }

            var insuranceData = _insuranceRepository.GetInsuranceListById(inputData.HpId, inputData.PtId, inputData.SinDate);
            var hokenFirst = insuranceData?.ListInsurance.FirstOrDefault();

            var pageTotal = query?.Count() ?? 0;

            List<ReceptionModel>? rainInfs;
            var startDateSearch = 0;
            if (inputData.SearchType == 0)
            {
                rainInfs = query?.OrderByDescending(c => c.SinDate).Skip(inputData.EndPage + 1).Take(inputData.EndPage - inputData.StartPage).ToList();
            }
            else
            {
                rainInfs = query?.OrderByDescending(c => c.SinDate).ToList();

                var rainMarkObj = rainInfs?.FirstOrDefault(r => r.RaiinNo == raiinNoMark);
                var index = rainMarkObj == null ? 0 : rainInfs?.IndexOf(rainMarkObj) ?? 0;

                if (inputData.SearchType == 1)
                {
                    rainInfs = rainInfs?.Where(r => r.RaiinNo > raiinNoMark).Take(inputData.EndPage - inputData.StartPage).ToList();
                }
                else
                {
                    if (index <= 4)
                    {
                        rainInfs = rainInfs?.Where(r => r.RaiinNo < raiinNoMark).Take(inputData.EndPage - inputData.StartPage).ToList();
                        index = 0;
                    }
                    else if (index >= (inputData.EndPage - inputData.StartPage))
                    {
                        rainInfs = rainInfs?.Where(r => r.RaiinNo < raiinNoMark).Skip(index + 1 - (inputData.EndPage - inputData.StartPage)).Take(inputData.EndPage - inputData.StartPage).ToList();
                        index = index - (inputData.EndPage - inputData.StartPage);
                    }
                }
                startDateSearch = index;
            }

            if (!(rainInfs?.Count > 0))
                return new GetMedicalExaminationHistoryOutputData(0, new List<HistoryKarteOdrRaiinItem>(), GetMedicalExaminationHistoryStatus.NoData, 0);

            foreach (var raiinInf in rainInfs)
            {
                var doctorFirst = _userRepository.GetDoctorsList(raiinInf.TantoId).FirstOrDefault(c => c.UserId == raiinInf.TantoId);
                var kaMst = _kaRepository.GetByKaId(raiinInf.KaId);

                var historyKarteOdrRaiin = new HistoryKarteOdrRaiinItem(raiinInf.RaiinNo, raiinInf.SinDate, raiinInf.HokenPid, String.Empty, hokenFirst == null ? string.Empty : hokenFirst?.DisplayRateOnly ?? string.Empty, raiinInf.SyosaisinKbn, raiinInf.JikanKbn, raiinInf.KaId, kaMst == null ? String.Empty : kaMst.KaName, raiinInf.TantoId, doctorFirst == null ? String.Empty : doctorFirst.Sname, raiinInf.SanteiKbn, new List<HokenGroupHistoryItem>(), new List<GrpKarteHistoryItem>());


                List<KarteInfModel> karteInfByRaiinNo = allkarteInfs.Where(odr => odr.RaiinNo == historyKarteOdrRaiin.RaiinNo).OrderBy(c => c.KarteKbn).ThenBy(c => c.IsDeleted).ToList();

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
                                c.RichText
                                )
            ).ToList())
                                                             select karteGrp);

                var odrInfListByRaiinNo = allOdrInfs.Where(o => o.RaiinNo == raiinInf.RaiinNo)
                                                    .OrderBy(odr => odr.OdrKouiKbn)
                                                    .ThenBy(odr => odr.RpNo)
                                                    .ThenBy(odr => odr.RpEdaNo)
                                                    .ThenBy(odr => odr.SortNo)
                                                    .ToList();



                // Find By Hoken
                List<int> hokenPidList = odrInfListByRaiinNo.GroupBy(odr => odr.HokenPid).Select(grp => grp.Key).ToList();

                foreach (int hokenPid in hokenPidList)
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

                    foreach (var groupOdrInf in groupOdrInfList)
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

                        foreach (var rpOdrInf in rpOdrInfs.OrderBy(c => c.IsDeleted))
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
                                rpOdrInf.CreateName
                             );

                            group.OdrInfs.Add(odrModel);
                        }
                        hokenGrp.GroupOdrHistories.Add(group);
                    }

                    historyKarteOdrRaiin.HokenGroups.Add(hokenGrp);
                }
                historyKarteOdrRaiins.Add(historyKarteOdrRaiin);
            }

            var result = new GetMedicalExaminationHistoryOutputData(pageTotal, historyKarteOdrRaiins, GetMedicalExaminationHistoryStatus.Successed, startDateSearch);

            #endregion
            if (historyKarteOdrRaiins?.Count > 0)
                return result;
            else
                return new GetMedicalExaminationHistoryOutputData(0, new List<HistoryKarteOdrRaiinItem>(), GetMedicalExaminationHistoryStatus.NoData, 0);
        }
    }
}
