using Domain.Models.PatientInfor;

namespace EmrCloudApi.Responses.PatientInfor.Dto;

public class VisitTimesManagementDto
{
    public VisitTimesManagementDto(VisitTimesManagementModel model)
    {
        PtId = model.PtId;
        SinDate = model.SinDate;
        HokenPid = model.HokenPid;
        KohiId = model.KohiId;
        SeqNo = model.SeqNo;
        SortKey = model.SortKey;
    }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }

    public int HokenPid { get; private set; }

    public int KohiId { get; private set; }

    public int SeqNo { get; private set; }

    public string SortKey { get; private set; }
}
