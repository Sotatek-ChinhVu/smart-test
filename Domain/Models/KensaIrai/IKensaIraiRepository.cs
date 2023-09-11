﻿using Domain.Common;

namespace Domain.Models.KensaIrai;

public interface IKensaIraiRepository : IRepositoryBase
{
    KensaCenterMstModel GetKensaCenterMst(int hpId, string centerCd);

    List<KensaInfModel> GetKensaInf(int hpId, long ptId, long raiinNo, string centerCd);

    List<KensaInfDetailModel> GetKensaInfDetail(int hpId, long ptId, long raiinNo, string centerCd);

    bool SaveKensaInf(int hpId, int userId, List<KensaInfModel> kensaInfModels, List<KensaInfDetailModel> kensaInfDetailModels);

    List<KensaIraiModel> GetKensaIraiModels(int hpId, long ptId, int startDate, int endDate, string kensaCenterMstCenterCd, int kensaCenterMstPrimaryKbn);

    bool CreateDataKensaIraiRenkei(int hpId, int userId, List<KensaIraiModel> kensaIraiList, string centerCd, int systemDate);
}
