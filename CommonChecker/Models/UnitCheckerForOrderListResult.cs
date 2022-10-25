using CommonCheckers.OrderRealtimeChecker.Enums;
using Domain.Types;

namespace CommonCheckers.OrderRealtimeChecker.Models
{
    public class UnitCheckerForOrderListResult<TOdrInf, TOdrDetail>
        where TOdrInf : class, IOdrInfModel<TOdrDetail>
        where TOdrDetail : class, IOdrInfDetailModel
    {
        public int Sinday { get; private set; }

        public long PtId { get; private set; }

        public ActionResultType ActionType { get; set; } = ActionResultType.OK;

        public bool IsError => ErrorOrderList != null && ErrorOrderList.Count > 0;

        public List<TOdrInf> ErrorOrderList { get; set; }

        public object ErrorInfo { get; set; }

        public RealtimeCheckerType CheckerType { get; private set; }

        public List<TOdrInf> CheckingOrderList { get; private set; }

        public UnitCheckerForOrderListResult(RealtimeCheckerType checkerType, List<TOdrInf> checkingOrderList, int sinday, long ptId)
        {
            CheckerType = checkerType;
            CheckingOrderList = checkingOrderList;
            Sinday = sinday;
            PtId = ptId;
            ErrorOrderList = new List<TOdrInf>();
            ErrorInfo = string.Empty;
        }
    }
}
