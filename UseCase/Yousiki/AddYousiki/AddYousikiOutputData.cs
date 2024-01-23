using UseCase.Core.Sync.Core;

namespace UseCase.Yousiki.AddYousiki;

public class AddYousikiOutputData : IOutputData
{
    public AddYousikiOutputData(string messageType, AddYousikiStatus status)
    {
        MessageType = messageType;
        Status = status;
    }

    public string MessageType { get; private set; }

    public AddYousikiStatus Status { get; private set; }
}
