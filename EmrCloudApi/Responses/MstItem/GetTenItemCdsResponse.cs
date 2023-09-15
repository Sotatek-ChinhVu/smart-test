namespace EmrCloudApi.Responses.MstItem
{
    public class GetTenItemCdsResponse
    {
        public GetTenItemCdsResponse(List<string> datas)
        {
            Datas = datas;
        }

        public List<string> Datas { get; private set; }
    }
}
