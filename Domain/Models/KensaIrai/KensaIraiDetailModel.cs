namespace Domain.Models.KensaIrai;

public class KensaIraiDetailModel
{
    public KensaIraiDetailModel(string tenKensaItemCd, string itemCd, string itemName, string kanaName1, string centerCd, string kensaItemCd, string centerItemCd, string kensaKana, string kensaName, long containerCd, long rpNo, long rpEdaNo, int rowNo, int seqNo)
    {
        TenKensaItemCd = tenKensaItemCd;
        ItemCd = itemCd;
        ItemName = itemName;
        KanaName1 = kanaName1;
        CenterCd = centerCd;
        KensaItemCd = kensaItemCd;
        CenterItemCd = centerItemCd;
        KensaKana = kensaKana;
        KensaName = kensaName;
        ContainerCd = containerCd;
        RpNo = rpNo;
        RpEdaNo = rpEdaNo;
        RowNo = rowNo;
        SeqNo = seqNo;
    }

    public string TenKensaItemCd { get; private set; }

    public string ItemCd { get; private set; }

    public string ItemName { get; private set; }

    public string KanaName1 { get; private set; }

    public string CenterCd { get; private set; }

    public string KensaItemCd { get; private set; }

    public string CenterItemCd { get; private set; }

    public string KensaKana { get; private set; }

    public string KensaName { get; private set; }

    public long ContainerCd { get; private set; }

    public long RpNo { get; private set; }

    public long RpEdaNo { get; private set; }

    public int RowNo { get; private set; }

    public int SeqNo { get; private set; }
}
