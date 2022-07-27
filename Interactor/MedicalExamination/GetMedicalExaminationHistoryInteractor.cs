using Domain.Models.KarteInfs;
using Domain.Models.KarteKbnMst;
using Domain.Models.OrdInfs;
using UseCase.MedicalExamination;
using UseCase.MedicalExamination.GetHistory;

namespace Interactor.MedicalExamination
{
    public class GetMedicalExaminationHistoryInteractor : IGetMedicalExaminationHistoryInputPort
    {
        private readonly IOrdInfRepository _ordInfRepository;
        private readonly IKarteInfRepository _karteInfRepository;
        private readonly IKarteKbnMstRepository _karteKbnRepository;
        public GetMedicalExaminationHistoryInteractor(IOrdInfRepository ordInfRepository, IKarteInfRepository karteInfRepository, IKarteKbnMstRepository karteKbnRepository)
        {
            _ordInfRepository = ordInfRepository;
            _karteInfRepository = karteInfRepository;
            _karteKbnRepository = karteKbnRepository;
        }

        public GetMedicalExaminationHistoryOutputData Handle(GetMedicalExaminationHistoryInputData inputData)
        {
            if (inputData.HpId <= 0)
            {
                return new GetMedicalExaminationHistoryOutputData(0, new List<HistoryKarteOdrRaiinItem>(), GetMedicalExaminationHistoryStatus.InvalidHpId);
            }
            if (inputData.PageIndex < 0)
            {
                return new GetMedicalExaminationHistoryOutputData(0, new List<HistoryKarteOdrRaiinItem>(), GetMedicalExaminationHistoryStatus.InvalidPageIndex);
            }
            if (inputData.PtId <= 0)
            {
                return new GetMedicalExaminationHistoryOutputData(0, new List<HistoryKarteOdrRaiinItem>(), GetMedicalExaminationHistoryStatus.InvalidPtId);
            }
            if (inputData.SinDate <= 0)
            {
                return new GetMedicalExaminationHistoryOutputData(0, new List<HistoryKarteOdrRaiinItem>(), GetMedicalExaminationHistoryStatus.InvalidSinDate);
            }
            if (inputData.PageSize <= 0)
            {
                return new GetMedicalExaminationHistoryOutputData(0, new List<HistoryKarteOdrRaiinItem>(), GetMedicalExaminationHistoryStatus.InvalidPageSize);
            }
            var historyKarteOdrRaiins = new List<HistoryKarteOdrRaiinItem>();

            #region karte
            List<KarteKbnMstModel> allkarteKbns = _karteKbnRepository.GetList(inputData.HpId, true);
            List<KarteInfModel> allkarteInfs = _karteInfRepository.GetList(inputData.PtId, inputData.HpId).OrderBy(c => c.KarteKbn).ToList();
            #endregion
            #region Odr
            List<OrdInfModel> allOdrInfs = _ordInfRepository
              .GetList(inputData.PtId, inputData.HpId)
                .ToList();

            //Hash code because no have KaMst, HokenPattern which join with user to return data
            var historyKarteOdrRaiin = new HistoryKarteOdrRaiinItem(901139649, 20220401, 30, "Hoken Title", "2022/04/01", 1, 1, 1, "Test", 0, "Tantoname", 1, new List<HokenGroupHistoryItem>(), new List<GrpKarteHistoryItem>());

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
                            c.IsDeleted)
        ).ToList())
                                                         select karteGrp);

            List<OrdInfModel> odrInfListByRaiinNo = allOdrInfs.Where(odr => odr.RaiinNo == historyKarteOdrRaiin.RaiinNo)
                                                .OrderBy(odr => odr.OdrKouiKbn)
                                                .ThenBy(odr => odr.RpNo)
                                                .ThenBy(odr => odr.RpEdaNo)
                                                .ThenBy(odr => odr.SortNo)
                                                .ToList();



            // Find By Hoken
            List<int> hokenPidList = odrInfListByRaiinNo.GroupBy(odr => odr.HokenPid).Select(grp => grp.Key).ToList();

            foreach (int hokenPid in hokenPidList)
            {
                var hokenGrp = new HokenGroupHistoryItem(hokenPid, String.Empty, new List<GroupOdrGHistoryItem>());

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
                                    od.CommentNewline
                            )
                            ).ToList()
                         );

                        group.OdrInfs.Add(odrModel);
                    }
                    hokenGrp.GroupOdrHistories.Add(group);
                }

                historyKarteOdrRaiin.HokenGroups.Add(hokenGrp);
            }
            historyKarteOdrRaiins.Add(historyKarteOdrRaiin);

            var result = new GetMedicalExaminationHistoryOutputData(1, historyKarteOdrRaiins, GetMedicalExaminationHistoryStatus.Successed);

            #endregion
            if (historyKarteOdrRaiins?.Count > 0)
                return result;
            else
                return new GetMedicalExaminationHistoryOutputData(0, new List<HistoryKarteOdrRaiinItem>(), GetMedicalExaminationHistoryStatus.NoData);
        }
    }
}
