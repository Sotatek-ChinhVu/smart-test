using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.SearchPatientInfoByPtNum;

public class SearchPatientInfoByPtNumInputData : IInputData<SearchPatientInfoByPtNumOutputData>
{
    public SearchPatientInfoByPtNumInputData(int hpId, long ptNum, int sinDate)
    {
        HpId = hpId;
        PtNum = ptNum;
        SinDate = sinDate;
    }

    public int HpId { get; private set; }

    public long PtNum { get; private set; }

    public int SinDate { get; private set; }
}
