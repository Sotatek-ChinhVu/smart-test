namespace EmrCloudApi.Responses.DrugDetail
{
    public class ShowProductInfResponse
    {
        public ShowProductInfResponse(string htmlData)
        {
            HtmlData = htmlData;
        }

        public string HtmlData { get; private set; }
    }
}
