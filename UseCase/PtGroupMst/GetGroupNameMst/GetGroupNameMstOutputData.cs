using Domain.Models.PtGroupMst;
using UseCase.Core.Sync.Core;

namespace UseCase.PtGroupMst.GetGroupNameMst
{
    public class GetGroupNameMstOutputData : IOutputData
    {
        public GetGroupNameMstOutputData(GetGroupNameMstStatus status , List<GroupNameMstModel> data)
        {
            Status = status;
            Data = data;
        }

        public GetGroupNameMstStatus Status { get; private set; }

        public List<GroupNameMstModel> Data { get; private set; }
    }
}
