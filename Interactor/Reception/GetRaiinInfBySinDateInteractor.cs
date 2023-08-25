using Domain.Models.Reception;
using UseCase.Reception.GetRaiinInfBySinDate;

namespace Interactor.Reception;

public class GetRaiinInfBySinDateInteractor : IGetRaiinInfBySinDateInputPort
{
    private readonly IReceptionRepository _receptionRepository;

    public GetRaiinInfBySinDateInteractor(IReceptionRepository receptionRepository)
    {
        _receptionRepository = receptionRepository;
    }

    public GetRaiinInfBySinDateOutputData Handle(GetRaiinInfBySinDateInputData inputData)
    {
        try
        {
            var result = _receptionRepository.GetRaiinInfBySinDate(inputData.HpId, inputData.PtId, inputData.SinDate);
            return new GetRaiinInfBySinDateOutputData(result, GetRaiinInfBySinDateStatus.Successed);
        }
        finally
        {
            _receptionRepository.ReleaseResource();
        }
    }
}
