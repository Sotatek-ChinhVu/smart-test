using Domain.Models.Ka;
using UseCase.Ka.GetKaCodeList;

namespace Interactor.Ka;

public class GetKaCodeMstInteractor : IGetKaCodeMstListInputPort
{
    private readonly IKaRepository _kaMstRepository;

    public GetKaCodeMstInteractor(IKaRepository kaMstRepository)
    {
        _kaMstRepository = kaMstRepository;
    }

    public GetKaCodeMstListOutputData Handle(GetKaCodeMstInputData inputData)
    {
        try
        {
            var departments = _kaMstRepository.GetListKacode();
            var status = departments.Any() ? GetKaCodeMstListStatus.Success : GetKaCodeMstListStatus.NoData;
            return new GetKaCodeMstListOutputData(status, departments);
        }
        catch (Exception)
        {
            return new GetKaCodeMstListOutputData(GetKaCodeMstListStatus.Failed, new List<KaCodeMstModel>());
        }
    }
}
