using Domain.Constant;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class InsuranceRepository : IInsuranceRepository
    {
        private readonly TenantDataContext _tenantDataContext;
        public InsuranceRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetDataContext();
        }

        public IEnumerable<InsuranceModel> GetInsuranceListById(int hpId, long ptId, int SinDate)
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
                                Kohi1 = ptKohi1 != null ? new KohiInfModel(
                                        ptKohi1.FutansyaNo,
                                        ptKohi1.JyukyusyaNo,
                                        ptKohi1.HokenId,
                                        ptKohi1.StartDate,
                                        ptKohi1.EndDate,
                                        GetConfirmDate(ptHokenPattern.Kohi1Id, HokenGroupConstant.HokenGroupKohi),
                                        ptKohi1.Rate,
                                        ptKohi1.GendoGaku,
                                        ptKohi1.SikakuDate,
                                        ptKohi1.KofuDate,
                                        ptKohi1.TokusyuNo,
                                        ptKohi1.HokenSbtKbn,
                                        ptKohi1.Houbetu
                                    ) : null,
                                Kohi2 = ptKohi2 != null ? new KohiInfModel(
                                            ptKohi2.FutansyaNo,
                                            ptKohi2.JyukyusyaNo,
                                            ptKohi2.HokenId,
                                            ptKohi2.StartDate,
                                            ptKohi2.EndDate,
                                            GetConfirmDate(ptHokenPattern.Kohi2Id, HokenGroupConstant.HokenGroupKohi),
                                            ptKohi2.Rate,
                                            ptKohi2.GendoGaku,
                                            ptKohi2.SikakuDate,
                                            ptKohi2.KofuDate,
                                            ptKohi2.TokusyuNo,
                                            ptKohi2.HokenSbtKbn,
                                            ptKohi2.Houbetu
                                        ) : null,
                                Kohi3 = ptKohi3 != null ? new KohiInfModel(
                                                ptKohi3.FutansyaNo,
                                                ptKohi3.JyukyusyaNo,
                                                ptKohi3.HokenId,
                                                ptKohi3.StartDate,
                                                ptKohi3.EndDate,
                                                GetConfirmDate(ptHokenPattern.Kohi3Id, HokenGroupConstant.HokenGroupKohi),
                                                ptKohi3.Rate,
                                                ptKohi3.GendoGaku,
                                                ptKohi3.SikakuDate,
                                                ptKohi3.KofuDate,
                                                ptKohi3.TokusyuNo,
                                                ptKohi3.HokenSbtKbn,
                                                ptKohi3.Houbetu
                                            ) : null,
                                Kohi4 = ptKohi4 != null ? new KohiInfModel(
                                                ptKohi4.FutansyaNo,
                                                ptKohi4.JyukyusyaNo,
                                                ptKohi4.HokenId,
                                                ptKohi4.StartDate,
                                                ptKohi4.EndDate,
                                                GetConfirmDate(ptHokenPattern.Kohi4Id, HokenGroupConstant.HokenGroupKohi),
                                                ptKohi4.Rate,
                                                ptKohi4.GendoGaku,
                                                ptKohi4.SikakuDate,
                                                ptKohi4.KofuDate,
                                                ptKohi4.TokusyuNo,
                                                ptKohi4.HokenSbtKbn,
                                                ptKohi4.Houbetu
                                            ) : null,
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
                                ptHokenInf.RousaiReceCount
                            };
            var data = joinQuery.AsEnumerable().Select(
                    x => new InsuranceModel(
                        x.HpId,
                        x.PtId,
                        x.HokenId,
                        x.SeqNo,
                        x.HokenNo,
                        x.HokenEdaNo,
                        x.HokenSbtCd,
                        x.HokenPid,
                        x.HokenKbn,
                        x.Kohi1Id,
                        x.Kohi2Id,
                        x.Kohi3Id,
                        x.Kohi4Id,
                        "",
                        x.HokensyaNo,
                        x.Kigo,
                        x.Bango,
                        x.EdaNo,
                        x.HonkeKbn,
                        x.StartDate,
                        x.EndDate,
                        x.SikakuDate,
                        x.KofuDate,
                        x.ConfirmDate,
                        x.Kohi1,
                        x.Kohi2,
                        x.Kohi3,
                        x.Kohi4,
                        x.KogakuKbn,
                        x.TasukaiYm,
                        x.TokureiYm1,
                        x.TokureiYm2,
                        x.GenmenKbn,
                        x.GenmenRate,
                        x.GenmenGaku,
                        x.SyokumuKbn,
                        x.KeizokuKbn,
                        x.Tokki1,
                        x.Tokki2,
                        x.Tokki3,
                        x.Tokki4,
                        x.Tokki5,
                        x.RousaiKofuNo,
                        x.NenkinBango,
                        x.RousaiRoudouCd,
                        x.KenkoKanriBango,
                        x.RousaiSaigaiKbn,
                        x.RousaiKantokuCd,
                        x.RousaiSyobyoDate,
                        x.RyoyoStartDate,
                        x.RyoyoEndDate,
                        x.RousaiSyobyoCd,
                        x.RousaiJigyosyoName,
                        x.RousaiPrefName,
                        x.RousaiCityName,
                        x.RousaiReceCount,
                        0,
                        0,
                        0
                    )).ToList();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    var hokenMst = _tenantDataContext.HokenMsts.Where(x => x.HpId == item.HpId && x.HokenNo == item.HokenNo && x.HokenEdaNo == item.HokenEdaNo).FirstOrDefault();
                    item.HokenName = GetHokenName(item, hokenMst, SinDate);
                    var ptRousaiTenkis = _tenantDataContext.PtRousaiTenkis.Where(x => x.HpId == item.HpId && x.PtId == item.PtId && x.HokenId == item.HokenId).FirstOrDefault();
                    if (ptRousaiTenkis != null)
                    {
                        item.RousaiTenkiSinkei = ptRousaiTenkis.Sinkei;
                        item.RousaiTenkiTenki = ptRousaiTenkis.Tenki;
                        item.RousaiTenkiEndDate = ptRousaiTenkis.EndDate;
                    }
                }
            }
            return data;
        }

        public InsuranceModel? GetInsuranceById(int hpId, long ptId, int sinDate, int hokenPid)
        {
            var dataHokenPatterList = _tenantDataContext.PtHokenPatterns.Where(x => x.IsDeleted == DeleteStatus.None && x.PtId == ptId && x.HpId == hpId && x.HokenPid == hokenPid).OrderByDescending(x => x.HokenPid).ToList();
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
                                Kohi1 = ptKohi1 != null ? new KohiInfModel(
                                        ptKohi1.FutansyaNo,
                                        ptKohi1.JyukyusyaNo,
                                        ptKohi1.HokenId,
                                        ptKohi1.StartDate,
                                        ptKohi1.EndDate,
                                        GetConfirmDate(ptHokenPattern.Kohi1Id, HokenGroupConstant.HokenGroupKohi),
                                        ptKohi1.Rate,
                                        ptKohi1.GendoGaku,
                                        ptKohi1.SikakuDate,
                                        ptKohi1.KofuDate,
                                        ptKohi1.TokusyuNo,
                                        ptKohi1.HokenSbtKbn,
                                        ptKohi1.Houbetu
                                    ) : null,
                                Kohi2 = ptKohi2 != null ? new KohiInfModel(
                                            ptKohi2.FutansyaNo,
                                            ptKohi2.JyukyusyaNo,
                                            ptKohi2.HokenId,
                                            ptKohi2.StartDate,
                                            ptKohi2.EndDate,
                                            GetConfirmDate(ptHokenPattern.Kohi2Id, HokenGroupConstant.HokenGroupKohi),
                                            ptKohi2.Rate,
                                            ptKohi2.GendoGaku,
                                            ptKohi2.SikakuDate,
                                            ptKohi2.KofuDate,
                                            ptKohi2.TokusyuNo,
                                            ptKohi2.HokenSbtKbn,
                                            ptKohi2.Houbetu
                                        ) : null,
                                Kohi3 = ptKohi3 != null ? new KohiInfModel(
                                                ptKohi3.FutansyaNo,
                                                ptKohi3.JyukyusyaNo,
                                                ptKohi3.HokenId,
                                                ptKohi3.StartDate,
                                                ptKohi3.EndDate,
                                                GetConfirmDate(ptHokenPattern.Kohi3Id, HokenGroupConstant.HokenGroupKohi),
                                                ptKohi3.Rate,
                                                ptKohi3.GendoGaku,
                                                ptKohi3.SikakuDate,
                                                ptKohi3.KofuDate,
                                                ptKohi3.TokusyuNo,
                                                ptKohi3.HokenSbtKbn,
                                                ptKohi3.Houbetu
                                            ) : null,
                                Kohi4 = ptKohi4 != null ? new KohiInfModel(
                                                ptKohi4.FutansyaNo,
                                                ptKohi4.JyukyusyaNo,
                                                ptKohi4.HokenId,
                                                ptKohi4.StartDate,
                                                ptKohi4.EndDate,
                                                GetConfirmDate(ptHokenPattern.Kohi4Id, HokenGroupConstant.HokenGroupKohi),
                                                ptKohi4.Rate,
                                                ptKohi4.GendoGaku,
                                                ptKohi4.SikakuDate,
                                                ptKohi4.KofuDate,
                                                ptKohi4.TokusyuNo,
                                                ptKohi4.HokenSbtKbn,
                                                ptKohi4.Houbetu
                                            ) : null,
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
                                ptHokenInf.RousaiReceCount
                            };
            var data = joinQuery.AsEnumerable().Select(
                    x => new InsuranceModel(
                        x.HpId,
                        x.PtId,
                        x.HokenId,
                        x.SeqNo,
                        x.HokenNo,
                        x.HokenEdaNo,
                        x.HokenSbtCd,
                        x.HokenPid,
                        x.HokenKbn,
                        x.Kohi1Id,
                        x.Kohi2Id,
                        x.Kohi3Id,
                        x.Kohi4Id,
                        "",
                        x.HokensyaNo,
                        x.Kigo,
                        x.Bango,
                        x.EdaNo,
                        x.HonkeKbn,
                        x.StartDate,
                        x.EndDate,
                        x.SikakuDate,
                        x.KofuDate,
                        x.ConfirmDate,
                        x.Kohi1,
                        x.Kohi2,
                        x.Kohi3,
                        x.Kohi4,
                        x.KogakuKbn,
                        x.TasukaiYm,
                        x.TokureiYm1,
                        x.TokureiYm2,
                        x.GenmenKbn,
                        x.GenmenRate,
                        x.GenmenGaku,
                        x.SyokumuKbn,
                        x.KeizokuKbn,
                        x.Tokki1,
                        x.Tokki2,
                        x.Tokki3,
                        x.Tokki4,
                        x.Tokki5,
                        x.RousaiKofuNo,
                        x.NenkinBango,
                        x.RousaiRoudouCd,
                        x.KenkoKanriBango,
                        x.RousaiSaigaiKbn,
                        x.RousaiKantokuCd,
                        x.RousaiSyobyoDate,
                        x.RyoyoStartDate,
                        x.RyoyoEndDate,
                        x.RousaiSyobyoCd,
                        x.RousaiJigyosyoName,
                        x.RousaiPrefName,
                        x.RousaiCityName,
                        x.RousaiReceCount,
                        0,
                        0,
                        0
                    )).ToList();
            if (data.Count > 0)
            {
                var item = data[0];
                var hokenMst = _tenantDataContext.HokenMsts.Where(x => x.HpId == item.HpId && x.HokenNo == item.HokenNo && x.HokenEdaNo == item.HokenEdaNo).FirstOrDefault();
                item.HokenName = GetHokenName(item, hokenMst, sinDate);
                var ptRousaiTenkis = _tenantDataContext.PtRousaiTenkis.Where(x => x.HpId == item.HpId && x.PtId == item.PtId && x.HokenId == item.HokenId).FirstOrDefault();
                if (ptRousaiTenkis != null)
                {
                    item.RousaiTenkiSinkei = ptRousaiTenkis.Sinkei;
                    item.RousaiTenkiTenki = ptRousaiTenkis.Tenki;
                    item.RousaiTenkiEndDate = ptRousaiTenkis.EndDate;
                }
                return item;
            }
            return null;

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

        private string GetHokenName(InsuranceModel item, HokenMst? hokenMst, int SinDate)
        {
            string hokenName = item.HokenPid.ToString().PadLeft(3, '0') + ". ";

            if (!(item.StartDate <= SinDate && item.EndDate >= SinDate))
            {
                hokenName = "×" + hokenName;
            }

            string prefix = string.Empty;
            string postfix = string.Empty;
            var Kohi1 = item.Kohi1;
            var Kohi2 = item.Kohi2;
            var Kohi3 = item.Kohi3;
            var Kohi4 = item.Kohi4;
            if (item.HokenSbtCd == 0)
            {
                switch (item.HokenKbn)
                {
                    case 0:
                        if (hokenMst != null)
                        {
                            if (hokenMst.Houbetu?.ToString() == HokenConstant.HOUBETU_JIHI_108)
                            {
                                hokenName += "自費 " + hokenMst.FutanRate + "%";
                            }
                            else if (hokenMst.Houbetu?.ToString() == HokenConstant.HOUBETU_JIHI_109)
                            {
                                hokenName += "自費レセ " + hokenMst.FutanRate + "%";
                            }
                            else
                            {
                                hokenName += "自費";
                            }
                        }
                        else
                        {
                            hokenName += "自費";
                        }
                        break;
                    case 11:
                        hokenName += "労災（短期給付）";
                        break;
                    case 12:
                        hokenName += "労災（傷病年金）";
                        break;
                    case 13:
                        hokenName += "労災（アフターケア）";
                        break;
                    case 14:
                        hokenName += "自賠責";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (item.HokenSbtCd < 0)
                {
                    return hokenName;
                }
                string hokenSbtCd = item.HokenSbtCd.ToString().PadRight(3, '0');
                int firstNum = Int32.Parse(hokenSbtCd[0].ToString());
                int secondNum = Int32.Parse(hokenSbtCd[1].ToString());
                int thirNum = Int32.Parse(hokenSbtCd[2].ToString());
                switch (firstNum)
                {
                    case 1:
                        hokenName += "社保";
                        break;
                    case 2:
                        hokenName += "国保";
                        break;
                    case 3:
                        hokenName += "後期";
                        break;
                    case 4:
                        hokenName += "退職";
                        break;
                    case 5:
                        hokenName += "公費";
                        break;
                }

                if (secondNum > 0)
                {

                    if (thirNum == 1)
                    {
                        prefix += "単独";
                    }
                    else
                    {
                        prefix += thirNum + "併";
                    }

                    if (Kohi1 != null)
                    {
                        if (!string.IsNullOrEmpty(postfix))
                        {
                            postfix += "+";
                        }
                        if (Kohi1.HokenSbtKbn != 2)
                        {
                            postfix += Kohi1.Houbetu;
                        }
                        else
                        {
                            postfix += "マル長";
                        }
                    }
                    if (Kohi2 != null)
                    {
                        if (!string.IsNullOrEmpty(postfix))
                        {
                            postfix += "+";
                        }
                        if (Kohi2.HokenSbtKbn != 2)
                        {
                            postfix += Kohi2.Houbetu;
                        }
                        else
                        {
                            postfix += "マル長";
                        }
                    }
                    if (Kohi3 != null)
                    {
                        if (!string.IsNullOrEmpty(postfix))
                        {
                            postfix += "+";
                        }
                        if (Kohi3.HokenSbtKbn != 2)
                        {
                            postfix += Kohi3.Houbetu;
                        }
                        else
                        {
                            postfix += "マル長";
                        }
                    }
                    if (Kohi4 != null)
                    {
                        if (!string.IsNullOrEmpty(postfix))
                        {
                            postfix += "+";
                        }
                        if (Kohi4.HokenSbtKbn != 2)
                        {
                            postfix += Kohi4.Houbetu;
                        }
                        else
                        {
                            postfix += "マル長";
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(postfix))
            {
                return hokenName + prefix + "(" + postfix + ")";
            }
            return hokenName + prefix;
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


    }
}