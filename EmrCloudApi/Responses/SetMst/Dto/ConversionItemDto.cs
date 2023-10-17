using Domain.Models.SuperSetDetail;

namespace EmrCloudApi.Responses.SetMst.Dto;

public class ConversionItemDto
{
    public ConversionItemDto(ConversionItemModel model)
    {
        ItemCd = model.ItemCd;
        ItemName = model.ItemName;
        Quantity = model.Quantity;
        CmtName = model.CmtName;
        CmtOpt = model.CmtOpt;
        UnitName = model.UnitName;
        Ten = model.Ten;
        HandanGrpKbn = model.HandanGrpKbn;
        MasterSbt = model.MasterSbt;
        EndDate = model.EndDate;
        SortNo = model.SortNo;
        KensaItemCd = model.KensaItemCd;
        KensaItemSeqNo = model.KensaItemSeqNo;
        IpnNameCd = model.IpnNameCd;
        QuantityBinding = model.QuantityBinding;
        EndDateBinding = model.EndDateBinding;
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

    public string QuantityBinding { get; private set; }

    public string EndDateBinding { get; private set; }
}
