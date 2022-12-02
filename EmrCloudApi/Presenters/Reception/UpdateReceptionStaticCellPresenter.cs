using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Reception;
using UseCase.Reception.UpdateStaticCell;

namespace EmrCloudApi.Presenters.Reception;

public class UpdateReceptionStaticCellPresenter : IUpdateReceptionStaticCellOutputPort
{
    public Response<UpdateReceptionStaticCellResponse> Result { get; set; } = new Response<UpdateReceptionStaticCellResponse>();

    public void Complete(UpdateReceptionStaticCellOutputData outputData)
    {
        var success = IsSuccess(outputData.Status);
        Result.Data = new UpdateReceptionStaticCellResponse(success);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(UpdateReceptionStaticCellStatus status) => status switch
    {
        UpdateReceptionStaticCellStatus.RaiinInfUpdated
        or UpdateReceptionStaticCellStatus.RaiinCmtUpdated
        or UpdateReceptionStaticCellStatus.PatientCmtUpdated => ResponseMessage.UpdateReceptionStaticCellSuccess,
        UpdateReceptionStaticCellStatus.InvalidHpId => ResponseMessage.UpdateReceptionStaticCellInvalidHpId,
        UpdateReceptionStaticCellStatus.InvalidSinDate => ResponseMessage.UpdateReceptionStaticCellInvalidSinDate,
        UpdateReceptionStaticCellStatus.InvalidRaiinNo => ResponseMessage.UpdateReceptionStaticCellInvalidRaiinNo,
        UpdateReceptionStaticCellStatus.InvalidPtId => ResponseMessage.UpdateReceptionStaticCellInvalidPtId,
        _ => ResponseMessage.UpdateReceptionStaticCellUnknownError
    };

    private bool IsSuccess(UpdateReceptionStaticCellStatus status) => status switch
    {
        UpdateReceptionStaticCellStatus.RaiinInfUpdated
        or UpdateReceptionStaticCellStatus.RaiinCmtUpdated
        or UpdateReceptionStaticCellStatus.PatientCmtUpdated => true,
        _ => false
    };
}
