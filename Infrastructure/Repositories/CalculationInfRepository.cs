using Domain.CalculationInf;
using Domain.Models.CalculationInf;
using Domain.Models.Medical;
using Domain.Models.Receipt;
using Domain.Models.Receipt.Recalculation;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class CalculationInfRepository : RepositoryBase, ICalculationInfRepository
    {
        public CalculationInfRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
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

        public int GetCountReceInfs(int hpId, List<long> ptIds, int sinYm)
        {
            return NoTrackingDataContext.ReceInfs.Where(p => p.HpId == hpId && p.SeikyuYm == sinYm && (ptIds.Count == 0 || ptIds.Contains(p.PtId))).Count();
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

        private ReceCheckOptModel GetDefaultReceCheckOpt(string errCd)
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
                    **************************************************************************************************
                    ));
            }
            return receInfModels;
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

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
