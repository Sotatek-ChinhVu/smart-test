using Domain.Models.Ka;
using UseCase.Ka.GetList;

namespace Interactor.Ka;

public class GetKaMstListInteractor : IGetKaMstListInputPort
{
    private readonly IKaRepository _kaMstRepository;

    public GetKaMstListInteractor(IKaRepository kaMstRepository)
    {
        _kaMstRepository = kaMstRepository;
    }

    public GetKaMstListOutputData Handle(GetKaMstListInputData input)
    {
        try
        {
            var departments = _kaMstRepository.GetList();
            var status = departments.Any() ? GetKaMstListStatus.Success : GetKaMstListStatus.NoData;
            return new GetKaMstListOutputData(status, departments);
        }
        catch (Exception)
        {
            return new GetKaMstListOutputData(GetKaMstListStatus.Failed, new List<KaMstModel>());
        }
        finally
        {
            _kaMstRepository.ReleaseResource();
        }
    }
}
