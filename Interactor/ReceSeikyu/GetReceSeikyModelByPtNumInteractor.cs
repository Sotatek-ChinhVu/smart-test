using Domain.Models.ReceSeikyu;
using UseCase.ReceSeikyu.GetReceSeikyModelByPtNum;

namespace Interactor.ReceSeikyu;

public class GetReceSeikyModelByPtNumInteractor : IGetReceSeikyModelByPtNumInputPort
{
    private readonly IReceSeikyuRepository _receSeikyuRepository;

    public GetReceSeikyModelByPtNumInteractor(IReceSeikyuRepository receSeikyuRepository)
    {
        _receSeikyuRepository = receSeikyuRepository;
    }

    public GetReceSeikyModelByPtNumOutputData Handle(GetReceSeikyModelByPtNumInputData inputData)
    {
        try
        {
            var result = _receSeikyuRepository.GetReceSeikyModelByPtNum(inputData.HpId, inputData.SinDate, inputData.SinYm, inputData.PtNum);
            return new GetReceSeikyModelByPtNumOutputData(GetReceSeikyModelByPtNumStatus.Successed, result);
        }
        finally
        {
            _receSeikyuRepository.ReleaseResource();
        }
    }
}
