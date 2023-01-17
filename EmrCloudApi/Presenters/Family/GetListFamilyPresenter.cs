using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Family;
using UseCase.Family.GetListFamily;

namespace EmrCloudApi.Presenters.Family;

public class GetListFamilyPresenter : IGetListFamilyOutputPort
{
    public Response<GetListFamilyResponse> Result { get; private set; } = new();

    public void Complete(GetListFamilyOutputData output)
    {
        Result.Data = new GetListFamilyResponse(output.ListFamily.Select(item => new FamilyDto(item)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetListFamilyStatus status) => status switch
    {
        GetListFamilyStatus.Successed => ResponseMessage.Success,
        GetListFamilyStatus.InvalidPtId => ResponseMessage.PtInfNotFould,
        GetListFamilyStatus.InvalidSindate => ResponseMessage.InvalidSinDate,
        _ => string.Empty
    };
}
