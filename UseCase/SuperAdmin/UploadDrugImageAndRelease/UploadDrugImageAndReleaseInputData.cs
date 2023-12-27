using Helper.Messaging;
using Microsoft.AspNetCore.Http;
using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.UploadDrugImage;

public class UploadDrugImageAndReleaseInputData : IInputData<UploadDrugImageAndReleaseOutputData>
{
    public UploadDrugImageAndReleaseInputData(IFormFile? fileUpdateData, IMessenger messenger, dynamic webSocketService)
    {
        FileUpdateData = fileUpdateData;
        Messenger = messenger;
        WebSocketService = webSocketService;
    }

    public IFormFile? FileUpdateData { get; private set; }

    public IMessenger Messenger { get; private set; }

    public dynamic WebSocketService { get; private set; }
}
