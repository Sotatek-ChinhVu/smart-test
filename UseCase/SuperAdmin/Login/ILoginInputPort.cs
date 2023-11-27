using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.Login;

public interface ILoginInputPort : IInputPort<LoginInputData, LoginOutputData>
{
}
