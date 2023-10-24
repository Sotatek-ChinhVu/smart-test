using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using UseCase.MstItem.GetRenkeiConf;
using EmrCloudApi.Responses.MstItem;
using EmrCloudApi.Responses.MstItem.Dto;

namespace EmrCloudApi.Presenters.MstItem;

public class GetRenkeiConfPresenter : IGetRenkeiConfOutputPort
{
    public Response<GetRenkeiConfResponse> Result { get; private set; } = new();

    public void Complete(GetRenkeiConfOutputData output)
    {
        Result.Data = new GetRenkeiConfResponse(output.RenkeiConfList.Select(item => new RenkeiConfDto(item)).ToList(),
                                                output.RenkeiMstModelList.Select(item => new RenkeiMstDto(item)).ToList(),
                                                output.RenkeiTemplateMstModelList.Select(item => new RenkeiTemplateMstDto(item)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetRenkeiConfStatus status) => status switch
    {
        GetRenkeiConfStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
