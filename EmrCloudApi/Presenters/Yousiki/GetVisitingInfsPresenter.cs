using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Yousiki;
using EmrCloudApi.Responses.Yousiki.Dto;
using UseCase.Yousiki.GetVisitingInfs;

namespace EmrCloudApi.Presenters.Yousiki;

public class GetVisitingInfsPresenter : IGetVisitingInfsOutputPort
{
    public Response<GetVisitingInfsResponse> Result { get; private set; } = new();

    public void Complete(GetVisitingInfsOutputData output)
    {
        Result.Data = new GetVisitingInfsResponse(output.AllGrpDictionary, output.VisitingInfList.Select(item => new VisitingInfDto(item)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetVisitingInfsStatus status) => status switch
    {
        GetVisitingInfsStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
