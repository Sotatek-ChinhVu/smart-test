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
                .Select (b => new PtDiseaseModel(
                        b.HpId,
                        b.PtId,
                        b.SeqNo,
                        b.ByomeiCd,
                        b.SortNo,
                        b.SyusyokuCd1,
                        b.SyusyokuCd2,
                        b.SyusyokuCd3,
                        b.SyusyokuCd4,
                        b.SyusyokuCd5,
                        b.SyusyokuCd6,
                        b.SyusyokuCd7,
                        b.SyusyokuCd8,
                        b.SyusyokuCd9,
                        b.SyusyokuCd10,
                        b.SyusyokuCd11,
                        b.SyusyokuCd12,
                        b.SyusyokuCd13,
                        b.SyusyokuCd14,
                        b.SyusyokuCd15,
                        b.SyusyokuCd16,
                        b.SyusyokuCd17,
                        b.SyusyokuCd18,
                        b.SyusyokuCd19,
                        b.SyusyokuCd20,
                        b.SyusyokuCd21,
                        b.Byomei,
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
