using Domain.Models.Yousiki;
using UseCase.Yousiki.GetYousiki1InfModelWithCommonInf;

namespace Interactor.Yousiki.GetYousiki1InfModelWithCommonInf;

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
            var result = _yousikiRepository.GetYousiki1InfModelWithCommonInf(inputData.HpId, inputData.SinYm, inputData.PtNum, inputData.DataTypes, inputData.Status);
            return new GetYousiki1InfModelWithCommonInfOutputData(result, GetYousiki1InfModelWithCommonInfStatus.Successed);
        }
        finally
        {
            _yousikiRepository.ReleaseResource();
        }
    }
}
