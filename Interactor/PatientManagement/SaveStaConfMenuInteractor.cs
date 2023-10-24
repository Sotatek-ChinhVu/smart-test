using Domain.Models.MainMenu;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.PatientManagement.SaveStaConf;

namespace Interactor.PatientManagement
{
    public class SaveStaConfMenuInteractor : ISaveStaConfMenuInputPort
    {
        private readonly IStatisticRepository _statisticRepository;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;

        public SaveStaConfMenuInteractor(ITenantProvider tenantProvider, IStatisticRepository statisticRepository)
        {
            _statisticRepository = statisticRepository;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public SaveStaConfMenuOutputData Handle(SaveStaConfMenuInputData inputData)
        {
            try
            {
                var result = _statisticRepository.SaveStaConfMenu(inputData.HpId, inputData.UserId, inputData.StatisticMenu);

                return new SaveStaConfMenuOutputData(result ? SaveStaConfMenuStatus.Successed : SaveStaConfMenuStatus.Failed);
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
}
