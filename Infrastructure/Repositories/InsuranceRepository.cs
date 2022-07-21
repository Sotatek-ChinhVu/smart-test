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

        public IEnumerable<InsuranceModel> GetInsuranceListById(int hpId, long ptId)
        {
            var dataHokenPatterList = _tenantDataContext.PtHokenPatterns.Where(x => x.IsDeleted == DeleteStatus.None).ToList();
            var dataKohi = _tenantDataContext.PtKohis.Where(x => x.IsDeleted == DeleteStatus.None).ToList();
            var dataHokenInf = _tenantDataContext.PtHokenInfs.Where(x => x.PtId == ptId).ToList();

            var joinQuery = from ptHokenPattern in dataHokenPatterList
                            join ptHokenInf in dataHokenInf on
                                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenId } equals
                                new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId }
                            join ptKohi1 in dataKohi on
                                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi1Id } equals
                                new { ptKohi1.HpId, ptKohi1.PtId, Kohi1Id = ptKohi1.HokenId }
                            join ptKohi2 in dataKohi on
                                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi2Id } equals
                                new { ptKohi2.HpId, ptKohi2.PtId, Kohi2Id = ptKohi2.HokenId }
                            join ptKohi3 in dataKohi on
                                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi3Id } equals
                                new { ptKohi3.HpId, ptKohi3.PtId, Kohi3Id = ptKohi3.HokenId }
                            join ptKohi4 in dataKohi on
                                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi4Id } equals
                                new { ptKohi4.HpId, ptKohi4.PtId, Kohi4Id = ptKohi4.HokenId }
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
                                    Kohi1FutansyaNo = ptKohi1.FutansyaNo,
                                    Kohi1JyukyusyaNo = ptKohi1.JyukyusyaNo,
                                    Kohi1HokenId = ptKohi1.HokenId,
                                    Kohi1StartDate = ptKohi1.StartDate,
                                    Kohi1EndDate = ptKohi1.EndDate,
                                    Kohi1ConfirmDate = GetConfirmDate(ptHokenPattern.Kohi1Id, HokenGroupConstant.HokenGroupKohi),
                                    Kohi1Rate = ptKohi1.Rate,
                                    Kohi1GendoGaku = ptKohi1.GendoGaku,
                                    Kohi1SikakuDate = ptKohi1.SikakuDate,
                                    Kohi1KofuDate = ptKohi1.KofuDate,
                                    Kohi1TokusyuNo = ptKohi1.TokusyuNo,
                                    Kohi2FutansyaNo = ptKohi2.FutansyaNo,
                                    Kohi2JyukyusyaNo = ptKohi2.JyukyusyaNo,
                                    Kohi2HokenId = ptKohi2.HokenId,
                                    Kohi2StartDate = ptKohi2.StartDate,
                                    Kohi2EndDate = ptKohi2.EndDate,
                                    Kohi2ConfirmDate = GetConfirmDate(ptHokenPattern.Kohi2Id, HokenGroupConstant.HokenGroupKohi),
                                    Kohi2Rate = ptKohi2.Rate,
                                    Kohi2GendoGaku = ptKohi2.GendoGaku,
                                    Kohi2SikakuDate = ptKohi2.SikakuDate,
                                    Kohi2KofuDate = ptKohi2.KofuDate,
                                    Kohi2TokusyuNo = ptKohi2.TokusyuNo,
                                    Kohi3FutansyaNo = ptKohi3.FutansyaNo,
                                    Kohi3JyukyusyaNo = ptKohi3.JyukyusyaNo,
                                    Kohi3HokenId = ptKohi3.HokenId,
                                    Kohi3StartDate = ptKohi3.StartDate,
                                    Kohi3EndDate = ptKohi3.EndDate,
                                    Kohi3ConfirmDate = GetConfirmDate(ptHokenPattern.Kohi3Id, HokenGroupConstant.HokenGroupKohi),
                                    Kohi3Rate = ptKohi3.Rate,
                                    Kohi3GendoGaku = ptKohi3.GendoGaku,
                                    Kohi3SikakuDate = ptKohi3.SikakuDate,
                                    Kohi3KofuDate = ptKohi3.KofuDate,
                                    Kohi3TokusyuNo = ptKohi3.TokusyuNo,
                                    Kohi4FutansyaNo = ptKohi4.FutansyaNo,
                                    Kohi4JyukyusyaNo = ptKohi4.JyukyusyaNo,
                                    Kohi4HokenId = ptKohi4.HokenId,
                                    Kohi4StartDate = ptKohi4.StartDate,
                                    Kohi4EndDate = ptKohi4.EndDate,
                                    Kohi4ConfirmDate = GetConfirmDate(ptHokenPattern.Kohi4Id, HokenGroupConstant.HokenGroupKohi),
                                    Kohi4Rate = ptKohi4.Rate,
                                    Kohi4GendoGaku = ptKohi4.GendoGaku,
                                    Kohi4SikakuDate = ptKohi4.SikakuDate,
                                    Kohi4KofuDate = ptKohi4.KofuDate,
                                    Kohi4TokusyuNo = ptKohi4.TokusyuNo,
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
                        x.Kohi1FutansyaNo,
                        x.Kohi1JyukyusyaNo,
                        x.Kohi1HokenId,
                        x.Kohi1StartDate,
                        x.Kohi1EndDate,
                        x.Kohi1ConfirmDate,
                        x.Kohi1Rate,
                        x.Kohi1GendoGaku,
                        x.Kohi1SikakuDate,
                        x.Kohi1KofuDate,
                        x.Kohi1TokusyuNo,
                        x.Kohi2FutansyaNo,
                        x.Kohi2JyukyusyaNo,
                        x.Kohi2HokenId,
                        x.Kohi2StartDate,
                        x.Kohi2EndDate,
                        x.Kohi2ConfirmDate,
                        x.Kohi2Rate,
                        x.Kohi2GendoGaku,
                        x.Kohi2SikakuDate,
                        x.Kohi2KofuDate,
                        x.Kohi2TokusyuNo,
                        x.Kohi3FutansyaNo,
                        x.Kohi3JyukyusyaNo,
                        x.Kohi3HokenId,
                        x.Kohi3StartDate,
                        x.Kohi3EndDate,
                        x.Kohi3ConfirmDate,
                        x.Kohi3Rate,
                        x.Kohi3GendoGaku,
                        x.Kohi3SikakuDate,
                        x.Kohi3KofuDate,
                        x.Kohi3TokusyuNo,
                        x.Kohi4FutansyaNo,
                        x.Kohi4JyukyusyaNo,
                        x.Kohi4HokenId,
                        x.Kohi4StartDate,
                        x.Kohi4EndDate,
                        x.Kohi4ConfirmDate,
                        x.Kohi4Rate,
                        x.Kohi4GendoGaku,
                        x.Kohi4SikakuDate,
                        x.Kohi4KofuDate,
                        x.Kohi4TokusyuNo,
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
                    var hokenName = GetHokenName(item, hokenMst);
                    item.HokenName = hokenName;
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

        private string NenkinBango(string? rousaiKofuNo)
        {
            string nenkinBango = "";
            if (rousaiKofuNo != null && rousaiKofuNo.Length == 9)
            {
                nenkinBango = rousaiKofuNo.Substring(0, 2);
            }
            return nenkinBango;
        }

        private string GetHokenName(InsuranceModel item, HokenMst? hokenMst)
        {
            string hokenName = item.HokenPid.ToString().PadLeft(3, '0') + ". ";
            string prefix = string.Empty;
            string postfix = string.Empty;
            var Kohi1 = _tenantDataContext.PtKohis.Where(x => x.HpId == item.HpId && x.HokenNo == item.HokenNo && x.HokenEdaNo == item.HokenEdaNo && (hokenMst == null || x.PrefNo == hokenMst.PrefNo) && x.HokenId == item.Kohi1Id).FirstOrDefault();
            var Kohi2 = _tenantDataContext.PtKohis.Where(x => x.HpId == item.HpId && x.HokenNo == item.HokenNo && x.HokenEdaNo == item.HokenEdaNo && (hokenMst == null || x.PrefNo == hokenMst.PrefNo) && x.HokenId == item.Kohi2Id).FirstOrDefault();
            var Kohi3 = _tenantDataContext.PtKohis.Where(x => x.HpId == item.HpId && x.HokenNo == item.HokenNo && x.HokenEdaNo == item.HokenEdaNo && (hokenMst == null || x.PrefNo == hokenMst.PrefNo) && x.HokenId == item.Kohi3Id).FirstOrDefault();
            var Kohi4 = _tenantDataContext.PtKohis.Where(x => x.HpId == item.HpId && x.HokenNo == item.HokenNo && x.HokenEdaNo == item.HokenEdaNo && (hokenMst == null || x.PrefNo == hokenMst.PrefNo) && x.HokenId == item.Kohi4Id).FirstOrDefault();
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