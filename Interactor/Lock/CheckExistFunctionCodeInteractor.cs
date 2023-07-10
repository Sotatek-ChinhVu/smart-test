using Domain.Models.Lock;
using UseCase.Lock.CheckExistFunctionCode;

namespace Interactor.Lock;

public class CheckExistFunctionCodeInteractor : ICheckExistFunctionCodeInputPort
{
    private readonly ILockRepository _lockRepository;
    public CheckExistFunctionCodeInteractor(ILockRepository lockRepository)
    {
        _lockRepository = lockRepository;
    }

    public CheckExistFunctionCodeOutputData Handle(CheckExistFunctionCodeInputData inputData)
    {
        try
        {
            var result = _lockRepository.CheckOpenSpecialNote(inputData.HpId, inputData.FunctionCd, inputData.PtId);
            return new CheckExistFunctionCodeOutputData(result);
        }
        finally
        {
            _lockRepository.ReleaseResource();
        }
    }
}
