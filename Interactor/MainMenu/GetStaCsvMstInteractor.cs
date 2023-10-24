using Domain.Models.MainMenu;
using UseCase.MainMenu.GetStaCsvMstModel;

namespace Interactor.MainMenu;

public class GetStaCsvMstInteractor : IGetStaCsvMstInputPort
{
    private readonly IStatisticRepository _statisticRepository;

    public GetStaCsvMstInteractor(IStatisticRepository statisticRepository)
    {
        _statisticRepository = statisticRepository;
    }

    public GetStaCsvMstOutputData Handle(GetStaCsvMstInputData inputData)
    {
        try
        {
            var result = _statisticRepository.GetStaCsvMstModels(inputData.HpId);

            return new GetStaCsvMstOutputData(result, GetStaCsvMstStatus.Successed);
        }
        finally
        {
            _statisticRepository.ReleaseResource();
        }
    }
}
