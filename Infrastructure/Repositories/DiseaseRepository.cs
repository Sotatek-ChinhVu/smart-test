using Domain.Constant;
using Domain.Enum;
using Domain.Models.Diseases;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Infrastructure.Services;

namespace Infrastructure.Repositories
{
    public class DiseaseRepository : RepositoryBase, IPtDiseaseRepository
    {
        private const string FREE_WORD = "0000999";
        public DiseaseRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public List<PtDiseaseModel> GetListPatientDiseaseForReport(int hpId, long ptId, int hokenPid, int sinDate, bool tenkiByomei)
        {
            List<int> tenkiKbns = new List<int> { TenkiKbnConst.Continued };

            if (tenkiByomei)
            {
                tenkiKbns.AddRange(new List<int> { TenkiKbnConst.Cured, TenkiKbnConst.Dead, TenkiKbnConst.Canceled, TenkiKbnConst.Other });
            }

            var ptByomeiList = NoTrackingDataContext.PtByomeis.Where(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                (p.HokenPid == 0 || p.HokenPid == hokenPid) &&
                tenkiKbns.Contains(p.TenkiKbn) &&
                p.IsDeleted == DeleteStatus.None
            )
            .OrderBy(p => p.StartDate)
            .ThenBy(p => p.TenkiDate)
            .ThenBy(p => p.Byomei)
            .ThenBy(p => p.SyubyoKbn)
            .ToList();


            var byomeiMstQuery = NoTrackingDataContext.ByomeiMsts.Where(b => b.HpId == hpId)
                                                             .Select(item => new { item.HpId, item.ByomeiCd, item.Sbyomei, item.Icd101, item.Icd102, item.Icd1012013, item.Icd1022013 });

            var byomeiMstList = (from ptByomei in ptByomeiList
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
                PtDiseaseModel ptDiseaseModel = new PtDiseaseModel(
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
                        ptByomei.HosokuCmt ?? string.Empty,
                        ptByomei.TogetuByomei,
                        0,
                        ptByomei.HokenPid
                        );
                result.Add(ptDiseaseModel);
            }
            return result;

        }

        public List<PtDiseaseModel> GetPatientDiseaseList(int hpId, long ptId, int sinDate, int hokenId, DiseaseViewType openFrom, bool isContiFiltered, bool isInMonthFiltered)
        {
            IQueryable<PtByomei> ptByomeiListQueryable;
            if (openFrom == DiseaseViewType.FromReceiptCheck)
            {
                ptByomeiListQueryable = NoTrackingDataContext.PtByomeis.Where(p => p.HpId == hpId &&
                                                                              p.PtId == ptId &&
                                                                              p.IsDeleted != 1 &&
                                                                              (openFrom != DiseaseViewType.FromReception || p.TenkiKbn == TenkiKbnConst.Continued ||
                                                                              (p.StartDate <= sinDate && p.TenkiDate >= sinDate)));
            }
            else
            {
                ptByomeiListQueryable = NoTrackingDataContext.PtByomeis
                .Where(p => p.HpId == hpId &&
                            p.PtId == ptId &&
                            p.IsDeleted != 1 &&
                            ((openFrom != DiseaseViewType.FromReception && openFrom != DiseaseViewType.FromMedicalExamination) || p.TenkiKbn == TenkiKbnConst.Continued || (p.StartDate <= sinDate && p.TenkiDate >= sinDate)));
            }

            if (hokenId > 0)
            {
                ptByomeiListQueryable = ptByomeiListQueryable.Where(b => b.HokenPid == hokenId || b.HokenPid == 0);
            }

            if (isContiFiltered)
            {
                ptByomeiListQueryable = ptByomeiListQueryable.Where(x => x.TenkiKbn <= TenkiKbnConst.Continued);
            }
            else if (isInMonthFiltered)
            {
                ptByomeiListQueryable = ptByomeiListQueryable.Where(x => x.TenkiKbn <= TenkiKbnConst.Continued || (x.StartDate <= (sinDate / 100 * 100 + 31) && x.TenkiDate >= (sinDate / 100 * 100 + 1)));
            }

            var ptByomeiList = ptByomeiListQueryable.OrderBy(p => p.TenkiKbn)
                                                    .ThenBy(p => p.SortNo)
                                                    .ThenByDescending(p => p.StartDate)
                                                    .ThenByDescending(p => p.TenkiDate)
                                                    .ThenBy(p => p.Id).ToList();

            var byomeiMstQuery = NoTrackingDataContext.ByomeiMsts.Where(b => b.HpId == hpId)
                                                             .Select(item => new { item.HpId, item.ByomeiCd, item.Sbyomei, item.Icd101, item.Icd102, item.Icd1012013, item.Icd1022013 });

            var byomeiMstList = (from ptByomei in ptByomeiListQueryable
                                 join ptByomeiMst in byomeiMstQuery on new { ptByomei.HpId, ptByomei.ByomeiCd } equals new { ptByomeiMst.HpId, ptByomeiMst.ByomeiCd }
                                 select ptByomeiMst).ToList();

            List<PtDiseaseModel> result = new();
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
                var ptDiseaseModel = new PtDiseaseModel(
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
                        ptByomei.HosokuCmt ?? string.Empty,
                        ptByomei.TogetuByomei,
                        0,
                        ptByomei.HokenPid
                        );
                result.Add(ptDiseaseModel);
            }

            return result;
        }

        public List<ByomeiSetMstModel> GetDataTreeSetByomei(int hpId, int sinDate)
        {
            var genarationMst = NoTrackingDataContext.ByomeiSetGenerationMsts
                                         .Where(p => p.IsDeleted == DeleteTypes.None)
                                         .OrderByDescending(p => p.StartDate)
                                         .FirstOrDefault(q => q.StartDate <= sinDate);

            if (genarationMst == null) return new List<ByomeiSetMstModel>();

            var byomeiSetMst = NoTrackingDataContext.ByomeiSetMsts
                                        .Where(p => p.HpId == hpId &&
                                               p.IsDeleted == DeleteTypes.None &&
                                               p.GenerationId == genarationMst.GenerationId);

            var byomeiMsts = NoTrackingDataContext.ByomeiMsts.Where(p => p.HpId == hpId);


            var query = from byomeiSet in byomeiSetMst
                        join byomeiMst in byomeiMsts
                        on byomeiSet.ByomeiCd equals byomeiMst.ByomeiCd into ps
                        from p in ps.DefaultIfEmpty()
                        select new { byomeiSet = byomeiSet, byomeiMst = p };

            return query.Select(x => new ByomeiSetMstModel(x.byomeiSet.GenerationId,
                                                                x.byomeiSet.SeqNo,
                                                                x.byomeiSet.Level1,
                                                                x.byomeiSet.Level2,
                                                                x.byomeiSet.Level3,
                                                                x.byomeiSet.Level4,
                                                                x.byomeiSet.Level5,
                                                                x.byomeiSet.ByomeiCd ?? string.Empty,
                                                                x.byomeiMst.Byomei ?? string.Empty,
                                                                x.byomeiMst.Icd101 ?? string.Empty,
                                                                x.byomeiMst.Icd102 ?? string.Empty,
                                                                x.byomeiMst.Icd1012013 ?? string.Empty,
                                                                x.byomeiMst.Icd1022013 ?? string.Empty,
                                                                x.byomeiSet.SetName ?? string.Empty,
                                                                x.byomeiSet.IsTitle,
                                                                x.byomeiSet.SelectType)).ToList();
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        public List<long> Upsert(List<PtDiseaseModel> inputDatas, int hpId, int userId)
        {
            var byomeis = new List<PtByomei>();
            foreach (var inputData in inputDatas)
            {
                if (inputData.IsDeleted == DeleteTypes.Deleted)
                {
                    var ptByomei = TrackingDataContext.PtByomeis.FirstOrDefault(p => p.HpId == inputData.HpId && p.PtId == inputData.PtId && p.Id == inputData.Id);
                    if (ptByomei != null)
                    {
                        ptByomei.IsDeleted = DeleteTypes.Deleted;
                    }
                }
                else
                {
                    var ptByomei = TrackingDataContext.PtByomeis.FirstOrDefault(p => p.HpId == inputData.HpId && p.PtId == inputData.PtId && p.Id == inputData.Id);
                    PtByomei byomei;

                    if (ptByomei != null)
                    {
                        byomei = ConvertFromModelToPtByomei(inputData, hpId, userId);
                        TrackingDataContext.PtByomeis.Add(byomei);

                        ptByomei.IsDeleted = DeleteTypes.Deleted;
                        ptByomei.UpdateId = userId;
                        ptByomei.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    }
                    else
                    {
                        byomei = ConvertFromModelToPtByomei(inputData, hpId, userId);
                        TrackingDataContext.PtByomeis.Add(byomei);
                    }

                    byomeis.Add(byomei);
                }
            }
            TrackingDataContext.SaveChanges();

            return byomeis.Select(b => b.Id).ToList();
        }

        private PtByomei ConvertFromModelToPtByomei(PtDiseaseModel model, int hpId, int userId)
        {
            var preSuffixList = model.PrefixSuffixList;
            return new PtByomei
            {
                HpId = hpId,
                PtId = model.PtId,
                ByomeiCd = model.ByomeiCd,
                SortNo = model.SortNo,
                SyusyokuCd1 = preSuffixList.Count > 0 ? preSuffixList[0].Code : string.Empty,
                SyusyokuCd2 = preSuffixList.Count > 1 ? preSuffixList[1].Code : string.Empty,
                SyusyokuCd3 = preSuffixList.Count > 2 ? preSuffixList[2].Code : string.Empty,
                SyusyokuCd4 = preSuffixList.Count > 3 ? preSuffixList[3].Code : string.Empty,
                SyusyokuCd5 = preSuffixList.Count > 4 ? preSuffixList[4].Code : string.Empty,
                SyusyokuCd6 = preSuffixList.Count > 5 ? preSuffixList[5].Code : string.Empty,
                SyusyokuCd7 = preSuffixList.Count > 6 ? preSuffixList[6].Code : string.Empty,
                SyusyokuCd8 = preSuffixList.Count > 7 ? preSuffixList[7].Code : string.Empty,
                SyusyokuCd9 = preSuffixList.Count > 8 ? preSuffixList[8].Code : string.Empty,
                SyusyokuCd10 = preSuffixList.Count > 9 ? preSuffixList[9].Code : string.Empty,
                SyusyokuCd11 = preSuffixList.Count > 10 ? preSuffixList[10].Code : string.Empty,
                SyusyokuCd12 = preSuffixList.Count > 11 ? preSuffixList[11].Code : string.Empty,
                SyusyokuCd13 = preSuffixList.Count > 12 ? preSuffixList[12].Code : string.Empty,
                SyusyokuCd14 = preSuffixList.Count > 13 ? preSuffixList[13].Code : string.Empty,
                SyusyokuCd15 = preSuffixList.Count > 14 ? preSuffixList[14].Code : string.Empty,
                SyusyokuCd16 = preSuffixList.Count > 15 ? preSuffixList[15].Code : string.Empty,
                SyusyokuCd17 = preSuffixList.Count > 16 ? preSuffixList[16].Code : string.Empty,
                SyusyokuCd18 = preSuffixList.Count > 17 ? preSuffixList[17].Code : string.Empty,
                SyusyokuCd19 = preSuffixList.Count > 18 ? preSuffixList[18].Code : string.Empty,
                SyusyokuCd20 = preSuffixList.Count > 19 ? preSuffixList[19].Code : string.Empty,
                SyusyokuCd21 = preSuffixList.Count > 20 ? preSuffixList[20].Code : string.Empty,
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
                CreateId = userId,
                CreateDate = CIUtil.GetJapanDateTimeNow(),
                SeqNo = model.SeqNo,
                IsImportant = model.IsImportant,
                UpdateId = userId,
                UpdateDate = CIUtil.GetJapanDateTimeNow()
            };
        }

        private List<PrefixSuffixModel> SyusyokuCdToList(PtByomei ptByomei)
        {
            List<string> codeList = new()
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
            codeList = codeList.Where(c => c != string.Empty).ToList();

            if (codeList.Count == 0)
            {
                return new List<PrefixSuffixModel>();
            }

            var byomeiMstList = NoTrackingDataContext.ByomeiMsts.Where(b => codeList.Contains(b.ByomeiCd)).ToList();

            List<PrefixSuffixModel> result = new();
            foreach (var code in codeList)
            {
                var byomeiMst = byomeiMstList.FirstOrDefault(b => b.ByomeiCd == code);
                if (byomeiMst == null)
                {
                    continue;
                }
                result.Add(new PrefixSuffixModel(code, byomeiMst.Byomei ?? string.Empty));
            }

            return result;
        }

        public List<PtDiseaseModel> GetByomeiInThisMonth(int hpId, int sinYm, long ptId, int hokenId)
        {
            int firstDateOfThisMonth = sinYm * 100 + 1;
            int endDateOfThisMonth = sinYm * 100 + DateTime.DaysInMonth(sinYm / 100, sinYm % 100);
            var ptByomeiList = NoTrackingDataContext.PtByomeis.Where(item => item.HpId == hpId
                                                                             && item.PtId == ptId
                                                                             && item.IsDeleted == DeleteTypes.None
                                                                             && item.IsNodspRece == 0
                                                                             && (item.TenkiKbn == TenkiKbnConst.Continued
                                                                                 || item.StartDate <= endDateOfThisMonth && item.TenkiDate >= firstDateOfThisMonth)
                                                                             && (item.HokenPid == hokenId || item.HokenPid == 0))
                                                              .ToList();

            var byomeiCdList = ptByomeiList.Select(item => item.ByomeiCd).ToList();

            var byomeiMstList = NoTrackingDataContext.ByomeiMsts.Where(item => item.HpId == hpId && byomeiCdList.Contains(item.ByomeiCd)).ToList();

            List<PtDiseaseModel> result = new();
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
                        byomeiName = byomeiMst.Sbyomei ?? string.Empty;

                        icd10 = byomeiMst.Icd101 ?? string.Empty;
                        if (!string.IsNullOrEmpty(byomeiMst.Icd102))
                        {
                            icd10 += "/" + byomeiMst.Icd102;
                        }
                        icd102013 = byomeiMst.Icd1012013 ?? string.Empty;
                        if (!string.IsNullOrEmpty(byomeiMst.Icd1022013))
                        {
                            icd102013 += "/" + byomeiMst.Icd1022013;
                        }

                        icd1012013 = byomeiMst.Icd1012013 ?? string.Empty;
                        icd1022013 = byomeiMst.Icd1022013 ?? string.Empty;
                    }
                }
                var ptDiseaseModel = new PtDiseaseModel(
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
                        0,
                        icd10,
                        icd102013,
                        icd1012013,
                        icd1022013,
                        ptByomei.HokenPid,
                        ptByomei.HosokuCmt ?? string.Empty,
                        ptByomei.TogetuByomei,
                        byomeiMst?.DelDate ?? 0,
                        ptByomei.HokenPid
                        );
                result.Add(ptDiseaseModel);
            }

            return result;
        }
    }
}
