using UseCase.Core.Sync.Core;

namespace UseCase.UserToken.SiginRefresh
{
    public interface ISigninRefreshTokenInputPort : IInputPort<SigninRefreshTokenInputData, SigninRefreshTokenOutputData>
    {
    }
}
