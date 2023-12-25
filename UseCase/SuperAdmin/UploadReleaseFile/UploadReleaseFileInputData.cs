using Helper.Messaging;
using Microsoft.AspNetCore.Http;
using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.UploadReleaseFile;

public class UploadReleaseFileInputData : IInputData<UploadReleaseFileOutputData>
{
    public UploadReleaseFileInputData(IFormFile? fileUpdateData, IMessenger messenger)
    {
        FileUpdateData = fileUpdateData;
        Messenger = messenger;
    }

    public IFormFile? FileUpdateData { get; private set; }

    public IMessenger Messenger { get; private set; }
}
