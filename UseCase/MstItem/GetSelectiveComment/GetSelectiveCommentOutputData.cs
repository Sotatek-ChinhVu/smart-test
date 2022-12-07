using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetSelectiveComment
{
    public class GetSelectiveCommentOutputData : IOutputData
    {
        public GetSelectiveCommentOutputData(List<GetSelectiveCommentItemOfList> comments, GetSelectiveCommentStatus status)
        {
            Comments = comments;
            Status = status;
        }

        public List<GetSelectiveCommentItemOfList> Comments { get; private set; }
        public GetSelectiveCommentStatus Status { get; private set; }
    }
}
