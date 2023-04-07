using Domain.Models.MstItem;

namespace EmrCloudApi.Responses.MstItem
{
    public class GetListTenMstOriginResponse
    {
        public GetListTenMstOriginResponse(List<TenMstOriginModel> tenMsts, IEnumerable<int> startDateDisplay)
        {
            TenMsts = tenMsts;
            StartDateDisplay = startDateDisplay;
        }

        public List<TenMstOriginModel> TenMsts { get; private set; }

        public IEnumerable<int> StartDateDisplay { get; private set; }
    }
}
