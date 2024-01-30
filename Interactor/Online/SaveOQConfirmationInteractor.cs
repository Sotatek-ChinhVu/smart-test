using Domain.Models.Online;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using System.Xml;
using UseCase.Online.SaveOQConfirmation;

namespace Interactor.Online;

public class SaveOQConfirmationInteractor : ISaveOQConfirmationInputPort
{
    private readonly IOnlineRepository _onlineRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly ILoggingHandler _loggingHandler;
    private readonly ITenantProvider _tenantProvider;
    private readonly IReceptionRepository _receptionRepository;

    public SaveOQConfirmationInteractor(ITenantProvider tenantProvider, IOnlineRepository onlineRepository, IPatientInforRepository patientInforRepository, IReceptionRepository receptionRepository)
    {
        _onlineRepository = onlineRepository;
        _patientInforRepository = patientInforRepository;
        _tenantProvider = tenantProvider;
        _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        _receptionRepository = receptionRepository;
    }

    public SaveOQConfirmationOutputData Handle(SaveOQConfirmationInputData inputData)
    {
        try
        {
            var validateResult = ValidateData(inputData);
            if (validateResult != SaveOQConfirmationStatus.ValidateSuccessed)
            {
                return new SaveOQConfirmationOutputData(validateResult);
            }
            if (_onlineRepository.SaveOQConfirmation(inputData.HpId, inputData.UserId, inputData.OnlineHistoryId, inputData.PtId, inputData.ConfirmationResult, inputData.OnlineConfirmationDateString, inputData.ConfirmationType, inputData.InfConsFlg, inputData.UketukeStatus, inputData.IsUpdateRaiinInf))
            {
                var sinDate = inputData.OnlineConfirmationDateString.Substring(0, 8).AsInteger();
                var receptionInfos = _receptionRepository.GetList(inputData.HpId, sinDate, CommonConstants.InvalidId, inputData.PtId, isDeleted: 0);
                return new SaveOQConfirmationOutputData(SaveOQConfirmationStatus.Successed, receptionInfos);
            }
            return new SaveOQConfirmationOutputData(SaveOQConfirmationStatus.Failed);
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

    private SaveOQConfirmationStatus ValidateData(SaveOQConfirmationInputData inputData)
    {
        if (!_patientInforRepository.CheckExistIdList(new List<long>() { inputData.PtId }))
        {
            return SaveOQConfirmationStatus.InvalidPtId;
        }
        else if (inputData.OnlineHistoryId != 0 && !_onlineRepository.CheckExistIdList(new List<long>() { inputData.OnlineHistoryId }))
        {
            return SaveOQConfirmationStatus.InvalidPtId;
        }
        else if (string.IsNullOrEmpty(inputData.ConfirmationResult))
        {
            return SaveOQConfirmationStatus.InvalidConfirmationResult;
        }
        try
        {
            XmlDocument xmlDoc = new();
            xmlDoc.LoadXml(inputData.ConfirmationResult);
        }
        catch
        {
            return SaveOQConfirmationStatus.InvalidConfirmationResult;
        }
        return SaveOQConfirmationStatus.ValidateSuccessed;
    }
}
