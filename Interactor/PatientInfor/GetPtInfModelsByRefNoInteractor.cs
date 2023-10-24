using Domain.Models.PatientInfor;
using UseCase.PatientInfor.GetPtInfModelsByRefNo;

namespace Interactor.PatientInfor;

public class GetPtInfModelsByRefNoInteractor : IGetPtInfModelsByRefNoInputPort
{
    private readonly IPatientInforRepository _patientInforRepository;

    public GetPtInfModelsByRefNoInteractor(IPatientInforRepository patientInforRepository)
    {
        _patientInforRepository = patientInforRepository;
    }

    public GetPtInfModelsByRefNoOutputData Handle(GetPtInfModelsByRefNoInputData inputData)
    {
        try
        {
            var result = _patientInforRepository.GetPtInfModels(inputData.HpId, inputData.RefNo);
            return new GetPtInfModelsByRefNoOutputData(result, GetPtInfModelsByRefNoStatus.Successed);
        }
        finally
        {
            _patientInforRepository.ReleaseResource();
        }
    }
}
