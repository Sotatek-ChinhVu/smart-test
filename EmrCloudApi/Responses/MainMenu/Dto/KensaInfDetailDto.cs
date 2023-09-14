using Domain.Models.KensaIrai;

namespace EmrCloudApi.Responses.MainMenu.Dto;

public class KensaInfDetailDto
{
    public KensaInfDetailDto(KensaInfDetailModel model)
    {
        PtId = model.PtId;
        IraiDate = model.IraiDate;
        RaiinNo = model.RaiinNo;
        IraiCd = model.IraiCd;
        SeqNo = model.SeqNo;
        KensaItemCd = model.KensaItemCd;
        ResultVal = model.ResultVal;
        ResultType = model.ResultType;
        AbnormalKbn = model.AbnormalKbn;
        IsDeleted = model.IsDeleted;
        CmtCd1 = model.CmtCd1;
        CmtCd2 = model.CmtCd2;
    }

    public long PtId { get; private set; }

    public int IraiDate { get; private set; }

    public long RaiinNo { get; private set; }

    public long IraiCd { get; private set; }

    public long SeqNo { get; private set; }

    public string KensaItemCd { get; private set; }

    public string ResultVal { get; private set; }

    public string ResultType { get; private set; }

    public string AbnormalKbn { get; private set; }

    public int IsDeleted { get; private set; }

    public string CmtCd1 { get; private set; }

    public string CmtCd2 { get; private set; }
}
