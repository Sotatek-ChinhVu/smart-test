using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetSelectiveComment
{
    public class GetSelectiveCommentInputData : IInputData<GetSelectiveCommentOutputData>
    {
        public GetSelectiveCommentInputData(int hpId, List<string> listItemCd, int sinDate)
        {
            HpId = hpId;
            ListItemCd = listItemCd;
            SinDate = sinDate;
        }

        public int HpId { get; private set; }

        public List<string> ListItemCd { get; private set; }

        public int SinDate { get; private set; }
    }
}
