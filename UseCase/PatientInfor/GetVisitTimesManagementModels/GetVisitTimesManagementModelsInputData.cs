using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.GetVisitTimesManagementModels;

public class GetVisitTimesManagementModelsInputData : IInputData<GetVisitTimesManagementModelsOutputData>
{
    public GetVisitTimesManagementModelsInputData(int hpId, int sinYm, long ptId, int kohiId)
    {
        HpId = hpId;
        SinYm = sinYm;
        PtId = ptId;
        KohiId = kohiId;
    }

    public int HpId { get; private set; }

    public int SinYm { get; private set; }

    public long PtId { get; private set; }

    public int KohiId { get; private set; }
}
