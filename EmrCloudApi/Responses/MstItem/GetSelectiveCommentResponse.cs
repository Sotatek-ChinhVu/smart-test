
using UseCase.MstItem.GetSelectiveComment;

namespace EmrCloudApi.Responses.MstItem
{
    public class GetSelectiveCommentResponse
    {
        public GetSelectiveCommentResponse(List<GetSelectiveCommentItemOfList> comments)
        {
            Comments = comments;
        }
        public List<GetSelectiveCommentItemOfList> Comments { get; private set; }
    }
}
