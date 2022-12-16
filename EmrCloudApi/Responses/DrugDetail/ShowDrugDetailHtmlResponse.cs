namespace EmrCloudApi.Responses.DrugDetail
{
    public class ShowDrugDetailHtmlResponse
    {
        public ShowDrugDetailHtmlResponse(string htmlData)
        {
            HtmlData = htmlData;
        }

        public string HtmlData { get; private set; }
    }
}
