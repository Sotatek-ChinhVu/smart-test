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
            var ptIdList = yousikiInfList.Select(item => item.PtId).Distinct().ToList();
            var yousikiInfDetailList = _yousikiRepository.GetYousiki1InfDetails(inputData.HpId, inputData.SinYm, ptIdList);
            foreach (var yousikiInf in yousikiInfList)
            {
                var yousikiInfDetailItemList = yousikiInfDetailList.Where(item => item.PtId == yousikiInf.PtId && item.SeqNo == yousikiInf.SeqNo).ToList();
                yousikiInf.ChangeYousiki1InfDetailList(yousikiInfDetailItemList);
            }
            return new GetYousiki1InfModelWithCommonInfOutputData(yousikiInfList, GetYousiki1InfModelWithCommonInfStatus.Successed);
        }
        finally
        {
            _yousikiRepository.ReleaseResource();
        }
    }
}
