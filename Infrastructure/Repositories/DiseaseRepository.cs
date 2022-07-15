using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.Diseases;
using Domain.CommonObject;
using PostgreDataContext;
using Infrastructure.Interfaces;
using Domain.Enum;
using Domain.Constant;

namespace Infrastructure.Repositories
{
    public class DiseaseRepository : IDiseaseRepository
    {
        private readonly TenantDataContext _tenantDataContext;
        public DiseaseRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetDataContext();
        }

        public IEnumerable<Disease> GetAll(HpId hpId, PtId ptId, SinDate sinDate, DiseaseViewType openFrom)
        {
            return _tenantDataContext.PtByomeis.Where(p =>  p.HpId == hpId.Value && 
                                                            p.PtId == ptId.Value && 
                                                            p.IsDeleted != 1 &&
                                                            (openFrom != DiseaseViewType.FromReception || 
                                                            p.TenkiKbn == TenkiKbnConst.Continued ||
                                                            p.StartDate <= sinDate.Value && p.TenkiDate >= sinDate.Value))
                .Select(u => new Disease(
                    u.HpId,
                    u.PtId,
                    u.SeqNo,
                    u.ByomeiCd,
                    u.SortNo,
                    u.SyusyokuCd1,
                    u.SyusyokuCd2,
                    u.SyusyokuCd3,
                    u.SyusyokuCd4,
                    u.SyusyokuCd5,
                    u.SyusyokuCd6,
                    u.SyusyokuCd7,
                    u.SyusyokuCd8,
                    u.SyusyokuCd9,
                    u.SyusyokuCd10,
                    u.SyusyokuCd11,
                    u.SyusyokuCd12,
                    u.SyusyokuCd13,
                    u.SyusyokuCd14,
                    u.SyusyokuCd15,
                    u.SyusyokuCd16,
                    u.SyusyokuCd17,
                    u.SyusyokuCd18,
                    u.SyusyokuCd19,
                    u.SyusyokuCd20,
                    u.SyusyokuCd21,
                    u.Byomei,
                    u.StartDate,
                    u.TenkiKbn,
                    u.TenkiDate,
                    u.SyubyoKbn,
                    u.SikkanKbn,
                    u.NanByoCd,
                    u.HosokuCmt,
                    u.HokenPid,
                    u.TogetuByomei,
                    u.IsNodspRece,
                    u.IsNodspKarte,
                    u.IsDeleted,
                    u.Id,
                    u.IsImportant,
                    sinDate.Value
                    ));
        }
    }
}
