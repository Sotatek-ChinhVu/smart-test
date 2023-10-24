using Domain.Models.PatientInfor;
using UseCase.PatientInfor.GetPtInfByRefNo;

namespace Interactor.PatientInfor;

public class GetPtInfByRefNoInteractor : IGetPtInfByRefNoInputPort
{
    private readonly IPatientInforRepository _patientInforRepository;

    public GetPtInfByRefNoInteractor(IPatientInforRepository patientInforRepository)
    {
        _patientInforRepository = patientInforRepository;
    }

    public GetPtInfByRefNoOutputData Handle(GetPtInfByRefNoInputData inputData)
    {
        try
        {
            var result = _patientInforRepository.GetPtInfByRefNo(inputData.HpId, inputData.RefNo);
            return new GetPtInfByRefNoOutputData(result, GetPtInfByRefNoStatus.Successed);
        }
        finally
        {
            _patientInforRepository.ReleaseResource();
        }
    }
}
