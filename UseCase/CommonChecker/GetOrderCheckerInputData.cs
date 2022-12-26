
using CommonChecker.Models.OrdInf;
using UseCase.Core.Sync.Core;

namespace UseCase.CommonChecker
{
    public class GetOrderCheckerInputData : IInputData<GetOrderCheckerOutputData>
    {
        public GetOrderCheckerInputData(long ptId, int hpId, int sinDay, List<OrdInfoModel> currentListOdr, List<OrdInfoModel> listCheckingOrder)
        {
            PtId = ptId;
            HpId = hpId;
            SinDay = sinDay;
            CurrentListOdr = currentListOdr;
            ListCheckingOrder = listCheckingOrder;
        }

        public long PtId { get; private set; }

        public int HpId { get; private set; }

        public int SinDay { get; private set; }

        public List<OrdInfoModel> CurrentListOdr { get; private set; }
        public List<OrdInfoModel> ListCheckingOrder { get; private set; }
    }
}
