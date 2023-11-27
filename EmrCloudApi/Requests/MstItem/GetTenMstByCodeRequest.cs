namespace EmrCloudApi.Requests.MstItem;

public class GetTenMstByCodeRequest
{
    public string ItemCd { get; set; } = string.Empty;
    public int SetKbn {  get; set; }
    public int SinDate {  get; set; }
}
