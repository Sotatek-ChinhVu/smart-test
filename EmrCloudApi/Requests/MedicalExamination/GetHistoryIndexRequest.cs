namespace EmrCloudApi.Requests.MedicalExamination;

public class GetHistoryIndexRequest
{
    public long PtId { get; set; }

    public int FilterId { get; set; }

    public int IsDeleted { get; set; }

    public long RaiinNo { get; set; }
}
