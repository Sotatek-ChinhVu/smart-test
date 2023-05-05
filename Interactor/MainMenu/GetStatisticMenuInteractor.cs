using Domain.Models.MainMenu;
using UseCase.MainMenu;
using UseCase.MainMenu.GetStatisticMenu;

namespace Interactor.MainMenu;

public class GetStatisticMenuInteractor : IGetStatisticMenuInputPort
{
    private readonly IStatisticRepository _statisticRepository;

    public GetStatisticMenuInteractor(IStatisticRepository statisticRepository)
    {
        _statisticRepository = statisticRepository;
    }

    public GetStatisticMenuOutputData Handle(GetStatisticMenuInputData inputData)
    {
        try
        {
            var grpList = _statisticRepository.GetStaGrp(inputData.HpId, inputData.GrpId)
                                              .Select(grp => new StaGrpItem(grp))
                                              .ToList();

            var result = _statisticRepository.GetStatisticMenu(inputData.HpId, inputData.GrpId)
                                             .Select(item => new StatisticMenuItem(item))
                                             .ToList();

            return new GetStatisticMenuOutputData(result, grpList, GetStatisticMenuStatus.Successed);
        }
        finally
        {
            _statisticRepository.ReleaseResource();
        }
    }
}
