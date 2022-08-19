using Domain.Models.RaiinFilterMst;
using UseCase.Core.Sync.Core;

namespace UseCase.RaiinFilterMst.SaveList;

public class SaveRaiinFilterMstListInputData : IInputData<SaveRaiinFilterMstListOutputData>
{
    public SaveRaiinFilterMstListInputData(List<RaiinFilterMstModel> filterMsts)
    {
        FilterMsts = filterMsts;
    }

    public List<RaiinFilterMstModel> FilterMsts { get; private set; }
}
