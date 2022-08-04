using Domain.Models.UketukeSbtMst;
using UseCase.UketukeSbtMst.GetNext;

namespace Interactor.UketukeSbtMst;

public class GetNextUketukeSbtMstInteractor : IGetNextUketukeSbtMstInputPort
{
    private readonly IUketukeSbtMstRepository _uketukeSbtMstRepository;

    public GetNextUketukeSbtMstInteractor(IUketukeSbtMstRepository uketukeSbtMstRepository)
    {
        _uketukeSbtMstRepository = uketukeSbtMstRepository;
    }

    public GetNextUketukeSbtMstOutputData Handle(GetNextUketukeSbtMstInputData input)
    {
        var receptionType = GetNextReceptionType(input.KbnId);
        var status = receptionType is null ? GetNextUketukeSbtMstStatus.NotFound : GetNextUketukeSbtMstStatus.Success;
        return new GetNextUketukeSbtMstOutputData(status, receptionType);
    }

    private UketukeSbtMstModel? GetNextReceptionType(int kbnId)
    {
        var uketukeSbtMsts = _uketukeSbtMstRepository.GetList().OrderBy(u => u.SortNo).ToList();
        if (uketukeSbtMsts.Count == 0)
        {
            return null;
        }

        var currentMst = uketukeSbtMsts.FirstOrDefault(u => u.KbnId == kbnId);
        var currentSortNo = currentMst?.SortNo ?? -1;
        var nextMst = uketukeSbtMsts.FirstOrDefault(u => u.SortNo > currentSortNo) ?? uketukeSbtMsts.First();

        return nextMst;
    }
}
