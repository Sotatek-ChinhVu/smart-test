using Helper.Constants;

namespace Domain.Models.SuperSetDetail;

public class OdrSetNameModel
{
    public OdrSetNameModel(int setCd, int setKbn, int setKbnEdaNo, int generationId, int level1, int level2, int level3, string setName, int rowNo, int sortNo, string rpName, string itemCd, string itemName, string cmtName, string cmtOpt, double quantity, string unitName, int unitSbt, double odrTermVal, int kohatuKbn, string ipnCd, string ipnName, int drugKbn, int sinKouiKbn, int syohoKbn, int syohoLimitKbn, double ten, int handanGrpKbn, string masterSbt, int startDate, int endDate, int yohoKbn, int odrKouiKbn, int buiKbn, long setOrdInfId)
    {
        SetCd = setCd;
        SetKbn = setKbn;
        SetKbnEdaNo = setKbnEdaNo;
        GenerationId = generationId;
        Level1 = level1;
        Level2 = level2;
        Level3 = level3;
        SetName = setName;
        RowNo = rowNo;
        SortNo = sortNo;
        RpName = rpName;
        ItemCd = itemCd;
        ItemName = itemName;
        CmtName = cmtName;
        CmtOpt = cmtOpt;
        Quantity = quantity;
        UnitName = unitName;
        UnitSbt = unitSbt;
        OdrTermVal = odrTermVal;
        KohatuKbn = kohatuKbn;
        IpnCd = ipnCd;
        IpnName = ipnName;
        DrugKbn = drugKbn;
        SinKouiKbn = sinKouiKbn;
        SyohoKbn = syohoKbn;
        SyohoLimitKbn = syohoLimitKbn;
        Ten = ten;
        HandanGrpKbn = handanGrpKbn;
        MasterSbt = masterSbt;
        StartDate = startDate;
        EndDate = endDate;
        YohoKbn = yohoKbn;
        OdrKouiKbn = odrKouiKbn;
        BuiKbn = buiKbn;
        SetOrdInfId = setOrdInfId;
    }

    public OdrSetNameModel(int setCd, int rowNo, string itemCd, string cmtOpt, double quantity, long setOrdInfId)
    {
        SetCd = setCd;
        RowNo = rowNo;
        ItemCd = itemCd;
        CmtOpt = cmtOpt;
        Quantity = quantity;
        SetOrdInfId = setOrdInfId;
        IpnName = string.Empty;
        IpnCd = string.Empty;
        SetName = string.Empty;
        RpName = string.Empty;
        ItemName = string.Empty;
        CmtName = string.Empty;
        UnitName = string.Empty;
        MasterSbt = string.Empty;
    }

    public int SetCd { get; private set; }

    public int SetKbn { get; private set; }

    public int SetKbnEdaNo { get; private set; }

    public int GenerationId { get; private set; }

    public int Level1 { get; private set; }

    public int Level2 { get; private set; }

    public int Level3 { get; private set; }

    public string SetName { get; private set; }

    public int RowNo { get; private set; }

    public int SortNo { get; private set; }

    public string RpName { get; private set; }

    public string ItemCd { get; private set; }

    public string ItemName { get; private set; }

    public string CmtName { get; private set; }

    public string CmtOpt { get; private set; }

    public double Quantity { get; private set; }

    public string UnitName { get; private set; }

    public int UnitSbt { get; private set; }

    public double OdrTermVal { get; private set; }

    public int KohatuKbn { get; private set; }

    public string IpnCd { get; private set; }

    public string IpnName { get; private set; }

    public int DrugKbn { get; private set; }

    public int SinKouiKbn { get; private set; }

    public int SyohoKbn { get; private set; }

    public int SyohoLimitKbn { get; private set; }

    public double Ten { get; private set; }

    public int HandanGrpKbn { get; private set; }

    public string MasterSbt { get; private set; }

    public int StartDate { get; private set; }

    public int EndDate { get; private set; }

    public int YohoKbn { get; private set; }

    public int OdrKouiKbn { get; private set; }

    public int BuiKbn { get; private set; }

    public long SetOrdInfId { get; private set; }

    #region extend param
    public bool Is830Cmt => !string.IsNullOrEmpty(ItemCd) && ItemCd.StartsWith(ItemCdConst.Comment830Pattern);

    public bool Is831Cmt => !string.IsNullOrEmpty(ItemCd) && ItemCd.StartsWith(ItemCdConst.Comment831Pattern);

    public bool Is840Cmt => !string.IsNullOrEmpty(ItemCd) && ItemCd.StartsWith(ItemCdConst.Comment840Pattern) && ItemCd != ItemCdConst.GazoDensibaitaiHozon;

    public bool Is842Cmt => !string.IsNullOrEmpty(ItemCd) && ItemCd.StartsWith(ItemCdConst.Comment842Pattern);

    public bool Is850Cmt => !string.IsNullOrEmpty(ItemCd) && ItemCd.StartsWith(ItemCdConst.Comment850Pattern);

    public bool Is851Cmt => !string.IsNullOrEmpty(ItemCd) && ItemCd.StartsWith(ItemCdConst.Comment851Pattern);

    public bool Is852Cmt => !string.IsNullOrEmpty(ItemCd) && ItemCd.StartsWith(ItemCdConst.Comment852Pattern);

    public bool Is853Cmt => !string.IsNullOrEmpty(ItemCd) && ItemCd.StartsWith(ItemCdConst.Comment853Pattern);

    public bool Is880Cmt => !string.IsNullOrEmpty(ItemCd) && ItemCd.StartsWith(ItemCdConst.Comment880Pattern);

    public bool IsCommentMaster => Is830Cmt || Is831Cmt || Is840Cmt || Is842Cmt || Is850Cmt || Is851Cmt || Is852Cmt || Is853Cmt || Is880Cmt;
    #endregion
}
