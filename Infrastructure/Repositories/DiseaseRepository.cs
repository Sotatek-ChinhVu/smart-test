using Domain.Constant;
using Domain.Enum;
using Domain.Models.Diseases;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class DiseaseRepository : IPtDiseaseRepository
    {
        private const string FREE_WORD = "0000999";
        private readonly TenantNoTrackingDataContext _tenantNoTrackingDataContext;
        private readonly TenantDataContext _tenantTrackingDataContext;
        public DiseaseRepository(ITenantProvider tenantProvider)
        {
            _tenantNoTrackingDataContext = tenantProvider.GetNoTrackingDataContext();
            _tenantTrackingDataContext = tenantProvider.GetTrackingTenantDataContext();
        }

        public List<PtDiseaseModel> GetPatientDiseaseList(int hpId, long ptId, int sinDate, int hokenId, DiseaseViewType openFrom)
        {
            var ptByomeiListQueryable = _tenantNoTrackingDataContext.PtByomeis
                .Where(p => p.HpId == hpId &&
                            p.PtId == ptId &&
                            p.IsDeleted != 1 &&
                            (openFrom != DiseaseViewType.FromReception || p.TenkiKbn == TenkiKbnConst.Continued || (p.StartDate <= sinDate && p.TenkiDate >= sinDate)));

            var ptByomeiList = ptByomeiListQueryable.ToList();

            var byomeiMstQuery = _tenantNoTrackingDataContext.ByomeiMsts.Where(b => b.HpId == hpId)
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
                        ptByomei.HokenPid,
                        ptByomei.HosokuCmt ?? string.Empty
                        );
                result.Add(ptDiseaseModel);
            }
            return result;
        }

        public void Upsert(List<PtDiseaseModel> inputDatas)
        {
            foreach (var inputData in inputDatas)
            {
                if (inputData.IsDeleted == DeleteTypes.Deleted)
                {
                    var ptByomei = _tenantTrackingDataContext.PtByomeis.FirstOrDefault(p => p.HpId == inputData.HpId && p.PtId == inputData.PtId && p.Id == inputData.Id);
                    if (ptByomei != null)
                    {
                        ptByomei.IsDeleted = DeleteTypes.Deleted;
                    }
                }
                else
                {
                    var ptByomei = _tenantTrackingDataContext.PtByomeis.FirstOrDefault(p => p.HpId == inputData.HpId && p.PtId == inputData.PtId && p.Id == inputData.Id);

                    if (ptByomei != null)
                    {
                        _tenantTrackingDataContext.PtByomeis.Add(ConvertFromModelToPtByomei(inputData));

                        ptByomei.IsDeleted = DeleteTypes.Deleted;
                        ptByomei.UpdateId = TempIdentity.UserId;
                        ptByomei.UpdateDate = DateTime.UtcNow;
                        ptByomei.UpdateMachine = TempIdentity.ComputerName;
                    }
                    else
                    {
                        _tenantTrackingDataContext.PtByomeis.Add(ConvertFromModelToPtByomei(inputData));
                    }
                }
            }

            _tenantTrackingDataContext.SaveChanges();
        }

        private PtByomei ConvertFromModelToPtByomei(PtDiseaseModel model)
        {
            var syusyokuCd = model.SyusyokuCd;

            return new PtByomei
            {
                HpId = model.HpId,
                PtId = model.PtId,
                ByomeiCd = model.ByomeiCd,
                SortNo = model.SortNo,
                SyusyokuCd1 = syusyokuCd == null ? String.Empty : syusyokuCd["SyusyokuCd1"],
                SyusyokuCd2 = syusyokuCd == null ? String.Empty : syusyokuCd["SyusyokuCd2"],
                SyusyokuCd3 = syusyokuCd == null ? String.Empty : syusyokuCd["SyusyokuCd3"],
                SyusyokuCd4 = syusyokuCd == null ? String.Empty : syusyokuCd["SyusyokuCd4"],
                SyusyokuCd5 = syusyokuCd == null ? String.Empty : syusyokuCd["SyusyokuCd5"],
                SyusyokuCd6 = syusyokuCd == null ? String.Empty : syusyokuCd["SyusyokuCd6"],
                SyusyokuCd7 = syusyokuCd == null ? String.Empty : syusyokuCd["SyusyokuCd7"],
                SyusyokuCd8 = syusyokuCd == null ? String.Empty : syusyokuCd["SyusyokuCd8"],
                SyusyokuCd9 = syusyokuCd == null ? String.Empty : syusyokuCd["SyusyokuCd9"],
                SyusyokuCd10 = syusyokuCd == null ? String.Empty : syusyokuCd["SyusyokuCd10"],
                SyusyokuCd11 = syusyokuCd == null ? String.Empty : syusyokuCd["SyusyokuCd11"],
                SyusyokuCd12 = syusyokuCd == null ? String.Empty : syusyokuCd["SyusyokuCd12"],
                SyusyokuCd13 = syusyokuCd == null ? String.Empty : syusyokuCd["SyusyokuCd13"],
                SyusyokuCd14 = syusyokuCd == null ? String.Empty : syusyokuCd["SyusyokuCd14"],
                SyusyokuCd15 = syusyokuCd == null ? String.Empty : syusyokuCd["SyusyokuCd15"],
                SyusyokuCd16 = syusyokuCd == null ? String.Empty : syusyokuCd["SyusyokuCd16"],
                SyusyokuCd17 = syusyokuCd == null ? String.Empty : syusyokuCd["SyusyokuCd17"],
                SyusyokuCd18 = syusyokuCd == null ? String.Empty : syusyokuCd["SyusyokuCd18"],
                SyusyokuCd19 = syusyokuCd == null ? String.Empty : syusyokuCd["SyusyokuCd19"],
                SyusyokuCd20 = syusyokuCd == null ? String.Empty : syusyokuCd["SyusyokuCd20"],
                SyusyokuCd21 = syusyokuCd == null ? String.Empty : syusyokuCd["SyusyokuCd21"],
                Byomei = model.Byomei,
                StartDate = model.StartDate,
                TenkiKbn = model.TenkiKbn == TenkiKbnConst.InMonth ? TenkiKbnConst.Cured : model.TenkiKbn,
                TenkiDate = model.TenkiDate,
                SyubyoKbn = model.SyubyoKbn,
                SikkanKbn = model.SikkanKbn,
                NanByoCd = model.NanbyoCd,
                HosokuCmt = model.HosokuCmt,
                HokenPid = model.HokenPid,
                TogetuByomei = model.TenkiKbn == TenkiKbnConst.InMonth ? 1 : 0,
                IsNodspRece = model.IsNodspRece,
                IsNodspKarte = model.IsNodspKarte,
                CreateId = TempIdentity.UserId,
                CreateMachine = TempIdentity.ComputerName,
                CreateDate = DateTime.UtcNow,
                SeqNo = model.SeqNo,
                IsImportant = model.IsImportant,
                UpdateId = TempIdentity.UserId,
                UpdateDate = DateTime.UtcNow,
                UpdateMachine = TempIdentity.ComputerName
            };
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
