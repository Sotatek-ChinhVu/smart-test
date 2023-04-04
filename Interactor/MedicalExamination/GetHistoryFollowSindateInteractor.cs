using Domain.Models.HistoryOrder;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Domain.Models.OrdInfs;
using UseCase.MedicalExamination.GetHistory;
using UseCase.MedicalExamination.GetHistoryFollowSindate;
using UseCase.OrdInfs.GetListTrees;

namespace Interactor.MedicalExamination
{
    public class GetHistoryFollowSindateInteractor : IGetHistoryFollowSindateInputPort
    {
        private readonly IHistoryOrderRepository _historyOrderRepository;
        private readonly IInsuranceRepository _insuranceRepository;

        public GetHistoryFollowSindateInteractor(IHistoryOrderRepository historyOrderRepository, IInsuranceRepository insuranceRepository)
        {
            _historyOrderRepository = historyOrderRepository;
            _insuranceRepository = insuranceRepository;
        }

        public GetHistoryFollowSindateOutputData Handle(GetHistoryFollowSindateInputData inputData)
        {
            try
            {
                var historyKarteOdrRaiins = new List<HistoryKarteOdrRaiinItem>();

                var historyList = _historyOrderRepository.GetListByRaiin(
                    inputData.HpId,
                    inputData.UserId,
                    inputData.PtId,
                    inputData.SinDate,
                    0,
                    inputData.DeleteConditon,
                    inputData.RaiinNo
                    );

                var insuranceModelList = _insuranceRepository.GetInsuranceList(inputData.HpId, inputData.PtId, inputData.SinDate, true);
                foreach (HistoryOrderModel history in historyList)
                {
                    var karteInfs = history.KarteInfModels;
                    var karteInfHistoryItems = karteInfs.Select(karteInf => new KarteInfHistoryItem(karteInf.HpId, karteInf.RaiinNo, karteInf.KarteKbn, karteInf.SeqNo, karteInf.PtId, karteInf.SinDate, karteInf.Text, karteInf.UpdateDate, karteInf.CreateDate, karteInf.IsDeleted, karteInf.RichText, karteInf.CreateName)).ToList();

                    var historyKarteOdrRaiin = new HistoryKarteOdrRaiinItem
                        (
                            history.RaiinNo,
                            history.SinDate,
                            history.HokenPid,
                            history.HokenTitle,
                            history.HokenRate,
                            history.SyosaisinKbn,
                            history.JikanKbn,
                            history.KaId,
                            history.KaName,
                            history.TantoId,
                            history.TantoName,
                            history.SanteiKbn,
                            history.TagNo,
                            history.SinryoTitle,
                            history.HokenType,
                            new List<HokenGroupHistoryItem>(),
                            new(),
                            new()
                        );

                    //Excute order
                    ExcuteOrder(insuranceModelList, history.OrderInfList, historyKarteOdrRaiin, historyKarteOdrRaiins);
                }
                return new GetHistoryFollowSindateOutputData(historyKarteOdrRaiins.OrderByDescending(x => x.SinDate).ToList(), GetHistoryFollowSindateStatus.Successed);
            }
            finally
            {
                _historyOrderRepository.ReleaseResource();
            }
        }

        private static void ExcuteOrder(List<InsuranceModel> insuranceData, List<OrdInfModel> orderInfList, HistoryKarteOdrRaiinItem historyKarteOdrRaiin, List<HistoryKarteOdrRaiinItem> historyKarteOdrRaiins)
        {
            var odrInfListByRaiinNo = orderInfList.OrderBy(odr => odr.OdrKouiKbn)
                                      .ThenBy(odr => odr.RpNo)
                                      .ThenBy(odr => odr.RpEdaNo)
                                      .ThenBy(odr => odr.SortNo)
                                      .ToList();

            // Find By Hoken
            List<int> hokenPidList = odrInfListByRaiinNo.GroupBy(odr => odr.HokenPid).Select(grp => grp.Key).ToList();

            foreach (var hokenPid in hokenPidList)
            {
                var hoken = insuranceData?.FirstOrDefault(c => c.HokenPid == hokenPid);
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
                                                            rpOdrInf.OrdInfDetails.OrderBy(o => o.RowNo).Select(od =>
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
                                                                    od.Kasan2,
                                                                    od.CnvUnitName,
                                                                    od.OdrUnitName,
                                                                    od.HasCmtName,
                                                                    od.CenterItemCd1,
                                                                    od.CenterItemCd2
                                                            )
                                                            ).ToList(),
                                                            rpOdrInf.CreateDate,
                                                            rpOdrInf.CreateId,
                                                            rpOdrInf.CreateName,
                                                            rpOdrInf.UpdateDate,
                                                            rpOdrInf.IsDeleted
                                                         );

                        group.OdrInfs.Add(odrModel);
                    }
                    hokenGrp.GroupOdrItems.Add(group);
                }
                historyKarteOdrRaiin.HokenGroups.Add(hokenGrp);
            }
            historyKarteOdrRaiins.Add(historyKarteOdrRaiin);
        }
    }
}