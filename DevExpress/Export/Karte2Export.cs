using DevExpress.DataAccess.ObjectBinding;
using DevExpress.Inteface;
using DevExpress.Models.Karte2;
using DevExpress.Template.Karte2;
using DevExpress.XtraPrinting;
using Domain.Models.Insurance;
using Domain.Models.Ka;
using Domain.Models.KarteInfs;
using Domain.Models.KarteKbnMst;
using Domain.Models.OrdInfs;
using Domain.Models.PatientInfor;
using Domain.Models.RainListTag;
using Domain.Models.Reception;
using Domain.Models.User;
using Helper.Common;
using Interactor.ExportPDF.Karte2;
using System.Text;
using UseCase.MedicalExamination.GetHistory;
using UseCase.OrdInfs.GetListTrees;

namespace DevExpress.Export;

public class Karte2Export : IKarte2Export
{
    private readonly IOrdInfRepository _ordInfRepository;
    private readonly IKarteInfRepository _karteInfRepository;
    private readonly IKarteKbnMstRepository _karteKbnRepository;
    private readonly IReceptionRepository _receptionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IKaRepository _kaRepository;
    private readonly IRaiinListTagRepository _rainListTagRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IInsuranceRepository _insuranceRepository;

    public Karte2Export(IOrdInfRepository ordInfRepository, IKarteInfRepository karteInfRepository, IKarteKbnMstRepository karteKbnRepository, IReceptionRepository receptionRepository, IUserRepository userRepository, IKaRepository kaRepository, IRaiinListTagRepository rainListTagRepository, IPatientInforRepository patientInforRepository, IInsuranceRepository insuranceRepository)
    {
        _ordInfRepository = ordInfRepository;
        _karteInfRepository = karteInfRepository;
        _karteKbnRepository = karteKbnRepository;
        _receptionRepository = receptionRepository;
        _userRepository = userRepository;
        _kaRepository = kaRepository;
        _rainListTagRepository = rainListTagRepository;
        _patientInforRepository = patientInforRepository;
        _insuranceRepository = insuranceRepository;
    }

    public Karte2Output ExportToPdf(Karte2ExportInput inputData)
    {
        var patientInfo = _patientInforRepository.GetById(inputData.HpId, inputData.PtId, inputData.SinDate, 0);
        if (patientInfo == null || patientInfo.PtId == 0)
        {
            return new Karte2Output(Karte2Status.InvalidUser);
        }
        var historyKarteOdrRaiinItemWithStatus = GetHistoryKarteOdrRaiinItem(inputData);
        if (historyKarteOdrRaiinItemWithStatus.Item2 != Karte2Status.Success)
        {
            return new Karte2Output(historyKarteOdrRaiinItemWithStatus.Item2);
        }
        var historyKarteOdrRaiinItem = historyKarteOdrRaiinItemWithStatus.Item1;
        var karte2ExportModel = new Karte2ExportModel(
            hpId: patientInfo.HpId,
            ptId: patientInfo.PtId,
            sinDate: historyKarteOdrRaiinItemWithStatus.Item1.First().SinDate,
            raiinNo: historyKarteOdrRaiinItemWithStatus.Item1.First().RaiinNo,
            kanaName: patientInfo.KanaName,
            name: patientInfo.Name,
            sex: patientInfo.Sex == 1 ? "（男）" : "（女）",
            birthday: CIUtil.SDateToShowWDate2(patientInfo.Birthday),
            currentTime: DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm"),
            startDate: CIUtil.SDateToShowSDate2(inputData.StartDate),
            endDate: CIUtil.SDateToShowSDate2(inputData.EndDate),
            richTextKarte2Models: ConvertToRichTextKarteOrder(historyKarteOdrRaiinItem, inputData)
        );

        try
        {
            var res = ExportToPdf(karte2ExportModel);
            if (res.Length > 0)
            {
                return new Karte2Output(Karte2Status.Success, res);
            }
            return new Karte2Output(Karte2Status.CanNotExportPdf);
        }
        catch (Exception)
        {
            return new Karte2Output(Karte2Status.Failed);
        }

    }

    private List<RichTextKarteOrder> ConvertToRichTextKarteOrder(List<HistoryKarteOdrRaiinItem> historyKarteOdrRaiinItems, Karte2ExportInput inputData)
    {
        StringBuilder text_karte;
        StringBuilder text_order;

        List<RichTextKarteOrder> result = new();
        foreach (var karte_order in historyKarteOdrRaiinItems)
        {
            text_karte = new StringBuilder();
            text_order = new StringBuilder();
            // karte
            if (karte_order.KarteHistories.Any())
            {
                for (int i = 0; i < karte_order.KarteHistories.Count; i++)
                {
                    var karte = karte_order.KarteHistories[i];
                    if (karte.KarteData.Any())
                    {
                        var karteData = karte.KarteData.OrderBy(x => x.SinDate).ThenByDescending(x => x.IsDeleted).ToList();
                        for (int j = 0; j < karteData.Count; j++)
                        {
                            var data = karte.KarteData[j];
                            if (data.IsDeleted == 1)
                            {
                                if (inputData.IsCheckedInputDate)
                                {
                                    text_karte.Append("<span style=\"color: #3328fc \"><s> ");
                                    text_karte.Append(data.CreateDateDisplay);
                                    text_karte.Append("&nbsp;");
                                    text_karte.Append(data.CreateName);
                                    text_karte.Append("</s> ");
                                    text_karte.Append(data.UpdateDateDisplay);
                                    text_karte.Append("&nbsp;");
                                    text_karte.Append(data.CreateName);
                                    text_karte.Append("</span><br>");
                                }
                                text_karte.Append("<s>");
                                text_karte.Append(data.RichText);
                                text_karte.Append("</s><br>");
                            }
                            else if (data.IsDeleted == 0)
                            {
                                if (inputData.IsCheckedInputDate)
                                {
                                    text_karte.Append("<span style=\"color: #a51652 \"> ");
                                    text_karte.Append(data.CreateDateDisplay);
                                    text_karte.Append("&nbsp;");
                                    text_karte.Append(data.CreateName);
                                    text_karte.Append("</span><br>");
                                }
                                text_karte.Append(data.RichText);
                            }
                        }
                    }
                }
            }

            // order
            text_order.Append("<span style=\"color: #a51652\">&nbsp;");

            StringBuilder orderRaiinTitle = new();
            if (inputData.IsCheckedVisitingTime)
            {
                orderRaiinTitle.Append("受付:&nbsp;");
                orderRaiinTitle.Append(karte_order.UketukeTime);
                orderRaiinTitle.Append("&nbsp;");
            }
            if (inputData.IsUketsukeNameChecked)
            {
                orderRaiinTitle.Append(karte_order.UketsukeName);
            }
            if (inputData.IsCheckedStartTime)
            {
                orderRaiinTitle.Append("&nbsp;診察:&nbsp;");
                orderRaiinTitle.Append(karte_order.SinStartTime);
                if (inputData.IsCheckedEndTime)
                {
                    orderRaiinTitle.Append("&nbsp;-&nbsp;");
                    orderRaiinTitle.Append(karte_order.SinEndTime);
                }
            }
            if (inputData.IsCheckedDoctor)
            {
                orderRaiinTitle.Append("&nbsp;");
                orderRaiinTitle.Append(karte_order.TantoName);
            }
            if (inputData.IsCheckedApproved)
            {
                orderRaiinTitle.Append("&nbsp;（承認:&nbsp;");
                orderRaiinTitle.Append(karte_order.UpdateUserDisplay);
                orderRaiinTitle.Append("&nbsp;");
                orderRaiinTitle.Append(karte_order.UpdateDateDisplay);
            }
            if (orderRaiinTitle.Length == 0)
            {
                orderRaiinTitle.Append("＊");
            }
            text_order.Append(orderRaiinTitle);
            text_order.Append("）</span><br>");

            if (karte_order.HokenGroups.Any() && !inputData.IsCheckedHideOrder)
            {
                foreach (var group in karte_order.HokenGroups.Where(hoken_group => hoken_group.GroupOdrItems.Count > 0).SelectMany(hoken_group => hoken_group.GroupOdrItems))
                {
                    text_order.Append("<b><u> ");
                    text_order.Append(group.GroupName);
                    text_order.Append("</u></b><br>");
                    if (group.OdrInfs.Any())
                    {
                        for (int i = 0; i < group.OdrInfs.Count; i++)
                        {
                            var rp = group.OdrInfs[i];
                            if (rp.IsDeleted == 1)
                            {
                                text_order.Append("<span>&nbsp;＊");
                                if (inputData.IsCheckedInputDate)
                                {
                                    text_order.Append("<span style=\"color: #3328fc \"><s> ");
                                    text_order.Append(rp.CreateDateDisplay);
                                    text_order.Append("&nbsp;");
                                    text_order.Append(rp.CreateName);
                                    text_order.Append("</s>&nbsp;");
                                    text_order.Append(rp.UpdateDateDisplay);
                                    text_order.Append("&nbsp;");
                                    text_order.Append(rp.CreateName);
                                    text_order.Append("</span><br>");
                                }
                                text_order.Append("</span>");

                                if (rp.RpName != string.Empty && inputData.IsCheckedSetName)
                                {
                                    text_order.Append("<span><s>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;【");
                                    text_order.Append(rp.RpName);
                                    text_order.Append("】</s></span><br>");
                                }
                                if (rp.OdrDetails.Count > 0)
                                {
                                    foreach (var detail in rp.OdrDetails.Where(detail => detail.ItemName != string.Empty))
                                    {
                                        text_order.Append("<span><s>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
                                        text_order.Append(detail.ItemName);
                                        text_order.Append("</s></span><br>");
                                    }
                                }
                            }
                            else if (rp.IsDeleted == 0)
                            {
                                text_order.Append("<span>&nbsp;＊");
                                if (inputData.IsCheckedInputDate)
                                {
                                    text_order.Append("<span style=\"color: #a51652 \">&nbsp;");
                                    text_order.Append(rp.CreateDateDisplay);
                                    text_order.Append("&nbsp;");
                                    text_order.Append(rp.CreateName);
                                    text_order.Append("</span><br>");
                                }
                                text_order.Append("</span>");
                                if (rp.RpName != string.Empty)
                                {
                                    text_order.Append("<span>&nbsp;&nbsp;&nbsp;【");
                                    text_order.Append(rp.RpName);
                                    text_order.Append("】</span><br>");
                                }
                                if (rp.OdrDetails.Count > 0)
                                {
                                    foreach (var detail in rp.OdrDetails.Where(detail => detail.ItemName != string.Empty))
                                    {
                                        text_order.Append("<span>&nbsp;&nbsp;&nbsp;");

                                        StringBuilder itemInfo = new();
                                        bool isExistIpnName = !string.IsNullOrEmpty(detail.IpnName) && detail.SyohoKbn == 3 && group.IsDrug;
                                        if (isExistIpnName && !inputData.IsIppanNameChecked && group.InOutKbn != 0) //Jira: AIN-6552
                                        {
                                            itemInfo.Append("【般】");
                                        }
                                        itemInfo.Append(detail.ItemName);
                                        itemInfo.Append("&nbsp;");
                                        itemInfo.Append(detail.DisplayedQuantity);
                                        itemInfo.Append(detail.UnitName);
                                        if (isExistIpnName && inputData.IsIppanNameChecked)
                                        {
                                            text_order.Append("（");
                                            text_order.Append(itemInfo);
                                            text_order.Append("）");
                                        }
                                        else
                                        {
                                            text_order.Append(itemInfo);
                                        }
                                        text_order.Append("</span><br>");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            result.Add(new RichTextKarteOrder(
                    CIUtil.SDateToShowSDate(karte_order.SinDate),
                    text_karte.ToString(),
                    text_order.ToString()
                ));
        }
        return result;
    }
    private (List<HistoryKarteOdrRaiinItem>, Karte2Status) GetHistoryKarteOdrRaiinItem(Karte2ExportInput inputData)
    {
        var validate = Validate(inputData);
        if (validate != Karte2Status.Success)
        {
            return new(new List<HistoryKarteOdrRaiinItem>(), validate);
        }

        var query = from raiinInf in _receptionRepository.GetList(inputData.HpId, inputData.PtId, inputData.DeletedOdrVisibilitySetting)
                    join ptHokenPattern in _insuranceRepository.GetListHokenPattern(inputData.HpId, inputData.PtId, true)
                    on raiinInf.HokenPid equals ptHokenPattern.HokenPid
                    where inputData.StartDate <= raiinInf.SinDate &&
                    raiinInf.SinDate <= inputData.EndDate
                    select raiinInf;

        var allRaiinInf = query?.ToList();
        var raiinNos = allRaiinInf?.Select(q => q.RaiinNo)?.ToList();
        var tantoIds = allRaiinInf?.Select(r => r.TantoId).ToList();
        var kaIds = allRaiinInf?.Select(r => r.TantoId).ToList();
        var sinDates = allRaiinInf?.Select(r => r.SinDate).ToList();
        var listUserIds = allRaiinInf?.Select(d => d.UketukeId).ToList();
        var listCreateIds = allRaiinInf?.Select(d => d.CreateId).ToList();
        if (listCreateIds != null)
        {
            listUserIds?.AddRange(listCreateIds);
        }

        var historyKarteOdrRaiins = new List<HistoryKarteOdrRaiinItem>();

        #region karte
        var allkarteKbns = _karteKbnRepository.GetList(inputData.HpId, true);
        var allkarteInfs = raiinNos == null ? new List<KarteInfModel>() : _karteInfRepository.GetList(inputData.PtId, inputData.HpId, inputData.DeletedOdrVisibilitySetting, raiinNos).OrderBy(c => c.KarteKbn).ToList();
        #endregion

        #region Odr
        var allOdrInfs = raiinNos == null ? new List<OrdInfModel>() : _ordInfRepository
       .GetList(inputData.PtId, inputData.HpId, inputData.UserId, inputData.DeletedOdrVisibilitySetting, raiinNos)
                                              .Where(x => inputData.IsCheckedSyosai || x.OdrKouiKbn != 10)
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
        var listUsers = listUserIds == null ? new List<UserMstModel>() : _userRepository.GetListAnyUser(listUserIds)?.ToList();
        IEnumerable<ApproveInfModel>? approveInfs = null;

        if (allRaiinInf == null || !allRaiinInf.Any())
        {
            return new(new List<HistoryKarteOdrRaiinItem>(), Karte2Status.NoData);
        }

        var obj = new object();
        Parallel.ForEach(allRaiinInf, raiinInf =>
        {
            lock (obj)
            {
                //Infor relation
                var doctorFirst = doctors?.FirstOrDefault(d => d.UserId == raiinInf.TantoId);
                var kaMst = kaMsts?.FirstOrDefault(k => k.KaId == raiinInf.KaId);
                var raiinTag = raiinListTags?.FirstOrDefault(r => r.RaiinNo == raiinInf.RaiinNo && r.SinDate == raiinInf.SinDate);
                var approveInf = approveInfs?.FirstOrDefault(a => a.RaiinNo == raiinInf.RaiinNo);

                //Composite karte and order
                var uketuke = listUsers?.FirstOrDefault(uke => uke.UserId == raiinInf.UketukeId);
                var updateUser = listUsers?.FirstOrDefault(uke => uke.UserId == raiinInf.UpdateId);
                var historyKarteOdrRaiin = new HistoryKarteOdrRaiinItem(
                                                    raiinInf.RaiinNo,
                                                    raiinInf.SinDate,
                                                    raiinInf.HokenPid,
                                                    hokenFirst == null ? string.Empty : hokenFirst.HokenName,
                                                    hokenFirst == null ? string.Empty : hokenFirst.DisplayRateOnly,
                                                    raiinInf.SyosaisinKbn,
                                                    raiinInf.JikanKbn, raiinInf.KaId,
                                                    kaMst == null ? String.Empty : kaMst.KaName,
                                                    raiinInf.TantoId,
                                                    doctorFirst == null ? String.Empty : doctorFirst.Sname,
                                                    raiinInf.SanteiKbn,
                                                    raiinTag?.TagNo ?? 0,
                                                    approveInf?.DisplayApprovalInfo ?? string.Empty,
                                                    GetHokenPatternType(hokenFirst?.HokenKbn ?? 0),
                                                    raiinInf.Status,
                                                    new List<HokenGroupHistoryItem>(),
                                                    new List<GrpKarteHistoryItem>(),
                                                    CIUtil.TimeToShowTime(int.Parse(CIUtil.Copy(raiinInf.UketukeTime, 1, 4))),
                                                    uketuke != null ? uketuke.Sname : string.Empty,
                                                    CIUtil.TimeToShowTime(int.Parse(CIUtil.Copy(raiinInf.SinStartTime, 1, 4))),
                                                    CIUtil.TimeToShowTime(int.Parse(CIUtil.Copy(raiinInf.SinEndTime, 1, 4))),
                                                    raiinInf.UpdateDate.ToString("yyyy/MM/dd HH:mm"),
                                                    updateUser != null ? updateUser.Sname : string.Empty
                                               );

                //Excute karte
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
                                )).ToList())
                                                             select karteGrp);
                //Excute order
                ExcuteOrder(insuranceData, allOdrInfs, historyKarteOdrRaiin, historyKarteOdrRaiins);
            }
        });

        var result = historyKarteOdrRaiins.OrderBy(x => x.SinDate).ToList();
        FilterData(ref result, inputData);

        #endregion
        if (result.Any())
        {
            return (result, Karte2Status.Success);
        }
        return new(new List<HistoryKarteOdrRaiinItem>(), Karte2Status.NoData);
    }

    private void FilterData(ref List<HistoryKarteOdrRaiinItem> historyKarteOdrRaiinItems, Karte2ExportInput inputData)
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
            if (history.HokenGroups == null || !history.HokenGroups.Any())
            {
                continue;
            }

            if (listAcceptedHokenType.Contains((OrderHokenType)history.HokenType))
            {
                filteredKaruteList.Add(history);
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

        //historyKarteOdrRaiinItems.ForEach((karute) =>
        //{
        //    //Filter order as hoken setting
        //    if (karute.HokenGroups != null && karute.HokenGroups.Any())
        //    {
        //        var listHoken = karute.HokenGroups.Where(h => listAcceptedHokenType.Contains((OrderHokenType)karute.HokenType)).ToList();
        //        listHoken.ForEach((hoken) =>
        //        {
        //            if (!inputData.IsCheckedJihi)
        //            {
        //                hoken = new HokenGroupHistoryItem(hoken.HokenPid, hoken.HokenTitle, hoken.GroupOdrItems.Where(o => o.SanteiKbn == 0).ToList());
        //            }
        //            foreach (var group in hoken.GroupOdrItems.ToList())
        //            {
        //                if (!inputData.IsCheckedJihi && group != null && group.OdrInfs.Any())
        //                {
        //                    if (inputData.DeletedOdrVisibilitySetting == 0)
        //                    {
        //                        group = new GroupOdrGHistoryItem(group.HokenPid, group.SinkyuName,
        //                            group.OdrInfs.Where(o => o.IsDeleted == 0)
        //                            .OrderBy(o => o.SortNo)
        //                            .ToList());
        //                    }
        //                    else if (inputData.DeletedOdrVisibilitySetting == 2)
        //                    {
        //                        group = new GroupOdrGHistoryItem(group.HokenPid, group.SinkyuName,
        //                            group.OdrInfs.Where(o => o.IsDeleted != 2)
        //                            .OrderBy(o => o.SortNo)
        //                            .ToList());
        //                    }
        //                }
        //            }
        //            hoken.GroupOdrItems.ToList().ForEach((group) =>
        //            {
        //                if (!inputData.IsCheckedJihi && group != null && group.OdrInfs.Any())
        //                {
        //                    if (inputData.DeletedOdrVisibilitySetting == 0)
        //                    {
        //                        group = new GroupOdrGHistoryItem(group.HokenPid, group.SinkyuName, 
        //                            group.OdrInfs.Where(o => o.IsDeleted == 0)
        //                            .OrderBy(o => o.SortNo)
        //                            .ToList());
        //                    }
        //                    else if (inputData.DeletedOdrVisibilitySetting == 2)
        //                    {
        //                        group = new GroupOdrGHistoryItem(group.HokenPid, group.SinkyuName, 
        //                            group.OdrInfs?.Where(o => o.IsDeleted != 2)
        //                            .OrderBy(o => o.SortNo)
        //                            .ToList());
        //                    }
        //                }
        //            });
        //        });

        //        karute = new HistoryKarteOdrRaiinItem(
        //                karute.RaiinNo,
        //                karute.SinDate,
        //                karute.HokenPid,
        //                karute.HokenTitle,
        //                karute.HokenRate,
        //                karute.SyosaisinKbn,
        //                karute.JikanKbn,
        //                karute.KaId,
        //                karute.KaName,
        //                karute.TantoId,
        //                karute.TantoName,
        //                karute.SanteiKbn,
        //                karute.TagNo,
        //                karute.SinryoTitle,
        //                karute.HokenType,
        //                karute.Status,
        //                listHoken,
        //                karute.KarteHistories,
        //                karute.UketukeTime,
        //                karute.UketsukeName,
        //                karute.SinStartTime,
        //                karute.SinEndTime,
        //                karute.UpdateDateDisplay,
        //                karute.UpdateUserDisplay
        //            );
        //    }
        //});

        //Filter karte and order empty
        historyKarteOdrRaiinItems = historyKarteOdrRaiinItems.Where(k => k.HokenGroups != null && k.HokenGroups.Any() && k.KarteHistories != null && k.KarteHistories.Any()).ToList();
    }
    private static int GetHokenPatternType(int hokenKbn)
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

    private static Karte2Status Validate(Karte2ExportInput inputData)
    {
        if (inputData.HpId <= 0)
        {
            return Karte2Status.InvalidHpId;
        }

        if (inputData.PtId <= 0)
        {
            return Karte2Status.InvalidPtId;
        }

        if (inputData.SinDate <= 0)
        {
            return Karte2Status.InvalidSinDate;
        }

        if (inputData.StartDate <= 0)
        {
            return Karte2Status.InvalidStartDate;
        }
        if (inputData.EndDate <= 0)
        {
            return Karte2Status.InvalidEndDate;
        }

        if (!(inputData.DeletedOdrVisibilitySetting >= 0 && inputData.DeletedOdrVisibilitySetting <= 2))
        {
            return Karte2Status.InvalidDeleteCondition;
        }

        if (inputData.UserId <= 0)
        {
            return Karte2Status.InvalidUser;
        }

        return Karte2Status.Success;
    }

    private void ExcuteOrder(InsuranceDataModel? insuranceData, List<OrdInfModel> allOdrInfs, HistoryKarteOdrRaiinItem historyKarteOdrRaiin, List<HistoryKarteOdrRaiinItem> historyKarteOdrRaiins)
    {
        var odrInfListByRaiinNo = allOdrInfs.Where(o => o.RaiinNo == historyKarteOdrRaiin.RaiinNo)
                                            .Select(o => o.ChangeOdrDetail(o.OrdInfDetails.Where(od => od != null && od.RaiinNo == historyKarteOdrRaiin.RaiinNo)
                                            .ToList()));
        odrInfListByRaiinNo = odrInfListByRaiinNo.OrderBy(odr => odr.OdrKouiKbn)
                                  .ThenBy(odr => odr.RpNo)
                                  .ThenBy(odr => odr.RpEdaNo)
                                  .ThenBy(odr => odr.SortNo)
                                  .ToList();

        // Find By Hoken
        List<int> hokenPidList = odrInfListByRaiinNo.GroupBy(odr => odr.HokenPid).Select(grp => grp.Key).ToList();

        var obj = new object();
        Parallel.ForEach(hokenPidList, hokenPid =>
        {
            lock (obj)
            {
                var hoken = insuranceData?.ListInsurance.FirstOrDefault(c => c.HokenPid == hokenPid);
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

                var objgroup = new object();
                Parallel.ForEach(groupOdrInfList, groupOdrInf =>
                {
                    lock (objgroup)
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

                        var objOdrInf = new object();
                        Parallel.ForEach(rpOdrInfs.OrderBy(c => c.IsDeleted), rpOdrInf =>
                        {
                            lock (objOdrInf)
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
                            }
                        });
                        hokenGrp.GroupOdrItems.Add(group);
                    }
                });
                historyKarteOdrRaiin.HokenGroups.Add(hokenGrp);
            }
        });
        historyKarteOdrRaiins.Add(historyKarteOdrRaiin);
    }

    private MemoryStream ExportToPdf(Karte2ExportModel data)
    {
        try
        {
            var report = new Karte2Template();
            var dataSource = new ObjectDataSource();
            dataSource.DataSource = data;

            report.DataSource = dataSource;
            report.DataMember = "RichTextKarte2Models";

            PdfExportOptions pdfExportOptions = new PdfExportOptions()
            {
                PdfACompatibility = PdfACompatibility.PdfA1b
            };

            // Export the report.
            MemoryStream stream = new MemoryStream();
            report.ExportToPdf(stream, pdfExportOptions);
            return stream;
        }
        catch (Exception)
        {
            return new MemoryStream();
        }
    }
}
