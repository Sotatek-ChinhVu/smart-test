using Domain.Models.Online;
using Domain.Models.PatientInfor;
using UseCase.Online.GetListOnlineConfirmationHistoryModel;

namespace Interactor.Online;

public class GetListOnlineConfirmationHistoryModelInteractor : IGetListOnlineConfirmationHistoryModelInputPort
{
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IOnlineRepository _onlineRepository;

    public GetListOnlineConfirmationHistoryModelInteractor(IPatientInforRepository patientInforRepository, IOnlineRepository onlineRepository)
    {
        _patientInforRepository = patientInforRepository;
        _onlineRepository = onlineRepository;
    }

    public GetListOnlineConfirmationHistoryModelOutputData Handle(GetListOnlineConfirmationHistoryModelInputData inputData)
    {
        List<OnlineConfirmationHistoryModel> result;
        if (inputData.PtId > 0 && !inputData.OnlQuaResFileDict.Any() && !inputData.OnlQuaConfirmationTypeDict.Any() && !_patientInforRepository.CheckExistIdList(new List<long>() { inputData.PtId }))
        {
            return new GetListOnlineConfirmationHistoryModelOutputData(new(), GetListOnlineConfirmationHistoryModelStatus.InvalidPtId);
        }
        if (inputData.PtId > 0)
        {
            result = _onlineRepository.GetListOnlineConfirmationHistoryModel(inputData.PtId);
        }
        else
        {
            result = _onlineRepository.GetListOnlineConfirmationHistoryModel(inputData.UserId, inputData.OnlQuaResFileDict, inputData.OnlQuaConfirmationTypeDict);
        }
        return new GetListOnlineConfirmationHistoryModelOutputData(result, GetListOnlineConfirmationHistoryModelStatus.Successed);
    }
}
