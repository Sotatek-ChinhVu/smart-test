using UseCase.Core.Sync.Core;

namespace UseCase.SuperSetDetail.SaveConversion;

public class SaveConversionOutputData : IOutputData
{
    public SaveConversionOutputData(SaveConversionStatus status)
    {
        Status = status;
    }

    public SaveConversionStatus Status { get; private set; }
}
