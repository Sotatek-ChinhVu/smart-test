using Domain.Models.Reception;
using UseCase.Reception.GetYoyakuRaiinInf;

namespace Interactor.Reception;

public class GetYoyakuRaiinInfInteractor : IGetYoyakuRaiinInfInputPort
{
    private readonly IReceptionRepository _receptionRepository;

    public GetYoyakuRaiinInfInteractor(IReceptionRepository receptionRepository)
    {
        _receptionRepository = receptionRepository;
    }

    public GetYoyakuRaiinInfOutputData Handle(GetYoyakuRaiinInfInputData inputData)
    {
        try
        {
            var result = _receptionRepository.GetYoyakuRaiinInf(inputData.HpId, inputData.PtId, inputData.SinDate);
            return new GetYoyakuRaiinInfOutputData(result, GetYoyakuRaiinInfStatus.Successed);
        }
        finally
        {
            _receptionRepository.ReleaseResource();
        }
    }
}
