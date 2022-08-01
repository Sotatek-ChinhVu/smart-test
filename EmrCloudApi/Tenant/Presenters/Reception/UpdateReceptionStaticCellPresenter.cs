using EmrCloudApi.Tenant.Constants;
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
        UpdateReceptionStaticCellStatus.UnknownError => ResponseMessage.UpdateReceptionStaticCellUnknownError,
        UpdateReceptionStaticCellStatus.Success => ResponseMessage.UpdateReceptionStaticCellSuccess,
        UpdateReceptionStaticCellStatus.InvalidHpId => ResponseMessage.UpdateReceptionStaticCellInvalidHpId,
        UpdateReceptionStaticCellStatus.InvalidSinDate => ResponseMessage.UpdateReceptionStaticCellInvalidSinDate,
        UpdateReceptionStaticCellStatus.InvalidRaiinNo => ResponseMessage.UpdateReceptionStaticCellInvalidRaiinNo,
        UpdateReceptionStaticCellStatus.InvalidPtId => ResponseMessage.UpdateReceptionStaticCellInvalidPtId,
        _ => string.Empty
    };
}
