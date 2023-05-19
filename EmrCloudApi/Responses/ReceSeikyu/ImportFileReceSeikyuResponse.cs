using UseCase.ReceSeikyu.ImportFile;

namespace EmrCloudApi.Responses.ReceSeikyu
{
    public class ImportFileReceSeikyuResponse
    {
        public ImportFileReceSeikyuResponse(ImportFileReceSeikyuStatus status)
        {
            Status = status;
        }

        public ImportFileReceSeikyuStatus Status { get; private set; }
    }
}
