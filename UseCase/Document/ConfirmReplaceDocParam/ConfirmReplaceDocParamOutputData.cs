using UseCase.Core.Sync.Core;

namespace UseCase.Document.ConfirmReplaceDocParam;

public class ConfirmReplaceDocParamOutputData : IOutputData
{
    public ConfirmReplaceDocParamOutputData(List<DocCommentOutputItem> docComments, ConfirmReplaceDocParamStatus status)
    {
        DocComments = docComments;
        Status = status;
    }

    public ConfirmReplaceDocParamOutputData(ConfirmReplaceDocParamStatus status)
    {
        DocComments = new();
        Status = status;
    }

    public List<DocCommentOutputItem> DocComments { get; private set; }

    public ConfirmReplaceDocParamStatus Status { get; private set; }
}
