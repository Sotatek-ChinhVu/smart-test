using Microsoft.AspNetCore.Mvc;
using UseCase.ReceiptCreation.CreateUKEFile;

namespace EmrCloudApi.Responses.ReceiptCreation
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
