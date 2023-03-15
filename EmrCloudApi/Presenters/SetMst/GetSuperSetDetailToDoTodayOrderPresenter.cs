using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SetMst;
using UseCase.SuperSetDetail.GetSuperSetDetailToDoTodayOrder;

namespace EmrCloudApi.Presenters.SetMst;

public class GetSuperSetDetailToDoTodayOrderPresenter : IGetSuperSetDetailToDoTodayOrderOutputPort
{
    public Response<GetSuperSetDetailToDoTodayOrderResponse> Result { get; private set; } = new();

    public void Complete(GetSuperSetDetailToDoTodayOrderOutputData output)
    {
        Result.Data = new GetSuperSetDetailToDoTodayOrderResponse(output.SetByomeiItems, output.SetKarteInfItems, output.SetOrderInfItems);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetSuperSetDetailToDoTodayOrderStatus status) => status switch
    {
        GetSuperSetDetailToDoTodayOrderStatus.Successed => ResponseMessage.Success,
        GetSuperSetDetailToDoTodayOrderStatus.Failed => ResponseMessage.Failed,
        GetSuperSetDetailToDoTodayOrderStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        GetSuperSetDetailToDoTodayOrderStatus.InvalidUserId => ResponseMessage.InvalidUserId,
        GetSuperSetDetailToDoTodayOrderStatus.InvalidSetCd => ResponseMessage.InvalidSetCd,
        GetSuperSetDetailToDoTodayOrderStatus.InvalidSinDate => ResponseMessage.InvalidSinDate,
        GetSuperSetDetailToDoTodayOrderStatus.NoData => ResponseMessage.NoData,
        _ => string.Empty
    };
}
