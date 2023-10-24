using Domain.Models.Online;
using Domain.Models.PatientInfor;
using UseCase.Online.UpdateOnlineHistoryById;

namespace Interactor.Online;

public class UpdateOnlineHistoryByIdInteractor : IUpdateOnlineHistoryByIdInputPort
{
    private readonly IOnlineRepository _onlineRepository;
    private readonly IPatientInforRepository _patientInforRepository;

    public UpdateOnlineHistoryByIdInteractor(IOnlineRepository onlineRepository, IPatientInforRepository patientInforRepository)
    {
        _onlineRepository = onlineRepository;
        _patientInforRepository = patientInforRepository;
    }

    public UpdateOnlineHistoryByIdOutputData Handle(UpdateOnlineHistoryByIdInputData inputData)
    {
        try
        {
            var resultValidate = ValidateData(inputData);
            if (resultValidate != UpdateOnlineHistoryByIdStatus.ValidateSuccessed)
            {
                return new UpdateOnlineHistoryByIdOutputData(resultValidate);
            }
            if (_onlineRepository.UpdateOnlineHistoryById(inputData.UserId, inputData.Id, inputData.PtId, inputData.UketukeStatus, inputData.ConfirmationType))
            {
                return new UpdateOnlineHistoryByIdOutputData(UpdateOnlineHistoryByIdStatus.Successed);
            }
            return new UpdateOnlineHistoryByIdOutputData(UpdateOnlineHistoryByIdStatus.Failed);
        }
        finally
        {
            _onlineRepository.ReleaseResource();
            _patientInforRepository.ReleaseResource();
        }
    }

    private UpdateOnlineHistoryByIdStatus ValidateData(UpdateOnlineHistoryByIdInputData inputData)
    {
        if (inputData.PtId != 0 && inputData.PtId != -1 && !_patientInforRepository.CheckExistIdList(new List<long>() { inputData.PtId }))
        {
            return UpdateOnlineHistoryByIdStatus.InvalidPtId;
        }
        else if (!_onlineRepository.CheckExistIdList(new List<long>() { inputData.Id }))
        {
            return UpdateOnlineHistoryByIdStatus.InvalidId;
        }
        else if (!new List<int> { -1, 0, 1, 2, 9 }.Contains(inputData.UketukeStatus))
        {
            return UpdateOnlineHistoryByIdStatus.InvalidUketukeStatus;
        }
        return UpdateOnlineHistoryByIdStatus.ValidateSuccessed;
    }
}
