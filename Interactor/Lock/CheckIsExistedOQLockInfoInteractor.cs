using Domain.Models.Lock;
using UseCase.Lock.CheckIsExistedOQLockInfo;

namespace Interactor.Lock;

public class CheckIsExistedOQLockInfoInteractor : ICheckIsExistedOQLockInfoInputPort
{
    private readonly ILockRepository _lockRepository;
    public CheckIsExistedOQLockInfoInteractor(ILockRepository lockRepository)
    {
        _lockRepository = lockRepository;
    }

    public CheckIsExistedOQLockInfoOutputData Handle(CheckIsExistedOQLockInfoInputData inputData)
    {
        try
        {
            var result = _lockRepository.CheckIsExistedOQLockInfo(inputData.HpId, inputData.UserId, inputData.PtId, inputData.FunctionCd, inputData.RaiinNo, inputData.SinDate);
            return new CheckIsExistedOQLockInfoOutputData(result, CheckIsExistedOQLockInfoStatus.Successed);
        }
        finally
        {
            _lockRepository.ReleaseResource();
        }
    }
}
