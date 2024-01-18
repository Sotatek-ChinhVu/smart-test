using Domain.Models.Yousiki;
using UseCase.Yousiki.GetVisitingInfs;

namespace Interactor.Yousiki;

public class GetVisitingInfsInteractor : IGetVisitingInfsInputPort
{
    private readonly IYousikiRepository _yousikiRepository;

    public GetVisitingInfsInteractor(IYousikiRepository yousikiRepository)
    {
        _yousikiRepository = yousikiRepository;
    }

    public GetVisitingInfsOutputData Handle(GetVisitingInfsInputData inputData)
    {
        try
        {
            var result = _yousikiRepository.GetVisitingInfs(inputData.HpId, inputData.PtId, inputData.SinYm);
            return new GetVisitingInfsOutputData(result, GetVisitingInfsStatus.Successed);
        }
        finally
        {
            _yousikiRepository.ReleaseResource();
        }
    }
}
