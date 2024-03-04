using Domain.Models.ReceSeikyu;
using UseCase.Core.Sync.Core;

namespace UseCase.ReceSeikyu.ImportFile
{
    public class ImportFileReceSeikyuOutputData : IOutputData
    {
        public ImportFileReceSeikyuOutputData(ImportFileReceSeikyuStatus status, List<ReceInfo> receInfList, string message)
        {
            Status = status;
            ReceInfList = receInfList;
            Message = message;
        }

        public ImportFileReceSeikyuOutputData(ImportFileReceSeikyuStatus status, string message)
        {
            Status = status;
            ReceInfList = new();
            Message = message;
        }

        public ImportFileReceSeikyuStatus Status { get; private set; }

        public List<ReceInfo> ReceInfList { get; private set; }

        public string Message { get; private set; }
    }
}
