using UseCase.Receipt.CreateUKEFile;

namespace EmrCloudApi.Responses.Receipt
{
    public class CreateUKEFileResponse
    {
        public CreateUKEFileResponse(CreateUKEFileStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        public CreateUKEFileStatus Status { get; private set; }

        public string Message { get; private set; }
    }
}
