using Domain.Models.MstItem;

namespace EmrCloudApi.Responses.MstItem
{
    public class GetSetDataTenMstResponse
    {
        public GetSetDataTenMstResponse(SetDataTenMstOriginModel setData)
        {
            SetData = setData;
        }

        public SetDataTenMstOriginModel SetData { get; private set; }
    }
}
