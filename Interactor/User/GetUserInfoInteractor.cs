using Domain.Models.User;
using UseCase.User.UserInfo;

namespace Interactor.User
{
    public class GetUserInfoInteractor : IGetUserInfoInputPort
    {
        private readonly IUserRepository _userRepository;

        public GetUserInfoInteractor(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public GetUserInfoOutputData Handle(GetUserInfoInputData inputData)
        {
            try
            {
                var userInfo = _userRepository.GetUserInfo(inputData.HpId, inputData.UserId);

                return new GetUserInfoOutputData(GetUserInfoStatus.Success, userInfo);
            }
            finally
            {
                _userRepository.ReleaseResource();
            }
        }
    }
}
