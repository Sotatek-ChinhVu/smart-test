using EmrCloudApi.Constants;
using EmrCloudApi.Responses.MstItem;
using EmrCloudApi.Responses;
using UseCase.MstItem.SaveRenkei;

namespace EmrCloudApi.Presenters.MstItem;

public class SaveRenkeiPresenter : ISaveRenkeiOutputPort
{
    public Response<SaveRenkeiResponse> Result { get; private set; } = new();

    public void Complete(SaveRenkeiOutputData output)
    {
        Result.Data = new SaveRenkeiResponse(output.Status ==  SaveRenkeiStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(SaveRenkeiStatus status) => status switch
    {
        SaveRenkeiStatus.Successed => ResponseMessage.Success,
        SaveRenkeiStatus.Failed => ResponseMessage.Failed,
        SaveRenkeiStatus.InvalidRenkeiId => ResponseMessage.InvalidRenkeiId,
        SaveRenkeiStatus.InvalidParam => ResponseMessage.InvalidParamRenki,
        SaveRenkeiStatus.InvalidTemplateId => ResponseMessage.InvalidTemplateId,
        SaveRenkeiStatus.InvalidIsInvalid => ResponseMessage.InvalidIsInvalid,
        SaveRenkeiStatus.InvalidBiko => ResponseMessage.InvalidBiko,
        SaveRenkeiStatus.InvalidPath => ResponseMessage.InvalidPath,
        SaveRenkeiStatus.InvalidMachine => ResponseMessage.InvalidMachine,
        SaveRenkeiStatus.InvalidWorkPath => ResponseMessage.InvalidWorkPath,
        SaveRenkeiStatus.InvalidUser => ResponseMessage.InvalidUser,
        SaveRenkeiStatus.InvalidPassWord => ResponseMessage.InvalidPassWord,
        SaveRenkeiStatus.InvalidEventCd => ResponseMessage.InvalidEventCd,
        SaveRenkeiStatus.InvalidRenkeiSbt => ResponseMessage.InvalidRenkeiSbt,
        SaveRenkeiStatus.InvalidRenkeiTimingModelList => ResponseMessage.InvalidRenkeiTimingModelList,
        _ => string.Empty
    };
}
