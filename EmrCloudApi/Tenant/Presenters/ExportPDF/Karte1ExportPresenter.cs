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
        _ => string.Empty
    };
}
