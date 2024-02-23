﻿using CommonChecker.Models.Futan;
using CommonChecker.Models.MstItem;
using Domain.Common;

namespace CommonCheckers.OrderRealtimeChecker.DB
{
    public interface IMasterFinder : IRepositoryBase
    {
        SanteiGrpDetailModel FindSanteiGrpDetail(int hpId, string itemCd);
        SanteiCntCheckModel FindSanteiCntCheck(int hpId, int santeiGrpCd, int sinDate);
        double GetOdrCountInMonth(int hpId, long ptId, int sinDate, string itemCd);
        TenMstModel FindTenMst(int hpId, string itemCd, int sinDate);
        IpnNameMstModel FindIpnNameMst(int hpId, string ipnNameCd, int sinDate);
    }
}
