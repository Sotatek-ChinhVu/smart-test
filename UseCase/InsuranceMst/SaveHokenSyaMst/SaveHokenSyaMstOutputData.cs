using Domain.Models.InsuranceMst;
using UseCase.Core.Sync.Core;

namespace UseCase.InsuranceMst.SaveHokenSyaMst
{
    public class SaveHokenSyaMstOutputData : IOutputData
    {
        public SaveHokenSyaMstStatus Status { get; private set; }
        public string Message { get; private set; }

        public SaveHokenSyaMstOutputData(SaveHokenSyaMstStatus status, string message)
        {
            Status = status;
            Message = message;
        }
    }
}
