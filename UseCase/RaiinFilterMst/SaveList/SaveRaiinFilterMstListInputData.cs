using Domain.Models.RaiinFilterMst;
using UseCase.Core.Sync.Core;

namespace UseCase.RaiinFilterMst.SaveList;

public class SaveRaiinFilterMstListInputData : IInputData<SaveRaiinFilterMstListOutputData>
{
    public SaveRaiinFilterMstListInputData(List<RaiinFilterMstModel> filterMsts, int hpId, int userId)
    {
        FilterMsts = filterMsts;
        HpId = hpId;
        UserId = userId;
    }

    public List<RaiinFilterMstModel> FilterMsts { get; private set; }

    public int HpId { get; private set; }

    public int UserId { get; private set; }
}
