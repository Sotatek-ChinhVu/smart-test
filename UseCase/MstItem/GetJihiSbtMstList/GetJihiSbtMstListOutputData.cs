using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetJihiSbtMstList
{
    public class GetJihiSbtMstListOutputData : IOutputData
    {
        public GetJihiSbtMstListOutputData(GetJihiSbtMstListStatus status, List<JihiSbtMstModel> jihiSbtMstModels)
        {
            Status = status;
            JihiSbtMstModels = jihiSbtMstModels;
        }

        public GetJihiSbtMstListStatus Status { get; private set; }
        public List<JihiSbtMstModel> JihiSbtMstModels { get; private set; }
    }

}
