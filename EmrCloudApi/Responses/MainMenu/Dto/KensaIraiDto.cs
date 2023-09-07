using Domain.Models.KensaIrai;

namespace EmrCloudApi.Responses.MainMenu.Dto;

public class KensaIraiDto
{
    public KensaIraiDto(KensaIraiModel model)
    {
        SinDate = model.SinDate;
        RaiinNo = model.RaiinNo;
        IraiCd = model.IraiCd;
        PtId = model.PtId;
        PtNum = model.PtNum;
        Name = model.Name;
        KanaName = model.KanaName;
        Sex = model.Sex;
        Birthday = model.Birthday;
        TosekiKbn = model.TosekiKbn;
        SikyuKbn = model.SikyuKbn;
        KensaIraiDetails = model.KensaIraiDetails.Select(item => new KensaIraiDetailDto(item)).ToList();
    }

    public int SinDate { get; private set; }

    public long RaiinNo { get; private set; }

    public long IraiCd { get; private set; }

    public long PtId { get; private set; }

    public long PtNum { get; private set; }

    public string Name { get; private set; }

    public string KanaName { get; private set; }

    public int Sex { get; private set; }

    public int Birthday { get; private set; }

    public int TosekiKbn { get; private set; }

    public int SikyuKbn { get; private set; }

    public List<KensaIraiDetailDto> KensaIraiDetails { get; private set; }
}
