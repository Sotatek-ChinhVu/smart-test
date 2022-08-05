using UseCase.Core.Sync.Core;

namespace UseCase.UketukeSbtMst.GetNext;

public class GetNextUketukeSbtMstInputData : IInputData<GetNextUketukeSbtMstOutputData>
{
    public GetNextUketukeSbtMstInputData(int kbnId)
    {
        KbnId = kbnId;
    }

    public int KbnId { get; private set; }
}
