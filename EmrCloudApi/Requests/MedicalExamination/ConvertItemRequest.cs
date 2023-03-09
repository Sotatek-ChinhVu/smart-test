using EmrCloudApi.Responses.MstItem;
using OdrInfItemOfTodayOrder = UseCase.OrdInfs.GetListTrees.OdrInfItem;

namespace EmrCloudApi.Requests.MedicalExamination
{
    public class ConvertItemRequest
    {
        public long RaiinNo { get; set; }

        public long PtId { get; set; }

        public int SinDate { get; set; }

        public List<OdrInfItemOfTodayOrder> OdrInfItems { get; set; } = new();

        public Dictionary<string, List<TenItemDto>> ExpiredItems { get; set; } = new();
    }
}
