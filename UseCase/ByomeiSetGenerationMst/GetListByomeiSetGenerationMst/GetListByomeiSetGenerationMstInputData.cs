using UseCase.Core.Sync.Core;

namespace UseCase.ByomeiSetGenerationMst.GetListByomeiSetGenerationMst
{
    public sealed class GetListByomeiSetGenerationMstInputData : IInputData<GetListByomeiSetGenerationMstOutputData>
    {
        public GetListByomeiSetGenerationMstInputData(int hpId)
        {
            HpId = hpId;
        }
        public int HpId {  get; private set; }
    }
}
