using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetHeaderVistitDate;

public class GetHeaderVistitDateInputData : IInputData<GetHeaderVistitDateOutputData>
{
    public GetHeaderVistitDateInputData(int hpId, int userId, long ptId, int sinDate)
    {
        HpId = hpId;
        UserId = userId;
        PtId = ptId;
        SinDate = sinDate;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }
}
