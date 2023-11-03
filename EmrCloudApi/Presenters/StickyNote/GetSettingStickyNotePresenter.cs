﻿using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.StickyNote;
using UseCase.StickyNote;

namespace EmrCloudApi.Presenters.StickyNote;

public class GetSettingStickyNotePresenter : IGetSettingStickyNoteOutputPort
{
    public Response<GetSettingStickyNoteResponse> Result { get; private set; } = default!;

    public void Complete(GetSettingStickyNoteOutputData outputData)
    {
        Result = new Response<GetSettingStickyNoteResponse>()
        {
            Data = new GetSettingStickyNoteResponse(outputData.StartDate,outputData.EndDate, outputData.FontSize, outputData.Opacity, outputData.Width, outputData.Height, outputData.IsDspUketuke, outputData.IsDspKarte, outputData.IsDspKaikei, outputData.TagGrpCd),
            Status = (byte)outputData.Status
        };
        switch (outputData.Status)
        {
            case UpdateStickyNoteStatus.InvalidValue:
                Result.Message = ResponseMessage.InvalidValue;
                break;
            case UpdateStickyNoteStatus.Successed:
                Result.Message = ResponseMessage.Success;
                break;
        }
    }
}
