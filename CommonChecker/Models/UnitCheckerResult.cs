using CommonChecker.Types;
using CommonCheckers.OrderRealtimeChecker.Enums;

namespace CommonCheckers.OrderRealtimeChecker.Models
{
    public class UnitCheckerResult<TOdrInf, TOdrDetail>
        where TOdrInf : class, IOdrInfoModel<TOdrDetail>
        where TOdrDetail : class, IOdrInfoDetailModel
    {
        public int Sinday { get; set; }

        public long PtId { get; set; }

        public ActionResultType ActionType { get; set; } = ActionResultType.OK;

        public RealtimeCheckerType CheckerType { get; private set; }

        public bool IsError { get; set; } = false;

        public List<TOdrInf>? ErrorOrderList { get; set; }

        public object ErrorInfo { get; set; } = new();

        public TOdrInf CheckingData { get; private set; }

        public List<string> AdditionData { get; set; } = new List<string>();

        public UnitCheckerResult(RealtimeCheckerType checkerType, TOdrInf checkingData, int sinday, long ptId)
        {
            CheckerType = checkerType;
            CheckingData = checkingData;
            Sinday = sinday;
            PtId = ptId;
        }
    }
}
