using Domain.Models.PatientInfor;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.SearchPatientInfoByPtIdList;

public class SearchPatientInfoByPtIdListOutputData : IOutputData
{
    public SearchPatientInfoByPtIdListOutputData(List<PatientInforModel> ptInfList, SearchPatientInfoByPtIdListStatus status)
    {
        PtInfList = ptInfList;
        Status = status;
    }

    public List<PatientInforModel> PtInfList { get; private set; }

    public SearchPatientInfoByPtIdListStatus Status { get; private set; }
}
