using Domain.Models.KensaIrai;

namespace EmrCloudApi.Responses.MainMenu.Dto;

public class KensaIraiLogDto
{
    public KensaIraiLogDto(KensaIraiLogModel model)
    {
        IraiDate = model.IraiDate;
        CenterCd = model.CenterCd;
        CenterName = model.CenterName;
        FromDate = model.FromDate;
        ToDate = model.ToDate;
        IraiFile = model.IraiFile;
        IraiList = Convert.ToBase64String(model.IraiList);
        CreateDate = model.CreateDate;
    }

    public int IraiDate { get; private set; }

    public string CenterCd { get; private set; }

    public string CenterName { get; private set; }

    public int FromDate { get; private set; }

    public int ToDate { get; private set; }

    public string IraiFile { get; private set; }

    public string IraiList { get; private set; }

    public DateTime CreateDate { get; private set; }
}
