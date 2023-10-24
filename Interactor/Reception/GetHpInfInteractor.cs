using Domain.Models.HpInf;
using UseCase.Reception.GetHpInf;

namespace Interactor.Reception;

public class GetHpInfInteractor : IGetHpInfInputPort
{
    private readonly IHpInfRepository _hpInfRepository;

    public GetHpInfInteractor(IHpInfRepository hpInfRepository)
    {
        _hpInfRepository = hpInfRepository;
    }

    public GetHpInfOutputData Handle(GetHpInfInputData inputData)
    {
        try
        {
            var hpInfList = _hpInfRepository.GetListHpInf(inputData.HpId);
            var hpInf = hpInfList.FirstOrDefault(item => item.StartDate <= inputData.SinDate);
            if (hpInf == null)
            {
                hpInf = hpInfList.OrderByDescending(item => item.StartDate).FirstOrDefault() ?? new();
            }
            return new GetHpInfOutputData(hpInf, GetHpInfStatus.Successed);
        }
        finally
        {
            _hpInfRepository.ReleaseResource();
        }
    }
}
