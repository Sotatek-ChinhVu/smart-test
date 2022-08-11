using Domain.Models.KaMst;
using UseCase.KaMst.GetList;

namespace Interactor.KaMst;

public class GetKaMstListInteractor : IGetKaMstListInputPort
{
    private readonly IKaMstRepository _kaMstRepository;

    public GetKaMstListInteractor(IKaMstRepository kaMstRepository)
    {
        _kaMstRepository = kaMstRepository;
    }

    public GetKaMstListOutputData Handle(GetKaMstListInputData input)
    {
        var departments = _kaMstRepository.GetList();
        var status = departments.Any() ? GetKaMstListStatus.Success : GetKaMstListStatus.NoData;
        return new GetKaMstListOutputData(status, departments);
    }
}
