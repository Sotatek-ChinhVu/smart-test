using Domain.Models.Online;
using Domain.Models.PatientInfor;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.Online.SaveAllOQConfirmation;

namespace Interactor.Online;

public class SaveAllOQConfirmationInteractor : ISaveAllOQConfirmationInputPort
{
    private readonly IOnlineRepository _onlineRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly ILoggingHandler _loggingHandler;
    private readonly ITenantProvider _tenantProvider;

    public SaveAllOQConfirmationInteractor(ITenantProvider tenantProvider, IOnlineRepository onlineRepository, IPatientInforRepository patientInforRepository)
    {
        _onlineRepository = onlineRepository;
        _patientInforRepository = patientInforRepository;
        _tenantProvider = tenantProvider;
        _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
    }

    public SaveAllOQConfirmationOutputData Handle(SaveAllOQConfirmationInputData inputData)
    {
        try
        {
            if (inputData.PtId != 0 && !_patientInforRepository.CheckExistIdList(inputData.HpId, new List<long>() { inputData.PtId }))
            {
                return new SaveAllOQConfirmationOutputData(SaveAllOQConfirmationStatus.InvalidPtId);
            }
            else if (_onlineRepository.SaveAllOQConfirmation(inputData.HpId, inputData.UserId, inputData.PtId, inputData.OnlQuaResFileDict, inputData.OnlQuaConfirmationTypeDict))
            {
                return new SaveAllOQConfirmationOutputData(SaveAllOQConfirmationStatus.Successed);
            }
            return new SaveAllOQConfirmationOutputData(SaveAllOQConfirmationStatus.Failed);
        }
        catch (Exception ex)
        {
            _loggingHandler.WriteLogExceptionAsync(ex);
            throw;
        }
        finally
        {
            _onlineRepository.ReleaseResource();
            _loggingHandler.Dispose();
        }
    }
}
