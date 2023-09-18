using Domain.Models.PatientInfor;
using Reporting.Statistics.Sta9000.DB;
using UseCase.PatientManagement;

namespace Interactor.PatientManagement
{
    public class SearchPtInfsInteractor : ISearchPtInfsInputPort
    {
        private readonly ICoSta9000Finder _coSta9000Finder;

        public SearchPtInfsInteractor(ICoSta9000Finder coSta9000Finder)
        {
            _coSta9000Finder = coSta9000Finder;
        }

        public SearchPtInfsOutputData Handle(SearchPtInfsInputData inputData)
        {
            try
            {
                var pageCount = inputData.PageCount;
                var pageIndex = inputData.PageIndex;
                var hpId = inputData.HpId;
                if (pageIndex <= 0)
                {
                    return new SearchPtInfsOutputData(0, new(), SearchPtInfsStatus.InvalidPageIndex);
                }
                else if (pageCount < 0)
                {
                    return new SearchPtInfsOutputData(0, new(), SearchPtInfsStatus.InvalidPageCount);
                }

                var coPtInfs = _coSta9000Finder.GetPtInfs(inputData.HpId, inputData.CoSta9000PtConf, inputData.CoSta9000HokenConf, inputData.CoSta9000ByomeiConf,
                                                             inputData.CoSta9000RaiinConf, inputData.CoSta9000SinConf, inputData.CoSta9000KarteConf, inputData.CoSta9000KensaConf);

                var totalCount = coPtInfs.Count();
                if (inputData.OutputOrder == 0)
                {
                    coPtInfs = coPtInfs.OrderBy(u => u.PtNum).Skip((pageIndex - 1) * pageCount).Take(pageCount).ToList();
                }
                else
                {
                    coPtInfs = coPtInfs.OrderBy(u => u.KanaName).ThenBy(u => u.PtNum).Skip((pageIndex - 1) * pageCount).Take(pageCount).ToList();
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
            finally
            {
                _coSta9000Finder.ReleaseResource();
            }
        }
    }
}
