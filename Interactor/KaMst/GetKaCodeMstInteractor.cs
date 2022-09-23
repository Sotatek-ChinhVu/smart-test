using Domain.Models.KaMst;
using UseCase.KaMst.GetKaCodeList;

namespace Interactor.KaMst;

public class GetKaCodeMstInteractor : IGetKaCodeMstListInputPort
{
    private readonly IKaMstRepository _kaMstRepository;

    public GetKaCodeMstInteractor(IKaMstRepository kaMstRepository)
    {
        _kaMstRepository = kaMstRepository;
    }

    public GetKaCodeMstListOutputData Handle(GetKaCodeMstInputData inputData)
    {
        var departments = _kaMstRepository.GetListKacode();
        var status = departments.Any() ? GetKaCodeMstListStatus.Success : GetKaCodeMstListStatus.NoData;
        return new GetKaCodeMstListOutputData(status, departments);
    }
}
