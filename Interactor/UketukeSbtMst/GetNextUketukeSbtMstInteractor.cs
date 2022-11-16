using Domain.Models.UketukeSbtDayInf;
using Domain.Models.UketukeSbtMst;
using UseCase.UketukeSbtMst.GetNext;

namespace Interactor.UketukeSbtMst;

public class GetNextUketukeSbtMstInteractor : IGetNextUketukeSbtMstInputPort
{
    private readonly IUketukeSbtMstRepository _uketukeSbtMstRepository;
    private readonly IUketukeSbtDayInfRepository _uketukeSbtDayInfRepository;

    public GetNextUketukeSbtMstInteractor(IUketukeSbtMstRepository uketukeSbtMstRepository,
        IUketukeSbtDayInfRepository uketukeSbtDayInfRepository)
    {
        _uketukeSbtMstRepository = uketukeSbtMstRepository;
        _uketukeSbtDayInfRepository = uketukeSbtDayInfRepository;
    }

    public GetNextUketukeSbtMstOutputData Handle(GetNextUketukeSbtMstInputData input)
    {
        var nextReceptionType = GetNextReceptionType(input.CurrentKbnId);
        var status = GetNextUketukeSbtMstStatus.NotFound;
        if (nextReceptionType is not null)
        {
            status = GetNextUketukeSbtMstStatus.Success;
            _uketukeSbtDayInfRepository.Upsert(input.SinDate, nextReceptionType.KbnId, 0, input.UserId);
        }

        return new GetNextUketukeSbtMstOutputData(status, nextReceptionType);
    }

    private UketukeSbtMstModel? GetNextReceptionType(int currentKbnId)
    {
        var uketukeSbtMsts = _uketukeSbtMstRepository.GetList().OrderBy(u => u.SortNo).ToList();
        if (uketukeSbtMsts.Count == 0)
        {
            return null;
        }

        var currentMst = uketukeSbtMsts.FirstOrDefault(u => u.KbnId == currentKbnId);
        var currentSortNo = currentMst?.SortNo ?? -1;
        var nextMst = uketukeSbtMsts.FirstOrDefault(u => u.SortNo > currentSortNo) ?? uketukeSbtMsts.First();

        return nextMst;
    }
}
