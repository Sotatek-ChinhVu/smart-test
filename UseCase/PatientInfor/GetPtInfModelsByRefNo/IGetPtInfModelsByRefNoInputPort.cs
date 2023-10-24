using UseCase.Core.Async.Core;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.GetPtInfModelsByRefNo;

public interface IGetPtInfModelsByRefNoInputPort : IInputPort<GetPtInfModelsByRefNoInputData, GetPtInfModelsByRefNoOutputData>
{
}
