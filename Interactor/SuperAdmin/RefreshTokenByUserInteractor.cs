using Domain.Models.UserToken;
using Domain.SuperAdminModels.Admin;
using UseCase.UserToken.GetInfoRefresh;

namespace Interactor.SuperAdmin
{
    public class RefreshTokenByUserInteractor : IRefreshTokenByUserInputPort
    {
        private readonly IAdminRepository _userTokenRepository;

        public RefreshTokenByUserInteractor(IAdminRepository userTokenRepository) => _userTokenRepository = userTokenRepository;

        public RefreshTokenByUserOutputData Handle(RefreshTokenByUserInputData inputData)
        {
            if (inputData.UserId <= 0)
                return new RefreshTokenByUserOutputData(RefreshTokenByUserStatus.InvalidUserId, new UserTokenModel());

            if (string.IsNullOrEmpty(inputData.RefreshToken))
                return new RefreshTokenByUserOutputData(RefreshTokenByUserStatus.InvalidRefreshToken, new UserTokenModel());

            try
            {
                UserTokenModel result = _userTokenRepository.RefreshTokenByUser(inputData.UserId, inputData.RefreshToken, inputData.NewRefreshToken);
                if (result.RefreshTokenIsValid)
                    return new RefreshTokenByUserOutputData(RefreshTokenByUserStatus.Successful, result);
                else
                    return new RefreshTokenByUserOutputData(RefreshTokenByUserStatus.CurrentRefreshTokenIsInvalid, result);
            }
            finally
            {
                _userTokenRepository.ReleaseResource();
            }
        }
    }
}
