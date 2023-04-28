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
            var result = _statisticRepository.GetDailyStatisticMenu(inputData.HpId, inputData.MenuId)
                                             .Select(item => new StatisticMenuItem(item))
                                             .ToList();
            return new GetDailyStatisticMenuOutputData(result, GetDailyStatisticMenuStatus.Successed);
        }
        finally
        {
            _statisticRepository.ReleaseResource();
        }
    }
}
