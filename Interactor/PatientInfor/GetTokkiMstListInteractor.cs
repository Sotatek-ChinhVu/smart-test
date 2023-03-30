using Domain.Models.PatientInfor;
using UseCase.PatientInfor.GetTokiMstList;

namespace Interactor.PatientInfor;

public class GetTokkiMstListInteractor : IGetTokkiMstListInputPort
{
    private readonly IPatientInforRepository _patientInforRepository;

    public GetTokkiMstListInteractor(IPatientInforRepository patientInforRepository)
    {
        _patientInforRepository = patientInforRepository;
    }

    public GetTokkiMstListOutputData Handle(GetTokkiMstListInputData inputData)
    {
        try
        {
            var result = _patientInforRepository.GetListTokki(inputData.HpId, inputData.SeikyuYm * 100 + 1);
            return new GetTokkiMstListOutputData(result, GetTokkiMstListStatus.Successed);
        }
        finally
        {
            _patientInforRepository.ReleaseResource();
        }
    }
}
