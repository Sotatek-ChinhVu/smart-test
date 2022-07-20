using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetList;

public class GetReceptionListInputData : IInputData<GetReceptionListOutputData>
{
    public GetReceptionListInputData(int hpId, int sinDate, List<int> grpIds)
    {
        HpId = hpId;
        SinDate = sinDate;
        GrpIds = grpIds;
    }

    public int HpId { get; private set; }
    public int SinDate { get; private set; }
    public List<int> GrpIds { get; private set; }
}
