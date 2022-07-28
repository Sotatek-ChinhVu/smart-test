using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetList;

public class GetReceptionListInputData : IInputData<GetReceptionListOutputData>
{
    public GetReceptionListInputData(int hpId, int sinDate)
    {
        HpId = hpId;
        SinDate = sinDate;
    }

    public int HpId { get; private set; }
    public int SinDate { get; private set; }
}
