namespace EmrCloudApi.Responses.MstItem
{
    public class GetKensaCenterMstsResponse
    {
        public GetKensaCenterMstsResponse(Dictionary<string, string> kensaCenterMsts)
        {
            KensaCenterMsts = kensaCenterMsts;
        }

        public Dictionary<string, string> KensaCenterMsts { get; private set; }
    }
}
