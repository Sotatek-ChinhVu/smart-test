using Domain.Models.Receipt;
using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetListSokatuMst
{
    public class GetListSokatuMstOutputData : IOutputData
    {
        public GetListSokatuMstOutputData(GetListSokatuMstStatus status, List<SokatuMstModel> sokatuMstModels)
        {
            Status = status;
            SokatuMstModels = sokatuMstModels;
        }

        public GetListSokatuMstStatus Status { get; private set; }

        public List<SokatuMstModel> SokatuMstModels { get; private set; }
    }
}
