using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.UpdateSingleDoseMst
{
    public sealed class UpdateSingleDoseMstInputData : IInputData<UpdateSingleDoseMstOutputData>
    {
        public UpdateSingleDoseMstInputData(int hpId, int userId, List<SingleDoseMstModel> singleDoseMsts)
        {
            HpId = hpId;
            SingleDoseMsts = singleDoseMsts;
            UserId = userId;
        }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public List<SingleDoseMstModel> SingleDoseMsts { get; private set; }
    }
}
