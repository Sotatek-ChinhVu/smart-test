using Domain.CalculationInf;
using Domain.Constant;
using Domain.Models.CalculationInf;
using Domain.Models.Diseases;
using Domain.Models.Medical;
using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.Receipt;
using Domain.Models.Receipt.Recalculation;
using Domain.Models.SystemConf;
using Domain.Models.TodayOdr;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Infrastructure.Services;

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

        public void CheckErrorInMonth(int hpId, int userId, string userName, int seikyuYm, List<long> ptIds)
        {
            try
            {
                IsStopCalc = false;
                var AllCheckCount = GetCountReceInfs(hpId, ptIds, seikyuYm);
                var CheckedCount = 0;

                var receCheckOpts = GetReceCheckOpts(hpId);
                var receInfModels = GetReceInfModels(hpId, ptIds, seikyuYm);

                var newReceCheckErrs = new List<ReceCheckErr>();

                foreach (var receInfModel in receInfModels)
                {
                    if (IsStopCalc) break;
                    if (CancellationToken.IsCancellationRequested) return;
                    var oldReceCheckErrs = ClearReceCmtErr(hpId, receInfModel.PtId, receInfModel.HokenId, receInfModel.SinYm);
                    var sinKouiCounts = GetSinKouiCounts(hpId, receInfModel.PtId, receInfModel.SinYm, receInfModel.HokenId);

                    CheckHoken(hpId, userId, userName, receInfModel, receCheckOpts, sinKouiCounts, oldReceCheckErrs, newReceCheckErrs);
                    CheckByomei(hpId, userId, userName, receInfModel, receCheckOpts, sinKouiCounts, oldReceCheckErrs, newReceCheckErrs);
                    CheckOrder(hpId, userId, userName, receInfModel, receCheckOpts, sinKouiCounts, oldReceCheckErrs, newReceCheckErrs);
                    CheckRosai(receInfModel);
                    CheckAftercare(receInfModel);

                    CheckedCount++;
                }
                _commandHandler.SaveChanged();
                PrintReceCheck(seikyuYm, ptIds);
            }
            finally
            {
                Log.WriteLogEnd(ModuleNameConst.EmrCommonView, this, nameof(CheckErrorInMonth), ICDebugConf.logLevel);
            }
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
                result.Add(new PtDiseaseModel(entity.PtByomei, entity.ByomeiMst));
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

            var receStates = NoTrackingDataContext.ReceStatuses.Where(p => p.HpId == hpId && p.SeikyuYm == sinYM && (ptIds.Count > 0 ? ptIds.Contains(p.PtId) : true));

            var receInfs = NoTrackingDataContext.ReceInfs.Where(p => p.HpId == hpId && p.SeikyuYm == sinYM && (ptIds.Count > 0 ? ptIds.Contains(p.PtId) : true));

            var ptHokenInfs = NoTrackingDataContext.PtHokenInfs.Where(p => p.HpId == hpId && p.IsDeleted == DeleteTypes.None && (ptIds.Count > 0 ? ptIds.Contains(p.PtId) : true));

            var ptKohiInfs = NoTrackingDataContext.PtKohis.Where(p => p.HpId == hpId && p.IsDeleted == DeleteTypes.None && (ptIds.Count > 0 ? ptIds.Contains(p.PtId) : true));

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
                                                                                             (ptIds.Count > 0 ? ptIds.Contains(p.PtID) : true));

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
                receInfModels.Add(new ReceInfModel(
                    ConvertToReceInfModel(entity.ReceInf)
                    ));
            }
            return receInfModels;
        }

        public ReceInfModel ConvertToReceInfModel(ReceInf receInf)
        {
            return new ReceInfModel(
                       receInf.HpId,
                       receInf.SeikyuYm,
                       receInf.PtId,
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
                       receInf?.Kohi4ReceFutan ?? -1
                );
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
                                      PtId = sinKouiCountJoinDetail.SinKouiCount.PtId,
                                      SinDate = sinKouiCountJoinDetail.SinKouiCount.SinDate,
                                      RaiinNo = sinKouiCountJoinDetail.SinKouiCount.RaiinNo,
                                      SinKouiCount = sinKouiCountJoinDetail.SinKouiCount,
                                      SinKouiDetail = sinKouiCountJoinDetail.SinKouiDetail,
                                      sinKouiCountJoinDetail.ptHokenPattern,
                                      TenMst = tempTenMstList.OrderByDescending(p => p.StartDate).FirstOrDefault(p => p.StartDate <= sinKouiCountJoinDetail.SinKouiCount.SinDate)
                                  };

            var groupKeys = joinTenMstQuery.GroupBy(p => new { p.PtId, p.SinDate, p.RaiinNo })
                                           .Select(p => p.FirstOrDefault());

            foreach (var groupKey in groupKeys)
            {
                var entities = joinTenMstQuery.Where(p => p.PtId == groupKey.PtId && p.SinDate == groupKey.SinDate && p.RaiinNo == groupKey.RaiinNo);
                var sinKouiDetailModels = new List<SinKouiDetailModel>();
                foreach (var entity in entities)
                {
                    sinKouiDetailModels.Add(new SinKouiDetailModel(entity.TenMst, entity.SinKouiDetail));
                }
                result.Add(new SinKouiCountModel(entities.Select(p => p.ptHokenPattern).Distinct().ToList(), groupKey.SinKouiCount, sinKouiDetailModels));
            }
            return result;
        }

        public List<ReceCheckErr> ClearReceCmtErr(int hpId, long ptId, int hokenId, int sinYm)
        {
            var oldReceCheckErrs = NoTrackingDataContext.ReceCheckErrs
                                                        .Where(p => p.HpId == hpId &&
                                                                    p.SinYm == sinYm &&
                                                                    p.PtId == ptId &&
                                                                    p.HokenId == hokenId)
                                                        .ToList();
            TrackingDataContext.ReceCheckErrs.RemoveRange(oldReceCheckErrs);

            return oldReceCheckErrs;
        }

        public void InsertReceCmtErr(int hpId, int userId, string userName, List<ReceCheckErrModel> oldReceCheckErrs, List<ReceCheckErrModel> newReceCheckErrs, ReceInfModel receInfModel, string errCd, string errMsg1, string errMsg2 = "", string aCd = " ", string bCd = " ", int sinDate = 0)
        {
            if (!string.IsNullOrEmpty(errMsg1) && errMsg1.Length > 100)
            {
                errMsg1 = CIUtil.Copy(errMsg1, 1, 99) + "…";
            }
            if (!string.IsNullOrEmpty(errMsg2) && errMsg2.Length > 100)
            {
                errMsg2 = CIUtil.Copy(errMsg2, 1, 99) + "…";
            }

            var existNewReceCheckErr = newReceCheckErrs.FirstOrDefault(p => p.HpId == hpId &&
                                           p.PtId == receInfModel.PtId &&
                                           p.SinYm == receInfModel.SinYm &&
                                           p.SinDate == sinDate &&
                                           p.HokenId == receInfModel.HokenId &&
                                           p.ErrCd == errCd &&
                                           p.ACd == aCd &&
                                           p.BCd == bCd);

            if (existNewReceCheckErr != null)
            {
                if (errCd == ReceErrCdConst.SanteiCountCheckErrCd)
                {
                    existNewReceCheckErr.ChangeMessage1(errMsg1);
                    existNewReceCheckErr.ChangeMessage2(errMsg2);
                }
                return;
            }

            var newReceCheckErr = new ReceCheckErrModel(hpId, receInfModel.PtId, receInfModel.SinYm, receInfModel.HokenId, errCd, sinDate, aCd, bCd, errMsg1, errMsg2, 0);

            var existedReceCheckErr = oldReceCheckErrs.FirstOrDefault(p => p.HpId == newReceCheckErr.HpId &&
                                                                            p.PtId == newReceCheckErr.PtId &&
                                                                            p.SinYm == newReceCheckErr.SinYm &&
                                                                            p.SinDate == newReceCheckErr.SinDate &&
                                                                            p.HokenId == newReceCheckErr.HokenId &&
                                                                            p.ErrCd == newReceCheckErr.ErrCd &&
                                                                            p.ACd == newReceCheckErr.ACd &&
                                                                            p.BCd == newReceCheckErr.BCd);
            if (existedReceCheckErr != null)
            {
                newReceCheckErr.ChangeIsChecked(existedReceCheckErr.IsChecked);
            }
            newReceCheckErrs.Add(newReceCheckErr);
        }

        public List<OrdInfDetailModel> GetOdrInfsBySinDate(int hpId, long ptId, int sinDate, int hokenId)
        {
            var result = new List<OrdInfDetailModel>();

            var hokenPids = NoTrackingDataContext.PtHokenPatterns.Where(p => p.HpId == hpId &&
                                                                                              p.PtId == ptId &&
                                                                                              p.HokenId == hokenId &&
                                                                                              p.IsDeleted == DeleteTypes.None)
                                                                        .Select(p => p.HokenPid).ToList();

            var tenMstQuery = NoTrackingDataContext.TenMsts.Where(p => p.HpId == hpId);

            var odrInfDetails = NoTrackingDataContext.OdrInfDetails.Where(odrDetail => odrDetail.HpId == hpId &&
                                                                                       odrDetail.PtId == ptId &&
                                                                                       odrDetail.SinDate == sinDate);

            var odrInfs = NoTrackingDataContext.OdrInfs.Where(odr => odr.HpId == hpId &&
                                                                     odr.PtId == ptId &&
                                                                     odr.SinDate == sinDate &&
                                                                     odr.SanteiKbn == 0 &&
                                                                     hokenPids.Contains(odr.HokenPid) &&
                                                                     odr.IsDeleted == DeleteTypes.None &&
                                                                     odr.OdrKouiKbn != 10);

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
                result.AddRange(odrInfJoinDetail.OdrInfDetails.Select(p => new OrdInfDetailModel(p)));
            }
            return result;
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

            var detailJoinTenMstQuery = from odrDetail in odrInfDetails
                                        join tenMst in tenMstQuery on odrDetail.ItemCd equals tenMst.ItemCd into tempTenMsts
                                        select new
                                        {
                                            OdrDetail = odrDetail,
                                            TenMsts = tempTenMsts
                                        };
            var odrInfs = NoTrackingDataContext.OdrInfs.Where(odr => odr.HpId == hpId &&
                                                                     odr.PtId == ptId &&
                                                                     odr.SinDate / 100 == sinYm &&
                                                                     hokenPIds.Contains(odr.HokenPid) &&
                                                                     odr.IsDeleted == DeleteTypes.None &&
                                                                     odr.OdrKouiKbn != 10);
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
                var OrdInfDetailModels = new List<OrdInfDetailModel>();
                foreach (var odrInfDetail in odrInfJoinDetail.OdrInfDetails)
                {
                    OrdInfDetailModels.Add(new OrdInfDetailModel(odrInfDetail.OdrDetail, odrInfDetail.TenMsts.ToList()));
                }
                result.Add(new OrdInfModel(odrInfJoinDetail.OdrInf, OrdInfDetailModels));
            }

            return result;
        }

        public bool ValidateByomeiReflectOdrSite(string buiOdr, string byomeiName, int LrKbn, int BothKbn)
        {
            string GetDirection(string name)
            {
                string str = name.Length >= 2 ? name.Substring(0, 2) : name;
                if (str.Contains(BOTH))
                {
                    return BOTH;
                }
                else if (str == $"{LEFT}{RIGHT}" || str == $"{RIGHT}{LEFT}")
                {
                    return str;
                }
                else if (str.Contains(LEFT))
                {
                    return LEFT;
                }
                else if (str.Contains(RIGHT))
                {
                    return RIGHT;
                }
                return "";
            }
            if (LrKbn == 0 && BothKbn == 0)
            {
                return true;
            }
            else if ((LrKbn == 1 && BothKbn == 1) || (LrKbn == 0 && BothKbn == 1))
            {
                string buiOdrDirection = GetDirection(buiOdr);
                string byomeiNameDirection = GetDirection(byomeiName);
                // Convert names to the left-right direction if they contain 両 character or right-left direction.
                string buiOdrLeftRight = buiOdrDirection.Replace($"{BOTH}", $"{LEFT}{RIGHT}").Replace($"{RIGHT}{LEFT}", $"{LEFT}{RIGHT}");
                string byomeiNameLeftRight = byomeiNameDirection.Replace($"{BOTH}", $"{LEFT}{RIGHT}").Replace($"{RIGHT}{LEFT}", $"{LEFT}{RIGHT}");
                return byomeiNameLeftRight.Contains(buiOdrLeftRight);
            }
            else if (LrKbn == 1 && BothKbn == 0)
            {
                string buiOdrDirection = GetDirection(buiOdr);
                string byomeiNameDirection = GetDirection(byomeiName);
                // Convert names to the left-right direction if they contain 両 character or right-left direction.
                string buiOdrLeftRight = buiOdrDirection.Replace($"{BOTH}", $"{LEFT}{RIGHT}").Replace($"{RIGHT}{LEFT}", $"{LEFT}{RIGHT}");
                string byomeiNameLeftRight = byomeiNameDirection.Replace($"{BOTH}", $"{LEFT}{RIGHT}").Replace($"{RIGHT}{LEFT}", $"{LEFT}{RIGHT}");
                if (byomeiNameLeftRight.Contains($"{LEFT}{RIGHT}") && (buiOdrLeftRight == LEFT || buiOdrLeftRight == RIGHT))
                {
                    return false;
                }
                return byomeiNameLeftRight.Contains(buiOdrLeftRight);
            }
            return true;
        }

        public string OdrKouiKbnToString(int odrKouiKbn)
        {
            if (30 <= odrKouiKbn && odrKouiKbn <= 39)
            {
                return "注射";
            }
            else if (40 <= odrKouiKbn && odrKouiKbn <= 49)
            {
                return "処置";
            }
            else if (50 <= odrKouiKbn && odrKouiKbn <= 59)
            {
                return "手術";
            }
            else if (60 <= odrKouiKbn && odrKouiKbn <= 69)
            {
                return "検査";
            }
            else if (70 <= odrKouiKbn && odrKouiKbn <= 79)
            {
                return "画像";
            }
            else if (80 <= odrKouiKbn && odrKouiKbn <= 89)
            {
                return "その他";
            }
            return "";
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
                                                            x.OdrBui,
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

        public List<DensiSanteiKaisuModel> FindDensiSanteiKaisuList(int sinDate, string itemCd)
        {
            List<int> unitCds = new List<int> { 53, 121, 131, 138, 141, 142, 143, 144, 145, 146, 147, 148, 997, 998, 999 };

            var entities = dbService.DensiSanteiKaisuRepository.FindListQueryableNoTrack((x) =>
                    x.HpId == Session.HospitalID &&
                    x.ItemCd == itemCd &&
                    x.StartDate <= sinDate &&
                    x.EndDate >= sinDate &&
                    x.IsInvalid == 0 &&
                    unitCds.Contains(x.UnitCd)
                ).ToList();

            List<DensiSanteiKaisuModel> results = new List<DensiSanteiKaisuModel>();
            entities?.ForEach(entity =>
            {
                results.Add(new DensiSanteiKaisuModel(entity));
            });

            return results;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
