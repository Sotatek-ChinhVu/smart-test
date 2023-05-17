using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.ValidateCreateUKEFile
{
    public class ValidateCreateUKEFileOutputData : IOutputData
    {
        public ValidateCreateUKEFileOutputData(ValidateCreateUKEFileStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        public ValidateCreateUKEFileStatus Status { get; private set; }

        public string Message { get; private set; }
    }
}
