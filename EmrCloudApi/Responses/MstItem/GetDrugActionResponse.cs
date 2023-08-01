namespace EmrCloudApi.Responses.MstItem
{
    public class GetDrugActionResponse
    {
        public GetDrugActionResponse(string drugInfo)
        {
            DrugInfo = drugInfo;
        }

        public string DrugInfo { get; private set; }
    }
}
