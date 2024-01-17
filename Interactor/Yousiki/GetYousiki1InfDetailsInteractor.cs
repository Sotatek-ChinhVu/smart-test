using Domain.Models.Yousiki;
using UseCase.Yousiki.GetYousiki1InfDetails;

namespace Interactor.Yousiki;

public class GetYousiki1InfDetailsInteractor : IGetYousiki1InfDetailsInputPort
{
    private readonly IYousikiRepository _yousikiRepository;

    public GetYousiki1InfDetailsInteractor(IYousikiRepository yousikiRepository)
    {
        _yousikiRepository = yousikiRepository;
    }

    public GetYousiki1InfDetailsOutputData Handle(GetYousiki1InfDetailsInputData inputData)
    {
        try
        {
            var result = _yousikiRepository.GetYousiki1InfDetails(inputData.HpId, inputData.SinYm, inputData.PtId, inputData.DataType, inputData.SeqNo);
            return new GetYousiki1InfDetailsOutputData(result, GetYousiki1InfDetailsStatus.Successed);
        }
        finally
        {
            _yousikiRepository.ReleaseResource();
        }
    }
}
