using Domain.Models.User;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetListUser
{
    public sealed class GetListUserOutputData : IOutputData
    {
        public GetListUserOutputData(List<UserMstModel> userMsts, GetListUserStatus status)
        {
            UserMsts = userMsts;
            Status = status;
        }
        public List<UserMstModel> UserMsts { get; private set; }
        public GetListUserStatus Status { get; private set; }
    }
}
