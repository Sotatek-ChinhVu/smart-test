using Domain.Models.KensaIrai;

namespace EmrCloudApi.Responses.MainMenu.Dto;

public class KensaInfMessageDto
{
    public KensaInfMessageDto(KensaInfMessageModel model)
    {
        PtId = model.PtId;
        IraiCd = model.IraiCd;
        PtNum = model.PtNum;
        PtName = model.PtName;
        KensaInfDetailList = model.KensaInfDetailList.Select(item => new KensaInfDetailMessageDto(item)).ToList();
    }

    public long PtId { get; private set; }

    public long IraiCd { get; private set; }

    public long PtNum { get; private set; }

    public string PtName { get; private set; }

    public List<KensaInfDetailMessageDto> KensaInfDetailList { get; private set; }
}

public class KensaInfDetailMessageDto
{
    public KensaInfDetailMessageDto(KensaInfDetailMessageModel model)
    {
        KensaItemCd = model.KensaItemCd;
        KensaMstName = model.KensaMstName;
    }

    public string KensaItemCd { get; private set; }

    public string KensaMstName { get; private set; }
}

