using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetSetDataTenMst
{
    public class GetSetDataTenMstOutputData : IOutputData
    {
        public GetSetDataTenMstStatus Status { get; private set; }
    }
}
