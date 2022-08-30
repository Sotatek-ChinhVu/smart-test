using Domain.Models.PatientInfor.Domain.Models.PatientInfor;

namespace Domain.Models.PatientInfor
{
    public interface IPatientInforRepository
    {
        PatientInforModel? GetById(int hpId, long ptId, int sinDate, int raiinNo);

        List<PatientInforModel> SearchSimple(string keyword, bool isContainMode);

        List<PatientInforModel> GetAdvancedSearchResults(PatientAdvancedSearchInput input);
        bool CheckListId(List<long> ptIds);
    }
}