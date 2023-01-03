using Domain.Models.UketukeSbtMst;
using UseCase.UketukeSbtMst.GetList;

namespace Interactor.UketukeSbtMst;

public class GetUketukeSbtMstListInteractor : IGetUketukeSbtMstListInputPort
{
    private readonly IUketukeSbtMstRepository _uketukeSbtMstRepository;

    public GetUketukeSbtMstListInteractor(IUketukeSbtMstRepository uketukeSbtMstRepository)
    {
        _uketukeSbtMstRepository = uketukeSbtMstRepository;
    }

    public GetUketukeSbtMstListOutputData Handle(GetUketukeSbtMstListInputData input)
    {
        try
        {
            var receptionTypes = _uketukeSbtMstRepository.GetList();
            var status = receptionTypes.Any() ? GetUketukeSbtMstListStatus.Success : GetUketukeSbtMstListStatus.NoData;
            return new GetUketukeSbtMstListOutputData(status, receptionTypes);
        }
        finally
        {
            _uketukeSbtMstRepository.ReleaseResource();
        }
    }
}
