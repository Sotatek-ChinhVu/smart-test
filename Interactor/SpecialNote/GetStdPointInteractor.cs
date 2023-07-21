using Domain.Models.SpecialNote.PatientInfo;
using UseCase.SpecialNote.GetStdPoint;

namespace Interactor.SpecialNote;

public class GetStdPointInteractor : IGetStdPointInputPort
{
    private readonly IPatientInfoRepository _patientInfoGetStdRepository;

    public GetStdPointInteractor(IPatientInfoRepository patientInfoGetStdRepository)
    {
        _patientInfoGetStdRepository = patientInfoGetStdRepository;
    }

    public GetStdPointOutputData Handle(GetStdPointInputData inputData)
    {
        try
        {
            var gcStdMsts = _patientInfoGetStdRepository.GetStdPoint(inputData.HpId, inputData.Sex);
            return new GetStdPointOutputData(gcStdMsts, GetStdPointStatus.Successed);
        }
        finally
        {
            _patientInfoGetStdRepository.ReleaseResource();
        }
    }
}
