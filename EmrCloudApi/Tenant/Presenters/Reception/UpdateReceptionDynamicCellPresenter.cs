using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Reception;
using UseCase.Reception.UpdateDynamicCell;

namespace EmrCloudApi.Tenant.Presenters.Reception;

public class UpdateReceptionDynamicCellPresenter : IUpdateReceptionDynamicCellOutputPort
{
    public Response<UpdateReceptionDynamicCellResponse> Result { get; set; } = new Response<UpdateReceptionDynamicCellResponse>();

    public void Complete(UpdateReceptionDynamicCellOutputData output)
    {
        Result.Data = new UpdateReceptionDynamicCellResponse { Success = output.Success };
        Result.Message = output.Message;
        Result.Status = output.Status;
    }
}
