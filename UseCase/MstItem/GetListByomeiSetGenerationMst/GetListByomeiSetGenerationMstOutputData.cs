using Domain.Models.ByomeiSetGenerationMst;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetListByomeiSetGenerationMst
{
    public sealed class GetListByomeiSetGenerationMstOutputData : IOutputData
    {
        public GetListByomeiSetGenerationMstOutputData(List<ByomeiSetGenerationMstModel> byomeiSetGenerationMsts, GetListByomeiSetGenerationMstStatus status)
        {
            ByomeiSetGenerationMsts = byomeiSetGenerationMsts;
            Status = status;
        }

        public List<ByomeiSetGenerationMstModel> ByomeiSetGenerationMsts { get; private set; }
        public GetListByomeiSetGenerationMstStatus Status { get; private set; }
    }
}
