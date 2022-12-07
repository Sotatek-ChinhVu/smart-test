namespace EmrCloudApi.Responses.Schema;

public class SaveListFileTodayOrderResponse
{
    public SaveListFileTodayOrderResponse(List<string> listKarteFile)
    {
        ListKarteFile = listKarteFile;
    }

    public List<string> ListKarteFile { get; private set; }
}
