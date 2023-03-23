using Domain.Models.PatientInfor;
using UseCase.PatientInfor.SearchPatientInfoByPtNum;
using UseCase.PatientInfor.SearchSimple;

namespace Interactor.PatientInfor;

public class SearchPatientInfoByPtNumInteractor : ISearchPatientInfoByPtNumInputPort
{
    private readonly IPatientInforRepository _patientInforRepository;

    public SearchPatientInfoByPtNumInteractor(IPatientInforRepository patientInforRepository)
    {
        _patientInforRepository = patientInforRepository;
    }

    public SearchPatientInfoByPtNumOutputData Handle(SearchPatientInfoByPtNumInputData inputData)
    {
        (PatientInforModel ptInfModel, bool isFound) = _patientInforRepository.SearchExactlyPtNum(inputData.PtNum, inputData.HpId);
        return new SearchPatientInfoByPtNumOutputData(ptInfModel, SearchPatientInfoByPtNumStatus.Successed);
    }
}
