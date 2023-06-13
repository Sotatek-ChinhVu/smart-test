using Domain.Models.User;
using UseCase.User.GetListUserByCurrentUser;

namespace Interactor.User
{
    public class GetListUserByCurrentUserInteractor : IGetListUserByCurrentUserInputPort
    {
        private readonly IUserRepository _userRepository;

        public GetListUserByCurrentUserInteractor(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public GetListUserByCurrentUserOutputData Handle(GetListUserByCurrentUserInputData input)
        {
            try
            {
                if (input.HpId <= 0)
                {
                    return new GetListUserByCurrentUserOutputData(GetListUserByCurrentUserStatus.InvalidHpId, new List<UserMstModel>(), false);
                }
                if (input.UserId <= 0)
                {
                    return new GetListUserByCurrentUserOutputData(GetListUserByCurrentUserStatus.InvalidUserId, new List<UserMstModel>(), false);
                }

                var users = _userRepository.GetUsersByCurrentUser(input.HpId, input.UserId);
                bool getShowRenkei = _userRepository.GetShowRenkeiCd1ColumnSetting();
                if (users.Any())
                    return new GetListUserByCurrentUserOutputData(GetListUserByCurrentUserStatus.Successful, users, getShowRenkei);
                else
                    return new GetListUserByCurrentUserOutputData(GetListUserByCurrentUserStatus.NoData, users, getShowRenkei);
            }
            finally
            {
                _userRepository.ReleaseResource();
            }
        }
    }
}
