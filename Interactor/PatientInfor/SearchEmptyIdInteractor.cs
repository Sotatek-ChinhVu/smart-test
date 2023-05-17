using Domain.Models.PatientInfor;
using Domain.Models.SystemConf;
using UseCase.PatientInfor.SearchEmptyId;

namespace Interactor.PatientInfor
{
    public class SearchEmptyIdInteractor : ISearchEmptyIdInputPort
    {
        private readonly IPatientInforRepository _patientInforRepository;
        private readonly ISystemConfRepository _systemConfRepository;

        public SearchEmptyIdInteractor(IPatientInforRepository patientInforRepository, ISystemConfRepository systemConfRepository)
        {
            _patientInforRepository = patientInforRepository;
            _systemConfRepository = systemConfRepository;
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

                bool isPtNumCheckDigit = (int)_systemConfRepository.GetSettingValue(1001, 0, inputData.HpId) == 1;
                int autoSetting = (int)_systemConfRepository.GetSettingValue(1014, 0, inputData.HpId);

                var listEmptyId = _patientInforRepository.SearchEmptyId(inputData.HpId, inputData.PtNum, inputData.PageIndex, inputData.PageSize, isPtNumCheckDigit, autoSetting);

                if (!listEmptyId.Any())
                    return new SearchEmptyIdOutputData(new List<PatientInforModel>(), SearchEmptyIdStatus.NoData);

                return new SearchEmptyIdOutputData(listEmptyId, SearchEmptyIdStatus.Success);
            }
            catch (Exception)
            {
                return new SearchEmptyIdOutputData(new List<PatientInforModel>(), SearchEmptyIdStatus.Failed);
            }
            finally
            {
                _patientInforRepository.ReleaseResource();
            }
        }

    }
}
