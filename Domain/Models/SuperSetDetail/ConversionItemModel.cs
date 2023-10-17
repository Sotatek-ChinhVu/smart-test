using Helper.Common;
using Helper.Constants;
using Helper.Extension;

namespace Domain.Models.SuperSetDetail;

public class ConversionItemModel
{
    public ConversionItemModel(string itemCd, string itemName, double quantity, string cmtName, string cmtOpt, string unitName, double ten, int handanGrpKbn, string masterSbt, int endDate, int sortNo, string kensaItemCd, int kensaItemSeqNo, string ipnNameCd)
    {
        ItemCd = itemCd;
        ItemName = itemName;
        Quantity = quantity;
        CmtName = cmtName;
        CmtOpt = cmtOpt;
        UnitName = unitName;
        Ten = ten;
        HandanGrpKbn = handanGrpKbn;
        MasterSbt = masterSbt;
        EndDate = endDate;
        SortNo = sortNo;
        KensaItemCd = kensaItemCd;
        KensaItemSeqNo = kensaItemSeqNo;
        IpnNameCd = ipnNameCd;
    }

    public ConversionItemModel(string itemCd, string itemName)
    {
        ItemCd = itemCd;
        ItemName = itemName;
        CmtName = string.Empty;
        CmtOpt = string.Empty;
        UnitName = string.Empty;
        MasterSbt = string.Empty;
        KensaItemCd = string.Empty;
        IpnNameCd = string.Empty;
    }

    public ConversionItemModel UpdateCmtName(string cmtName, string cmtOpt)
    {
        CmtName = cmtName;
        CmtOpt = cmtOpt;
        return this;
    }

    public ConversionItemModel UpdateConversionItem(double quantity, string unitName, double ten, int handanGrpKbn, int endDate, string kensaItemCd, int kensaItemSeqNo, string ipnNameCd)
    {
        Quantity = quantity;
        UnitName = unitName;
        Ten = ten;
        HandanGrpKbn = handanGrpKbn;
        EndDate = endDate;
        KensaItemCd = kensaItemCd;
        KensaItemSeqNo = kensaItemSeqNo;
        IpnNameCd = ipnNameCd;
        return this;
    }

    public string ItemCd { get; private set; }

    public string ItemName { get; private set; }

    public double Quantity { get; private set; }

    public string CmtName { get; private set; }

    public string CmtOpt { get; private set; }

    public string UnitName { get; private set; }

    public double Ten { get; private set; }

    public int HandanGrpKbn { get; private set; }

    public string MasterSbt { get; private set; }

    public int EndDate { get; private set; }

    public int SortNo { get; private set; }

    public string KensaItemCd { get; private set; }

    public int KensaItemSeqNo { get; private set; }

    public string IpnNameCd { get; private set; }

    public string QuantityBinding
    {
        get
        {
            if (Is830Cmt || Is831Cmt || Is880Cmt)
            {
                return string.Empty;
            }
            else if (Is840Cmt || Is842Cmt || Is850Cmt || Is851Cmt || Is852Cmt || Is853Cmt)
            {
                return CmtOpt;
            }
            else if (string.IsNullOrWhiteSpace(UnitName))
            {
                return string.Empty;
            }
            else
            {
                return Quantity.AsString();
            }
        }
    }

    public string EndDateBinding
    {
        get
        {
            if (EndDate <= 0 || EndDate == 99999999)
            {
                return string.Empty;
            }
            else
            {
                return CIUtil.SDateToShowSDate(EndDate);
            }
        }
    }

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

}
