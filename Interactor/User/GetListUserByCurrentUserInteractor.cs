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
                    return new GetListUserByCurrentUserOutputData(GetListUserByCurrentUserStatus.InvalidHpId, new List<UserMstModel>(), false, 0);
                }
                if (input.UserId <= 0)
                {
                    return new GetListUserByCurrentUserOutputData(GetListUserByCurrentUserStatus.InvalidUserId, new List<UserMstModel>(), false, 0);
                }

                var users = _userRepository.GetUsersByPermission(input.HpId, input.ManagerKbn);
                bool getShowRenkei = _userRepository.GetShowRenkeiCd1ColumnSetting();
                var currentInfo = _userRepository.GetByUserId(input.UserId);
                if (users.Any())
                    return new GetListUserByCurrentUserOutputData(GetListUserByCurrentUserStatus.Successful, users, getShowRenkei, currentInfo.ManagerKbn);
                else
                    return new GetListUserByCurrentUserOutputData(GetListUserByCurrentUserStatus.NoData, users, getShowRenkei, currentInfo.ManagerKbn);
            }
            finally
            {
                _userRepository.ReleaseResource();
            }
        }
    }
}
