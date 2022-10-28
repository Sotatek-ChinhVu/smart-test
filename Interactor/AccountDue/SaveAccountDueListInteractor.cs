using Domain.Models.AccountDue;
using UseCase.AccountDue.SaveAccountDueList;

namespace Interactor.AccountDue;

public class SaveAccountDueListInteractor : ISaveAccountDueListInputPort
{
    private readonly IAccountDueRepository _accountDueRepository;

    public SaveAccountDueListInteractor(IAccountDueRepository accountDueRepository)
    {
        _accountDueRepository = accountDueRepository;
    }

    public SaveAccountDueListOutputData Handle(SaveAccountDueListInputData inputData)
    {
        throw new NotImplementedException();
    }
}
