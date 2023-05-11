using EmrCloudApi.Constants;
using EmrCloudApi.Responses.Family;
using EmrCloudApi.Responses;
using UseCase.Family.GetMaybeFamilyList;

namespace EmrCloudApi.Presenters.Family;

public class GetMaybeFamilyListPresenter : IGetMaybeFamilyListOutputPort
{
    public Response<GetMaybeFamilyListResponse> Result { get; private set; } = new();

    public void Complete(GetMaybeFamilyListOutputData output)
    {
        Result.Data = new GetMaybeFamilyListResponse(output.FamilyList.Select(item => new FamilyDto(item)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetMaybeFamilyListStatus status) => status switch
    {
        GetMaybeFamilyListStatus.Successed => ResponseMessage.Success,
        GetMaybeFamilyListStatus.InvalidPtId => ResponseMessage.NotFoundPtInf,
        GetMaybeFamilyListStatus.InvalidSindate => ResponseMessage.InvalidSinDate,
        _ => string.Empty
    };
}
