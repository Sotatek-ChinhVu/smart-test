using Domain.Models.KensaIrai;

namespace EmrCloudApi.Responses.MainMenu.Dto;

public class KensaInfDto
{
    public KensaInfDto(KensaInfModel model)
    {
        PtId = model.PtId;
        IraiDate = model.IraiDate;
        RaiinNo = model.RaiinNo;
        IraiCd = model.IraiCd;
        InoutKbn = model.InoutKbn;
        Status = model.Status;
        TosekiKbn = model.TosekiKbn;
        SikyuKbn = model.SikyuKbn;
        ResultCheck = model.ResultCheck;
        CenterCd = model.CenterCd;
        Nyubi = model.Nyubi;
        Yoketu = model.Yoketu;
        Bilirubin = model.Bilirubin;
        IsDeleted = model.IsDeleted;
        CreateId = model.CreateId;
        PrimaryKbn = model.PrimaryKbn;
        PtNum = model.PtNum;
        PtName = model.PtName;
        KensaCenterName = model.KensaCenterName;
        CreateDate = model.CreateDate;
        KensaInfDetailModelList = model.KensaInfDetailModelList.Select(item => new KensaInfDetailDto(item)).ToList();
    }

    public long PtId { get; private set; }

    public int IraiDate { get; private set; }

    public long RaiinNo { get; private set; }

    public long IraiCd { get; private set; }

    public int InoutKbn { get; private set; }

    public int Status { get; private set; }

    public int TosekiKbn { get; private set; }

    public int SikyuKbn { get; private set; }

    public int ResultCheck { get; private set; }

    public string CenterCd { get; private set; }

    public string Nyubi { get; private set; }

    public string Yoketu { get; private set; }

    public string Bilirubin { get; private set; }

    public bool IsDeleted { get; private set; }

    public int CreateId { get; private set; }

    public int PrimaryKbn { get; private set; }

    public long PtNum { get; private set; }

    public string PtName { get; private set; }

    public string KensaCenterName { get; private set; }

    public DateTime CreateDate { get; private set; }

    public List<KensaInfDetailDto> KensaInfDetailModelList { get; private set; }
}
