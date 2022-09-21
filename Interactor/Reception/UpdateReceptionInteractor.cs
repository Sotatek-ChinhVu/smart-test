using Domain.Models.Reception;
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
        var success = _receptionRepository.Update(input.Dto);
        var status = success ? UpdateReceptionStatus.Success : UpdateReceptionStatus.NotFound;
        return new UpdateReceptionOutputData(status);
    }
}
