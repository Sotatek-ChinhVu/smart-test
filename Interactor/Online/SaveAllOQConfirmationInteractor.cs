using Domain.Models.Online;
using Domain.Models.PatientInfor;
using UseCase.Online.SaveAllOQConfirmation;

namespace Interactor.Online;

public class SaveAllOQConfirmationInteractor : ISaveAllOQConfirmationInputPort
{
    private readonly IOnlineRepository _onlineRepository;
    private readonly IPatientInforRepository _patientInforRepository;

    public SaveAllOQConfirmationInteractor(IOnlineRepository onlineRepository, IPatientInforRepository patientInforRepository)
    {
        _onlineRepository = onlineRepository;
        _patientInforRepository = patientInforRepository;
    }

    public SaveAllOQConfirmationOutputData Handle(SaveAllOQConfirmationInputData inputData)
    {
        try
        {
            if (inputData.PtId != 0 && !_patientInforRepository.CheckExistIdList(new List<long>() { inputData.PtId }))
            {
                return new SaveAllOQConfirmationOutputData(SaveAllOQConfirmationStatus.InvalidPtId);
            }
            else if (_onlineRepository.SaveAllOQConfirmation(inputData.HpId, inputData.UserId, inputData.PtId, inputData.OnlQuaResFileDict, inputData.OnlQuaConfirmationTypeDict))
            {
                return new SaveAllOQConfirmationOutputData(SaveAllOQConfirmationStatus.Successed);
            }
            return new SaveAllOQConfirmationOutputData(SaveAllOQConfirmationStatus.Failed);
        }
        finally
        {
            _onlineRepository.ReleaseResource();
        }
    }
}
