using UseCase.Core.Sync.Core;

namespace UseCase.User.GetListUserByCurrentUser
{
    public class GetListUserByCurrentUserInputData : IInputData<GetListUserByCurrentUserOutputData>
    {
        public GetListUserByCurrentUserInputData(int hpId, int userId, int managerKbn)
        {
            HpId = hpId;
            UserId = userId;
            ManagerKbn = managerKbn;
        }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public int ManagerKbn { get; private set; }
    }
}
