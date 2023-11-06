using Domain.Models.HistoryOrder;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Domain.Models.OrdInfs;
using Domain.Models.PatientInfor;
using Helper.Constants;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using System.Text;
using UseCase.MedicalExamination.GetHistory;
using UseCase.MedicalExamination.GetHistoryFollowSindate;
using UseCase.OrdInfs.GetListTrees;

namespace Interactor.MedicalExamination
{
    public class GetHistoryFollowSindateInteractor : IGetHistoryFollowSindateInputPort
    {
        private readonly IHistoryOrderRepository _historyOrderRepository;
        private readonly IInsuranceRepository _insuranceRepository;
        private readonly IAmazonS3Service _amazonS3Service;
        private readonly AmazonS3Options _options;
        private readonly IPatientInforRepository _patientInforRepository;

        public GetHistoryFollowSindateInteractor(IHistoryOrderRepository historyOrderRepository, IInsuranceRepository insuranceRepository, IOptions<AmazonS3Options> optionsAccessor, IAmazonS3Service amazonS3Service, IPatientInforRepository patientInforRepository)
        {
            _historyOrderRepository = historyOrderRepository;
            _insuranceRepository = insuranceRepository;
            _options = optionsAccessor.Value;
            _amazonS3Service = amazonS3Service;
            _patientInforRepository = patientInforRepository;
        }

        //flag == 0 : get for accounting
        //flag == 1 : get for one rp in todayorder
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
                    inputData.RaiinNo,
                    inputData.Flag,
                    new(),
                    inputData.IsShowApproval
                    );

                var insuranceModelList = _insuranceRepository.GetInsuranceList(inputData.HpId, inputData.PtId, inputData.SinDate, true);
                var ptInf = _patientInforRepository.GetById(inputData.HpId, inputData.PtId, 0, 0);
                List<string> listFolders = new();
                listFolders.Add(CommonConstants.Store);
                listFolders.Add(CommonConstants.Karte);
                string path = _amazonS3Service.GetFolderUploadToPtNum(listFolders, ptInf != null ? ptInf.PtNum : 0);
                var host = new StringBuilder();
                host.Append(_options.BaseAccessUrl);
                host.Append("/");
                host.Append(path);

                foreach (HistoryOrderModel history in historyList)
                {
                    var karteInfs = history.KarteInfModels;
                    var karteInfHistoryItems = karteInfs.Select(karteInf => new KarteInfHistoryItem(karteInf.HpId, karteInf.RaiinNo, karteInf.KarteKbn, karteInf.SeqNo, karteInf.PtId, karteInf.SinDate, karteInf.Text, karteInf.UpdateDate, karteInf.CreateDate, karteInf.IsDeleted, karteInf.RichText, karteInf.CreateName)).ToList();
                    var historyKarteOdrRaiin = !(inputData.Flag == 1) ? new HistoryKarteOdrRaiinItem
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
                            history.TantoFullName,
                            history.SanteiKbn,
                            history.TagNo,
                            history.SinryoTitle,
                            history.HokenType,
                            new List<HokenGroupHistoryItem>(),
                            new(),
                            new(),
                            history.Status,
                            string.Empty,
                            string.Empty,
                            string.Empty,
                            string.Empty
                        ) : new HistoryKarteOdrRaiinItem
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
                            history.TantoFullName,
                            history.SanteiKbn,
                            history.TagNo,
                            history.SinryoTitle,
                            history.HokenType,
                            new List<HokenGroupHistoryItem>(),
                            new List<GrpKarteHistoryItem> {
                                new GrpKarteHistoryItem(
                                karteInfHistoryItems.FirstOrDefault()?.KarteKbn ?? 0,
                                string.Empty,
                                string.Empty,
                                1,
                                0,
                                karteInfHistoryItems.OrderBy(k => k.SeqNo).ToList())
                            },
                            history.ListKarteFile.Select(item => new FileInfOutputItem(item, host.ToString()))
                                                 .OrderBy(item => item.SeqNo)
                                                 .ToList(),
                            history.Status,
                            string.Empty,
                            string.Empty,
                            string.Empty,
                            string.Empty,
                            history.HeaderOrderModels
                        );

                    //Excute order
                    ExcuteOrder(insuranceModelList, history.OrderInfList, historyKarteOdrRaiin, historyKarteOdrRaiins);
                }
                return new GetHistoryFollowSindateOutputData(historyKarteOdrRaiins.OrderByDescending(x => x.SinDate).ToList(), GetHistoryFollowSindateStatus.Successed);
            }
            finally
            {
                _historyOrderRepository.ReleaseResource();
                _historyOrderRepository.Dispose();
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
                                                                    od.DisplayItemName,
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
                                                            rpOdrInf.IsDeleted,
                                                            rpOdrInf.CreateMachine,
                                                            rpOdrInf.UpdateMachine,
                                                            rpOdrInf.UpdateName
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