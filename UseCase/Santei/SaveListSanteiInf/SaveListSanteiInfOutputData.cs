using UseCase.Core.Sync.Core;

namespace UseCase.Santei.SaveListSanteiInf;

public class SaveListSanteiInfOutputData : IOutputData
{
    public SaveListSanteiInfOutputData(SaveListSanteiInfStatus status)
    {
        Status = status;
    }

    public SaveListSanteiInfStatus Status { get; set; }
}
