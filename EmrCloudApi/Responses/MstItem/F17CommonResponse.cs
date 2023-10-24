using Domain.Models.MstItem;
using UseCase.IsUsingKensa;

namespace EmrCloudApi.Responses.MstItem
{
    public class F17CommonResponse
    {
        public F17CommonResponse(List<string> kensaItemCd, F17CommonStatus status, List<KensaStdMstModel> kensaStdMsts, List<string> itemCd, Dictionary<int, string> materialMsts, Dictionary<int, string> containerMsts, Dictionary<string, string> kensaCenterMsts, Dictionary<string, double> tenOfItem, double latestSedai, List<TenItemModel> tenItemModels)
        {
            KensaItemCd = kensaItemCd;
            Status = status;
            KensaStdMsts = kensaStdMsts;
            ItemCd = itemCd;
            MaterialMsts = materialMsts;
            ContainerMsts = containerMsts;
            KensaCenterMsts = kensaCenterMsts;
            TenOfItem = tenOfItem;
            TenOfItem = tenOfItem;
            LatestSedai = latestSedai;
            TenItemModels = tenItemModels;
        }

        public List<string> KensaItemCd { get; private set; }

        public F17CommonStatus Status { get; private set; }

        public List<KensaStdMstModel> KensaStdMsts { get; private set; }

        public List<string> ItemCd { get; private set; }

        public Dictionary<int, string> MaterialMsts { get; private set; }

        public Dictionary<int, string> ContainerMsts { get; private set; }

        public Dictionary<string, string> KensaCenterMsts { get; private set; }

        public Dictionary<string, double> TenOfItem { get; private set; }

        public double LatestSedai { get; private set; }

        public List<TenItemModel> TenItemModels { get; private set; }
    }
}
