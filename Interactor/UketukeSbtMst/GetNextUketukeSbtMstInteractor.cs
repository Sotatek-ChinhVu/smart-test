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
        if (input.SinDate <= 0)
        {
            return new GetNextUketukeSbtMstOutputData(GetNextUketukeSbtMstStatus.InvalidSinDate);
        }

        var receptionType = GetNextReceptionType(input.SinDate, input.KbnId);
        var status = receptionType is null ? GetNextUketukeSbtMstStatus.NotFound : GetNextUketukeSbtMstStatus.Success;
        return new GetNextUketukeSbtMstOutputData(status, receptionType);
    }

    public UketukeSbtMstModel? GetNextReceptionType(int sinDate, int kbnId)
    {
        var uketukeSbtMsts = _uketukeSbtMstRepository.GetList().OrderBy(u => u.SortNo).ToList();
        if (uketukeSbtMsts.Count == 0)
        {
            return null;
        }

        var currentMst = uketukeSbtMsts.FirstOrDefault(u => u.KbnId == kbnId);
        var currentSortNo = currentMst?.SortNo ?? -1;
        var nextMst = uketukeSbtMsts.FirstOrDefault(u => u.SortNo > currentSortNo) ?? uketukeSbtMsts.First();

        _uketukeSbtDayInfRepository.Upsert(sinDate, nextMst.KbnId, 0);

        return nextMst;
    }
}
