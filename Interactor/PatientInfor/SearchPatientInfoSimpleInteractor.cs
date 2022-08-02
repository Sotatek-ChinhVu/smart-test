using Domain.Models.GroupInf;
using Domain.Models.PatientInfor;
using Domain.Models.PatientInfor.Domain.Models.PatientInfor;
using UseCase.PatientInfor.SearchSimple;

namespace Interactor.PatientInfor
{
    public class SearchPatientInfoSimpleInteractor : ISearchPatientInfoSimpleInputPort
    {
        private readonly IPatientInforRepository _patientInforRepository;
        private readonly IGroupInfRepository _groupInfRepository;

        public SearchPatientInfoSimpleInteractor(IPatientInforRepository patientInforRepository, IGroupInfRepository groupInfRepository)
        {
            _patientInforRepository = patientInforRepository;
            _groupInfRepository = groupInfRepository;
        }

        public SearchPatientInfoSimpleOutputData Handle(SearchPatientInfoSimpleInputData inputData)
        {
            if (string.IsNullOrWhiteSpace(inputData.Keyword))
            {
                return new SearchPatientInfoSimpleOutputData(new List<PatientInfoWithGroup>(), SearchPatientInfoSimpleStatus.InvalidKeyword);
            }
            
            var patientInfoList = _patientInforRepository.SearchSimple(inputData.Keyword, inputData.ContainMode);
            if (patientInfoList.Count == 0)
            {
                return new SearchPatientInfoSimpleOutputData(new List<PatientInfoWithGroup>(), SearchPatientInfoSimpleStatus.NotFound);
            }

            var ptIdList = patientInfoList.Select(p => p.PtId).ToList();
            var patientGroupInfList = _groupInfRepository.GetAllByPtIdList(ptIdList);

            List<PatientInfoWithGroup> patientInfoListWithGroup = new List<PatientInfoWithGroup>();
            foreach (var patientInfo in patientInfoList)
            {
                long ptId = patientInfo.PtId;
                var groupInfList = patientGroupInfList.Where(p => p.PtId == ptId).ToList();

                patientInfoListWithGroup.Add(new PatientInfoWithGroup(patientInfo, groupInfList));
            }

            return new SearchPatientInfoSimpleOutputData(patientInfoListWithGroup, SearchPatientInfoSimpleStatus.Success);
        }
    }
}
