using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.StickyNote;
using UseCase.StickyNote;

public class GetStickyNotePresenter : IGetStickyNoteOutputPort
{
    public Response<GetStickyNoteResponse> Result { get; private set; } = default!;

    public void Complete(GetStickyNoteOutputData outputData)
    {
        Result = new Response<GetStickyNoteResponse>()
        {
            Data = new GetStickyNoteResponse(outputData.StickyNoteModels),
            Status = (byte)outputData.Status
        };
        switch (outputData.Status)
        {
            case GetStickyNoteStatus.InvalidHpId:
                Result.Message = ResponseMessage.InvalidHpId;
                break;
            case GetStickyNoteStatus.NoData:
                Result.Message = ResponseMessage.NoData;
                break;
            case GetStickyNoteStatus.Successed:
                Result.Message = ResponseMessage.Success;
                break;
        }
    }
}
