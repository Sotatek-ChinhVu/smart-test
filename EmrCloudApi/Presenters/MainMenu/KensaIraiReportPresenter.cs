using EmrCloudApi.Constants;
using EmrCloudApi.Responses.MainMenu;
using EmrCloudApi.Responses;
using UseCase.MainMenu.KensaIraiReport;

namespace EmrCloudApi.Presenters.MainMenu;

public class KensaIraiReportPresenter : IKensaIraiReportOutputPort
{
    public Response<KensaIraiReportResponse> Result { get; private set; } = new();

    public void Complete(KensaIraiReportOutputData output)
    {
        Result.Data = new KensaIraiReportResponse(output.PdfFile, output.DatFile);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(KensaIraiReportStatus status) => status switch
    {
        KensaIraiReportStatus.Successed => ResponseMessage.Success,
        KensaIraiReportStatus.Failed => ResponseMessage.Failed,
        _ => string.Empty
    };
}
