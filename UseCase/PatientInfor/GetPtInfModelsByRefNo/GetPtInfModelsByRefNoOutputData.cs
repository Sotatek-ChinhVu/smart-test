using Domain.Models.PatientInfor;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.GetPtInfModelsByRefNo;

public class GetPtInfModelsByRefNoOutputData : IOutputData
{
    public GetPtInfModelsByRefNoOutputData(List<PatientInforModel> patientInfoList, GetPtInfModelsByRefNoStatus status)
    {
        PatientInfoList = patientInfoList;
        Status = status;
    }

    public List<PatientInforModel> PatientInfoList { get; private set; }

    public GetPtInfModelsByRefNoStatus Status { get; private set; }
}
