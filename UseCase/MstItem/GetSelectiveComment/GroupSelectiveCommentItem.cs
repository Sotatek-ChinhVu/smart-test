using Domain.Models.MstItem;

namespace UseCase.MstItem.GetSelectiveComment
{
    public class GroupSelectiveCommentItem
    {
        public GroupSelectiveCommentItem(List<RecedenCmtSelectModel> listComment)
        {
            ListComment = listComment;
        }

        public List<RecedenCmtSelectModel> ListComment { get; private set; }
    }
}
