using Domain.Models.Online;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using Helper.Common;
using Helper.Constants;
using System.Globalization;
using UseCase.Online.UpdateOnlineInRaiinInf;

namespace Interactor.Online;

public class UpdateOnlineInRaiinInfInteractor : IUpdateOnlineInRaiinInfInputPort
{
    private readonly IOnlineRepository _onlineRepository;
    private readonly IReceptionRepository _receptionRepository;
    private readonly IPatientInforRepository _patientInforRepository;

    public UpdateOnlineInRaiinInfInteractor(IOnlineRepository onlineRepository, IPatientInforRepository patientInforRepository, IReceptionRepository receptionRepository)
    {
        _onlineRepository = onlineRepository;
        _patientInforRepository = patientInforRepository;
        _receptionRepository = receptionRepository;
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
                int sindate = CIUtil.DateTimeToInt(confirmationDate);
                var receptionInfos = _receptionRepository.GetList(inputData.HpId, sindate, CommonConstants.InvalidId, inputData.PtId, isDeleted: 0);
                return new UpdateOnlineInRaiinInfOutputData(UpdateOnlineInRaiinInfStatus.Successed, receptionInfos);
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
