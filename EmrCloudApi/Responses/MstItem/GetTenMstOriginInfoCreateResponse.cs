namespace EmrCloudApi.Responses.MstItem
{
    public class GetTenMstOriginInfoCreateResponse
    {
        public GetTenMstOriginInfoCreateResponse(string itemCd, int jihiSbt)
        {
            ItemCd = itemCd;
            JihiSbt = jihiSbt;
        }

        public string ItemCd { get; private set; }

        public int JihiSbt { get; private set; }
    }
}
