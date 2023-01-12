using CommonChecker.Types;
using CommonCheckers.OrderRealtimeChecker.Enums;

namespace CommonCheckers.OrderRealtimeChecker.Models
{
    public class UnitCheckerForOrderListResult<TOdrInf, TOdrDetail>
        where TOdrInf : class, IOdrInfoModel<TOdrDetail>
        where TOdrDetail : class, IOdrInfoDetailModel
    {
        public int RpNo { get; private set; }

        public int RpEdaNo { get; private set; }

        public int RowNo { get; private set; }

        public int Sinday { get; private set; }

        public long PtId { get; private set; }

        public ActionResultType ActionType { get; set; } = ActionResultType.OK;

        public bool IsError => ErrorOrderList != null && ErrorOrderList.Count > 0;

        public List<TOdrInf> ErrorOrderList { get; set; }

        public object ErrorInfo { get; set; }

        public RealtimeCheckerType CheckerType { get; private set; }

        public List<TOdrInf> CheckingOrderList { get; private set; }

        public UnitCheckerForOrderListResult(int rpNo, int rpEdaNo, int rowNo, RealtimeCheckerType checkerType, List<TOdrInf> checkingOrderList, int sinday, long ptId)
        {
            RpNo = rpNo;
            RpEdaNo = rpEdaNo;
            RowNo = rowNo;
            CheckerType = checkerType;
            CheckingOrderList = checkingOrderList;
            Sinday = sinday;
            PtId = ptId;
            ErrorOrderList = new List<TOdrInf>();
            ErrorInfo = string.Empty;
        }
    }
}
