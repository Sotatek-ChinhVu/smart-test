namespace EmrCloudApi.Requests.Receipt;

public class ReceiptListAdvancedSearchRequest
{
    public int SeikyuYm { get; set; }

    public string Tokki { get; set; } = string.Empty;

    public bool IsAdvanceSearch { get; set; }

    public List<int> HokenSbts { get; set; } = new();

    #region 確認 Status Kbn
    public bool IsAll { get; set; }

    public bool IsNoSetting { get; set; }

    public bool IsSystemSave { get; set; }

    public bool IsSave1 { get; set; }

    public bool IsSave2 { get; set; }

    public bool IsSave3 { get; set; }

    public bool IsTempSave { get; set; }

    public bool IsDone { get; set; }
    #endregion

    #region レセプト種別 Receipt Sbt
    public int ReceSbtCenter { get; set; }

    public int ReceSbtRight { get; set; }
    #endregion

    #region 法別番号 Houbetsu
    public string HokenHoubetu { get; set; } = string.Empty;

    public int Kohi1Houbetu { get; set; }

    public int Kohi2Houbetu { get; set; }

    public int Kohi3Houbetu { get; set; }

    public int Kohi4Houbetu { get; set; }

    public bool IsIncludeSingle { get; set; }
    #endregion

    #region 保険者番号 HokensyaNo
    public string HokensyaNoFrom { get; set; } = string.Empty;

    public string HokensyaNoTo { get; set; } = string.Empty;

    public long HokensyaNoFromLong { get; set; }

    public long HokensyaNoToLong { get; set; }
    #endregion

    #region 患者番号 PtId
    public string PtId { get; set; } = string.Empty;

    public long PtIdFrom { get; set; }

    public long PtIdTo { get; set; }

    public int PtSearchOption { get; set; }

    #endregion

    #region 点数 Tensu
    public long TensuFrom { get; set; }

    public long TensuTo { get; set; }
    #endregion

    #region 最終来院日 LastRaiinDate
    public int LastRaiinDateFrom { get; set; }

    public int LastRaiinDateTo { get; set; }
    #endregion

    #region 生年月日 Birthday
    public int BirthDayFrom { get; set; }

    public int BirthDayTo { get; set; }
    #endregion

    #region 項目 Item 
    public List<ItemSearchRequestItem> ItemList { get; set; } = new();

    public int ItemQuery { get; set; }
    #endregion

    #region 病名 Byomei 
    public bool IsOnlySuspectedDisease { get; set; }

    public int ByomeiQuery { get; set; }

    public List<SearchByoMstRequestItem> ByomeiCdList { get; set; } = new();
    #endregion

    #region 負担者番号 FutansyaNo
    public bool IsFutanIncludeSingle { get; set; }

    public long FutansyaNoFromLong { get; set; }

    public long FutansyaNoToLong { get; set; }
    #endregion

    #region Other
    public int KaId { get; set; }

    public int DoctorId { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsTestPatientSearch { get; set; }

    public bool IsNotDisplayPrinted { get; set; }

    public Dictionary<int, string> GroupSearchModels { get; set; } = new();

    public bool SeikyuKbnAll { get; set; }

    public bool SeikyuKbnDenshi { get; set; }

    public bool SeikyuKbnPaper { get; set; }
    #endregion
}
