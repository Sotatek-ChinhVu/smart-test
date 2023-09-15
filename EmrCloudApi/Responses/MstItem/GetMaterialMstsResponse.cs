namespace EmrCloudApi.Responses.MstItem
{
    public class GetMaterialMstsResponse
    {
        public GetMaterialMstsResponse(Dictionary<int, string> getMaterialMsts)
        {
            GetMaterialMsts = getMaterialMsts;
        }

        public Dictionary<int, string> GetMaterialMsts { get; private set; }
    }
}
