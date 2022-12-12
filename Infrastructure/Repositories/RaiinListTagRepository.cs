﻿using Domain.Models.RainListTag;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class RaiinListTagRepository : IRaiinListTagRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        public RaiinListTagRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public IEnumerable<RaiinListTagModel> GetList(int hpId, long ptId, bool isNoWithWhiteStar, List<int> sinDates, List<long> raiinNos)
        {
            var result = _tenantDataContext.RaiinListTags.Where(r => r.HpId == hpId && r.PtId == ptId && r.IsDeleted == 0 && (!isNoWithWhiteStar && r.TagNo != 0) && sinDates.Contains(r.SinDate) && raiinNos.Contains(r.RaiinNo));

            return result.Select(x => new RaiinListTagModel(
                    x.HpId,
                    x.PtId,
                    x.SinDate,
                    x.RaiinNo,
                    x.SeqNo,
                    x.TagNo,
                    x.IsDeleted
                ));
        }

        public IEnumerable<RaiinListTagModel> GetList(int hpId, long ptId, bool isNoWithWhiteStar)
        {
            var result = _tenantDataContext.RaiinListTags.Where(r => r.HpId == hpId && r.PtId == ptId && r.IsDeleted == 0 && (!isNoWithWhiteStar && r.TagNo != 0));

            return result.Select(x => new RaiinListTagModel(
                    x.HpId,
                    x.PtId,
                    x.SinDate,
                    x.RaiinNo,
                    x.SeqNo,
                    x.TagNo,
                    x.IsDeleted
                ));
        }
        
        public List<RaiinListTagModel> GetList(int hpId, long ptId, List<long> raiinNoList)
        {
            var result = _tenantDataContext.RaiinListTags.Where(r => r.HpId == hpId && r.PtId == ptId && r.IsDeleted == 0 && raiinNoList.Contains(r.RaiinNo));

            return result.Select(x => new RaiinListTagModel(
                    x.HpId,
                    x.PtId,
                    x.SinDate,
                    x.RaiinNo,
                    x.SeqNo,
                    x.TagNo,
                    x.IsDeleted
                )).ToList();
        }

        public RaiinListTagModel Get(int hpId, long ptId, long raiinNo, int sinDate)
        {
            var result = _tenantDataContext.RaiinListTags.FirstOrDefault(r => r.HpId == hpId && r.PtId == ptId && r.IsDeleted == 0 && r.SinDate == sinDate && r.RaiinNo == raiinNo);

            return new RaiinListTagModel(
                    result?.HpId ?? 0,
                    result?.PtId ?? 0,
                    result?.SinDate ?? 0,
                    result?.RaiinNo ?? 0,
                    result?.SeqNo ?? 0,
                    result?.TagNo ?? 0,
                    result?.IsDeleted ?? 0
                );
        }
    }
}
