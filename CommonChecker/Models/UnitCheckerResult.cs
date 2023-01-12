﻿using CommonChecker.Types;
using CommonCheckers.OrderRealtimeChecker.Enums;

namespace CommonCheckers.OrderRealtimeChecker.Models
{
    public class UnitCheckerResult<TOdrInf, TOdrDetail>
        where TOdrInf : class, IOdrInfoModel<TOdrDetail>
        where TOdrDetail : class, IOdrInfoDetailModel
    {
        public int Id;

        public int Sinday;

        public long PtId;

        public ActionResultType ActionType = ActionResultType.OK;

        public RealtimeCheckerType CheckerType { get; private set; }

        public bool IsError = false;

        public List<TOdrInf>? ErrorOrderList { get; set; }

        public object ErrorInfo { get; set; } = null;

        public TOdrInf CheckingData { get; private set; }

        public List<string> AdditionData { get; set; } = new List<string>();

        public UnitCheckerResult(int id, RealtimeCheckerType checkerType, TOdrInf checkingData, int sinday, long ptId)
        {
            Id = id;
            CheckerType = checkerType;
            CheckingData = checkingData;
            Sinday = sinday;
            PtId = ptId;
        }
    }
}
