namespace EmrCloudApi.Requests.DrugInfor;

public class GetContentDrugUsageHistoryRequest
{
    public long PtId { get; set; }

    public int GrpId { get; set; }

    public int StartDate { get; set; }

    public int EndDate { get; set; }
}
