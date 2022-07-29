using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Reception;
using UseCase.Reception.UpdateStaticCell;

namespace EmrCloudApi.Tenant.Presenters.Reception;

public class UpdateReceptionStaticCellPresenter : IUpdateReceptionStaticCellOutputPort
{
    public Response<UpdateReceptionStaticCellResponse> Result { get; set; } = new Response<UpdateReceptionStaticCellResponse>();

    public void Complete(UpdateReceptionStaticCellOutputData outputData)
    {
        Result.Data = new UpdateReceptionStaticCellResponse(outputData.Status == UpdateReceptionStaticCellStatus.Success);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(UpdateReceptionStaticCellStatus status) => status switch
    {
        UpdateReceptionStaticCellStatus.UnknownError => "Failed to update cell value.",
        UpdateReceptionStaticCellStatus.Success => "Cell value updated successfully.",
        UpdateReceptionStaticCellStatus.InvalidHpId => "HpId must be greater than 0.",
        UpdateReceptionStaticCellStatus.InvalidSinDate => "SinDate must be greater than 0.",
        UpdateReceptionStaticCellStatus.InvalidRaiinNo => "RaiinNo must be greater than 0.",
        UpdateReceptionStaticCellStatus.InvalidPtId => "PtId must be greater than 0.",
        _ => string.Empty
    };
}
