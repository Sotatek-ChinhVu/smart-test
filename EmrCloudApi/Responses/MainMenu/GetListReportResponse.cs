namespace EmrCloudApi.Responses.MainMenu;

public class GetListReportResponse
{
    public GetListReportResponse(List<string> fileNameList)
    {
        FileNameList = fileNameList;
    }

    public List<string> FileNameList { get; private set; }
}
