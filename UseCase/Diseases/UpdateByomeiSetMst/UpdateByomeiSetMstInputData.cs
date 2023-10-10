using Domain.Models.Diseases;
using Domain.Models.ListSetMst;
using UseCase.CalculationInf;
using UseCase.Core.Sync.Core;

namespace UseCase.ByomeiSetMst.UpdateByomeiSetMst
{
    public class UpdateByomeiSetMstInputData : IInputData<UpdateByomeiSetMstOutputData>
    {
        public UpdateByomeiSetMstInputData(int userId, int hpId, List<ByomeiSetMstUpdateModel> byomeiSetMstUpdates)
        {
            UserId = userId;
            HpId = hpId;
            ByomeiSetMstUpdates = byomeiSetMstUpdates;
        }

        public int UserId { get; private set; }
        public int HpId { get; private set; }
        public List<ByomeiSetMstUpdateModel> ByomeiSetMstUpdates { get; private set; }
    }
}
