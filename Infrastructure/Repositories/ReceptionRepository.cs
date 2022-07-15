using Domain.CommonObject;
using Domain.Models.Reception;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ReceptionRepository : IReceptionRepository
    {
        private readonly TenantDataContext _tenantDataContext;
        public ReceptionRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetDataContext();
        }

        public ReceptionModel? Get(RaiinNo raiinNo)
        {
            var receptionEntity = _tenantDataContext.RaiinInfs.SingleOrDefault(r => r.RaiinNo == raiinNo.Value);

            if (receptionEntity == null)
            {
                return null;
            }

            return new ReceptionModel
                (
                    HpId.From(receptionEntity.HpId),
                    PtId.From(receptionEntity.PtId),
                    SinDate.From(receptionEntity.SinDate),
                    RaiinNo.From(receptionEntity.RaiinNo),
                    OyaRaiinNo.From(receptionEntity.OyaRaiinNo),
                    HokenPid.From(receptionEntity.HokenPid),
                    receptionEntity.SanteiKbn,
                    receptionEntity.Status,
                    receptionEntity.IsYoyaku,
                    receptionEntity.YoyakuTime,
                    receptionEntity.YoyakuId,
                    receptionEntity.UketukeSbt,
                    receptionEntity.UketukeTime,
                    receptionEntity.UketukeId,
                    receptionEntity.UketukeNo,
                    receptionEntity.SinStartTime,
                    receptionEntity.SinEndTime,
                    receptionEntity.KaikeiTime,
                    receptionEntity.KaikeiId,
                    receptionEntity.KaId,
                    receptionEntity.TantoId,
                    receptionEntity.SyosaisinKbn,
                    receptionEntity.JikanKbn
                );
        }
    }
}
