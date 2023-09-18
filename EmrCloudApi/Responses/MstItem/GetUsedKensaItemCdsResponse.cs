namespace EmrCloudApi.Responses.MstItem
{
    public class GetUsedKensaItemCdsResponse
    {
        public GetUsedKensaItemCdsResponse(List<string> datas)
        {
            Datas = datas;
        }

        public List<string> Datas { get; private set;} = new List<string>();
    }
}
