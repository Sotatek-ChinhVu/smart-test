using Domain.Constant;
using Domain.Enum;
using Domain.Models.Diseases;
using Domain.Models.MstItem;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using System.Linq.Dynamic.Core;

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
                        0
                        );
                result.Add(ptDiseaseModel);
            }
            return result;

        }

        public List<PtDiseaseModel> GetPatientDiseaseList(int hpId, long ptId, int sinDate, int hokenId, DiseaseViewType openFrom, bool isContiFiltered, bool isInMonthFiltered)
        {
            IQueryable<PtByomei> ptByomeiListQueryable = NoTrackingDataContext.PtByomeis.Where(p => p.HpId == hpId &&
                                                                              p.PtId == ptId &&
                                                                              p.IsDeleted != 1 &&
                                                                              (openFrom != DiseaseViewType.FromReception || p.TenkiKbn == TenkiKbnConst.Continued ||
                                                                              (p.StartDate <= sinDate && p.TenkiDate >= sinDate)));

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
                        0
                        );
                result.Add(ptDiseaseModel);
            }

            return result;
        }

        public List<PtDiseaseModel> GetAllByomeiByPtId(int hpId, long ptId, int pageIndex, int pageSize)
        {
            var ptByomeiList = NoTrackingDataContext.PtByomeis.Where(p => p.HpId == hpId &&
                                                                          p.PtId == ptId)
                                                              .OrderByDescending(p => p.UpdateDate)
                                                              .ThenByDescending(p => p.Id)
                                                              .Skip((pageIndex - 1) * pageSize)
                                                              .Take(pageSize)
                                                              .ToList();

            var userIdList = ptByomeiList.Select(item => item.CreateId).ToList();
            userIdList.AddRange(ptByomeiList.Select(item => item.UpdateId).ToList());
            userIdList = userIdList.Distinct().ToList();

            var byomeiCdList = ptByomeiList.Select(item => item.ByomeiCd).Distinct().ToList();
            var byomeiMstList = NoTrackingDataContext.ByomeiMsts.Where(item => byomeiCdList.Contains(item.ByomeiCd)).ToList();

            var userMstList = NoTrackingDataContext.UserMsts.Where(item => item.HpId == hpId
                                                                           && userIdList.Contains(item.UserId)
                                                                           && item.IsDeleted == 0)
                                                            .ToList();

            List<PtDiseaseModel> result = new();
            foreach (var ptByomei in ptByomeiList)
            {
                string createName = userMstList.FirstOrDefault(item => item.UserId == ptByomei.CreateId)?.Sname ?? string.Empty;
                string updateName = userMstList.FirstOrDefault(item => item.UserId == ptByomei.UpdateId)?.Sname ?? string.Empty;
                string byomeiName = "";
                if (ptByomei.ByomeiCd != "0000999")
                {
                    byomeiName = byomeiMstList.FirstOrDefault(item => item.ByomeiCd == ptByomei.ByomeiCd)?.Byomei ?? ptByomei.Byomei ?? string.Empty;
                }
                else
                {
                    byomeiName = ptByomei.Byomei ?? (byomeiMstList.FirstOrDefault(item => item.ByomeiCd == ptByomei.ByomeiCd)?.Byomei ?? string.Empty);
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
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        ptByomei.HokenPid,
                        ptByomei.HosokuCmt ?? string.Empty,
                        ptByomei.TogetuByomei,
                        0
                        );
                ptDiseaseModel = ptDiseaseModel.ChangeCreateUserUpdateDate(createName, updateName, ptByomei.CreateDate, ptByomei.UpdateDate);
                result.Add(ptDiseaseModel);
            }
            result = result.OrderByDescending(p => p.UpdateDate).ThenByDescending(p => p.Id).ToList();
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
                        select new { byomeiSet = byomeiSet, byomeiMst = p ?? new ByomeiMst() };

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
                                                                x.byomeiSet.SelectType,
                                                                ConvertToByomeiMstModel(x.byomeiMst))).ToList();
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        public List<long> Upsert(List<PtDiseaseModel> inputDatas, int hpId, int userId)
        {
            var byomeiIdList = inputDatas.Select(item => item.Id).Distinct().ToList();
            var byomeiListDb = TrackingDataContext.PtByomeis.Where(item => item.HpId == hpId
                                                                           && byomeiIdList.Contains(item.Id)
                                                            ).ToList();

            inputDatas = inputDatas.OrderBy(item => item.SortNo).ToList();
            int maxSortNo = NoTrackingDataContext.PtByomeis.OrderBy(item => item.SortNo).LastOrDefault()?.SortNo ?? 0;
            var byomeis = new List<PtByomei>();
            foreach (var inputData in inputDatas)
            {
                if (inputData.IsDeleted == DeleteTypes.Deleted)
                {
                    var ptByomei = byomeiListDb.FirstOrDefault(p => p.HpId == inputData.HpId && p.PtId == inputData.PtId && p.Id == inputData.Id);
                    if (ptByomei != null)
                    {
                        ptByomei.IsDeleted = DeleteTypes.Deleted;
                    }
                }
                else
                {
                    var ptByomei = byomeiListDb.FirstOrDefault(p => p.HpId == inputData.HpId && p.PtId == inputData.PtId && p.Id == inputData.Id);
                    PtByomei byomei;

                    if (ptByomei != null)
                    {
                        byomei = ConvertFromModelToPtByomei(inputData, hpId, userId);
                        if (IsModified(ptByomei, inputData))
                        {
                            TrackingDataContext.PtByomeis.Add(byomei);
                            ptByomei.IsDeleted = DeleteTypes.Deleted;
                            ptByomei.UpdateId = userId;
                            ptByomei.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        }
                        else
                        {
                            ptByomei.SortNo = inputData.SortNo;
                        }
                    }
                    else
                    {
                        byomei = ConvertFromModelToPtByomei(inputData, hpId, userId);
                        if (byomei.SortNo == 0)
                        {
                            byomei.SortNo = maxSortNo + 1;
                            maxSortNo++;
                        }
                        TrackingDataContext.PtByomeis.Add(byomei);
                    }

                    byomeis.Add(byomei);
                }
            }
            TrackingDataContext.SaveChanges();

            return byomeis.Select(b => b.Id).ToList();
        }

        private bool IsModified(PtByomei byomei, PtDiseaseModel model)
        {
            if (byomei.ByomeiCd != model.ByomeiCd
                || byomei.SyusyokuCd1 != (model.PrefixSuffixList.Count > 0 ? model.PrefixSuffixList[0].Code : string.Empty)
                || byomei.SyusyokuCd2 != (model.PrefixSuffixList.Count > 1 ? model.PrefixSuffixList[1].Code : string.Empty)
                || byomei.SyusyokuCd3 != (model.PrefixSuffixList.Count > 2 ? model.PrefixSuffixList[2].Code : string.Empty)
                || byomei.SyusyokuCd4 != (model.PrefixSuffixList.Count > 3 ? model.PrefixSuffixList[3].Code : string.Empty)
                || byomei.SyusyokuCd5 != (model.PrefixSuffixList.Count > 4 ? model.PrefixSuffixList[4].Code : string.Empty)
                || byomei.SyusyokuCd6 != (model.PrefixSuffixList.Count > 5 ? model.PrefixSuffixList[5].Code : string.Empty)
                || byomei.SyusyokuCd7 != (model.PrefixSuffixList.Count > 6 ? model.PrefixSuffixList[6].Code : string.Empty)
                || byomei.SyusyokuCd8 != (model.PrefixSuffixList.Count > 7 ? model.PrefixSuffixList[7].Code : string.Empty)
                || byomei.SyusyokuCd9 != (model.PrefixSuffixList.Count > 8 ? model.PrefixSuffixList[8].Code : string.Empty)
                || byomei.SyusyokuCd10 != (model.PrefixSuffixList.Count > 9 ? model.PrefixSuffixList[9].Code : string.Empty)
                || byomei.SyusyokuCd11 != (model.PrefixSuffixList.Count > 10 ? model.PrefixSuffixList[10].Code : string.Empty)
                || byomei.SyusyokuCd12 != (model.PrefixSuffixList.Count > 11 ? model.PrefixSuffixList[11].Code : string.Empty)
                || byomei.SyusyokuCd13 != (model.PrefixSuffixList.Count > 12 ? model.PrefixSuffixList[12].Code : string.Empty)
                || byomei.SyusyokuCd14 != (model.PrefixSuffixList.Count > 13 ? model.PrefixSuffixList[13].Code : string.Empty)
                || byomei.SyusyokuCd15 != (model.PrefixSuffixList.Count > 14 ? model.PrefixSuffixList[14].Code : string.Empty)
                || byomei.SyusyokuCd16 != (model.PrefixSuffixList.Count > 15 ? model.PrefixSuffixList[15].Code : string.Empty)
                || byomei.SyusyokuCd17 != (model.PrefixSuffixList.Count > 16 ? model.PrefixSuffixList[16].Code : string.Empty)
                || byomei.SyusyokuCd18 != (model.PrefixSuffixList.Count > 17 ? model.PrefixSuffixList[17].Code : string.Empty)
                || byomei.SyusyokuCd19 != (model.PrefixSuffixList.Count > 18 ? model.PrefixSuffixList[18].Code : string.Empty)
                || byomei.SyusyokuCd20 != (model.PrefixSuffixList.Count > 19 ? model.PrefixSuffixList[19].Code : string.Empty)
                || byomei.SyusyokuCd21 != (model.PrefixSuffixList.Count > 20 ? model.PrefixSuffixList[20].Code : string.Empty)
                || byomei.Byomei != model.Byomei
                || byomei.StartDate != model.StartDate
                || byomei.TenkiKbn != model.TenkiKbn
                || byomei.TenkiDate != model.TenkiDate
                || byomei.SyubyoKbn != model.SyubyoKbn
                || byomei.SikkanKbn != model.SikkanKbn
                || byomei.NanByoCd != model.NanbyoCd
                || byomei.HosokuCmt != model.HosokuCmt
                || byomei.HokenPid != model.HokenPid
                || byomei.TogetuByomei != model.TenkiKbn
                || byomei.IsNodspRece != model.IsNodspRece
                || byomei.IsNodspKarte != model.IsNodspKarte
                || byomei.SeqNo != model.SeqNo
                || byomei.IsImportant != model.IsImportant)
            {
                return true;
            }
            return false;
        }

        private PtByomei ConvertFromModelToPtByomei(PtDiseaseModel model, int hpId, int userId)
        {
            var preSuffixList = model.PrefixSuffixList;
            return new PtByomei
            {
                Id = 0,
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
            codeList = codeList.Where(c => c != string.Empty).Distinct().ToList();

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

        public List<PtDiseaseModel> GetByomeisInMonth(int hpId, long ptId, int sinYearMonth)
        {
            List<PtDiseaseModel> result = new();
            int firstDateOfThisMonth = sinYearMonth * 100 + 1;
            int endDateOfThisMonth = sinYearMonth * 100 + 31;
            var ptByomeis = NoTrackingDataContext.PtByomeis.Where(x => x.HpId == hpId &&
                                                                       x.PtId == ptId &&
                                                                       x.IsDeleted == 0 &&
                                                                       x.StartDate <= endDateOfThisMonth &&
                                                                       (x.TenkiKbn == TenkiKbnConst.Continued || x.TenkiDate >= firstDateOfThisMonth));
            var byomeiMstQuery = NoTrackingDataContext.ByomeiMsts.Where(b => b.HpId == hpId)
                                                                 .Select(item => new { item.HpId, item.ByomeiCd, item.Sbyomei, item.SikkanCd, item.Icd101, item.Icd102, item.Icd1012013, item.Icd1022013 });

            var ptByomeiModels = new List<PtDiseaseModel>();

            foreach (var ptByomei in ptByomeis)
            {
                var byomeiMst = byomeiMstQuery.FirstOrDefault(item => item.ByomeiCd == ptByomei.ByomeiCd);

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
                        ptByomei.Byomei ?? string.Empty,
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
                        0
                        );
                ptByomeiModels.Add(ptDiseaseModel);
            }

            var SyusyokuCdList = ptByomeis.Select(item => item.SyusyokuCd1)
                                 .Union(ptByomeis.Select(item => item.SyusyokuCd2))
                                 .Union(ptByomeis.Select(item => item.SyusyokuCd3))
                                 .Union(ptByomeis.Select(item => item.SyusyokuCd4))
                                 .Union(ptByomeis.Select(item => item.SyusyokuCd5))
                                 .Union(ptByomeis.Select(item => item.SyusyokuCd6))
                                 .Union(ptByomeis.Select(item => item.SyusyokuCd7))
                                 .Union(ptByomeis.Select(item => item.SyusyokuCd8))
                                 .Union(ptByomeis.Select(item => item.SyusyokuCd9))
                                 .Union(ptByomeis.Select(item => item.SyusyokuCd10))
                                 .Union(ptByomeis.Select(item => item.SyusyokuCd11))
                                 .Union(ptByomeis.Select(item => item.SyusyokuCd12))
                                 .Union(ptByomeis.Select(item => item.SyusyokuCd13))
                                 .Union(ptByomeis.Select(item => item.SyusyokuCd14))
                                 .Union(ptByomeis.Select(item => item.SyusyokuCd15))
                                 .Union(ptByomeis.Select(item => item.SyusyokuCd16))
                                 .Union(ptByomeis.Select(item => item.SyusyokuCd17))
                                 .Union(ptByomeis.Select(item => item.SyusyokuCd18))
                                 .Union(ptByomeis.Select(item => item.SyusyokuCd19))
                                 .Union(ptByomeis.Select(item => item.SyusyokuCd20))
                                 .Union(ptByomeis.Select(item => item.SyusyokuCd21))
                                 .Distinct().ToList();

            var byomeiMstList = (from ptByomei in ptByomeis
                                 join ptByomeiMst in byomeiMstQuery on new { ptByomei.HpId, ptByomei.ByomeiCd } equals new { ptByomeiMst.HpId, ptByomeiMst.ByomeiCd }
                                 select ptByomeiMst).ToList();

            var byomeiMstForSyusyokuList = byomeiMstQuery.Where(item => SyusyokuCdList.Contains(item.ByomeiCd)).ToList();

            foreach (var ptByomeiModel in ptByomeiModels)
            {
                try
                {
                    if (ptByomeiModel.IsFreeWord)
                    {
                        ptByomeiModel.Byomei = ptByomeiModel.FullByomei;
                        continue;
                    }

                    var byomeiMst = byomeiMstList.FirstOrDefault(item => item.ByomeiCd == ptByomeiModel.ByomeiCd);

                    if (byomeiMst != null)
                    {
                        ptByomeiModel.Byomei = byomeiMst.Sbyomei;
                        ptByomeiModel.Icd10 = byomeiMst.Icd101;
                        ptByomeiModel.ChangeSikkanCd(byomeiMst.SikkanCd);
                        if (!string.IsNullOrEmpty(byomeiMst.Icd102))
                        {
                            ptByomeiModel.Icd10 += "/" + byomeiMst.Icd102;
                        }
                        ptByomeiModel.Icd102013 = byomeiMst.Icd1012013;
                        if (!string.IsNullOrEmpty(byomeiMst.Icd1022013))
                        {
                            ptByomeiModel.Icd102013 += "/" + byomeiMst.Icd1022013;
                        }

                        ptByomeiModel.Icd1012013 = byomeiMst.Icd1012013;
                        ptByomeiModel.Icd1022013 = byomeiMst.Icd1022013;
                    }
                    else
                    {
                        ptByomeiModel.Icd1012013 = string.Empty;
                        ptByomeiModel.Icd1022013 = string.Empty;
                    }

                    var ptByomei = ptByomeis.Where(x => x.ByomeiCd == ptByomeiModel.ByomeiCd).First();

                    for (int i = 1; i <= 21; i++)
                    {
                        string byoCd = ptByomei.GetMemberValue("SyusyokuCd" + i).AsString();
                        if (string.IsNullOrEmpty(byoCd))
                        {
                            break;
                        }
                    }
                }
                finally
                {
                    result.Add(ptByomeiModel);
                }
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
                        byomeiMst?.DelDate ?? 0
                        );
                result.Add(ptDiseaseModel);
            }

            return result;
        }

        public List<PtDiseaseModel> GetPtByomeisByHokenId(int hpId, long ptId, int hokenId)
        {
            var ptByomeiList = NoTrackingDataContext.PtByomeis.Where(item => item.HpId == hpId
                                                                             && item.PtId == ptId
                                                                             && item.IsDeleted == DeleteTypes.None
                                                                             && item.HokenPid == hokenId
                                                                             && item.TenkiKbn == TenkiKbnConst.Continued)
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
                        byomeiMst?.DelDate ?? 0
                        );
                result.Add(ptDiseaseModel);
            }

            return result;
        }

        public List<PtDiseaseModel> GetTekiouByomeiByOrder(int hpId, List<string> itemCds)
        {
            itemCds = itemCds.Distinct().ToList();
            var tekiouByomeiMstList = NoTrackingDataContext.TekiouByomeiMsts.Where(item => item.HpId == hpId
                                                                                        && itemCds.Contains(item.ItemCd)
                                                                                        && item.IsInvalid == 0)
                                                                            .ToList();

            var byomeiCdList = tekiouByomeiMstList.Select(item => item.ByomeiCd).Distinct().ToList();

            var byomeiMstList = NoTrackingDataContext.ByomeiMsts.Where(item => item.HpId == hpId && byomeiCdList.Contains(item.ByomeiCd)).ToList();


            List<PtDiseaseModel> result = new();
            foreach (var tekiouByomei in tekiouByomeiMstList)
            {
                var byomeiMst = byomeiMstList.FirstOrDefault(item => item.ByomeiCd == tekiouByomei.ByomeiCd);

                if (byomeiMst == null || string.IsNullOrEmpty(tekiouByomei.ItemCd))
                {
                    continue;
                }
                var ptDiseaseModel = new PtDiseaseModel(
                                         tekiouByomei.ItemCd,
                                         byomeiMst.ByomeiCd ?? string.Empty,
                                         byomeiMst.Sbyomei ?? string.Empty,
                                         byomeiMst.SikkanCd,
                                         byomeiMst.IsAdopted == 1,
                                         byomeiMst.NanbyoCd);
                result.Add(ptDiseaseModel);
            }

            return result;
        }

        private static ByomeiMstModel ConvertToByomeiMstModel(ByomeiMst mst)
        {
            return new ByomeiMstModel(
                    mst.ByomeiCd,
                    mst.Byomei ?? string.Empty,
                    ConvertByomeiCdDisplay(mst.ByomeiCd),
                    mst.Sbyomei ?? string.Empty,
                    mst.KanaName1 ?? string.Empty,
                    mst.SikkanCd,
                    ConvertSikkanDisplay(mst.SikkanCd),
                    mst.NanbyoCd == NanbyoConst.Gairai ? "難病" : string.Empty,
                    ConvertIcd10Display(mst.Icd101 ?? string.Empty, mst.Icd102 ?? string.Empty),
                    ConvertIcd102013Display(mst.Icd1012013 ?? string.Empty, mst.Icd1022013 ?? string.Empty),
                    mst.IsAdopted == 1,
                    mst.NanbyoCd
                );
        }

        /// Get the ByomeiCdDisplay depend on ByomeiCd
        private static string ConvertByomeiCdDisplay(string byomeiCd)
        {
            string result = "";

            if (string.IsNullOrEmpty(byomeiCd)) return result;

            if (byomeiCd.Length != 4)
            {
                result = "病名";
            }
            else
            {
                if (byomeiCd.StartsWith("8"))
                {
                    result = "接尾語";
                }
                else if (byomeiCd.StartsWith("9"))
                {
                    result = "その他";
                }
                else
                {
                    result = "接頭語";
                }
            }
            return result;
        }

        /// Get the SikkanCd for display
        private static string ConvertSikkanDisplay(int SikkanCd)
        {
            string sikkanDisplay = "";
            switch (SikkanCd)
            {
                case 0:
                    sikkanDisplay = "";
                    break;
                case 5:
                    sikkanDisplay = "特疾";
                    break;
                case 3:
                    sikkanDisplay = "皮１";
                    break;
                case 4:
                    sikkanDisplay = "皮２";
                    break;
                case 7:
                    sikkanDisplay = "てんかん";
                    break;
                case 8:
                    sikkanDisplay = "特疾又はてんかん";
                    break;
            }
            return sikkanDisplay;
        }

        /// Get the Icd10Display depend on Icd101 and Icd102
        private static string ConvertIcd10Display(string icd101, string icd102)
        {
            string result = icd101;
            if (!string.IsNullOrWhiteSpace(result))
            {
                if (!string.IsNullOrWhiteSpace(icd102))
                {
                    result = result + "/" + icd102;
                }
            }
            else
            {
                result = icd102;
            }
            return result;
        }

        /// Get the Icd10Display depend on Icd1012013 and Icd1022013
        private static string ConvertIcd102013Display(string icd1012013, string icd1022013)
        {
            string rs = icd1012013;
            if (!string.IsNullOrWhiteSpace(rs))
            {
                if (!string.IsNullOrWhiteSpace(icd1022013))
                {
                    rs = rs + "/" + icd1022013;
                }
            }
            else
            {
                rs = icd1022013;
            }
            return rs;
        }

        public bool UpdateByomeiSetMst(int userId, int hpId, List<ByomeiSetMstUpdateModel> listData)
        {
            foreach (var item in listData)
            {
                // Create
                if (item.SeqNo == 0)
                {
                    var listSetMst = TrackingDataContext.ByomeiSetMsts.FirstOrDefault(x => x.HpId == item.HpId && x.GenerationId == item.GenerationId
                    && x.SeqNo == item.SeqNo && x.ByomeiCd == item.ByomeiCd && x.Level1 == item.Level1 && x.Level2 == item.Level2 && x.Level3 == item.Level3
                    && x.Level4 == item.Level4 && x.Level5 == item.Level5);
                    if (listSetMst != null)
                    {
                        return false;
                    }

                    TrackingDataContext.ByomeiSetMsts.Add(new ByomeiSetMst()
                    {
                        CreateId = userId,
                        UpdateId = userId,
                        HpId = hpId,
                        GenerationId = item.GenerationId,
                        SetName = item.SetName,
                        ByomeiCd = item.ByomeiCd,
                        CreateMachine = CIUtil.GetComputerName(),
                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                        UpdateDate = CIUtil.GetJapanDateTimeNow(),
                        Level1 = item.Level1,
                        Level2 = item.Level2,
                        Level3 = item.Level3,
                        Level4 = item.Level4,
                        Level5 = item.Level5,
                        IsDeleted = 0,
                        IsTitle = item.IsTitle,
                        SelectType = item.SelectType

                    });
                }

                // Update
                else
                {
                    var listSetMst = TrackingDataContext.ByomeiSetMsts.FirstOrDefault(x => x.HpId == item.HpId && x.GenerationId == item.GenerationId
                    && x.SeqNo == item.SeqNo);
                    if (listSetMst == null)
                    {
                        return false;
                    }
                    listSetMst.UpdateId = userId;
                    listSetMst.SetName = item.SetName;
                    listSetMst.ByomeiCd = item.ByomeiCd;
                    listSetMst.Level1 = item.Level1;
                    listSetMst.Level2 = item.Level2;
                    listSetMst.Level3 = item.Level3;
                    listSetMst.Level4 = item.Level4;
                    listSetMst.Level5 = item.Level5;
                    listSetMst.IsDeleted = item.IsDeleted;
                    listSetMst.IsTitle = item.IsTitle;
                    listSetMst.SelectType = item.SelectType;
                    listSetMst.UpdateMachine = CIUtil.GetComputerName();
                    listSetMst.UpdateDate = CIUtil.GetJapanDateTimeNow();
                }

            }
            TrackingDataContext.SaveChanges();
            return true;
        }

        public Dictionary<string, string> GetByomeiMst(int hpId, List<string> byomeiCds)
        {
            var result = new Dictionary<string, string>();

            var byomeiMstList = NoTrackingDataContext.ByomeiMsts.Where(item => item.HpId == hpId && byomeiCds.Contains(item.ByomeiCd)).ToList();
            foreach (var byomeiCd in byomeiCds)
            {
                if (!string.IsNullOrEmpty(byomeiCd))
                {
                    var byomei = byomeiMstList.FirstOrDefault(item => item.ByomeiCd == byomeiCd);
                    if (byomei != null && !result.ContainsKey(byomei.ByomeiCd))
                    {
                        result.Add(byomei.ByomeiCd, byomei.Sbyomei ?? string.Empty);
                    }
                }
            }

            return result;
        }

        public bool IsHokenInfInUsed(int hpId, long ptId, int hokenPId)
        {
            var result = NoTrackingDataContext.PtByomeis.Any(p => p.HpId == hpId && p.PtId == ptId && p.HokenPid == hokenPId && p.IsDeleted == 0);
            return result;
        }
    }
}
