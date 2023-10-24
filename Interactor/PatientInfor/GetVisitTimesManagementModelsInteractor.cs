using Domain.Models.PatientInfor;
using UseCase.PatientInfor.GetVisitTimesManagementModels;

namespace Interactor.PatientInfor;

public class GetVisitTimesManagementModelsInteractor : IGetVisitTimesManagementModelsInputPort
{
    private readonly IPatientInforRepository _patientInforRepository;

    public GetVisitTimesManagementModelsInteractor(IPatientInforRepository patientInforRepository)
    {
        _patientInforRepository = patientInforRepository;
    }

    public GetVisitTimesManagementModelsOutputData Handle(GetVisitTimesManagementModelsInputData inputData)
    {
        try
        {
            var result = _patientInforRepository.GetVisitTimesManagementModels(inputData.HpId, inputData.SinYm, inputData.PtId, inputData.KohiId);
            return new GetVisitTimesManagementModelsOutputData(result, GetVisitTimesManagementModelsStatus.Successed);
        }
        finally
        {
            _patientInforRepository.ReleaseResource();
        }
    }
}

