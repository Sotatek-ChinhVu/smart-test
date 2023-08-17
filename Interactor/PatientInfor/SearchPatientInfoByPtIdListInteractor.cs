using Domain.Models.PatientInfor;
using UseCase.PatientInfor.SearchPatientInfoByPtIdList;

namespace Interactor.PatientInfor;

public class SearchPatientInfoByPtIdListInteractor : ISearchPatientInfoByPtIdListInputPort
{
    private readonly IPatientInforRepository _patientInforRepository;

    public SearchPatientInfoByPtIdListInteractor(IPatientInforRepository patientInforRepository)
    {
        _patientInforRepository = patientInforRepository;
    }

    public SearchPatientInfoByPtIdListOutputData Handle(SearchPatientInfoByPtIdListInputData inputData)
    {
        try
        {
            var ptIdList = inputData.PtIdList.Distinct().ToList();
            var result = _patientInforRepository.SearchPatient(inputData.HpId, ptIdList);
            return new SearchPatientInfoByPtIdListOutputData(result, SearchPatientInfoByPtIdListStatus.Success);
        }
        finally
        {
            _patientInforRepository.ReleaseResource();
        }
    }
}
