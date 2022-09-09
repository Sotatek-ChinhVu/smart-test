using Domain.Models.Reception;
using UseCase.Reception.Insert;

namespace Interactor.Reception;

public class InsertReceptionInteractor : IInsertReceptionInputPort
{
    private readonly IReceptionRepository _receptionRepository;

    public InsertReceptionInteractor(IReceptionRepository receptionRepository)
    {
        _receptionRepository = receptionRepository;
    }

    public InsertReceptionOutputData Handle(InsertReceptionInputData input)
    {
        var raiinNo = _receptionRepository.Insert(input.Dto);
        return new InsertReceptionOutputData(InsertReceptionStatus.Success, raiinNo);
    }
}
