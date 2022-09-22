using Domain.Models.InsuranceMst;
using Domain.Models.PatientInfor.Domain.Models.PatientInfor;

namespace Domain.Models.PatientInfor
{
    public interface IPatientInforRepository
    {
        PatientInforModel? GetById(int hpId, long ptId, int sinDate, int raiinNo);

        List<PatientInforModel> SearchSimple(string keyword, bool isContainMode);

        List<PatientInforModel> GetAdvancedSearchResults(PatientAdvancedSearchInput input);

        bool CheckListId(List<long> ptIds);

        List<TokkiMstModel> GetListTokki(int hpId, int sinDate);

        double GetSettingValue(int groupCd, int grpEdaNo);

        string GetSettingParams(int groupCd, int grpEdaNo);
    }
}