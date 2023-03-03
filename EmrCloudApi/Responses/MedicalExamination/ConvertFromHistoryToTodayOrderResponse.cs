
using UseCase.OrdInfs.GetListTrees;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class ConvertFromHistoryToTodayOrderResponse
    {
        public ConvertFromHistoryToTodayOrderResponse(List<OdrInfItem> odrInfItems)
        {
            OdrInfItems = odrInfItems;
        }

        public List<OdrInfItem> OdrInfItems { get; private set; }
    }
}
