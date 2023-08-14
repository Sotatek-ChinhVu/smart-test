﻿using Helper.Common;
using System.Text.Json.Serialization;

namespace Domain.Models.Receipt.ReceiptListAdvancedSearch;

public class ReceiptListModel
{
    [JsonConstructor]
    public ReceiptListModel(int seikyuKbn, int sinYm, int isReceInfDetailExist, int isPaperRece, int hokenId, int hokenKbn, int output, int fusenKbn, int statusKbn, int isPending, long ptId, long ptNum, string kanaName, string name, int sex, int lastSinDateByHokenId, int birthDay, string receSbt, string hokensyaNo, int tensu, int hokenSbtCd, int kohi1Nissu, int isSyoukiInfExist, int isReceCmtExist, int isSyobyoKeikaExist, string receSeikyuCmt, int lastVisitDate, string kaName, string sName, int isPtKyuseiExist, string futansyaNoKohi1, string futansyaNoKohi2, string futansyaNoKohi3, string futansyaNoKohi4, bool isPtTest, int kohi1ReceKisai, int kohi2ReceKisai, int kohi3ReceKisai, int kohi4ReceKisai, string tokki, int hokenNissu, string receCheckCmt)
    {
        SeikyuKbn = seikyuKbn;
        SinYm = sinYm;
        IsReceInfDetailExist = isReceInfDetailExist;
        IsPaperRece = isPaperRece;
        HokenId = hokenId;
        HokenKbn = hokenKbn;
        Output = output;
        FusenKbn = fusenKbn;
        StatusKbn = statusKbn;
        IsPending = isPending;
        PtId = ptId;
        PtNum = ptNum;
        KanaName = kanaName;
        Name = name;
        Sex = sex;
        Age = CIUtil.SDateToAge(birthDay, lastSinDateByHokenId);
        LastSinDateByHokenId = lastSinDateByHokenId;
        BirthDay = birthDay;
        ReceSbt = receSbt;
        HokensyaNo = hokensyaNo;
        Tensu = tensu;
        HokenSbtCd = hokenSbtCd;
        Kohi1Nissu = kohi1Nissu;
        IsSyoukiInfExist = isSyoukiInfExist;
        IsReceCmtExist = isReceCmtExist;
        IsSyobyoKeikaExist = isSyobyoKeikaExist;
        ReceSeikyuCmt = receSeikyuCmt;
        LastVisitDate = lastVisitDate;
        KaName = kaName;
        SName = sName;
        IsPtKyuseiExist = isPtKyuseiExist;
        FutansyaNoKohi1 = futansyaNoKohi1;
        FutansyaNoKohi2 = futansyaNoKohi2;
        FutansyaNoKohi3 = futansyaNoKohi3;
        FutansyaNoKohi4 = futansyaNoKohi4;
        IsPtTest = isPtTest;
        Kohi1ReceKisai = kohi1ReceKisai;
        Kohi2ReceKisai = kohi2ReceKisai;
        Kohi3ReceKisai = kohi3ReceKisai;
        Kohi4ReceKisai = kohi4ReceKisai;
        Tokki = tokki;
        HokenNissu = hokenNissu;
        ReceCheckCmt = receCheckCmt;
        JibaiHokenName = string.Empty;
        JibaiHokenTanto = string.Empty;
        JibaiHokenTel = string.Empty;
        RousaiCityName = string.Empty;
        RousaiJigyosyoName = string.Empty;
        RousaiKofuNo = string.Empty;
        RousaiPrefName = string.Empty;
    }

    public ReceiptListModel(int seikyuKbn, int sinYm, int isReceInfDetailExist, int isPaperRece, int hokenId, int hokenKbn, int output, int fusenKbn, int statusKbn, int isPending, long ptId, long ptNum, string kanaName, string name, int sex, int lastSinDateByHokenId, int birthDay, string receSbt, string hokensyaNo, int tensu, int hokenSbtCd, int kohi1Nissu, int isSyoukiInfExist, int isReceCmtExist, int isSyobyoKeikaExist, string receSeikyuCmt, int lastVisitDate, string kaName, string sName, int isPtKyuseiExist, string futansyaNoKohi1, string futansyaNoKohi2, string futansyaNoKohi3, string futansyaNoKohi4, bool isPtTest, int kohi1ReceKisai, int kohi2ReceKisai, int kohi3ReceKisai, int kohi4ReceKisai, string tokki, int hokenNissu, string receCheckCmt, string jibaiHokenName, string jibaiHokenTanto, string jibaiHokenTel, string rousaiCityName, string rousaiJigyosyoName, string rousaiKofuNo, string rousaiPrefName)
    {
        SeikyuKbn = seikyuKbn;
        SinYm = sinYm;
        IsReceInfDetailExist = isReceInfDetailExist;
        IsPaperRece = isPaperRece;
        HokenId = hokenId;
        HokenKbn = hokenKbn;
        Output = output;
        FusenKbn = fusenKbn;
        StatusKbn = statusKbn;
        IsPending = isPending;
        PtId = ptId;
        PtNum = ptNum;
        KanaName = kanaName;
        Name = name;
        Sex = sex;
        Age = CIUtil.SDateToAge(birthDay, lastSinDateByHokenId);
        LastSinDateByHokenId = lastSinDateByHokenId;
        BirthDay = birthDay;
        ReceSbt = receSbt;
        HokensyaNo = hokensyaNo;
        Tensu = tensu;
        HokenSbtCd = hokenSbtCd;
        Kohi1Nissu = kohi1Nissu;
        IsSyoukiInfExist = isSyoukiInfExist;
        IsReceCmtExist = isReceCmtExist;
        IsSyobyoKeikaExist = isSyobyoKeikaExist;
        ReceSeikyuCmt = receSeikyuCmt;
        LastVisitDate = lastVisitDate;
        KaName = kaName;
        SName = sName;
        IsPtKyuseiExist = isPtKyuseiExist;
        FutansyaNoKohi1 = futansyaNoKohi1;
        FutansyaNoKohi2 = futansyaNoKohi2;
        FutansyaNoKohi3 = futansyaNoKohi3;
        FutansyaNoKohi4 = futansyaNoKohi4;
        IsPtTest = isPtTest;
        Kohi1ReceKisai = kohi1ReceKisai;
        Kohi2ReceKisai = kohi2ReceKisai;
        Kohi3ReceKisai = kohi3ReceKisai;
        Kohi4ReceKisai = kohi4ReceKisai;
        Tokki = tokki;
        HokenNissu = hokenNissu;
        ReceCheckCmt = receCheckCmt;
        JibaiHokenName = jibaiHokenName;
        JibaiHokenTanto = jibaiHokenTanto;
        JibaiHokenTel = jibaiHokenTel;
        RousaiCityName = rousaiCityName;
        RousaiJigyosyoName = rousaiJigyosyoName;
        RousaiKofuNo = rousaiKofuNo;
        RousaiPrefName = rousaiPrefName;
    }

    public int SeikyuKbn { get; private set; }

    public int SinYm { get; private set; }

    public int IsReceInfDetailExist { get; private set; }

    public int IsPaperRece { get; private set; }

    public int HokenId { get; private set; }

    public int HokenKbn { get; private set; }

    public int Output { get; private set; }

    public int FusenKbn { get; private set; }

    public int StatusKbn { get; private set; }

    public int IsPending { get; private set; }

    public long PtId { get; private set; }

    public long PtNum { get; private set; }

    public string KanaName { get; private set; }

    public string Name { get; private set; }

    public int Sex { get; private set; }

    public int Age { get; private set; }

    public int LastSinDateByHokenId { get; private set; }

    public int BirthDay { get; private set; }

    public string ReceSbt { get; private set; }

    public string HokensyaNo { get; private set; }

    public int Tensu { get; private set; }

    public int HokenSbtCd { get; private set; }

    public int Kohi1Nissu { get; private set; }

    public int IsSyoukiInfExist { get; private set; }

    public int IsReceCmtExist { get; private set; }

    public int IsSyobyoKeikaExist { get; private set; }

    public string ReceSeikyuCmt { get; private set; }

    public int LastVisitDate { get; private set; }

    public string KaName { get; private set; }

    public string SName { get; private set; }

    public int IsPtKyuseiExist { get; private set; }

    public string FutansyaNoKohi1 { get; private set; }

    public string FutansyaNoKohi2 { get; private set; }

    public string FutansyaNoKohi3 { get; private set; }

    public string FutansyaNoKohi4 { get; private set; }

    public bool IsPtTest { get; private set; }

    public int Kohi1ReceKisai { get; private set; }

    public int Kohi2ReceKisai { get; private set; }

    public int Kohi3ReceKisai { get; private set; }

    public int Kohi4ReceKisai { get; private set; }

    public string Tokki { get; private set; }

    public int HokenNissu { get; private set; }

    public string ReceCheckCmt { get; private set; }

    public string JibaiHokenName { get; private set; }

    public string JibaiHokenTanto { get; private set; }

    public string JibaiHokenTel { get; private set; }

    public string RousaiCityName { get; private set; }

    public string RousaiJigyosyoName { get; private set; }

    public string RousaiKofuNo { get; private set; }

    public string RousaiPrefName { get; private set; }
}
