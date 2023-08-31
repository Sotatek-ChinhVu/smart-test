using Domain.Models.Online;
using Domain.Models.PatientInfor;
using System.Globalization;
using UseCase.Online.UpdateOnlineInRaiinInf;

namespace Interactor.Online;

public class UpdateOnlineInRaiinInfInteractor : IUpdateOnlineInRaiinInfInputPort
{
    private readonly IOnlineRepository _onlineRepository;
    private readonly IPatientInforRepository _patientInforRepository;

    public UpdateOnlineInRaiinInfInteractor(IOnlineRepository onlineRepository, IPatientInforRepository patientInforRepository)
    {
        _onlineRepository = onlineRepository;
        _patientInforRepository = patientInforRepository;
    }
    public UpdateOnlineInRaiinInfOutputData Handle(UpdateOnlineInRaiinInfInputData inputData)
    {
        try
        {
            var  resultValidate = ValidateData(inputData);
            if (resultValidate!=UpdateOnlineInRaiinInfStatus.ValidateSuccess)
            {
                return new UpdateOnlineInRaiinInfOutputData(resultValidate);
            }
            DateTime confirmationDate = TimeZoneInfo.ConvertTimeToUtc(DateTime.ParseExact(inputData.OnlineConfirmationDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture)); ;
            if(_onlineRepository.UpdateOnlineInRaiinInf(inputData.HpId, inputData.UserId, inputData.PtId, confirmationDate, inputData.ConfirmationType, inputData.InfConsFlg))
            {
                return new UpdateOnlineInRaiinInfOutputData(UpdateOnlineInRaiinInfStatus.Successed);
            }
            return new UpdateOnlineInRaiinInfOutputData(UpdateOnlineInRaiinInfStatus.Failed);
        }
        finally
        {
            _onlineRepository.ReleaseResource();
            _patientInforRepository.ReleaseResource();
        }
    }

    private UpdateOnlineInRaiinInfStatus ValidateData(UpdateOnlineInRaiinInfInputData inputData)
    {
        if (inputData.PtId != 0 && inputData.PtId != -1 && !_patientInforRepository.CheckExistIdList(new List<long>() { inputData.PtId }))
        {
            return UpdateOnlineInRaiinInfStatus.InvalidPtId;
        }
        return UpdateOnlineInRaiinInfStatus.ValidateSuccess;
    }
}
