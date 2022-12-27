using Domain.Models.PatientInfor;

namespace Domain.Models.RaiinFilterMst;

public interface IRaiinFilterMstRepository
{
    List<RaiinFilterMstModel> GetList();

    void SaveList(List<RaiinFilterMstModel> mstModels, int hpId, int userId);

    List<RaiinFilterMstModel> GetListRaiinInf(int hpId, long ptId);
}
