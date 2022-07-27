using EmrCloudApi.Tenant.Responses;
using UseCase.Reception.UpdateStaticCell;

namespace EmrCloudApi.Tenant.Presenters.Reception;

public class UpdateReceptionStaticCellPresenter : IUpdateReceptionStaticCellOutputPort
{
    public Response<bool> Result { get; set; } = new Response<bool>();

    public void Complete(UpdateReceptionStaticCellOutputData outputData)
    {
        Result.Data = outputData.Success;
        Result.Message = outputData.Message;
        Result.Status = outputData.Status;
    }
}
