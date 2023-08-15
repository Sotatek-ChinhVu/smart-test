﻿using Domain.Constant;
using Domain.Models.Diseases;
using Domain.Models.Ka;
using Domain.Models.Medical;
using Domain.Models.MedicalExamination;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.SystemConf;
using Domain.Models.TodayOdr;
using Domain.Types;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using System.Text;
using static Helper.Constants.OrderInfConst;

namespace Infrastructure.Repositories
{
    public class MedicalExaminationRepository : RepositoryBase, IMedicalExaminationRepository
    {
        private readonly ISystemConfRepository _systemConf;
        private readonly ITodayOdrRepository _todayOdrRepository;

        public MedicalExaminationRepository(ITenantProvider tenantProvider, ISystemConfRepository systemConf) : base(tenantProvider)
        {
            _systemConf = systemConf;
        }
        public List<CheckedOrderModel> IgakuTokusitu(int hpId, int sinDate, int hokenId, int syosaisinKbn, List<PtDiseaseModel> ByomeiModelList, List<OrdInfDetailModel> allOdrInfDetail, bool isJouhou)
        {
            var checkedOrderModelList = new List<CheckedOrderModel>();
            var igakuTokusituItem = allOdrInfDetail.FirstOrDefault(detail => detail.ItemCd == ItemCdConst.IgakuTokusitu || detail.ItemCd == ItemCdConst.IgakuTokusitu1);

            // 既に入力されている場合は不要
            if (igakuTokusituItem != null)
            {
                return checkedOrderModelList;
            }

            TenMst? tenMstModel = null;
            if (isJouhou)
            {
                if (sinDate >= 20220401)
                {
                    tenMstModel = FindTenMst(hpId, ItemCdConst.IgakuTokusitu1, sinDate);
                    if (string.IsNullOrEmpty(tenMstModel.ItemCd))
                    {
                        return checkedOrderModelList;
                    }
                }
                else
                {
                    return checkedOrderModelList;
                }
            }
            else
            {
                tenMstModel = FindTenMst(hpId, ItemCdConst.IgakuTokusitu, sinDate);
                if (string.IsNullOrEmpty(tenMstModel.ItemCd))
                {
                    return checkedOrderModelList;
                }
            }

            // 初診の場合は算定不可
            if (syosaisinKbn == SyosaiConst.Syosin ||
                syosaisinKbn == SyosaiConst.Syosin2 ||
                syosaisinKbn == SyosaiConst.Unspecified)
            {
                return checkedOrderModelList;
            }

            // 電話再診の場合は算定不可
            if (syosaisinKbn == SyosaiConst.SaisinDenwa ||
                syosaisinKbn == SyosaiConst.SaisinDenwa2)
            {
                return checkedOrderModelList;
            }

            // 初診日から1カ月以内は算定不可
            // 背反設定されている場合は不可

            var byomeiCondition = NoTrackingDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == 2002 && p.GrpEdaNo == 4)?.Val ?? 0;
            // 対象疾病の有無
            bool existByoMeiSpecial = ByomeiModelList
                                .Any(b => (byomeiCondition == 1 ? b.SyubyoKbn == 1 : true) &&
                                    b.SikkanKbn == SikkanKbnConst.Special &&
                                    (b.HokenPid == hokenId || b.HokenPid == 0) &&
                                    b.StartDate <= sinDate &&
                                    (b.TenkiKbn == TenkiKbnConst.Continued || b.TenkiDate > sinDate));
            if (existByoMeiSpecial)
            {
                var santeiKanren = NoTrackingDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == 4001 && p.GrpEdaNo == 0)?.Val ?? 0;
                bool santei = false;
                if (santeiKanren == 0)
                {
                    santei = false;
                }
                else if (santeiKanren == 1 && syosaisinKbn != SyosaiConst.None)
                {
                    santei = true;
                }
                var checkedContent = FormatSanteiMessage(tenMstModel.Name ?? string.Empty);

                var checkedOrderModel = new CheckedOrderModel(CheckingType.MissingCalculate, santei, checkedContent, tenMstModel.ItemCd, tenMstModel.SinKouiKbn, tenMstModel.Name ?? string.Empty, 0);

                checkedOrderModelList.Add(checkedOrderModel);

                return checkedOrderModelList;
            }

            bool existByoMeiOther = ByomeiModelList
                            .Any(b => (byomeiCondition == 1 ? b.SyubyoKbn == 1 : true) &&
                                b.SikkanKbn == SikkanKbnConst.Other &&
                                (b.HokenPid == hokenId || b.HokenPid == 0) &&
                                b.StartDate <= sinDate &&
                                (b.TenkiKbn == TenkiKbnConst.Continued || b.TenkiDate > sinDate));
            if (existByoMeiOther)
            {
                var checkedContent = FormatSanteiMessage(tenMstModel.Name ?? string.Empty);
                var checkedOrderModel = new CheckedOrderModel(CheckingType.MissingCalculate, false, checkedContent, tenMstModel.ItemCd, tenMstModel.SinKouiKbn, tenMstModel.Name ?? string.Empty, 0);

                checkedOrderModelList.Add(checkedOrderModel);

                return checkedOrderModelList;
            }
            return checkedOrderModelList;
        }

        public List<CheckedOrderModel> IgakuTokusituIsChecked(int hpId, int sinDate, int syosaisinKbn, List<CheckedOrderModel> checkedOrders, List<OrdInfDetailModel> allOdrInfDetail)
        {
            var result = new List<CheckedOrderModel>();
            var igakuTokusituItems = checkedOrders.Where(detail => detail.ItemCd == ItemCdConst.IgakuTokusitu || detail.ItemCd == ItemCdConst.IgakuTokusitu1);
            var igakuTokusituItemOthers = checkedOrders.Where(detail => detail.ItemCd != ItemCdConst.IgakuTokusitu || detail.ItemCd != ItemCdConst.IgakuTokusitu1);
            if (syosaisinKbn == SyosaiConst.None)
            {
                bool containCdKbn = false;
                foreach (var detail in allOdrInfDetail)
                {
                    var tenMstModel = FindTenMst(hpId, detail.ItemCd, sinDate);
                    if (tenMstModel == null) continue;
                    if (tenMstModel.CdKbn == "C" && tenMstModel.CdKbnno == 1 && tenMstModel.Kokuji2 == "1" || tenMstModel.ItemCd.Contains("@Z"))
                    {
                        var santeiKanren = NoTrackingDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == 4001 && p.GrpEdaNo == 0)?.Val ?? 0;
                        if (santeiKanren == 0)
                        {
                            containCdKbn = false;
                        }
                        else
                        {
                            containCdKbn = true;
                        }
                    }
                }
                if (igakuTokusituItems != null && igakuTokusituItems.Count() > 0)
                {
                    var igaku = igakuTokusituItems.FirstOrDefault();
                    if (igaku != null)
                    {
                        result.Add(igaku.ChangeSantei(containCdKbn));
                    }
                }
            }

            result.AddRange(igakuTokusituItemOthers);

            return result;
        }

        public List<CheckedOrderModel> SihifuToku1(int hpId, long ptId, int sinDate, int hokenId, int syosaisinKbn, long raiinNo, long oyaRaiinNo, List<PtDiseaseModel> ByomeiModelList, List<OrdInfDetailModel> allOdrInfDetail, bool isJouhou)
        {
            var checkedOrderModelList = new List<CheckedOrderModel>();
            var sihifu1Item = allOdrInfDetail.FirstOrDefault(detail => detail.ItemCd == ItemCdConst.SiHifuToku1 || detail.ItemCd == ItemCdConst.SiHifuToku1JyohoTusin
                                                                    || detail.ItemCd == ItemCdConst.SiHifuToku2 || detail.ItemCd == ItemCdConst.SiHifuToku2JyohoTusin);


            // 既に入力されている場合は不要
            if (sihifu1Item != null)
            {
                return checkedOrderModelList;
            }

            TenMst? tenMstModel = null;
            if (isJouhou)
            {
                if (sinDate >= 20220401)
                {
                    tenMstModel = FindTenMst(hpId, ItemCdConst.SiHifuToku1JyohoTusin, sinDate);
                    if (string.IsNullOrEmpty(tenMstModel.ItemCd))
                    {
                        return checkedOrderModelList;
                    }
                }
                else
                {
                    return checkedOrderModelList;
                }
            }
            else
            {
                tenMstModel = FindTenMst(hpId, ItemCdConst.SiHifuToku1, sinDate);
                if (string.IsNullOrEmpty(tenMstModel.ItemCd))
                {
                    return checkedOrderModelList;
                }
            }

            var hifukaSetting = NoTrackingDataContext.SystemGenerationConfs.FirstOrDefault(p => p.HpId == hpId
                    && p.GrpCd == 8001
                    && p.GrpEdaNo == 1
                    && p.StartDate <= sinDate
                    && p.EndDate >= sinDate)?.Val ?? 0;
            //皮膚科標榜
            if (hifukaSetting != 1)
            {
                return checkedOrderModelList;
            }

            // 対象疾患の有無
            bool existByoMeiSkin1 = ByomeiModelList
                          .Any(b => b.SikkanKbn == SikkanKbnConst.Skin1 &&
                              (b.HokenPid == hokenId || b.HokenPid == 0) &&
                              b.StartDate <= sinDate &&
                              (b.TenkiKbn == TenkiKbnConst.Continued || b.TenkiDate > sinDate));
            if (!existByoMeiSkin1)
            {
                return checkedOrderModelList;
            }

            // 初診の場合は算定不可
            if (syosaisinKbn == SyosaiConst.Syosin ||
                syosaisinKbn == SyosaiConst.Syosin2 ||
                syosaisinKbn == SyosaiConst.Unspecified)
            {
                return checkedOrderModelList;
            }

            // 電話再診の場合は算定不可
            if (syosaisinKbn == SyosaiConst.SaisinDenwa ||
                syosaisinKbn == SyosaiConst.SaisinDenwa2)
            {
                return checkedOrderModelList;
            }

            // 算定回数チェック（1日1回）: 同一診療内に
            int odrCountInDay = GetSameVisitOdrCountInDay(hpId, ptId, sinDate, raiinNo, oyaRaiinNo,
                new List<string>() { ItemCdConst.SiHifuToku1, ItemCdConst.SiHifuToku2 });
            if (odrCountInDay > 0)
            {
                return checkedOrderModelList;
            }

            // 算定回数チェック（月2回）
            // 同月診療日以前の診療内に、算定されている場合は不可
            int santeiCount = GetSanteiCountInMonth(hpId, ptId, sinDate, new List<string>() { ItemCdConst.SiHifuToku1, ItemCdConst.SiHifuToku2 });
            //（基本的に、月１ 回に限りだが、処方の内容に変更があった場合は、その都度算定できるため）
            if (santeiCount > 0)
            {
                return checkedOrderModelList;
            }

            // 背反設定されている場合は不可
            var santeiKanren = NoTrackingDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == 4001 && p.GrpEdaNo == 2)?.Val ?? 0;
            bool santei = false;
            if (santeiKanren == 0)
            {
                santei = false;
            }
            else if (santeiKanren == 1 && syosaisinKbn != SyosaiConst.None)
            {
                santei = true;
            }
            var checkedContent = FormatSanteiMessage(tenMstModel.Name ?? string.Empty);
            var checkedOrderModel = new CheckedOrderModel(CheckingType.MissingCalculate, santei, checkedContent, tenMstModel.ItemCd, tenMstModel.SinKouiKbn, tenMstModel.Name ?? string.Empty, 0);

            checkedOrderModelList.Add(checkedOrderModel);

            return checkedOrderModelList;
        }

        private TenMst FindTenMst(int hpId, string itemCd, int sinDate)
        {
            var entity = NoTrackingDataContext.TenMsts.FirstOrDefault(p =>
                   p.HpId == hpId &&
                   p.StartDate <= sinDate &&
                   p.EndDate >= sinDate &&
                   p.ItemCd == itemCd);

            return entity ?? new TenMst();
        }

        private int GetSameVisitOdrCountInDay(int hpId, long ptId, int sinDate, long raiinNo, long oyaRaiinNo, IEnumerable<string> itemCd)
        {
            var raiinInfQuery = NoTrackingDataContext.RaiinInfs
               .Where(s => s.HpId == hpId && s.PtId == ptId && s.SinDate == sinDate && s.OyaRaiinNo == oyaRaiinNo && s.RaiinNo != raiinNo);
            var count = raiinInfQuery.Count();
            var odrInfQuery = NoTrackingDataContext.OdrInfs
                .Where(o => o.HpId == hpId && o.PtId == ptId && o.RaiinNo == raiinNo && o.IsDeleted == 0);
            count = odrInfQuery.Count();
            var odrInfDetailQuery = NoTrackingDataContext.OdrInfDetails
                .Where(o => o.HpId == hpId && o.PtId == ptId && o.RaiinNo == raiinNo && itemCd.Contains(o.ItemCd));
            var resultQuery = from raiinInf in raiinInfQuery.AsEnumerable()
                              join odrInf in odrInfQuery
                              on new { raiinInf.HpId, raiinInf.PtId, raiinInf.RaiinNo }
                              equals new { odrInf.HpId, odrInf.PtId, odrInf.RaiinNo }
                              join odrInfDetail in odrInfDetailQuery
                              on new { odrInf.HpId, odrInf.PtId, odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo }
                              equals new { odrInfDetail.HpId, odrInfDetail.PtId, odrInfDetail.RaiinNo, odrInfDetail.RpNo, odrInfDetail.RpEdaNo } into odrDetailList
                              select new
                              {
                                  OdrDetailList = odrDetailList,
                              };
            var firstRecord = resultQuery.FirstOrDefault();
            int result = 0;
            if (firstRecord != null)
            {
                result = firstRecord.OdrDetailList.Count();
            }
            return result;
        }

        private int GetSanteiCountInMonth(int hpId, long ptId, int sinDate, IEnumerable<string> itemCd)
        {
            int sinYM = sinDate / 100;
            int sinDay = sinDate - sinYM * 100;
            var sinKouiCountQuery = NoTrackingDataContext.SinKouiCounts
                .Where(s => s.HpId == hpId && s.PtId == ptId && s.SinYm == sinYM && s.SinDay < sinDay);

            var sinKouiDetailQuery = NoTrackingDataContext.SinKouiDetails
                .Where(s => s.HpId == hpId && s.PtId == ptId && itemCd.Contains(s.ItemCd));

            var resultQuery = from sinKouiCount in sinKouiCountQuery.AsEnumerable()
                              join sinKouiDetail in sinKouiDetailQuery
                              on new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.RpNo, sinKouiCount.SinYm }
                              equals new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.RpNo, sinKouiDetail.SinYm }
                              select new
                              {
                                  SinKouiCount = sinKouiCount,
                              };
            return resultQuery.AsEnumerable().Sum(s => s.SinKouiCount.Count);
        }

        private string FormatSanteiMessage(string santeiItemName)
        {
            return $"\"{santeiItemName}\"を算定できる可能性があります。";

        }

        public List<CheckedOrderModel> SihifuToku2(int hpId, long ptId, int sinDate, int hokenId, int iBirthDay, long raiinNo, int syosaisinKbn, long oyaRaiinNo, List<PtDiseaseModel> byomeiModelList, List<OrdInfDetailModel> allOdrInfDetail, List<int> odrInfs, bool isJouhou)
        {
            var checkedOrderModelList = new List<CheckedOrderModel>();
            var sihifu2Item = allOdrInfDetail.FirstOrDefault(detail => detail.ItemCd == ItemCdConst.SiHifuToku1 || detail.ItemCd == ItemCdConst.SiHifuToku1JyohoTusin
                                                                    || detail.ItemCd == ItemCdConst.SiHifuToku2 || detail.ItemCd == ItemCdConst.SiHifuToku2JyohoTusin);

            // 既に入力されている場合は不要
            if (sihifu2Item != null)
            {
                return checkedOrderModelList;
            }

            var sihifu1Item = checkedOrderModelList.FirstOrDefault(i => i.ItemCd == ItemCdConst.SiHifuToku1);
            if (sihifu1Item != null)
            {
                return checkedOrderModelList;
            }

            TenMst? tenMstModel = null;
            if (isJouhou)
            {
                if (sinDate >= 20220401)
                {
                    tenMstModel = FindTenMst(hpId, ItemCdConst.SiHifuToku2JyohoTusin, sinDate);
                    if (string.IsNullOrEmpty(tenMstModel.ItemCd))
                    {
                        return checkedOrderModelList;
                    }
                }
                else
                {
                    return checkedOrderModelList;
                }
            }
            else
            {
                tenMstModel = FindTenMst(hpId, ItemCdConst.SiHifuToku2, sinDate);
                if (string.IsNullOrEmpty(tenMstModel.ItemCd))
                {
                    return checkedOrderModelList;
                }
            }

            var hifukaSetting = NoTrackingDataContext.SystemGenerationConfs.FirstOrDefault(p => p.HpId == hpId
                    && p.GrpCd == 8001
                    && p.GrpEdaNo == 1
                    && p.StartDate <= sinDate
                    && p.EndDate >= sinDate)?.Val;
            //皮膚科標榜
            if (hifukaSetting != 1)
            {
                return checkedOrderModelList;
            }

            // 対象疾患の有無
            bool existByoMeiSkin2 = byomeiModelList
                          .Any(b => b.SikkanKbn == SikkanKbnConst.Skin2 &&
                              (b.HokenPid == hokenId || b.HokenPid == 0) &&
                              b.StartDate <= sinDate &&
                              (b.TenkiKbn == TenkiKbnConst.Continued || b.TenkiDate > sinDate)
                               //&&
                               //((!string.IsNullOrEmpty(b.Icd1012013) && b.Icd1012013.StartsWith("L20"))
                               //  || (!string.IsNullOrEmpty(b.Icd1022013) && b.Icd1022013.StartsWith("L20")))
                               );

            if (!existByoMeiSkin2)
            {
                return checkedOrderModelList;
            }

            bool existByoMeiSkin2WithoutL20 = byomeiModelList
                          .Any(b => b.SikkanKbn == SikkanKbnConst.Skin2 &&
                              (b.HokenPid == hokenId || b.HokenPid == 0) &&
                              b.StartDate <= sinDate &&
                              (b.TenkiKbn == TenkiKbnConst.Continued || b.TenkiDate > sinDate)
                               &&
                               !((!string.IsNullOrEmpty(b.Icd1012013) && b.Icd1012013.StartsWith("L20"))
                                 || (!string.IsNullOrEmpty(b.Icd1022013) && b.Icd1022013.StartsWith("L20")))
                               );

            if (!existByoMeiSkin2WithoutL20)
            {
                if (!odrInfs.Contains(23))
                {
                    return checkedOrderModelList;
                }
                int age = CIUtil.SDateToAge(iBirthDay, sinDate);
                if (age < 16)
                {
                    return checkedOrderModelList;
                }
            }

            // 初診の場合は算定不可
            if (syosaisinKbn == SyosaiConst.Syosin ||
                syosaisinKbn == SyosaiConst.Syosin2 ||
                syosaisinKbn == SyosaiConst.Unspecified)
            {
                return checkedOrderModelList;
            }

            // 電話再診の場合は算定不可
            if (syosaisinKbn == SyosaiConst.SaisinDenwa ||
                syosaisinKbn == SyosaiConst.SaisinDenwa2)
            {
                return checkedOrderModelList;
            }

            // 算定回数チェック（1日1回）: 同一診療内に
            int odrCountInDay = GetSameVisitOdrCountInDay(hpId, ptId, sinDate, raiinNo, oyaRaiinNo,
                new List<string>() { ItemCdConst.SiHifuToku1, ItemCdConst.SiHifuToku2 });
            if (odrCountInDay > 0)
            {
                return checkedOrderModelList;
            }

            // 算定回数チェック（月2回）
            // 同月診療日以前の診療内に、算定されている場合は不可
            int santeiCount = GetSanteiCountInMonth(hpId, ptId, sinDate, new List<string>() { ItemCdConst.SiHifuToku1, ItemCdConst.SiHifuToku2 });
            //（基本的に、月１ 回に限りだが、処方の内容に変更があった場合は、その都度算定できるため）
            if (santeiCount > 0)
            {
                return checkedOrderModelList;
            }

            // 背反設定されている場合は不可
            var santeiKanren = NoTrackingDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == 4001 && p.GrpEdaNo == 2)?.Val ?? 0;
            bool santei = false;
            if (santeiKanren == 0)
            {
                santei = false;
            }
            else if (santeiKanren == 1 && syosaisinKbn != SyosaiConst.None)
            {
                santei = true;
            }
            var checkedContent = FormatSanteiMessage(tenMstModel?.Name ?? string.Empty);
            var checkedOrderModel = new CheckedOrderModel(CheckingType.MissingCalculate, santei, checkedContent, tenMstModel?.ItemCd ?? string.Empty, tenMstModel?.SinKouiKbn ?? 0, tenMstModel?.Name ?? string.Empty, 0);
            checkedOrderModelList.Add(checkedOrderModel);
            return checkedOrderModelList;
        }

        public List<CheckedOrderModel> IgakuTenkan(int hpId, int sinDate, int hokenId, int syosaisinKbn, List<PtDiseaseModel> ByomeiModelList, List<OrdInfDetailModel> allOdrInfDetail, bool isJouhou)
        {
            var checkedOrderModelList = new List<CheckedOrderModel>();
            var igakuTenkanItem = allOdrInfDetail.FirstOrDefault(detail => detail.ItemCd == ItemCdConst.IgakuTenkan || detail.ItemCd == ItemCdConst.IgakuTenkanJyohoTusin);

            // 既に入力されている場合は不要
            if (igakuTenkanItem != null)
            {
                return checkedOrderModelList;
            }

            TenMst? tenMstModel = null;
            if (isJouhou)
            {
                tenMstModel = FindTenMst(hpId, ItemCdConst.IgakuTenkanJyohoTusin, sinDate);
                if (string.IsNullOrEmpty(tenMstModel.ItemCd))
                {
                    return checkedOrderModelList;
                }
            }
            else
            {
                tenMstModel = FindTenMst(hpId, ItemCdConst.IgakuTenkan, sinDate);
                if (string.IsNullOrEmpty(tenMstModel.ItemCd))
                {
                    return checkedOrderModelList;
                }
            }


            // 小児科、神経科、神経内科、精神科、脳神経外科又は心療内科を標榜
            var kaMstList = GetKaMsts(hpId);
            // 09  小児科
            // 03  神経科
            // 04  神経内科
            // 02  精神科
            // 14  脳神経外科
            // 33  心療内科
            bool existReceKaCd = false;
            foreach (var kaMst in kaMstList)
            {
                if (new List<string> { "09", "03", "04", "02", "14", "33" }.Contains(kaMst.ReceKaCd))
                {
                    existReceKaCd = true;
                    break;
                }
            }
            if (!existReceKaCd)
            {
                return checkedOrderModelList;
            }

            // 初診の場合は算定不可
            if (syosaisinKbn == SyosaiConst.Syosin ||
                syosaisinKbn == SyosaiConst.Syosin2 ||
                syosaisinKbn == SyosaiConst.Unspecified)
            {
                return checkedOrderModelList;
            }

            // 電話再診の場合は算定不可
            if (syosaisinKbn == SyosaiConst.SaisinDenwa ||
                syosaisinKbn == SyosaiConst.SaisinDenwa2)
            {
                return checkedOrderModelList;
            }

            // 算定回数チェック（1日1回、月2回）
            // 初診日から1カ月以内は算定不可（休日などの特例なし）
            // 背反設定されている場合は不可

            // 対象疾患の有無
            bool existByoMeiEpilepsy = ByomeiModelList
                                .Any(b => b.SyubyoKbn == 1 &&
                                    b.SikkanKbn == SikkanKbnConst.Epilepsy &&
                                    (b.HokenPid == hokenId || b.HokenPid == 0) &&
                                    b.StartDate <= sinDate &&
                                    (b.TenkiKbn == TenkiKbnConst.Continued || b.TenkiDate > sinDate));
            if (existByoMeiEpilepsy)
            {
                var santeiKanren = NoTrackingDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == 4001 && p.GrpEdaNo == 6)?.Val ?? 0;
                var santei = false;
                if (santeiKanren == 0)
                {
                    santei = false;
                }
                else if (santeiKanren == 1 && syosaisinKbn != SyosaiConst.None)
                {
                    santei = true;
                }
                var checkedContent = FormatSanteiMessage(tenMstModel.Name ?? string.Empty);
                var checkedOrderModel = new CheckedOrderModel(CheckingType.MissingCalculate, santei, checkedContent, tenMstModel?.ItemCd ?? string.Empty, tenMstModel?.SinKouiKbn ?? 0, tenMstModel?.Name ?? string.Empty, 0);

                checkedOrderModelList.Add(checkedOrderModel);

                return checkedOrderModelList;
            }

            bool existByoMeiOther = ByomeiModelList
            .Any(b => b.SyubyoKbn == 1 &&
                                b.SikkanKbn == SikkanKbnConst.Other &&
                                (b.HokenPid == hokenId || b.HokenPid == 0) &&
                                b.StartDate <= sinDate &&
                                (b.TenkiKbn == TenkiKbnConst.Continued || b.TenkiDate > sinDate));
            if (existByoMeiOther)
            {
                var checkedOrderModel = new CheckedOrderModel(CheckingType.MissingCalculate, false, FormatSanteiMessage(tenMstModel.Name ?? string.Empty), tenMstModel?.ItemCd ?? string.Empty, tenMstModel?.SinKouiKbn ?? 0, tenMstModel?.Name ?? string.Empty, 0);
                checkedOrderModelList.Add(checkedOrderModel);

                return checkedOrderModelList;
            }

            return checkedOrderModelList;
        }

        private List<KaMstModel> GetKaMsts(int hpId)
        {
            var entities = NoTrackingDataContext.KaMsts.Where(k => k.HpId == hpId && k.IsDeleted == 0).OrderBy(u => u.SortNo).ThenBy(u => u.KaId).ToList();
            List<KaMstModel> results = new List<KaMstModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new KaMstModel(entity.Id, entity.KaId, entity.SortNo, entity.ReceKaCd ?? string.Empty, entity.KaSname ?? string.Empty, entity.KaName ?? string.Empty));
            });

            return results;
        }

        public List<CheckedOrderModel> IgakuNanbyo(int hpId, int sinDate, int hokenId, int syosaisinKbn, List<PtDiseaseModel> ByomeiModelList, List<OrdInfDetailModel> allOdrInfDetail, bool isJouhou)
        {
            var checkedOrderModelList = new List<CheckedOrderModel>();
            var igakuNanbyoItem = allOdrInfDetail.FirstOrDefault(detail => detail.ItemCd == ItemCdConst.IgakuNanbyo || detail.ItemCd == ItemCdConst.IgakuNanbyoJyohoTusin);

            // 既に入力されている場合は不要
            if (igakuNanbyoItem != null)
            {
                return checkedOrderModelList;
            }

            TenMst? tenMstModel = null;
            if (isJouhou)
            {
                if (sinDate >= 20220401)
                {
                    tenMstModel = FindTenMst(hpId, ItemCdConst.IgakuNanbyoJyohoTusin, sinDate);
                    if (string.IsNullOrEmpty(tenMstModel.ItemCd))
                    {
                        return checkedOrderModelList;
                    }
                }
                else
                {
                    return checkedOrderModelList;
                }
            }
            else
            {
                tenMstModel = FindTenMst(hpId, ItemCdConst.IgakuNanbyo, sinDate);
                if (string.IsNullOrEmpty(tenMstModel.ItemCd))
                {
                    return checkedOrderModelList;
                }
            }

            // 初診の場合は算定不可
            if (syosaisinKbn == SyosaiConst.Syosin ||
                syosaisinKbn == SyosaiConst.Syosin2 ||
                syosaisinKbn == SyosaiConst.Unspecified)
            {
                return checkedOrderModelList;
            }

            // 電話再診の場合は算定不可
            if (syosaisinKbn == SyosaiConst.SaisinDenwa ||
                syosaisinKbn == SyosaiConst.SaisinDenwa2)
            {
                return checkedOrderModelList;
            }

            // 算定回数チェック（1日1回、月2回）
            // 初診日から1カ月以内は算定不可（休日などの特例なし）
            // 背反設定されている場合は不可

            // 対象疾患の有無
            bool existByoMeiSanteiGai = ByomeiModelList
                            .Any(b => b.SyubyoKbn == 1 &&
                                b.NanbyoCd == NanbyoConst.Gairai &&
                                (b.HokenPid == hokenId || b.HokenPid == 0) &&
                                b.StartDate <= sinDate &&
                                (b.TenkiKbn == TenkiKbnConst.Continued || b.TenkiDate > sinDate));
            if (existByoMeiSanteiGai)
            {
                var santeiKanren = NoTrackingDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == 4001 && p.GrpEdaNo == 7)?.Val;
                var santei = false;
                if (santeiKanren == 0)
                {
                    santei = false;
                }
                else if (santeiKanren == 1 && syosaisinKbn != SyosaiConst.None)
                {
                    santei = true;
                }
                var checkedContent = FormatSanteiMessage(tenMstModel.Name ?? string.Empty);
                var checkedOrderModel = new CheckedOrderModel(CheckingType.MissingCalculate, santei, checkedContent, tenMstModel.ItemCd, tenMstModel.SinKouiKbn, tenMstModel.Name ?? String.Empty, 0);

                checkedOrderModelList.Add(checkedOrderModel);

                return checkedOrderModelList;
            }

            return checkedOrderModelList;
        }

        public List<CheckedOrderModel> InitPriorityCheckDetail(List<CheckedOrderModel> checkedOrderModelList)
        {
            bool igakuNanbyoChecked = checkedOrderModelList.Any(c => c.ItemCd == ItemCdConst.IgakuNanbyo && c.Santei);
            if (igakuNanbyoChecked)
            {
                var uncheckedList = checkedOrderModelList.FindAll(c => c.ItemCd == ItemCdConst.IgakuTokusitu ||
                c.ItemCd == ItemCdConst.SiHifuToku2 ||
                c.ItemCd == ItemCdConst.IgakuTenkan ||
                c.ItemCd == ItemCdConst.SiHifuToku1);
                foreach (var checkModel in uncheckedList)
                {
                    checkModel.ChangeSantei(false);
                }
            }

            bool igakuTenkanChecked = checkedOrderModelList.Any(c => c.ItemCd == ItemCdConst.IgakuTenkan && c.Santei);
            if (igakuTenkanChecked)
            {
                var uncheckedList = checkedOrderModelList.FindAll(c => c.ItemCd == ItemCdConst.IgakuTokusitu ||
                c.ItemCd == ItemCdConst.SiHifuToku2 ||
                c.ItemCd == ItemCdConst.SiHifuToku1);
                foreach (var checkModel in uncheckedList)
                {
                    checkModel.ChangeSantei(false);
                }
            }

            bool sihifuToku1Checked = checkedOrderModelList.Any(c => c.ItemCd == ItemCdConst.SiHifuToku1 && c.Santei);
            if (sihifuToku1Checked)
            {
                var uncheckedList = checkedOrderModelList.FindAll(c => c.ItemCd == ItemCdConst.IgakuTokusitu ||
                c.ItemCd == ItemCdConst.SiHifuToku2);
                foreach (var checkModel in uncheckedList)
                {
                    checkModel.ChangeSantei(false);
                }
            }

            bool igakuTokusituChecked = checkedOrderModelList.Any(c => c.ItemCd == ItemCdConst.IgakuTokusitu && c.Santei);
            if (igakuTokusituChecked)
            {
                var sihifuToku2 = checkedOrderModelList.FirstOrDefault(c => c.ItemCd == ItemCdConst.SiHifuToku2);
                if (sihifuToku2 != null)
                {
                    sihifuToku2.ChangeSantei(false);
                }
            }

            return checkedOrderModelList;
        }

        public List<CheckedOrderModel> TouyakuTokusyoSyoho(int hpId, int sinDate, int hokenId, List<PtDiseaseModel> ByomeiModelList, List<OrdInfDetailModel> allOdrInfDetail, List<OrdInfModel> allOdrInf)
        {


            var checkedOrderModelList = new List<CheckedOrderModel>();

            var touyakuTokuSyoSyohoItem = allOdrInfDetail.FirstOrDefault(detail => detail.ItemCd == ItemCdConst.TouyakuTokuSyo1Syoho
                                                                                || detail.ItemCd == ItemCdConst.TouyakuTokuSyo2Syoho
                                                                                || detail.ItemCd == ItemCdConst.TouyakuTokuSyo1Syohosen
                                                                                || detail.ItemCd == ItemCdConst.TouyakuTokuSyo2Syohosen);

            // 既に入力されている場合は不要
            if (touyakuTokuSyoSyohoItem != null)
            {
                return checkedOrderModelList;
            }

            List<OrdInfModel> checkedOdrList;
            string itemTokusyoCd2;
            string itemTokusyoCd1;
            int inoutKbn;

            var checkedDetail = new List<OrdInfDetailModel>();
            var outDrug = allOdrInf.Where(o => o.IsDrug && o.InoutKbn == 1);
            if (outDrug.Count() > 0)
            {
                // Contains OutDrug
                checkedOdrList = allOdrInf.Where(o => o.IsDrug).ToList();
                itemTokusyoCd2 = ItemCdConst.TouyakuTokuSyo2Syohosen;
                itemTokusyoCd1 = ItemCdConst.TouyakuTokuSyo1Syohosen;
                inoutKbn = 1;
            }
            else
            {
                // Contains InDrug only
                checkedOdrList = allOdrInf.Where(o => o.IsDrug && o.InoutKbn == 0).ToList();
                if (checkedOdrList.Count() == 0)
                {
                    return checkedOrderModelList;
                }
                itemTokusyoCd2 = ItemCdConst.TouyakuTokuSyo2Syoho;
                itemTokusyoCd1 = ItemCdConst.TouyakuTokuSyo1Syoho;
                inoutKbn = 0;
            }

            CheckedOrderModel? checkedOdr = null;
            bool isCheckShuByomeiOnly2 = NoTrackingDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == 2002 && p.GrpEdaNo == 2)?.Val == 1;
            bool isCheckTeikyoByomei2 = NoTrackingDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == 2002 && p.GrpEdaNo == 3)?.Val == 1;
            bool isCheckShuByomeiOnly1 = NoTrackingDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == 2002 && p.GrpEdaNo == 0)?.Val == 1;
            bool isCheckTeikyoByomei1 = NoTrackingDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == 2002 && p.GrpEdaNo == 1)?.Val == 1;

            foreach (var odrInf in checkedOdrList)
            {
                var usageItem = odrInf.OrdInfDetails.FirstOrDefault(d => d.SinKouiKbn == 21);
                var drugItems = odrInf.OrdInfDetails.Where(d => d.IsDrug);
                if (usageItem != null && usageItem.Suryo >= 28)
                {
                    foreach (var drug in drugItems)
                    {
                        var checkedMoreThan28DaysOdr = CheckByoMei(hpId, sinDate, hokenId, isCheckShuByomeiOnly2, isCheckTeikyoByomei2, itemTokusyoCd2, drug.ItemCd, inoutKbn, ByomeiModelList);
                        if (!(checkedMoreThan28DaysOdr.CheckingType == 0 && string.IsNullOrEmpty(checkedMoreThan28DaysOdr.ItemCd) && string.IsNullOrEmpty(checkedMoreThan28DaysOdr.ItemName) && string.IsNullOrEmpty(checkedMoreThan28DaysOdr.CheckingContent)))
                        {
                            // having item with usage day >= 28, just break
                            checkedOrderModelList.Add(checkedMoreThan28DaysOdr);
                            return checkedOrderModelList;
                        }
                    }
                }
                // just check item have suryo < 28 one time
                if (checkedOdr != null) continue;
                foreach (var drug in drugItems)
                {
                    checkedOdr = CheckByoMei(hpId, sinDate, hokenId, isCheckShuByomeiOnly1, isCheckTeikyoByomei1, itemTokusyoCd1, drug.ItemCd, inoutKbn, ByomeiModelList);
                    if (!(checkedOdr.CheckingType == 0 && string.IsNullOrEmpty(checkedOdr.ItemCd) && string.IsNullOrEmpty(checkedOdr.ItemName) && string.IsNullOrEmpty(checkedOdr.CheckingContent)))
                    {
                        break;
                    }
                }
            }
            if (checkedOdr != null && checkedOdr.CheckingType != 0 && !string.IsNullOrEmpty(checkedOdr.CheckingContent))
            {
                checkedOrderModelList.Add(checkedOdr);
                return checkedOrderModelList;
            }

            // 算定回数チェック（1日1回、月2回）
            // 背反設定されている場合は不可

            return checkedOrderModelList;
        }

        public CheckedOrderModel CheckByoMei(int hpId, int sinDate, int hokenId, bool isCheckShuByomeiOnly, bool isCheckTeikyoByomei, string itemTokusyoCd, string itemCd, int inoutKbn, List<PtDiseaseModel> ByomeiModelList)
        {
            var tenMstModel = FindTenMst(hpId, itemTokusyoCd, sinDate);

            var byoMeiSpecialList = ByomeiModelList
                            .Where(b => (isCheckShuByomeiOnly ? b.SyubyoKbn == 1 : true) &&
                                b.SikkanKbn == SikkanKbnConst.Special &&
                                (b.HokenPid == hokenId || b.HokenPid == 0) &&
                                b.StartDate <= sinDate &&
                                (b.TenkiKbn == TenkiKbnConst.Continued || b.TenkiDate > sinDate))
                            .ToList();
            if (byoMeiSpecialList.Count > 0)
            {
                bool isSantei = false;
                if (isCheckTeikyoByomei)
                {
                    var byomeiCdList = byoMeiSpecialList.Select(b => b.ByomeiCd);
                    var byomeiCdTenkiouList = GetByomeiCdFromTenkiou(hpId, itemCd, true);
                    foreach (var byomeiCd in byomeiCdTenkiouList)
                    {
                        if (byomeiCdList.Contains(byomeiCd))
                        {
                            isSantei = true;
                            break;
                        }
                    }
                }
                else
                {
                    isSantei = true;
                }
                if (isSantei)
                {
                    var santei = false;
                    var touyaku = NoTrackingDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == 4001 && p.GrpEdaNo == 1)?.Val;
                    if (touyaku == 1)
                    {
                        santei = true;
                    }
                    var checkedContent = FormatSanteiMessage(tenMstModel.Name ?? string.Empty);
                    var checkedOrderModel = new CheckedOrderModel(CheckingType.MissingCalculate, santei, checkedContent, tenMstModel?.ItemCd ?? string.Empty, tenMstModel?.SinKouiKbn ?? 0, tenMstModel?.Name ?? string.Empty, inoutKbn);

                    return checkedOrderModel;
                }
            }

            var byoMeiOtherList = ByomeiModelList
                            .Where(b => (isCheckShuByomeiOnly ? b.SyubyoKbn == 1 : true) &&
                                b.SikkanKbn == SikkanKbnConst.Other &&
                                (b.HokenPid == hokenId || b.HokenPid == 0) &&
                                b.StartDate <= sinDate &&
                                (b.TenkiKbn == TenkiKbnConst.Continued || b.TenkiDate > sinDate))
                            .ToList();
            if (byoMeiOtherList.Count > 0)
            {
                bool isSantei = false;
                if (isCheckTeikyoByomei)
                {
                    var byomeiCdList = byoMeiOtherList.Select(b => b.ByomeiCd);
                    var byomeiCdFromTenkiouList = GetByomeiCdFromTenkiou(hpId, itemCd, true);
                    foreach (var byomeiCdFromTenkiou in byomeiCdFromTenkiouList)
                    {
                        if (byomeiCdList.Contains(byomeiCdFromTenkiou))
                        {
                            isSantei = true;
                            break;
                        }
                    }
                }
                else
                {
                    isSantei = true;
                }
                if (isSantei)
                {
                    var checkedContent = FormatSanteiMessage(tenMstModel.Name ?? string.Empty);
                    var checkedOrderModel = new CheckedOrderModel(CheckingType.MissingCalculate, false, checkedContent, tenMstModel?.ItemCd ?? string.Empty, tenMstModel?.SinKouiKbn ?? 0, tenMstModel?.Name ?? string.Empty, inoutKbn);

                    return checkedOrderModel;
                }
            }
            return new CheckedOrderModel();
        }

        private List<string> GetByomeiCdFromTenkiou(int hpId, string itemCd, bool isFromCheckingView = false)
        {
            List<string> result;
            var teikyoByomeis = NoTrackingDataContext.TekiouByomeiMsts.Where(
                (x) => x.HpId == hpId && x.ItemCd == itemCd && (!isFromCheckingView || x.IsInvalidTokusyo != 1));
            var byomeiMsts = NoTrackingDataContext.ByomeiMsts.Where(
                (x) => x.HpId == hpId);
            var query = from teikyoByomei in teikyoByomeis
                        join byomeiMst in byomeiMsts on
                        teikyoByomei.ByomeiCd equals byomeiMst.ByomeiCd
                        select new
                        {
                            TeikyoByomei = teikyoByomei,
                            ByomeiMst = byomeiMst
                        };
            result = query.AsEnumerable()
                          .Select(x => x.TeikyoByomei.ByomeiCd)
                          .ToList();

            return result;
        }

        public List<CheckedOrderModel> ChikiHokatu(int hpId, long ptId, int userId, int sinDate, int primaryDoctor, int tantoId, List<OrdInfDetailModel> allOdrInfDetail, int syosaisinKbn)
        {
            var checkedOrderModelList = new List<CheckedOrderModel>();
            var tikiHokatu = NoTrackingDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == 4001 && p.GrpEdaNo == 8)?.Val ?? 0;

            var tikiHokatuItem = allOdrInfDetail.FirstOrDefault(detail => detail.ItemCd == ItemCdConst.SaisinTiikiHoukatu1
                                                                        || detail.ItemCd == ItemCdConst.SaisinTiikiHoukatu2
                                                                        || detail.ItemCd == ItemCdConst.SaisinNintiTiikiHoukatu1
                                                                        || detail.ItemCd == ItemCdConst.SaisinNintiTiikiHoukatu2);

            // 既に入力されている場合は不要
            if (tikiHokatuItem != null)
            {
                return checkedOrderModelList;
            }

            // 再診のとき、算定可
            if (syosaisinKbn != SyosaiConst.Saisin)
            {
                return checkedOrderModelList;
            }

            // 地域包括対象疾病の患者である
            var ptSanteiConfList = GetPtCalculationInfById(hpId, ptId, sinDate);
            List<string> santeiItemCds = new List<string>();

            var tiikiSanteiConf = ptSanteiConfList.FirstOrDefault(c => c.Item1 == 3 && c.Item2 == 1);
            if (tiikiSanteiConf != null)
            {
                //SystemSetting.TikiHokatu = 1 --> 加算１
                //                         = 0 --> 加算２
                //                         = 0 --> Default 加算２
                if (tikiHokatu == 1)
                {
                    santeiItemCds.Add(ItemCdConst.SaisinTiikiHoukatu1);
                }
                else
                {
                    santeiItemCds.Add(ItemCdConst.SaisinTiikiHoukatu2);
                }
            }

            var ninTiikiSanteiConf = ptSanteiConfList.FirstOrDefault(c => c.Item1 == 3 && c.Item2 == 2);
            if (ninTiikiSanteiConf != null)
            {
                //SystemSetting.TikiHokatu = 1 --> 加算１
                //                         = 0 --> 加算２
                //                         = 0 --> Default 加算２
                if (tikiHokatu == 1)
                {
                    santeiItemCds.Add(ItemCdConst.SaisinNintiTiikiHoukatu1);
                }
                else
                {
                    santeiItemCds.Add(ItemCdConst.SaisinNintiTiikiHoukatu2);
                }
            }

            if (santeiItemCds.Count == 0)
            {
                return checkedOrderModelList;
            }

            // 主治医の設定がある
            if (primaryDoctor == 0)
            {
                return checkedOrderModelList;
            }
            string userSName = NoTrackingDataContext.UserMsts.FirstOrDefault(u => u.UserId == userId && (sinDate <= 0 || u.StartDate <= sinDate && u.EndDate >= sinDate))?.Sname ?? string.Empty;
            if (string.IsNullOrEmpty(userSName))
            {
                return checkedOrderModelList;
            }

            if (primaryDoctor != tantoId)
            {
                return checkedOrderModelList;
            }

            var oshinDetails = allOdrInfDetail.Where(d => d.SinKouiKbn == 14);
            foreach (var detail in oshinDetails)
            {
                var tenMst = FindTenMst(hpId, detail.ItemCd, sinDate);
                //往診...TEN_MST.CD_KBN='C' and CD_KBNNO=0 and KOKUJI2=1
                if (tenMst.CdKbn == "C" && tenMst.CdKbnno == 0 && tenMst.Kokuji2 == "1")
                {
                    return checkedOrderModelList;
                }
                //在宅患者訪問診療料（Ⅰ）（Ⅱ）...TEN_MST.CD_KBN='C' and CD_KBNNO=0 and CD_EDANO in (0,2) and KOKUJI2=1
                if (tenMst.CdKbn == "C" && tenMst.CdKbnno == 1 && new List<int> { 0, 2 }.Contains(tenMst.CdEdano) && tenMst.Kokuji2 == "1")
                {
                    return checkedOrderModelList;
                }
            }

            var tikiHokatuJidoSantei = NoTrackingDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == 4001 && p.GrpEdaNo == 4)?.Val ?? 0;
            foreach (var itemCd in santeiItemCds)
            {
                var tenMstModel = FindTenMst(hpId, itemCd, sinDate);
                if (string.IsNullOrEmpty(tenMstModel.ItemCd) && tenMstModel.HpId == 0)
                {
                    continue;
                }

                var santei = false;
                if (tikiHokatuJidoSantei == 1)
                {
                    santei = true;
                }
                var checkedContent = FormatSanteiMessage(tenMstModel.Name ?? string.Empty);
                var checkedOrderModel = new CheckedOrderModel(CheckingType.MissingCalculate, santei, checkedContent, tenMstModel?.ItemCd ?? string.Empty, tenMstModel?.SinKouiKbn ?? 0, tenMstModel?.Name ?? string.Empty, 0);

                checkedOrderModelList.Add(checkedOrderModel);
            }

            return checkedOrderModelList;
        }


        private List<Tuple<int, int>> GetPtCalculationInfById(int hpId, long ptId, int sinDate)
        {
            return NoTrackingDataContext.PtSanteiConfs
                .Where(pt =>
                    pt.HpId == hpId && pt.PtId == ptId && pt.StartDate <= sinDate && pt.EndDate >= sinDate && pt.IsDeleted == 0)
                .AsEnumerable()
                .Select(item => new Tuple<int, int>(item.KbnNo, item.EdaNo)).ToList();
        }

        public List<CheckedOrderModel> YakkuZai(int hpId, long ptId, int sinDate, int birthDay, List<OrdInfDetailModel> allOdrInfDetail, List<OrdInfModel> allOdrInf)
        {
            var checkedOrderModelList = new List<CheckedOrderModel>();
            var yakkuzaiJoho = NoTrackingDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == 4001 && p.GrpEdaNo == 5)?.Val;

            var yakuzaiItem = allOdrInfDetail.FirstOrDefault(detail => detail.ItemCd == ItemCdConst.YakuzaiJoho);

            // 既に入力されている場合は不要
            if (yakuzaiItem != null)
            {
                return checkedOrderModelList;
            }

            var existInOrder = allOdrInf.Any(odr => odr.IsDrug && odr.InoutKbn == 0);
            if (!existInOrder)
            {
                return checkedOrderModelList;
            }

            var shonikaSetting = NoTrackingDataContext.SystemGenerationConfs.FirstOrDefault(p => p.HpId == hpId
                    && p.GrpCd == 8001
                    && p.GrpEdaNo == 0
                    && p.StartDate <= sinDate
                    && p.EndDate >= sinDate)?.Val;
            // 小児科外来診療料算定あり、3歳未満の場合は不可
            if (shonikaSetting == 1)
            {
                // 小児科外来診療料を算定しない
                var autoSanteiItem = FindAutoSanteiMst(hpId, ItemCdConst.IgakuSyouniGairaiSyosinKofuAri, sinDate);
                if (autoSanteiItem)
                {
                    // 3歳未満の場合は不可
                    int age = CIUtil.SDateToAge(birthDay, sinDate);
                    if (age > 0 && age <= 3)
                    {
                        return checkedOrderModelList;
                    }
                }
            }

            var tenMstModel = FindTenMst(hpId, ItemCdConst.YakuzaiJoho, sinDate);
            if (string.IsNullOrEmpty(tenMstModel.ItemCd))
            {
                return checkedOrderModelList;
            }

            var tenMstTeiyoModel = FindTenMst(hpId, ItemCdConst.YakuzaiJohoTeiyo, sinDate);
            if (string.IsNullOrEmpty(tenMstTeiyoModel.ItemCd))
            {
                return checkedOrderModelList;
            }

            bool santei = false;
            if (yakkuzaiJoho == 1 || yakkuzaiJoho == 2)
            {
                santei = true;
            }
            var checkedContent = FormatSanteiMessage(tenMstModel.Name ?? string.Empty);

            var checkedOrderModel = new CheckedOrderModel(CheckingType.MissingCalculate, santei, checkedContent, tenMstModel?.ItemCd ?? string.Empty, tenMstModel?.SinKouiKbn ?? 0, tenMstModel?.Name ?? string.Empty, 0);

            checkedOrderModelList.Add(checkedOrderModel);

            if (yakkuzaiJoho == 2)
            {
                santei = true;
            }
            checkedContent = FormatSanteiMessage(tenMstTeiyoModel.Name ?? string.Empty);

            var checkedOrderTeiyoModel = new CheckedOrderModel(CheckingType.MissingCalculate, santei, checkedContent, tenMstTeiyoModel?.ItemCd ?? string.Empty, tenMstTeiyoModel?.SinKouiKbn ?? 0, tenMstTeiyoModel?.Name ?? string.Empty, 0);
            checkedOrderModelList.Add(checkedOrderTeiyoModel);

            // 当月すでに1回以上算定されている場合、チェックOFFで表示する
            int santeiCount = GetSanteiCountInMonth(hpId, ptId, sinDate, new List<string>() { ItemCdConst.YakuzaiJoho, ItemCdConst.YakuzaiJohoTeiyo });
            //（基本的に、月１ 回に限りだが、処方の内容に変更があった場合は、その都度算定できるため）

            var result = new List<CheckedOrderModel>();
            if (santeiCount >= 1)
            {
                foreach (var checkModel in checkedOrderModelList)
                {
                    result.Add(checkModel.ChangeSantei(false));
                }
            }
            else
            {
                result.AddRange(checkedOrderModelList);
            }

            return result;
        }

        private bool FindAutoSanteiMst(int hpId, string itemCd, int sinDate)
        {
            var check = NoTrackingDataContext.AutoSanteiMsts.Any(e =>
                 e.HpId == hpId &&
                 e.ItemCd == itemCd &&
                 e.StartDate <= sinDate &&
                 e.EndDate >= sinDate);

            return check;
        }

        public List<CheckedOrderModel> SiIkuji(int hpId, int sinDate, int birthDay, List<OrdInfDetailModel> allOdrInfDetail, bool isJouhou, int syosaisinKbn)
        {
            var checkedOrderModelList = new List<CheckedOrderModel>();

            var siIkujiItem = allOdrInfDetail.FirstOrDefault(detail => detail.ItemCd == ItemCdConst.SiIkuji || detail.ItemCd == ItemCdConst.SiIkujiJyohoTusin);

            // 既に入力されている場合は不要
            if (siIkujiItem != null)
            {
                return checkedOrderModelList;
            }

            TenMst? tenMstModel = null;
            if (isJouhou)
            {
                if (sinDate >= 20220401)
                {
                    tenMstModel = FindTenMst(hpId, ItemCdConst.SiIkujiJyohoTusin, sinDate);
                    if (string.IsNullOrEmpty(tenMstModel.ItemCd))
                    {
                        return checkedOrderModelList;
                    }
                }
                else
                {
                    return checkedOrderModelList;
                }
            }
            else
            {
                tenMstModel = FindTenMst(hpId, ItemCdConst.SiIkuji, sinDate);
                if (string.IsNullOrEmpty(tenMstModel.ItemCd))
                {
                    return checkedOrderModelList;
                }
            }

            var shonika = NoTrackingDataContext.SystemGenerationConfs.FirstOrDefault(p => p.HpId == hpId
                    && p.GrpCd == 8001
                    && p.GrpEdaNo == 0
                    && p.StartDate <= sinDate
                    && p.EndDate >= sinDate)?.Val ?? 0;
            // 小児科外来診療料算定あり、3歳未満の場合は不可
            if (shonika != 1)
            {
                // 小児科外来診療料算定しない
                return checkedOrderModelList;
            }
            var autoSanteiItem = FindAutoSanteiMst(hpId, ItemCdConst.IgakuSyouniGairaiSyosinKofuAri, sinDate);
            if (autoSanteiItem)
            {
                // 自動算定されるため、算定不可
                return checkedOrderModelList;
            }

            // ３歳未満の乳幼児
            int age = CIUtil.SDateToAge(birthDay, sinDate);
            if (age > 3)
            {
                return checkedOrderModelList;
            }

            // 初診時
            // 初診以上の場合は算定不可
            if (syosaisinKbn != SyosaiConst.Syosin &&
                syosaisinKbn != SyosaiConst.Syosin2)
            {
                return checkedOrderModelList;
            }

            // 背反設定されている場合は不可
            var checkedContent = FormatSanteiMessage(tenMstModel.Name ?? string.Empty);
            var checkedOrderModel = new CheckedOrderModel(CheckingType.MissingCalculate, true, checkedContent, tenMstModel?.ItemCd ?? string.Empty, tenMstModel?.SinKouiKbn ?? 0, tenMstModel?.Name ?? string.Empty, 0);

            checkedOrderModelList.Add(checkedOrderModel);

            return checkedOrderModelList;
        }

        public List<CheckedOrderModel> Zanyaku(int hpId, int sinDate, List<OrdInfDetailModel> allOdrInfDetail, List<OrdInfModel> allOrderInf)
        {
            var checkedOrderModelList = new List<CheckedOrderModel>();

            var existZanyakuItem = allOdrInfDetail.Any(detail => detail.ItemCd == ItemCdConst.ZanGigi || detail.ItemCd == ItemCdConst.ZanTeiKyo);

            // 既に入力されている場合は不要
            if (existZanyakuItem)
            {
                return checkedOrderModelList;
            }

            //院外処方オーダーがあるかチェック（算定外、自費含む）
            var existOutOrder = allOrderInf.Any(odr => odr.IsDrug && odr.InoutKbn == 1);
            if (!existOutOrder)
            {
                return checkedOrderModelList;
            }

            var zanyakuSetting = NoTrackingDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == 2012 && p.GrpEdaNo == 0)?.Val;

            var zangigiTenMstModel = FindTenMst(hpId, ItemCdConst.ZanGigi, sinDate);
            if (zangigiTenMstModel != null)
            {
                var santei = zanyakuSetting == 1;
                var checkedContent = $"\"{zangigiTenMstModel.Name}\"を指示する。";
                var checkedOrderModelZangigi = new CheckedOrderModel(CheckingType.Order, santei, checkedContent, zangigiTenMstModel?.ItemCd ?? string.Empty, zangigiTenMstModel?.SinKouiKbn ?? 0, zangigiTenMstModel?.Name ?? string.Empty, 1);

                checkedOrderModelList.Add(checkedOrderModelZangigi);
            }

            var zanteikyotenMstModel = FindTenMst(hpId, ItemCdConst.ZanTeiKyo, sinDate);
            if (zanteikyotenMstModel != null)
            {
                var santei = zanyakuSetting == 2;
                var checkedContent = $"\"{zanteikyotenMstModel.Name}\"を指示する。";
                var checkedOrderModelZanTeikyo = new CheckedOrderModel(CheckingType.Order, santei, checkedContent, zanteikyotenMstModel?.ItemCd ?? string.Empty, zanteikyotenMstModel?.SinKouiKbn ?? 0, zanteikyotenMstModel?.Name ?? string.Empty, 1);

                checkedOrderModelList.Add(checkedOrderModelZanTeikyo);
            }

            return checkedOrderModelList;
        }

        public List<CheckedOrderModel> TrialIryoJyohoKibanCalculation(int hpId, long ptId, int sinDate, long raiinNo, List<OrdInfDetailModel> allOdrInfDetail)
        {
            List<CheckedOrderModel> checkingOrderModelList = new List<CheckedOrderModel>();
            var autoSanteiItem = FindAutoSanteiMst(hpId, ItemCdConst.SyosinIryoJyohoKiban1, sinDate);
            if (!autoSanteiItem)
            {
                return checkingOrderModelList;
            }

            var existAutoItem = allOdrInfDetail.Any(detail => ItemCdConst.IryoJyohoKibanList.Contains(detail.ItemCd));
            if (existAutoItem)
            {
                return checkingOrderModelList;
            }

            bool isExistFirstVisit = allOdrInfDetail.Any(x => x.ItemCd == ItemCdConst.SyosaiKihon && x.Suryo == 1);
            bool isExistReturnVisit = allOdrInfDetail.Any(x => x.ItemCd == ItemCdConst.SyosaiKihon && x.Suryo == 3);
            bool isGairaiRiha = CheckGairaiRiha(hpId, ptId, sinDate, raiinNo, allOdrInfDetail);
            if (isExistFirstVisit)
            {
                var FirstVisitDevelopmentSystemEnhanceAdd1TenMstModel = FindTenMst(hpId, ItemCdConst.SyosinIryoJyohoKiban1, sinDate);
                if (FirstVisitDevelopmentSystemEnhanceAdd1TenMstModel != null)
                {
                    CheckedOrderModel checkingOrderModel = new CheckedOrderModel(
                        CheckingType.MissingCalculate,
                        santei: true,
                        checkingContent: FormatSanteiMessage(FirstVisitDevelopmentSystemEnhanceAdd1TenMstModel.Name ?? string.Empty),
                        itemCd: FirstVisitDevelopmentSystemEnhanceAdd1TenMstModel.ItemCd,
                        sinKouiKbn: FirstVisitDevelopmentSystemEnhanceAdd1TenMstModel.SinKouiKbn,
                        itemName: FirstVisitDevelopmentSystemEnhanceAdd1TenMstModel.Name ?? string.Empty,
                        inOutKbn: 0
                        );

                    checkingOrderModelList.Add(checkingOrderModel);
                }

                var MedicalDevelopmentSystemEnhanceAdd1TenMstModel = FindTenMst(hpId, ItemCdConst.IgakuIryoJyohoKiban1, sinDate);
                if (MedicalDevelopmentSystemEnhanceAdd1TenMstModel != null)
                {
                    CheckedOrderModel checkingOrderModel = new CheckedOrderModel(
                        CheckingType.MissingCalculate,
                        santei: true,
                        checkingContent: FormatSanteiMessage(MedicalDevelopmentSystemEnhanceAdd1TenMstModel.Name ?? string.Empty),
                        itemCd: MedicalDevelopmentSystemEnhanceAdd1TenMstModel.ItemCd,
                        sinKouiKbn: MedicalDevelopmentSystemEnhanceAdd1TenMstModel.SinKouiKbn,
                        itemName: MedicalDevelopmentSystemEnhanceAdd1TenMstModel.Name ?? string.Empty,
                        inOutKbn: 0
                        );

                    checkingOrderModelList.Add(checkingOrderModel);
                }
            }
            else if (isExistReturnVisit || isGairaiRiha)
            {
                var VisitDevelopmentSystemEnhanceAdd3TenMstModel = FindTenMst(hpId, ItemCdConst.SaisinIryoJyohoKiban3, sinDate);
                if (VisitDevelopmentSystemEnhanceAdd3TenMstModel != null)
                {
                    CheckedOrderModel checkingOrderModel = new CheckedOrderModel(
                        CheckingType.MissingCalculate,
                        santei: true,
                        checkingContent: FormatSanteiMessage(VisitDevelopmentSystemEnhanceAdd3TenMstModel.Name ?? string.Empty),
                        itemCd: VisitDevelopmentSystemEnhanceAdd3TenMstModel.ItemCd,
                        sinKouiKbn: VisitDevelopmentSystemEnhanceAdd3TenMstModel.SinKouiKbn,
                        itemName: VisitDevelopmentSystemEnhanceAdd3TenMstModel.Name ?? string.Empty,
                        inOutKbn: 0
                        );

                    checkingOrderModelList.Add(checkingOrderModel);
                }

                var ReturnVisitDevelopmentSystemEnhanceAdd3TenMstModel = FindTenMst(hpId, ItemCdConst.IgakuIryoJyohoKiban3, sinDate);
                if (ReturnVisitDevelopmentSystemEnhanceAdd3TenMstModel != null)
                {
                    CheckedOrderModel checkingOrderModel = new CheckedOrderModel(
                        CheckingType.MissingCalculate,
                        santei: true,
                        checkingContent: FormatSanteiMessage(ReturnVisitDevelopmentSystemEnhanceAdd3TenMstModel.Name ?? string.Empty),
                        itemCd: ReturnVisitDevelopmentSystemEnhanceAdd3TenMstModel.ItemCd,
                        sinKouiKbn: ReturnVisitDevelopmentSystemEnhanceAdd3TenMstModel.SinKouiKbn,
                        itemName: ReturnVisitDevelopmentSystemEnhanceAdd3TenMstModel.Name ?? string.Empty,
                        inOutKbn: 0
                        );

                    checkingOrderModelList.Add(checkingOrderModel);
                }
            }
            return checkingOrderModelList;
        }

        private bool CheckGairaiRiha(int hpId, long ptId, int sinDate, long raiinNo, List<OrdInfDetailModel> allOdrInfDetail)
        {
            if (_systemConf.GetSettingValue(2016, 0, hpId) == 0)
            {
                return false;
            }

            if (allOdrInfDetail.Any(x => x.ItemCd == ItemCdConst.SyosaiKihon && x.Suryo == 0))
            {
                // 既に存在
                if (allOdrInfDetail.Any(detail => detail.ItemCd == ItemCdConst.IgakuGairaiRiha1
                                                || detail.ItemCd == ItemCdConst.IgakuGairaiRiha2))
                {
                    return true;
                }

                // 外来リハビリテーション診療料１
                int lastDaySanteiRiha1 = _todayOdrRepository.GetLastDaySantei(hpId, ptId, sinDate, raiinNo, ItemCdConst.IgakuGairaiRiha1);
                if (lastDaySanteiRiha1 != 0)
                {
                    int tgtDay = CIUtil.SDateInc(lastDaySanteiRiha1, 6);
                    if (lastDaySanteiRiha1 <= sinDate && tgtDay >= sinDate)
                    {
                        return true;
                    }
                }

                // 外来リハビリテーション診療料２
                int lastDaySanteiRiha2 = _todayOdrRepository.GetLastDaySantei(hpId, ptId, sinDate, raiinNo, ItemCdConst.IgakuGairaiRiha2);
                if (lastDaySanteiRiha2 != 0)
                {
                    int tgtDay = CIUtil.SDateInc(lastDaySanteiRiha2, 13);
                    if (lastDaySanteiRiha2 <= sinDate && tgtDay >= sinDate)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public (List<string>, List<SinKouiCountModel>) GetCheckedAfter327Screen(int hpId, long ptId, int sinDate, List<CheckedOrderModel> checkedTenMstResult, bool isTokysyoOrder, bool isTokysyosenOrder)
        {
            #region Checking
            // 特定疾患処方管理加算２（処方料）・算定
            List<string> msgs = new();
            var tokysyoItem = checkedTenMstResult.FirstOrDefault(d => d.ItemCd == ItemCdConst.TouyakuTokuSyo2Syoho);
            List<SinKouiCountModel> lastSanteiInMonth = new List<SinKouiCountModel>();
            if (tokysyoItem != null && tokysyoItem.Santei)
            {
                TenMst? touyakuTokuSyo1Syoho = null;
                // 当月すでに1回以上算定されている場合、警告を表示する
                lastSanteiInMonth = GetSinkouCountInMonth(hpId, ptId, sinDate, ItemCdConst.TouyakuTokuSyo1Syoho);
                if (lastSanteiInMonth.Count == 0)
                {
                    lastSanteiInMonth = GetSinkouCountInMonth(hpId, ptId, sinDate, ItemCdConst.TouyakuTokuSyo1Syohosen);
                    if (lastSanteiInMonth.Count > 0)
                    {
                        touyakuTokuSyo1Syoho = FindTenMst(hpId, ItemCdConst.TouyakuTokuSyo1Syohosen, sinDate);
                    }
                }
                else
                {
                    touyakuTokuSyo1Syoho = FindTenMst(hpId, ItemCdConst.TouyakuTokuSyo1Syoho, sinDate);
                }
                if (lastSanteiInMonth.Count > 0)
                {
                    var touyakuTokuSyo2Syoho = FindTenMst(hpId, ItemCdConst.TouyakuTokuSyo2Syoho, sinDate);
                    if (touyakuTokuSyo1Syoho != null && touyakuTokuSyo2Syoho != null)
                    {
                        msgs.Insert(0, BuildMessage(touyakuTokuSyo1Syoho.Name ?? string.Empty, touyakuTokuSyo2Syoho.Name ?? string.Empty, SanteiDateFormat(lastSanteiInMonth)));
                    }
                }
            }

            // 特定疾患処方管理加算２（処方料）・オーダー
            if (isTokysyoOrder)
            {
                TenMst? touyakuTokuSyo1Syoho = null;
                // 当月すでに1回以上算定されている場合、警告を表示する
                lastSanteiInMonth = GetSinkouCountInMonth(hpId, ptId, sinDate, ItemCdConst.TouyakuTokuSyo1Syoho);
                if (lastSanteiInMonth.Count == 0)
                {
                    lastSanteiInMonth = GetSinkouCountInMonth(hpId, ptId, sinDate, ItemCdConst.TouyakuTokuSyo1Syohosen);
                    if (lastSanteiInMonth.Count > 0)
                    {
                        touyakuTokuSyo1Syoho = FindTenMst(hpId, ItemCdConst.TouyakuTokuSyo1Syohosen, sinDate);
                    }
                }
                else
                {
                    touyakuTokuSyo1Syoho = FindTenMst(hpId, ItemCdConst.TouyakuTokuSyo1Syoho, sinDate);
                }

                if (lastSanteiInMonth.Count > 0)
                {
                    var touyakuTokuSyo2Syoho = FindTenMst(hpId, ItemCdConst.TouyakuTokuSyo2Syoho, sinDate);
                    if (touyakuTokuSyo1Syoho != null && touyakuTokuSyo2Syoho != null)
                    {
                        msgs.Insert(1, BuildMessage(touyakuTokuSyo1Syoho.Name ?? string.Empty, touyakuTokuSyo2Syoho.Name ?? string.Empty, SanteiDateFormat(lastSanteiInMonth)));
                    }
                }
            }

            // 特定疾患処方管理加算２（処方箋料）・算定
            var tokysyosenItem = checkedTenMstResult.FirstOrDefault(d => d.ItemCd == ItemCdConst.TouyakuTokuSyo2Syohosen);
            if (tokysyosenItem != null && tokysyosenItem.Santei)
            {
                TenMst? touyakuTokuSyo1Syohosen = null;
                // 当月すでに1回以上算定されている場合、警告を表示する
                lastSanteiInMonth = GetSinkouCountInMonth(hpId, ptId, sinDate, ItemCdConst.TouyakuTokuSyo1Syohosen);
                if (lastSanteiInMonth.Count == 0)
                {
                    lastSanteiInMonth = GetSinkouCountInMonth(hpId, ptId, sinDate, ItemCdConst.TouyakuTokuSyo1Syoho);
                    if (lastSanteiInMonth.Count > 0)
                    {
                        touyakuTokuSyo1Syohosen = FindTenMst(hpId, ItemCdConst.TouyakuTokuSyo1Syoho, sinDate);
                    }
                }
                else
                {
                    touyakuTokuSyo1Syohosen = FindTenMst(hpId, ItemCdConst.TouyakuTokuSyo1Syohosen, sinDate);
                }
                if (lastSanteiInMonth.Count > 0)
                {
                    var touyakuTokuSyo2Syohosen = FindTenMst(hpId, ItemCdConst.TouyakuTokuSyo2Syohosen, sinDate);
                    if (touyakuTokuSyo2Syohosen != null && touyakuTokuSyo2Syohosen != null)
                    {
                        msgs.Insert(2, BuildMessage(touyakuTokuSyo1Syohosen?.Name ?? string.Empty, touyakuTokuSyo2Syohosen.Name ?? string.Empty, SanteiDateFormat(lastSanteiInMonth)));
                    }
                }
            }

            // 特定疾患処方管理加算２（処方箋料）・オーダー
            if (isTokysyosenOrder)
            {
                TenMst? touyakuTokuSyo1Syohosen = null;
                // 当月すでに1回以上算定されている場合、警告を表示する
                lastSanteiInMonth = GetSinkouCountInMonth(hpId, ptId, sinDate, ItemCdConst.TouyakuTokuSyo1Syohosen);
                if (lastSanteiInMonth.Count == 0)
                {
                    lastSanteiInMonth = GetSinkouCountInMonth(hpId, ptId, sinDate, ItemCdConst.TouyakuTokuSyo1Syoho);
                    if (lastSanteiInMonth.Count > 0)
                    {
                        touyakuTokuSyo1Syohosen = FindTenMst(hpId, ItemCdConst.TouyakuTokuSyo1Syoho, sinDate);
                    }
                }
                else
                {
                    touyakuTokuSyo1Syohosen = FindTenMst(hpId, ItemCdConst.TouyakuTokuSyo1Syohosen, sinDate);
                }
                if (lastSanteiInMonth.Count > 0)
                {
                    var touyakuTokuSyo2Syohosen = FindTenMst(hpId, ItemCdConst.TouyakuTokuSyo2Syohosen, sinDate);
                    if (touyakuTokuSyo1Syohosen != null && touyakuTokuSyo2Syohosen != null)
                    {
                        msgs.Insert(3, BuildMessage(touyakuTokuSyo1Syohosen.Name ?? string.Empty, touyakuTokuSyo2Syohosen.Name ?? string.Empty, SanteiDateFormat(lastSanteiInMonth)));
                    }
                }
            }
            #endregion

            return new(msgs, lastSanteiInMonth);
        }

        string BuildMessage(string touyaku1Name, string touyaku2Name, string dateSantei)
        {
            StringBuilder msg = new StringBuilder();
            msg.Append("'");
            msg.Append(touyaku1Name);
            msg.Append("' ");
            msg.Append("が");
            msg.Append(dateSantei);
            msg.Append("に算定されているため、");
            msg.Append(Environment.NewLine);

            msg.Append("'");
            msg.Append(touyaku2Name);
            msg.Append("' ");
            msg.Append("を算定すると差額が発生します。");
            msg.Append(Environment.NewLine);

            msg.Append("'");
            msg.Append(touyaku2Name);
            msg.Append("' ");
            msg.Append("を算定しますか？");

            return msg.ToString();
        }

        string SanteiDateFormat(List<SinKouiCountModel> sinKouiCountList)
        {
            if (sinKouiCountList == null || sinKouiCountList.Count == 0)
            {
                return string.Empty;
            }
            var dateInt = sinKouiCountList.Select(d => d.SinDay + "日");
            return string.Join(", ", dateInt);
        }

        public List<SinKouiCountModel> GetSinkouCountInMonth(int hpId, long ptId, int sinDate, string itemCd)
        {
            int sinYM = sinDate / 100;
            int sinDay = sinDate - sinYM * 100;
            var sinKouiCountQuery = NoTrackingDataContext.SinKouiCounts
                .Where(s => s.HpId == hpId && s.PtId == ptId && s.SinYm == sinYM && s.SinDay < sinDay);

            var sinKouiDetailQuery = NoTrackingDataContext.SinKouiDetails
                .Where(s => s.HpId == hpId && s.PtId == ptId && itemCd == s.ItemCd);

            var resultQuery = from sinKouiCount in sinKouiCountQuery
                              join sinKouiDetail in sinKouiDetailQuery
                              on new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.RpNo, sinKouiCount.SinYm }
                              equals new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.RpNo, sinKouiDetail.SinYm }
                              select new
                              {
                                  SinKouiCount = sinKouiCount,
                              };
            var sinKouiCountList = resultQuery.AsEnumerable().Select(s => s.SinKouiCount);
            var santeiCount = sinKouiCountList.Sum(s => s.Count);
            if (santeiCount > 0)
            {
                return sinKouiCountList.OrderBy(s => s.SinDay).Select(d => new SinKouiCountModel(d.HpId, d.PtId, d.SinYm, d.SinDay, d.SinDay, d.RaiinNo, d.RpNo, d.SeqNo, d.Count)).ToList();
            }
            return new List<SinKouiCountModel>();
        }

        public Dictionary<string, DateTime> GetMaxAuditTrailLogDateForPrint(long ptID, int sinDate, long raiinNo)
        {
            Dictionary<string, DateTime> result = new Dictionary<string, DateTime>();

            List<string> eventCds = new List<string>();
            eventCds.Add(EventCode.ReportInDrug);
            eventCds.Add(EventCode.ReportDrugNoteSeal);
            eventCds.Add(EventCode.ReportYakutai);
            eventCds.Add(EventCode.ReportDrugInf);
            eventCds.Add(EventCode.ReportOutDrug);
            eventCds.Add(EventCode.ReportOrderLabel);
            eventCds.Add(EventCode.ReportSijisen);

            var auditTrailLogs = NoTrackingDataContext.AuditTrailLogs.Where(x =>
                            (x.EventCd != null && eventCds.Contains(x.EventCd)) &&
                            x.PtId == ptID &&
                            x.SinDay == sinDate &&
                            x.RaiinNo == raiinNo).ToList();
            foreach (var eventCd in eventCds)
            {
                var eventAuditTrailLogs = auditTrailLogs.Where(a => a.EventCd == eventCd).ToList();
                var maxDate = eventAuditTrailLogs.Count == 0 ? DateTime.MinValue : eventAuditTrailLogs.Max(x => x.LogDate);
                result.Add(eventCd, maxDate);
            }
            return result;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        public long GetMaxRpNo(int hpId, long ptId, long raiinNo, int sinDate)
        {
            var odrListQuery = NoTrackingDataContext.OdrInfs
                .Where(odr => odr.HpId == hpId && odr.PtId == ptId && odr.RaiinNo == raiinNo && odr.SinDate == sinDate).ToList();
            if (odrListQuery.Any())
            {
                return odrListQuery.Max(odr => odr.RpNo);
            }
            return 0;
        }

        public List<AuditTrailLogModel> GetKensaAuditTrailLogs(int hpId, string eventCd, long ptID, int sinDate, long raiinNo)
        {
            var trailLogs = NoTrackingDataContext.AuditTrailLogs.Where(x =>
                                   x.HpId == hpId &&
                                   x.EventCd == eventCd &&
                                   x.PtId == ptID &&
                                   x.SinDay == sinDate &&
                                   x.RaiinNo == raiinNo);
            var detailLogs = NoTrackingDataContext.AuditTrailLogDetails;
            var query = from trailLog in trailLogs
                        join detailLog in detailLogs on
                        trailLog.LogId equals detailLog.LogId
                        select new
                        {
                            TrailLog = trailLog,
                            Hosuke = detailLog.Hosoku
                        };
            return query.AsEnumerable().Select(x => new AuditTrailLogModel(x.TrailLog.LogId, x.TrailLog.LogDate, x.TrailLog.HpId, x.TrailLog.UserId, x.TrailLog.EventCd ?? string.Empty, x.TrailLog.PtId, x.TrailLog.SinDay, x.TrailLog.RaiinNo, x.TrailLog.Machine ?? string.Empty, x.Hosuke)).ToList();
        }

        //Item1: InoutKbn
        //Item2: OdrKouiKbn
        //Item3: IsDeleted
        //Item4: Details (Item1: Itemcd, Item2: MasterSbt) 
        public List<KensaPrinterItemModel> GetContainerMstModels(int hpId, int sinDate, List<Tuple<int, int, int, List<Tuple<string, string>>>> orderInfs, bool defaultChecked)
        {
            var kensaLabelCheckInHospital = _systemConf.GetSettingValue(92009, 1, hpId);
            bool checkInHospital(string itemCd)
            {
                var orderKensaLabels = orderInfs.Where(x => x.Item3 == 0 && x.Item4.Any(detail => !string.IsNullOrEmpty(itemCd) &&
                                                                                                                   (detail.Item1.StartsWith("J") ||
                                                                                                                   detail.Item1.StartsWith("Z") ||
                                                                                                                   detail.Item2 == "S"))).ToList();
                return (kensaLabelCheckInHospital == 0 && !(orderKensaLabels.Where(o => o.Item3 == 0 && (o.Item1 == 0 && o.Item2 >= 60 && o.Item2 <= 69) && o.Item4.Any(detail => detail.Item1 == itemCd)).Count() == 0)) ||
                       (kensaLabelCheckInHospital == 1 && !(orderKensaLabels.Where(o => o.Item3 == 0 && (o.Item1 == 1 && o.Item2 >= 60 && o.Item2 <= 69) && o.Item4.Any(detail => detail.Item1 == itemCd)).Count() == 0)) ||
                       (!(orderKensaLabels.Where(o => o.Item3 == 0 && !(o.Item2 >= 60 && o.Item2 <= 69) && o.Item4.Any(detail => detail.Item1 == itemCd)).Count() == 0)) ||
                       kensaLabelCheckInHospital == 2;
            }
            List<string> itemcds = new List<string>();
            List<KensaPrinterItemModel> kensaItems = new List<KensaPrinterItemModel>();
            foreach (var order in orderInfs)
            {
                if (order.Item3 != 0) continue;
                var kensaItemCds = order.Item4.Where(x => !string.IsNullOrEmpty(x.Item1) &&
                                                                       (x.Item1.StartsWith("J") || x.Item1.StartsWith("Z") || x.Item2 == "S"));

                itemcds.AddRange(kensaItemCds.Select(x => x.Item1));
            }
            var tenmsts = NoTrackingDataContext.TenMsts.Where(x => x.HpId == hpId &&
                                                                                   x.StartDate <= sinDate &&
                                                                                   x.EndDate >= sinDate &&
                                                                                   x.KensaLabel > 0 &&
                                                                                   x.IsDeleted == DeleteTypes.None);
            var kensaMsts = NoTrackingDataContext.KensaMsts.Where(x => x.HpId == hpId && x.IsDelete == 0);
            var containerMsts = NoTrackingDataContext.ContainerMsts.Where(x => x.HpId == hpId);
            List<KensaPrinterItemModel> allItems = new List<KensaPrinterItemModel>();



            foreach (var itemcd in itemcds)
            {

                var query = (from tenmst in tenmsts
                             where tenmst.ItemCd == itemcd
                             join kensaMst in kensaMsts on
                             new { tenmst.KensaItemCd, tenmst.KensaItemSeqNo } equals
                             new { kensaMst.KensaItemCd, kensaMst.KensaItemSeqNo } into tenMstKensas
                             from tenMstKensa in tenMstKensas.DefaultIfEmpty()
                             join containerMst in containerMsts on
                             tenMstKensa.ContainerCd equals containerMst.ContainerCd into tenMstKensaContainers
                             from tenMstKensaContainer in tenMstKensaContainers.DefaultIfEmpty()
                             select new
                             {
                                 ItemCd = tenmst.ItemCd,
                                 Name = tenMstKensaContainer == null ? tenmst.Name ?? string.Empty : tenMstKensaContainer.ContainerName,
                                 ContainerName = tenMstKensaContainer == null ? "" : tenMstKensaContainer.ContainerName,
                                 ContainerCd = tenMstKensaContainer == null ? 0 : tenMstKensaContainer.ContainerCd,
                                 KensaLabel = tenmst.KensaLabel
                             }).ToList();
                allItems.AddRange(query.Select(x => new KensaPrinterItemModel(x.ItemCd, x.Name, x.ContainerName, x.KensaLabel, x.ContainerCd, defaultChecked)));
            }
            allItems = allItems.Where(mst => checkInHospital(mst.ItemCd)).ToList();
            var groupKensaExistContainerCds = allItems.Where(x => x.ContainerCd != 0).GroupBy(x => new { x.ContainerCd });
            foreach (var group in groupKensaExistContainerCds)
            {
                int maxKensaLabel = group.ToList().Max(x => x.KensaLabel);
                var item = group.FirstOrDefault();
                item?.ChangeKensaLabel(maxKensaLabel);
                if (item != null)
                {
                    kensaItems.Add(item);
                }
            }

            var groupKensaNotExistContainercds = allItems.Where(x => x.ContainerCd == 0).GroupBy(x => x.ItemCd);
            foreach (var group in groupKensaNotExistContainercds)
            {
                var item = group.FirstOrDefault();
                if (item != null)
                {
                    kensaItems.Add(item);
                }
            }
            for (int i = 0; i < kensaItems.Count; i++)
            {
                if (i == 0)
                {
                    kensaItems[i].ChangeTextBoxBorderThickness(new Thickness(1, 0.5, 0, 0.5));
                    kensaItems[i].ChangeComboboxBorderThickness(new Thickness(1, 0, 0, 0));
                }
                else if (kensaItems.Count == i + 1)
                {
                    kensaItems[i].ChangeTextBoxBorderThickness(new Thickness(1, 1, 0, 0));
                    kensaItems[i].ChangeComboboxBorderThickness(new Thickness(1, 1, 0, 0));
                }
                else
                {
                    kensaItems[i].ChangeTextBoxBorderThickness(new Thickness(1, 1, 0, 0.5));
                    kensaItems[i].ChangeTextBoxBorderThickness(new Thickness(1, 1, 0, 0));
                }
                var odrInf = orderInfs.FirstOrDefault(o => o.Item4.Any(d => d.Item1 == kensaItems[i].ItemCd));
                if (odrInf == null) continue;
                kensaItems[i].ChangeInoutKbnOdrKouiKbn(odrInf.Item1, odrInf.Item2);
            }
            return kensaItems;
        }
    }
}
