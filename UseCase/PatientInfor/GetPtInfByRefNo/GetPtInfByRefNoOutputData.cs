using Domain.Models.PatientInfor;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.GetPtInfByRefNo;

public class GetPtInfByRefNoOutputData : IOutputData
{
    public GetPtInfByRefNoOutputData(PatientInforModel patientInfo, GetPtInfByRefNoStatus status)
    {
        PatientInfo = patientInfo;
        Status = status;
    }

    public PatientInforModel PatientInfo { get; private set; }

    public GetPtInfByRefNoStatus Status { get; private set; }
}
