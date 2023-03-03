using Schema.Insurance.SaveInsuranceScan;

namespace EmrCloudApi.Responses.Schema;

public class SaveImageResponse
{
    public SaveImageResponse(SaveInsuranceScanStatus state, IEnumerable<string> filePaths)
    {
        Status = state;
        FilePaths = filePaths;
    }

    public SaveInsuranceScanStatus Status { get; private set; }

    public IEnumerable<string> FilePaths { get; private set; }
}
