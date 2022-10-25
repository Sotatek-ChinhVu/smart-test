using Domain.Models.TodayOdr;
using UseCase.MedicalExamination.UpsertTodayOrd;
using UseCase.OrdInfs.CheckedSpecialItem;

namespace EmrCloudApi.Tenant.Requests.MedicalExamination
{
    public class CheckedSpecialItemRequest
    {
        public int HpId { get; set; }

        public int UserId { get; set; }

        public long PtId { get; set; }

        public int SinDate { get; set; }

        public int IBirthDay { get; set; }

        public int CheckAge { get; set; }

        public long RaiinNo { get; set; }

        public bool EnabledInputCheck { get; set; }

        public bool EnabledCommentCheck { get; set; }

        public List<OdrInfItemInputData> OdrInfs { get; set; } = new();

        public List<CheckedOrderItem> CheckedOrderItems { get; set; } = new();

        public KarteItemInputData KarteInf { get; set; } = new();

        public CheckedSpecialItemStatus Status { get; set; }
    }
}
