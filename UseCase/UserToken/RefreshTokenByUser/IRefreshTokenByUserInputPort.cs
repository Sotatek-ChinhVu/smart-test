using UseCase.Core.Sync.Core;

namespace UseCase.UserToken.GetInfoRefresh
{
    public interface IRefreshTokenByUserInputPort : IInputPort<RefreshTokenByUserInputData, RefreshTokenByUserOutputData>
    {
    }
}
