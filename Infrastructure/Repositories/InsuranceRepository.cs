using Domain.Constant;
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

        public InsuranceDataModel GetInsuranceListById(int hpId, long ptId, int sinDate)
        {
            var dataHokenPatterList = _tenantDataContext.PtHokenPatterns.Where(x => x.IsDeleted == DeleteStatus.None && x.PtId == ptId && x.HpId == hpId).OrderByDescending(x => x.HokenPid);
            var dataKohi = _tenantDataContext.PtKohis.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == DeleteStatus.None);
            var dataHokenInf = _tenantDataContext.PtHokenInfs.Where(x => x.HpId == hpId && x.PtId == ptId);
            var dataHokenCheck = _tenantDataContext.PtHokenChecks.Where(x => x.HpId == hpId && x.PtID == ptId && x.IsDeleted == DeleteStatus.None);
            var dataPtInf = _tenantDataContext.PtInfs.Where(pt => pt.HpId == hpId && pt.PtId == ptId && pt.IsDelete == DeleteStatus.None);
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
                                ptRousaiTenkis = _tenantDataContext.PtRousaiTenkis.FirstOrDefault(x => x.HpId == ptHokenPattern.HpId && x.PtId == ptHokenPattern.PtId && x.HokenId == ptHokenPattern.HokenId),
                                ptHokenCheckOfHokenPattern = dataHokenCheck
                                    .Where(x => x.HokenId == ptHokenPattern.HokenId && x.HokenGrp == HokenGroupConstant.HokenGroupHokenPattern)
                                    .OrderByDescending(x => x.CheckDate).FirstOrDefault(),
                                ptHokenCheckOfKohi1 = dataHokenCheck
                                    .Where(x => x.HokenId == ptHokenPattern.Kohi1Id && x.HokenGrp == HokenGroupConstant.HokenGroupKohi)
                                    .OrderByDescending(x => x.CheckDate).FirstOrDefault(),
                                ptHokenCheckOfKohi2 = dataHokenCheck
                                    .Where(x => x.HokenId == ptHokenPattern.Kohi2Id && x.HokenGrp == HokenGroupConstant.HokenGroupKohi)
                                    .OrderByDescending(x => x.CheckDate).FirstOrDefault(),
                                ptHokenCheckOfKohi3 = dataHokenCheck
                                    .Where(x => x.HokenId == ptHokenPattern.Kohi3Id && x.HokenGrp == HokenGroupConstant.HokenGroupKohi)
                                    .OrderByDescending(x => x.CheckDate).FirstOrDefault(),
                                ptHokenCheckOfKohi4 = dataHokenCheck
                                    .Where(x => x.HokenId == ptHokenPattern.Kohi4Id && x.HokenGrp == HokenGroupConstant.HokenGroupKohi)
                                    .OrderByDescending(x => x.CheckDate).FirstOrDefault(),
                                ptKohi1,
                                ptKohi2,
                                ptKohi3,
                                ptKohi4,
                                hokenMst = _tenantDataContext.HokenMsts.FirstOrDefault(h => h.HokenNo == ptHokenInf.HokenNo && h.HokenEdaNo == ptHokenInf.HokenEdaNo),
                                hokenMst1 = _tenantDataContext.HokenMsts.FirstOrDefault(h => h.HokenNo == ptKohi1.HokenNo && h.HokenEdaNo == ptKohi1.HokenEdaNo),
                                hokenMst2 = _tenantDataContext.HokenMsts.FirstOrDefault(h => h.HokenNo == ptKohi2.HokenNo && h.HokenEdaNo == ptKohi2.HokenEdaNo),
                                hokenMst3 = _tenantDataContext.HokenMsts.FirstOrDefault(h => h.HokenNo == ptKohi3.HokenNo && h.HokenEdaNo == ptKohi3.HokenEdaNo),
                                hokenMst4 = _tenantDataContext.HokenMsts.FirstOrDefault(h => h.HokenNo == ptKohi4.HokenNo && h.HokenEdaNo == ptKohi4.HokenEdaNo),
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
                                ptHokenInf.JibaiJyusyouDate,
                                ptInf.Birthday,
                                ptHokenPattern.HokenMemo
                            };
            var itemList = joinQuery.ToList();

            List<InsuranceModel> listInsurance = new List<InsuranceModel>();
            var listHokenInf = new List<HokenInfModel>();
            var listKohi = new List<KohiInfModel>();

            if (itemList.Count > 0)
            {
                foreach (var item in itemList)
                {
                    string houbetu = string.Empty;
                    int futanRate = 0;
                    int futanKbn = 0;
                    if (item.hokenMst != null)
                    {
                        houbetu = item.hokenMst.Houbetu;
                        futanRate = item.hokenMst.FutanRate;
                        futanKbn = item.hokenMst.FutanKbn;
                    }

                    int rousaiTenkiSinkei = 0;
                    int rousaiTenkiTenki = 0;
                    int rousaiTenkiEndDate = 0;
                    if (item.ptRousaiTenkis != null)
                    {
                        rousaiTenkiSinkei = item.ptRousaiTenkis.Sinkei;
                        rousaiTenkiTenki = item.ptRousaiTenkis.Tenki;
                        rousaiTenkiEndDate = item.ptRousaiTenkis.EndDate;
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
                        confirmDate: GetConfirmDate(item.ptHokenCheckOfHokenPattern),
                        kohi1: GetKohiInfModel(item.ptKohi1, item.ptHokenCheckOfKohi1, item.hokenMst1, sinDate),
                        kohi2: GetKohiInfModel(item.ptKohi2, item.ptHokenCheckOfKohi2, item.hokenMst2, sinDate),
                        kohi3: GetKohiInfModel(item.ptKohi3, item.ptHokenCheckOfKohi3, item.hokenMst3, sinDate),
                        kohi4: GetKohiInfModel(item.ptKohi4, item.ptHokenCheckOfKohi4, item.hokenMst4, sinDate),
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
                        nenkinBango: NenkinBango(item.RousaiKofuNo),
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
                        item.Birthday,
                        item.JibaiHokenName,
                        item.JibaiHokenTanto,
                        item.JibaiHokenTel,
                        item.JibaiJyusyouDate,
                        item.HokenMemo,
                        futanKbn
                    );

                    listInsurance.Add(insuranceModel);
                }
            }

            var hokenInfs = _tenantDataContext.PtHokenInfs.Where(h => h.HpId == hpId && h.PtId == ptId)
                            .OrderByDescending(x => x.HokenId).ToList();
            if (hokenInfs.Count > 0)
            {
                foreach (var item in hokenInfs)
                {
                    var ptRousaiTenkis = _tenantDataContext.PtRousaiTenkis.FirstOrDefault(x => x.HpId == hpId && x.PtId == ptId && x.HokenId == item.HokenId);
                    var hokenMst = _tenantDataContext.HokenMsts.FirstOrDefault(h => h.HokenNo == item.HokenNo && h.HokenEdaNo == item.HokenEdaNo);
                    var dataHokenCheckHoken = _tenantDataContext.PtHokenChecks.FirstOrDefault(x => x.HpId == hpId && x.PtID == ptId && x.IsDeleted == DeleteStatus.None && x.HokenId == item.HokenId);
                    string houbetuHokenInf = string.Empty;
                    int futanRateHokenInf = 0;
                    int futanKbnHokenInf = 0;
                    int isHaveHokenMst = 0;
                    int hokenMstSubNumber = 0;
                    if (hokenMst != null)
                    {
                        houbetuHokenInf = hokenMst.Houbetu;
                        futanRateHokenInf = hokenMst.FutanRate;
                        futanKbnHokenInf = hokenMst.FutanKbn;
                        isHaveHokenMst = 1;
                        hokenMstSubNumber = hokenMst.HokenSbtKbn;
                    }

                    var tenkiSenkei = 0;
                    var tenkiTenki = 0;
                    var tenkiEndDate = 0;
                    if (ptRousaiTenkis != null)
                    {
                        tenkiSenkei = ptRousaiTenkis.Sinkei;
                        tenkiTenki = ptRousaiTenkis.Tenki;
                        tenkiEndDate = ptRousaiTenkis.EndDate;
                    }
                    var itemHokenInf = new HokenInfModel(
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
                                            GetConfirmDate(dataHokenCheckHoken),
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
                                            nenkinBango: NenkinBango(item.RousaiKofuNo),
                                            item.RousaiRoudouCd ?? string.Empty,
                                            item.RousaiKofuNo ?? string.Empty,
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
                                            tenkiSenkei,
                                            tenkiTenki,
                                            tenkiEndDate,
                                            houbetuHokenInf,
                                            futanRateHokenInf,
                                            futanKbnHokenInf,
                                            sinDate,
                                            item.JibaiHokenName ?? string.Empty,
                                            item.JibaiHokenTanto ?? string.Empty,
                                            item.JibaiHokenTel ?? string.Empty,
                                            item.JibaiJyusyouDate,
                                            isHaveHokenMst,
                                            hokenMstSubNumber,
                                            item.Houbetu ?? string.Empty
                                            );

                    listHokenInf.Add(itemHokenInf);
                }
                listHokenInf = listHokenInf.OrderBy(x => x.IsExpirated).OrderByDescending(x => x.HokenId).ToList();
            }

            var kohis = _tenantDataContext.PtKohis.Where(x => x.HpId == hpId && x.PtId == ptId).OrderByDescending(entity => entity.HokenId).ToList();
            if (kohis.Count > 0)
            {
                foreach (var item in kohis)
                {
                    var ptHokenCheckOfKohi = dataHokenCheck
                                    .Where(x => x.HokenId == item.HokenId && x.HokenGrp == HokenGroupConstant.HokenGroupKohi)
                                    .OrderByDescending(x => x.CheckDate).FirstOrDefault();
                    listKohi.Add(new KohiInfModel(
                                        item.FutansyaNo ?? string.Empty, 
                                        item.JyukyusyaNo ?? string.Empty, 
                                        item.HokenId, 
                                        item.StartDate, 
                                        item.EndDate, 
                                        GetConfirmDate(ptHokenCheckOfKohi), 
                                        item.Rate, 
                                        item.GendoGaku, 
                                        item.SikakuDate, 
                                        item.KofuDate, 
                                        item.TokusyuNo ?? string.Empty, 
                                        item.HokenSbtKbn, 
                                        item.Houbetu ?? string.Empty, 
                                        item.HokenNo, 
                                        item.HokenEdaNo,
                                        item.PrefNo, 
                                        new HokenMstModel(0, 0), 
                                        sinDate));
                }
            }

            return new InsuranceDataModel(listInsurance, listHokenInf, listKohi);
        }

        public bool CheckHokenPIdList(List<int> hokenPIds)
        {
            var check = _tenantDataContext.PtHokenPatterns.Any(p => hokenPIds.Contains(p.HokenPid) && p.IsDeleted != 1);
            return check;
        }

        private KohiInfModel GetKohiInfModel(PtKohi kohiInf, PtHokenCheck? ptHokenCheck, HokenMst? hokenMst, int sinDate)
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
                sinDate);
        }

        private HokenMstModel GetHokenMstModel(HokenMst? hokenMst)
        {
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