using UseCase.Core.Sync.Core;

namespace UseCase.Document.GetListDocComment;

public class GetListDocCommentOutputData : IOutputData
{
    public GetListDocCommentOutputData(List<DocCommentOutputItem> docComments, GetListDocCommentStatus status)
    {
        DocComments = docComments;
        Status = status;
    }

    public GetListDocCommentOutputData(GetListDocCommentStatus status)
    {
        DocComments = new();
        Status = status;
    }

    public List<DocCommentOutputItem> DocComments { get; private set; }

    public GetListDocCommentStatus Status { get; private set; }
}
