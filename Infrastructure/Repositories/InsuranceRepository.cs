﻿using Domain.Constant;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Domain.Models.InsuranceMst;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Mapping;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class InsuranceRepository : RepositoryBase, IInsuranceRepository
    {
        public InsuranceRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public InsuranceDataModel GetInsuranceListById(int hpId, long ptId, int sinDate)
        {
            int prefCd = 0;
            var hpInf = NoTrackingDataContext.HpInfs.Where(x => x.HpId == hpId).OrderByDescending(p => p.StartDate).FirstOrDefault();
            if (hpInf != null)
            {
                prefCd = hpInf.PrefNo;
            }

            #region PtHokenInf
            IQueryable<PtHokenInf> hokenInfQuery = NoTrackingDataContext.PtHokenInfs.Where(h => h.HpId == hpId && h.PtId == ptId && h.IsDeleted == DeleteTypes.None).OrderByDescending(x => x.HokenId);

            var hokenMasterInfQuery = NoTrackingDataContext.HokenMsts.Where(h => h.HpId == hpId && h.StartDate <= sinDate && sinDate <= h.EndDate &&
                                                                            (h.PrefNo == prefCd || h.PrefNo == 0 || h.IsOtherPrefValid == 1))
                                     .GroupBy(x => new
                                     {
                                         x.HpId,
                                         x.HokenNo,
                                         x.HokenEdaNo,
                                     }).Select(grp => new
                                     {
                                         HpId = grp.Key.HpId,
                                         HokenNo = (int?)grp.Key.HokenNo ?? 0,
                                         HokenEdaNo = grp.Key.HokenEdaNo,
                                         StartDate = grp.Max(x => x.StartDate)
                                     });

            var hokenMasterInfQueryDF = NoTrackingDataContext.HokenMsts.Where(h => h.HpId == hpId &&
                                                                            (h.PrefNo == prefCd || h.PrefNo == 0 || h.IsOtherPrefValid == 1))
                                        .GroupBy(x => new
                                        {
                                            x.HpId,
                                            x.HokenNo,
                                            x.HokenEdaNo
                                        }).Select(grp => new
                                        {
                                            HpId = grp.Key.HpId,
                                            HokenNo = grp.Key.HokenNo,
                                            HokenEdaNo = grp.Key.HokenEdaNo,
                                            StartDate = grp.Max(x => x.StartDate)
                                        });

            var hokenMasterInfQueryTarget = from hokenDf in hokenMasterInfQueryDF
                                            join hokenPrioritize in hokenMasterInfQuery
                                            on new { hokenDf.HokenEdaNo, hokenDf.HokenNo, hokenDf.HpId } equals new { hokenPrioritize.HokenEdaNo, hokenPrioritize.HokenNo, hokenPrioritize.HpId } into obj
                                            from hoken in obj.DefaultIfEmpty()
                                            select new
                                            {
                                                HpId = hokenDf.HpId,
                                                HokenNo = hokenDf.HokenNo,
                                                HokenEdaNo = hokenDf.HokenEdaNo,
                                                StartDate = (hoken.HokenNo == 0 && hoken.HokenEdaNo == 0) ? hokenDf.StartDate : (int?)hoken.StartDate ?? 0
                                            };

            IQueryable<HokenMst> hokenMasterFinal = from hoken in hokenMasterInfQueryTarget
                                                    select NoTrackingDataContext.HokenMsts.FirstOrDefault(h => h.HpId == hpId && h.HokenNo == hoken.HokenNo && h.HokenEdaNo == hoken.HokenEdaNo && h.StartDate == hoken.StartDate && (h.PrefNo == prefCd || h.PrefNo == 0 || h.IsOtherPrefValid == 1));

            var queryHokenInf = (from inf in hokenInfQuery
                                 join hkMaster in hokenMasterFinal on new { inf.HokenNo, inf.HokenEdaNo } equals new { hkMaster.HokenNo, hkMaster.HokenEdaNo } into hkMtObject
                                 from hkObj in hkMtObject.DefaultIfEmpty()
                                 join roudou in NoTrackingDataContext.RoudouMsts on hkObj.PrefNo.ToString() equals roudou.RoudouCd into rouObject
                                 from rou in rouObject.DefaultIfEmpty()
                                 select new
                                 {
                                     HokenInf = inf,
                                     HokenMaster = hkObj,
                                     HokenCheckList = (from hkC in NoTrackingDataContext.PtHokenChecks.Where(x => x.HpId == hpId && x.PtID == ptId && x.IsDeleted == DeleteStatus.None && x.HokenGrp == HokenGroupConstant.HokenGroupHokenPattern
                                                                            && x.HokenId == inf.HokenId).OrderByDescending(o => o.CheckDate)
                                                       join userMst in NoTrackingDataContext.UserMsts
                                                       on hkC.CheckId equals userMst.UserId
                                                       select new
                                                       {
                                                           HokenCheck = hkC,
                                                           UserInfo = userMst
                                                       }
                                                ).ToList(),
                                     RousaiTenkis = NoTrackingDataContext.PtRousaiTenkis.Where(x => x.HpId == hpId && x.PtId == ptId && x.HokenId == inf.HokenId && x.IsDeleted == DeleteStatus.None).OrderBy(x => x.EndDate).ToList(),
                                     Roudou = rou
                                 }).ToList();

            List<HokenInfModel> hokenInfList = queryHokenInf.Select(item =>
            {
                string houbetuNo = string.Empty;
                string hokensyaNoSearch = string.Empty;
                CIUtil.GetHokensyaHoubetu(item.HokenInf.HokensyaNo ?? string.Empty, ref hokensyaNoSearch, ref houbetuNo);
                HokensyaMst? hokensyaMst = NoTrackingDataContext.HokensyaMsts.FirstOrDefault(x => x.HpId == hpId && x.HokensyaNo == hokensyaNoSearch && x.Houbetu == houbetuNo);
                bool isReceKisaiOrNoHoken = false;
                if (item.HokenMaster != null)
                    isReceKisaiOrNoHoken = IsReceKisai(item.HokenMaster) || IsNoHoken(item.HokenMaster, item.HokenInf.HokenKbn, item.HokenInf.Houbetu ?? string.Empty);

                return new HokenInfModel(hpId,
                                        ptId,
                                        item.HokenInf.HokenId,
                                        item.HokenInf.SeqNo,
                                        item.HokenInf.HokenNo,
                                        item.HokenInf.HokenEdaNo,
                                        item.HokenInf.HokenKbn,
                                        item.HokenInf.HokensyaNo ?? string.Empty,
                                        item.HokenInf.Kigo ?? string.Empty,
                                        item.HokenInf.Bango ?? string.Empty,
                                        item.HokenInf.EdaNo ?? string.Empty,
                                        item.HokenInf.HonkeKbn,
                                        item.HokenInf.StartDate,
                                        item.HokenInf.EndDate,
                                        item.HokenInf.SikakuDate,
                                        item.HokenInf.KofuDate,
                                        GetConfirmDate(item.HokenCheckList.FirstOrDefault()?.HokenCheck),
                                        item.HokenInf.KogakuKbn,
                                        item.HokenInf.TasukaiYm,
                                        item.HokenInf.TokureiYm1,
                                        item.HokenInf.TokureiYm2,
                                        item.HokenInf.GenmenKbn,
                                        item.HokenInf.GenmenRate,
                                        item.HokenInf.GenmenGaku,
                                        item.HokenInf.SyokumuKbn,
                                        item.HokenInf.KeizokuKbn,
                                        item.HokenInf.Tokki1 ?? string.Empty,
                                        item.HokenInf.Tokki2 ?? string.Empty,
                                        item.HokenInf.Tokki3 ?? string.Empty,
                                        item.HokenInf.Tokki4 ?? string.Empty,
                                        item.HokenInf.Tokki5 ?? string.Empty,
                                        item.HokenInf.RousaiKofuNo ?? string.Empty,
                                        item.HokenInf.RousaiRoudouCd ?? string.Empty,
                                        item.HokenInf.RousaiSaigaiKbn,
                                        item.HokenInf.RousaiKantokuCd ?? string.Empty,
                                        item.HokenInf.RousaiSyobyoDate,
                                        item.HokenInf.RyoyoStartDate,
                                        item.HokenInf.RyoyoEndDate,
                                        item.HokenInf.RousaiSyobyoCd ?? string.Empty,
                                        item.HokenInf.RousaiJigyosyoName ?? string.Empty,
                                        item.HokenInf.RousaiPrefName ?? string.Empty,
                                        item.HokenInf.RousaiCityName ?? string.Empty,
                                        item.HokenInf.RousaiReceCount,
                                        string.Empty,
                                        string.Empty,
                                        string.Empty,
                                        sinDate,
                                        item.HokenInf.JibaiHokenName ?? string.Empty,
                                        item.HokenInf.JibaiHokenTanto ?? string.Empty,
                                        item.HokenInf.JibaiHokenTel ?? string.Empty,
                                        item.HokenInf.JibaiJyusyouDate,
                                        item.HokenInf.Houbetu ?? string.Empty,
                                        item.HokenCheckList.Select(c => new ConfirmDateModel(c.HokenCheck.HokenGrp, c.HokenCheck.HokenId, c.HokenCheck.SeqNo, c.HokenCheck.CheckId, c.UserInfo.Name ?? string.Empty, c.HokenCheck.CheckCmt ?? string.Empty, c.HokenCheck.CheckDate)).ToList(),
                                        item.RousaiTenkis.Select(x => new RousaiTenkiModel(x.Sinkei, x.Tenki, x.EndDate, x.IsDeleted, x.SeqNo)).ToList(),
                                        isReceKisaiOrNoHoken,
                                        item.HokenInf.IsDeleted,
                                        ConvertHokenMstModel(item.HokenMaster, item.Roudou?.RoudouName ?? string.Empty),
                                        hokensyaMst == null ? new HokensyaMstModel() : new HokensyaMstModel(
                                                                                        hokensyaMst.HpId,
                                                                                        hokensyaMst.Name ?? string.Empty,
                                                                                        hokensyaMst.KanaName ?? string.Empty,
                                                                                        hokensyaMst.HoubetuKbn ?? string.Empty,
                                                                                        hokensyaMst.Houbetu ?? string.Empty,
                                                                                        hokensyaMst.HokenKbn,
                                                                                        hokensyaMst.PrefNo,
                                                                                        hokensyaMst.HokensyaNo ?? string.Empty,
                                                                                        hokensyaMst.Kigo ?? string.Empty,
                                                                                        hokensyaMst.Bango ?? string.Empty,
                                                                                        hokensyaMst.RateHonnin,
                                                                                        hokensyaMst.RateKazoku,
                                                                                        hokensyaMst.PostCode ?? string.Empty,
                                                                                        hokensyaMst.Address1 ?? string.Empty,
                                                                                        hokensyaMst.Address2 ?? string.Empty,
                                                                                        hokensyaMst.Tel1 ?? string.Empty,
                                                                                        hokensyaMst.IsKigoNa),
                                        false,
                                        false
                                        );
            }).ToList();

            #endregion PtHokenInf

            #region PtHokenKohi
            IQueryable<PtKohi> kohiQuery = NoTrackingDataContext.PtKohis.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == DeleteTypes.None).OrderByDescending(entity => entity.HokenId);

            var hokenMasterKohiQuery = NoTrackingDataContext.HokenMsts.Where(h => h.HpId == hpId && h.StartDate <= sinDate && sinDate <= h.EndDate &&
                                                                            (h.PrefNo == prefCd || h.PrefNo == 0 || h.IsOtherPrefValid == 1))
                                       .GroupBy(x => new
                                       {
                                           x.HpId,
                                           x.HokenNo,
                                           x.HokenEdaNo,
                                           x.PrefNo
                                       }).Select(grp => new
                                       {
                                           HpId = grp.Key.HpId,
                                           HokenNo = (int?)grp.Key.HokenNo ?? 0,
                                           HokenEdaNo = grp.Key.HokenEdaNo,
                                           PrefNo = grp.Key.PrefNo,
                                           StartDate = grp.Max(x => x.StartDate)
                                       });

            var hokenMasterKohiQueryDF = NoTrackingDataContext.HokenMsts.Where(h => h.HpId == hpId &&
                                                                            (h.PrefNo == prefCd || h.PrefNo == 0 || h.IsOtherPrefValid == 1))
                                        .GroupBy(x => new
                                        {
                                            x.HpId,
                                            x.HokenNo,
                                            x.HokenEdaNo,
                                            x.PrefNo
                                        }).Select(grp => new
                                        {
                                            HpId = grp.Key.HpId,
                                            HokenNo = grp.Key.HokenNo,
                                            HokenEdaNo = grp.Key.HokenEdaNo,
                                            PrefNo = grp.Key.PrefNo,
                                            StartDate = grp.Max(x => x.StartDate)
                                        });


            var hokenMasterKohiQueryTarget = from hokenDf in hokenMasterKohiQueryDF
                                             join hokenPrioritize in hokenMasterKohiQuery
                                             on new { hokenDf.HokenEdaNo, hokenDf.HokenNo, hokenDf.HpId, hokenDf.PrefNo } equals new { hokenPrioritize.HokenEdaNo, hokenPrioritize.HokenNo, hokenPrioritize.HpId, hokenPrioritize.PrefNo } into obj
                                             from hoken in obj.DefaultIfEmpty()
                                             select new
                                             {
                                                 HpId = hokenDf.HpId,
                                                 HokenNo = hokenDf.HokenNo,
                                                 HokenEdaNo = hokenDf.HokenEdaNo,
                                                 PrefNo = hokenDf.PrefNo,
                                                 StartDate = (hoken.HokenNo == 0 && hoken.HokenEdaNo == 0) ? hokenDf.StartDate : (int?)hoken.StartDate ?? 0
                                             };

            IQueryable<HokenMst> hokenMasterKohiFinal = from hoken in hokenMasterKohiQueryTarget
                                                        select NoTrackingDataContext.HokenMsts.FirstOrDefault(h => h.HpId == hpId && h.HokenNo == hoken.HokenNo && h.HokenEdaNo == hoken.HokenEdaNo && h.StartDate == hoken.StartDate && h.PrefNo == hoken.PrefNo);

            var queryKohi = (from kohi in kohiQuery
                             join hkMaster in hokenMasterKohiFinal on new { kohi.HokenNo, kohi.HokenEdaNo, kohi.PrefNo } equals new { hkMaster.HokenNo, hkMaster.HokenEdaNo, hkMaster.PrefNo } into hkMtObject
                             from hkObj in hkMtObject.DefaultIfEmpty()
                             join roudou in NoTrackingDataContext.RoudouMsts on hkObj.PrefNo.ToString() equals roudou.RoudouCd into rouObject
                             from rou in rouObject.DefaultIfEmpty()
                             select new
                             {
                                 Kohi = kohi,
                                 HokenMaster = hkObj,
                                 HokenCheckList = (from hkC in NoTrackingDataContext.PtHokenChecks.Where(x => x.HpId == hpId && x.PtID == ptId && x.IsDeleted == DeleteStatus.None && x.HokenGrp == HokenGroupConstant.HokenGroupKohi
                                                                              && x.HokenId == kohi.HokenId).OrderByDescending(o => o.CheckDate)
                                                   join userMst in NoTrackingDataContext.UserMsts
                                                   on hkC.CheckId equals userMst.UserId
                                                   select new
                                                   {
                                                       HokenCheck = hkC,
                                                       UserInfo = userMst
                                                   }
                                                 ).ToList(),
                                 Roudou = rou
                             }).ToList();

            List<KohiInfModel> kohiInfList = queryKohi.Select(obj => new KohiInfModel(obj.Kohi.FutansyaNo ?? string.Empty,
                                                          obj.Kohi.JyukyusyaNo ?? string.Empty,
                                                          obj.Kohi.HokenId,
                                                          obj.Kohi.StartDate,
                                                          obj.Kohi.EndDate,
                                                          GetConfirmDate(obj.HokenCheckList.FirstOrDefault()?.HokenCheck),
                                                          obj.Kohi.Rate,
                                                          obj.Kohi.GendoGaku,
                                                          obj.Kohi.SikakuDate,
                                                          obj.Kohi.KofuDate,
                                                          obj.Kohi.TokusyuNo ?? string.Empty,
                                                          obj.Kohi.HokenSbtKbn,
                                                          obj.Kohi.Houbetu ?? string.Empty,
                                                          obj.Kohi.HokenNo,
                                                          obj.Kohi.HokenEdaNo,
                                                          obj.Kohi.PrefNo,
                                                          ConvertHokenMstModel(obj.HokenMaster, obj.Roudou?.RoudouName ?? string.Empty),
                                                          sinDate,
                                                          obj.HokenCheckList.Select(c => new ConfirmDateModel(c.HokenCheck.HokenGrp, c.HokenCheck.HokenId, c.HokenCheck.SeqNo, c.HokenCheck.CheckId, c.UserInfo.Name ?? string.Empty, c.HokenCheck.CheckCmt ?? string.Empty, c.HokenCheck.CheckDate)).ToList(),
                                                          false,
                                                          obj.Kohi.IsDeleted,
                                                          false,
                                                          obj.Kohi.SeqNo)).ToList();
            #endregion PtHokenKohi

            #region PtHokenPattern
            var dataHokenPatterList = NoTrackingDataContext.PtHokenPatterns.Where(x => x.PtId == ptId && x.HpId == hpId && x.IsDeleted == DeleteTypes.None).OrderByDescending(x => x.HokenPid);
            var dataKohi = NoTrackingDataContext.PtKohis.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == DeleteStatus.None);
            var dataHokenInf = NoTrackingDataContext.PtHokenInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == DeleteTypes.None);
            var dataHokenCheck = NoTrackingDataContext.PtHokenChecks.Where(x => x.HpId == hpId && x.PtID == ptId && x.IsDeleted == DeleteStatus.None);
            var dataPtInf = NoTrackingDataContext.PtInfs.Where(pt => pt.HpId == hpId && pt.PtId == ptId && pt.IsDelete == DeleteStatus.None);

            var joinQuery = from ptHokenPattern in dataHokenPatterList
                            join ptHokenInf in dataHokenInf on
                                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenId } equals
                                new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId } //into ptHokenInfs from ptHokenInf in ptHokenInfs.DefaultIfEmpty()
                            join ptKohi1 in dataKohi on
                                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi1Id } equals
                                new { ptKohi1.HpId, ptKohi1.PtId, Kohi1Id = ptKohi1.HokenId } into datakohi1
                            from ptKohi1 in datakohi1.DefaultIfEmpty()
                            join ptKohi2 in dataKohi on
                                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi2Id } equals
                                new { ptKohi2.HpId, ptKohi2.PtId, Kohi2Id = ptKohi2.HokenId } into datakohi2
                            from ptKohi2 in datakohi2.DefaultIfEmpty()
                            join ptKohi3 in dataKohi on
                                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi3Id } equals
                                new { ptKohi3.HpId, ptKohi3.PtId, Kohi3Id = ptKohi3.HokenId } into datakohi3
                            from ptKohi3 in datakohi3.DefaultIfEmpty()
                            join ptKohi4 in dataKohi on
                                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi4Id } equals
                                new { ptKohi4.HpId, ptKohi4.PtId, Kohi4Id = ptKohi4.HokenId } into datakohi4
                            from ptKohi4 in datakohi4.DefaultIfEmpty()
                            from ptInf in dataPtInf
                            select new
                            {
                                ptHokenPattern.HpId,
                                ptHokenPattern.PtId,
                                ptHokenPattern.HokenId,
                                ptHokenPattern.SeqNo,
                                ptHokenInf.HokenNo,
                                ptHokenInf.HokenEdaNo,
                                ptHokenPattern.HokenSbtCd,
                                ptHokenPattern.HokenPid,
                                ptHokenPattern.HokenKbn,
                                ptHokenInf = ptHokenInf,
                                ptHokenInf.HokensyaNo,
                                ptHokenInf.Kigo,
                                ptHokenInf.Bango,
                                ptHokenInf.EdaNo,
                                ptHokenInf.HonkeKbn,
                                ptHokenPattern.StartDate,
                                ptHokenPattern.EndDate,
                                ptHokenInf.SikakuDate,
                                ptHokenInf.KofuDate,
                                ptKohi1,
                                ptKohi2,
                                ptKohi3,
                                ptKohi4,
                                ptHokenInf.KogakuKbn,
                                ptHokenInf.TasukaiYm,
                                ptHokenInf.TokureiYm1,
                                ptHokenInf.TokureiYm2,
                                ptHokenInf.GenmenKbn,
                                ptHokenInf.GenmenRate,
                                ptHokenInf.GenmenGaku,
                                ptHokenInf.SyokumuKbn,
                                ptHokenInf.KeizokuKbn,
                                ptHokenInf.Tokki1,
                                ptHokenInf.Tokki2,
                                ptHokenInf.Tokki3,
                                ptHokenInf.Tokki4,
                                ptHokenInf.Tokki5,
                                ptHokenInf.RousaiKofuNo,
                                ptHokenInf.RousaiRoudouCd,
                                ptHokenInf.RousaiSaigaiKbn,
                                ptHokenInf.RousaiKantokuCd,
                                ptHokenInf.RousaiSyobyoDate,
                                ptHokenInf.RyoyoStartDate,
                                ptHokenInf.RyoyoEndDate,
                                ptHokenInf.RousaiSyobyoCd,
                                ptHokenInf.RousaiJigyosyoName,
                                ptHokenInf.RousaiPrefName,
                                ptHokenInf.RousaiCityName,
                                ptHokenInf.RousaiReceCount,
                                ptHokenInf.JibaiHokenName,
                                ptHokenInf.JibaiHokenTanto,
                                ptHokenInf.JibaiHokenTel,
                                ptHokenInf.JibaiJyusyouDate,
                                ptInf.Birthday,
                                ptHokenPattern.HokenMemo,
                                HobetuHokenInf = ptHokenInf.Houbetu,
                                HokenInfStartDate = ptHokenInf.StartDate,
                                HokenInfEndDate = ptHokenInf.EndDate,
                                HokenInfIsDeleted = ptHokenInf.IsDeleted,
                                PatternIsDeleted = ptHokenPattern.IsDeleted
                            };


            var itemList = joinQuery.ToList();
            List<InsuranceModel> listInsurance = new List<InsuranceModel>();
            foreach (var item in itemList)
            {
                HokenInfModel hokenInf = hokenInfList.FirstOrDefault(h => h.HokenNo == item.HokenNo && h.HokenEdaNo == item.HokenEdaNo && h.HokenId == item.HokenId) ?? new HokenInfModel();

                KohiInfModel Kohi1 = kohiInfList.FirstOrDefault(h => item.ptKohi1 != null && h.HokenNo == item.ptKohi1.HokenNo && h.HokenEdaNo == item.ptKohi1.HokenEdaNo && item.ptKohi1.HokenId == h.HokenId) ?? new KohiInfModel(0);
                KohiInfModel Kohi2 = kohiInfList.FirstOrDefault(h => item.ptKohi2 != null && h.HokenNo == item.ptKohi2.HokenNo && h.HokenEdaNo == item.ptKohi2.HokenEdaNo && item.ptKohi2.HokenId == h.HokenId) ?? new KohiInfModel(0);
                KohiInfModel Kohi3 = kohiInfList.FirstOrDefault(h => item.ptKohi3 != null && h.HokenNo == item.ptKohi3.HokenNo && h.HokenEdaNo == item.ptKohi3.HokenEdaNo && item.ptKohi3.HokenId == h.HokenId) ?? new KohiInfModel(0);
                KohiInfModel Kohi4 = kohiInfList.FirstOrDefault(h => item.ptKohi4 != null && h.HokenNo == item.ptKohi4.HokenNo && h.HokenEdaNo == item.ptKohi4.HokenEdaNo && item.ptKohi4.HokenId == h.HokenId) ?? new KohiInfModel(0);

                listInsurance.Add(new InsuranceModel(
                    item.HpId,
                    item.PtId,
                    item.Birthday,
                    item.SeqNo,
                    item.HokenSbtCd,
                    item.HokenPid,
                    item.HokenKbn,
                    sinDate,
                    item.HokenMemo,
                    hokenInf,
                    kohi1: Kohi1,
                    kohi2: Kohi2,
                    kohi3: Kohi3,
                    kohi4: Kohi4,
                    item.PatternIsDeleted,
                    item.StartDate,
                    item.EndDate,
                    false
                ));
            }
            #endregion PtHokenPattern

            return new InsuranceDataModel(listInsurance, hokenInfList, kohiInfList);
        }

        public bool CheckExistHokenPIdList(List<int> hokenPIds, List<int> hpIds, List<long> ptIds)
        {
            if (hokenPIds.Count == 0) return true;
            var countPtHokens = NoTrackingDataContext.PtHokenInfs.Count(p => hokenPIds.Contains(p.HokenId) && p.IsDeleted != 1 && hpIds.Contains(p.HpId) && ptIds.Contains(p.PtId));
            return countPtHokens == hokenPIds.Count;
        }

        public bool CheckExistHokenId(int hokenId)
        {
            var check = NoTrackingDataContext.PtHokenInfs.Any(h => h.HokenId == hokenId && h.IsDeleted == 0);
            return check;
        }

        public bool CheckExistHokenPid(int hokenPid)
        {
            var check = NoTrackingDataContext.PtHokenPatterns.Any(h => h.HokenPid == hokenPid && h.IsDeleted == 0);
            return check;
        }

        public List<HokenInfModel> GetCheckListHokenInf(int hpId, long ptId, List<int> hokenPids)
        {
            var result = NoTrackingDataContext.PtHokenPatterns.Where(h => h.HpId == hpId && hokenPids.Contains(h.HokenPid) && h.PtId == ptId && h.IsDeleted == 0);
            return result.Select(r => new HokenInfModel(r.HokenPid, r.PtId, r.HpId, r.StartDate, r.EndDate)).ToList();
        }

        private KohiInfModel GetKohiInfModel(PtKohi? kohiInf, PtHokenCheck? ptHokenCheck, HokenMst? hokenMst, int sinDate, List<ConfirmDateModel> confirmDateList)
        {
            if (kohiInf == null)
            {
                return new KohiInfModel(0);
            }
            return new KohiInfModel(
                kohiInf.FutansyaNo ?? string.Empty,
                kohiInf.JyukyusyaNo ?? string.Empty,
                kohiInf.HokenId,
                kohiInf.StartDate,
                kohiInf.EndDate,
                GetConfirmDate(ptHokenCheck),
                kohiInf.Rate,
                kohiInf.GendoGaku,
                kohiInf.SikakuDate,
                kohiInf.KofuDate,
                kohiInf.TokusyuNo ?? string.Empty,
                kohiInf.HokenSbtKbn,
                kohiInf.Houbetu ?? string.Empty,
                kohiInf.HokenNo,
                kohiInf.HokenEdaNo,
                kohiInf.PrefNo,
                GetHokenMstModel(hokenMst),
                sinDate,
                confirmDateList,
                false,
                kohiInf.IsDeleted,
                false,
                kohiInf.SeqNo
                );
        }

        private HokenMstModel GetHokenMstModel(HokenMst? hokenMst)
        {
            if (hokenMst == null)
            {
                return new HokenMstModel();
            }
            return Mapper.Map(hokenMst, new HokenMstModel(), (src, dest) =>
            {
                return dest;
            });
        }

        private string NenkinBango(string? rousaiKofuNo)
        {
            string nenkinBango = "";
            if (rousaiKofuNo != null && rousaiKofuNo.Length == 9)
            {
                nenkinBango = rousaiKofuNo.Substring(0, 2);
            }
            return nenkinBango;
        }

        private int GetConfirmDate(PtHokenCheck? ptHokenCheck)
        {
            return ptHokenCheck is null ? 0 : DateTimeToInt(ptHokenCheck.CheckDate);
        }

        private static int DateTimeToInt(DateTime dateTime, string format = "yyyyMMdd")
        {
            int result = 0;
            result = Int32.Parse(dateTime.ToString(format));
            return result;
        }

        public IEnumerable<InsuranceModel> GetListHokenPattern(int hpId, long ptId, int sinDate, bool allowDisplayDeleted, bool isAllHoken = true, bool isHoken = true, bool isJihi = true, bool isRosai = true, bool isJibai = true)
        {

            var result = NoTrackingDataContext.PtHokenPatterns.Where
                                (
                                    p => p.HpId == hpId && p.PtId == ptId && (p.IsDeleted == 0 || allowDisplayDeleted) &&
                                        (
                                            isAllHoken ||
                                            isHoken && (p.HokenKbn == 1 || p.HokenKbn == 2) ||
                                            isJihi && p.HokenKbn == 0 ||
                                            isRosai && (p.HokenKbn == 11 || p.HokenKbn == 12 || p.HokenKbn == 13) ||
                                            isJibai && p.HokenKbn == 14));

            return result.Select(r => new InsuranceModel(
                        r.HpId,
                        r.PtId,
                        r.SeqNo,
                        r.HokenSbtCd,
                        r.HokenPid,
                        r.HokenKbn,
                        r.HokenId,
                        r.Kohi1Id,
                        r.Kohi2Id,
                        r.Kohi3Id,
                        r.Kohi4Id,
                        r.StartDate,
                        r.EndDate,
                        sinDate
                        ));
        }

        private bool IsReceKisai(HokenMst HokenMasterModel)
        {

            if (HokenMasterModel != null)
            {
                return HokenMasterModel.ReceKisai == 3;
            }
            return false;

        }

        private bool IsNoHoken(HokenMst HokenMasterModel, int hokenKbn, string houbetu)
        {

            if (HokenMasterModel != null)
            {
                return HokenMasterModel.HokenSbtKbn == 0;
            }
            return hokenKbn == 1 && houbetu == HokenConstant.HOUBETU_NASHI;
        }

        public InsuranceModel GetPtHokenInf(int hpId, int hokenPid, long ptId, int sinDate)
        {
            var dataHokenPatterList = NoTrackingDataContext.PtHokenPatterns.Where(x => x.IsDeleted == DeleteStatus.None && x.PtId == ptId && x.HpId == hpId && x.HokenPid == hokenPid).OrderByDescending(x => x.HokenPid);
            var dataKohi = NoTrackingDataContext.PtKohis.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == DeleteStatus.None);
            var dataHokenInf = NoTrackingDataContext.PtHokenInfs.Where(x => x.HpId == hpId && x.PtId == ptId);
            var dataHokenCheck = NoTrackingDataContext.PtHokenChecks.Where(x => x.HpId == hpId && x.PtID == ptId && x.IsDeleted == DeleteStatus.None);
            var dataPtInf = NoTrackingDataContext.PtInfs.Where(pt => pt.HpId == hpId && pt.PtId == ptId && pt.IsDelete == DeleteStatus.None);
            var joinQuery = from ptHokenPattern in dataHokenPatterList
                            join ptHokenInf in dataHokenInf on
                                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenId } equals
                                new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId } //into ptHokenInfs from ptHokenInf in ptHokenInfs.DefaultIfEmpty()
                            join ptKohi1 in dataKohi on
                                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi1Id } equals
                                new { ptKohi1.HpId, ptKohi1.PtId, Kohi1Id = ptKohi1.HokenId } into datakohi1
                            from ptKohi1 in datakohi1.DefaultIfEmpty()
                            join ptKohi2 in dataKohi on
                                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi2Id } equals
                                new { ptKohi2.HpId, ptKohi2.PtId, Kohi2Id = ptKohi2.HokenId } into datakohi2
                            from ptKohi2 in datakohi2.DefaultIfEmpty()
                            join ptKohi3 in dataKohi on
                                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi3Id } equals
                                new { ptKohi3.HpId, ptKohi3.PtId, Kohi3Id = ptKohi3.HokenId } into datakohi3
                            from ptKohi3 in datakohi3.DefaultIfEmpty()
                            join ptKohi4 in dataKohi on
                                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi4Id } equals
                                new { ptKohi4.HpId, ptKohi4.PtId, Kohi4Id = ptKohi4.HokenId } into datakohi4
                            from ptKohi4 in datakohi4.DefaultIfEmpty()
                            from ptInf in dataPtInf
                            select new
                            {
                                ptHokenPattern.HpId,
                                ptHokenPattern.PtId,
                                ptHokenPattern.HokenId,
                                ptHokenPattern.SeqNo,
                                ptHokenInf.HokenNo,
                                ptHokenInf.HokenEdaNo,
                                ptHokenPattern.HokenSbtCd,
                                ptHokenPattern.HokenPid,
                                ptHokenPattern.HokenKbn,
                                ptHokenInf = ptHokenInf,
                                ptHokenInf.HokensyaNo,
                                ptHokenInf.Kigo,
                                ptHokenInf.Bango,
                                ptHokenInf.EdaNo,
                                ptHokenInf.HonkeKbn,
                                ptHokenInf.HokensyaAddress,
                                ptHokenInf.HokensyaName,
                                ptHokenInf.HokensyaTel,
                                ptHokenPattern.StartDate,
                                ptHokenPattern.EndDate,
                                ptHokenInf.SikakuDate,
                                ptHokenInf.KofuDate,
                                ptKohi1,
                                ptKohi2,
                                ptKohi3,
                                ptKohi4,
                                ptHokenInf.KogakuKbn,
                                ptHokenInf.TasukaiYm,
                                ptHokenInf.TokureiYm1,
                                ptHokenInf.TokureiYm2,
                                ptHokenInf.GenmenKbn,
                                ptHokenInf.GenmenRate,
                                ptHokenInf.GenmenGaku,
                                ptHokenInf.SyokumuKbn,
                                ptHokenInf.KeizokuKbn,
                                ptHokenInf.Tokki1,
                                ptHokenInf.Tokki2,
                                ptHokenInf.Tokki3,
                                ptHokenInf.Tokki4,
                                ptHokenInf.Tokki5,
                                ptHokenInf.RousaiKofuNo,
                                ptHokenInf.RousaiRoudouCd,
                                ptHokenInf.RousaiSaigaiKbn,
                                ptHokenInf.RousaiKantokuCd,
                                ptHokenInf.RousaiSyobyoDate,
                                ptHokenInf.RyoyoStartDate,
                                ptHokenInf.RyoyoEndDate,
                                ptHokenInf.RousaiSyobyoCd,
                                ptHokenInf.RousaiJigyosyoName,
                                ptHokenInf.RousaiPrefName,
                                ptHokenInf.RousaiCityName,
                                ptHokenInf.RousaiReceCount,
                                ptHokenInf.JibaiHokenName,
                                ptHokenInf.JibaiHokenTanto,
                                ptHokenInf.JibaiHokenTel,
                                ptHokenInf.JibaiJyusyouDate,
                                ptInf.Birthday,
                                ptHokenPattern.HokenMemo,
                                HobetuHokenInf = ptHokenInf.Houbetu,
                                HokenInfStartDate = ptHokenInf.StartDate,
                                HokenInfEndDate = ptHokenInf.EndDate
                            };
            var itemList = joinQuery.ToList();
            List<InsuranceModel> listInsurance = new List<InsuranceModel>();

            var confirmDateList =
                (
                    from hokenCheck in NoTrackingDataContext.PtHokenChecks.Where(p => p.PtID == ptId && p.HpId == hpId && p.IsDeleted == 0)
                    join userMst in NoTrackingDataContext.UserMsts.AsQueryable()
                    on hokenCheck.CheckId equals userMst.UserId
                    select new
                    {
                        hokenCheck,
                        userMst
                    }
                ).ToList();

            List<ConfirmDateModel> GetConfirmDateList(int hokenGrp, int hokenId)
            {
                if (confirmDateList == null)
                {
                    return new List<ConfirmDateModel>();
                }

                return confirmDateList
                    .Where(c => c.hokenCheck.HokenGrp == hokenGrp && c.hokenCheck.HokenId == hokenId)
                    .Select(c => new ConfirmDateModel(c.hokenCheck.HokenGrp, c.hokenCheck.HokenId, c.hokenCheck.SeqNo, c.hokenCheck.CheckId, c.userMst.KanaName ?? string.Empty, c.hokenCheck.CheckCmt ?? string.Empty, c.hokenCheck.CheckDate))
                    .ToList();
            }

            List<int> hokenNoList = new List<int>();
            hokenNoList.AddRange(itemList.Select(i => i.ptHokenInf != null ? i.ptHokenInf.HokenNo : 0));
            hokenNoList.AddRange(itemList.Select(i => i.ptKohi1 != null ? i.ptKohi1.HokenNo : 0));
            hokenNoList.AddRange(itemList.Select(i => i.ptKohi2 != null ? i.ptKohi2.HokenNo : 0));
            hokenNoList.AddRange(itemList.Select(i => i.ptKohi3 != null ? i.ptKohi3.HokenNo : 0));
            hokenNoList.AddRange(itemList.Select(i => i.ptKohi4 != null ? i.ptKohi4.HokenNo : 0));
            hokenNoList = hokenNoList.Distinct().ToList();

            List<int> hokenEdaNoList = new List<int>();
            hokenEdaNoList.AddRange(itemList.Select(i => i.ptHokenInf != null ? i.ptHokenInf.HokenEdaNo : 0).ToList());
            hokenEdaNoList.AddRange(itemList.Select(i => i.ptKohi1 != null ? i.ptKohi1.HokenEdaNo : 0));
            hokenEdaNoList.AddRange(itemList.Select(i => i.ptKohi2 != null ? i.ptKohi2.HokenEdaNo : 0));
            hokenEdaNoList.AddRange(itemList.Select(i => i.ptKohi3 != null ? i.ptKohi3.HokenEdaNo : 0));
            hokenEdaNoList.AddRange(itemList.Select(i => i.ptKohi4 != null ? i.ptKohi4.HokenEdaNo : 0));
            hokenEdaNoList = hokenEdaNoList.Distinct().ToList();

            List<HokenMst> hokenMstList = NoTrackingDataContext.HokenMsts.Where(h => h.HpId == hpId && hokenNoList.Contains(h.HokenNo) && hokenEdaNoList.Contains(h.HokenEdaNo)).ToList();

            if (itemList.Count > 0)
            {
                foreach (var item in itemList)
                {
                    string houbetu = string.Empty;
                    bool isReceKisaiOrNoHoken = false;

                    HokenMst? hokenMst = hokenMstList.FirstOrDefault(h => h.HokenNo == item.HokenNo && h.HokenEdaNo == item.HokenEdaNo);
                    if (hokenMst != null)
                    {
                        houbetu = hokenMst.Houbetu ?? string.Empty;
                        isReceKisaiOrNoHoken = IsReceKisai(hokenMst) || IsNoHoken(hokenMst, item.HokenKbn, houbetu ?? string.Empty);
                    }
                    var ptRousaiTenkis = NoTrackingDataContext.PtRousaiTenkis.Where(x => x.HpId == hpId && x.PtId == ptId && x.HokenId == item.HokenId).OrderBy(x => x.EndDate)
                        .Select(x => new RousaiTenkiModel(x.Sinkei, x.Tenki, x.EndDate, x.IsDeleted, x.SeqNo)).ToList();

                    //get FindHokensyaMstByNoNotrack
                    string houbetuNo = string.Empty;
                    string hokensyaNoSearch = string.Empty;
                    CIUtil.GetHokensyaHoubetu(item.HokensyaNo ?? string.Empty, ref hokensyaNoSearch, ref houbetuNo);
                    var hokensyaMst = NoTrackingDataContext.HokensyaMsts.Where(x => x.HpId == hpId && x.HokensyaNo == hokensyaNoSearch && x.Houbetu == houbetuNo).Select(x => new HokensyaMstModel(x.IsKigoNa)).FirstOrDefault();

                    var ptHokenCheckOfHokenPattern = dataHokenCheck
                    .Where(x => x.HokenId == item.HokenId && x.HokenGrp == HokenGroupConstant.HokenGroupHokenPattern)
                    .OrderByDescending(x => x.CheckDate).FirstOrDefault();

                    HokenInfModel hokenInf = new HokenInfModel(
                                            hpId,
                                            ptId,
                                            item.HokenId,
                                            item.SeqNo,
                                            item.HokenNo,
                                            item.HokenEdaNo,
                                            item.HokenKbn,
                                            item.HokensyaNo ?? string.Empty,
                                            item.Kigo ?? string.Empty,
                                            item.Bango ?? string.Empty,
                                            item.EdaNo ?? string.Empty,
                                            item.HonkeKbn,
                                            item.StartDate,
                                            item.EndDate,
                                            item.SikakuDate,
                                            item.KofuDate,
                                            GetConfirmDate(ptHokenCheckOfHokenPattern),
                                            item.KogakuKbn,
                                            item.TasukaiYm,
                                            item.TokureiYm1,
                                            item.TokureiYm2,
                                            item.GenmenKbn,
                                            item.GenmenRate,
                                            item.GenmenGaku,
                                            item.SyokumuKbn,
                                            item.KeizokuKbn,
                                            item.Tokki1 ?? string.Empty,
                                            item.Tokki2 ?? string.Empty,
                                            item.Tokki3 ?? string.Empty,
                                            item.Tokki4 ?? string.Empty,
                                            item.Tokki5 ?? string.Empty,
                                            item.RousaiKofuNo ?? string.Empty,
                                            item.RousaiRoudouCd ?? string.Empty,
                                            item.RousaiSaigaiKbn,
                                            item.RousaiKantokuCd ?? string.Empty,
                                            item.RousaiSyobyoDate,
                                            item.RyoyoStartDate,
                                            item.RyoyoEndDate,
                                            item.RousaiSyobyoCd ?? string.Empty,
                                            item.RousaiJigyosyoName ?? string.Empty,
                                            item.RousaiPrefName ?? string.Empty,
                                            item.RousaiCityName ?? string.Empty,
                                            item.RousaiReceCount,
                                            item.HokensyaName ?? string.Empty,
                                            item.HokensyaAddress ?? string.Empty,
                                            item.HokensyaTel ?? string.Empty,
                                            sinDate,
                                            item.JibaiHokenName ?? string.Empty,
                                            item.JibaiHokenTanto ?? string.Empty,
                                            item.JibaiHokenTel ?? string.Empty,
                                            item.JibaiJyusyouDate,
                                            houbetu ?? string.Empty,
                                            GetConfirmDateList(1, item.HokenId),
                                            ptRousaiTenkis,
                                            isReceKisaiOrNoHoken,
                                            0,
                                            Mapper.Map(hokenMst, new HokenMstModel(), (src, dest) =>
                                            {
                                                return dest;
                                            }),
                                            hokensyaMst ?? new HokensyaMstModel(),
                                            false,
                                            false
                                            );

                    HokenMst? hokenMst1 = null;
                    PtHokenCheck? ptHokenCheckOfKohi1 = null;
                    HokenMst? hokenMst2 = null;
                    PtHokenCheck? ptHokenCheckOfKohi2 = null;
                    HokenMst? hokenMst3 = null;
                    PtHokenCheck? ptHokenCheckOfKohi3 = null;
                    HokenMst? hokenMst4 = null;
                    PtHokenCheck? ptHokenCheckOfKohi4 = null;

                    if (item.ptKohi1 is not null)
                    {
                        hokenMst1 = hokenMstList.FirstOrDefault(h => h.HokenNo == item.ptKohi1.HokenNo && h.HokenEdaNo == item.ptKohi1.HokenEdaNo);
                        ptHokenCheckOfKohi1 = dataHokenCheck
                                                .Where(x => x.HokenId == item.ptKohi1.HokenId && x.HokenGrp == HokenGroupConstant.HokenGroupKohi)
                                                .OrderByDescending(x => x.CheckDate).FirstOrDefault();
                    }


                    if (item.ptKohi2 is not null)
                    {
                        hokenMst2 = hokenMstList.FirstOrDefault(h => h.HokenNo == item.ptKohi2.HokenNo && h.HokenEdaNo == item.ptKohi2.HokenEdaNo);
                        ptHokenCheckOfKohi2 = dataHokenCheck
                                               .Where(x => x.HokenId == item.ptKohi2.HokenId && x.HokenGrp == HokenGroupConstant.HokenGroupKohi)
                                               .OrderByDescending(x => x.CheckDate).FirstOrDefault();
                    }


                    if (item.ptKohi3 is not null)
                    {
                        hokenMst3 = hokenMstList.FirstOrDefault(h => h.HokenNo == item.ptKohi3.HokenNo && h.HokenEdaNo == item.ptKohi3.HokenEdaNo);
                        ptHokenCheckOfKohi3 = dataHokenCheck
                                               .Where(x => x.HokenId == item.ptKohi3.HokenId && x.HokenGrp == HokenGroupConstant.HokenGroupKohi)
                                               .OrderByDescending(x => x.CheckDate).FirstOrDefault();
                    }


                    if (item.ptKohi4 is not null)
                    {
                        hokenMst4 = hokenMstList.FirstOrDefault(h => h.HokenNo == item.ptKohi4.HokenNo && h.HokenEdaNo == item.ptKohi4.HokenEdaNo);
                        ptHokenCheckOfKohi4 = dataHokenCheck
                                               .Where(x => x.HokenId == item.ptKohi4.HokenId && x.HokenGrp == HokenGroupConstant.HokenGroupKohi)
                                               .OrderByDescending(x => x.CheckDate).FirstOrDefault();
                    }

                    InsuranceModel insuranceModel = new InsuranceModel(
                        item.HpId,
                        item.PtId,
                        item.Birthday,
                        item.SeqNo,
                        item.HokenSbtCd,
                        item.HokenPid,
                        item.HokenKbn,
                        sinDate,
                        item.HokenMemo,
                        hokenInf,
                        kohi1: GetKohiInfModel(item.ptKohi1, ptHokenCheckOfKohi1, hokenMst1, sinDate, GetConfirmDateList(2, item.ptKohi1?.HokenId ?? 0)),
                        kohi2: GetKohiInfModel(item.ptKohi2, ptHokenCheckOfKohi2, hokenMst2, sinDate, GetConfirmDateList(2, item.ptKohi2?.HokenId ?? 0)),
                        kohi3: GetKohiInfModel(item.ptKohi3, ptHokenCheckOfKohi3, hokenMst3, sinDate, GetConfirmDateList(2, item.ptKohi3?.HokenId ?? 0)),
                        kohi4: GetKohiInfModel(item.ptKohi4, ptHokenCheckOfKohi4, hokenMst4, sinDate, GetConfirmDateList(2, item.ptKohi4?.HokenId ?? 0)),
                        0,
                        item.StartDate,
                        item.EndDate,
                        false
                    );
                    listInsurance.Add(insuranceModel);
                }
            }
            return listInsurance.FirstOrDefault() ?? new InsuranceModel();
        }

        public int GetDefaultSelectPattern(int hpId, long ptId, int sinDate, int historyPid, int selectedHokenPid)
        {
            bool _isSameKohiHoubetu(InsuranceModel pattern1, InsuranceModel pattern2)
            {
                if (pattern1.HokenSbtCd == pattern2.HokenSbtCd)
                {
                    return pattern1.Kohi1.Houbetu == pattern2.Kohi1.Houbetu
                        && pattern1.Kohi2.Houbetu == pattern2.Kohi2.Houbetu
                        && pattern1.Kohi3.Houbetu == pattern2.Kohi3.Houbetu
                        && pattern1.Kohi4.Houbetu == pattern2.Kohi4.Houbetu;
                }

                return false;
            }
            var hokenPatternModels = GetInsuranceList(hpId, ptId, sinDate).Where(i => i.StartDate <= sinDate && i.EndDate >= sinDate).ToList();
            var historyHokenPattern = hokenPatternModels.FirstOrDefault(p => p.HokenPid == historyPid);
            if (historyHokenPattern == null)
            {
                return selectedHokenPid;
            }

            var syosaisinHokenPattern = hokenPatternModels.FirstOrDefault(p => p.HokenPid == selectedHokenPid);
            if (syosaisinHokenPattern?.HokenSbtCd == 0)
            {
                // Rousai, jibai, jihi => use syosaisin
                return selectedHokenPid;
            }
            else if (syosaisinHokenPattern?.HokenSbtCd >= 500)
            {
                if (historyHokenPattern.StartDate <= sinDate && historyHokenPattern.EndDate >= sinDate)
                {
                    // ① 履歴のPIDが有効な保険パターンの場合は、履歴と同じPID
                    return historyHokenPattern.HokenPid;
                }
                // HokenNashi - 保険なし
                else if (_isSameKohiHoubetu(historyHokenPattern, syosaisinHokenPattern))
                {
                    // ② 初再診と履歴が同じ組合せの法別番号の公費を持つ場合は初再診のPID
                    return syosaisinHokenPattern.HokenPid;
                }
                else
                {
                    var sameKohiPattern = hokenPatternModels
                        .Where(p => p.HokenSbtCd >= 500 && !(historyHokenPattern.StartDate <= sinDate && historyHokenPattern.EndDate >= sinDate) && _isSameKohiHoubetu(historyHokenPattern, p))
                        .OrderBy(p => p.IsExpirated)
                        .ThenBy(p => p.HokenPid)
                        .FirstOrDefault();
                    if (sameKohiPattern != null)
                    {
                        // ③ 主保険なしで有効な保険パターンの中で、履歴と同じ組合せの法別番号の公費を持つPID
                        return sameKohiPattern.HokenPid;
                    }
                    else if (!syosaisinHokenPattern.IsExpirated)
                    {
                        // ④ ③までに該当する保険パターンが存在しない場合、初再診が有効な保険組合せの場合は初再診のPID	
                        return syosaisinHokenPattern.HokenPid;
                    }
                    else
                    {
                        return historyPid;
                    }
                }
            }
            else
            {
                if (historyHokenPattern.StartDate <= sinDate && historyHokenPattern.EndDate >= sinDate)
                {
                    // ① 履歴のPIDが有効な保険パターンの場合は、履歴と同じPID
                    return historyHokenPattern.HokenPid;
                }
                else
                {
                    // Kenpo - 主保険あり
                    var sameHokenPatternBuntenKohi = hokenPatternModels
                                                .Where(p => p.HokenSbtCd < 500 && p.HokenSbtCd > 0
                                                       && !p.IsEmptyHoken
                                                       && !p.IsExpirated
                                                       && p.HokenPid == syosaisinHokenPattern?.HokenPid
                                                       && p.BuntenKohis.Count > 0)
                                                .OrderBy(p => p.IsExpirated)
                                                .ThenBy(p => p.HokenPid)
                                                .FirstOrDefault();
                    if (sameHokenPatternBuntenKohi == null)
                    {
                        // ② 初再診と同じ主保険を持つ有効な保険パターンの中で、分点公費（HOKEN_MST.HOKEN_SBT_KBN=6）を持つ保険パターンがない場合は、初再診の保険PID
                        return syosaisinHokenPattern?.HokenPid ?? 0;
                    }
                    else
                    {
                        var sameHokenPattern = hokenPatternModels
                                                .Where(p => p.HokenSbtCd < 500 && p.HokenSbtCd > 0
                                                       && !p.IsEmptyHoken
                                                       && !p.IsExpirated
                                                       && p.HokenPid == syosaisinHokenPattern?.HokenPid
                                                       && _isSameKohiHoubetu(historyHokenPattern, p))
                                                .OrderBy(p => p.IsExpirated)
                                                .ThenBy(p => p.HokenPid)
                                                .FirstOrDefault();
                        if (sameHokenPattern != null)
                        {
                            // ① 初再診と同じ主保険を持つ保険パターンの中で、履歴と同じ組合せの法別番号の公費を持つPID
                            return sameHokenPattern.HokenPid;
                        }
                        else
                        {
                            // ② 初再診と同じ主保険を持つ保険パターンの中で、履歴の法別番号の一致率が高くて組合せ数が少ないPID
                            var sameHokenPatternDiffHoubetu = hokenPatternModels
                                                    .Where(p => p.HokenSbtCd < 500 && p.HokenSbtCd > 0
                                                           && !p.IsEmptyHoken
                                                           && !p.IsExpirated
                                                           && p.HokenPid == syosaisinHokenPattern?.HokenPid)
                                                    .OrderBy(p => p.IsExpirated)
                                                    .ThenBy(p => p.HokenPid)
                                                    .ToList();
                            if (sameHokenPatternDiffHoubetu.Count > 0)
                            {
                                List<string> historyHoubetuList = new List<string>();
                                if (!historyHokenPattern.IsEmptyKohi1 && !string.IsNullOrEmpty(historyHokenPattern.Kohi1.Houbetu))
                                {
                                    historyHoubetuList.Add(historyHokenPattern.Kohi1.Houbetu);
                                }
                                if (!historyHokenPattern.IsEmptyKohi2 && !string.IsNullOrEmpty(historyHokenPattern.Kohi2.Houbetu))
                                {
                                    historyHoubetuList.Add(historyHokenPattern.Kohi2.Houbetu);
                                }
                                if (!historyHokenPattern.IsEmptyKohi3 && !string.IsNullOrEmpty(historyHokenPattern.Kohi3.Houbetu))
                                {
                                    historyHoubetuList.Add(historyHokenPattern.Kohi3.Houbetu);
                                }
                                if (!historyHokenPattern.IsEmptyKohi4 && !string.IsNullOrEmpty(historyHokenPattern.Kohi4.Houbetu))
                                {
                                    historyHoubetuList.Add(historyHokenPattern.Kohi4.Houbetu);
                                }

                                int maxPoint = 0;
                                InsuranceModel? foundPattern = null;
                                foreach (var hokenPattern in sameHokenPatternDiffHoubetu)
                                {
                                    int houbetuPoint = hokenPattern.HoubetuPoint(historyHoubetuList);
                                    if (houbetuPoint > maxPoint)
                                    {
                                        maxPoint = houbetuPoint;
                                        foundPattern = hokenPattern;
                                    }
                                    else if (houbetuPoint == maxPoint)
                                    {
                                        if (foundPattern != null && hokenPattern.KohiCount < foundPattern.KohiCount)
                                        {
                                            maxPoint = houbetuPoint;
                                            foundPattern = hokenPattern;
                                        }
                                    }
                                }
                                if (foundPattern != null)
                                {
                                    return foundPattern.HokenPid;
                                }
                                //else
                                //{
                                //    return syosaisinHokenPattern?.HokenPid ?? 0;
                                //}
                            }
                            //else
                            //{
                            //    return syosaisinHokenPattern?.HokenPid ?? 0;
                            //}
                        }
                    }
                }
                return historyPid;
            }
        }

        public List<InsuranceModel> GetInsuranceList(int hpId, long ptId, int sinDate, bool isDeleted = false)
        {
            PtInf? ptInf = NoTrackingDataContext.PtInfs.FirstOrDefault(p => p.HpId == hpId && p.PtId == ptId && p.IsDelete == 0);
            if (ptInf == null)
            {
                return new List<InsuranceModel>();
            }
            int birthDay = ptInf.Birthday;

            var dataHokenPatterList = NoTrackingDataContext.PtHokenPatterns.Where(x => x.HpId == hpId && x.PtId == ptId && (x.IsDeleted == DeleteStatus.None || isDeleted)).OrderByDescending(x => x.HokenPid);
            var dataKohi = NoTrackingDataContext.PtKohis.Where(x => x.HpId == hpId && x.PtId == ptId && (x.IsDeleted == DeleteStatus.None || isDeleted));
            var dataHokenInf = NoTrackingDataContext.PtHokenInfs.Where(x => x.HpId == hpId && x.PtId == ptId && (x.IsDeleted == DeleteStatus.None || isDeleted));
            var joinQuery = from ptHokenPattern in dataHokenPatterList
                            join ptHokenInf in dataHokenInf on
                                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenId } equals
                                new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId }
                            join ptKohi1 in dataKohi on
                                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi1Id } equals
                                new { ptKohi1.HpId, ptKohi1.PtId, Kohi1Id = ptKohi1.HokenId } into datakohi1
                            from ptKohi1 in datakohi1.DefaultIfEmpty()
                            join ptKohi2 in dataKohi on
                                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi2Id } equals
                                new { ptKohi2.HpId, ptKohi2.PtId, Kohi2Id = ptKohi2.HokenId } into datakohi2
                            from ptKohi2 in datakohi2.DefaultIfEmpty()
                            join ptKohi3 in dataKohi on
                                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi3Id } equals
                                new { ptKohi3.HpId, ptKohi3.PtId, Kohi3Id = ptKohi3.HokenId } into datakohi3
                            from ptKohi3 in datakohi3.DefaultIfEmpty()
                            join ptKohi4 in dataKohi on
                                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi4Id } equals
                                new { ptKohi4.HpId, ptKohi4.PtId, Kohi4Id = ptKohi4.HokenId } into datakohi4
                            from ptKohi4 in datakohi4.DefaultIfEmpty()
                            select new
                            {
                                ptHokenPattern.HpId,
                                ptHokenPattern.PtId,
                                ptHokenPattern.HokenId,
                                ptHokenPattern.SeqNo,
                                ptHokenInf.HokenNo,
                                ptHokenInf.HokenEdaNo,
                                ptHokenPattern.HokenSbtCd,
                                ptHokenPattern.HokenPid,
                                ptHokenPattern.HokenKbn,
                                ptHokenInf = ptHokenInf,
                                ptHokenInf.HokensyaNo,
                                ptHokenInf.Kigo,
                                ptHokenInf.Bango,
                                ptHokenInf.EdaNo,
                                ptHokenInf.HonkeKbn,
                                ptHokenPattern.StartDate,
                                ptHokenPattern.EndDate,
                                ptHokenInf.SikakuDate,
                                ptHokenInf.KofuDate,
                                ptKohi1,
                                ptKohi2,
                                ptKohi3,
                                ptKohi4,
                                ptHokenInf.KogakuKbn,
                                ptHokenInf.TasukaiYm,
                                ptHokenInf.TokureiYm1,
                                ptHokenInf.TokureiYm2,
                                ptHokenInf.GenmenKbn,
                                ptHokenInf.GenmenRate,
                                ptHokenInf.GenmenGaku,
                                ptHokenInf.SyokumuKbn,
                                ptHokenInf.KeizokuKbn,
                                ptHokenInf.Tokki1,
                                ptHokenInf.Tokki2,
                                ptHokenInf.Tokki3,
                                ptHokenInf.Tokki4,
                                ptHokenInf.Tokki5,
                                ptHokenInf.RousaiKofuNo,
                                ptHokenInf.RousaiRoudouCd,
                                ptHokenInf.RousaiSaigaiKbn,
                                ptHokenInf.RousaiKantokuCd,
                                ptHokenInf.RousaiSyobyoDate,
                                ptHokenInf.RyoyoStartDate,
                                ptHokenInf.RyoyoEndDate,
                                ptHokenInf.RousaiSyobyoCd,
                                ptHokenInf.RousaiJigyosyoName,
                                ptHokenInf.RousaiPrefName,
                                ptHokenInf.RousaiCityName,
                                ptHokenInf.RousaiReceCount,
                                ptHokenInf.JibaiHokenName,
                                ptHokenInf.JibaiHokenTanto,
                                ptHokenInf.JibaiHokenTel,
                                ptHokenInf.JibaiJyusyouDate,
                                ptHokenPattern.HokenMemo,
                                HobetuHokenInf = ptHokenInf.Houbetu,
                                HokenInfStartDate = ptHokenInf.StartDate,
                                HokenInfEndDate = ptHokenInf.EndDate,
                                HokenInfIsDeleted = ptHokenInf.IsDeleted,
                                PatternIsDeleted = ptHokenPattern.IsDeleted
                            };

            var itemList = joinQuery.ToList();

            List<int> hokenNoList = new List<int>();
            hokenNoList.AddRange(itemList.Select(i => i.ptHokenInf != null ? i.ptHokenInf.HokenNo : 0).ToList());
            hokenNoList.AddRange(itemList.Select(i => i.ptKohi1 != null ? i.ptKohi1.HokenNo : 0).ToList());
            hokenNoList.AddRange(itemList.Select(i => i.ptKohi2 != null ? i.ptKohi2.HokenNo : 0).ToList());
            hokenNoList.AddRange(itemList.Select(i => i.ptKohi3 != null ? i.ptKohi3.HokenNo : 0).ToList());
            hokenNoList.AddRange(itemList.Select(i => i.ptKohi4 != null ? i.ptKohi4.HokenNo : 0).ToList());
            hokenNoList = hokenNoList.Distinct().ToList();

            List<int> hokenEdaNoList = new List<int>();
            hokenEdaNoList.AddRange(itemList.Select(i => i.ptHokenInf != null ? i.ptHokenInf.HokenEdaNo : 0).ToList());
            hokenEdaNoList.AddRange(itemList.Select(i => i.ptKohi1 != null ? i.ptKohi1.HokenEdaNo : 0).ToList());
            hokenEdaNoList.AddRange(itemList.Select(i => i.ptKohi2 != null ? i.ptKohi2.HokenEdaNo : 0).ToList());
            hokenEdaNoList.AddRange(itemList.Select(i => i.ptKohi3 != null ? i.ptKohi3.HokenEdaNo : 0).ToList());
            hokenEdaNoList.AddRange(itemList.Select(i => i.ptKohi4 != null ? i.ptKohi4.HokenEdaNo : 0).ToList());
            hokenEdaNoList = hokenEdaNoList.Distinct().ToList();

            List<HokenMst> hokenMstList = NoTrackingDataContext.HokenMsts.Where(h => h.HpId == hpId && hokenNoList.Contains(h.HokenNo) && hokenEdaNoList.Contains(h.HokenEdaNo)).ToList();
            List<PtHokenCheck> ptHokenCheckList = NoTrackingDataContext.PtHokenChecks.Where(x => x.HpId == hpId && x.PtID == ptId && x.IsDeleted == DeleteStatus.None).ToList();

            List<InsuranceModel> listInsurance = new List<InsuranceModel>();

            var confirmDateList =
                (
                    from hokenCheck in NoTrackingDataContext.PtHokenChecks.Where(p => p.PtID == ptId && p.HpId == hpId && p.IsDeleted == 0)
                    join userMst in NoTrackingDataContext.UserMsts.AsQueryable()
                    on hokenCheck.CheckId equals userMst.UserId
                    select new
                    {
                        hokenCheck,
                        userMst
                    }
                ).ToList();

            List<ConfirmDateModel> GetConfirmDateList(int hokenGrp, int hokenId)
            {
                if (confirmDateList == null)
                {
                    return new List<ConfirmDateModel>();
                }

                return confirmDateList
                    .Where(c => c.hokenCheck.HokenGrp == hokenGrp && c.hokenCheck.HokenId == hokenId)
                    .Select(c => new ConfirmDateModel(c.hokenCheck.HokenGrp, c.hokenCheck.HokenId, c.hokenCheck.SeqNo, c.hokenCheck.CheckId, c.userMst.KanaName ?? string.Empty, c.hokenCheck.CheckCmt ?? string.Empty, c.hokenCheck.CheckDate))
                    .ToList();
            }

            PtHokenCheck? GetLastPtHokenCheck(int id, int hokenGrp)
            {
                return ptHokenCheckList
                    .Where(h => h.HokenId == id && h.HokenGrp == hokenGrp)
                    .OrderByDescending(x => x.CheckDate)
                    .FirstOrDefault();
            }

            KohiInfModel GenerateKohiModel(PtKohi? ptKohi)
            {
                if (ptKohi == null)
                {
                    return GetKohiInfModel(null, null, null, sinDate, new List<ConfirmDateModel>());
                }
                int hokenNo = ptKohi.HokenNo;
                int hokenEdaNo = ptKohi.HokenEdaNo;
                HokenMst? hokenMst = hokenMstList.FirstOrDefault(h => h.HokenNo == hokenNo && h.HokenEdaNo == hokenEdaNo);

                return GetKohiInfModel(
                    ptKohi,
                    GetLastPtHokenCheck(ptKohi.HokenId, HokenGroupConstant.HokenGroupKohi),
                    hokenMst,
                    sinDate,
                    GetConfirmDateList(HokenGroupConstant.HokenGroupKohi, ptKohi.HokenId));
            }

            foreach (var item in itemList)
            {
                HokenMst? hokenMst = hokenMstList.FirstOrDefault(h => h.HokenNo == item.ptHokenInf.HokenNo && h.HokenEdaNo == item.ptHokenInf.HokenEdaNo);
                string houbetu = string.Empty;
                bool isReceKisaiOrNoHoken = false;
                if (hokenMst != null)
                {
                    houbetu = hokenMst.Houbetu ?? string.Empty;
                    isReceKisaiOrNoHoken = IsReceKisai(hokenMst) || IsNoHoken(hokenMst, item.HokenKbn, houbetu ?? string.Empty);
                }

                //get FindHokensyaMstByNoNotrack
                string houbetuNo = string.Empty;
                string hokensyaNoSearch = string.Empty;
                CIUtil.GetHokensyaHoubetu(item.HokensyaNo ?? string.Empty, ref hokensyaNoSearch, ref houbetuNo);

                HokenInfModel hokenInf = new HokenInfModel(
                                        hpId,
                                        ptId,
                                        item.HokenId,
                                        item.SeqNo,
                                        item.HokenNo,
                                        item.HokenEdaNo,
                                        item.HokenKbn,
                                        item.HokensyaNo ?? string.Empty,
                                        item.Kigo ?? string.Empty,
                                        item.Bango ?? string.Empty,
                                        item.EdaNo ?? string.Empty,
                                        item.HonkeKbn,
                                        item.StartDate,
                                        item.EndDate,
                                        item.SikakuDate,
                                        item.KofuDate,
                                        GetConfirmDate(GetLastPtHokenCheck(item.ptHokenInf.HokenId, HokenGroupConstant.HokenGroupHokenPattern)),
                                        item.KogakuKbn,
                                        item.TasukaiYm,
                                        item.TokureiYm1,
                                        item.TokureiYm2,
                                        item.GenmenKbn,
                                        item.GenmenRate,
                                        item.GenmenGaku,
                                        item.SyokumuKbn,
                                        item.KeizokuKbn,
                                        item.Tokki1 ?? string.Empty,
                                        item.Tokki2 ?? string.Empty,
                                        item.Tokki3 ?? string.Empty,
                                        item.Tokki4 ?? string.Empty,
                                        item.Tokki5 ?? string.Empty,
                                        item.RousaiKofuNo ?? string.Empty,
                                        item.RousaiRoudouCd ?? string.Empty,
                                        item.RousaiSaigaiKbn,
                                        item.RousaiKantokuCd ?? string.Empty,
                                        item.RousaiSyobyoDate,
                                        item.RyoyoStartDate,
                                        item.RyoyoEndDate,
                                        item.RousaiSyobyoCd ?? string.Empty,
                                        item.RousaiJigyosyoName ?? string.Empty,
                                        item.RousaiPrefName ?? string.Empty,
                                        item.RousaiCityName ?? string.Empty,
                                        item.RousaiReceCount,
                                        string.Empty,
                                        string.Empty,
                                        string.Empty,
                                        sinDate,
                                        item.JibaiHokenName ?? string.Empty,
                                        item.JibaiHokenTanto ?? string.Empty,
                                        item.JibaiHokenTel ?? string.Empty,
                                        item.JibaiJyusyouDate,
                                        houbetu ?? string.Empty,
                                        GetConfirmDateList(1, item.HokenId),
                                        new List<RousaiTenkiModel>(),
                                        isReceKisaiOrNoHoken,
                                        item.HokenInfIsDeleted,
                                        Mapper.Map(hokenMst, new HokenMstModel(), (src, dest) =>
                                        {
                                            return dest;
                                        }),
                                        new HokensyaMstModel(),
                                        false,
                                        false
                                        );

                InsuranceModel insuranceModel = new InsuranceModel(
                    item.HpId,
                    item.PtId,
                    birthDay,
                    item.SeqNo,
                    item.HokenSbtCd,
                    item.HokenPid,
                    item.HokenKbn,
                    sinDate,
                    item.HokenMemo,
                    hokenInf,
                    kohi1: GenerateKohiModel(item.ptKohi1),
                    kohi2: GenerateKohiModel(item.ptKohi2),
                    kohi3: GenerateKohiModel(item.ptKohi3),
                    kohi4: GenerateKohiModel(item.ptKohi4),
                    item.PatternIsDeleted,
                    item.StartDate,
                    item.EndDate,
                    false
                );
                listInsurance.Add(insuranceModel);
            }

            return listInsurance;
        }
        private HokenMstModel ConvertHokenMstModel(HokenMst? hokenMst, string prefactureName)
        {
            if (hokenMst != null)
            {
                var itemHokenMst = new HokenMstModel(
                                        hokenMst.FutanKbn,
                                        hokenMst.FutanRate,
                                        hokenMst.StartDate,
                                        hokenMst.EndDate,
                                        hokenMst.HokenNo,
                                        hokenMst.HokenEdaNo,
                                        hokenMst.HokenSname ?? string.Empty,
                                        hokenMst.Houbetu ?? string.Empty,
                                        hokenMst.HokenSbtKbn,
                                        hokenMst.CheckDigit,
                                        hokenMst.AgeStart,
                                        hokenMst.AgeEnd,
                                        hokenMst.IsFutansyaNoCheck,
                                        hokenMst.IsJyukyusyaNoCheck,
                                        hokenMst.JyukyuCheckDigit,
                                        hokenMst.IsTokusyuNoCheck,
                                        hokenMst.HokenName ?? string.Empty,
                                        hokenMst.HokenNameCd ?? string.Empty,
                                        hokenMst.HokenKohiKbn,
                                        hokenMst.IsOtherPrefValid,
                                        hokenMst.ReceKisai,
                                        hokenMst.IsLimitList,
                                        hokenMst.IsLimitListSum,
                                        hokenMst.EnTen,
                                        hokenMst.KaiLimitFutan,
                                        hokenMst.DayLimitFutan,
                                        hokenMst.MonthLimitFutan,
                                        hokenMst.MonthLimitCount,
                                        hokenMst.LimitKbn,
                                        hokenMst.CountKbn,
                                        hokenMst.FutanYusen,
                                        hokenMst.CalcSpKbn,
                                        hokenMst.MonthSpLimit,
                                        hokenMst.KogakuTekiyo,
                                        hokenMst.KogakuTotalKbn,
                                        hokenMst.KogakuHairyoKbn,
                                        hokenMst.ReceSeikyuKbn,
                                        hokenMst.ReceKisaiKokho,
                                        hokenMst.ReceKisai2,
                                        hokenMst.ReceTenKisai,
                                        hokenMst.ReceFutanRound,
                                        hokenMst.ReceZeroKisai,
                                        hokenMst.ReceSpKbn,
                                        prefactureName,
                                        hokenMst.PrefNo,
                                        hokenMst.SortNo,
                                        hokenMst.SeikyuYm,
                                        hokenMst.ReceFutanHide,
                                        hokenMst.ReceFutanKbn,
                                        hokenMst.KogakuTotalAll,
                                        false,
                                        hokenMst.DayLimitCount,
                                        new List<ExceptHokensyaModel>());
                return itemHokenMst;
            }
            return new HokenMstModel();
        }

        public bool DeleteInsuranceScan(int hpId, long seqNo, int userId)
        {
            var model = TrackingDataContext.PtHokenScans.FirstOrDefault(x => x.HpId == hpId && x.SeqNo == seqNo && x.IsDeleted == DeleteStatus.None);

            if (model is null)
                return false;

            model.IsDeleted = DeleteStatus.DeleteFlag;
            model.UpdateDate = CIUtil.GetJapanDateTimeNow();
            model.UpdateId = userId;

            return TrackingDataContext.SaveChanges() > 0;
        }

        public bool CheckHokenPatternUsed(int hpId, long ptId, int hokenPid)
        {
            return NoTrackingDataContext.OdrInfs.Any(
                                 x => x.HpId == hpId &&
                                 x.PtId == ptId &&
                                 x.HokenPid == hokenPid &&
                                 x.IsDeleted == DeleteStatus.None);
        }

        public List<KohiPriorityModel> GetKohiPriorityList()
        {
            return NoTrackingDataContext.KohiPriorities.Select(x => new KohiPriorityModel(x.PriorityNo, x.PrefNo, x.Houbetu)).ToList();
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        public List<InsuranceScanModel> GetListInsuranceScanByPtId(int hpId, long ptId)
        {
            Stream nullMemory = Stream.Null;
            var datas = NoTrackingDataContext.PtHokenScans.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == DeleteTypes.None).ToList();
            if (datas.Any())
            {
                return datas.Select(x => new InsuranceScanModel(
                                    x.HpId,
                                    x.PtId,
                                    x.SeqNo,
                                    x.HokenGrp,
                                    x.HokenId,
                                    x.FileName ?? string.Empty,
                                    nullMemory,
                                    x.IsDeleted,
                                    x.UpdateDate.ToString("yyyy/MM/dd HH:mm"))).ToList();
            }
            else
            {
                return new List<InsuranceScanModel>();
            }
        }

        public int GetHokenKbnByHokenId(int hpId, int hokenId, long ptId)
        {
            var ptHokenInf = NoTrackingDataContext.PtHokenInfs.FirstOrDefault(item => item.HpId == hpId && item.HokenId == hokenId && item.PtId == ptId);
            return ptHokenInf?.HokenKbn ?? 0;
        }
    }
}
