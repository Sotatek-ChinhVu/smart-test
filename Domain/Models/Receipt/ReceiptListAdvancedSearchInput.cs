using Helper.Enum;

namespace Domain.Models.Receipt;

public class ReceiptListAdvancedSearchInput
{
    public bool IsAdvanceSearch { get; private set; }
    public List<int> HokenSbts { get; private set; }

    #region 確認 Status Kbn
    //public bool IsAll { get; private set; }
    //public bool IsNoSetting { get; private set; }
    //public bool IsSystemSave { get; private set; }
    //public bool IsSave1 { get; private set; }
    //public bool IsSave2 { get; private set; }
    //public bool IsSave3 { get; private set; }
    //public bool IsTempSave { get; private set; }
    //public bool IsDone { get; private set; }
    #endregion

    #region レセプト種別 Receipt Sbt
    //public int HokenKbnQuery { get; private set; }
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

    #region Other
    public int KaId { get; private set; }
    public int DoctorId { get; private set; }
    public string Name { get; private set; }
    public bool IsTestPatientSearch { get; private set; }
    public bool IsNotDisplayPrinted { get; private set; }
    public Dictionary<int, string> GroupSearchModels { get; private set; }


    #endregion
}
