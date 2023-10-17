using UseCase.Core.Sync.Core;

namespace UseCase.SuperSetDetail.SaveConversion;

public class SaveConversionOutputData : IOutputData
{
    public SaveConversionOutputData(SaveConversionStatus status, string errorMessage)
    {
        Status = status;
        ErrorMessage = errorMessage;
    }

    public string ErrorMessage { get; private set; }

    public SaveConversionStatus Status { get; private set; }
}
