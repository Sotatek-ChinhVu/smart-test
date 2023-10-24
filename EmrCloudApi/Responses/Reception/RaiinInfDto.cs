using Domain.Models.Reception;

namespace EmrCloudApi.Responses.Reception;

public class RaiinInfDto
{
    public RaiinInfDto(ReceptionModel model)
    {
        PtId = model.PtId;
        SinDate = model.SinDate;
        RaiinNo = model.RaiinNo;
        OyaRaiinNo = model.OyaRaiinNo;
        HokenPid = model.HokenPid;
        Status = model.Status;
        UketukeSbt = model.UketukeSbt;
        UketukeNo = model.UketukeNo;
        SinStartTime = model.SinStartTime ?? string.Empty;
        SinEndTime = model.SinEndTime ?? string.Empty;
        KaikeiTime = model.KaikeiTime ?? string.Empty;
        KaikeiId = model.KaikeiId;
        KaId = model.KaId;
        TantoId = model.TantoId;
        SyosaisinKbn = model.SyosaisinKbn;
    }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }

    public int UketukeNo { get; private set; }

    public long RaiinNo { get; private set; }

    public long OyaRaiinNo { get; private set; }

    public int HokenPid { get; private set; }

    public int SanteiKbn { get; private set; }

    public int Status { get; private set; }

    public int UketukeSbt { get; private set; }

    public string SinStartTime { get; private set; }

    public string SinEndTime { get; private set; }

    public string KaikeiTime { get; private set; }

    public int KaikeiId { get; private set; }

    public int KaId { get; private set; }

    public int TantoId { get; private set; }

    public int SyosaisinKbn { get; private set; }
}
