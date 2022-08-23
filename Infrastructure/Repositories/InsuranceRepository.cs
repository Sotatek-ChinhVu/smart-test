﻿using Domain.Constant;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class InsuranceRepository : IInsuranceRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        public InsuranceRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public IEnumerable<InsuranceModel> GetInsuranceListById(int hpId, long ptId, int sinDate)
        {
            var dataHokenPatterList = _tenantDataContext.PtHokenPatterns.Where(x => x.IsDeleted == DeleteStatus.None && x.PtId == ptId && x.HpId == hpId).OrderByDescending(x => x.HokenPid).ToList();
            var dataKohi = _tenantDataContext.PtKohis.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == DeleteStatus.None).ToList();
            var dataHokenInf = _tenantDataContext.PtHokenInfs.Where(x => x.HpId == hpId && x.PtId == ptId).ToList();
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
                                ptHokenPattern.Kohi1Id,
                                ptHokenPattern.Kohi2Id,
                                ptHokenPattern.Kohi3Id,
                                ptHokenPattern.Kohi4Id,
                                ptHokenInf = ptHokenInf,
                                ptHokenInf.HokensyaNo,
                                ptHokenInf.Kigo,
                                ptHokenInf.Bango,
                                ptHokenInf.EdaNo,
                                ptHokenInf.HonkeKbn,
                                ptHokenInf.StartDate,
                                ptHokenInf.EndDate,
                                ptHokenInf.SikakuDate,
                                ptHokenInf.KofuDate,
                                ConfirmDate = GetConfirmDate(ptHokenPattern.HokenId, HokenGroupConstant.HokenGroupHokenPattern),
                                Kohi1 = GetKohiInfModel(ptKohi1, ptHokenPattern.Kohi1Id),
                                Kohi2 = GetKohiInfModel(ptKohi2, ptHokenPattern.Kohi2Id),
                                Kohi3 = GetKohiInfModel(ptKohi3, ptHokenPattern.Kohi3Id),
                                Kohi4 = GetKohiInfModel(ptKohi4, ptHokenPattern.Kohi4Id),
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
                                NenkinBango = NenkinBango(ptHokenInf.RousaiKofuNo),
                                ptHokenInf.RousaiRoudouCd,
                                KenkoKanriBango = ptHokenInf.RousaiKofuNo,
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
                                ptHokenInf.JibaiJyusyouDate
                            };
            var itemList = joinQuery.ToList();

            List<InsuranceModel> result = new List<InsuranceModel>();
            if (itemList.Count > 0)
            {
                var ptInf = _tenantDataContext.PtInfs.Where(pt => pt.HpId == hpId && pt.PtId == ptId && pt.IsDelete == 0).FirstOrDefault();
                foreach (var item in itemList)
                {
                    string houbetu = string.Empty;
                    int futanRate = 0;
                    var hokenMst = _tenantDataContext.HokenMsts.FirstOrDefault(x => x.HpId == item.HpId && x.HokenNo == item.HokenNo && x.HokenEdaNo == item.HokenEdaNo);
                    
                    if (hokenMst != null)
                    {
                        houbetu = hokenMst.Houbetu;
                        futanRate = hokenMst.FutanRate;
                    }

                    int rousaiTenkiSinkei = 0;
                    int rousaiTenkiTenki = 0;
                    int rousaiTenkiEndDate = 0;
                    var ptRousaiTenkis = _tenantDataContext.PtRousaiTenkis.FirstOrDefault(x => x.HpId == item.HpId && x.PtId == item.PtId && x.HokenId == item.HokenId);
                    if (ptRousaiTenkis != null)
                    {
                        rousaiTenkiSinkei = ptRousaiTenkis.Sinkei;
                        rousaiTenkiTenki = ptRousaiTenkis.Tenki;
                        rousaiTenkiEndDate = ptRousaiTenkis.EndDate;
                    }

                    InsuranceModel insuranceModel = new InsuranceModel(
                        item.HpId,
                        item.PtId,
                        item.HokenId,
                        item.SeqNo,
                        item.HokenNo,
                        item.HokenEdaNo,
                        item.HokenSbtCd,
                        item.HokenPid,
                        item.HokenKbn,
                        item.Kohi1Id,
                        item.Kohi2Id,
                        item.Kohi3Id,
                        item.Kohi4Id,
                        item.HokensyaNo,
                        item.Kigo,
                        item.Bango,
                        item.EdaNo,
                        item.HonkeKbn,
                        item.StartDate,
                        item.EndDate,
                        item.SikakuDate,
                        item.KofuDate,
                        item.ConfirmDate,
                        item.Kohi1,
                        item.Kohi2,
                        item.Kohi3,
                        item.Kohi4,
                        item.KogakuKbn,
                        item.TasukaiYm,
                        item.TokureiYm1,
                        item.TokureiYm2,
                        item.GenmenKbn,
                        item.GenmenRate,
                        item.GenmenGaku,
                        item.SyokumuKbn,
                        item.KeizokuKbn,
                        item.Tokki1,
                        item.Tokki2,
                        item.Tokki3,
                        item.Tokki4,
                        item.Tokki5,
                        item.RousaiKofuNo,
                        item.NenkinBango,
                        item.RousaiRoudouCd,
                        item.KenkoKanriBango,
                        item.RousaiSaigaiKbn,
                        item.RousaiKantokuCd,
                        item.RousaiSyobyoDate,
                        item.RyoyoStartDate,
                        item.RyoyoEndDate,
                        item.RousaiSyobyoCd,
                        item.RousaiJigyosyoName,
                        item.RousaiPrefName,
                        item.RousaiCityName,
                        item.RousaiReceCount,
                        rousaiTenkiSinkei,
                        rousaiTenkiTenki,
                        rousaiTenkiEndDate,
                        houbetu,
                        futanRate,
                        sinDate,
                        ptInf?.Birthday == null ? 0 : ptInf.Birthday,
                        item.JibaiHokenName,
                        item.JibaiHokenTanto,
                        item.JibaiHokenTel,
                        item.JibaiJyusyouDate
                    );

                    result.Add(insuranceModel);
                }
            }
            return result;
        }

        private KohiInfModel GetKohiInfModel(PtKohi kohiInf, int hokenId)
        {
            if (kohiInf == null)
            {
                return new KohiInfModel();
            }
            return new KohiInfModel(
                kohiInf.FutansyaNo ?? string.Empty,
                kohiInf.JyukyusyaNo ?? string.Empty,
                kohiInf.HokenId,
                kohiInf.StartDate,
                kohiInf.EndDate,
                GetConfirmDate(hokenId, HokenGroupConstant.HokenGroupKohi),
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
                GetHokenMstModel(kohiInf.HokenNo, kohiInf.HokenEdaNo));
        }

        private HokenMstModel GetHokenMstModel(int hokenNo, int hokenEdaNo)
        {
            var hokenMst = _tenantDataContext.HokenMsts.FirstOrDefault(h => h.HokenNo == hokenNo && hokenEdaNo == h.HokenEdaNo);
            if (hokenMst == null)
            {
                return new HokenMstModel(0, 0);
            }
            return new HokenMstModel(hokenMst.FutanKbn, hokenMst.FutanRate);
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

        private int GetConfirmDate(int hokenId, int typeHokenGroup)
        {
            var validHokenCheck = _tenantDataContext.PtHokenChecks.Where(x => x.IsDeleted == 0 && x.HokenId == hokenId && x.HokenGrp == typeHokenGroup)
                .OrderByDescending(x => x.CheckDate)
                .ToList();
            if (!validHokenCheck.Any())
            {
                return 0;
            }
            return DateTimeToInt(validHokenCheck[0].CheckDate);
        }

        private static int DateTimeToInt(DateTime dateTime, string format = "yyyyMMdd")
        {
            int result = 0;
            result = Int32.Parse(dateTime.ToString(format));
            return result;
        }

        public IEnumerable<InsuranceModel> GetListPokenPattern(int hpId, long ptId, bool allowDisplayDeleted)
        {
            bool isAllHoken = true;
            bool isHoken = true;   //HokenKbn: 1,2
            bool isJihi = true;     //HokenKbn: 0
            bool isRosai = true;   //HokenKbn: 11,12,13 
            bool isJibai = true;   //HokenKbn: 14

            var result = _tenantDataContext.PtHokenPatterns.Where
                                (
                                    p => p.HpId == hpId && p.PtId == ptId && (p.IsDeleted == 0 || allowDisplayDeleted) &&
                                        (
                                            isAllHoken ||
                                            isHoken && (p.HokenKbn == 1 || p.HokenKbn == 2) ||
                                            isJihi && p.HokenKbn == 0 ||
                                            isRosai && (p.HokenKbn == 11 || p.HokenKbn == 12 || p.HokenKbn == 13) ||
                                            isJibai && p.HokenKbn == 14)).ToList();

            return result.Select(r => new InsuranceModel(
                        r.HpId,
                        r.PtId,
                        r.HokenPid,
                        r.SeqNo,
                        r.HokenKbn,
                        r.HokenSbtCd,
                        r.HokenId,
                        r.Kohi1Id,
                        r.Kohi2Id,
                        r.Kohi3Id,
                        r.Kohi4Id,
                        r.StartDate,
                        r.EndDate)).ToList();
        }
    }
}