using UseCase.Core.Sync.Core;
using UseCase.PatientInfor.SearchSimple;

namespace UseCase.PatientInfor.SearchAdvanced;

public class SearchPatientInfoAdvancedOutputData : IOutputData
{
    public SearchPatientInfoAdvancedOutputData(SearchPatientInfoAdvancedStatus status, List<PatientInfoWithGroup> patientInfos)
    {
        Status = status;
        PatientInfos = patientInfos;
    }

    public SearchPatientInfoAdvancedStatus Status { get; private set; }
    public List<PatientInfoWithGroup> PatientInfos { get; private set; }
}
