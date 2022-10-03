﻿using DevExpress.Export;
using DevExpress.Interface;
using DevExpress.Mode;
using DevExpress.Models;
using Domain.Constant;
using Domain.Models.Diseases;
using Domain.Models.Insurance;
using Domain.Models.PatientInfor;
using Helper.Common;
using UseCase.ExportPDF.ExportKarte1;

namespace Interactor.ExportPDF;

public class ExportKarte1Interactor : IExportKarte1InputPort
{
    private readonly IPtDiseaseRepository _diseaseRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IInsuranceRepository _insuranceRepository;
    private readonly IKarte1Export _karte1Export;

    public ExportKarte1Interactor(IPtDiseaseRepository diseaseRepository, IPatientInforRepository patientInforRepository, IInsuranceRepository insuranceRepository, IKarte1Export karte1Export)
    {
        _diseaseRepository = diseaseRepository;
        _patientInforRepository = patientInforRepository;
        _insuranceRepository = insuranceRepository;
        _karte1Export = karte1Export;
    }

    public ExportKarte1OutputData Handle(ExportKarte1InputData input)
    {
        var ptInf = _patientInforRepository.GetById(input.HpId, input.PtId, input.SinDate, 0);
        var ptByomeis = _diseaseRepository.GetListPatientDiseaseForReport(input.HpId, input.PtId, input.HokenPid, input.SinDate, input.TenkiByomei);
        var hoken = _insuranceRepository.GetPtHokenInf(input.HpId, input.HokenPid, input.PtId, input.SinDate);
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
        List<Karte1ByomeiModel> listByomeiModels_p1 = new();
        List<Karte1ByomeiModel> listByomeiModels_p2 = new();
        if (ptByomeis != null && ptByomeis.Count > 0)
        {
            for (int i = 0; i < 5; i++)
            {
                foreach (var byomei in ptByomeis)
                {
                    string byomeiDisplay = byomei.Byomei + byomei.Byomei + byomei.Byomei + byomei.Byomei + byomei.Byomei + byomei.Byomei + byomei.Byomei + "chekckasfjkjfashkjfgakslghjsghjsghjkdshjghjsdghj";
                    if (byomei.SyubyoKbn == 1)
                    {
                        byomeiDisplay = "（主）" + byomeiDisplay;
                    }
                    var byomeiStartDateWFormat = CIUtil.SDateToShowWDate3(byomei.StartDate).Ymd;
                    var byomeiTenkiDateWFormat = CIUtil.SDateToShowWDate3(byomei.TenkiDate).Ymd;
                    var tenkiChusiMaru = byomei.TenkiKbn != TenkiKbnConst.Canceled ? "<div style=\"position: relative;display: inline-block;line-height: 12px;font-size: 9px;\">中⽌•</div>" : "<div style=\"position: relative;display: inline-block;line-height: 12px;font-size: 9px;\">中⽌•<div style=\"position: absolute;top: 0;left: 50%;font-size: 12px;transform: translateX(-50%);\">〇</div></div>";
                    var tenkiSiboMaru = byomei.TenkiKbn != TenkiKbnConst.Dead ? "<div style=\"position: relative;display: inline-block;line-height: 12px;font-size: 9px;\">死亡•</div>" : "<div style=\"position: relative;display: inline-block;line-height: 12px;font-size: 9px;\">死亡•<div style=\"position: absolute;top: 0;left: 50%;font-size: 12px;transform: translateX(-50%);\">〇</div></div>";
                    var tenkiSonota = byomei.TenkiKbn != TenkiKbnConst.Other ? "<div style=\"position: relative;display: inline-block;line-height: 12px;font-size: 9px;\">他</div>" : "<div style=\"position: relative;display: inline-block;line-height: 12px;font-size: 9px;\">他<div style=\"position: absolute;top: 0;left: 50%;font-size: 12px;transform: translateX(-50%);\">〇</div></div>";
                    var tenkiTiyuMaru = byomei.TenkiKbn != TenkiKbnConst.Cured ? "<div style=\"position: relative;display: inline-block;line-height: 12px;font-size: 9px;\">治ゆ•</div>" : "<div style=\"position: relative;display: inline-block;line-height: 12px;font-size: 9px;\">治ゆ•<div style=\"position: absolute;top: 0;left: 50%;font-size: 12px;transform: translateX(-50%);\">〇</div></div>";

                    listByomeiModels_p1.Add(new Karte1ByomeiModel(
                                                byomeiDisplay,
                                                byomeiStartDateWFormat != null ? byomeiStartDateWFormat : string.Empty,
                                                byomeiTenkiDateWFormat != null ? byomeiTenkiDateWFormat : string.Empty,
                                                tenkiChusiMaru,
                                                tenkiSiboMaru,
                                                tenkiSonota,
                                                tenkiTiyuMaru
                                            ));
                }
            }
        }

        var model = new Karte1Model(
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
                listByomeiModels_p1,
                listByomeiModels_p2
            );

        _karte1Export.ExportToPdf(model);

        return new ExportKarte1OutputData(ExportKarte1Status.Success);
    }
}
