using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.GetListPatient;

public class GetPatientInfoInputData : IInputData<GetPatientInfoOutputData>
{
    public GetPatientInfoInputData(int hpId, long ptId)
    {
        HpId = hpId;
        PtId = ptId;
    }
    public int HpId { get; private set; }
    public long PtId { get; private set; }
}
