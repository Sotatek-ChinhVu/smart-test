using Helper.Enum;

namespace EmrCloudApi.Requests.MstItem
{
    public class GetListDrugImageRequest
    {
        public ImageTypeDrug Type { get; set; }

        public string YjCd { get; set; } = string.Empty;

        public string SelectedImage { get; set; } = string.Empty;
    }
}
