using EmrCloudApi.Constants;
using EmrCloudApi.Responses.ReceSeikyu;
using EmrCloudApi.Responses;
using UseCase.ReceSeikyu.ImportFile;

namespace EmrCloudApi.Presenters.ReceSeikyu
{
    public class ImportFileReceSeikyuPresenter : IImportFileReceSeikyuOutputPort
    {
        public Response<ImportFileReceSeikyuResponse> Result { get; private set; } = new Response<ImportFileReceSeikyuResponse>();

        public void Complete(ImportFileReceSeikyuOutputData output)
        {
            Result.Data = new ImportFileReceSeikyuResponse(output.ReceInfList.Select(item => new ReceInfoDto(item)).ToList());
            Result.Status = (int)output.Status;
            Result.Message = string.IsNullOrEmpty(output.Message) ? GetMessage(output.Status) : output.Message;
        }

        private string GetMessage(ImportFileReceSeikyuStatus status) => status switch
        {
            ImportFileReceSeikyuStatus.Successful => ResponseMessage.Success,
            ImportFileReceSeikyuStatus.Failed => ResponseMessage.Failed,
            ImportFileReceSeikyuStatus.InvalidContentFile => ResponseMessage.InvalidContentFile,
            _ => string.Empty
        };
    }
}
