using UseCase.Core.Sync.Core;

namespace UseCase.User.UserInfo
{
    public class GetUserInfoInputData : IInputData<GetUserInfoOutputData>
    {
        public GetUserInfoInputData(int hpId, int userId)
        {
            HpId = hpId;
            UserId = userId;
        }

        public int HpId { get; private set; }
        public int UserId { get; private set; }
    }
}
