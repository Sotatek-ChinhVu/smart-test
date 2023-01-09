using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.TodayOdr;
using UseCase.MedicalExamination.GetCheckDisease;

namespace Interactor.MedicalExamination
{
    public class GetCheckDiseaseInteractor : IGetCheckDiseaseInputPort
    {
        private readonly ITodayOdrRepository _todayOdrRepository;
        public GetCheckDiseaseInteractor(ITodayOdrRepository todayOdrRepository)
        {
            _todayOdrRepository = todayOdrRepository;
        }

        public GetCheckDiseaseOutputData Handle(GetCheckDiseaseInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new GetCheckDiseaseOutputData(new(), GetCheckDiseaseStatus.InvalidHpId);
                }
                if (inputData.SinDate <= 0)
                {
                    return new GetCheckDiseaseOutputData(new(), GetCheckDiseaseStatus.InvalidSinDate);
                }
                if (inputData.TodayOdrs.Count == 0)
                {
                    return new GetCheckDiseaseOutputData(new(), GetCheckDiseaseStatus.InvalidDrugOrByomei);
                }

                var result = _todayOdrRepository.GetCheckDiseases(inputData.HpId, inputData.SinDate, inputData.TodayByomeis, inputData.TodayOdrs.Select(
                    i => new OrdInfModel(
                             i.HpId,
                             i.RaiinNo,
                             i.RpNo,
                             i.RpEdaNo,
                             i.PtId,
                             i.SinDate,
                             i.HokenPid,
                             i.OdrKouiKbn,
                             i.RpName,
                             i.InoutKbn,
                             i.SikyuKbn,
                             i.SyohoSbt,
                             i.SanteiKbn,
                             i.TosekiKbn,
                             i.DaysCnt,
                             i.SortNo,
                             i.IsDeleted,
                             i.Id,
                             i.OdrDetails.Select(
                                odr => new OrdInfDetailModel(
                                    odr.HpId,
                                    odr.RaiinNo,
                                    odr.RpNo,
                                    odr.RpEdaNo,
                                    odr.RowNo,
                                    odr.PtId,
                                    odr.SinDate,
                                    odr.SinKouiKbn,
                                    odr.ItemCd,
                                    odr.ItemName,
                                    odr.Suryo,
                                    odr.UnitName,
                                    odr.UnitSbt,
                                    odr.TermVal,
                                    odr.KohatuKbn,
                                    odr.SyohoKbn,
                                    odr.SyohoLimitKbn,
                                    odr.DrugKbn,
                                    odr.YohoKbn,
                                    odr.Kokuji1,
                                    odr.Kokuji2,
                                    odr.IsNodspRece,
                                    odr.IpnCd,
                                    odr.IpnName,
                                    odr.JissiKbn,
                                    odr.JissiDate,
                                    odr.JissiId,
                                    odr.JissiMachine,
                                    odr.ReqCd,
                                    odr.Bunkatu,
                                    odr.CmtName,
                                    odr.CmtOpt,
                                    odr.FontColor,
                                    odr.CommentNewline,
                                    string.Empty,
                                    0,
                                    0,
                                    false,
                                    0,
                                    0,
                                    0,
                                    0,
                                    0,
                                    0,
                                    0,
                                    0,
                                    string.Empty,
                                    new List<YohoSetMstModel>(),
                                    0,
                                    0,
                                    "",
                                    "",
                                    "",
                                    ""
                                   )
                             ).ToList(),
                             DateTime.MinValue,
                             0,
                             string.Empty,
                             DateTime.MinValue,
                             0,
                             string.Empty
                          )
                        ).ToList()
                    );

                if (result.Count == 0)
                {
                    return new GetCheckDiseaseOutputData(new(), GetCheckDiseaseStatus.NoData);
                }

                return new GetCheckDiseaseOutputData(
                       result.Select(r => new GetCheckDiseaseItemOutputData(r.Item1, r.Item2, r.Item3.Select(r3 =>
                       new CheckedDiseaseItem(r3.SikkanCd, r3.NanByoCd, r3.Byomei, r3.OdrItemNo, new PtDiseaseItem(r3.PtDiseaseModel), new ByomeiItem(r3.ByomeiMst), r3.IsAdopted)).ToList())).ToList(), GetCheckDiseaseStatus.Successed);
            }
            catch
            {
                return new GetCheckDiseaseOutputData(new(), GetCheckDiseaseStatus.Failed);
            }
            finally
            {
                _todayOdrRepository.ReleaseResource();
            }
        }
    }
}
