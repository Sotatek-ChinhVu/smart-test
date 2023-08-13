using Domain.Models.Lock;
using UseCase.Lock.Get;

namespace Interactor.Lock;

public class CheckLockVisitingInteractor : ICheckLockVisitingInputPort
{
    private readonly ILockRepository _lockRepository;
    public CheckLockVisitingInteractor(ILockRepository lockRepository)
    {
        _lockRepository = lockRepository;
    }
    public CheckLockVisitingOutputData Handle(CheckLockVisitingInputData inputData)
    {
        try
        {
            var result = _lockRepository.GetVisitingLockStatus(inputData.HpId, inputData.UserId, inputData.PtId, inputData.FunctionCode);
            if (result.Any())
            {
                if (!string.IsNullOrEmpty(inputData.TabKey) && result.Exists(item => item.TabKey.Contains(inputData.TabKey)))
                {
                    return new CheckLockVisitingOutputData(CheckLockVisitingStatus.None, new());
                }
                return new CheckLockVisitingOutputData(CheckLockVisitingStatus.Locked, result);
            }
            return new CheckLockVisitingOutputData(CheckLockVisitingStatus.None, new());
        }
        finally
        {
            _lockRepository.ReleaseResource();
        }
    }
}
