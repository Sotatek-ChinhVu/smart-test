using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Reception;
using UseCase.Reception.GetList;

namespace EmrCloudApi.Presenters.Reception;

public class GetReceptionListPresenter : IGetReceptionListOutputPort
{
    public Response<GetReceptionListResponse> Result { get; private set; } = new Response<GetReceptionListResponse>();

    public void Complete(GetReceptionListOutputData output)
    {
        Result.Data = new GetReceptionListResponse(output.ReceptionInfos);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetReceptionListStatus status) => status switch
    {
        GetReceptionListStatus.Success => ResponseMessage.Success,
        GetReceptionListStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        GetReceptionListStatus.InvalidSinDate => ResponseMessage.InvalidSinDate,
        _ => string.Empty
    };
}
