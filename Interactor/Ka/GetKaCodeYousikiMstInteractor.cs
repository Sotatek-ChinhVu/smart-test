using Domain.Models.Ka;
using UseCase.Ka.GetKacodeMstYossi;
using UseCase.Ka.GetKacodeYousikiMst;

namespace Interactor.Ka;

public class GetKaCodeYousikiMstInteractor : IGetKaCodeYousikiMstInputPort
{
    private readonly IKaRepository _kaMstRepository;

    public GetKaCodeYousikiMstInteractor(IKaRepository kaMstRepository)
    {
        _kaMstRepository = kaMstRepository;
    }

    public GetKaCodeYousikiMstOutputData Handle(GetKaCodeYousikiMstInputData inputData)
    {
        try
        {
            var result = _kaMstRepository.GetKacodeYousikiMst(inputData.HpId);
            var status = result.Any() ? GetKaCodeYousikiMstStatus.Success : GetKaCodeYousikiMstStatus.NoData;
            return new GetKaCodeYousikiMstOutputData(status, result);
        }
        finally
        {
            _kaMstRepository.ReleaseResource();
        }
    }
}

