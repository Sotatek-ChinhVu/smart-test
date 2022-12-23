using UseCase.Core.Sync.Core;

namespace UseCase.Document.ConfirmReplaceDocParam;

public class ConfirmReplaceDocParamInputData : IInputData<ConfirmReplaceDocParamOutputData>
{
    public ConfirmReplaceDocParamInputData(string textFile)
    {
        TextFile = textFile;
    }

    public string TextFile { get; private set; }
}
