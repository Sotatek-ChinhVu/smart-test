using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Family;
using UseCase.Family.GetFamilyList;

namespace EmrCloudApi.Presenters.Family;

public class GetFamilyListPresenter : IGetFamilyListOutputPort
{
    public Response<GetFamilyListResponse> Result { get; private set; } = new();

    public void Complete(GetFamilyListOutputData output)
    {
        Result.Data = new GetFamilyListResponse(output.FamilyList.Select(item => new FamilyDto(item)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetFamilyListStatus status) => status switch
    {
        GetFamilyListStatus.Successed => ResponseMessage.Success,
        GetFamilyListStatus.InvalidPtId => ResponseMessage.NotFoundPtInf,
        GetFamilyListStatus.InvalidSindate => ResponseMessage.InvalidSinDate,
        _ => string.Empty
    };
}
