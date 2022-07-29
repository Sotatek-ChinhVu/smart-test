using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Reception;
using UseCase.Reception.UpdateStaticCell;

namespace EmrCloudApi.Tenant.Presenters.Reception;

public class UpdateReceptionStaticCellPresenter : IUpdateReceptionStaticCellOutputPort
{
    public Response<UpdateReceptionStaticCellResponse> Result { get; set; } = new Response<UpdateReceptionStaticCellResponse>();

    public void Complete(UpdateReceptionStaticCellOutputData outputData)
    {
        Result.Data = new UpdateReceptionStaticCellResponse { Success = outputData.Success };
        Result.Message = outputData.Message;
        Result.Status = outputData.Status;
    }
}
