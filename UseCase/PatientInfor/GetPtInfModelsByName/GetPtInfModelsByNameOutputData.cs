using Domain.Models.PatientInfor;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.GetPtInfModelsByName;

public class GetPtInfModelsByNameOutputData : IOutputData
{
    public GetPtInfModelsByNameOutputData(List<PatientInforModel> patientInfoList, GetPtInfModelsByNameStatus status)
    {
        PatientInfoList = patientInfoList;
        Status = status;
    }

    public List<PatientInforModel> PatientInfoList { get; private set; }

    public GetPtInfModelsByNameStatus Status { get; private set; }
}
