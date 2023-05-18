using Helper.Enum;

namespace EmrCloudApi.Requests.MstItem
{
    public class GetTenMstListByItemTypeRequest
    {
        public ItemTypeEnums ItemType { get; set; }

        public int SinDate { get; set; }
    }
}
