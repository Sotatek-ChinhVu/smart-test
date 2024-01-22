using UseCase.Core.Sync.Core;

namespace UseCase.Yousiki.CreateYuIchiFile;

public class CreateYuIchiFileOutputData : IOutputData
{
    public CreateYuIchiFileOutputData(string messageType, string confirmMessage, CreateYuIchiFileStatus status)
    {
        MessageType = messageType;
        ConfirmMessage = confirmMessage;
        Status = status;
    }

    public string MessageType { get; private set; }

    public string ConfirmMessage { get; private set; }

    public CreateYuIchiFileStatus Status { get; private set; }
}
