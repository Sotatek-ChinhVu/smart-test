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
            var checkLock = _lockRepository.CheckLockOpenAccounting(inputData.HpId, inputData.PtId, inputData.RaiinNo);
            return new CheckLockOpenAccountingOutputData(checkLock ? CheckLockOpenAccountingStatus.Locked : CheckLockOpenAccountingStatus.NotLock);
        }
        finally
        {
            _lockRepository.ReleaseResource();
        }
    }
}
