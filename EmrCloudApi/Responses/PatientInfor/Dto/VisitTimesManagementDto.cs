using Domain.Models.PatientInfor;
using Helper.Common;

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
        IsOutHospital = model.IsOutHospital;
    }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }

    public int HokenPid { get; private set; }

    public int KohiId { get; private set; }

    public int SeqNo { get; private set; }

    public string SortKey { get; private set; }

    public bool IsOutHospital { get; private set; }

    public string DisplaySinDate => IsOutHospital ? string.Empty : (SinDate > 0 ? CIUtil.SDateToShowSDate(SinDate) : string.Empty);

    public string InOrOutHospital => IsOutHospital ? "他院" : "自院";
}
