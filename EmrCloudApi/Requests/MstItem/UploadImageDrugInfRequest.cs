using Helper.Enum;

namespace EmrCloudApi.Requests.MstItem;

public class UploadImageDrugInfRequest
{
    public ImageTypeDrug Type { get; set; }

    public string YjCd { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }
}
