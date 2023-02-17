using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetReceHenReason;

public class GetReceHenReasonOutputData : IOutputData
{
    public GetReceHenReasonOutputData(GetReceHenReasonStatus status, string receReasonCmt)
    {
        Status = status;
        ReceReasonCmt = receReasonCmt;
    }

    public GetReceHenReasonStatus Status { get; private set; }

    public string ReceReasonCmt { get; private set; }
}
