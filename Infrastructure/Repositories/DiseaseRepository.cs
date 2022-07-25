using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.Diseases;
using PostgreDataContext;
using Infrastructure.Interfaces;
using Domain.Enum;
using Domain.Constant;
using Entity.Tenant;

namespace Infrastructure.Repositories
{
    public class DiseaseRepository : IPtDiseaseRepository
    {
        private readonly TenantDataContext _tenantDataContext;
        public DiseaseRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetDataContext();
        }

        public IEnumerable<PtDiseaseModel> GetAllDiseaseInMonth(int hpId, long ptId, int sinDate, int hokenId, DiseaseViewType openFrom)
        {
            return _tenantDataContext.PtByomeis
                .Where(b => b.HpId == hpId &&
                            b.PtId == ptId &&
                            b.IsDeleted != DeleteTypes.Deleted &&
                            b.IsNodspKarte != 1 &&
                            (b.TenkiKbn == TenkiKbnConst.Continued ||
                            b.StartDate <= sinDate && b.TenkiDate >= sinDate ||
                            b.StartDate <= (sinDate / 100 * 100 + 31) && b.TenkiDate >= (sinDate / 100 * 100 + 1)) &&
                            (b.HokenPid == 0 || b.HokenPid == hokenId))
                .OrderBy(b => b.TenkiKbn)
                .ThenByDescending(b => b.IsImportant)
                .ThenBy(b => b.SortNo)
                .ThenBy(b => b.Id)
                .Select(b => new PtDiseaseModel(
                        b.HpId,
                        b.PtId,
                        b.SeqNo,
                        b.ByomeiCd ?? string.Empty,
                        b.SortNo,
                        b.SyusyokuCd1 ?? string.Empty,
                        b.SyusyokuCd2 ?? string.Empty,
                        b.SyusyokuCd3 ?? string.Empty,
                        b.SyusyokuCd4 ?? string.Empty,
                        b.SyusyokuCd5 ?? string.Empty,
                        b.SyusyokuCd6 ?? string.Empty,
                        b.SyusyokuCd7 ?? string.Empty,
                        b.SyusyokuCd8 ?? string.Empty,
                        b.SyusyokuCd9 ?? string.Empty,
                        b.SyusyokuCd10 ?? string.Empty,
                        b.SyusyokuCd11 ?? string.Empty,
                        b.SyusyokuCd12 ?? string.Empty,
                        b.SyusyokuCd13 ?? string.Empty,
                        b.SyusyokuCd14 ?? string.Empty,
                        b.SyusyokuCd15 ?? string.Empty,
                        b.SyusyokuCd16 ?? string.Empty,
                        b.SyusyokuCd17 ?? string.Empty,
                        b.SyusyokuCd18 ?? string.Empty,
                        b.SyusyokuCd19 ?? string.Empty,
                        b.SyusyokuCd20 ?? string.Empty,
                        b.SyusyokuCd21 ?? string.Empty,
                        b.Byomei ?? string.Empty,
                        b.StartDate,
                        b.TenkiKbn,
                        b.TenkiDate,
                        b.SyubyoKbn,
                        b.SikkanKbn,
                        b.NanByoCd,
                        b.IsNodspRece,
                        b.IsNodspKarte,
                        b.IsDeleted,
                        b.Id,
                        b.IsImportant,
                        sinDate
                        ));
        }
    }
}
