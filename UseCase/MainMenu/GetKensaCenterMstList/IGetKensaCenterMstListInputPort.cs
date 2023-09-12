using UseCase.Core.Async.Core;
using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.GetKensaCenterMstList;

public interface IGetKensaCenterMstListInputPort : IInputPort<GetKensaCenterMstListInputData, GetKensaCenterMstListOutputData>
{
}
