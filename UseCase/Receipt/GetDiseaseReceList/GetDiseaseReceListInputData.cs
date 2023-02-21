using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetDiseaseReceList;

public class GetDiseaseReceListInputData : IInputData<GetDiseaseReceListOutputData>
{
    public GetDiseaseReceListInputData(int hpId, int userId, long ptId, int hokenId, int sinYm)
    {
        HpId = hpId;
        UserId = userId;
        PtId = ptId;
        HokenId = hokenId;
        SinYm = sinYm;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long PtId { get; private set; }

    public int HokenId { get; private set; }

    public int SinYm { get; private set; }
}
