using Domain.Models.MainMenu;
using UseCase.MainMenu;
using UseCase.MainMenu.GetDailyStatisticMenu;

namespace Interactor.MainMenu;

public class GetDailyStatisticMenuInteractor : IGetDailyStatisticMenuInputPort
{
    private readonly IStatisticRepository _statisticRepository;

    public GetDailyStatisticMenuInteractor(IStatisticRepository statisticRepository)
    {
        _statisticRepository = statisticRepository;
    }

    public GetDailyStatisticMenuOutputData Handle(GetDailyStatisticMenuInputData inputData)
    {
        try
        {
            var grpList = _statisticRepository.GetStaGrp(inputData.HpId, inputData.GrpId)
                                              .Select(grp => new StaGrpItem(grp))
                                              .ToList();

            var result = _statisticRepository.GetStatisticMenu(inputData.HpId, inputData.GrpId)
                                             .Select(item => new StatisticMenuItem(item))
                                             .ToList();

            return new GetDailyStatisticMenuOutputData(result, grpList, GetDailyStatisticMenuStatus.Successed);
        }
        finally
        {
            _statisticRepository.ReleaseResource();
        }
    }
}
