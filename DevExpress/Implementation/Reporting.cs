using DevExpress.Export;
using DevExpress.Models.Karte1;
using DevExpress.Models.Karte2;
using DevExpress.Response.Karte1;
using Domain.Constant;
using Domain.Models.Diseases;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Domain.Models.Ka;
using Domain.Models.KarteInfs;
using Domain.Models.KarteKbnMst;
using Domain.Models.OrdInfs;
using Domain.Models.PatientInfor;
using Domain.Models.RainListTag;
using Domain.Models.Reception;
using Domain.Models.User;
using Helper.Common;
using Interactor.ExportPDF;
using Interactor.ExportPDF.Karte1;
using Interactor.ExportPDF.Karte2;
using System.Text;
using UseCase.MedicalExamination.GetHistory;
using UseCase.OrdInfs.GetListTrees;

namespace DevExpress.Implementation;

public class Reporting : IReporting
{
    private readonly IPtDiseaseRepository _diseaseRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IInsuranceRepository _insuranceRepository;
    private readonly IOrdInfRepository _ordInfRepository;
    private readonly IKarteInfRepository _karteInfRepository;
    private readonly IKarteKbnMstRepository _karteKbnRepository;
    private readonly IReceptionRepository _receptionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IKaRepository _kaRepository;
    private readonly IRaiinListTagRepository _rainListTagRepository;
    private readonly Karte1Export _karte1Export;
    private readonly Karte2Export _karte2Export;

    public Reporting(IPtDiseaseRepository diseaseRepository, IPatientInforRepository patientInforRepository, IInsuranceRepository insuranceRepository, IOrdInfRepository ordInfRepository, IKarteInfRepository karteInfRepository, IKarteKbnMstRepository karteKbnRepository, IReceptionRepository receptionRepository, IUserRepository userRepository, IKaRepository kaRepository, IRaiinListTagRepository rainListTagRepository, Karte1Export karte1Export, Karte2Export karte2Export)
    {
        _diseaseRepository = diseaseRepository;
        _patientInforRepository = patientInforRepository;
        _insuranceRepository = insuranceRepository;
        _ordInfRepository = ordInfRepository;
        _karteInfRepository = karteInfRepository;
        _karteKbnRepository = karteKbnRepository;
        _receptionRepository = receptionRepository;
        _userRepository = userRepository;
        _kaRepository = kaRepository;
        _rainListTagRepository = rainListTagRepository;
        _karte1Export = karte1Export;
        _karte2Export = karte2Export;
    }

    #region Print Karte 1
    public Karte1Output PrintKarte1(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei)
    {
        if (hpId <= 0)
        {
            return new Karte1Output(Karte1Status.InvalidHpId);
        }
        else if (sinDate <= 10000000 || sinDate >= 99999999)
        {
            return new Karte1Output(Karte1Status.InvalidSindate);
        }

        var ptInf = _patientInforRepository.GetById(hpId, ptId, sinDate, 0);
        if (ptInf == null)
        {
            return new Karte1Output(Karte1Status.PtInfNotFould);
        }
        var hoken = _insuranceRepository.GetPtHokenInf(hpId, hokenPid, ptId, sinDate);
        if (hoken == null)
        {
            return new Karte1Output(Karte1Status.HokenNotFould);
        }
        var ptByomeis = _diseaseRepository.GetListPatientDiseaseForReport(hpId, ptId, hokenPid, sinDate, tenkiByomei);

        var listByomeiModelsPage1 = ConvertToListKarte1ByomeiModel(ptByomeis).Item1;
        var listByomeiModelsPage2 = ConvertToListKarte1ByomeiModel(ptByomeis).Item2;

        var dataModel = ConvertToKarte1ExportModel(ptInf, hoken, listByomeiModelsPage1, listByomeiModelsPage2);
        try
        {
            var res = _karte1Export.ExportToPdf(dataModel);
            if (res.Length > 0)
            {
                return new Karte1Output(Karte1Status.Success, res);
            }
            return new Karte1Output(Karte1Status.CanNotExportPdf);
        }
        catch (Exception)
        {
            return new Karte1Output(Karte1Status.Failed);
        }
    }

    private Tuple<List<Karte1ByomeiModel>, List<Karte1ByomeiModel>> ConvertToListKarte1ByomeiModel(List<PtDiseaseModel> ptByomeis)
    {
        List<Karte1ByomeiModel> listByomeiModelsPage1 = new();
        List<Karte1ByomeiModel> listByomeiModelsPage2 = new();
        int index = 1;
        if (ptByomeis != null && ptByomeis.Count > 0)
        {
            foreach (var byomei in ptByomeis)
            {
                string byomeiDisplay = byomei.Byomei;
                if (byomei.SyubyoKbn == 1)
                {
                    byomeiDisplay = "（主）" + byomeiDisplay;
                }
                if (byomeiDisplay.Length >= 26 && index <= 12)
                {
                    byomeiDisplay = byomeiDisplay.Substring(0, 26);
                }
                var byomeiStartDateWFormat = CIUtil.SDateToShowWDate3(byomei.StartDate).Ymd;
                var byomeiTenkiDateWFormat = CIUtil.SDateToShowWDate3(byomei.TenkiDate).Ymd;
                var tenkiChusiMaru = byomei.TenkiKbn == TenkiKbnConst.Canceled;
                var tenkiSiboMaru = byomei.TenkiKbn == TenkiKbnConst.Dead;
                var tenkiSonota = byomei.TenkiKbn == TenkiKbnConst.Other;
                var tenkiTiyuMaru = byomei.TenkiKbn == TenkiKbnConst.Cured;
                var byomeiModel = new Karte1ByomeiModel(
                                            byomeiDisplay,
                                            byomeiStartDateWFormat != null ? byomeiStartDateWFormat : string.Empty,
                                            byomeiTenkiDateWFormat != null ? byomeiTenkiDateWFormat : string.Empty,
                                            tenkiChusiMaru,
                                            tenkiSiboMaru,
                                            tenkiSonota,
                                            tenkiTiyuMaru
                                        );
                if (index <= 12)
                {
                    listByomeiModelsPage1.Add(byomeiModel);
                }
                else
                {
                    listByomeiModelsPage2.Add(byomeiModel);
                }
                index += 1;
            }
        }
        return Tuple.Create(listByomeiModelsPage1, listByomeiModelsPage2);
    }

    private Karte1ExportModel ConvertToKarte1ExportModel(PatientInforModel ptInf, InsuranceModel hoken, List<Karte1ByomeiModel> listByomeiModelsPage1, List<Karte1ByomeiModel> listByomeiModelsPage2)
    {
        var printoutDateTime = DateTime.UtcNow;
        var ptNum = string.Empty;
        var hokensyaNo = string.Empty;
        var kigoBango = string.Empty;
        var ptKanaName = string.Empty;
        var ptName = string.Empty;
        var hokenKigenW = string.Empty;
        var setainusi = string.Empty;
        var birthDateW = string.Empty;
        var age = string.Empty;
        var sex = "男";
        var hokenSyutokuW = string.Empty;
        var ptPostCode = string.Empty;
        var ptAddress1 = string.Empty;
        var ptAddress2 = string.Empty;
        var officeAddress = string.Empty;
        var officeTel = string.Empty;
        var ptTel = string.Empty;
        var office = string.Empty;
        var ptRenrakuTel = string.Empty;
        var job = string.Empty;
        var hokensyaAddress = string.Empty;
        var hokensyaName = string.Empty;
        var zokugara = string.Empty;
        var hokensyaTel = string.Empty;
        var futansyaNo_K1 = string.Empty;
        var jyukyusyaNo_K1 = string.Empty;
        var futansyaNo_K2 = string.Empty;
        var jyukyusyaNo_K2 = string.Empty;

        var sysDateTimeS = printoutDateTime.ToString("yyyy/MM/dd HH:mm");
        if (ptInf != null)
        {
            ptNum = ptInf.PtNum != 0 ? ptInf.PtNum.ToString() : string.Empty;
            ptKanaName = ptInf.KanaName != null ? ptInf.KanaName.ToString() : string.Empty;
            ptName = ptInf.Name != null ? ptInf.Name.ToString() : string.Empty;
            setainusi = ptInf.Setanusi != null ? ptInf.Setanusi.ToString() : string.Empty;
            var warekiBirthDay = CIUtil.SDateToShowWDate3(ptInf.Birthday);
            birthDateW = warekiBirthDay.Ymd != null ? warekiBirthDay.Ymd.ToString() : string.Empty;
            var ageNum = CIUtil.SDateToAge(ptInf.Birthday, CIUtil.DateTimeToInt(DateTime.Now));
            age = ageNum != 0 ? ageNum.ToString() : "0";
            sex = ptInf.Sex == 2 ? "女" : "男";
            ptPostCode = ptInf.HomePost != null ? ptInf.HomePost.ToString() : string.Empty;
            ptAddress1 = ptInf.HomeAddress1 != null ? ptInf.HomeAddress1.ToString() : string.Empty;
            ptAddress2 = ptInf.HomeAddress2 != null ? ptInf.HomeAddress2.ToString() : string.Empty;
            officeAddress = ptInf.OfficeAddress1 != null ? ptInf.OfficeAddress1.ToString() : string.Empty;
            officeTel = ptInf.OfficeTel != null ? ptInf.OfficeTel.ToString() : string.Empty;
            if (ptInf.Tel1 != "")
            {
                ptTel = ptInf.Tel1 != null ? ptInf.Tel1 : string.Empty;
            }
            else if (ptInf.Tel2 != "")
            {
                ptTel = ptInf.Tel2 != null ? ptInf.Tel2 : string.Empty;
            }
            else if (ptInf.RenrakuTel != "")
            {
                ptTel = ptInf.RenrakuTel != null ? ptInf.RenrakuTel : string.Empty;
            }
            office = ptInf.OfficeName != null ? ptInf.OfficeName : string.Empty;
            ptRenrakuTel = ptInf.RenrakuTel != null ? ptInf.RenrakuTel : string.Empty;
            job = ptInf.Job != null ? ptInf.Job : string.Empty;
        }

        if (hoken != null)
        {
            hokensyaNo = hoken.HokensyaNo != string.Empty ? hoken.HokensyaNo.PadLeft(8, ' ') : string.Empty;
            if (new int[] { 11, 12, 13 }.Contains(hoken.HokenKbn))
            {
                // 労災
                kigoBango = hoken.RousaiKofuNo ?? string.Empty;
            }
            else if (hoken.HokenKbn == 14)
            {
                // 自賠
                kigoBango = hoken.JibaiHokenName ?? string.Empty;
            }
            else
            {
                kigoBango = hoken.Kigo + "・" + hoken.Bango;
                if (!string.IsNullOrEmpty(hoken.EdaNo))
                {
                    kigoBango = kigoBango + "(" + hoken.EdaNo ?? string.Empty + ")";
                }
            }
            var warekiEndate = CIUtil.SDateToShowWDate3(hoken.EndDate);
            hokenKigenW = warekiEndate.Ymd != null ? warekiEndate.Ymd : string.Empty;
            var warekiSyutokuDate = CIUtil.SDateToShowWDate3(hoken.SikakuDate);
            hokenSyutokuW = warekiSyutokuDate.Ymd != null ? warekiSyutokuDate.Ymd : string.Empty;
            hokensyaName = hoken.HokensyaName;
            hokensyaAddress = hoken.HokensyaAddress;
            zokugara = hoken.KeizokuKbn.ToString();
            hokensyaTel = hoken.HokensyaTel;
            futansyaNo_K1 = hoken.Kohi1.FutansyaNo;
            jyukyusyaNo_K1 = hoken.Kohi1.JyukyusyaNo;
            futansyaNo_K2 = hoken.Kohi2.FutansyaNo;
            jyukyusyaNo_K2 = hoken.Kohi2.JyukyusyaNo;
        }
        return new Karte1ExportModel(
                sysDateTimeS,
                ptNum,
                futansyaNo_K1,
                hokensyaNo,
                jyukyusyaNo_K1,
                kigoBango,
                ptKanaName,
                ptName,
                hokenKigenW,
                setainusi,
                birthDateW,
                age,
                sex,
                hokenSyutokuW,
                ptPostCode,
                ptAddress1,
                ptAddress2,
                officeAddress,
                officeTel,
                ptTel,
                office,
                ptRenrakuTel,
                hokensyaAddress,
                zokugara,
                hokensyaTel,
                job,
                hokensyaName,
                futansyaNo_K2,
                jyukyusyaNo_K2,
                listByomeiModelsPage1,
                listByomeiModelsPage2
            );
    }
    #endregion

    #region Print Karte 2
    public Karte2Output PrintKarte2(Karte2ExportInput inputData)
    {
        var patientInfo = _patientInforRepository.GetById(inputData.HpId, inputData.PtId, inputData.SinDate, (int)inputData.RaiinNo);
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
            sex: patientInfo.Sex == 1 ? "男" : "女",
            birthday: CIUtil.SDateToShowWDate2(patientInfo.Birthday),
            currentTime: DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm"),
            startDate: CIUtil.SDateToShowSDate(inputData.StartDate),
            endDate: CIUtil.SDateToShowSDate(inputData.EndDate),
            richTextKarte2Models: ConvertToRichTextKarteOrder(historyKarteOdrRaiinItem)
        );

        try
        {
            var res = _karte2Export.ExportToPdf(karte2ExportModel);
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

    private List<RichTextKarteOrder> ConvertToRichTextKarteOrder(List<HistoryKarteOdrRaiinItem> historyKarteOdrRaiinItems)
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
                                text_karte.Append("<span style=\"color: #3328fc \"><s> ");
                                text_karte.Append(data.CreateDateDisplay);
                                text_karte.Append("&nbsp;");
                                text_karte.Append(data.CreateName);
                                text_karte.Append("</s> ");
                                text_karte.Append(data.UpdateDateDisplay);
                                text_karte.Append("&nbsp;");
                                text_karte.Append(data.CreateName);
                                text_karte.Append("</span><br>");
                                text_karte.Append("<s>");
                                text_karte.Append(data.RichText);
                                text_karte.Append("</s><br>");
                            }
                            else if (data.IsDeleted == 0)
                            {
                                text_karte.Append("<span style=\"color: #a51652 \"> ");
                                text_karte.Append(data.CreateDateDisplay);
                                text_karte.Append("&nbsp;");
                                text_karte.Append(data.CreateName);
                                text_karte.Append("</span><br>");
                                text_karte.Append(data.RichText);
                            }
                        }
                    }
                }
            }

            // order
            text_order.Append("<span style=\"color: #a51652\">&nbsp;");
            text_order.Append("受付:&nbsp;");
            text_order.Append(karte_order.UketukeTime);
            text_order.Append("&nbsp;");
            text_order.Append(karte_order.UketsukeName);
            text_order.Append("&nbsp;診察:&nbsp;");
            text_order.Append(karte_order.SinStartTime);
            text_order.Append("&nbsp;-&nbsp;");
            text_order.Append(karte_order.SinEndTime);
            text_order.Append("&nbsp;");
            text_order.Append(karte_order.TantoName);
            text_order.Append("&nbsp;(&nbsp;承認:&nbsp;");
            text_order.Append(karte_order.CreateUser);
            text_order.Append("&nbsp;");
            text_order.Append(karte_order.CreateDateDisplay);
            text_order.Append(")</span><br>");
            if (karte_order.HokenGroups.Any())
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
                                text_order.Append("<span>&nbsp;*<span style=\"color: #3328fc \"><s> ");
                                text_order.Append(rp.CreateDateDisplay);
                                text_order.Append("&nbsp;");
                                text_order.Append(rp.CreateName);
                                text_order.Append("</s>&nbsp;");
                                text_order.Append(rp.UpdateDateDisplay);
                                text_order.Append("&nbsp;");
                                text_order.Append(rp.CreateName);
                                text_order.Append("</span></span><br>");

                                if (rp.RpName != string.Empty)
                                {
                                    text_order.Append("<span><s>&nbsp;&nbsp;&nbsp;");
                                    text_order.Append(rp.RpName);
                                    text_order.Append("</s></span><br>");
                                }
                                if (rp.OdrDetails.Count > 0)
                                {
                                    foreach (var detail in rp.OdrDetails.Where(detail => detail.ItemName != string.Empty))
                                    {
                                        text_order.Append("<span><s>&nbsp;&nbsp;&nbsp;&nbsp;");
                                        text_order.Append(detail.ItemName);
                                        text_order.Append("</s></span><br>");
                                    }
                                }
                            }
                            else if (rp.IsDeleted == 0)
                            {
                                text_order.Append("<span>&nbsp;*<span style=\"color: #a51652 \">  ");
                                text_order.Append(rp.CreateDateDisplay);
                                text_order.Append("&nbsp;");
                                text_order.Append(rp.CreateName);
                                text_order.Append("</span></span><br>");
                                if (rp.RpName != string.Empty)
                                {
                                    text_order.Append("<span>&nbsp;&nbsp;&nbsp;");
                                    text_order.Append(rp.RpName);
                                    text_order.Append("</span><br>");
                                }
                                if (rp.OdrDetails.Count > 0)
                                {
                                    foreach (var detail in rp.OdrDetails.Where(detail => detail.ItemName != string.Empty))
                                    {
                                        text_order.Append("<span>&nbsp;&nbsp;&nbsp;&nbsp;");
                                        text_order.Append(detail.ItemName);
                                        text_order.Append("</span><br>");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            result.Add(new RichTextKarteOrder(
                    CIUtil.SDateToShowSDate(karte_order.SinDate) + ".....................",
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
                var createUser = listUsers?.FirstOrDefault(uke => uke.UserId == raiinInf.CreateId);
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
                                                    raiinInf.CreateDate.ToString("yyyy/MM/dd HH:mm"),
                                                    createUser != null ? createUser.Sname : string.Empty
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
        //FilterData(ref result, inputData);


        #endregion
        if (result?.Count > 0)
        {
            return (result, Karte2Status.Success);
        }
        else
            return new(new List<HistoryKarteOdrRaiinItem>(), Karte2Status.NoData);
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
    #endregion
}