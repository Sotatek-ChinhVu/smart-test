using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Santei;
using UseCase.Santei.SaveListSanteiInf;

namespace EmrCloudApi.Presenters.Santei;

public class SaveListSanteiInfPresenter : ISaveListSanteiInfOutputPort
{
    public Response<SaveListSanteiInfResponse> Result { get; private set; } = new();

    public void Complete(SaveListSanteiInfOutputData outputData)
    {
        Result.Data = new SaveListSanteiInfResponse(outputData.Status == SaveListSanteiInfStatus.Successed);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(SaveListSanteiInfStatus status) => status switch
    {
        SaveListSanteiInfStatus.Successed => ResponseMessage.Success,
        SaveListSanteiInfStatus.Failed => ResponseMessage.Failed,
        SaveListSanteiInfStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        SaveListSanteiInfStatus.InvalidItemCd => ResponseMessage.InvalidSanteiItemCd,
        SaveListSanteiInfStatus.InvalidAlertDays => ResponseMessage.InvalidAlertDays,
        SaveListSanteiInfStatus.InvalidAlertTerm => ResponseMessage.InvalidAlertTerm,
        SaveListSanteiInfStatus.InvalidEndDate => ResponseMessage.InvalidEndDate,
        SaveListSanteiInfStatus.InvalidKisanSbt => ResponseMessage.InvalidKisanSbt,
        SaveListSanteiInfStatus.InvalidKisanDate => ResponseMessage.InvalidKisanDate,
        SaveListSanteiInfStatus.InvalidByomei => ResponseMessage.InvalidByomei,
        SaveListSanteiInfStatus.InvalidHosokuComment => ResponseMessage.InvalidHosokuComment,
        SaveListSanteiInfStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        SaveListSanteiInfStatus.InvalidUserId => ResponseMessage.InvalidUserId,
        SaveListSanteiInfStatus.ThisSanteiInfDoesNotAllowSanteiInfDetail => ResponseMessage.ThisSanteiInfDoesNotAllowSanteiInfDetail,
        _ => string.Empty
    };
}

