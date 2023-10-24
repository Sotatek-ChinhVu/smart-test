namespace EmrCloudApi.Requests.SetMst;

public class SaveOdrSetRequest
{
    public SaveOdrSetRequest(int sinDate, List<SaveOdrSetRequestItem> setNameModelList)
    {
        SinDate = sinDate;
        SetNameModelList = setNameModelList;
    }

    public int SinDate { get;private set; }

    public List<SaveOdrSetRequestItem> SetNameModelList { get;private set; }
}
