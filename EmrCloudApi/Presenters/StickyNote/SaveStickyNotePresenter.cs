using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using UseCase.StickyNote;
using EmrCloudApi.Responses.StickyNote;

public class SaveStickyNotePresenter : ISaveStickyNoteOutputPort
{
    public Response<ActionStickyNoteResponse> Result { get; private set; } = default!;

    public void Complete(SaveStickyNoteOutputData outputData)
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
            case UpdateStickyNoteStatus.InvalidDate:
                Result.Message = ResponseMessage.InvalidDate;
                break;
            case UpdateStickyNoteStatus.InvalidColor:
                Result.Message = ResponseMessage.InvalidColor;
                break;
            case UpdateStickyNoteStatus.InvalidMemo:
                Result.Message = ResponseMessage.InvalidMemo;
                break;
            case UpdateStickyNoteStatus.InvalidValue:
                Result.Message = ResponseMessage.InvalidValue;
                break;
            case UpdateStickyNoteStatus.Successed:
                Result.Message = ResponseMessage.Success;
                break;
        }
    }
}
        