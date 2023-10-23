using Domain.Models.MainMenu;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.MainMenu.SaveStaCsvMst;

namespace Interactor.MainMenu;

public class SaveStaCsvMstInteractor : ISaveStaCsvMstInputPort
{
    private readonly IStatisticRepository _statisticRepository;
    private readonly ILoggingHandler _loggingHandler;
    private readonly ITenantProvider _tenantProvider;

    public SaveStaCsvMstInteractor(ITenantProvider tenantProvider, IStatisticRepository statisticRepository)
    {
        _statisticRepository = statisticRepository;
        _tenantProvider = tenantProvider;
        _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
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
        catch (Exception ex)
        {
            _loggingHandler.WriteLogExceptionAsync(ex);
            throw;
        }
        finally
        {
            _statisticRepository.ReleaseResource();
            _loggingHandler.Dispose();
        }
    }
}
