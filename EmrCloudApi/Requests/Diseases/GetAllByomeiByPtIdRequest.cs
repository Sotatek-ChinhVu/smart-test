namespace EmrCloudApi.Requests.Diseases;

public class GetAllByomeiByPtIdRequest
{
    public long PtId { get; set; }

    public int PageIndex { get; set; }

    public int PageSize { get; set; }
}
