using UseCase.Core.Sync.Core;

namespace UseCase.DrugDetailData.ShowProductInf
{
    public class ShowProductInfOutputData : IOutputData
    {
        public ShowProductInfOutputData(string htmlData, ShowProductInfStatus status)
        {
            HtmlData = htmlData;
            Status = status;
        }

        public string HtmlData { get; private set; }

        public ShowProductInfStatus Status { get; private set; }
    }
}
