using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SetMst;
using EmrCloudApi.Responses.SetMst.Dto;
using UseCase.SuperSetDetail.GetConversion;

namespace EmrCloudApi.Presenters.SetMst;

public class GetConversionPresenter : IGetConversionOutputPort
{
    public Response<GetConversionResponse> Result { get; private set; } = new();

    public void Complete(GetConversionOutputData output)
    {
        Result.Data = new GetConversionResponse(new ConversionItemDto(output.ConversionSourceItem), output.ConversionCandidateItemList.Select(item => new ConversionItemDto(item)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetConversionStatus status) => status switch
    {
        GetConversionStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}