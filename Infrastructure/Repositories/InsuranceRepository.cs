using Domain.Constant;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
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
            var dataHokenPatterList = _tenantDataContext.PtHokenPatterns.Where(x => x.IsDeleted == DeleteStatus.None && x.PtId == ptId && x.HpId == hpId && (x.StartDate <= sinDate && x.EndDate >= sinDate)  ).OrderByDescending(x => x.HokenPid).ToList();
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
                                        ptKohi1.FutansyaNo ?? string.Empty,
                                        ptKohi1.JyukyusyaNo ?? string.Empty,
                                        ptKohi1.HokenId,
                                        ptKohi1.StartDate,
                                        ptKohi1.EndDate,
                                        GetConfirmDate(ptHokenPattern.Kohi1Id, HokenGroupConstant.HokenGroupKohi),
                                        ptKohi1.Rate,
                                        ptKohi1.GendoGaku,
                                        ptKohi1.SikakuDate,
                                        ptKohi1.KofuDate,
                                        ptKohi1.TokusyuNo ?? string.Empty,
                                        ptKohi1.HokenSbtKbn,
                                        ptKohi1.Houbetu ?? string.Empty
                                    ) : null,
                                Kohi2 = ptKohi2 != null ? new KohiInfModel(
                                            ptKohi2.FutansyaNo ?? string.Empty,
                                            ptKohi2.JyukyusyaNo ?? string.Empty,
                                            ptKohi2.HokenId,
                                            ptKohi2.StartDate,
                                            ptKohi2.EndDate,
                                            GetConfirmDate(ptHokenPattern.Kohi2Id, HokenGroupConstant.HokenGroupKohi),
                                            ptKohi2.Rate,
                                            ptKohi2.GendoGaku,
                                            ptKohi2.SikakuDate,
                                            ptKohi2.KofuDate,
                                            ptKohi2.TokusyuNo ?? string.Empty,
                                            ptKohi2.HokenSbtKbn,
                                            ptKohi2.Houbetu ?? string.Empty
                                        ) : null,
                                Kohi3 = ptKohi3 != null ? new KohiInfModel(
                                                ptKohi3.FutansyaNo ?? string.Empty,
                                                ptKohi3.JyukyusyaNo ?? string.Empty,
                                                ptKohi3.HokenId,
                                                ptKohi3.StartDate,
                                                ptKohi3.EndDate,
                                                GetConfirmDate(ptHokenPattern.Kohi3Id, HokenGroupConstant.HokenGroupKohi),
                                                ptKohi3.Rate,
                                                ptKohi3.GendoGaku,
                                                ptKohi3.SikakuDate,
                                                ptKohi3.KofuDate,
                                                ptKohi3.TokusyuNo ?? string.Empty,
                                                ptKohi3.HokenSbtKbn,
                                                ptKohi3.Houbetu ?? string.Empty
                                            ) : null,
                                Kohi4 = ptKohi4 != null ? new KohiInfModel(
                                                ptKohi4.FutansyaNo ?? string.Empty,
                                                ptKohi4.JyukyusyaNo ?? string.Empty,
                                                ptKohi4.HokenId,
                                                ptKohi4.StartDate,
                                                ptKohi4.EndDate,
                                                GetConfirmDate(ptHokenPattern.Kohi4Id, HokenGroupConstant.HokenGroupKohi),
                                                ptKohi4.Rate,
                                                ptKohi4.GendoGaku,
                                                ptKohi4.SikakuDate,
                                                ptKohi4.KofuDate,
                                                ptKohi4.TokusyuNo ?? string.Empty,
                                                ptKohi4.HokenSbtKbn,
                                                ptKohi4.Houbetu ?? string.Empty
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
            var itemList = joinQuery.ToList();

            List<InsuranceModel> result = new List<InsuranceModel>();
            if (itemList.Count > 0)
            {
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
                        sinDate
                    );

                    result.Add(insuranceModel);
                }
            }
            return result;
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
    }
}