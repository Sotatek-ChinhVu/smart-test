namespace EmrCloudApi.Responses.Schema;

public class SaveListFileResponse
{
    public SaveListFileResponse(List<string> listKarteFile)
    {
        ListKarteFile = listKarteFile;
    }

    public List<string> ListKarteFile { get; private set; }
}
