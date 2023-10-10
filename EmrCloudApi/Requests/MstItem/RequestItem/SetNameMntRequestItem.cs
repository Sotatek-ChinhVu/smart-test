namespace EmrCloudApi.Requests.MstItem.RequestItem;

public class SetNameMntRequestItem
{
    public bool IsSet { get; set; }

    public string SetFlag { get; set; } = string.Empty;

    public string ItemCd { get; set; } = string.Empty;

    public string ItemNameTenMst { get; set; } = string.Empty;

    public string ItemNameTenMstBinding { get; set; } = string.Empty;

    public int SetCd { get; set; }

    public int RowNo { get; set; } = 0;

    public long RpNo { get; set; }

    public long RpEdaNo { get; set; }

    public int SetKbn { get; set; }

    public int SetKbnEdaNo { get; set; }

}
