using UseCase.Core.Sync.Core;

namespace UseCase.InsuranceMst.SaveHokenMaster
{
    public class SaveHokenMasterOutputData : IOutputData
    {
        public SaveHokenMasterStatus Status { get; private set; }

        public string Message { get; private set; }

        public SaveHokenMasterOutputData(SaveHokenMasterStatus status, string message)
        {
            Status = status;
            Message = message;
        }
    }
}
