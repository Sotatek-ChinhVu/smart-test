using Domain.Models.Ka;
using UseCase.Ka.GetKaCodeList;
using UseCase.Ka.GetKacodeMstYossi;

namespace Interactor.Ka;

public class GetKaCodeMstYossiInteractor : IGetKacodeMstYossiInputPort
{
    private readonly IKaRepository _kaMstRepository;

    public GetKaCodeMstYossiInteractor(IKaRepository kaMstRepository)
    {
        _kaMstRepository = kaMstRepository;
    }

    public GetKacodeMstYossiOutputData Handle(GetKacodeMstYossiInputData inputData)
    {
        try
        {
            var departments = _kaMstRepository.GetListKacode();
            var status = departments.Any() ? GetKacodeMstYossiStatus.Success : GetKacodeMstYossiStatus.NoData;
            return new GetKacodeMstYossiOutputData(status, departments);
        }
        catch (Exception)
        {
            return new GetKacodeMstYossiOutputData(GetKacodeMstYossiStatus.Failed, new List<KaCodeMstModel>());
        }
        finally
        {
            _kaMstRepository.ReleaseResource();
        }
    }
}
