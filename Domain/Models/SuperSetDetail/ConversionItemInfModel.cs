namespace Domain.Models.SuperSetDetail;

public class ConversionItemInfModel
{
    public ConversionItemInfModel(string sourceItemCd, string destItemCd, int sortNo, string itemCd, string name, double quantity, double ten, int handanGrpKbn, string masterSbt, int endDate, string kensaItemCd, int kensaItemSeqNo, string ipnNameCd, string odrUnitName, string cnvUnitName)
    {
        SourceItemCd = sourceItemCd;
        DestItemCd = destItemCd;
        SortNo = sortNo;
        ItemCd = itemCd;
        Name = name;
        Quantity = quantity;
        Ten = ten;
        HandanGrpKbn = handanGrpKbn;
        MasterSbt = masterSbt;
        EndDate = endDate;
        KensaItemCd = kensaItemCd;
        KensaItemSeqNo = kensaItemSeqNo;
        IpnNameCd = ipnNameCd;
        OdrUnitName = odrUnitName;
        CnvUnitName = cnvUnitName;
    }

    public string SourceItemCd { get; private set; }

    public string DestItemCd { get; private set; }

    public int SortNo { get; private set; }

    public string ItemCd { get; private set; }

    public string Name { get; private set; }

    public double Quantity { get; private set; }

    public double Ten { get; private set; }

    public int HandanGrpKbn { get; private set; }

    public string MasterSbt { get; private set; }

    public int EndDate { get; private set; }

    public string KensaItemCd { get; private set; }

    public int KensaItemSeqNo { get; private set; }

    public string IpnNameCd { get; private set; }

    public string OdrUnitName { get; private set; }

    public string CnvUnitName { get; private set; }

    public string UnitName => GetUnitName();

    private string GetUnitName()
    {
        string unitName;

        if (!string.IsNullOrEmpty(OdrUnitName))
        {
            unitName = OdrUnitName;
        }
        else if (!string.IsNullOrEmpty(CnvUnitName))
        {
            unitName = CnvUnitName;
        }
        else
        {
            unitName = string.Empty;
        }

        return unitName;
    }
}
