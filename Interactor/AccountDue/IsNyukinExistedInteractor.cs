using Domain.Models.AccountDue;
using UseCase.AccountDue.IsNyukinExisted;

namespace Interactor.AccountDue;

public class IsNyukinExistedInteractor : IIsNyukinExistedInputPort
{
    private readonly IAccountDueRepository _accountDueRepository;

    public IsNyukinExistedInteractor(IAccountDueRepository accountDueRepository)
    {
        _accountDueRepository = accountDueRepository;
    }

    public IsNyukinExistedOutputData Handle(IsNyukinExistedInputData inputData)
    {
        try
        {
            var allow = _accountDueRepository.IsNyukinExisted(inputData.HpId, inputData.PtId, inputData.RaiinNo, inputData.SinDate);
            return new IsNyukinExistedOutputData(IsNyukinExistedStatus.Successed, allow);
        }
        finally
        {
            _accountDueRepository.ReleaseResource();
        }
    }
}
