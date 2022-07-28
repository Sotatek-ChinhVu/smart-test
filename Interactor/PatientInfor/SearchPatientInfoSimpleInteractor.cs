using Domain.Models.PatientInfor;
using Domain.Models.PatientInfor.Domain.Models.PatientInfor;
using UseCase.PatientInfor.SearchSimple;

namespace Interactor.PatientInfor
{
    public class SearchPatientInfoSimpleInteractor : ISearchPatientInfoSimpleInputPort
    {
        private readonly IPatientInforRepository _patientInforRepository;

        public SearchPatientInfoSimpleInteractor(IPatientInforRepository patientInforRepository)
        {
            _patientInforRepository = patientInforRepository;
        }

        public SearchPatientInfoSimpleOutputData Handle(SearchPatientInfoSimpleInputData inputData)
        {
            if (string.IsNullOrWhiteSpace(inputData.Keyword))
            {
                return new SearchPatientInfoSimpleOutputData(new List<PatientInforModel>(), SearchPatientInfoSimpleStatus.InvalidKeyword);
            }
            
            var patientInfoList = _patientInforRepository.SearchSimple(inputData.Keyword, inputData.ContainMode);
            if (patientInfoList.Count == 0)
            {
                return new SearchPatientInfoSimpleOutputData(new List<PatientInforModel>(), SearchPatientInfoSimpleStatus.NotFound);
            }

            return new SearchPatientInfoSimpleOutputData(patientInfoList, SearchPatientInfoSimpleStatus.Success);
        }
    }
}
