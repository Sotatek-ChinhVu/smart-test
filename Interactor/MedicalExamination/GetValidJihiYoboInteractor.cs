using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.TodayOdr;
using UseCase.MedicalExamination.GetValidJihiYobo;

namespace Interactor.MedicalExamination
{
    public class GetValidJihiYoboInteractor : IGetValidJihiYoboInputPort
    {
        private readonly ITodayOdrRepository _todayOdrRepository;

        public GetValidJihiYoboInteractor(ITodayOdrRepository todayOdrRepository)
        {
            _todayOdrRepository = todayOdrRepository;
        }

        public GetValidJihiYoboOutputData Handle(GetValidJihiYoboInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new GetValidJihiYoboOutputData(0, false, GetValidJihiYoboStatus.InvalidHpId);
                }
                if (inputData.SinDate <= 0)
                {
                    return new GetValidJihiYoboOutputData(0, false, GetValidJihiYoboStatus.InvalidSinDate);
                }
                if (inputData.SyosaiKbn <= 0)
                {
                    return new GetValidJihiYoboOutputData(0, false, GetValidJihiYoboStatus.InvalidSyosaiKbn);
                }

                var check = _todayOdrRepository.GetValidJihiYobo(
                        inputData.HpId,
                        inputData.SinDate,
                        inputData.SyosaiKbn,
                        inputData.AllOdrInf.Select(item =>
                    new OrdInfModel(
                        item.HpId,
                        item.RaiinNo,
                        item.RpNo,
                        item.RpEdaNo,
                        item.PtId,
                        item.SinDate,
                        item.HokenPid,
                        item.OdrKouiKbn,
                        item.RpName,
                        item.InoutKbn,
                        item.SikyuKbn,
                        item.SyohoSbt,
                        item.SanteiKbn,
                        item.TosekiKbn,
                        item.DaysCnt,
                        item.SortNo,
                        item.IsDeleted,
                        item.Id,
                        item.OdrDetails.Select(itemDetail => new OrdInfDetailModel(
                                itemDetail.HpId,
                                itemDetail.RaiinNo,
                                itemDetail.RpNo,
                                itemDetail.RpEdaNo,
                                itemDetail.RowNo,
                                itemDetail.PtId,
                                itemDetail.SinDate,
                                itemDetail.SinKouiKbn,
                                itemDetail.ItemCd,
                                itemDetail.ItemName,
                                itemDetail.Suryo,
                                itemDetail.UnitName,
                                itemDetail.UnitSbt,
                                itemDetail.TermVal,
                                itemDetail.KohatuKbn,
                                itemDetail.SyohoKbn,
                                itemDetail.SyohoLimitKbn,
                                itemDetail.DrugKbn,
                                itemDetail.YohoKbn,
                                itemDetail.Kokuji1,
                                itemDetail.Kokuji2,
                                itemDetail.IsNodspRece,
                                itemDetail.IpnCd,
                                string.Empty,
                                itemDetail.JissiKbn,
                                itemDetail.JissiDate,
                                itemDetail.JissiId,
                                itemDetail.JissiMachine,
                                itemDetail.ReqCd,
                                itemDetail.Bunkatu,
                                itemDetail.CmtName,
                                itemDetail.CmtOpt,
                                itemDetail.FontColor,
                                itemDetail.CommentNewline,
                                string.Empty,
                                item?.InoutKbn ?? 0,
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
                                "",
                                new List<YohoSetMstModel>(),
                                0,
                                0,
                                "",
                                "",
                                "",
                                ""
                        )).ToList(),
                        DateTime.MinValue,
                        0,
                        "",
                        DateTime.MinValue,
                        0,
                        ""
                    )).ToList());



                return new GetValidJihiYoboOutputData(check.systemSetting, check.isExistYoboItemOnly, GetValidJihiYoboStatus.Successed);
            }
            catch
            {
                return new GetValidJihiYoboOutputData(0, false, GetValidJihiYoboStatus.Failed);
            }
            finally
            {
                _todayOdrRepository.ReleaseResource();
            }
        }
    }
}
