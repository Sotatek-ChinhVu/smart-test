using Domain.Constant;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Domain.Models.InsuranceMst;
using Entity.Tenant;
using Helper.Common;
using Helper.Mapping;
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
                                HokenInfEndDate = ptHokenInf.EndDate,
                                HokenInfIsDeleted = ptHokenInf.IsDeleted,
                                PatternIsDeleted = ptHokenPattern.IsDeleted
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
                    bool isReceKisaiOrNoHoken = false;
                    if (item.hokenMst != null)
                    {
                        houbetu = item.hokenMst.Houbetu;
                        isReceKisaiOrNoHoken = IsReceKisai(item.hokenMst) || IsNoHoken(item.hokenMst, item.HokenKbn, houbetu ?? string.Empty);
                    }
                    var ptRousaiTenkis = _tenantDataContext.PtRousaiTenkis.Where(x => x.HpId == hpId && x.PtId == ptId && x.HokenId == item.HokenId).OrderBy(x => x.EndDate)
                        .Select(x => new RousaiTenkiModel(x.Sinkei, x.Tenki, x.EndDate, x.IsDeleted, x.SeqNo)).ToList();

                    //get FindHokensyaMstByNoNotrack
                    string houbetuNo = string.Empty;
                    string hokensyaNoSearch = string.Empty;
                    CIUtil.GetHokensyaHoubetu(item.HokensyaNo ?? string.Empty, ref hokensyaNoSearch, ref houbetuNo);
                    var hokensyaMst = _tenantDataContext.HokensyaMsts.Where(x => x.HpId == hpId && x.HokensyaNo == hokensyaNoSearch && x.Houbetu == houbetuNo).Select(x => new HokensyaMstModel(x.IsKigoNa)).FirstOrDefault();
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
                                            ptRousaiTenkis,
                                            isReceKisaiOrNoHoken,
                                            item.HokenInfIsDeleted,
                                            Mapper.Map(item.hokenMst, new HokenMstModel(), (src, dest) =>
                                            {
                                                return dest;
                                            }),
                                            hokensyaMst ?? new HokensyaMstModel(),
                                            false,
                                            false,
                                            item.RousaiKofuNo ?? string.Empty
                                            ) ;

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
                        kohi4: GetKohiInfModel(item.ptKohi4, item.ptHokenCheckOfKohi4, item.hokenMst4, sinDate, GetConfirmDateList(2, item.ptKohi4?.HokenId ?? 0)),
                        item.PatternIsDeleted,
                        item.StartDate,
                        item.EndDate,
                        false
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
                        .Select(x => new RousaiTenkiModel(x.Sinkei, x.Tenki, x.EndDate, x.IsDeleted, x.SeqNo)).ToList();
                    var hokenMst = _tenantDataContext.HokenMsts.FirstOrDefault(h => h.HokenNo == item.HokenNo && h.HokenEdaNo == item.HokenEdaNo);
                    var dataHokenCheckHoken = _tenantDataContext.PtHokenChecks.FirstOrDefault(x => x.HpId == hpId && x.PtID == ptId && x.IsDeleted == DeleteStatus.None && x.HokenId == item.HokenId);
                    //get FindHokensyaMstByNoNotrack
                    string houbetuNo = string.Empty;
                    string hokensyaNoSearch = string.Empty;
                    CIUtil.GetHokensyaHoubetu(item.HokensyaNo ?? string.Empty, ref hokensyaNoSearch, ref houbetuNo);
                    var hokensyaMst = _tenantDataContext.HokensyaMsts.Where(x => x.HpId == hpId && x.HokensyaNo == hokensyaNoSearch && x.Houbetu == houbetuNo).Select(x => new HokensyaMstModel(x.IsKigoNa)).FirstOrDefault();
                    var isReceKisaiOrNoHoken = false;
                    if (hokenMst != null)
                    {
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
                                            string.Empty,
                                            string.Empty,
                                            string.Empty,
                                            sinDate,
                                            item.JibaiHokenName ?? string.Empty,
                                            item.JibaiHokenTanto ?? string.Empty,
                                            item.JibaiHokenTel ?? string.Empty,
                                            item.JibaiJyusyouDate,
                                            item.Houbetu ?? string.Empty,
                                            GetConfirmDateList(1, item.HokenId),
                                            ptRousaiTenkis,
                                            isReceKisaiOrNoHoken,
                                            item.IsDeleted,
                                            Mapper.Map(hokenMst, new HokenMstModel(), (src, dest) =>
                                            {
                                                return dest;
                                            }),
                                            hokensyaMst ?? new HokensyaMstModel(),
                                            false,
                                            false,
                                            item.RousaiKofuNo ?? string.Empty
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
                    var hokenMst = _tenantDataContext.HokenMsts.FirstOrDefault(h => h.HokenNo == item.HokenNo && h.HokenEdaNo == item.HokenEdaNo);

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
                                        Mapper.Map(hokenMst, new HokenMstModel(), (src, dest) =>
                                        {
                                            return dest;
                                        }),
                                        sinDate,
                                        GetConfirmDateList(2, item.HokenId), false,
                                        item.IsDeleted,
                                        false)
                        );
                }
            }

            return new InsuranceDataModel(listInsurance, listHokenInf, listKohi);
        }

        public bool CheckHokenPIdList(List<int> hokenPIds, List<int> hpIds, List<long> ptIds)
        {
            if (hokenPIds.Count == 0) return true;
            var countPtHokens = _tenantDataContext.PtHokenInfs.Count(p => hokenPIds.Contains(p.HokenId) && p.IsDeleted != 1 && hpIds.Contains(p.HpId) && ptIds.Contains(p.PtId));
            return countPtHokens >= hokenPIds.Count;
        }

        public bool CheckHokenPid(int hokenPId)
        {
            var check = _tenantDataContext.PtHokenInfs.Any(h => h.HokenId == hokenPId && h.IsDeleted == 0);
            return check;
        }

        public List<HokenInfModel> GetCheckListHokenInf(int hpId, long ptId, List<int> hokenPids)
        {
            var result = _tenantDataContext.PtHokenInfs.Where(h => h.HpId == hpId && hokenPids.Contains(h.HokenId) && h.PtId == ptId && h.IsDeleted == 0);
            return result.Select(r => new HokenInfModel(r.HokenId, r.PtId, r.HpId, r.StartDate, r.EndDate)).ToList();
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
                false
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

        public InsuranceModel GetPtHokenInf(int hpId, int hokenPid, long ptId, int sinDate)
        {
            var dataHokenPatterList = _tenantDataContext.PtHokenPatterns.Where(x => x.IsDeleted == DeleteStatus.None && x.PtId == ptId && x.HpId == hpId && x.HokenPid == hokenPid).OrderByDescending(x => x.HokenPid);
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
                                ptHokenInf.HokensyaAddress,
                                ptHokenInf.HokensyaName,
                                ptHokenInf.HokensyaTel,
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
                    bool isReceKisaiOrNoHoken = false;
                    if (item.hokenMst != null)
                    {
                        houbetu = item.hokenMst.Houbetu;
                        isReceKisaiOrNoHoken = IsReceKisai(item.hokenMst) || IsNoHoken(item.hokenMst, item.HokenKbn, houbetu ?? string.Empty);
                    }
                    var ptRousaiTenkis = _tenantDataContext.PtRousaiTenkis.Where(x => x.HpId == hpId && x.PtId == ptId && x.HokenId == item.HokenId).OrderBy(x => x.EndDate)
                        .Select(x => new RousaiTenkiModel(x.Sinkei, x.Tenki, x.EndDate, x.IsDeleted, x.SeqNo)).ToList();

                    //get FindHokensyaMstByNoNotrack
                    string houbetuNo = string.Empty;
                    string hokensyaNoSearch = string.Empty;
                    CIUtil.GetHokensyaHoubetu(item.HokensyaNo ?? string.Empty, ref hokensyaNoSearch, ref houbetuNo);
                    var hokensyaMst = _tenantDataContext.HokensyaMsts.Where(x => x.HpId == hpId && x.HokensyaNo == hokensyaNoSearch && x.Houbetu == houbetuNo).Select(x => new HokensyaMstModel(x.IsKigoNa)).FirstOrDefault();

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
                                            Mapper.Map(item.hokenMst, new HokenMstModel(), (src, dest) =>
                                            {
                                                return dest;
                                            }),
                                            hokensyaMst ?? new HokensyaMstModel(),
                                            false,
                                            false,
                                            item.RousaiKofuNo ?? string.Empty
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
                        kohi4: GetKohiInfModel(item.ptKohi4, item.ptHokenCheckOfKohi4, item.hokenMst4, sinDate, GetConfirmDateList(2, item.ptKohi4?.HokenId ?? 0)),
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
            var hokenPatternModels = GetInsuranceList(hpId, ptId, sinDate);
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
                if (!(historyHokenPattern.StartDate <= sinDate && historyHokenPattern.EndDate >= sinDate))
                {
                    // ① 履歴のPIDが有効な保険パターンの場合は、履歴と同じPID
                    return historyHokenPattern.HokenPid;
                }
                // HokenNashi - 保険なし
                else if (_isSameKohiHoubetu(historyHokenPattern, syosaisinHokenPattern))
                {
                    // ① 初再診と履歴が同じ組合せの法別番号の公費を持つ場合は初再診のPID
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
                        // ② 主保険なしで有効な保険パターンの中で、履歴と同じ組合せの法別番号の公費を持つPID
                        return sameKohiPattern.HokenPid;
                    }
                    else
                    {
                        // ③ ②までに該当する保険パターンが存在しない場合、初再診のPID
                        return syosaisinHokenPattern.HokenPid;
                    }
                }
            }
            else
            {
                if (!(historyHokenPattern.StartDate <= sinDate && historyHokenPattern.EndDate >= sinDate))
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
                                                       && p.HokenPid == syosaisinHokenPattern?.HokenPid
                                                       && p.BuntenKohis.Count > 0)
                                                .OrderBy(p => p.IsExpirated)
                                                .ThenBy(p => p.HokenPid)
                                                .FirstOrDefault();
                    if (sameHokenPatternBuntenKohi == null)
                    {
                        // ⓪ 初再診と同じ主保険を持つ保険パターンの中で、分点公費（HOKEN_MST.HOKEN_SBT_KBN=6）を持つ保険パターンがない場合は、初再診の保険PID
                        return syosaisinHokenPattern?.HokenPid ?? 0;
                    }
                    else
                    {
                        var sameHokenPattern = hokenPatternModels
                                                .Where(p => p.HokenSbtCd < 500 && p.HokenSbtCd > 0
                                                       && !p.IsEmptyHoken
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
                                else
                                {
                                    return syosaisinHokenPattern?.HokenPid ?? 0;
                                }
                            }
                            else
                            {
                                return syosaisinHokenPattern?.HokenPid ?? 0;
                            }
                        }
                    }
                }
            }
        }

        public List<InsuranceModel> GetInsuranceList(int hpId, long ptId, int sinDate)
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
                                HokenInfEndDate = ptHokenInf.EndDate,
                                HokenInfIsDeleted = ptHokenInf.IsDeleted,
                                PatternIsDeleted = ptHokenPattern.IsDeleted
                            };
            var itemList = joinQuery.ToList();
            List<InsuranceModel> listInsurance = new List<InsuranceModel>();

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
                var obj = new object();
                Parallel.ForEach(itemList, item =>
                {
                    {
                        string houbetu = string.Empty;
                        int futanRate = 0;
                        int futanKbn = 0;
                        int hokenMstSubNumber = 0;
                        int hokenMstStartDate = 0;
                        int hokenMstEndDate = 0;
                        int hokenMstHokenNo = 0;
                        int hokenMstHokenEdraNo = 0;
                        string hokenMstSName = string.Empty;
                        bool isReceKisaiOrNoHoken = false;
                        if (item.hokenMst != null)
                        {
                            houbetu = item.hokenMst.Houbetu;
                            futanRate = item.hokenMst.FutanRate;
                            futanKbn = item.hokenMst.FutanKbn;
                            hokenMstSubNumber = item.hokenMst.HokenSbtKbn;
                            hokenMstStartDate = item.hokenMst.StartDate;
                            hokenMstEndDate = item.hokenMst.EndDate;
                            hokenMstHokenNo = item.hokenMst.HokenNo;
                            hokenMstHokenEdraNo = item.hokenMst.HokenEdaNo;
                            hokenMstSName = item.hokenMst.HokenSname;
                            isReceKisaiOrNoHoken = IsReceKisai(item.hokenMst) || IsNoHoken(item.hokenMst, item.HokenKbn, houbetu ?? string.Empty);
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
                                                new HokenMstModel(futanKbn,
                                                                  futanRate,
                                                                  hokenMstStartDate,
                                                                  hokenMstEndDate,
                                                                  hokenMstHokenNo,
                                                                  hokenMstHokenEdraNo,
                                                                  hokenMstSName,
                                                                  houbetu ?? string.Empty,
                                                                  hokenMstSubNumber,
                                                                  item.hokenMst?.CheckDigit ?? 0,
                                                                  item.hokenMst?.AgeStart ?? 0,
                                                                  item.hokenMst?.AgeEnd ?? 0,
                                                                  item.hokenMst?.IsFutansyaNoCheck ?? 0,
                                                                  item.hokenMst?.IsJyukyusyaNoCheck ?? 0,
                                                                  item.hokenMst?.JyukyuCheckDigit ?? 0,
                                                                  item.hokenMst?.IsTokusyuNoCheck ?? 0
                                                                  ),
                                                new HokensyaMstModel(),
                                                false,
                                                false,
                                                item.RousaiKofuNo ?? string.Empty
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
                            kohi4: GetKohiInfModel(item.ptKohi4, item.ptHokenCheckOfKohi4, item.hokenMst4, sinDate, GetConfirmDateList(2, item.ptKohi4?.HokenId ?? 0)),
                            item.PatternIsDeleted,
                            item.StartDate,
                            item.EndDate,
                            false
                        );
                        lock (obj)
                        {
                            listInsurance.Add(insuranceModel);
                        }
                    }
                }
               );
            }

            return listInsurance;
        }
    }
}
