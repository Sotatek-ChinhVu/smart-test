namespace EmrCloudApi.Responses.Yousiki
{
    public class GetKacodeYousikiMstDictResponse
    {
        public GetKacodeYousikiMstDictResponse(Dictionary<string, string> kacodeYousikiMstDict)
        {
            KacodeYousikiMstDict = kacodeYousikiMstDict;
        }

        public Dictionary<string, string> KacodeYousikiMstDict { get; private set; }
    }
}
