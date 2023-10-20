using Domain.Models.PatientInfor;
using Helper.Extension;
using UseCase.PatientInfor.UpdateVisitTimesManagement;

namespace Interactor.PatientInfor;

public class UpdateVisitTimesManagementInteractor : IUpdateVisitTimesManagementInputPort
{
    private readonly IPatientInforRepository _patientInforRepository;

    public UpdateVisitTimesManagementInteractor(IPatientInforRepository patientInforRepository)
    {
        _patientInforRepository = patientInforRepository;
    }

    public UpdateVisitTimesManagementOutputData Handle(UpdateVisitTimesManagementInputData inputData)
    {
        try
        {
            if (!_patientInforRepository.CheckExistIdList(new List<long>() { inputData.PtId }))
            {
                return new UpdateVisitTimesManagementOutputData(UpdateVisitTimesManagementStatus.InvalidPtId);
            }
            var visitTimeDBList = _patientInforRepository.GetVisitTimesManagementModels(inputData.HpId, inputData.SinYm, inputData.PtId, inputData.KohiId);
            if (inputData.VisitTimesManagementList.Any(item => item.IsDeleted && !item.IsOutHospital))
            {
                return new UpdateVisitTimesManagementOutputData(UpdateVisitTimesManagementStatus.CanNotDeleted);
            }
            else if (!inputData.VisitTimesManagementList.Any() && visitTimeDBList.Any(item => !item.IsOutHospital))
            {
                return new UpdateVisitTimesManagementOutputData(UpdateVisitTimesManagementStatus.CanNotDeleted);
            }
            if (_patientInforRepository.UpdateVisitTimesManagement(inputData.HpId, inputData.UserId, inputData.PtId, inputData.KohiId, inputData.VisitTimesManagementList))
            {
                return new UpdateVisitTimesManagementOutputData(UpdateVisitTimesManagementStatus.Successed);
            }
            return new UpdateVisitTimesManagementOutputData(UpdateVisitTimesManagementStatus.Failed);
        }
        finally
        {
            _patientInforRepository.ReleaseResource();
        }
    }
}