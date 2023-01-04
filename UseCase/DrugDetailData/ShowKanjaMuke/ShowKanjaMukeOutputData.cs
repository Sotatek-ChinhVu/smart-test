using UseCase.Core.Sync.Core;

namespace UseCase.DrugDetailData.ShowKanjaMuke
{
    public class ShowKanjaMukeOutputData : IOutputData
    {
        public ShowKanjaMukeOutputData(string htmlData, ShowKanjaMukeStatus status)
        {
            HtmlData = htmlData;
            Status = status;
        }

        public string HtmlData { get; private set; }

        public ShowKanjaMukeStatus Status { get; private set; }
    }
}
