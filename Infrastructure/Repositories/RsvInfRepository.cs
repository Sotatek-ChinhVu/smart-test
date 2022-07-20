using Domain.Models.RsvInfo;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class RsvInfRepository : IRsvInfRepository
    {
        private readonly TenantDataContext _tenantDataContext;
        public RsvInfRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetDataContext();
        }
        public RsvInfModel GetRsvInfModel(long ptId, int sinDate)
        {
            var rsvInfEntity = _tenantDataContext.RsvInfs.SingleOrDefault(p => p.PtId == ptId && p.SinDate >= sinDate);
            if (rsvInfEntity == null) return null;
            else return new RsvInfModel(
                rsvInfEntity.HpId,
                rsvInfEntity.RsvFrameId,
                rsvInfEntity.SinDate,
                rsvInfEntity.StartTime,
                rsvInfEntity.RaiinNo,
                rsvInfEntity.PtId,
                rsvInfEntity.RsvSbt,
                rsvInfEntity.TantoId,
                rsvInfEntity.KaId,
                rsvInfEntity.CreateDate,
                rsvInfEntity.CreateId,
                rsvInfEntity.CreateMachine,
                rsvInfEntity.UpdateDate,
                rsvInfEntity.UpdateId,
                rsvInfEntity.UpdateMachine
                );
        }
    }
}
