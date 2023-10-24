using Domain.Models.PatientInfor;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using Reporting.Statistics.Sta9000.DB;
using UseCase.PatientManagement.SearchPtInfs;

namespace Interactor.PatientManagement
{
    public class SearchPtInfsInteractor : ISearchPtInfsInputPort
    {
        private readonly ICoSta9000Finder _coSta9000Finder;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;

        public SearchPtInfsInteractor(ITenantProvider tenantProvider, ICoSta9000Finder coSta9000Finder)
        {
            _coSta9000Finder = coSta9000Finder;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public SearchPtInfsOutputData Handle(SearchPtInfsInputData inputData)
        {
            try
            {
                var hpId = inputData.HpId;

                var coPtInfs = _coSta9000Finder.GetPtInfs(inputData.HpId, inputData.CoSta9000PtConf, inputData.CoSta9000HokenConf, inputData.CoSta9000ByomeiConf,
                                                             inputData.CoSta9000RaiinConf, inputData.CoSta9000SinConf, inputData.CoSta9000KarteConf, inputData.CoSta9000KensaConf);

                var totalCount = coPtInfs.Count();
                if (inputData.OutputOrder == 0)
                {
                    coPtInfs = coPtInfs.OrderBy(u => u.PtNum).ToList();
                }
                else
                {
                    coPtInfs = coPtInfs.OrderBy(u => u.KanaName).ThenBy(u => u.PtNum).ToList();
                }

                var result = coPtInfs.Select(x => new PatientInforModel(hpId,
                                                                        x.PtId,
                                                                        x.PtNum,
                                                                        x.KanaName,
                                                                        x.PtName,
                                                                        x.PtInf.Sex,
                                                                        x.Birthday,
                                                                        x.IsDead,
                                                                        x.DeathDate,
                                                                        x.HomePost,
                                                                        x.HomeAddress1,
                                                                        x.HomeAddress2,
                                                                        x.Tel1,
                                                                        x.Tel2,
                                                                        x.Mail,
                                                                        x.Setainusi,
                                                                        x.Zokugara,
                                                                        x.Job,
                                                                        x.RenrakuName,
                                                                        x.RenrakuPost,
                                                                        x.RenrakuAddress1,
                                                                        x.RenrakuAddress2,
                                                                        x.RenrakuTel,
                                                                        x.RenrakuMemo,
                                                                        x.OfficeName,
                                                                        x.OfficePost,
                                                                        x.OfficeAddress1,
                                                                        x.OfficeAddress2,
                                                                        x.OfficeTel,
                                                                        x.OfficeMemo,
                                                                        x.IsRyosyuDetail,
                                                                        x.PrimaryDoctor,
                                                                        x.IsTester,
                                                                        x.FirstVisitDate,
                                                                        x.LastVisitDate
                                                                        ))
                                        .ToList();

                if (!result.Any()) return new SearchPtInfsOutputData(totalCount, new(), SearchPtInfsStatus.NoData);

                return new SearchPtInfsOutputData(totalCount, result, SearchPtInfsStatus.Successed);
            }
            catch (Exception ex)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
            finally
            {
                _coSta9000Finder.ReleaseResource();
                _loggingHandler.Dispose();
            }
        }
    }
}
