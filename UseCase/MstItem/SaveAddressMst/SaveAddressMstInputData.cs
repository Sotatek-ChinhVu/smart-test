using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.SaveAddressMst
{
    public class SaveAddressMstInputData : IInputData<SaveAddressMstOutputData>
    {
        public SaveAddressMstInputData(int hpId, int userId, List<PostCodeMstModel> postCodeMsts)
        {
            HpId = hpId;
            UserId = userId;
            PostCodeMsts = postCodeMsts;
        }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public List<PostCodeMstModel> PostCodeMsts { get; private set; }
    }
}
