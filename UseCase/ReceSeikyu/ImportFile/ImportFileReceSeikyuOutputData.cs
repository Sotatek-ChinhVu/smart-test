using UseCase.Core.Sync.Core;

namespace UseCase.ReceSeikyu.ImportFile
{
    public class ImportFileReceSeikyuOutputData : IOutputData
    {
        public ImportFileReceSeikyuOutputData(ImportFileReceSeikyuStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        public ImportFileReceSeikyuStatus Status { get; private set; }

        public string Message { get; private set; }
    }
}
