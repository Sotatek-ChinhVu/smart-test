using Domain.Models.PatientInfor;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.SearchPatientInfoByPtNum;

public class SearchPatientInfoByPtNumOutputData : IOutputData
{
    public SearchPatientInfoByPtNumOutputData(PatientInforModel patientInfo, SearchPatientInfoByPtNumStatus status)
    {
        PatientInfo = patientInfo;
        Status = status;
    }

    public PatientInforModel PatientInfo { get; private set; }

    public SearchPatientInfoByPtNumStatus Status { get; private set; }
}
