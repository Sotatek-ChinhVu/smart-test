using Helper.Messaging;
using Microsoft.AspNetCore.Http;
using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.UploadDrugImage;

public class UploadDrugImageInputData : IInputData<UploadDrugImageOutputData>
{
    public UploadDrugImageInputData(IFormFile? fileUpdateData, IMessenger messenger)
    {
        FileUpdateData = fileUpdateData;
        Messenger = messenger;
    }

    public IFormFile? FileUpdateData { get; private set; }

    public IMessenger Messenger { get; private set; }
}
