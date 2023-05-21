using Microsoft.AspNetCore.Http;
using UseCase.Core.Sync.Core;

namespace UseCase.ReceSeikyu.ImportFile
{
    public class ImportFileReceSeikyuInputData : IInputData<ImportFileReceSeikyuOutputData>
    {
        public ImportFileReceSeikyuInputData(int hpId, int userId, IFormFile file)
        {
            HpId = hpId;
            UserId = userId;
            File = file;
        }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public IFormFile File { get; private set; }
    }
}
