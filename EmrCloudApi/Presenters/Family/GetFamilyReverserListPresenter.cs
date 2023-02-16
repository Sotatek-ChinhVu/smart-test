using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Family;
using UseCase.Family.GetFamilyReverserList;

namespace EmrCloudApi.Presenters.Family;

public class GetFamilyReverserListPresenter : IGetFamilyReverserListOutputPort
{
    public Response<GetFamilyReverserListResponse> Result { get; private set; } = new();

    public void Complete(GetFamilyReverserListOutputData output)
    {
        Result.Data = new GetFamilyReverserListResponse(output.FamilyList.Select(item => new FamilyReverserDto(item)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetFamilyReverserListStatus status) => status switch
    {
        GetFamilyReverserListStatus.Successed => ResponseMessage.Success,
        GetFamilyReverserListStatus.InvalidPtId => ResponseMessage.PtInfNotFound,
        _ => string.Empty
    };
}
