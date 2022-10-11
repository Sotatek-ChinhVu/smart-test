using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using UseCase.StickyNote;
using EmrCloudApi.Tenant.Responses.StickyNote;

public class DeleteStickyNotePresenter : IDeleteStickyNoteOutputPort
{
    public Response<ActionStickyNoteResponse> Result { get; private set; } = default!;

    public void Complete(DeleteStickyNoteOutputData outputData)
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
