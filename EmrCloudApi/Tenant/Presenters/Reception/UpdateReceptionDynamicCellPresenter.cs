using EmrCloudApi.Tenant.Constants;
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
        UpdateReceptionDynamicCellStatus.Success => ResponseMessage.UpdateReceptionDynamicCellSuccess,
        UpdateReceptionDynamicCellStatus.InvalidHpId => ResponseMessage.UpdateReceptionDynamicCellInvalidHpId,
        UpdateReceptionDynamicCellStatus.InvalidSinDate => ResponseMessage.UpdateReceptionDynamicCellInvalidSinDate,
        UpdateReceptionDynamicCellStatus.InvalidRaiinNo => ResponseMessage.UpdateReceptionDynamicCellInvalidRaiinNo,
        UpdateReceptionDynamicCellStatus.InvalidPtId => ResponseMessage.UpdateReceptionDynamicCellInvalidPtId,
        UpdateReceptionDynamicCellStatus.InvalidGrpId => ResponseMessage.UpdateReceptionDynamicCellInvalidGrpId,
        _ => string.Empty
    };
}
