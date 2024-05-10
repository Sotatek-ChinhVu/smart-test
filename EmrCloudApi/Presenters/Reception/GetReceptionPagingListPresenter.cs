using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Reception;
using UseCase.Reception.GetPagingList;

namespace EmrCloudApi.Presenters.Reception;

public class GetReceptionPagingListPresenter : IGetReceptionPagingListOutputPort
{
    public Response<GetReceptionPagingListResponse> Result { get; private set; } = new();

    public void Complete(GetReceptionPagingListOutputData output)
    {
        Result.Data = new GetReceptionPagingListResponse(output.ReceptionInfos, output.TotalItems);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetReceptionPagingListStatus status) => status switch
    {
        GetReceptionPagingListStatus.Success => ResponseMessage.Success,
        GetReceptionPagingListStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        GetReceptionPagingListStatus.InvalidSinDate => ResponseMessage.InvalidSinDate,
        _ => string.Empty
    };
}
