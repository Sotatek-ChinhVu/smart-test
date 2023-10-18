using Domain.Models.PatientInfor;
using UseCase.PatientInfor.UpdateVisitTimesManagementNeedSave;

namespace Interactor.PatientInfor;

public class UpdateVisitTimesManagementNeedSaveInteractor : IUpdateVisitTimesManagementNeedSaveInputPort
{
    private readonly IPatientInforRepository _patientInforRepository;

    public UpdateVisitTimesManagementNeedSaveInteractor(IPatientInforRepository patientInforRepository)
    {
        _patientInforRepository = patientInforRepository;
    }

    public UpdateVisitTimesManagementNeedSaveOutputData Handle(UpdateVisitTimesManagementNeedSaveInputData inputData)
    {
        try
        {
            if (!_patientInforRepository.CheckExistIdList(new List<long>() { inputData.PtId }))
            {
                return new UpdateVisitTimesManagementNeedSaveOutputData(UpdateVisitTimesManagementNeedSaveStatus.InvalidPtId);
            }
            if (_patientInforRepository.UpdateVisitTimesManagementNeedSave(inputData.HpId, inputData.UserId, inputData.PtId, inputData.VisitTimesManagementList))
            {
                return new UpdateVisitTimesManagementNeedSaveOutputData(UpdateVisitTimesManagementNeedSaveStatus.Successed);
            }
            return new UpdateVisitTimesManagementNeedSaveOutputData(UpdateVisitTimesManagementNeedSaveStatus.Failed);
        }
        finally
        {
            _patientInforRepository.ReleaseResource();
        }
    }
}
