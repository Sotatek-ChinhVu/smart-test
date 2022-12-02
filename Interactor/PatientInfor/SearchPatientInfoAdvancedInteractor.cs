using Domain.Models.GroupInf;
using Domain.Models.PatientInfor;
using UseCase.PatientInfor.SearchAdvanced;
using UseCase.PatientInfor.SearchSimple;

namespace Interactor.PatientInfor;

public class SearchPatientInfoAdvancedInteractor : ISearchPatientInfoAdvancedInputPort
{
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IGroupInfRepository _groupInfRepository;

    public SearchPatientInfoAdvancedInteractor(IPatientInforRepository patientInforRepository, IGroupInfRepository groupInfRepository)
    {
        _patientInforRepository = patientInforRepository;
        _groupInfRepository = groupInfRepository;
    }

    public SearchPatientInfoAdvancedOutputData Handle(SearchPatientInfoAdvancedInputData input)
    {
        var patientInfos = _patientInforRepository.GetAdvancedSearchResults(input.SearchInput, input.HpId, input.PageIndex, input.PageSize);
        var ptIds = patientInfos.Select(p => p.PtId).ToList();
        var patientGroups = _groupInfRepository.GetAllByPtIdList(ptIds);
        var ptWithGroups =
            from pt in patientInfos
            join grp in patientGroups on pt.PtId equals grp.PtId into groups
            select new PatientInfoWithGroup(pt, groups.ToList());

        var status = ptWithGroups.Any() ? SearchPatientInfoAdvancedStatus.Success : SearchPatientInfoAdvancedStatus.NoData;
        return new SearchPatientInfoAdvancedOutputData(status, ptWithGroups.ToList());
    }
}
