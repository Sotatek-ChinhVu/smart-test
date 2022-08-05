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
        private const string FREE_WORD = "0000999";
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        public DiseaseRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public List<PtDiseaseModel>  GetPatientDiseaseList(int hpId, long ptId, int sinDate, int hokenId, DiseaseViewType openFrom)
        {
            var ptByomeiListQueryable = _tenantDataContext.PtByomeis
                .Where(p => p.HpId == hpId &&
                            p.PtId == ptId &&
                            p.IsDeleted != 1 &&
                            (openFrom != DiseaseViewType.FromReception || p.TenkiKbn == TenkiKbnConst.Continued || (p.StartDate <= sinDate && p.TenkiDate >= sinDate)));

            var ptByomeiList = ptByomeiListQueryable.ToList();

            var byomeiMstQuery = _tenantDataContext.ByomeiMsts.Where(b => b.HpId == hpId)
                                                             .Select(item => new { item.HpId, item.ByomeiCd, item.Sbyomei, item.Icd101, item.Icd102, item.Icd1012013, item.Icd1022013 });

            var byomeiMstList = (from ptByomei in ptByomeiListQueryable
                                 join ptByomeiMst in byomeiMstQuery on new { ptByomei.HpId, ptByomei.ByomeiCd } equals new { ptByomeiMst.HpId, ptByomeiMst.ByomeiCd }
                                 select ptByomeiMst).ToList();

            List<PtDiseaseModel> result = new List<PtDiseaseModel>();
            foreach (var ptByomei in ptByomeiList)
            {
                var byomeiMst = byomeiMstList.FirstOrDefault(item => item.ByomeiCd == ptByomei.ByomeiCd);

                string byomeiName = string.Empty;
                string icd10 = string.Empty;
                string icd102013 = string.Empty;
                string icd1012013 = string.Empty;
                string icd1022013 = string.Empty;

                if (ptByomei.ByomeiCd != null && ptByomei.ByomeiCd.Equals(FREE_WORD))
                {
                    byomeiName = ptByomei.Byomei ?? string.Empty;
                }
                else
                {
                    if (byomeiMst != null)
                    {
                        byomeiName = byomeiMst.Sbyomei;

                        icd10 = byomeiMst.Icd101;
                        if (!string.IsNullOrEmpty(byomeiMst.Icd102))
                        {
                            icd10 += "/" + byomeiMst.Icd102;
                        }
                        icd102013 = byomeiMst.Icd1012013;
                        if (!string.IsNullOrEmpty(byomeiMst.Icd1022013))
                        {
                            icd102013 += "/" + byomeiMst.Icd1022013;
                        }

                        icd1012013 = byomeiMst.Icd1012013;
                        icd1022013 = byomeiMst.Icd1022013;
                    }
                }
                PtDiseaseModel ptDiseaseModel = new(
                        ptByomei.HpId,
                        ptByomei.PtId,
                        ptByomei.SeqNo,
                        ptByomei.ByomeiCd ?? string.Empty,
                        ptByomei.SortNo,
                        SyusyokuCdToList(ptByomei),
                        byomeiName,
                        ptByomei.StartDate,
                        ptByomei.TenkiKbn,
                        ptByomei.TenkiDate,
                        ptByomei.SyubyoKbn,
                        ptByomei.SikkanKbn,
                        ptByomei.NanByoCd,
                        ptByomei.IsNodspRece,
                        ptByomei.IsNodspKarte,
                        ptByomei.IsDeleted,
                        ptByomei.Id,
                        ptByomei.IsImportant,
                        sinDate,
                        icd10,
                        icd102013,
                        icd1012013,
                        icd1022013,
                        ptByomei.HokenPid);
                result.Add(ptDiseaseModel);
            }
            return result;
        }

        private List<string> SyusyokuCdToList(PtByomei ptByomei)
        {
            return new List<string>()
            {
                ptByomei.SyusyokuCd1 ?? string.Empty,
                ptByomei.SyusyokuCd2 ?? string.Empty,
                ptByomei.SyusyokuCd3 ?? string.Empty,
                ptByomei.SyusyokuCd4 ?? string.Empty,
                ptByomei.SyusyokuCd5 ?? string.Empty,
                ptByomei.SyusyokuCd6 ?? string.Empty,
                ptByomei.SyusyokuCd7 ?? string.Empty,
                ptByomei.SyusyokuCd8 ?? string.Empty,
                ptByomei.SyusyokuCd9 ?? string.Empty,
                ptByomei.SyusyokuCd10 ?? string.Empty,
                ptByomei.SyusyokuCd11 ?? string.Empty,
                ptByomei.SyusyokuCd12 ?? string.Empty,
                ptByomei.SyusyokuCd13 ?? string.Empty,
                ptByomei.SyusyokuCd14 ?? string.Empty,
                ptByomei.SyusyokuCd15 ?? string.Empty,
                ptByomei.SyusyokuCd16 ?? string.Empty,
                ptByomei.SyusyokuCd17 ?? string.Empty,
                ptByomei.SyusyokuCd18 ?? string.Empty,
                ptByomei.SyusyokuCd19 ?? string.Empty,
                ptByomei.SyusyokuCd20 ?? string.Empty,
                ptByomei.SyusyokuCd21 ?? string.Empty
            };
        }
    }
}
