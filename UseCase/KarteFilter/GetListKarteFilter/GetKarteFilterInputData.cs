using UseCase.Core.Sync.Core;

namespace UseCase.KarteFilter.GetListKarteFilter;

public class GetKarteFilterInputData : IInputData<GetKarteFilterOutputData>
{
    public GetKarteFilterInputData(int hpId, int userId)
    {
        HpId = hpId;
        UserId = userId;
    }

    public int HpId { get; private set; }
    public int UserId { get; private set; }
}
