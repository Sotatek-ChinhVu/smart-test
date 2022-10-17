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
        ReceptionSaveDto dto = input.Dto;

        if (dto!.Insurances.Any(i => !i.IsValidData()))
        {
            return new InsertReceptionOutputData(InsertReceptionStatus.InvalidInsuranceList, 0);
        }

        var raiinNo = _receptionRepository.Insert(dto);
        return new InsertReceptionOutputData(InsertReceptionStatus.Success, raiinNo);
    }
}
