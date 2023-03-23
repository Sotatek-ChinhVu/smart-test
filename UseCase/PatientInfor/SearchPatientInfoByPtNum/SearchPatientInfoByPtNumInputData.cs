using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.SearchPatientInfoByPtNum;

public class SearchPatientInfoByPtNumInputData : IInputData<SearchPatientInfoByPtNumOutputData>
{
    public SearchPatientInfoByPtNumInputData(int hpId, long ptNum)
    {
        HpId = hpId;
        PtNum = ptNum;
    }

    public int HpId { get; private set; }

    public long PtNum { get; private set; }
}
