using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using UseCase.StickyNote;
using EmrCloudApi.Tenant.Responses.StickyNote;

public class DeleteStickyNotePresenter : IDeleteStickyNoteOutputPort
{
    public Response<DeleteRevertStickyNoteResponse> Result { get; private set; } = default!;

    public void Complete(DeleteStickyNoteOutputData outputData)
    {
        Result = new Response<DeleteRevertStickyNoteResponse>()
        {
            Data = new DeleteRevertStickyNoteResponse(outputData.Successed),
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
                Result.Data = new DeleteRevertStickyNoteResponse(outputData.Successed);
                break;
        }
    }
}
