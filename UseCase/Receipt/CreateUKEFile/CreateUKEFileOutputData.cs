using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.CreateUKEFile
{
    public class CreateUKEFileOutputData : IOutputData
    {
        public CreateUKEFileOutputData(CreateUKEFileStatus status, string message, int typeMessage, List<UKEFileOutputData> uKEFiles)
        {
            Status = status;
            Message = message;
            TypeMessage = typeMessage;
            UKEFiles = uKEFiles;
        }

        public CreateUKEFileStatus Status { get; private set; }

        public string Message { get; private set; }

        public int TypeMessage { get; private set; }

        public List<UKEFileOutputData> UKEFiles { get; private set; }
    }

    public class UKEFileOutputData
    {
        public UKEFileOutputData(MemoryStream outputStream, string fileName)
        {
            OutputStream = outputStream;
            FileName = fileName;
        }

        public MemoryStream OutputStream { get; private set; }

        public string FileName { get; private set; }
    }
}
