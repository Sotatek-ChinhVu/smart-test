using Domain.Models.PatientInfor.Domain.Models.PatientInfor;

namespace Domain.Models.PatientInfor
{
    public interface IPatientInforRepository
    {
        PatientInforModel? GetById(int hpId, long ptId, int sinDate, int raiinNo);

        (PatientInforModel, bool) SearchExactlyPtNum(int ptNum);

        List<PatientInforModel> SearchContainPtNum(int ptNum, string keyword);

        List<PatientInforModel> SearchBySindate(int sindate);

        List<PatientInforModel> SearchPhone(string keyword, bool isContainMode);

        List<PatientInforModel> SearchName(string keyword, bool isContainMode);

        List<PatientInforModel> SearchSimple(string keyword, bool isContainMode);

        List<PatientInforModel> GetAdvancedSearchResults(PatientAdvancedSearchInput input);

        PatientInforModel PatientCommentModels(int hpId, long ptId);

        List<PatientInforModel> SearchEmptyPatientID(long ptId);

        bool CheckListId(List<long> ptIds);
    }
}