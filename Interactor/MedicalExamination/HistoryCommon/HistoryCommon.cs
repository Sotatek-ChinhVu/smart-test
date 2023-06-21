using Domain.Models.HistoryOrder;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Domain.Models.OrdInfs;
using Domain.Models.PatientInfor;
using Domain.Models.User;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using System.Text;
using UseCase.MedicalExamination.GetDataPrintKarte2;
using UseCase.MedicalExamination.GetHistory;
using UseCase.OrdInfs.GetListTrees;

namespace Interactor.MedicalExamination.HistoryCommon;

public class HistoryCommon : IHistoryCommon
{
    private readonly IInsuranceRepository _insuranceRepository;
    private readonly IHistoryOrderRepository _historyOrderRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAmazonS3Service _amazonS3Service;
    private readonly AmazonS3Options _options;

    public HistoryCommon(IOptions<AmazonS3Options> optionsAccessor, IInsuranceRepository insuranceRepository, IHistoryOrderRepository historyOrderRepository, IAmazonS3Service amazonS3Service, IPatientInforRepository patientInforRepository, IUserRepository userRepository)
    {
        _insuranceRepository = insuranceRepository;
        _historyOrderRepository = historyOrderRepository;
        _amazonS3Service = amazonS3Service;
        _options = optionsAccessor.Value;
        _patientInforRepository = patientInforRepository;
        _userRepository = userRepository;
    }

    public GetMedicalExaminationHistoryOutputData GetHistoryOutput(int hpId, long ptId, int sinDate, (int totalCount, List<HistoryOrderModel> historyOrderModelList) historyList)
    {
        var historyKarteOdrRaiins = new List<HistoryKarteOdrRaiinItem>();
        var ptInf = _patientInforRepository.GetById(hpId, ptId, 0, 0);
        var listUserIds = historyList.historyOrderModelList?.Select(d => d.UketukeId).ToList();
        var listUsers = listUserIds == null ? new List<UserMstModel>() : _userRepository.GetListAnyUser(listUserIds)?.ToList();
        var insuranceModelList = _insuranceRepository.GetInsuranceList(hpId, ptId, sinDate, true);
        List<string> listFolders = new();
        listFolders.Add(CommonConstants.Store);
        listFolders.Add(CommonConstants.Karte);
        string path = _amazonS3Service.GetFolderUploadToPtNum(listFolders, ptInf != null ? ptInf.PtNum : 0);

        var host = new StringBuilder();
        host.Append(_options.BaseAccessUrl);
        host.Append("/");
        host.Append(path);
        foreach (HistoryOrderModel history in historyList.historyOrderModelList ?? new())
        {
            var karteInfs = history.KarteInfModels;
            var uketuke = listUsers?.FirstOrDefault(uke => uke.UserId == history.UketukeId);
            var karteInfHistoryItems = karteInfs.Select(karteInf => new KarteInfHistoryItem(karteInf.HpId, karteInf.RaiinNo, karteInf.KarteKbn, karteInf.SeqNo, karteInf.PtId, karteInf.SinDate, karteInf.Text, karteInf.UpdateDate, karteInf.CreateDate, karteInf.IsDeleted, karteInf.RichText, karteInf.CreateName)).ToList();
            List<GrpKarteHistoryItem> karteHistoryList = new List<GrpKarteHistoryItem> {
                        new GrpKarteHistoryItem(
                        karteInfHistoryItems.FirstOrDefault()?.KarteKbn ?? 0,
                        string.Empty,
                        string.Empty,
                        1,
                        0,
                        karteInfHistoryItems)
                    };

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
                    new(),
                    karteHistoryList,
                    history.ListKarteFile.Select(item => new FileInfOutputItem(item, host.ToString()))
                                         .OrderBy(item => item.SeqNo)
                                         .ToList(),
                    history.Status,
                    CIUtil.TimeToShowTime(ParseInt(CIUtil.Copy(history.UketukeTime, 1, 4))),
                    uketuke != null ? uketuke.Sname : string.Empty,
                    CIUtil.TimeToShowTime(ParseInt(CIUtil.Copy(history.SinStartTime, 1, 4))),
                    CIUtil.TimeToShowTime(ParseInt(CIUtil.Copy(history.SinEndTime, 1, 4)))
                );

            //Excute order
            ExcuteOrder(insuranceModelList, history.OrderInfList, historyKarteOdrRaiin, historyKarteOdrRaiins);
        }
        var result = new GetMedicalExaminationHistoryOutputData(historyList.totalCount, historyKarteOdrRaiins.OrderByDescending(x => x.SinDate).ToList(), GetMedicalExaminationHistoryStatus.Successed, 0);

        if (historyKarteOdrRaiins?.Count > 0)
            return result;
        else
            return new GetMedicalExaminationHistoryOutputData(0, new List<HistoryKarteOdrRaiinItem>(), GetMedicalExaminationHistoryStatus.NoData, 0);
    }

    public void FilterData(ref List<HistoryKarteOdrRaiinItem> historyKarteOdrRaiinItems, GetDataPrintKarte2InputData inputData)
    {
        List<OrderHokenType> GetListAcceptedHokenType()
        {
            List<OrderHokenType> result = new();
            if (inputData.IsCheckedHoken)
            {
                result.Add(OrderHokenType.Hoken);
            }
            if (inputData.IsCheckedJihi)
            {
                result.Add(OrderHokenType.Jihi);
            }
            if (inputData.IsCheckedHokenJihi)
            {
                result.Add(OrderHokenType.HokenJihi);
            }
            if (inputData.IsCheckedJihiRece)
            {
                result.Add(OrderHokenType.JihiRece);
            }
            if (inputData.IsCheckedHokenRousai)
            {
                result.Add(OrderHokenType.Rousai);
            }
            if (inputData.IsCheckedHokenJibai)
            {
                result.Add(OrderHokenType.Jibai);
            }
            return result;
        }

        if (!inputData.IsIncludeTempSave)
        {
            historyKarteOdrRaiinItems = historyKarteOdrRaiinItems.Where(k => k.Status != 3).ToList();
        }

        List<OrderHokenType> listAcceptedHokenType = GetListAcceptedHokenType();

        //Filter raiin as hoken setting
        List<HistoryKarteOdrRaiinItem> filteredKaruteList = new();
        foreach (var history in historyKarteOdrRaiinItems)
        {
            if (history.RaiinNo == 902550356 || history.RaiinNo == 902550392)
            {
                var temp = "abc";
            }
            if (listAcceptedHokenType.Contains((OrderHokenType)history.HokenType))
            {
                filteredKaruteList.Add(history);
                continue;
            }

            if (history.HokenGroups == null || !history.HokenGroups.Any())
            {
                continue;
            }

            if (inputData.DeletedOdrVisibilitySetting == 0)
            {
                foreach (var hokenGroup in history.HokenGroups)
                {
                    bool isDataExisted = false;
                    foreach (var group in hokenGroup.GroupOdrItems)
                    {
                        isDataExisted = group.OdrInfs.Any(o => o.IsDeleted == 0);
                        if (isDataExisted)
                        {
                            break;
                        }
                    }

                    if (isDataExisted && listAcceptedHokenType.Contains((OrderHokenType)history.HokenType))
                    {
                        filteredKaruteList.Add(history);
                        break;
                    }
                }
            }
            else if (inputData.DeletedOdrVisibilitySetting == 2)
            {
                foreach (var hokenGroup in history.HokenGroups)
                {
                    bool isDataExisted = false;
                    foreach (var group in hokenGroup.GroupOdrItems)
                    {
                        isDataExisted = group.OdrInfs.Any(o => o.IsDeleted != 2);
                        if (isDataExisted)
                        {
                            break;
                        }
                    }

                    if (isDataExisted && listAcceptedHokenType.Contains((OrderHokenType)history.HokenType))
                    {
                        filteredKaruteList.Add(history);
                        break;
                    }
                }
            }
        }

        historyKarteOdrRaiinItems = filteredKaruteList;

        //Filter karte and order empty
        historyKarteOdrRaiinItems = historyKarteOdrRaiinItems.Where(k => k.HokenGroups != null && k.HokenGroups.Any() && k.KarteHistories != null && k.KarteHistories.Any()).ToList();
    }

    public GetMedicalExaminationHistoryOutputData GetDataKarte2(GetDataPrintKarte2InputData inputData)
    {
        try
        {
            var patientInfo = _patientInforRepository.GetById(inputData.HpId, inputData.PtId, inputData.SinDate, 0);

            (int totalCount, List<HistoryOrderModel> historyOrderModelList) historyList = new();
            if (!inputData.EmptyMode)
            {
                historyList = _historyOrderRepository.GetList(inputData.HpId,
                                                              inputData.PtId,
                                                              inputData.SinDate,
                                                              inputData.StartDate,
                                                              inputData.EndDate,
                                                              1,
                                                              0,
                                                              1
                                                              );
            }

            var result = GetHistoryOutput(inputData.HpId, inputData.PtId, inputData.SinDate, historyList);
            List<HistoryKarteOdrRaiinItem> historyKarteOdrRaiinList = result.RaiinfList.OrderBy(r => r.SinDate).ThenBy(r => r.RaiinNo).ToList();
            FilterData(ref historyKarteOdrRaiinList, inputData);
            return new GetMedicalExaminationHistoryOutputData(result.Total, historyKarteOdrRaiinList, GetMedicalExaminationHistoryStatus.Successed, 0, inputData, patientInfo ?? new());
        }
        finally
        {
            ReleaseResources();
        }
    }

    private int ParseInt(string input)
    {
        try
        {
            return int.Parse(input);
        }
        catch
        {
            return 0;
        }
    }

    #region private function
    /// <summary>
    /// Excute Order
    /// </summary>
    /// <param name="insuranceData"></param>
    /// <param name="allOdrInfs"></param>
    /// <param name="historyKarteOdrRaiin"></param>
    /// <param name="historyKarteOdrRaiins"></param>
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
                                                                od.CmtOpt,
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
                                                                od.CenterItemCd2,
                                                                od.CmtColKeta1,
                                                                od.CmtColKeta2,
                                                                od.CmtColKeta3,
                                                                od.CmtColKeta4,
                                                                od.CmtCol1,
                                                                od.CmtCol2,
                                                                od.CmtCol3,
                                                                od.CmtCol4,
                                                                od.HandanGrpKbn,
                                                                od.IsKensaMstEmpty
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
    #endregion

    public void ReleaseResources()
    {
        _historyOrderRepository.ReleaseResource();
        _insuranceRepository.ReleaseResource();
        _patientInforRepository.ReleaseResource();
        _userRepository.ReleaseResource();
    }
}
