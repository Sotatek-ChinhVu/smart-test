using CommonChecker.Models.OrdInf;

namespace EmrCloudApi.Requests.PatientInfor
{
    public class OrderRealtimeCheckerRequest
    {
        public OrderRealtimeCheckerRequest(List<OrdInfoModel> currentListOdr, OrdInfoModel listCheckingOrder)
        {
            CurrentListOdr = currentListOdr;
            ListCheckingOrder = listCheckingOrder;
        }

        public List<OrdInfoModel> CurrentListOdr { get; private set; }
        public OrdInfoModel ListCheckingOrder { get; private set; }
    }
}
