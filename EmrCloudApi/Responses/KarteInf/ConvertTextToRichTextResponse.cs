namespace EmrCloudApi.Responses.KarteInf;

public class ConvertTextToRichTextResponse
{
    public ConvertTextToRichTextResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
