using Domain.Models.UserToken;
using Domain.SuperAdminModels.Admin;
using UseCase.UserToken.SiginRefresh;

namespace Interactor.SuperAdmin
{
    public class SigInRefreshTokenInteractor : ISigninRefreshTokenInputPort
    {
        private readonly IAdminRepository _userTokenRepository;

        public SigInRefreshTokenInteractor(IAdminRepository userTokenRepository) => _userTokenRepository = userTokenRepository;

        public SigninRefreshTokenOutputData Handle(SigninRefreshTokenInputData inputData)
        {
            if (inputData.UserId <= 0)
                return new SigninRefreshTokenOutputData(SigninRefreshTokenStatus.InvalidUserId);

            if (string.IsNullOrEmpty(inputData.RefreshToken))
                return new SigninRefreshTokenOutputData(SigninRefreshTokenStatus.InvalidRefreshToken);

            try
            {
                bool result = _userTokenRepository.SignInRefreshToken(inputData.UserId, inputData.RefreshToken, inputData.ExpToken);
                if (result)
                    return new SigninRefreshTokenOutputData(SigninRefreshTokenStatus.Successful);
                else
                    return new SigninRefreshTokenOutputData(SigninRefreshTokenStatus.Failed);
            }
            finally
            {
                _userTokenRepository.ReleaseResource();
            }
        }
    }
}
