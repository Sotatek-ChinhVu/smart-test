﻿using CommonCheckers.OrderRealtimeChecker.DB;
using CommonCheckers.OrderRealtimeChecker.Enums;
using CommonCheckers.OrderRealtimeChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Services.Interface;
using Domain.Types;

namespace CommonCheckers.OrderRealtimeChecker.Services
{
    public abstract class UnitChecker<TOdrInf, TOdrDetail> : IUnitChecker<TOdrInf, TOdrDetail>
        where TOdrInf : class, IOdrInfModel<TOdrDetail>
        where TOdrDetail : class, IOdrInfDetailModel
    {
        public IHandler<TOdrInf, TOdrDetail> Handler;
        public RealtimeCheckerType CheckType;
        public IRealtimeCheckerFinder Finder;
        public int HpID;
        public long PtID;
        public int Sinday;

        public UnitCheckerResult<TOdrInf, TOdrDetail> CheckOrder(TOdrInf checkingOrder)
        {
            UnitCheckerResult<TOdrInf, TOdrDetail> unitCheckResult = new UnitCheckerResult<TOdrInf, TOdrDetail>(CheckType, checkingOrder, Sinday, PtID);
            unitCheckResult = HandleCheckOrder(unitCheckResult);
            return unitCheckResult;
        }

        public UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> CheckOrderList(List<TOdrInf> checkingOrderList)
        {
            UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> unitCheckForOrderListResult = new UnitCheckerForOrderListResult<TOdrInf, TOdrDetail>(CheckType, checkingOrderList, Sinday, PtID);
            unitCheckForOrderListResult = HandleCheckOrderList(unitCheckForOrderListResult);
            return unitCheckForOrderListResult;
        }

        // For this checking, dont need to show error message
        public UnitCheckerResult<TOdrInf, TOdrDetail> CheckOnlyOrder(TOdrInf checkingOrder)
        {
            UnitCheckerResult<TOdrInf, TOdrDetail> unitCheckResult = new UnitCheckerResult<TOdrInf, TOdrDetail>(CheckType, checkingOrder, Sinday, PtID);
            return HandleCheckOrder(unitCheckResult);
        }

        public abstract UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> HandleCheckOrderList(UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> unitCheckerForOrderListResult);

        public abstract UnitCheckerResult<TOdrInf, TOdrDetail> HandleCheckOrder(UnitCheckerResult<TOdrInf, TOdrDetail> unitCheckerResult);

        public List<string> GetAllOdrDetailCodeByOrderList(List<TOdrInf> orderList)
        {
            List<string> result = new List<string>();

            foreach (var order in orderList)
            {
                result.AddRange(GetAllOdrDetailCodeByOrder(order));
            }
            return result;
        }

        public List<string> GetAllOdrDetailCodeByOrder(TOdrInf order)
        {
            List<TOdrDetail> odrDetailList = order.OdrInfDetailModelsIgnoreEmpty.Where(o => o.YohoKbn == 0 && o.DrugKbn > 0).ToList();
            return odrDetailList.Select(o => o.ItemCd.Trim()).ToList();
        }
    }
}
