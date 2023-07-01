namespace EmrCloudApi.Responses.SinKoui
{
    public class GetListSinKouiResponse
    {
        public GetListSinKouiResponse(List<string> sinYmBindings)
        {
            SinYmBindings = sinYmBindings;
        }

        public List<string> SinYmBindings { get; private set; }
    }
}
