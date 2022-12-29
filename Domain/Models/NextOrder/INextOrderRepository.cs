﻿using Domain.Common;
using Domain.Models.OrdInfs;
using Domain.Models.RaiinKbn;
using Domain.Models.RaiinKubunMst;

namespace Domain.Models.NextOrder
{
    public interface INextOrderRepository : IRepositoryBase
    {
        List<RsvkrtByomeiModel> GetByomeis(int hpId, long ptId, long rsvkrtNo, int type);

        RsvkrtKarteInfModel GetKarteInf(int hpId, long ptId, long rsvkrtNo);

        List<RsvkrtOrderInfModel> GetOrderInfs(int hpId, long ptId, long rsvkrtNo, int sinDate, int userId);

        List<NextOrderFileInfModel> GetNextOrderFiles(int hpId, long ptId, long rsvkrtNo);

        long GetLastNextOrderSeqNo(int hpId, long ptId);

        List<NextOrderModel> GetList(int hpId, long ptId, int rsvkrtKbn, bool isDeleted);

        long Upsert(int userId, int hpId, long ptId, List<NextOrderModel> nextOrderModels);

        List<RsvkrtOrderInfModel> GetCheckOrderInfs(int hpId, long ptId);

        bool SaveListFileNextOrder(int hpId, long ptId, long rsvkrtNo, string host, List<NextOrderFileInfModel> listFiles, bool saveTempFile);

        bool ClearTempData(int hpId, long ptId, List<string> listFileNames);

        List<RaiinKbnModel> InitDefaultByTodayOrder(List<RaiinKbnModel> raiinKbns, List<(int grpId, int kbnCd, int kouiKbn1, int kouiKbn2)> raiinKouiKbns, List<RaiinKbnItemModel> raiinKbnItemCds, List<OrdInfModel> todayOrds);
    }
}
