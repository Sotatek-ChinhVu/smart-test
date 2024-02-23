using Domain.Models.Online;
using Domain.Models.PatientInfor;
using UseCase.Online.UpdateRefNo;

namespace Interactor.Online;

public class UpdateRefNoInteractor : IUpdateRefNoInputPort
{
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IOnlineRepository _onlineRepository;

    public UpdateRefNoInteractor(IOnlineRepository onlineRepository, IPatientInforRepository patientInforRepository)
    {
        _onlineRepository = onlineRepository;
        _patientInforRepository = patientInforRepository;
    }

    public UpdateRefNoOutputData Handle(UpdateRefNoInputData inputData)
    {
        try
        {
            if (!_patientInforRepository.CheckExistIdList(inputData.HpId, new List<long> { inputData.PtId }))
            {
                return new UpdateRefNoOutputData(0, UpdateRefNoStatus.InvalidPtId);
            }
            var nextRefNo = _onlineRepository.UpdateRefNo(inputData.HpId, inputData.PtId);
            if (nextRefNo == 0)
            {
                return new UpdateRefNoOutputData(0, UpdateRefNoStatus.Failed);
            }
            return new UpdateRefNoOutputData(nextRefNo, UpdateRefNoStatus.Successed);
        }
        finally
        {
            _onlineRepository.ReleaseResource();
        }
    }
}
