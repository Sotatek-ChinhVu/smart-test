using CommonChecker.Types;
using CommonCheckers.OrderRealtimeChecker.Enums;

namespace CommonCheckers.OrderRealtimeChecker.Models
{
    public class UnitCheckerResult<TOdrInf, TOdrDetail>
        where TOdrInf : class, IOdrInfoModel<TOdrDetail>
        where TOdrDetail : class, IOdrInfoDetailModel
    {
        public int Sinday;

        public long PtId;

        public int RpNo;

        public int RpEdaNo;

        public int RowNo;

        public ActionResultType ActionType = ActionResultType.OK;

        public RealtimeCheckerType CheckerType { get; private set; }

        public bool IsError = false;

        public List<TOdrInf>? ErrorOrderList { get; set; }

        public object ErrorInfo { get; set; } = null;

        public TOdrInf CheckingData { get; private set; }

        public List<string> AdditionData { get; set; } = new List<string>();

        public UnitCheckerResult(int rpNo,int rpEdaNo, int rowNo, RealtimeCheckerType checkerType, TOdrInf checkingData, int sinday, long ptId)
        {
            RpNo = rpNo;
            RpEdaNo = rpEdaNo;
            RowNo = rowNo;
            CheckerType = checkerType;
            CheckingData = checkingData;
            Sinday = sinday;
            PtId = ptId;
        }
    }
}
