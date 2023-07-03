namespace EmrCloudApi.Responses.MedicalExamination;

public class GetHeaderVistitDateResponse
{
    public GetHeaderVistitDateResponse(string firstDate, string lastDate)
    {
        FirstDate = firstDate;
        LastDate = lastDate;
    }

    public string FirstDate { get; private set; }

    public string LastDate { get; private set; }
}
