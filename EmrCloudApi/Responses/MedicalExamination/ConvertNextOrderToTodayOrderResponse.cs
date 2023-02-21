using UseCase.OrdInfs.GetListTrees;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class ConvertNextOrderToTodayOrderResponse
    {
        public ConvertNextOrderToTodayOrderResponse(List<OdrInfItem> odrInfs)
        {
            OdrInfs = odrInfs;
        }

        public List<OdrInfItem> OdrInfs { get; private set; }
    }
}
