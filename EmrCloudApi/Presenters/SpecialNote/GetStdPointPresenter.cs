using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SpecialNote;
using UseCase.SpecialNote.GetStdPoint;

namespace EmrCloudApi.Presenters.SpecialNote;

public class GetStdPointPresenter : IGetStdPointOutputPort
{
    public Response<GetStdPointResponse> Result { get; private set; } = default!;

    public void Complete(GetStdPointOutputData outputData)
    {
        Result = new Response<GetStdPointResponse>()
        {
            Data = new GetStdPointResponse(new()),
            Status = (byte)outputData.Status
        };
        switch (outputData.Status)
        {
            case GetStdPointStatus.Successed:
                Result.Message = ResponseMessage.Success;
                Result.Data = new GetStdPointResponse(outputData.GcStdInfModels);
                break;
        }
    }
}
