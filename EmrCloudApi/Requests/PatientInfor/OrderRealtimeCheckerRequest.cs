using CommonChecker.Models.OrdInf;

namespace EmrCloudApi.Requests.PatientInfor
{
    public class OrderRealtimeCheckerRequest
    {
        public OrderRealtimeCheckerRequest(List<OrdInfoModel> currentListOdr, List<OrdInfoModel> listCheckingOrder)
        {
            CurrentListOdr = currentListOdr;
            ListCheckingOrder = listCheckingOrder;
        }

        public List<OrdInfoModel> CurrentListOdr { get; set; }
        public List<OrdInfoModel> ListCheckingOrder { get; set; }
    }
}
