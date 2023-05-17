using UseCase.Receipt.ValidateCreateUKEFile;

namespace EmrCloudApi.Responses.Receipt
{
    public class ValidateCreateUKEFileResponse
    {
        public ValidateCreateUKEFileResponse(ValidateCreateUKEFileStatus status)
        {
            Status = status;
        }

        public ValidateCreateUKEFileStatus Status { get; private set; }
    }
}
