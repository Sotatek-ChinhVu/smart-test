using CommonChecker.Models.OrdInf;

namespace EmrCloudApi.Requests.MedicalExamination
{
    public class OrderRealtimeCheckerRequest
    {
        public OrderRealtimeCheckerRequest(long ptId, int hpId, int sinDay, List<OrdInfoModel> currentListOdr, List<OrdInfoModel> listCheckingOrder)
        {
            PtId = ptId;
            HpId = hpId;
            SinDay = sinDay;
            CurrentListOdr = currentListOdr;
            ListCheckingOrder = listCheckingOrder;
        }

        public long PtId { get; set; }

        public int HpId { get; set; }

        public int SinDay { get; set; }

        public List<OrdInfoModel> CurrentListOdr { get; set; }
        public List<OrdInfoModel> ListCheckingOrder { get; set; }
    }
}
