using Domain.Constant;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Entity.Tenant;
using Helper.Common;
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
                                ptHokenPattern.HokenMemo,
                                HobetuHokenInf = ptHokenInf.Houbetu,
                                HokenInfStartDate = ptHokenInf.StartDate,
                                HokenInfEndDate = ptHokenInf.EndDate
                            };
            var itemList = joinQuery.ToList();
            List<InsuranceModel> listInsurance = new List<InsuranceModel>();
            var listHokenInf = new List<HokenInfModel>();
            var listKohi = new List<KohiInfModel>();

            var confirmDateList =
                (
                    from hokenCheck in _tenantDataContext.PtHokenChecks.Where(p => p.PtID == ptId && p.HpId == hpId && p.IsDeleted == 0)
                    join userMst in _tenantDataContext.UserMsts.Where(u => u.IsDeleted == 0)
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

            if (itemList.Count > 0)
            {
                foreach (var item in itemList)
                {
                    string houbetu = string.Empty;
                    int futanRate = 0;
                    int futanKbn = 0;
                    bool isHaveHokenMst = false;
                    int hokenMstSubNumber = 0;
                    int hokenMstStartDate = 0;
                    int hokenMstEndDate = 0;
                    int hokenMstHokenNo = 0;
                    int hokenMstHokenEdraNo = 0;
                    string hokenMstSName = string.Empty;
                    int hokenMstIsOtherPrefValid = 0;
                    if (item.hokenMst != null)
                    {
                        houbetu = item.hokenMst.Houbetu;
                        futanRate = item.hokenMst.FutanRate;
                        futanKbn = item.hokenMst.FutanKbn;
                        isHaveHokenMst = true;
                        hokenMstSubNumber = item.hokenMst.HokenSbtKbn;
                        hokenMstStartDate = item.hokenMst.StartDate;
                        hokenMstEndDate = item.hokenMst.EndDate;
                        hokenMstHokenNo = item.hokenMst.HokenNo;
                        hokenMstHokenEdraNo = item.hokenMst.HokenEdaNo;
                        hokenMstSName = item.hokenMst.HokenSname;
                        hokenMstIsOtherPrefValid = item.hokenMst.IsOtherPrefValid;

                    }
                    var ptRousaiTenkis = _tenantDataContext.PtRousaiTenkis.Where(x => x.HpId == hpId && x.PtId == ptId && x.HokenId == item.HokenId).OrderBy(x => x.EndDate)
                        .Select(x => new RousaiTenkiModel(x.Sinkei, x.Tenki, x.EndDate)).ToList();

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
                                            GetConfirmDate(item.ptHokenCheckOfHokenPattern),
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
                                            houbetu ?? string.Empty,
                                            futanRate,
                                            futanKbn,
                                            sinDate,
                                            item.JibaiHokenName ?? string.Empty,
                                            item.JibaiHokenTanto ?? string.Empty,
                                            item.JibaiHokenTel ?? string.Empty,
                                            item.JibaiJyusyouDate,
                                            isHaveHokenMst,
                                            hokenMstSubNumber,
                                            item.hokenMst?.Houbetu ?? string.Empty,
                                            GetConfirmDateList(1, item.HokenId),
                                            ptRousaiTenkis,
                                            isReceKisaiOrNoHoken
                                            );

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
                        kohi1: GetKohiInfModel(item.ptKohi1, item.ptHokenCheckOfKohi1, item.hokenMst1, sinDate, GetConfirmDateList(2, item.ptKohi1?.HokenId ?? 0)),
                        kohi2: GetKohiInfModel(item.ptKohi2, item.ptHokenCheckOfKohi2, item.hokenMst2, sinDate, GetConfirmDateList(2, item.ptKohi2?.HokenId ?? 0)),
                        kohi3: GetKohiInfModel(item.ptKohi3, item.ptHokenCheckOfKohi3, item.hokenMst3, sinDate, GetConfirmDateList(2, item.ptKohi3?.HokenId ?? 0)),
                        kohi4: GetKohiInfModel(item.ptKohi4, item.ptHokenCheckOfKohi4, item.hokenMst4, sinDate, GetConfirmDateList(2, item.ptKohi4?.HokenId ?? 0))
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
                    var ptRousaiTenkis = _tenantDataContext.PtRousaiTenkis.Where(x => x.HpId == hpId && x.PtId == ptId && x.HokenId == item.HokenId && item.IsDeleted == DeleteStatus.None).OrderBy(x => x.EndDate)
                        .Select( x => new RousaiTenkiModel(x.Sinkei, x.Tenki, x.EndDate)).ToList();
                    var hokenMst = _tenantDataContext.HokenMsts.FirstOrDefault(h => h.HokenNo == item.HokenNo && h.HokenEdaNo == item.HokenEdaNo);
                    var dataHokenCheckHoken = _tenantDataContext.PtHokenChecks.FirstOrDefault(x => x.HpId == hpId && x.PtID == ptId && x.IsDeleted == DeleteStatus.None && x.HokenId == item.HokenId);
                    string houbetuHokenInf = string.Empty;
                    int futanRateHokenInf = 0;
                    int futanKbnHokenInf = 0;
                    int isHaveHokenMst = 0;
                    int hokenMstSubNumber = 0;
                    var isReceKisaiOrNoHoken = false;

                    if (hokenMst != null)
                    {
                        houbetuHokenInf = hokenMst.Houbetu;
                        futanRateHokenInf = hokenMst.FutanRate;
                        futanKbnHokenInf = hokenMst.FutanKbn;
                        isHaveHokenMst = 1;
                        hokenMstSubNumber = hokenMst.HokenSbtKbn;
                        isReceKisaiOrNoHoken = IsReceKisai(hokenMst) || IsNoHoken(hokenMst, item.HokenKbn, item.Houbetu ?? string.Empty);
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
                                            item.Houbetu ?? string.Empty,
                                            GetConfirmDateList(1, item.HokenId),
                                            ptRousaiTenkis,
                                            isReceKisaiOrNoHoken
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
                    var HokenMasterModel = _tenantDataContext.HokenMsts.Where(hoken => hoken.HokenNo == item.HokenNo && hoken.HokenEdaNo == item.HokenEdaNo).FirstOrDefault();
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
                                        new HokenMstModel(), 
                                        sinDate,
                                        GetConfirmDateList(2, item.HokenId), false)
                        );
                }
            }

            return new InsuranceDataModel(listInsurance, listHokenInf, listKohi);
        }

        public bool CheckHokenPIdList(List<int> hokenPIds)
        {
            var check = _tenantDataContext.PtHokenPatterns.Any(p => hokenPIds.Contains(p.HokenPid) && p.IsDeleted != 1);
            return check;
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
                false
                );
        }

        private HokenMstModel GetHokenMstModel(HokenMst? hokenMst)
        {
            if (hokenMst == null)
            {
                return new HokenMstModel();
            }
            return new HokenMstModel(hokenMst.FutanKbn, hokenMst.FutanRate, hokenMst.StartDate, hokenMst.EndDate, hokenMst.HokenNo, hokenMst.HokenEdaNo, hokenMst.HokenSname);
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

        public IEnumerable<InsuranceModel> GetListHokenPattern(int hpId, long ptId, bool allowDisplayDeleted, bool isAllHoken = true, bool isHoken = true, bool isJihi = true, bool isRosai = true, bool isJibai = true)
        {

            var result = _tenantDataContext.PtHokenPatterns.Where
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
                        r.EndDate));
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
    }
}