using Domain.Models.GroupInf;
using Domain.Models.PatientInfor;
using Interactor.PatientInfor.SortPatientCommon;
using UseCase.PatientInfor;
using UseCase.PatientInfor.SearchAdvanced;

namespace Interactor.PatientInfor;

public class SearchPatientInfoAdvancedInteractor : ISearchPatientInfoAdvancedInputPort
{
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IGroupInfRepository _groupInfRepository;
    private readonly ISortPatientCommon _sortPatientCommon;
    private const string startGroupOrderKey = "group_";

    public SearchPatientInfoAdvancedInteractor(IPatientInforRepository patientInforRepository, IGroupInfRepository groupInfRepository, ISortPatientCommon sortPatientCommon)
    {
        _patientInforRepository = patientInforRepository;
        _groupInfRepository = groupInfRepository;
        _sortPatientCommon = sortPatientCommon;
    }

    public SearchPatientInfoAdvancedOutputData Handle(SearchPatientInfoAdvancedInputData input)
    {
        try
        {
            bool sortGroup = input.SortData.Select(item => item.Key).ToList().Exists(item => item.StartsWith(startGroupOrderKey));
            var patientInfos = _patientInforRepository.GetAdvancedSearchResults(input.SearchInput, input.HpId, input.PageIndex, input.PageSize, input.SortData);
            var ptIds = patientInfos.Select(p => p.PtId).ToList();
            var patientGroups = _groupInfRepository.GetAllByPtIdList(ptIds, input.HpId);
            var patientInfoList =
                from pt in patientInfos
                join grp in patientGroups on pt.PtId equals grp.PtId into groups
                select new PatientInfoWithGroup(pt, groups.ToList());

            var status = patientInfoList.Any() ? SearchPatientInfoAdvancedStatus.Success : SearchPatientInfoAdvancedStatus.NoData;
            if (sortGroup)
            {
                var result = _sortPatientCommon.SortData(patientInfoList, input.SortData, input.PageIndex, input.PageSize);
                return new SearchPatientInfoAdvancedOutputData(status, result);
            }
            return new SearchPatientInfoAdvancedOutputData(status, patientInfoList.ToList());
        }
        finally
        {
            _patientInforRepository.ReleaseResource();
            _groupInfRepository.ReleaseResource();
        }
    }
}
