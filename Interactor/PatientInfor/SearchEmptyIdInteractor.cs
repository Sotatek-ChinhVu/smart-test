using Domain.Models.PatientInfor;
using UseCase.PatientInfor.SearchEmptyId;

namespace Interactor.PatientInfor
{
    public class SearchEmptyIdInteractor : ISearchEmptyIdInputPort
    {
        private readonly IPatientInforRepository _patientInforRepository;

        public SearchEmptyIdInteractor(IPatientInforRepository patientInforRepository)
        {
            _patientInforRepository = patientInforRepository;
        }

        public SearchEmptyIdOutputData Handle(SearchEmptyIdInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                    return new SearchEmptyIdOutputData(new List<PatientInforModel>(), SearchEmptyIdStatus.InvalidHpId);

                if (inputData.PtNum <= 0)
                    return new SearchEmptyIdOutputData(new List<PatientInforModel>(), SearchEmptyIdStatus.InvalidPtNum);

                if (inputData.PageIndex <= 0)
                    return new SearchEmptyIdOutputData(new List<PatientInforModel>(), SearchEmptyIdStatus.InvalidPageIndex);

                if (inputData.PageSize <= 0)
                    return new SearchEmptyIdOutputData(new List<PatientInforModel>(), SearchEmptyIdStatus.InvalidPageSize);

                var listEmptyId = _patientInforRepository.SearchEmptyId(inputData.HpId, inputData.PtNum, inputData.PageIndex, inputData.PageSize);

                if (!listEmptyId.Any())
                    return new SearchEmptyIdOutputData(new List<PatientInforModel>(), SearchEmptyIdStatus.NoData);

                return new SearchEmptyIdOutputData(listEmptyId, SearchEmptyIdStatus.Success);
            }
            catch (Exception)
            {
                return new SearchEmptyIdOutputData(new List<PatientInforModel>(), SearchEmptyIdStatus.Failed);
            }
        }

    }
}
