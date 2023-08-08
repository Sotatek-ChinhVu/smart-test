using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using UseCase.MstItem.UploadImageDrugInf;
using EmrCloudApi.Responses.MstItem;

namespace EmrCloudApi.Presenters.MstItem;

public class UploadImageDrugInfPresenter : IUploadImageDrugInfOutputPort
{
    public Response<UploadImageDrugInfResponse> Result { get; private set; } = new();

    public void Complete(UploadImageDrugInfOutputData output)
    {
        Result.Data = new UploadImageDrugInfResponse(output.Link);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(UploadImageDrugInfStatus status) => status switch
    {
        UploadImageDrugInfStatus.Successed => ResponseMessage.Success,
        UploadImageDrugInfStatus.Failed => ResponseMessage.Failed,
        UploadImageDrugInfStatus.InvalidFileInput => ResponseMessage.InvalidFileInput,
        UploadImageDrugInfStatus.InvalidTypeImage => ResponseMessage.InvalidTypeUpload,
        _ => string.Empty
    };
}

