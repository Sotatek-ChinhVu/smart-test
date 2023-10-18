﻿namespace Domain.Models.SuperSetDetail;

public class OdrSetNameModel
{
    public OdrSetNameModel(int setCd, int setKbn, int setKbnEdaNo, int generationId, int level1, int level2, int level3, string setName, int rowNo, int sortNo, string rpName, string itemCd, string itemName, string cmtName, string cmtOpt, double quantity, string unitName, int unitSbt, double odrTermVal, int kohatuKbn, string ipnCd, string ipnName, int drugKbn, int sinKouiKbn, int syohoKbn, int syohoLimitKbn, double ten, int handanGrpKbn, string masterSbt, int startDate, int endDate, int yohoKbn, int odrKouiKbn)
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
}
