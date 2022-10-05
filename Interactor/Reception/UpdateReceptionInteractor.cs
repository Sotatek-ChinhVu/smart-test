using Domain.Models.Reception;
using UseCase.Reception.Insert;
using UseCase.Reception.Update;

namespace Interactor.Reception;

public class UpdateReceptionInteractor : IUpdateReceptionInputPort
{
    private readonly IReceptionRepository _receptionRepository;

    public UpdateReceptionInteractor(IReceptionRepository receptionRepository)
    {
        _receptionRepository = receptionRepository;
    }

    public UpdateReceptionOutputData Handle(UpdateReceptionInputData input)
    {
        ReceptionSaveDto dto = input.Dto;

        if (dto!.Insurances.Any(i => !i.IsValidData()))
        {
            return new UpdateReceptionOutputData(UpdateReceptionStatus.InvalidInsuranceList);
        }

        var success = _receptionRepository.Update(input.Dto);
        var status = success ? UpdateReceptionStatus.Success : UpdateReceptionStatus.NotFound;
        return new UpdateReceptionOutputData(status);
    }
}
