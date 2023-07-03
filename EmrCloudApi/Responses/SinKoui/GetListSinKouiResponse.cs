namespace EmrCloudApi.Responses.SinKoui
{
    public class GetListSinKouiResponse
    {
        public GetListSinKouiResponse(List<int> sinYms)
        {
            SinYms = sinYms;
        }

        public List<int> SinYms { get; private set; }
    }
}
