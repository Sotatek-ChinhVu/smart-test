using Domain.Models.SetGenerationMst;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetListSetGenerationMst
{
    public class GetListSetGenerationMstOutputData : IOutputData
    {
        public GetListSetGenerationMstOutputData(List<ListSetGenerationMstModel> listSetGenerationMsts, GetListSetGenerationMstStatus status)
        {
            ListSetGenerationMsts = listSetGenerationMsts;
            Status = status;
        }

        public List<ListSetGenerationMstModel> ListSetGenerationMsts { get; private set; }
        public GetListSetGenerationMstStatus Status { get; private set; }
    }
}
