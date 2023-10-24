using Domain.Models.KensaIrai;

namespace EmrCloudApi.Responses.MainMenu.Dto;

public class KensaIraiDetailDto
{
    public KensaIraiDetailDto(KensaIraiDetailModel model)
    {
        TenKensaItemCd = model.TenKensaItemCd;
        ItemCd = model.ItemCd;
        ItemName = model.ItemName;
        KanaName1 = model.KanaName1;
        CenterCd = model.CenterCd;
        KensaItemCd = model.KensaItemCd;
        CenterItemCd = model.CenterItemCd;
        KensaKana = model.KensaKana;
        KensaName = model.KensaName;
        ContainerCd = model.ContainerCd;
        RpNo = model.RpNo;
        RpEdaNo = model.RpEdaNo;
        RowNo = model.RowNo;
        SeqNo = model.SeqNo;
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
