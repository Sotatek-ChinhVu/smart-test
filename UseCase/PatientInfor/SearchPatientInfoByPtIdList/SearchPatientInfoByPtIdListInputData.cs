using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.SearchPatientInfoByPtIdList;

public class SearchPatientInfoByPtIdListInputData : IInputData<SearchPatientInfoByPtIdListOutputData>
{
    public SearchPatientInfoByPtIdListInputData(int hpId, List<long> ptIdList)
    {
        HpId = hpId;
        PtIdList = ptIdList;
    }

    public int HpId { get; private set; }

    public List<long> PtIdList { get; private set; }
}
