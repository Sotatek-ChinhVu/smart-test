using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.ExportPDF;
using UseCase.ExportPDF.ExportKarte1;

namespace EmrCloudApi.Tenant.Presenters.ExportPDF;

public class Karte1ExportPresenter : IExportKarte1OutputPort
{
    public Response<Karte1ExportResponse> Result { get; private set; } = new();

    public void Complete(ExportKarte1OutputData output)
    {
        Result.Data = new Karte1ExportResponse(output.Url);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(ExportKarte1Status status) => status switch
    {
        ExportKarte1Status.Success => ResponseMessage.Success,
        ExportKarte1Status.PtInfNotFould => ResponseMessage.PtInfNotFould,
        ExportKarte1Status.InvalidSindate => ResponseMessage.InvalidSinDate,
        ExportKarte1Status.InvalidHpId => ResponseMessage.InvalidHpId,
        ExportKarte1Status.HokenNotFould => ResponseMessage.HokenNotFould,
        ExportKarte1Status.Failed => ResponseMessage.Failed,
        ExportKarte1Status.CanNotExportPdf => ResponseMessage.CanNotExportPdf,
        ExportKarte1Status.CanNotReturnPdfFile => ResponseMessage.CanNotReturnPdfFile,
        _ => string.Empty
    };
}
