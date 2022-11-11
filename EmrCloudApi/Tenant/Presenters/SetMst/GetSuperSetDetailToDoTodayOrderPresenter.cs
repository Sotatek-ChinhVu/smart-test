using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.SetMst;
using UseCase.SuperSetDetail.GetSuperSetDetailToDoTodayOrder;

namespace EmrCloudApi.Tenant.Presenters.SetMst;

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
        GetSuperSetDetailToDoTodayOrderStatus.InvalidSetCd => ResponseMessage.InvalidSetCd,
        GetSuperSetDetailToDoTodayOrderStatus.InvalidSinDate => ResponseMessage.InvalidSinDate,
        GetSuperSetDetailToDoTodayOrderStatus.NoData => ResponseMessage.NoData,
        _ => string.Empty
    };
}
