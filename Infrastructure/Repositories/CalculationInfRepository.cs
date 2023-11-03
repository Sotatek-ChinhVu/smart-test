using Domain.CalculationInf;
using Domain.Constant;
using Domain.Models.Accounting;
using Domain.Models.CalculationInf;
using Domain.Models.Diseases;
using Domain.Models.Insurance;
using Domain.Models.Medical;
using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.PatientInfor;
using Domain.Models.Receipt;
using Domain.Models.Receipt.Recalculation;
using Domain.Models.ReceSeikyu;
using Domain.Models.SystemConf;
using Domain.Models.TodayOdr;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using System.Text;

namespace Infrastructure.Repositories
{
    public class CalculationInfRepository : RepositoryBase, ICalculationInfRepository
    {
        public readonly ISystemConfRepository _systemConfRepository;
        public CalculationInfRepository(ITenantProvider tenantProvider, ISystemConfRepository systemConfRepository) : base(tenantProvider)
        {
            _systemConfRepository = systemConfRepository;
        }

        public IEnumerable<CalculationInfModel> GetListDataCalculationInf(int hpId, long ptId)
        {
            var dataCalculation = NoTrackingDataContext.PtSanteiConfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == 0)
                .Select(x => new CalculationInfModel(
                        x.HpId,
                        x.PtId,
                        x.KbnNo,
                        x.EdaNo,
                        x.KbnVal,
                        x.StartDate,
                        x.EndDate,
                        x.SeqNo
                    ))
                .ToList();

            return dataCalculation;

        }

        public List<PtDiseaseModel> GetByomeiInThisMonth(int hpId, int sinYm, long ptId, int hokenId)
        {
            var result = new List<PtDiseaseModel>();
            int firstDateOfThisMonth = (sinYm + "01").AsInteger();
            int endDateOfThisMonth = (sinYm + "31").AsInteger();
            var ptByomeis = NoTrackingDataContext.PtByomeis.Where(p => p.HpId == hpId &&
                                                                                       p.PtId == ptId &&
                                                                                       p.IsDeleted == DeleteTypes.None &&
                                                                                       p.IsNodspRece == 0 &&
                                                                                       (p.TenkiKbn == TenkiKbnConst.Continued ||
                                                                                       p.StartDate <= endDateOfThisMonth && p.TenkiDate >= firstDateOfThisMonth) &&
                                                                                       (p.HokenPid == hokenId || p.HokenPid == 0));

            var byomeiMsts = NoTrackingDataContext.ByomeiMsts.Where(p => p.HpId == hpId);

            var query = from ptByomei in ptByomeis
                        join byomeiMst in byomeiMsts
                        on ptByomei.ByomeiCd equals byomeiMst.ByomeiCd into ByomeiMstList
                        from byomei in ByomeiMstList.DefaultIfEmpty()
                        select new
                        {
                            PtByomei = ptByomei,
                            ByomeiMst = byomei
                        };
            foreach (var entity in query)
            {
                result.Add(ConvertToModel(entity.PtByomei, entity.ByomeiMst));
            }
            return result;
        }

        public int GetCountReceInfs(int hpId, List<long> ptIds, int sinYm)
        {
            return NoTrackingDataContext.ReceInfs.Count(p => p.HpId == hpId && p.SeikyuYm == sinYm && (ptIds.Count == 0 || ptIds.Contains(p.PtId)));
        }

        public List<ReceCheckOptModel> GetReceCheckOpts(int hpId)
        {
            var receCheckOpts = NoTrackingDataContext.ReceCheckOpts.Where(p => p.HpId == hpId)
                    .Select(x => new ReceCheckOptModel(x.ErrCd, x.CheckOpt, x.Biko ?? string.Empty, x.IsInvalid)).ToList();

            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.ExpiredEndDateHokenErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.ExpiredEndDateHokenErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.UnConfirmedHokenErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.UnConfirmedHokenErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.NotExistByomeiErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.NotExistByomeiErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.CheckFirstVisit2003ByomeiErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.CheckFirstVisit2003ByomeiErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.HasNotMainByomeiErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.HasNotMainByomeiErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.InvalidByomeiErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.InvalidByomeiErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.FreeTextLengthByomeiErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.FreeTextLengthByomeiErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.CheckSuspectedByomeiErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.CheckSuspectedByomeiErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.HasNotByomeiWithOdrErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.HasNotByomeiWithOdrErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.ExpiredEndDateOdrErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.ExpiredEndDateOdrErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.FirstExamFeeCheckErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.FirstExamFeeCheckErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.SanteiCountCheckErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.SanteiCountCheckErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.TokuzaiItemCheckErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.TokuzaiItemCheckErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.ItemAgeCheckErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.ItemAgeCheckErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.CommentCheckErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.CommentCheckErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.ExceededDosageOdrErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.ExceededDosageOdrErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.DuplicateOdrErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.DuplicateOdrErrCd));
            }

            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.ByomeiBuiOrderByomeiChekkuErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.ByomeiBuiOrderByomeiChekkuErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.BuiOrderByomeiErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.BuiOrderByomeiErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.DuplicateByomeiCheckErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.DuplicateByomeiCheckErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.DuplicateSyusyokuByomeiCheckErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.DuplicateSyusyokuByomeiCheckErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.AdditionItemErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.AdditionItemErrCd));
            }
            return receCheckOpts;
        }

        public ReceCheckOptModel GetDefaultReceCheckOpt(string errCd)
        {
            var defaultReceCheckOpt = new ReceCheckOpt() { ErrCd = errCd };
            if (errCd == ReceErrCdConst.CheckSuspectedByomeiErrCd)
            {
                defaultReceCheckOpt.CheckOpt = 3;
            }
            return new ReceCheckOptModel(defaultReceCheckOpt.ErrCd, defaultReceCheckOpt.CheckOpt, defaultReceCheckOpt.Biko ?? string.Empty, defaultReceCheckOpt.IsInvalid);
        }

        public List<ReceInfModel> GetReceInfModels(int hpId, List<long> ptIds, int sinYM)
        {
            List<ReceInfModel> receInfModels = new List<ReceInfModel>();
            var ptInfs = NoTrackingDataContext.PtInfs.Where(p => p.HpId == hpId && p.IsDelete == DeleteTypes.None);

            var receStates = NoTrackingDataContext.ReceStatuses.Where(p => p.HpId == hpId && p.SeikyuYm == sinYM && (!ptIds.Any() || ptIds.Contains(p.PtId))).ToList();

            var receInfs = NoTrackingDataContext.ReceInfs.Where(p => p.HpId == hpId && p.SeikyuYm == sinYM && (!ptIds.Any() || ptIds.Contains(p.PtId))).ToList();

            var ptHokenInfs = NoTrackingDataContext.PtHokenInfs.Where(p => p.HpId == hpId && p.IsDeleted == DeleteTypes.None && (!ptIds.Any() || ptIds.Contains(p.PtId))).ToList();

            var ptKohiInfs = NoTrackingDataContext.PtKohis.Where(p => p.HpId == hpId && p.IsDeleted == DeleteTypes.None && (!ptIds.Any() || ptIds.Contains(p.PtId))).ToList();

            var receInfJoinPtInfQuery = from receInf in receInfs
                                        join ptInf in ptInfs
                                        on receInf.PtId equals ptInf.PtId
                                        select new
                                        {
                                            PtInf = ptInf,
                                            ReceInf = receInf
                                        };

            var query = from receInfJoinPtInf in receInfJoinPtInfQuery
                        join ptHokenInf in ptHokenInfs
                        on new { receInfJoinPtInf.ReceInf.PtId, receInfJoinPtInf.ReceInf.HokenId } equals new { ptHokenInf.PtId, ptHokenInf.HokenId } into tempPtHokenInfs
                        from tempPtHokenInf in tempPtHokenInfs.DefaultIfEmpty()
                        where receInfJoinPtInf.ReceInf.HokenId != 0
                        select new
                        {
                            PtInf = receInfJoinPtInf.PtInf,
                            ReceInf = receInfJoinPtInf.ReceInf,
                            PtHokenInf = tempPtHokenInf
                        };

            var query1 = from queryEntity in query
                         join ptKohiInf in ptKohiInfs
                         on new { queryEntity.ReceInf.PtId, HokenId = queryEntity.ReceInf.Kohi1Id } equals new { ptKohiInf.PtId, ptKohiInf.HokenId } into tempPtKohiInfs
                         from tempPtKohiInf in tempPtKohiInfs.DefaultIfEmpty()
                         select new
                         {
                             PtInf = queryEntity.PtInf,
                             ReceInf = queryEntity.ReceInf,
                             PtHokenInf = queryEntity.PtHokenInf,
                             PtKohi1Inf = tempPtKohiInf
                         };

            var query2 = from query1Entity in query1
                         join ptKohiInf in ptKohiInfs
                         on new { query1Entity.ReceInf.PtId, HokenId = query1Entity.ReceInf.Kohi2Id } equals new { ptKohiInf.PtId, ptKohiInf.HokenId } into tempPtKohiInfs
                         from tempPtKohiInf in tempPtKohiInfs.DefaultIfEmpty()
                         select new
                         {
                             PtInf = query1Entity.PtInf,
                             ReceInf = query1Entity.ReceInf,
                             PtHokenInf = query1Entity.PtHokenInf,
                             PtKohi1Inf = query1Entity.PtKohi1Inf,
                             PtKohi2Inf = tempPtKohiInf
                         };
            var query3 = from query2Entity in query2
                         join ptKohiInf in ptKohiInfs
                         on new { query2Entity.ReceInf.PtId, HokenId = query2Entity.ReceInf.Kohi3Id } equals new { ptKohiInf.PtId, ptKohiInf.HokenId } into tempPtKohiInfs
                         from tempPtKohiInf in tempPtKohiInfs.DefaultIfEmpty()
                         select new
                         {
                             PtInf = query2Entity.PtInf,
                             ReceInf = query2Entity.ReceInf,
                             PtHokenInf = query2Entity.PtHokenInf,
                             PtKohi1Inf = query2Entity.PtKohi1Inf,
                             PtKohi2Inf = query2Entity.PtKohi2Inf,
                             PtKohi3Inf = tempPtKohiInf
                         };
            var query4 = from query3Entity in query3
                         join ptKohiInf in ptKohiInfs
                         on new { query3Entity.ReceInf.PtId, HokenId = query3Entity.ReceInf.Kohi4Id } equals new { ptKohiInf.PtId, ptKohiInf.HokenId } into tempPtKohiInfs
                         from tempPtKohiInf in tempPtKohiInfs.DefaultIfEmpty()
                         select new
                         {
                             PtInf = query3Entity.PtInf,
                             ReceInf = query3Entity.ReceInf,
                             PtHokenInf = query3Entity.PtHokenInf,
                             PtKohi1Inf = query3Entity.PtKohi1Inf,
                             PtKohi2Inf = query3Entity.PtKohi2Inf,
                             PtKohi3Inf = query3Entity.PtKohi3Inf,
                             PtKohi4Inf = tempPtKohiInf
                         };

            var hokenChecks = NoTrackingDataContext.PtHokenChecks.Where(p => p.HpId == hpId &&
                                                                             p.IsDeleted == DeleteTypes.None &&
                                                                             (!ptIds.Any() || ptIds.Contains(p.PtID)));

            var query5 = from query4entity in query4
                         join hokenCheck in hokenChecks.Where(p => p.HokenGrp == 1)
                         on new { query4entity.ReceInf.PtId, query4entity.ReceInf.HokenId } equals new { PtId = hokenCheck.PtID, hokenCheck.HokenId } into tempHokenChecks
                         select new
                         {
                             PtInf = query4entity.PtInf,
                             ReceInf = query4entity.ReceInf,
                             PtHokenInf = query4entity.PtHokenInf,
                             PtKohi1Inf = query4entity.PtKohi1Inf,
                             PtKohi2Inf = query4entity.PtKohi2Inf,
                             PtKohi3Inf = query4entity.PtKohi3Inf,
                             PtKohi4Inf = query4entity.PtKohi4Inf,
                             HokenChecks = tempHokenChecks

                         };

            var query6 = from query5entity in query5
                         join hokenCheck in hokenChecks.Where(p => p.HokenGrp == 2)
                         on new { query5entity.ReceInf.PtId, HokenId = query5entity.ReceInf.Kohi1Id } equals new { PtId = hokenCheck.PtID, hokenCheck.HokenId } into tempHokenChecks
                         select new
                         {
                             PtInf = query5entity.PtInf,
                             ReceInf = query5entity.ReceInf,
                             PtHokenInf = query5entity.PtHokenInf,
                             PtKohi1Inf = query5entity.PtKohi1Inf,
                             PtKohi2Inf = query5entity.PtKohi2Inf,
                             PtKohi3Inf = query5entity.PtKohi3Inf,
                             PtKohi4Inf = query5entity.PtKohi4Inf,
                             HokenChecks = query5entity.HokenChecks,
                             Kohi1Checks = tempHokenChecks
                         };

            var query7 = from query6entity in query6
                         join hokenCheck in hokenChecks.Where(p => p.HokenGrp == 2)
                         on new { query6entity.ReceInf.PtId, HokenId = query6entity.ReceInf.Kohi2Id } equals new { PtId = hokenCheck.PtID, hokenCheck.HokenId } into tempHokenChecks
                         select new
                         {
                             PtInf = query6entity.PtInf,
                             ReceInf = query6entity.ReceInf,
                             PtHokenInf = query6entity.PtHokenInf,
                             PtKohi1Inf = query6entity.PtKohi1Inf,
                             PtKohi2Inf = query6entity.PtKohi2Inf,
                             PtKohi3Inf = query6entity.PtKohi3Inf,
                             PtKohi4Inf = query6entity.PtKohi4Inf,
                             HokenChecks = query6entity.HokenChecks,
                             Kohi1Checks = query6entity.Kohi1Checks,
                             Kohi2Checks = tempHokenChecks
                         };

            var query8 = from query7entity in query7
                         join hokenCheck in hokenChecks.Where(p => p.HokenGrp == 2)
                         on new { query7entity.ReceInf.PtId, HokenId = query7entity.ReceInf.Kohi3Id } equals new { PtId = hokenCheck.PtID, hokenCheck.HokenId } into tempHokenChecks
                         select new
                         {
                             PtInf = query7entity.PtInf,
                             ReceInf = query7entity.ReceInf,
                             PtHokenInf = query7entity.PtHokenInf,
                             PtKohi1Inf = query7entity.PtKohi1Inf,
                             PtKohi2Inf = query7entity.PtKohi2Inf,
                             PtKohi3Inf = query7entity.PtKohi3Inf,
                             PtKohi4Inf = query7entity.PtKohi4Inf,
                             HokenChecks = query7entity.HokenChecks,
                             Kohi1Checks = query7entity.Kohi1Checks,
                             Kohi2Checks = query7entity.Kohi2Checks,
                             Kohi3Checks = tempHokenChecks
                         };

            var query9 = from query8entity in query8
                         join hokenCheck in hokenChecks.Where(p => p.HokenGrp == 2)
                         on new { query8entity.ReceInf.PtId, HokenId = query8entity.ReceInf.Kohi4Id } equals new { PtId = hokenCheck.PtID, hokenCheck.HokenId } into tempHokenChecks
                         select new
                         {
                             PtInf = query8entity.PtInf,
                             ReceInf = query8entity.ReceInf,
                             PtHokenInf = query8entity.PtHokenInf,
                             PtKohi1Inf = query8entity.PtKohi1Inf,
                             PtKohi2Inf = query8entity.PtKohi2Inf,
                             PtKohi3Inf = query8entity.PtKohi3Inf,
                             PtKohi4Inf = query8entity.PtKohi4Inf,
                             HokenChecks = query8entity.HokenChecks,
                             Kohi1Checks = query8entity.Kohi1Checks,
                             Kohi2Checks = query8entity.Kohi2Checks,
                             Kohi3Checks = query8entity.Kohi3Checks,
                             Kohi4Checks = tempHokenChecks
                         };

            var query10 = from query9entity in query9
                          join receState in receStates
                          on new { query9entity.ReceInf.PtId, query9entity.ReceInf.HokenId } equals new { receState.PtId, receState.HokenId } into tempReceStates
                          select new
                          {
                              PtInf = query9entity.PtInf,
                              ReceInf = query9entity.ReceInf,
                              PtHokenInf = query9entity.PtHokenInf,
                              PtKohi1Inf = query9entity.PtKohi1Inf,
                              PtKohi2Inf = query9entity.PtKohi2Inf,
                              PtKohi3Inf = query9entity.PtKohi3Inf,
                              PtKohi4Inf = query9entity.PtKohi4Inf,
                              HokenChecks = query9entity.HokenChecks,
                              Kohi1Checks = query9entity.Kohi1Checks,
                              Kohi2Checks = query9entity.Kohi2Checks,
                              Kohi3Checks = query9entity.Kohi3Checks,
                              Kohi4Checks = query9entity.Kohi4Checks,
                              ReceStatus = tempReceStates.FirstOrDefault()
                          };

            foreach (var entity in query10)
            {
                receInfModels.Add(
                    ConvertToReceInfModel(entity.ReceInf, entity.PtInf, entity.PtHokenInf, entity.PtKohi1Inf, entity.PtKohi2Inf,
                    entity.PtKohi3Inf, entity.PtKohi4Inf, entity.HokenChecks.ToList(), entity.Kohi1Checks.ToList(), entity.Kohi2Checks.ToList(), entity.Kohi3Checks.ToList(), entity.Kohi4Checks.ToList(),
                    entity.ReceStatus));
            }
            return receInfModels;
        }

        private ReceInfModel ConvertToReceInfModel(ReceInf receInf, PtInf ptInf, PtHokenInf ptHokenInf, PtKohi kohi1,
            PtKohi kohi2, PtKohi kohi3, PtKohi kohi4, List<PtHokenCheck> ptHokenCheck, List<PtHokenCheck> kohi1Checks, List<PtHokenCheck> kohi2Checks,
            List<PtHokenCheck> kohi3Checks, List<PtHokenCheck> kohi4Checks, ReceStatus receStatus)
        {
            return new ReceInfModel(
                       receInf.HpId,
                       receInf.SeikyuKbn,
                       receInf.SeikyuYm,
                       receInf.PtId,
                       ptInf.PtNum,
                       receInf.SinYm,
                       receInf.HokenId,
                       receInf.HokenId2,
                       receInf.KaId,
                       receInf.TantoId,
                       receInf.ReceSbt ?? string.Empty,
                       receInf.HokenKbn,
                       receInf.HokenSbtCd,
                       receInf.Houbetu ?? string.Empty,
                       receInf.Kohi1Id,
                       receInf.Kohi2Id,
                       receInf.Kohi3Id,
                       receInf.Kohi4Id,
                       receInf.Kohi1Houbetu ?? string.Empty,
                       receInf.Kohi2Houbetu ?? string.Empty,
                       receInf.Kohi3Houbetu ?? string.Empty,
                       receInf.Kohi4Houbetu ?? string.Empty,
                       receInf.HonkeKbn,
                       receInf.Tokki1 ?? string.Empty,
                       receInf.Tokki2 ?? string.Empty,
                       receInf.Tokki3 ?? string.Empty,
                       receInf.Tokki4 ?? string.Empty,
                       receInf.Tokki5 ?? string.Empty,
                       receInf?.HokenNissu ?? -1,
                       receInf?.Kohi1Nissu ?? -1,
                       receInf?.Kohi2Nissu ?? -1,
                       receInf?.Kohi3Nissu ?? -1,
                       receInf?.Kohi4Nissu ?? -1,
                       receInf?.Kohi1ReceKyufu ?? -1,
                       receInf?.Kohi2ReceKyufu ?? -1,
                       receInf?.Kohi3ReceKyufu ?? -1,
                       receInf?.Kohi4ReceKyufu ?? -1,
                       receInf?.HokenReceTensu ?? -1,
                       receInf?.HokenReceFutan ?? -1,
                       receInf?.Kohi1ReceTensu ?? -1,
                       receInf?.Kohi1ReceFutan ?? -1,
                       receInf?.Kohi2ReceTensu ?? -1,
                       receInf?.Kohi2ReceFutan ?? -1,
                       receInf?.Kohi3ReceTensu ?? -1,
                       receInf?.Kohi3ReceFutan ?? -1,
                       receInf?.Kohi4ReceTensu ?? -1,
                       receInf?.Kohi4ReceFutan ?? -1,
                       receInf?.IsTester ?? 0,
                       ConvertPtInfModel(ptInf),
                       ConvertHokenInfModel(ptHokenInf),
                       ConvertKohiInfModel(kohi1),
                       ConvertKohiInfModel(kohi2),
                       ConvertKohiInfModel(kohi3),
                       ConvertKohiInfModel(kohi4),
                       ConvertConfirmDate(ptHokenCheck),
                       ConvertConfirmDate(kohi1Checks),
                       ConvertConfirmDate(kohi2Checks),
                       ConvertConfirmDate(kohi3Checks),
                       ConvertConfirmDate(kohi4Checks),
                       new ReceStatusModel(receStatus != null ? receStatus.IsPaperRece : 0)
                );
        }

        private PatientInforModel ConvertPtInfModel(PtInf ptInf)
        {
            return new PatientInforModel(
                          ptInf.HpId,
                          ptInf.PtId,
                          ptInf.ReferenceNo,
                          ptInf.SeqNo,
                          ptInf.PtNum,
                          ptInf.KanaName ?? string.Empty,
                          ptInf.Name ?? string.Empty,
                          ptInf.Sex,
                          ptInf.Birthday,
                          ptInf.LimitConsFlg,
                          ptInf.IsDead,
                          ptInf.DeathDate,
                          ptInf.HomePost ?? string.Empty,
                          ptInf.HomeAddress1 ?? string.Empty,
                          ptInf.HomeAddress2 ?? string.Empty,
                          ptInf.Tel1 ?? string.Empty,
                          ptInf.Tel2 ?? string.Empty,
                          ptInf.Mail ?? string.Empty,
                          ptInf.Setanusi ?? string.Empty,
                          ptInf.Zokugara ?? string.Empty,
                          ptInf.Job ?? string.Empty,
                          ptInf.RenrakuName ?? string.Empty,
                          ptInf.RenrakuPost ?? string.Empty,
                          ptInf.RenrakuAddress1 ?? string.Empty,
                          ptInf.RenrakuAddress2 ?? string.Empty,
                          ptInf.RenrakuTel ?? string.Empty,
                          ptInf.RenrakuMemo ?? string.Empty,
                          ptInf.OfficeName ?? string.Empty,
                          ptInf.OfficePost ?? string.Empty,
                          ptInf.OfficeAddress1 ?? string.Empty,
                          ptInf.OfficeAddress2 ?? string.Empty,
                          ptInf.OfficeTel ?? string.Empty,
                          ptInf.OfficeMemo ?? string.Empty,
                          ptInf.IsRyosyoDetail,
                          ptInf.PrimaryDoctor,
                          ptInf.IsTester,
                          ptInf.MainHokenPid,
                          string.Empty,
                          0,
                          0,
                          0,
                          string.Empty,
                          0
                      );
        }

        private HokenInfModel ConvertHokenInfModel(PtHokenInf ptHoken)
        {
            return new HokenInfModel(ptHoken != null ? ptHoken.PtId : 0,
                                     ptHoken != null ? ptHoken.StartDate : 0,
                                     ptHoken != null ? ptHoken.EndDate : 0,
                                     ptHoken != null ? ptHoken.RousaiSaigaiKbn : 0,
                                     ptHoken != null ? ptHoken.RousaiSyobyoDate : 0);
        }

        private KohiInfModel ConvertKohiInfModel(PtKohi ptKohi)
        {
            return new KohiInfModel(ptKohi != null ? ptKohi.StartDate : 0,
                                    ptKohi != null ? ptKohi.EndDate : 0);
        }

        private List<ConfirmDateModel> ConvertConfirmDate(List<PtHokenCheck> ptHokenCheck)
        {
            List<ConfirmDateModel> result = new();
            foreach (var item in ptHokenCheck)
            {
                result.Add(new ConfirmDateModel(item.HokenGrp,
                item.HokenId,
                item.CheckDate,
                item.CheckId,
                item.CheckMachine ?? string.Empty,
                item.CheckCmt ?? string.Empty,
                item.IsDeleted));
            }

            return result;
        }

        public List<SinKouiCountModel> GetSinKouiCounts(int hpId, long ptId, int sinYm, int hokenId)
        {
            var result = new List<SinKouiCountModel>();

            var sinKouis = NoTrackingDataContext.SinKouis.Where(p => p.HpId == hpId &&
                                                                     p.PtId == ptId &&
                                                                     p.SinYm == sinYm &&
                                                                     p.HokenId == hokenId &&
                                                                     p.IsNodspRece == 0 &&
                                                                     p.InoutKbn == 0 &&
                                                                     p.IsDeleted == DeleteTypes.None);

            var ptHokenPatterns = NoTrackingDataContext.PtHokenPatterns.Where(p => p.HpId == hpId &&
                                                                                                   p.PtId == ptId);

            var sinKouiJoinPatternQuery = from sinKoui in sinKouis
                                          join ptHokenPattern in ptHokenPatterns
                                          on sinKoui.HokenPid equals ptHokenPattern.HokenPid
                                          select new
                                          {
                                              sinKoui,
                                              ptHokenPattern
                                          };

            var sinKouiCounts = NoTrackingDataContext.SinKouiCounts.Where(p => p.HpId == hpId &&
                                                                          p.PtId == ptId &&
                                                                          p.SinYm == sinYm)
                                                                   .OrderBy(p => p.SinDate);

            var tenMsts = NoTrackingDataContext.TenMsts.Where(p => p.HpId == hpId && p.IsDeleted == DeleteTypes.None);

            var sinKouiDetails = NoTrackingDataContext.SinKouiDetails.Where(p => p.HpId == hpId &&
                                                                                 p.PtId == ptId &&
                                                                                 p.SinYm == sinYm &&
                                                                                 p.IsDeleted == DeleteTypes.None);

            var sinKouiJoinSinKouiCountquery = from sinKouiJoinPattern in sinKouiJoinPatternQuery
                                               join sinKouiCount in sinKouiCounts
                                               on new { sinKouiJoinPattern.sinKoui.RpNo, sinKouiJoinPattern.sinKoui.SeqNo } equals new { sinKouiCount.RpNo, sinKouiCount.SeqNo }
                                               select new
                                               {
                                                   sinKouiJoinPattern.ptHokenPattern,
                                                   SinKouiCount = sinKouiCount
                                               };

            var sinKouiCountJoinDetailQuery = from sinKouiJoinSinKouiCount in sinKouiJoinSinKouiCountquery
                                              join sinKouiDetail in sinKouiDetails
                                              on new { sinKouiJoinSinKouiCount.SinKouiCount.RpNo, sinKouiJoinSinKouiCount.SinKouiCount.SeqNo }
                                              equals new { sinKouiDetail.RpNo, sinKouiDetail.SeqNo }
                                              select new
                                              {
                                                  sinKouiJoinSinKouiCount.ptHokenPattern,
                                                  SinKouiCount = sinKouiJoinSinKouiCount.SinKouiCount,
                                                  SinKouiDetail = sinKouiDetail
                                              };

            var joinTenMstQuery = from sinKouiCountJoinDetail in sinKouiCountJoinDetailQuery
                                  join tenMst in tenMsts
                                  on sinKouiCountJoinDetail.SinKouiDetail.ItemCd equals tenMst.ItemCd into tempTenMstList
                                  select new
                                  {
                                      sinKouiCountJoinDetail.SinKouiCount.PtId,
                                      sinKouiCountJoinDetail.SinKouiCount.SinDate,
                                      sinKouiCountJoinDetail.SinKouiCount.RaiinNo,
                                      sinKouiCountJoinDetail.SinKouiCount,
                                      sinKouiCountJoinDetail.SinKouiDetail,
                                      sinKouiCountJoinDetail.ptHokenPattern,
                                      TenMst = tempTenMstList.OrderByDescending(p => p.StartDate).FirstOrDefault(p => p.StartDate <= sinKouiCountJoinDetail.SinKouiCount.SinDate)
                                  };

            var groupKeys = joinTenMstQuery.GroupBy(p => new { p.PtId, p.SinDate, p.RaiinNo })
                                           .Select(p => p.FirstOrDefault());
            if (groupKeys != null)
            {
                foreach (var groupKey in groupKeys)
                {
                    if (groupKey == null)
                    {
                        continue;
                    }
                    var entities = joinTenMstQuery.Where(p => p.PtId == groupKey.PtId && p.SinDate == groupKey.SinDate && p.RaiinNo == groupKey.RaiinNo);
                    var sinKouiDetailModels = new List<SinKouiDetailModel>();
                    foreach (var entity in entities)
                    {
                        sinKouiDetailModels.Add(new SinKouiDetailModel(
                                                                        entity.SinKouiDetail.PtId,
                                                                        0,
                                                                        entity.SinKouiDetail.SinYm,
                                                                        entity.TenMst.MaxAge ?? string.Empty,
                                                                        entity.TenMst.MinAge ?? string.Empty,
                                                                        entity.SinKouiDetail.ItemCd ?? string.Empty,
                                                                        entity.SinKouiDetail.CmtOpt ?? string.Empty,
                                                                        entity.SinKouiDetail.ItemName ?? string.Empty,
                                                                        entity.TenMst.ReceName ?? string.Empty,
                                                                        entity.SinKouiDetail.Suryo,
                                                                        entity.SinKouiDetail.IsNodspRece,
                                                                        entity.TenMst.MasterSbt ?? string.Empty,
                                                                        ConvertTenMstToModel(entity.TenMst)
                                                                        ));
                    }
                    result.Add(ConvertToModel(entities.Select(p => p.ptHokenPattern).Distinct().ToList(), groupKey?.SinKouiCount ?? new(), sinKouiDetailModels));
                }
            }
            return result;
        }

        public List<ReceCheckErrModel> ClearReceCmtErr(int hpId, long ptId, int hokenId, int sinYm)
        {
            var oldReceCheckErrs = TrackingDataContext.ReceCheckErrs
                                                        .Where(p => p.HpId == hpId &&
                                                                    p.SinYm == sinYm &&
                                                                    p.PtId == ptId &&
                                                                    p.HokenId == hokenId)
                                                        .ToList();
            TrackingDataContext.ReceCheckErrs.RemoveRange(oldReceCheckErrs);

            var result = oldReceCheckErrs.Select(x => new ReceCheckErrModel(
                                                                            x.HpId,
                                                                            x.PtId,
                                                                            x.SinYm,
                                                                            x.HokenId,
                                                                            x.ErrCd,
                                                                            x.SinDate,
                                                                            x.ACd,
                                                                            x.BCd,
                                                                            x.Message1 ?? string.Empty,
                                                                            x.Message2 ?? string.Empty,
                                                                            x.IsChecked
                                                                            )).ToList();
            return result;
        }

        public List<OrdInfDetailModel> GetOdrInfsBySinDate(int hpId, long ptId, int sinDate, int hokenId)
        {
            var result = new List<OrdInfDetailModel>();

            var hokenPids = NoTrackingDataContext.PtHokenPatterns.Where(p => p.HpId == hpId &&
                                                                                              p.PtId == ptId &&
                                                                                              p.HokenId == hokenId &&
                                                                                              p.IsDeleted == DeleteTypes.None)
                                                                        .Select(p => p.HokenPid).ToList();

            var odrInfDetails = NoTrackingDataContext.OdrInfDetails.Where(odrDetail => odrDetail.HpId == hpId &&
                                                                                       odrDetail.PtId == ptId &&
                                                                                       odrDetail.SinDate == sinDate).ToList();

            var odrInfs = NoTrackingDataContext.OdrInfs.Where(odr => odr.HpId == hpId &&
                                                                     odr.PtId == ptId &&
                                                                     odr.SinDate == sinDate &&
                                                                     odr.SanteiKbn == 0 &&
                                                                     hokenPids.Contains(odr.HokenPid) &&
                                                                     odr.IsDeleted == DeleteTypes.None &&
                                                                     odr.OdrKouiKbn != 10).ToList();

            var odrInfJoinDetailQuery = from odrInf in odrInfs
                                        join odrInfDetail in odrInfDetails
                                        on new { odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo }
                                        equals new { odrInfDetail.RaiinNo, odrInfDetail.RpNo, odrInfDetail.RpEdaNo }
                                        into tempOdrDetails
                                        select new
                                        {
                                            OdrInf = odrInf,
                                            OdrInfDetails = tempOdrDetails,
                                        };
            var odrInfJoinDetails = odrInfJoinDetailQuery.ToList();
            foreach (var odrInfJoinDetail in odrInfJoinDetails)
            {
                result.AddRange(odrInfJoinDetail.OdrInfDetails.Select(p => ToModel(p)));
            }
            return result;
        }

        private OrdInfDetailModel ToModel(OdrInfDetail odrInfDetail)
        {
            return new OrdInfDetailModel(
                                   odrInfDetail.HpId,
                                   odrInfDetail.RaiinNo,
                                   odrInfDetail.RpNo,
                                   odrInfDetail.RpEdaNo,
                                   odrInfDetail.RowNo,
                                   odrInfDetail.PtId,
                                   odrInfDetail.SinDate,
                                   odrInfDetail.SinKouiKbn,
                                   odrInfDetail.ItemCd ?? string.Empty,
                                   odrInfDetail.ItemName ?? string.Empty,
                                   odrInfDetail.Suryo,
                                   odrInfDetail.UnitName ?? string.Empty,
                                   odrInfDetail.UnitSBT,
                                   odrInfDetail.TermVal,
                                   odrInfDetail.KohatuKbn,
                                   odrInfDetail.SyohoKbn,
                                   odrInfDetail.SyohoLimitKbn,
                                   odrInfDetail.DrugKbn,
                                   odrInfDetail.YohoKbn,
                                   odrInfDetail.Kokuji1 ?? string.Empty,
                                   odrInfDetail.Kokiji2 ?? string.Empty,
                                   odrInfDetail.IsNodspRece,
                                   odrInfDetail.IpnCd ?? string.Empty,
                                   odrInfDetail.IpnName ?? string.Empty,
                                   odrInfDetail.JissiKbn,
                                   odrInfDetail.JissiDate ?? CIUtil.GetJapanDateTimeNow(),
                                   odrInfDetail.JissiId,
                                   odrInfDetail.JissiMachine ?? string.Empty,
                                   odrInfDetail.ReqCd ?? string.Empty,
                                   odrInfDetail.Bunkatu ?? string.Empty,
                                   odrInfDetail.CmtName ?? string.Empty,
                                   odrInfDetail.CmtOpt ?? string.Empty,
                                   odrInfDetail.FontColor ?? string.Empty,
                                   odrInfDetail.CommentNewline
                               );
        }

        public List<BuiOdrItemMstModel> GetBuiOdrItemMsts(int hpId)
        {
            return NoTrackingDataContext.BuiOdrItemMsts.Where(x => x.HpId == hpId)
                                                       .Select(x => new BuiOdrItemMstModel(x.ItemCd))
                                                       .ToList();
        }

        public List<BuiOdrItemByomeiMstModel> GetBuiOdrItemByomeiMsts(int hpId)
        {
            return NoTrackingDataContext.BuiOdrItemByomeiMsts.Where(x => x.HpId == hpId)
                                                             .Select(x => new BuiOdrItemByomeiMstModel(x.ItemCd, x.ByomeiBui, x.LrKbn, x.BothKbn))
                                                             .ToList();
        }

        public List<OrdInfModel> GetOdrInfModels(int hpId, long ptId, int sinYm, int hokenId)
        {
            List<OrdInfModel> result = new List<OrdInfModel>();

            List<int> hokenPIds = NoTrackingDataContext.SinKouis.Where(p => p.HpId == hpId &&
                                                                            p.PtId == ptId &&
                                                                            p.SinYm == sinYm &&
                                                                            p.HokenId == hokenId &&
                                                                            p.IsNodspRece == 0 &&
                                                                            p.IsDeleted == DeleteTypes.None)
                                                             .Select(p => p.HokenPid).Distinct().ToList();

            var tenMstQuery = NoTrackingDataContext.TenMsts.Where(p => p.HpId == hpId);

            var odrInfDetails = NoTrackingDataContext.OdrInfDetails.Where(odrDetail => odrDetail.HpId == hpId &&
                                                                                                odrDetail.PtId == ptId);

            var detailJoinTenMstQuery = (from odrDetail in odrInfDetails
                                         join tenMst in tenMstQuery on odrDetail.ItemCd equals tenMst.ItemCd into tempTenMsts
                                         select new
                                         {
                                             OdrDetail = odrDetail,
                                             TenMsts = tempTenMsts
                                         }).ToList();
            var odrInfs = NoTrackingDataContext.OdrInfs.Where(odr => odr.HpId == hpId &&
                                                                     odr.PtId == ptId &&
                                                                     odr.SinDate / 100 == sinYm &&
                                                                     hokenPIds.Contains(odr.HokenPid) &&
                                                                     odr.IsDeleted == DeleteTypes.None &&
                                                                     odr.OdrKouiKbn != 10).ToList();
            var odrInfJoinDetailQuery = from odrInf in odrInfs
                                        join detailJoinTenMst in detailJoinTenMstQuery
                                        on new { odrInf.RaiinNo, odrInf.SinDate, odrInf.RpNo, odrInf.RpEdaNo }
                                        equals new { detailJoinTenMst.OdrDetail.RaiinNo, detailJoinTenMst.OdrDetail.SinDate, detailJoinTenMst.OdrDetail.RpNo, detailJoinTenMst.OdrDetail.RpEdaNo }
                                        into tempOdrDetails
                                        select new
                                        {
                                            OdrInf = odrInf,
                                            OdrInfDetails = tempOdrDetails,
                                        };
            var odrInfJoinDetails = odrInfJoinDetailQuery.ToList();
            foreach (var odrInfJoinDetail in odrInfJoinDetails)
            {
                var ordInfDetailModels = new List<OrdInfDetailModel>();
                foreach (var odrInfDetail in odrInfJoinDetail.OdrInfDetails)
                {
                    ordInfDetailModels.Add(ConvertToModel(odrInfDetail.OdrDetail,
                                                          odrInfDetail.TenMsts.FirstOrDefault(p => p.StartDate <= odrInfDetail.OdrDetail.SinDate && p.EndDate >= odrInfDetail.OdrDetail.SinDate) ?? new()));
                }
                result.Add(ConvertToModel(odrInfJoinDetail.OdrInf, ordInfDetailModels));
            }

            return result;
        }

        private OrdInfDetailModel ConvertToModel(OdrInfDetail odrInfDetail, TenMst tenMst)
        {
            return new OrdInfDetailModel(
                                odrInfDetail.HpId,
                                odrInfDetail.RaiinNo,
                                odrInfDetail.RpNo,
                                odrInfDetail.RpEdaNo,
                                odrInfDetail.RowNo,
                                odrInfDetail.PtId,
                                odrInfDetail.SinDate,
                                odrInfDetail.SinKouiKbn,
                                odrInfDetail.ItemCd ?? string.Empty,
                                odrInfDetail.ItemName ?? string.Empty,
                                odrInfDetail.Suryo,
                                odrInfDetail.UnitName ?? string.Empty,
                                odrInfDetail.UnitSBT,
                                odrInfDetail.TermVal,
                                odrInfDetail.KohatuKbn,
                                odrInfDetail.SyohoKbn,
                                odrInfDetail.SyohoLimitKbn,
                                odrInfDetail.DrugKbn,
                                odrInfDetail.YohoKbn,
                                odrInfDetail.Kokuji1 ?? string.Empty,
                                odrInfDetail.Kokiji2 ?? string.Empty,
                                odrInfDetail.IsNodspRece,
                                odrInfDetail.IpnCd ?? string.Empty,
                                odrInfDetail.IpnName ?? string.Empty,
                                odrInfDetail.JissiKbn,
                                odrInfDetail.JissiDate ?? DateTime.MinValue,
                                odrInfDetail.JissiId,
                                odrInfDetail.JissiMachine ?? string.Empty,
                                odrInfDetail.ReqCd ?? string.Empty,
                                odrInfDetail.Bunkatu ?? string.Empty,
                                odrInfDetail.CmtName ?? string.Empty,
                                odrInfDetail.CmtOpt ?? string.Empty,
                                odrInfDetail.FontColor ?? string.Empty,
                                odrInfDetail.CommentNewline,
                                tenMst?.MasterSbt ?? string.Empty,
                                0,
                                0,
                                true,
                                0,
                                0,
                                tenMst?.Ten ?? 0,
                                0,
                                0,
                                0,
                                tenMst?.OdrTermVal ?? 0,
                                tenMst?.CnvTermVal ?? 0,
                                tenMst?.YjCd ?? string.Empty,
                                new(),
                                0,
                                0,
                                tenMst?.CnvUnitName ?? string.Empty,
                                tenMst?.OdrUnitName ?? string.Empty,
                                 string.Empty,
                                 string.Empty,
                                tenMst?.CmtColKeta1 ?? 0,
                                tenMst?.CmtColKeta2 ?? 0,
                                tenMst?.CmtColKeta3 ?? 0,
                                tenMst?.CmtColKeta4 ?? 0,
                                tenMst?.CmtCol2 ?? 0,
                                tenMst?.CmtCol3 ?? 0,
                                tenMst?.CmtCol4 ?? 0,
                                tenMst?.HandanGrpKbn ?? 0,
                                new()
                    );
        }

        private OrdInfModel ConvertToModel(OdrInf odrInf, List<OrdInfDetailModel> ordInfDetailModels)
        {
            return new OrdInfModel(
                         odrInf.HpId,
                         odrInf.RaiinNo,
                         odrInf.RpNo,
                         odrInf.RpEdaNo,
                         odrInf.PtId,
                         odrInf.SinDate,
                         odrInf.HokenPid,
                         odrInf.OdrKouiKbn,
                         odrInf.RpName ?? string.Empty,
                         odrInf.InoutKbn,
                         odrInf.SikyuKbn,
                         odrInf.SyohoSbt,
                         odrInf.SanteiKbn,
                         odrInf.TosekiKbn,
                         odrInf.DaysCnt,
                         odrInf.SortNo,
                         odrInf.IsDeleted,
                         odrInf.Id,
                         ordInfDetailModels,
                         DateTime.MinValue,
                         0,
                         "",
                         DateTime.MinValue,
                         0,
                         "",
                         string.Empty,
                         string.Empty
                     );
        }

        private SinKouiCountModel ConvertToModel(List<PtHokenPattern> ptHokenPatterns, SinKouiCount sinKouiCount, List<SinKouiDetailModel> sinKouiDetailModels)
        {
            return new SinKouiCountModel(
                                        sinKouiCount.HpId,
                                        sinKouiCount.PtId,
                                        sinKouiCount.SinDate,
                                        sinKouiCount.RaiinNo,
                                        ptHokenPatterns.Select(x => new PtHokenPatternModel(
                                                                        x.PtId,
                                                                        x.HokenPid,
                                                                        x.SeqNo,
                                                                        x.HokenKbn,
                                                                        x.HokenSbtCd,
                                                                        x.HokenId,
                                                                        x.Kohi1Id,
                                                                        x.Kohi2Id,
                                                                        x.Kohi3Id,
                                                                        x.Kohi4Id,
                                                                        x.HokenMemo ?? string.Empty,
                                                                        x.StartDate,
                                                                        x.EndDate
                                                        )).ToList(),
                                        sinKouiDetailModels
                                        );
        }

        private SinKouiDetailModel ConvertToModel(SinKouiDetail sinKouiDetail, SinKouiCount sinKouiCount, List<ItemCommentSuggestionModel> listCmtSelect)
        {
            return new SinKouiDetailModel(
                                          sinKouiDetail.PtId,
                                          sinKouiDetail.SinYm,
                                          sinKouiCount.SinDate,
                                          sinKouiDetail.ItemCd ?? string.Empty,
                                          sinKouiDetail.CmtOpt ?? string.Empty,
                                          sinKouiDetail.ItemName ?? string.Empty,
                                          sinKouiDetail.Suryo,
                                          sinKouiDetail.IsNodspRece,
                                          listCmtSelect
                                          );
        }

        private SinKouiDetailModel ConvertToModel(PtInf ptInf, TenMst tenMst, SinKouiDetail sinKouiDetail)
        {
            return new SinKouiDetailModel(
                                          ptInf.PtId,
                                          ptInf.PtNum,
                                          sinKouiDetail.SinYm,
                                          tenMst.MaxAge ?? string.Empty,
                                          tenMst.MinAge ?? string.Empty,
                                          sinKouiDetail.ItemCd ?? string.Empty,
                                          sinKouiDetail.CmtOpt ?? string.Empty,
                                          sinKouiDetail.ItemCd ?? string.Empty,
                                          tenMst.ReceName ?? string.Empty,
                                          sinKouiDetail.Suryo,
                                          sinKouiDetail.IsNodspRece,
                                          tenMst.MasterSbt ?? string.Empty,
                                          ConvertTenMstToModel(tenMst)
                );
        }

        private ReceSeikyuModel ConvertToModel(PtInf ptInf, ReceSeikyu receSeikyu)
        {
            return new ReceSeikyuModel(ptInf.PtId, receSeikyu.SinYm, receSeikyu.HokenId, ptInf.PtNum, receSeikyu.SeikyuKbn);
        }

        private PtDiseaseModel ConvertToModel(PtByomei ptByomei, ByomeiMst byomeiMst)
        {
            var prefixList = Enumerable.Range(1, 21)
                                       .Select(i => new PrefixSuffixModel($"SyusyokuCd{i}", ptByomei.GetPropertyValueOrDefault($"SyusyokuCd{i}", string.Empty)))
                                       .ToList();
            var byomeiCds = prefixList.Where(x => x.Name != string.Empty).Select(x => x.Name).Distinct().ToList();

            var byomeiMstList = NoTrackingDataContext.ByomeiMsts.Where(b => byomeiCds.Contains(b.ByomeiCd)).ToList();

            string fullByomei = string.Empty;
            StringBuilder byomeiStringBuilder = new();
            foreach (var item in byomeiCds)
            {
                var byomei = byomeiMstList.FirstOrDefault(b => item == b.ByomeiCd);
                if (byomei == null || byomei.ByomeiCd == null)
                {
                    continue;
                }
                byomeiStringBuilder.Append(byomei.Byomei ?? string.Empty);
            }

            fullByomei += byomeiStringBuilder.ToString();

            return new PtDiseaseModel(ptByomei != null ? ptByomei.HokenPid : 0,
                                      ptByomei != null ? ptByomei.ByomeiCd ?? string.Empty : string.Empty,
                                      fullByomei,
                                      ptByomei != null ? ptByomei.StartDate : 0,
                                      ptByomei != null ? ptByomei.TenkiDate : 0,
                                      ptByomei != null ? ptByomei.SyubyoKbn : 0,
                                      ptByomei != null ? ptByomei.Id : 0,
                                      byomeiMst != null ? byomeiMst.DelDate : 0,
                                      ptByomei != null ? ptByomei.TenkiKbn : 0,
                                      prefixList);
        }

        public string GetSanteiItemCd(int hpId, string itemCd, int sinDate)
        {
            var tenMst = NoTrackingDataContext.TenMsts.Where(p => p.HpId == hpId &&
                                                                  p.ItemCd == itemCd &&
                                                                  p.StartDate <= sinDate &&
                                                                  p.EndDate >= sinDate &&
                                                                  p.IsDeleted == DeleteTypes.None).FirstOrDefault();
            if (tenMst != null)
            {
                return tenMst.SanteiItemCd ?? string.Empty;
            }
            return string.Empty;
        }

        public List<string> GetTekiouByomei(int hpId, List<string> itemCds)
        {
            return NoTrackingDataContext.TekiouByomeiMsts
                                         .Where(p => p.HpId == hpId &&
                                                     itemCds.Contains(p.ItemCd) &&
                                                     p.IsInvalid == 0).Select(p => p.ByomeiCd).ToList();
        }

        public List<BuiOdrMstModel> GetBuiOdrMsts(int hpId)
        {
            return NoTrackingDataContext.BuiOdrMsts.Where(x => x.HpId == hpId)
                                                    .Select(x => new BuiOdrMstModel(
                                                            x.BuiId,
                                                            x.OdrBui ?? string.Empty,
                                                            x.LrKbn,
                                                            x.MustLrKbn,
                                                            x.BothKbn,
                                                            x.Koui30,
                                                            x.Koui40,
                                                            x.Koui50,
                                                            x.Koui60,
                                                            x.Koui70,
                                                            x.Koui80)).ToList();
        }

        public List<BuiOdrByomeiMstModel> GetBuiOdrByomeiMsts(int hpId)
        {
            return NoTrackingDataContext.BuiOdrByomeiMsts.Where(x => x.HpId == hpId)
                                                         .Select(x => new BuiOdrByomeiMstModel(x.BuiId, x.ByomeiBui ?? string.Empty))
                                                         .ToList();
        }

        public int GetFirstVisitWithSyosin(int hpId, long ptId, int sinDate)
        {
            var syosinBi = NoTrackingDataContext.RaiinInfs.Where(x => x.HpId == hpId
                                                                           && x.PtId == ptId
                                                                           && x.SinDate < sinDate
                                                                           && x.SyosaisinKbn == SyosaiConst.Syosin
                                                                           && x.Status >= RaiinState.TempSave
                                                                           && x.IsDeleted == DeleteTypes.None
                )
                .OrderByDescending(x => x.SinDate)
                .FirstOrDefault() ?? new();
            return syosinBi.SinDate;
        }

        public TenItemModel? FindFirstTenMst(int hpId, string itemCd)
        {
            var entity = NoTrackingDataContext.TenMsts.Where(p =>
                   p.HpId == hpId &&
                   p.ItemCd == itemCd &&
                   p.IsDeleted == DeleteTypes.None)
                .OrderBy(p => p.StartDate)
               .FirstOrDefault();

            if (entity != null)
            {
                return new TenItemModel(entity.HpId, entity.ItemCd, entity.MinAge ?? string.Empty, entity.MaxAge ?? string.Empty, entity.SanteiItemCd ?? string.Empty, entity.StartDate, entity.EndDate);
            }
            return null;
        }

        public TenItemModel? FindLastTenMst(int hpId, string itemCd)
        {
            var entity = NoTrackingDataContext.TenMsts.Where(p =>
                   p.HpId == hpId &&
                   p.ItemCd == itemCd &&
                   p.IsDeleted == DeleteTypes.None)
                .OrderByDescending(p => p.EndDate)
               .FirstOrDefault();

            if (entity != null)
            {
                return new TenItemModel(entity.HpId, entity.ItemCd, entity.MinAge ?? string.Empty, entity.MaxAge ?? string.Empty, entity.SanteiItemCd ?? string.Empty, entity.StartDate, entity.EndDate);
            }
            return null;
        }

        public List<DensiSanteiKaisuModel> FindDensiSanteiKaisuList(int hpId, int sinDate, string itemCd)
        {
            List<int> unitCds = new List<int> { 53, 121, 131, 138, 141, 142, 143, 144, 145, 146, 147, 148, 997, 998, 999 };

            var entities = NoTrackingDataContext.DensiSanteiKaisus.Where((x) =>
                    x.HpId == hpId &&
                    x.ItemCd == itemCd &&
                    x.StartDate <= sinDate &&
                    x.EndDate >= sinDate &&
                    x.IsInvalid == 0 &&
                    unitCds.Contains(x.UnitCd)
                ).ToList();

            List<DensiSanteiKaisuModel> results = new List<DensiSanteiKaisuModel>();
            entities?.ForEach(entity =>
            {
                results.Add(new DensiSanteiKaisuModel(entity.Id,
                                                      entity.HpId,
                                                      entity.ItemCd,
                                                      entity.UnitCd,
                                                      entity.MaxCount,
                                                      entity.SpJyoken,
                                                      entity.StartDate,
                                                      entity.EndDate,
                                                      entity.SeqNo,
                                                      entity.UserSetting,
                                                      entity.TargetKbn,
                                                      entity.TermCount,
                                                      entity.TermSbt,
                                                      entity.IsInvalid,
                                                      entity.ItemGrpCd));
            });

            return results;
        }

        public List<ItemGrpMstModel> FindItemGrpMst(int hpId, int sinDate, int grpSbt, long itemGrpCd)
        {
            List<ItemGrpMstModel> result = new List<ItemGrpMstModel>();
            var query = NoTrackingDataContext.itemGrpMsts.Where(p =>
                    p.HpId == hpId &&
                    p.GrpSbt == grpSbt &&
                    p.ItemGrpCd == itemGrpCd &&
                    p.StartDate <= sinDate &&
                    p.EndDate >= sinDate)
                .OrderBy(p => p.HpId)
                .ThenBy(p => p.ItemCd)
                .ThenBy(p => p.SeqNo)
                .ToList();
            foreach (var entity in query)
            {
                result.Add(new ItemGrpMstModel(entity.HpId, entity.GrpSbt, entity.ItemGrpCd, entity.StartDate, entity.EndDate, entity.ItemCd ?? string.Empty, entity.SeqNo));
            }
            return result;
        }

        public double SanteiCount(int hpId, long ptId, int startDate, int endDate, int sinDate, long raiinNo, List<string> itemCds, List<int> santeiKbns, List<int> hokenKbns)
        {
            int startYm = startDate / 100;
            int endYm = endDate / 100;

            List<int> checkHokenKbn = new List<int>();

            if (hokenKbns != null)
            {
                checkHokenKbn = hokenKbns;
            }

            List<int> checkSanteiKbn = new List<int>();

            if (santeiKbns != null)
            {
                checkSanteiKbn = santeiKbns;
            }

            var sinRpInfs = NoTrackingDataContext.SinRpInfs.Where(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinYm >= startYm &&
                o.SinYm <= endYm &&
                o.IsDeleted == DeleteStatus.None &&
                checkHokenKbn.Contains(o.HokenKbn) &&
                checkSanteiKbn.Contains(o.SanteiKbn)
            );
            var sinKouiCounts = NoTrackingDataContext.SinKouiCounts.Where(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinDate >= startDate &&
                o.SinDate <= endDate &&
                o.RaiinNo != raiinNo);
            var sinKouiDetails = NoTrackingDataContext.SinKouiDetails.Where(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.SinYm >= startYm &&
                p.SinYm <= endYm &&
                p.IsDeleted == DeleteStatus.None &&
                itemCds.Contains(p.ItemCd ?? string.Empty) &&
                p.FmtKbn != 10  // 在がん医総のダミー項目を除く
                );

            //from rece
            if (raiinNo == 0)
            {
                var sinKouis = NoTrackingDataContext.SinKouis.Where(o =>
                    o.HpId == hpId &&
                    o.PtId == ptId &&
                    o.IsDeleted == 0);

                var joinSinkouiWithSinKouiCount = from sinKouiCount in sinKouiCounts
                                                  join sinKoui in sinKouis on
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo } equals
                    new { sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.RpNo, sinKoui.SeqNo }
                                                  select new
                                                  {
                                                      SinKouiCount = sinKouiCount,
                                                      SinKoui = sinKoui
                                                  };
                var joinQuery = from sinKouiDetail in sinKouiDetails
                                join joinSinkouiCount in joinSinkouiWithSinKouiCount.Where(p => p.SinKoui.IsNodspRece == 0) on
                                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo } equals
                                    new { joinSinkouiCount.SinKouiCount.HpId, joinSinkouiCount.SinKouiCount.PtId, joinSinkouiCount.SinKouiCount.SinYm, joinSinkouiCount.SinKouiCount.RpNo, joinSinkouiCount.SinKouiCount.SeqNo }
                                join sinRpInf in sinRpInfs on
                                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo } equals
                                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                                where
                                    sinKouiDetail.HpId == hpId &&
                                    sinKouiDetail.PtId == ptId &&
                                    sinKouiDetail.SinYm >= startYm &&
                                    sinKouiDetail.SinYm <= endYm &&
                                    itemCds.Contains(sinKouiDetail.ItemCd ?? string.Empty) &&
                                    joinSinkouiCount.SinKouiCount.SinDate >= startDate &&
                                    joinSinkouiCount.SinKouiCount.SinDate <= endDate
                                group new { sinKouiDetail, joinSinkouiCount } by new { joinSinkouiCount.SinKouiCount.HpId } into A
                                select new { sum = A.Sum(a => (double)a.joinSinkouiCount.SinKouiCount.Count * (a.sinKouiDetail.Suryo <= 0 || ItemCdConst.ZaitakuTokushu.Contains(a.sinKouiDetail.ItemCd ?? string.Empty) ? 1 : a.sinKouiDetail.Suryo)) };

                var result = joinQuery.ToList();
                if (result.Any())
                {
                    return result.FirstOrDefault()?.sum ?? 0;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                var joinQuery = (
                from sinKouiDetail in sinKouiDetails
                join sinKouiCount in sinKouiCounts on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo } equals
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                where
                    sinKouiDetail.HpId == hpId &&
                    sinKouiDetail.PtId == ptId &&
                    sinKouiDetail.SinYm >= startYm &&
                    sinKouiDetail.SinYm <= endYm &&
                    itemCds.Contains(sinKouiDetail.ItemCd ?? string.Empty) &&
                    sinKouiCount.SinDate >= startDate &&
                    sinKouiCount.SinDate <= endDate &&
                    sinKouiCount.RaiinNo != raiinNo
                group new { sinKouiDetail, sinKouiCount } by new { sinKouiCount.HpId } into A
                select new { sum = A.Sum(a => (double)a.sinKouiCount.Count * (a.sinKouiDetail.Suryo <= 0 || ItemCdConst.ZaitakuTokushu.Contains(a.sinKouiDetail.ItemCd ?? string.Empty) ? 1 : a.sinKouiDetail.Suryo)) }
            );

                var result = joinQuery.ToList();
                if (result.Any())
                {
                    return result.FirstOrDefault()?.sum ?? 0;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<SinKouiModel> GetListSinKoui(int hpId, long ptId, int sinYm, int hokenId)
        {
            List<SinKouiModel> result = new();
            List<SinKoui> listSinKoui = NoTrackingDataContext.SinKouis.Where(p => p.HpId == hpId &&
                                                                                  p.PtId == ptId &&
                                                                                  p.SinYm == sinYm &&
                                                                                  p.HokenId == hokenId &&
                                                                                  p.IsNodspRece == 0)
                                                                    .ToList();
            listSinKoui.ForEach((sinKoui) =>
            {
                var listSinKouiDetailEntity = (from sinKouiDetail in NoTrackingDataContext.SinKouiDetails.Where(s => s.HpId == hpId &&
                                                                                                       s.PtId == ptId &&
                                                                                                       s.SinYm == sinYm &&
                                                                                                       s.IsNodspRece == 0 &&
                                                                                                       s.RpNo == sinKoui.RpNo &&
                                                                                                       s.SeqNo == sinKoui.SeqNo)
                                               join sinKouiCount in NoTrackingDataContext.SinKouiCounts.Where(s => s.HpId == hpId &&
                                                                                                       s.PtId == ptId &&
                                                                                                       s.SinYm == sinYm)
                                               on new { sinKouiDetail.RpNo, sinKouiDetail.SeqNo } equals new { sinKouiCount.RpNo, sinKouiCount.SeqNo }
                                               select new
                                               {
                                                   sinKouiDetail,
                                                   sinKouiCount
                                               })
                                              .ToList();

                List<SinKouiDetailModel> listSinKouiDetailModel = new();
                listSinKouiDetailEntity.ForEach((sinKouiDetail) =>
                {
                    string itemCd = sinKouiDetail.sinKouiDetail.ItemCd ?? string.Empty;
                    int sinDate = sinKouiDetail.sinKouiCount.SinDate;
                    List<ItemCommentSuggestionModel> listCmtSelect = GetListComment(hpId, new List<string> { itemCd }, sinDate, new List<int> { 0 }, true);
                    listSinKouiDetailModel.Add(ConvertToModel(sinKouiDetail.sinKouiDetail, sinKouiDetail.sinKouiCount, listCmtSelect));
                });
                result.Add(new SinKouiModel(
                    sinKoui.PtId,
                    sinKoui.SinYm,
                    sinKoui.RpNo,
                    sinKoui.HokenPid,
                    sinKoui.HokenId,
                    listSinKouiDetailModel));
            });

            return result;
        }

        public List<ItemCommentSuggestionModel> GetListComment(int hpCd, List<string> listItemCd, int sinDate, List<int> isInvalidList, bool isRecalculation = false)
        {
            List<ItemCommentSuggestionModel> result = NoTrackingDataContext.TenMsts
                                                 .Where(item => listItemCd.Contains(item.ItemCd) &&
                                                                              item.StartDate <= sinDate &&
                                                                              sinDate <= item.EndDate)
                                                 .AsEnumerable()
                                                 .Select(item => new ItemCommentSuggestionModel(item.ItemCd, "【" + item.Name + "】", item.SanteiItemCd ?? string.Empty, new()))
                                                 .ToList();

            if (result.Count <= 0)
                return new List<ItemCommentSuggestionModel>();

            var listItemCdCmtSelect = result.Select(item => item.SanteiItemCd).ToList();
            listItemCdCmtSelect.AddRange(listItemCd);
            listItemCdCmtSelect = listItemCdCmtSelect.Distinct().ToList();

            var listRecedenCmtSelectAll = NoTrackingDataContext.RecedenCmtSelects
                .Where(item => item.HpId == hpCd &&
                                               listItemCdCmtSelect.Contains(item.ItemCd) &&
                                               item.StartDate <= sinDate &&
                                               sinDate <= item.EndDate &&
                                               isInvalidList.Contains(item.IsInvalid))
                .ToList();

            if (listRecedenCmtSelectAll.Count <= 0)
                return new List<ItemCommentSuggestionModel>();

            var listItemNo = listRecedenCmtSelectAll.GroupBy(item => new { item.ItemCd, item.CommentCd })
                .Select(grp => grp.OrderBy(r => r.ItemNo).First())
                .Select(item => item.ItemNo)
                .Distinct()
                .ToList();

            List<RecedenCmtSelect> listRecedenCmtMinEda = new List<RecedenCmtSelect>();

            var listRecedenCmtSelect = NoTrackingDataContext.RecedenCmtSelects
               .Where(item => item.HpId == hpCd &&
                                              listItemCdCmtSelect.Contains(item.ItemCd) &&
                                              listItemNo.Contains(item.ItemNo) &&
                                              item.StartDate <= sinDate &&
                                              sinDate <= item.EndDate &&
                                              isInvalidList.Contains(item.IsInvalid))
               .ToList();
            listRecedenCmtMinEda.AddRange(listRecedenCmtSelect);

            if (!isRecalculation)
            {
                var listRecedenCmtSelectShinryo = NoTrackingDataContext.RecedenCmtSelects
                .Where(item => item.HpId == hpCd &&
                                      item.ItemCd == "199999999" &&
                                      listItemNo.Contains(item.ItemNo) &&
                                      item.StartDate <= sinDate &&
                                      sinDate <= item.EndDate &&
                                      isInvalidList.Contains(item.IsInvalid));
                listRecedenCmtMinEda.AddRange(listRecedenCmtSelectShinryo);
            }

            var listCommentCd = listRecedenCmtMinEda.Select(item => item.CommentCd).Distinct();

            var listTenMst = NoTrackingDataContext.TenMsts
                .Where(item => item.HpId == hpCd &&
                                         listCommentCd.Contains(item.ItemCd) &&
                                         item.StartDate <= sinDate &&
                                         sinDate <= item.EndDate);

            var listComment = (from recedenCmtSelect in listRecedenCmtMinEda
                               join tenMst in listTenMst on
                                   recedenCmtSelect.CommentCd equals tenMst.ItemCd
                               select new RecedenCmtSelectModel(tenMst.CmtSbt,
                                                               recedenCmtSelect.ItemCd,
                                                               recedenCmtSelect.CommentCd ?? string.Empty,
                                                               tenMst.Name ?? string.Empty,
                                                               recedenCmtSelect.ItemNo,
                                                               recedenCmtSelect.EdaNo,
                                                               tenMst.Name ?? string.Empty,
                                                               recedenCmtSelect.SortNo,
                                                               recedenCmtSelect.CondKbn,
                                                               ConvertTenMstToModel(tenMst)
                                                               )).ToList();

            foreach (var inputCodeItem in result)
            {
                var listCommentWithCode = listComment.Where(item =>
                    item.ItemCd == inputCodeItem.ItemCd || item.ItemCd == inputCodeItem.SanteiItemCd)
                    .OrderBy(item => item.ItemNo)
                    .ToList();

                if (listCommentWithCode.Count <= 0)
                {
                    inputCodeItem.SetRecedenCmtSelectModel(new());
                    continue;
                }

                if (!isRecalculation)
                {
                    var itemNo = listCommentWithCode[0].ItemNo;

                    listCommentWithCode.AddRange(listComment.Where(item =>
                            item.ItemCd == "199999999" && item.ItemNo == itemNo)
                        .ToList());
                }

                listCommentWithCode = listCommentWithCode.OrderBy(item => item.ItemNo)
                    .ThenBy(item => item.EdaNo)
                    .ThenBy(item => item.SortNo)
                    .ToList();

                inputCodeItem.SetRecedenCmtSelectModel(listCommentWithCode);
            }

            return result;
        }

        private static TenItemModel ConvertTenMstToModel(TenMst tenMst)
        {
            return new TenItemModel(
                        tenMst?.HpId ?? 0,
                        tenMst?.ItemCd ?? string.Empty,
                        tenMst?.RousaiKbn ?? 0,
                        tenMst?.KanaName1 ?? string.Empty,
                        tenMst?.Name ?? string.Empty,
                        tenMst?.KohatuKbn ?? 0,
                        tenMst?.MadokuKbn ?? 0,
                        tenMst?.KouseisinKbn ?? 0,
                        tenMst?.OdrUnitName ?? string.Empty,
                        tenMst?.EndDate ?? 0,
                        tenMst?.DrugKbn ?? 0,
                        tenMst?.MasterSbt ?? string.Empty,
                        tenMst?.BuiKbn ?? 0,
                        tenMst?.IsAdopted ?? 0,
                        tenMst?.Ten ?? 0,
                        tenMst?.TenId ?? 0,
                        string.Empty,
                        string.Empty,
                        tenMst?.CmtCol1 ?? 0,
                        tenMst?.IpnNameCd ?? string.Empty,
                        tenMst?.SinKouiKbn ?? 0,
                        tenMst?.YjCd ?? string.Empty,
                        tenMst?.CnvUnitName ?? string.Empty,
                        tenMst?.StartDate ?? 0,
                        tenMst?.YohoKbn ?? 0,
                        tenMst?.CmtColKeta1 ?? 0,
                        tenMst?.CmtColKeta2 ?? 0,
                        tenMst?.CmtColKeta3 ?? 0,
                        tenMst?.CmtColKeta4 ?? 0,
                        tenMst?.CmtCol2 ?? 0,
                        tenMst?.CmtCol3 ?? 0,
                        tenMst?.CmtCol4 ?? 0,
                        tenMst?.IpnNameCd ?? string.Empty,
                        tenMst?.MinAge ?? string.Empty,
                        tenMst?.MaxAge ?? string.Empty,
                        tenMst?.SanteiItemCd ?? string.Empty,
                        0,
                        0,
                        tenMst?.DefaultVal ?? 0,
                        tenMst?.Kokuji1 ?? string.Empty,
                        tenMst?.Kokuji2 ?? string.Empty,
                        string.Empty,
                        0,
                        0,
                        true
                        );
        }

        public List<string> GetListReceCmtItemCode(int hpId, long ptId, int sinYm, int hokenId)
        {
            return NoTrackingDataContext.ReceCmts
                                    .Where(p => p.HpId == hpId &&
                                                p.SinYm == sinYm &&
                                                p.PtId == ptId &&
                                                p.HokenId == hokenId &&
                                                p.IsDeleted == DeleteTypes.None)
                                    .Select(r => r.ItemCd ?? string.Empty)
                                    .Distinct()
                                    .ToList();
        }

        public List<CalcLogModel> GetAddtionItems(int hpId, long ptId, int sinYm, int hokenId)
        {
            return NoTrackingDataContext.CalcLogs.Where(p => p.HpId == hpId
                                                                 && p.DelSbt == 13
                                                                 && p.PtId == ptId
                                                                 && p.SinDate / 100 == sinYm
                                                                 && p.HokenId == hokenId)
                                                   .Select(x => new CalcLogModel(x.RaiinNo, x.LogSbt, x.Text ?? string.Empty))
                                                   .ToList();
        }

        public bool IsKantokuCdValid(int hpId, int hokenId, long ptId)
        {
            var hoken = NoTrackingDataContext.PtHokenInfs.FirstOrDefault(item => item.HpId == hpId
                                                                                        && item.IsDeleted == DeleteTypes.None
                                                                                        && item.PtId == ptId
                                                                                        && item.HokenId == hokenId);
            if (hoken == null) return false;
            var kantoku = NoTrackingDataContext.KantokuMsts.FirstOrDefault(item => item.RoudouCd == hoken.RousaiRoudouCd && item.KantokuCd == hoken.RousaiKantokuCd);
            return kantoku != null;
        }

        public bool ExistSyobyoKeikaData(int hpId, long ptId, int sinYm, int hokenId)
        {
            var syobyoKeika = NoTrackingDataContext.SyobyoKeikas.FirstOrDefault(p => p.HpId == hpId &&
                                                                                 p.IsDeleted == DeleteTypes.None &&
                                                                                 p.PtId == ptId &&
                                                                                 p.SinYm == sinYm &&
                                                                                 p.HokenId == hokenId);
            return syobyoKeika != null && !string.IsNullOrEmpty(syobyoKeika.Keika);
        }

        public List<ReceSeikyuModel> GetReceSeikyus(int hpId, List<long> ptIds, int seikyuYm)
        {
            List<ReceSeikyuModel> result = new();
            var ptInfs = NoTrackingDataContext.PtInfs.Where(p => p.HpId == hpId &&
                                                                 p.IsDelete == DeleteTypes.None &&
                                                                 (!ptIds.Any() || ptIds.Contains(p.PtId)))
                                                      .ToList();
            var receSeikyus = NoTrackingDataContext.ReceSeikyus.Where(p => p.HpId == hpId &&
                                                                                           p.SeikyuYm == seikyuYm &&
                                                                                           p.IsDeleted == DeleteTypes.None &&
                                                                                           (!ptIds.Any() || ptIds.Contains(p.PtId)))
                                                               .ToList();
            var query = from receSeikyu in receSeikyus
                        join ptInf in ptInfs
                        on receSeikyu.PtId equals ptInf.PtId
                        select new
                        {
                            ReceSeikyu = receSeikyu,
                            PtInf = ptInf
                        };
            foreach (var entity in query)
            {
                result.Add(ConvertToModel(entity.PtInf, entity.ReceSeikyu));
            }
            return result;
        }

        public List<TenItemModel> GetZaiganIsoItems(int hpId, int seikyuYm)
        {
            int firstDateOfMonth = seikyuYm * 100 + 1;
            int lastDateOfMonth = seikyuYm * 100 + 31;
            var tenMst = NoTrackingDataContext.TenMsts.Where(p => p.HpId == hpId &&
                                                                   p.StartDate <= lastDateOfMonth &&
                                                                   p.EndDate >= firstDateOfMonth &&
                                                                   p.CdKbn == "C" &&
                                                                   p.CdKbnno == 3 &&
                                                                   p.CdEdano == 0 &&
                                                                   (p.CdKouno == 1 || p.CdKouno == 2 || p.CdKouno == 4) &&
                                                                   p.IsDeleted == DeleteTypes.None)
                                                        .Select(x => ConvertTenMstToModel(x))
                                                        .ToList();
            return tenMst;
        }

        public List<SinKouiDetailModel> GetKouiDetailToCheckSantei(int hpId, List<long> ptIds, int seikyuYm, List<string> zaiganIsoItemCds, bool isCheckPartOfNextMonth)
        {
            var result = new List<SinKouiDetailModel>();
            int firstDateOfWeek = 0;
            int lastDateOfWeek = 0;
            int checkMonth = 0;

            if (isCheckPartOfNextMonth)
            {
                var firstDateOfNextMonth = new DateTime(seikyuYm / 100, seikyuYm % 100, 1).AddMonths(1);
                checkMonth = firstDateOfNextMonth.ToString("yyyyMM").AsInteger();
                firstDateOfWeek = CIUtil.DateTimeToInt(firstDateOfNextMonth);
                lastDateOfWeek = CIUtil.DateTimeToInt(firstDateOfNextMonth.AddDays(6 - (int)firstDateOfNextMonth.DayOfWeek));
            }
            else
            {
                var endDateOfLastMonth = new DateTime(seikyuYm / 100, seikyuYm % 100, 1).AddDays(-1);
                checkMonth = endDateOfLastMonth.ToString("yyyyMM").AsInteger();
                firstDateOfWeek = CIUtil.DateTimeToInt(endDateOfLastMonth.AddDays(-(int)endDateOfLastMonth.DayOfWeek));
                lastDateOfWeek = CIUtil.DateTimeToInt(endDateOfLastMonth);
            }


            var tenMsts = NoTrackingDataContext.TenMsts.Where(p => p.HpId == hpId && p.IsDeleted == DeleteTypes.None);
            var ptInfs = NoTrackingDataContext.PtInfs.Where(p => p.HpId == hpId &&
                                                                                       p.IsDelete == DeleteTypes.None &&
                                                                                       (!ptIds.Any() || ptIds.Contains(p.PtId)));

            var sinKouiCounts = NoTrackingDataContext.SinKouiCounts.Where(p => p.HpId == hpId &&
                                                                                               p.SinYm == checkMonth &&
                                                                                               p.SinDate >= firstDateOfWeek &&
                                                                                               p.SinDate <= lastDateOfWeek &&
                                                                                               (!ptIds.Any() || ptIds.Contains(p.PtId)));

            var sinKouiDetails = NoTrackingDataContext.SinKouiDetails.Where(p => p.HpId == hpId &&
                                                                                                 p.SinYm == checkMonth &&
                                                                                                 zaiganIsoItemCds.Contains(p.ItemCd ?? string.Empty) &&
                                                                                                 (!ptIds.Any() || ptIds.Contains(p.PtId)));

            var joinKouiCountWithDetailQuery = from sinKouiCount in sinKouiCounts
                                               join sinKouiDetail in sinKouiDetails
                                               on new { sinKouiCount.PtId, sinKouiCount.RpNo, sinKouiCount.SeqNo } equals new { sinKouiDetail.PtId, sinKouiDetail.RpNo, sinKouiDetail.SeqNo }
                                               select new
                                               {
                                                   SinKouiCount = sinKouiCount,
                                                   SinKouiDetail = sinKouiDetail
                                               };

            var joinTenMstQuery = from kouiCountDetail in joinKouiCountWithDetailQuery
                                  join tenMst in tenMsts
                                  on kouiCountDetail.SinKouiDetail.ItemCd equals tenMst.ItemCd
                                  where tenMst.StartDate <= kouiCountDetail.SinKouiCount.SinDate &&
                                  tenMst.EndDate >= kouiCountDetail.SinKouiCount.SinDate
                                  select new
                                  {
                                      kouiCountDetail.SinKouiDetail,
                                      TenMst = tenMst
                                  };

            var joinWithPtInf = from joinTenMst in joinTenMstQuery
                                join ptInf in ptInfs
                                on joinTenMst.SinKouiDetail.PtId equals ptInf.PtId
                                select new
                                {
                                    joinTenMst.SinKouiDetail,
                                    joinTenMst.TenMst,
                                    PtInf = ptInf
                                };

            foreach (var entity in joinWithPtInf)
            {
                result.Add(ConvertToModel(entity.PtInf, entity.TenMst, entity.SinKouiDetail));
            }
            return result;
        }

        public int GetSanteiStartDate(int hpId, long ptId, int seikyuYm)
        {
            var sinKouiCounts = NoTrackingDataContext.SinKouiCounts.Where(p => p.HpId == hpId &&
                                                                                               p.PtId == ptId &&
                                                                                               p.SinYm == seikyuYm);
            var sinKouiDetails = NoTrackingDataContext.SinKouiDetails.Where(p => p.HpId == hpId &&
                                                                                                 p.PtId == ptId &&
                                                                                                 p.SinYm == seikyuYm &&
                                                                                                 p.ItemCd == "X00013");
            var query = from sinKouiCount in sinKouiCounts
                        join sinKouiDetail in sinKouiDetails
                        on new { sinKouiCount.RpNo, sinKouiCount.SeqNo } equals new { sinKouiDetail.RpNo, sinKouiDetail.SeqNo }
                        select new
                        {
                            SinKouiCount = sinKouiCount
                        };
            if (query != null && query.Any())
            {
                var entity = query.First();
                return entity.SinKouiCount.SinDate;
            }
            return seikyuYm * 100 + 1;
        }

        public bool HasErrorWithSanteiByStartDate(int hpId, long ptId, int seikyuYm, int startDate, string itemCd)
        {
            var sinKouiCounts = NoTrackingDataContext.SinKouiCounts.Where(p => p.HpId == hpId &&
                                                                                               p.PtId == ptId &&
                                                                                               p.SinYm == seikyuYm &&
                                                                                               p.SinDate >= startDate);
            var sinKouiDetails = NoTrackingDataContext.SinKouiDetails.Where(p => p.HpId == hpId &&
                                                                                                 p.PtId == ptId &&
                                                                                                 p.SinYm == seikyuYm &&
                                                                                                 p.ItemCd == itemCd);
            var query = from sinKouiCount in sinKouiCounts
                        join sinKouiDetail in sinKouiDetails
                        on new { sinKouiCount.RpNo, sinKouiCount.SeqNo } equals new { sinKouiDetail.RpNo, sinKouiDetail.SeqNo }
                        select new
                        {
                            SinKouiCount = sinKouiCount
                        };
            return query.Count() == 0;
        }

        public bool HasErrorWithSanteiByEndDate(int hpId, long ptId, int seikyuYm, int endDate, string itemCd)
        {
            var sinKouiCounts = NoTrackingDataContext.SinKouiCounts.Where(p => p.HpId == hpId &&
                                                                                               p.PtId == ptId &&
                                                                                               p.SinYm == seikyuYm &&
                                                                                               p.SinDate <= endDate);
            var sinKouiDetails = NoTrackingDataContext.SinKouiDetails.Where(p => p.HpId == hpId &&
                                                                                                 p.PtId == ptId &&
                                                                                                 p.SinYm == seikyuYm &&
                                                                                                 p.ItemCd == itemCd);
            var query = from sinKouiCount in sinKouiCounts
                        join sinKouiDetail in sinKouiDetails
                        on new { sinKouiCount.RpNo, sinKouiCount.SeqNo } equals new { sinKouiDetail.RpNo, sinKouiDetail.SeqNo }
                        select new
                        {
                            SinKouiCount = sinKouiCount
                        };
            return query.Count() == 0;
        }

        public int GetSanteiEndDate(int hpId, long ptId, int seikyuYm)
        {
            var ptInf = NoTrackingDataContext.PtInfs.FirstOrDefault(p => p.HpId == hpId && p.PtId == ptId);
            if (ptInf != null && ptInf.DeathDate > 0 && ptInf.DeathDate < seikyuYm * 100 + 1)
            {
                return ptInf.DeathDate;
            }
            //get end date of current month
            return CIUtil.DateTimeToInt(new DateTime(seikyuYm / 100, seikyuYm % 100, 1).AddMonths(1).AddDays(-1));
        }

        public bool SaveChanged(int hpId, int userId, List<ReceCheckErrModel> receChecks)
        {
            foreach (var item in receChecks)
            {
                TrackingDataContext.Add(new ReceCheckErr()
                {
                    HpId = item.HpId,
                    PtId = item.PtId,
                    HokenId = item.HokenId,
                    SinYm = item.SinYm,
                    ErrCd = item.ErrCd,
                    SinDate = item.SinDate,
                    ACd = item.ACd,
                    BCd = item.BCd,
                    Message1 = item.Message1,
                    Message2 = item.Message2,
                    IsChecked = item.IsChecked,
                    CreateDate = CIUtil.GetJapanDateTimeNow(),
                    CreateMachine = string.Empty,
                    CreateId = userId,
                    UpdateDate = CIUtil.GetJapanDateTimeNow(),
                    UpdateMachine = string.Empty,
                    UpdateId = userId
                });
            }

            return TrackingDataContext.SaveChanges() > 0;
        }

        public bool DeleteReceiptInfEdit(int hpId, int userId, int seikyuYm, long ptId, int sinYm, int hokenId)
        {
            var listReceInfEdit = TrackingDataContext.ReceInfEdits.Where(item => item.HpId == hpId
                                                                                && item.SeikyuYm == seikyuYm
                                                                                && item.PtId == ptId
                                                                                && item.SinYm == sinYm
                                                                                && item.HokenId == hokenId
                                                                                && item.IsDeleted == 0)
                                                                   .ToList();

            if (listReceInfEdit.Any())
            {
                foreach (var receInfEdit in listReceInfEdit)
                {
                    receInfEdit.IsDeleted = 1;
                    receInfEdit.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    receInfEdit.UpdateId = userId;
                }

                return TrackingDataContext.SaveChanges() > 0;
            }

            return true;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
