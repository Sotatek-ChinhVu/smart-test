﻿using Domain.Models.KarteInfs;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System.Text;

namespace Infrastructure.Repositories
{
    public class KarteInfRepository : IKarteInfRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        public KarteInfRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public List<KarteInfModel> GetList(long ptId, long rainNo, long sinDate, bool isDeleted)
        {
            var karteInfEntity = _tenantDataContext.KarteInfs.Where(k => k.PtId == ptId && k.RaiinNo == rainNo && k.SinDate == sinDate && (isDeleted || k.IsDeleted == 0));

            if (karteInfEntity == null)
            {
                return new List<KarteInfModel>();
            }

            return karteInfEntity.Select(k =>
                    new KarteInfModel(
                        k.HpId,
                        k.RaiinNo,
                        k.KarteKbn,
                        k.SeqNo,
                        k.PtId,
                        k.SinDate,
                        k.Text,
                        k.IsDeleted,
                        k.RichText == null ? string.Empty : Encoding.UTF8.GetString(k.RichText),
                        k.CreateDate,
                        k.UpdateDate
                    )
                  ).ToList();
        }

        public List<KarteInfModel> GetList(long ptId, int hpId)
        {
            var karteInfEntity = _tenantDataContext.KarteInfs.Where(k => k.PtId == ptId && k.HpId == hpId);

            if (karteInfEntity == null)
            {
                return new List<KarteInfModel>();
            }

            return karteInfEntity.Select(k =>
                    new KarteInfModel(
                        k.HpId,
                        k.RaiinNo,
                        k.KarteKbn,
                        k.SeqNo,
                        k.PtId,
                        k.SinDate,
                        k.Text,
                        k.IsDeleted,
                        k.RichText == null ? string.Empty : Encoding.UTF8.GetString(k.RichText),
                        k.CreateDate,
                        k.UpdateDate
                    )
                  ).ToList();
        }
    }
}
