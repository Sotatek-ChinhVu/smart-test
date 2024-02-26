using Domain.Models.Yousiki;
using UseCase.Yousiki.GetYousiki1InfModelWithCommonInf;

namespace Interactor.Yousiki;

public class GetYousiki1InfModelWithCommonInfInteractor : IGetYousiki1InfModelWithCommonInfInputPort
{
    private readonly IYousikiRepository _yousikiRepository;

    public GetYousiki1InfModelWithCommonInfInteractor(IYousikiRepository yousikiRepository)
    {
        _yousikiRepository = yousikiRepository;
    }

    public GetYousiki1InfModelWithCommonInfOutputData Handle(GetYousiki1InfModelWithCommonInfInputData inputData)
    {
        try
        {
            var yousikiInfList = _yousikiRepository.GetYousiki1InfModelWithCommonInf(inputData.HpId, inputData.SinYm, inputData.PtNum, inputData.DataType, inputData.Status);
            return new GetYousiki1InfModelWithCommonInfOutputData(yousikiInfList, GetYousiki1InfModelWithCommonInfStatus.Successed);
        }
        finally
        {
            _yousikiRepository.ReleaseResource();
        }
    }
}
