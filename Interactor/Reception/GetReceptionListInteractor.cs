using Domain.Models.Reception;
using UseCase.Reception.GetList;

namespace Interactor.Reception;

public class GetReceptionListInteractor : IGetReceptionListInputPort
{
    private readonly IReceptionRepository _receptionRepository;

    public GetReceptionListInteractor(IReceptionRepository receptionRepository)
    {
        _receptionRepository = receptionRepository;
    }

    public GetReceptionListOutputData Handle(GetReceptionListInputData inputData)
    {
        if (inputData.HpId <= 0)
        {
            return new GetReceptionListOutputData("HpId must be greater than 0.");
        }
        if (inputData.SinDate <= 0)
        {
            return new GetReceptionListOutputData("SinDate must be greater than 0.");
        }

        var models = _receptionRepository.GetList(inputData.HpId, inputData.SinDate, inputData.GrpIds);
        return new GetReceptionListOutputData(models);
    }
}
