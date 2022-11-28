using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using UseCase.StickyNote;
using EmrCloudApi.Responses.StickyNote;

public class RevertStickyNotePresenter : IRevertStickyNoteOutputPort
{
    public Response<ActionStickyNoteResponse> Result { get; private set; } = default!;

    public void Complete(RevertStickyNoteOutputData outputData)
    {
        Result = new Response<ActionStickyNoteResponse>()
        {
            Data = new ActionStickyNoteResponse(outputData.Successed),
            Status = (byte)outputData.Status
        };
        switch (outputData.Status)
        {
            case UpdateStickyNoteStatus.InvalidHpId:
                Result.Message = ResponseMessage.InvalidHpId;
                break;
            case UpdateStickyNoteStatus.InvalidPtId:
                Result.Message = ResponseMessage.InvalidPtId;
                break;
            case UpdateStickyNoteStatus.InvalidSeqNo:
                Result.Message = ResponseMessage.InvalidSeqNo;
                break;
            case UpdateStickyNoteStatus.Failed:
                Result.Message = ResponseMessage.Failed;
                break;
            case UpdateStickyNoteStatus.Successed:
                Result.Message = ResponseMessage.Success;
                break;
        }
    }
}
        