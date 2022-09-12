﻿using Domain.Models.InputItem;
using Domain.Models.OrdInf;

namespace Domain.Models.OrdInfs
{
    public interface IOrdInfRepository
    {
        void Create(OrdInfModel ord);

        OrdInfModel Read(int ordId);

        void Update(OrdInfModel ord);

        void Delete(int ordId);

        bool CheckExistOrder(long rpNo, long rpEdaNo);

        bool CheckIsGetYakkaPrice(int hpId, InputItemModel? tenMst, int sinDate);

        IpnMinYakkaMstModel FindIpnMinYakkaMst(int hpId, string ipnNameCd, int sinDate);

        IEnumerable<OrdInfModel> GetList(int hpId, long ptId, long raiinNo, int sinDate, bool isDeleted);

        IEnumerable<OrdInfModel> GetList(long ptId, int hpId, int deleteCondition, List<long> raiinNos);
    }
}
