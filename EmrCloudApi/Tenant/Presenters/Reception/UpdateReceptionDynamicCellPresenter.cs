using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Reception;
using UseCase.Reception.UpdateDynamicCell;

namespace EmrCloudApi.Tenant.Presenters.Reception;

public class UpdateReceptionDynamicCellPresenter : IUpdateReceptionDynamicCellOutputPort
{
    public Response<UpdateReceptionDynamicCellResponse> Result { get; set; } = new Response<UpdateReceptionDynamicCellResponse>();

    public void Complete(UpdateReceptionDynamicCellOutputData output)
    {
        Result.Data = new UpdateReceptionDynamicCellResponse(output.Status == UpdateReceptionDynamicCellStatus.Success);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(UpdateReceptionDynamicCellStatus status) => status switch
    {
        UpdateReceptionDynamicCellStatus.Success => "Cell value updated successfully.",
        UpdateReceptionDynamicCellStatus.InvalidHpId => "HpId must be greater than 0.",
        UpdateReceptionDynamicCellStatus.InvalidSinDate => "SinDate must be greater than 0.",
        UpdateReceptionDynamicCellStatus.InvalidRaiinNo => "RaiinNo must be greater than 0.",
        UpdateReceptionDynamicCellStatus.InvalidPtId => "PtId must be greater than 0.",
        UpdateReceptionDynamicCellStatus.InvalidGrpId => "GrpId cannot be negative.",
        _ => string.Empty
    };
}
