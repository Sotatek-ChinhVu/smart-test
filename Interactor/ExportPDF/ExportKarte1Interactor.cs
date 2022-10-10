using DevExpress.Models;
using Domain.Constant;
using Domain.Models.Diseases;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Domain.Models.PatientInfor;
using Domain.Models.PatientInfor.Domain.Models.PatientInfor;
using Helper.Common;
using UseCase.ExportPDF.ExportKarte1;

namespace Interactor.ExportPDF;

public class ExportKarte1Interactor : IExportKarte1InputPort
{
    private readonly IPtDiseaseRepository _diseaseRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IInsuranceRepository _insuranceRepository;

    public ExportKarte1Interactor(IPtDiseaseRepository diseaseRepository, IPatientInforRepository patientInforRepository, IInsuranceRepository insuranceRepository)
    {
        _diseaseRepository = diseaseRepository;
        _patientInforRepository = patientInforRepository;
        _insuranceRepository = insuranceRepository;
    }

    public ExportKarte1OutputData Handle(ExportKarte1InputData input)
    {
        if (input.HpId <= 0)
        {
            return new ExportKarte1OutputData(ExportKarte1Status.InvalidHpId);
        }
        else if (input.SinDate <= 10000000 || input.SinDate >= 99999999)
        {
            return new ExportKarte1OutputData(ExportKarte1Status.InvalidSindate);
        }

        var ptInf = _patientInforRepository.GetById(input.HpId, input.PtId, input.SinDate, 0);
        if (ptInf == null)
        {
            return new ExportKarte1OutputData(ExportKarte1Status.PtInfNotFould);
        }
        var hoken = _insuranceRepository.GetPtHokenInf(input.HpId, input.HokenPid, input.PtId, input.SinDate);
        if (hoken == null)
        {
            return new ExportKarte1OutputData(ExportKarte1Status.HokenNotFould);
        }
        var ptByomeis = _diseaseRepository.GetListPatientDiseaseForReport(input.HpId, input.PtId, input.HokenPid, input.SinDate, input.TenkiByomei);

        var listByomeiModelsPage1 = ConvertToListKarte1ByomeiModel(ptByomeis).Item1;
        var listByomeiModelsPage2 = ConvertToListKarte1ByomeiModel(ptByomeis).Item2;

        var dataModel = ConvertToKarte1ExportModel(ptInf, hoken, listByomeiModelsPage1, listByomeiModelsPage2);
        try
        {
            var res = _karte1Export.ExportToPdf(dataModel);
            if (res.Length > 0)
            {
                return new ExportKarte1OutputData(Convert.ToBase64String(res.ToArray()), ExportKarte1Status.Success);
            }
            return new ExportKarte1OutputData(ExportKarte1Status.CanNotExportPdf);
        }
        catch (Exception)
        {
            return new ExportKarte1OutputData(ExportKarte1Status.Failed);
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
}
