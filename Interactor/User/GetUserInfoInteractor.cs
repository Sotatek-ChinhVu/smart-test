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

                return new GetUserInfoOutputData(GetUserInfoStatus.Success, new UserMstDto(userInfo.Id, userInfo.UserId, userInfo.JobCd, userInfo.ManagerKbn, userInfo.KaId, userInfo.KaSName, userInfo.KanaName, userInfo.Name, userInfo.Sname, userInfo.LoginId, userInfo.MayakuLicenseNo, userInfo.StartDate, userInfo.EndDate, userInfo.SortNo, userInfo.RenkeiCd1, userInfo.DrName, userInfo.HpId, userInfo.Permissions));
            }
            finally
            {
                _userRepository.ReleaseResource();
            }
        }
    }
}
