using Domain.Models.PatientInfor;
using UseCase.PatientInfor.GetPtInfModelsByName;

namespace Interactor.PatientInfor;

public class GetPtInfModelsByNameInteractor : IGetPtInfModelsByNameInputPort
{
    private readonly IPatientInforRepository _patientInforRepository;

    public GetPtInfModelsByNameInteractor(IPatientInforRepository patientInforRepository)
    {
        _patientInforRepository = patientInforRepository;
    }

    public GetPtInfModelsByNameOutputData Handle(GetPtInfModelsByNameInputData inputData)
    {
        try
        {
            var result = _patientInforRepository.GetPtInfModelsByName(inputData.HpId, inputData.KanaName, inputData.Name, inputData.BirthDate, inputData.Sex1, inputData.Sex2);
            return new GetPtInfModelsByNameOutputData(result, GetPtInfModelsByNameStatus.Successed);
        }
        finally
        {
            _patientInforRepository.ReleaseResource();
        }
    }
}
