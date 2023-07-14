using Domain.Models.Lock;
using UseCase.Lock.CheckLockOpenAccounting;

namespace Interactor.Lock;

public class CheckLockOpenAccountingInteractor : ICheckLockOpenAccountingInputPort
{
    private readonly ILockRepository _lockRepository;

    public CheckLockOpenAccountingInteractor(ILockRepository lockRepository)
    {
        _lockRepository = lockRepository;
    }

    public CheckLockOpenAccountingOutputData Handle(CheckLockOpenAccountingInputData inputData)
    {
        try
        {
            var result = _lockRepository.CheckLockOpenAccounting(inputData.HpId, inputData.PtId, inputData.RaiinNo, inputData.UserId);
            return new CheckLockOpenAccountingOutputData(result.Any() ? CheckLockOpenAccountingStatus.Locked : CheckLockOpenAccountingStatus.NotLock, result);
        }
        finally
        {
            _lockRepository.ReleaseResource();
        }
    }
}
