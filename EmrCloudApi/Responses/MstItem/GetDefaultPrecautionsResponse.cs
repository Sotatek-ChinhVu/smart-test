namespace EmrCloudApi.Responses.MstItem
{
    public class GetDefaultPrecautionsResponse
    {
        public GetDefaultPrecautionsResponse(string drugInfo)
        {
            DrugInfo = drugInfo;
        }

        public string DrugInfo { get; private set; }
    }
}
