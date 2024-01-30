using Domain.Models.Yousiki;
using Infrastructure.Interfaces;
using UseCase.Yousiki.GetYousiki1InfDetails;

namespace Interactor.Yousiki;

public class GetYousiki1InfDetailsInteractor : IGetYousiki1InfDetailsInputPort
{
    private readonly IYousikiRepository _yousikiRepository;
    private readonly IReturnYousikiTabService _returnYousikiTabService;

    public GetYousiki1InfDetailsInteractor(IYousikiRepository yousikiRepository, IReturnYousikiTabService returnYousikiTabService)
    {
        _yousikiRepository = yousikiRepository;
        _returnYousikiTabService = returnYousikiTabService;
    }

    public GetYousiki1InfDetailsOutputData Handle(GetYousiki1InfDetailsInputData inputData)
    {
        try
        {
            var result = _yousikiRepository.GetYousiki1InfDetails(inputData.HpId, inputData.SinYm, inputData.PtId, inputData.DataType, inputData.SeqNo);
            var kacodeYousikiMstDict = _yousikiRepository.GetKacodeYousikiMstDict(inputData.HpId);
            var tabYousiki = _returnYousikiTabService.RenderTabYousiki(result, kacodeYousikiMstDict);
            return new GetYousiki1InfDetailsOutputData(result, tabYousiki, GetYousiki1InfDetailsStatus.Successed);
        }
        finally
        {
            _yousikiRepository.ReleaseResource();
        }
    }
}
