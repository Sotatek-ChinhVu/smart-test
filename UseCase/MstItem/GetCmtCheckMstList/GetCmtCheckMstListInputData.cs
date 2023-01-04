using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetCmtCheckMstList
{
    public class GetCmtCheckMstListInputData : IInputData<GetCmtCheckMstListOutputData>
    {
        public GetCmtCheckMstListInputData(int hpId, int userId, List<string> itemCds)
        {
            HpId = hpId;
            UserId = userId;
            ItemCds = itemCds;
        }

        public int HpId { get; private set; }
        public int UserId { get; private set; }
        public List<string> ItemCds { get; private set; }
    }
}
