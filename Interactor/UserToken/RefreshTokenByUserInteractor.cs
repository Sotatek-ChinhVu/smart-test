using Domain.Models.UserToken;
using UseCase.UserToken.GetInfoRefresh;

namespace Interactor.UserToken
{
    public class RefreshTokenByUserInteractor : IRefreshTokenByUserInputPort
    {
        private readonly IUserTokenRepository _userTokenRepository;

        public RefreshTokenByUserInteractor(IUserTokenRepository userTokenRepository) => _userTokenRepository = userTokenRepository;

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
