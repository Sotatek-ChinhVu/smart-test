using UseCase.Core.Sync.Core;

namespace UseCase.SetKbnMst.GetSetKbnMstListByGenerationId;

public class GetSetKbnMstListByGenerationIdInputData : IInputData<GetSetKbnMstListByGenerationIdOutputData>
{
    public GetSetKbnMstListByGenerationIdInputData(int hpId, int generationId)
    {
        HpId = hpId;
        GenerationId = generationId;
    }

    public int HpId { get; private set; }

    public int GenerationId { get; private set; }
}
