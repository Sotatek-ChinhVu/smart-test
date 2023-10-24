using Microsoft.AspNetCore.Mvc;
using UseCase.Receipt.CreateUKEFile;

namespace EmrCloudApi.Responses.Receipt
{
    public class CreateUKEFileResponse
    {
        public CreateUKEFileResponse(CreateUKEFileStatus status, string message, int typeMessage)
        {
            Status = status;
            Message = message;
            TypeMessage = typeMessage;
        }

        public CreateUKEFileStatus Status { get; private set; }

        public string Message { get; private set; }

        public int TypeMessage { get; private set; }

        public List<FileContentResult> File { get; set; } = new();
    }
}
