using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.TodayOdr;
using UseCase.MedicalExamination.GetValidGairaiRiha;

namespace Interactor.MedicalExamination
{
    public class GetValidGairaiRihaInteractor : IGetValidGairaiRihaInputPort
    {
        private readonly ITodayOdrRepository _todayOdrRepository;

        public GetValidGairaiRihaInteractor(ITodayOdrRepository todayOdrRepository)
        {
            _todayOdrRepository = todayOdrRepository;
        }

        public GetValidGairaiRihaOutputData Handle(GetValidGairaiRihaInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new GetValidGairaiRihaOutputData(0, string.Empty, 0, string.Empty, GetValidGairaiRihaStatus.InvalidHpId);
                }
                if (inputData.PtId <= 0)
                {
                    return new GetValidGairaiRihaOutputData(0, string.Empty, 0, string.Empty, GetValidGairaiRihaStatus.InvalidPtId);
                }
                if (inputData.RaiinNo <= 0)
                {
                    return new GetValidGairaiRihaOutputData(0, string.Empty, 0, string.Empty, GetValidGairaiRihaStatus.InvalidRaiinNo);
                }
                if (inputData.SinDate <= 0)
                {
                    return new GetValidGairaiRihaOutputData(0, string.Empty, 0, string.Empty, GetValidGairaiRihaStatus.InvalidSinDate);
                }
                if (inputData.SyosaiKbn <= 0)
                {
                    return new GetValidGairaiRihaOutputData(0, string.Empty, 0, string.Empty, GetValidGairaiRihaStatus.InvalidSyosaiKbn);
                }

                var check = _todayOdrRepository.GetValidGairaiRiha(
                        inputData.HpId,
                        inputData.PtId,
                        inputData.RaiinNo,
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
                        "",
                        false
                    )).ToList());

                return new GetValidGairaiRihaOutputData(check.type, check.itemName, check.lastDaySanteiRiha, check.rihaItemName, GetValidGairaiRihaStatus.Successed);
            }
            catch
            {
                return new GetValidGairaiRihaOutputData(0, string.Empty, 0, string.Empty, GetValidGairaiRihaStatus.Failed);
            }
            finally
            {
                _todayOdrRepository.ReleaseResource();
            }
        }
    }
}
