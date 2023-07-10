using UseCase.Core.Sync.Core;

namespace UseCase.User.GetListUserByCurrentUser
{
    public class GetListUserByCurrentUserInputData : IInputData<GetListUserByCurrentUserOutputData>
    {
        public GetListUserByCurrentUserInputData(int hpId, int userId)
        {
            HpId = hpId;
            UserId = userId;
        }

        public int HpId { get; private set; }

        public int UserId { get; private set; }
    }
}
