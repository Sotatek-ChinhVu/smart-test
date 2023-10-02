using Domain.Models.MainMenu;
using UseCase.MainMenu.SaveStaCsvMst;

namespace Interactor.MainMenu;

public class SaveStaCsvMstInteractor : ISaveStaCsvMstInputPort
{
    private readonly IStatisticRepository _statisticRepository;

    public SaveStaCsvMstInteractor(IStatisticRepository statisticRepository)
    {
        _statisticRepository = statisticRepository;
    }

    public SaveStaCsvMstOutputData Handle(SaveStaCsvMstInputData inputData)
    {
        try
        {
            foreach (var staCsvModel in inputData.StaCsvModels)
            {
                foreach (var item in staCsvModel.StaCsvModelsSelected)
                {
                    if (item.ConfName.Length > 100)
                    {
                        return new SaveStaCsvMstOutputData(SaveStaCsvMstStatus.InvalidConFName);
                    }
                    if (item.Columns.Length > 100)
                    {
                        return new SaveStaCsvMstOutputData(SaveStaCsvMstStatus.InvalidColumnName);
                    }
                }
            }

            if (inputData.HpId < 0)
            {
                return new SaveStaCsvMstOutputData(SaveStaCsvMstStatus.InvalidHpId);
            }

            if (inputData.UserId < 0)
            {
                return new SaveStaCsvMstOutputData(SaveStaCsvMstStatus.InvalidUserId);
            }

            _statisticRepository.SaveStaCsvMst(inputData.HpId, inputData.UserId, inputData.StaCsvModels);

            return new SaveStaCsvMstOutputData(SaveStaCsvMstStatus.Successed);
        }
        finally
        {
            _statisticRepository.ReleaseResource();
        }
    }
}
