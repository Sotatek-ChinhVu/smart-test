using Helper.Enum;

namespace EmrCloudApi.Requests.MstItem
{
    public class GetListDrugImageRequest
    {
        public ImageTypeDrug Type { get; set; }

        public string YjCd { get; set; } = string.Empty;
    }
}
