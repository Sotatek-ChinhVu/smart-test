using Domain.Models.SummaryInf;

namespace UseCase.SpecialNote.Get
{
    public class SummaryTabItem
    {
        public SummaryTabItem(SummaryInfModel? summaryInfItem)
        {
            SummaryInfItem = summaryInfItem;
        }

        public SummaryInfModel? SummaryInfItem { get; private set; }
    }
}
