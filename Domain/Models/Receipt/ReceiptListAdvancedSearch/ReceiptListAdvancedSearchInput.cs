using Helper.Enum;

namespace Domain.Models.Receipt.ReceiptListAdvancedSearch;

public class ReceiptListAdvancedSearchInput
{
    public ReceiptListAdvancedSearchInput(bool isAdvanceSearch, string tokki, List<int> hokenSbts, bool isAll, bool isNoSetting, bool isSystemSave, bool isSave1, bool isSave2, bool isSave3, bool isTempSave, bool isDone, int receSbtCenter, int receSbtRight, string hokenHoubetu, int kohi1Houbetu, int kohi2Houbetu, int kohi3Houbetu, int kohi4Houbetu, bool isIncludeSingle, string hokensyaNoFrom, string hokensyaNoTo, long hokensyaNoFromLong, long hokensyaNoToLong, string ptId, long ptIdFrom, long ptIdTo, PtIdSearchOptionEnum ptSearchOption, long tensuFrom, long tensuTo, int lastRaiinDateFrom, int lastRaiinDateTo, int birthDayFrom, int birthDayTo, List<ItemSearchModel> itemList, QuerySearchEnum itemQuery, bool isOnlySuspectedDisease, QuerySearchEnum byomeiQuery, List<SearchByoMstModel> byomeiCdList, bool isFutanIncludeSingle, long futansyaNoFromLong, long futansyaNoToLong, int kaId, int doctorId, string name, bool isTestPatientSearch, bool isNotDisplayPrinted, Dictionary<int, string> groupSearchModels, bool seikyuKbnAll, bool seikyuKbnDenshi, bool seikyuKbnPaper)
    {
        IsAdvanceSearch = isAdvanceSearch;
        Tokki = tokki;
        HokenSbts = hokenSbts;
        IsAll = isAll;
        IsNoSetting = isNoSetting;
        IsSystemSave = isSystemSave;
        IsSave1 = isSave1;
        IsSave2 = isSave2;
        IsSave3 = isSave3;
        IsTempSave = isTempSave;
        IsDone = isDone;
        ReceSbtCenter = receSbtCenter;
        ReceSbtRight = receSbtRight;
        HokenHoubetu = hokenHoubetu;
        Kohi1Houbetu = kohi1Houbetu;
        Kohi2Houbetu = kohi2Houbetu;
        Kohi3Houbetu = kohi3Houbetu;
        Kohi4Houbetu = kohi4Houbetu;
        IsIncludeSingle = isIncludeSingle;
        HokensyaNoFrom = hokensyaNoFrom;
        HokensyaNoTo = hokensyaNoTo;
        HokensyaNoFromLong = hokensyaNoFromLong;
        HokensyaNoToLong = hokensyaNoToLong;
        PtId = ptId;
        PtIdFrom = ptIdFrom;
        PtIdTo = ptIdTo;
        PtSearchOption = ptSearchOption;
        TensuFrom = tensuFrom;
        TensuTo = tensuTo;
        LastRaiinDateFrom = lastRaiinDateFrom;
        LastRaiinDateTo = lastRaiinDateTo;
        BirthDayFrom = birthDayFrom;
        BirthDayTo = birthDayTo;
        ItemList = itemList;
        ItemQuery = itemQuery;
        IsOnlySuspectedDisease = isOnlySuspectedDisease;
        ByomeiQuery = byomeiQuery;
        ByomeiCdList = byomeiCdList;
        IsFutanIncludeSingle = isFutanIncludeSingle;
        FutansyaNoFromLong = futansyaNoFromLong;
        FutansyaNoToLong = futansyaNoToLong;
        KaId = kaId;
        DoctorId = doctorId;
        Name = name;
        IsTestPatientSearch = isTestPatientSearch;
        IsNotDisplayPrinted = isNotDisplayPrinted;
        GroupSearchModels = groupSearchModels;
        SeikyuKbnAll = seikyuKbnAll;
        SeikyuKbnDenshi = seikyuKbnDenshi;
        SeikyuKbnPaper = seikyuKbnPaper;
    }

    public bool IsAdvanceSearch { get; private set; }

    public string Tokki { get; private set; }

    public List<int> HokenSbts { get; private set; }

    #region 確認 Status Kbn
    public bool IsAll { get; private set; }

    public bool IsNoSetting { get; private set; }

    public bool IsSystemSave { get; private set; }

    public bool IsSave1 { get; private set; }

    public bool IsSave2 { get; private set; }

    public bool IsSave3 { get; private set; }

    public bool IsTempSave { get; private set; }

    public bool IsDone { get; private set; }
    #endregion

    #region レセプト種別 Receipt Sbt
    public int ReceSbtCenter { get; private set; }

    public int ReceSbtRight { get; private set; }
    #endregion

    #region 法別番号 Houbetsu
    public string HokenHoubetu { get; private set; }

    public int Kohi1Houbetu { get; private set; }

    public int Kohi2Houbetu { get; private set; }

    public int Kohi3Houbetu { get; private set; }

    public int Kohi4Houbetu { get; private set; }

    public bool IsIncludeSingle { get; private set; }
    #endregion

    #region 保険者番号 HokensyaNo
    public string HokensyaNoFrom { get; private set; }

    public string HokensyaNoTo { get; private set; }

    public long HokensyaNoFromLong { get; private set; }

    public long HokensyaNoToLong { get; private set; }
    #endregion

    #region 患者番号 PtId
    public string PtId { get; private set; }

    public long PtIdFrom { get; private set; }

    public long PtIdTo { get; private set; }

    public PtIdSearchOptionEnum PtSearchOption { get; private set; }

    #endregion

    #region 点数 Tensu
    public long TensuFrom { get; private set; }

    public long TensuTo { get; private set; }
    #endregion

    #region 最終来院日 LastRaiinDate
    public int LastRaiinDateFrom { get; private set; }

    public int LastRaiinDateTo { get; private set; }
    #endregion

    #region 生年月日 Birthday
    public int BirthDayFrom { get; private set; }

    public int BirthDayTo { get; private set; }
    #endregion

    #region 項目 Item 
    public List<ItemSearchModel> ItemList { get; private set; }

    public QuerySearchEnum ItemQuery { get; private set; }
    #endregion

    #region 病名 Byomei 
    public bool IsOnlySuspectedDisease { get; private set; }

    public QuerySearchEnum ByomeiQuery { get; private set; }

    public List<SearchByoMstModel> ByomeiCdList { get; private set; }
    #endregion

    #region 負担者番号 FutansyaNo
    public bool IsFutanIncludeSingle { get; private set; }

    public long FutansyaNoFromLong { get; private set; }

    public long FutansyaNoToLong { get; private set; }
    #endregion

    #region Other
    public int KaId { get; private set; }

    public int DoctorId { get; private set; }

    public string Name { get; private set; }

    public bool IsTestPatientSearch { get; private set; }

    public bool IsNotDisplayPrinted { get; private set; }

    public Dictionary<int, string> GroupSearchModels { get; private set; }

    public bool SeikyuKbnAll { get; private set; }

    public bool SeikyuKbnDenshi { get; private set; }

    public bool SeikyuKbnPaper { get; private set; }
    #endregion
}
