using Domain.Constant;
using Domain.Models.Diseases;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Domain.Models.PatientInfor;
using Helper.Common;
using Reporting.Interface;
using Reporting.Model.ExportKarte1;
using System.Text;

namespace Reporting.Service;

public class ExportKarte1 : IExportKarte1
{
    private readonly IPtDiseaseRepository _diseaseRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IInsuranceRepository _insuranceRepository;

    public ExportKarte1(IPtDiseaseRepository diseaseRepository, IPatientInforRepository patientInforRepository, IInsuranceRepository insuranceRepository)
    {
        _diseaseRepository = diseaseRepository;
        _patientInforRepository = patientInforRepository;
        _insuranceRepository = insuranceRepository;
    }

    public Karte1ExportModel GetDataKarte1(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei)
    {
        var ptInf = _patientInforRepository.GetById(hpId, ptId, sinDate, 0);
        var hoken = _insuranceRepository.GetPtHokenInf(hpId, hokenPid, ptId, sinDate);
        var ptByomeis = _diseaseRepository.GetListPatientDiseaseForReport(hpId, ptId, hokenPid, sinDate, tenkiByomei);
        if (ptInf != null)
        {
            return ConvertToKarte1ExportModel(ptInf, hoken, ptByomeis);
        }
        return new Karte1ExportModel();
    }

    private Karte1ExportModel ConvertToKarte1ExportModel(PatientInforModel ptInf, InsuranceModel hoken, List<PtDiseaseModel> ptByomeis)
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
            hokensyaNo = hoken.HokenInf.HokensyaNo != string.Empty ? hoken.HokenInf.HokensyaNo.PadLeft(8, ' ') : string.Empty;
            if (new int[] { 11, 12, 13 }.Contains(hoken.HokenKbn))
            {
                // 労災
                kigoBango = hoken.HokenInf.RousaiKofuNo ?? string.Empty;
            }
            else if (hoken.HokenKbn == 14)
            {
                // 自賠
                kigoBango = hoken.HokenInf.JibaiHokenName ?? string.Empty;
            }
            else
            {
                kigoBango = hoken.HokenInf.Kigo + "・" + hoken.HokenInf.Bango;
                if (!string.IsNullOrEmpty(hoken.HokenInf.EdaNo))
                {
                    kigoBango = kigoBango + "(" + hoken.HokenInf.EdaNo + ")";
                }
            }
            var warekiEndate = CIUtil.SDateToShowWDate3(hoken.EndDate);
            hokenKigenW = warekiEndate.Ymd != null ? warekiEndate.Ymd : string.Empty;
            var warekiSyutokuDate = CIUtil.SDateToShowWDate3(hoken.HokenInf.SikakuDate);
            hokenSyutokuW = warekiSyutokuDate.Ymd != null ? warekiSyutokuDate.Ymd : string.Empty;
            hokensyaName = hoken.HokenInf.HokensyaName;
            hokensyaAddress = hoken.HokenInf.HokensyaAddress;
            zokugara = hoken.HokenInf.KeizokuKbn > 0 ? hoken.HokenInf.KeizokuKbn.ToString() : string.Empty;
            hokensyaTel = hoken.HokenInf.HokensyaTel;
            futansyaNo_K1 = hoken.Kohi1.FutansyaNo;
            jyukyusyaNo_K1 = hoken.Kohi1.JyukyusyaNo;
            futansyaNo_K2 = hoken.Kohi2.FutansyaNo;
            jyukyusyaNo_K2 = hoken.Kohi2.JyukyusyaNo;
        }

        var listByomeiModels = ConvertToListKarte1ByomeiModel(ptByomeis);

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
                listByomeiModels
            );
    }

    private List<Karte1ByomeiModel> ConvertToListKarte1ByomeiModel(List<PtDiseaseModel> ptByomeis)
    {
        List<Karte1ByomeiModel> listByomeiModels = new();

        if (ptByomeis != null && ptByomeis.Any())
        {
            foreach (var byomei in ptByomeis)
            {
                string byomeiDisplay = string.Empty;
                StringBuilder prefixList = new();
                StringBuilder suffixList = new();

                if (byomei.PrefixSuffixList.Any())
                {
                    foreach (var item in byomei.PrefixSuffixList)
                    {
                        if (item.Code.StartsWith("8"))
                        {
                            suffixList.Append(item.Name);
                        }
                        else
                        {
                            prefixList.Append(item.Name);
                        }
                    }
                }
                byomeiDisplay = prefixList + byomei.Byomei + suffixList;

                if (byomei.SyubyoKbn == 1)
                {
                    byomeiDisplay = "（主）" + byomeiDisplay;
                }
                if (byomeiDisplay.Length >= 64)
                {
                    byomeiDisplay = byomeiDisplay.Substring(0, 64);
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

                listByomeiModels.Add(byomeiModel);
            }
        }
        return listByomeiModels;
    }

}
