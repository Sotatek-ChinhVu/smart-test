using Domain.Models.ListSetMst;
using UseCase.Core.Sync.Core;

namespace UseCase.ListSetMst.UpdateListSetMst
{
    public class UpdateListSetMstInputData : IInputData<UpdateListSetMstOutputData>
    {
            public UpdateListSetMstInputData(int userId, int hpId, List<ListSetMstUpdateModel> listSetMsts)
            {
                UserId = userId;
                HpId = hpId;
                ListSetMsts = listSetMsts;
            }

            public int UserId { get; private set; }
            public int HpId { get; private set; }
            public List<ListSetMstUpdateModel> ListSetMsts { get; private set; }
    }
}
