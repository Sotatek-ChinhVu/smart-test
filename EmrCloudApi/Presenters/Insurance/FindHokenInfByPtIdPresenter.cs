using EmrCloudApi.Constants;
using EmrCloudApi.Responses.AccountDue;
using EmrCloudApi.Responses;
using UseCase.Insurance.FindHokenInfByPtId;
using EmrCloudApi.Responses.Insurance;

namespace EmrCloudApi.Presenters.Insurance;

public class FindHokenInfByPtIdPresenter : IFindHokenInfByPtIdOutputPort
{
    public Response<FindHokenInfByPtIdResponse> Result { get; private set; } = new();

    public void Complete(FindHokenInfByPtIdOutputData output)
    {
        Result.Data = new FindHokenInfByPtIdResponse(output.HokenInfList.Select(item => new HokenInfDto(item)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(FindHokenInfByPtIdStatus status) => status switch
    {
        FindHokenInfByPtIdStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}