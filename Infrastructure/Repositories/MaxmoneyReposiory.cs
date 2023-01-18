using Domain.Models.MaxMoney;
using Entity.Tenant;
using Helper.Common;
using Helper.Extension;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class MaxmoneyReposiory : RepositoryBase, IMaxmoneyReposiory
    {
        public MaxmoneyReposiory(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public List<LimitListModel> GetListLimitModel(long ptId, int hpId)
        {
            IEnumerable<LimitListInf> maxMoneys = NoTrackingDataContext.LimitListInfs.Where(u => u.HpId == hpId
                                                                   && u.PtId == ptId
                                                                   && u.IsDeleted == 0)
                                                                   .OrderBy(u => u.SortKey)
                                                                   .ToList();
            return maxMoneys.Select(u => new LimitListModel(u.Id, u.KohiId, u.SinDate, u.HokenPid, u.SortKey ?? string.Empty, u.RaiinNo, u.FutanGaku, u.TotalGaku, u.Biko ?? string.Empty, u.IsDeleted, u.SeqNo)).ToList();
        }

        public MaxMoneyInfoHokenModel GetInfoHokenMoney(int hpId, long ptId, int kohiId, int sinYm)
        {
            var kohi = NoTrackingDataContext.PtKohis.FirstOrDefault(x => x.HpId == hpId
                                                                && x.PtId == ptId
                                                                && x.HokenId == kohiId);

            if (kohi is null) return new MaxMoneyInfoHokenModel(0, 0, 0, 0, 0, 0, string.Empty, string.Empty, 0, 0, 0, 0);

            var hokenMst = NoTrackingDataContext.HokenMsts.FirstOrDefault(x => x.HpId == hpId
                                                                && x.HokenNo == kohi.HokenNo
                                                                && x.HokenEdaNo == kohi.HokenEdaNo);

            if (hokenMst is null) return new MaxMoneyInfoHokenModel(0, 0, 0, 0, 0, 0, string.Empty, string.Empty, 0, 0, 0, 0);

            int limitFutan = 0;
            if (hokenMst.KaiLimitFutan > 0)
                limitFutan = hokenMst.KaiLimitFutan;
            else if (hokenMst.DayLimitFutan > 0)
                limitFutan = hokenMst.DayLimitFutan;
            else if (hokenMst.MonthLimitFutan > 0)
                limitFutan = hokenMst.MonthLimitFutan;

            return new MaxMoneyInfoHokenModel(kohi.HokenId,
                                                kohi.Rate,
                                                sinYm,
                                                hokenMst.FutanKbn,
                                                hokenMst.MonthLimitFutan,
                                                kohi.GendoGaku,
                                                hokenMst.Houbetu ?? string.Empty,
                                                hokenMst.HokenName ?? string.Empty,
                                                hokenMst.IsLimitListSum,
                                                hokenMst.IsLimitList,
                                                hokenMst.FutanRate,
                                                limitFutan);
        }

        public bool SaveMaxMoney(List<LimitListModel> dataInputs, int hpId, long ptId, int kohiId, int sinYm, int userId)
        {
            List<LimitListInf> maxMoneyDatabases = TrackingDataContext.LimitListInfs.Where(x => x.HpId == hpId
                                                                   && x.PtId == ptId
                                                                   && x.KohiId == kohiId
                                                                   && x.IsDeleted == 0)
                                                                   .OrderBy(u => u.SortKey)
                                                                   .ToList();

            foreach (var item in maxMoneyDatabases)
            {
                var exist = dataInputs.FirstOrDefault(x => x.SeqNo == item.SeqNo && x.Id == item.Id);
                if (exist == null)
                {
                    if (CIUtil.Copy(item.SinDate.AsString(), 1, 6).AsInteger() == sinYm)
                    {
                        item.IsDeleted = 1;
                        item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        item.UpdateId = userId;
                    }
                }
            }

            foreach (var item in dataInputs)
            {
                if (item.SeqNo == 0 && item.Id == 0)
                {
                    LimitListInf create = new LimitListInf()
                    {
                        PtId = ptId,
                        HpId = hpId,
                        HokenPid = item.HokenPid,
                        SinDate = item.SinDateY * 10000 + item.SinDateM * 100 + item.SinDateD,
                        SortKey = item.SortKey,
                        RaiinNo = item.RaiinNo,
                        FutanGaku = item.FutanGaku,
                        KohiId = item.KohiId,
                        TotalGaku = item.TotalGaku,
                        Biko = item.Biko,
                        IsDeleted = 0,
                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                        UpdateDate = CIUtil.GetJapanDateTimeNow(),
                        UpdateId = userId,
                        CreateId = userId
                    };
                    TrackingDataContext.LimitListInfs.Add(create);
                }
                else
                {
                    LimitListInf? update = maxMoneyDatabases.FirstOrDefault(x => x.Id == item.Id && x.SeqNo == item.SeqNo);
                    if (update != null)
                    {
                        update.SortKey = item.SortKey;
                        update.FutanGaku = item.FutanGaku;
                        update.TotalGaku = item.TotalGaku;
                        update.Biko = item.Biko;
                        update.SinDate = item.SinDateY * 10000 + item.SinDateM * 100 + item.SinDateD;
                        update.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        update.UpdateId = userId;
                    }
                }
            };

            return TrackingDataContext.SaveChanges() > 0;
        }

        public List<MaxMoneyInfoHokenModel> GetListInfoHokenMoneys(int hpId, long ptId, IEnumerable<int> kohiIds, int sinYm)
        {
            var result = new List<MaxMoneyInfoHokenModel>();
            var kohis = NoTrackingDataContext.PtKohis.Where(x => x.HpId == hpId
                                                                && x.PtId == ptId
                                                                && kohiIds.Contains(x.HokenId)).ToList();

            if (kohis is null || !kohis.Any())
                return result;

            var hokenNoIds = kohis.Select(x => x.HokenNo);
            var hokenEdaNoIds = kohis.Select(x => x.HokenEdaNo);

            var hokenMsts = NoTrackingDataContext.HokenMsts.Where(x => x.HpId == hpId
                                                                && hokenNoIds.Contains(x.HokenNo)
                                                                && hokenEdaNoIds.Contains(x.HokenEdaNo)).ToList();

            foreach(PtKohi kohi in kohis)
            {
                var hokenMst = hokenMsts.FirstOrDefault(x => x.HokenNo == kohi.HokenNo
                                                               && x.HokenEdaNo == kohi.HokenEdaNo);

                if (hokenMst is null)
                    continue;

                int limitFutan = 0;
                if (hokenMst.KaiLimitFutan > 0)
                    limitFutan = hokenMst.KaiLimitFutan;
                else if (hokenMst.DayLimitFutan > 0)
                    limitFutan = hokenMst.DayLimitFutan;
                else if (hokenMst.MonthLimitFutan > 0)
                    limitFutan = hokenMst.MonthLimitFutan;

                result.Add(new MaxMoneyInfoHokenModel(kohi.HokenId,
                                                kohi.Rate,
                                                sinYm,
                                                hokenMst.FutanKbn,
                                                hokenMst.MonthLimitFutan,
                                                kohi.GendoGaku,
                                                hokenMst.Houbetu ?? string.Empty,
                                                hokenMst.HokenName ?? string.Empty,
                                                hokenMst.IsLimitListSum,
                                                hokenMst.IsLimitList,
                                                hokenMst.FutanRate,
                                                limitFutan));
            }
            return result;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
