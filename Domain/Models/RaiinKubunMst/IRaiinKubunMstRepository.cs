using Domain.Common;

namespace Domain.Models.RaiinKubunMst
{
    public interface IRaiinKubunMstRepository : IRepositoryBase
    {
        List<RaiinKubunMstModel> GetList(bool isDeleted);

        List<RaiinKubunMstModel> LoadDataKubunSetting(int hpId, int userId);

        List<string> SaveDataKubunSetting(List<RaiinKubunMstModel> raiinKubunMstModels, int userId);

        List<(string, string)> GetListColumnName(int hpId);

        List<(int grpId, int kbnCd, int kouiKbn1, int kouiKbn2)> GetRaiinKouiKbns(int hpId);

        public List<RaiinKbnItemModel> GetRaiinKbnItems(int hpId);

        void Upsert(int hpId, long ptId, int sinDate, long raiinNo, int grpId, int kbnCd, int userId);

        bool SoftDelete(int hpId, long ptId, int sinDate, long raiinNo, int grpId);

        List<RaiinKbnModel> GetRaiinKbns(int hpId, long ptId, long raiinNo, int sinDate);

        List<RaiinKbnModel> InitDefaultByRsv(int hpId, int frameID, List<RaiinKbnModel> raiinKbns);

        IEnumerable<RaiinKbnModel> GetPatientRaiinKubuns(int hpId, long ptId, int raiinNo, int sinDate);

    }
}
