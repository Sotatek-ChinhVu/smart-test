using Domain.Common;
using Domain.Models.Reception;

namespace Domain.Models.RaiinKubunMst
{
    public interface IRaiinKubunMstRepository : IRepositoryBase
    {
        List<RaiinKubunMstModel> GetList(bool isDeleted);

        List<RaiinKubunMstModel> LoadDataKubunSetting(int hpId, int userId);

        List<string> SaveDataKubunSetting(List<RaiinKubunMstModel> raiinKubunMstModels, int userId);

        List<(string, string)> GetListColumnName(int hpId);

        bool SaveRaiinKbnInfs(int hpId, long ptId, int sinDate, long raiinNo, int userId, IEnumerable<RaiinKbnInfDto> kbnInfDtos);
    }
}
