using UseCase.Core.Sync.Core;

namespace UseCase.DrugDetailData.ShowMdbByomei
{
    public class ShowMdbByomeiOutputData : IOutputData
    {
        public ShowMdbByomeiOutputData(string htmlData, ShowMdbByomeiStatus status)
        {
            HtmlData = htmlData;
            Status = status;
        }

        public string HtmlData { get; private set; }

        public ShowMdbByomeiStatus Status { get; private set; }
    }
}
