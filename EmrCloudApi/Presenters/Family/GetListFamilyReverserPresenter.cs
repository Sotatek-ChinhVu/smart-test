using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Family;
using UseCase.Family.GetListFamilyReverser;

namespace EmrCloudApi.Presenters.Family;

public class GetListFamilyReverserPresenter : IGetListFamilyReverserOutputPort
{
    public Response<GetListFamilyReverserResponse> Result { get; private set; } = new();

    public void Complete(GetListFamilyReverserOutputData output)
    {
        Result.Data = new GetListFamilyReverserResponse(output.ListFamily.Select(item => new FamilyReverserDto(item)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetListFamilyReverserStatus status) => status switch
    {
        GetListFamilyReverserStatus.Successed => ResponseMessage.Success,
        GetListFamilyReverserStatus.InvalidPtId => ResponseMessage.PtInfNotFound,
        _ => string.Empty
    };
}
