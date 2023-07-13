using UseCase.MstItem.ConvertStringChkJISKj;

namespace EmrCloudApi.Responses.MstItem;

public class ConvertStringChkJISKjResponse
{
    public ConvertStringChkJISKjResponse(ConvertStringChkJISKjOutputData outputData)
    {
        Result = outputData.Result;
        SOut = outputData.SOut;
        ItemError = outputData.ItemError;
    }

    public string Result { get; private set; }

    public string SOut { get; private set; }

    public string ItemError { get; private set; }
}
