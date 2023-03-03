using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.TodayOdr;
using UseCase.MedicalExamination.ChangeAfterAutoCheckOrder;
using UseCase.MedicalExamination.UpsertTodayOrd;

namespace Interactor.MedicalExamination
{
    public class ChangeAfterAutoCheckOrderInteractor : IChangeAfterAutoCheckOrderInputPort
    {
        private readonly ITodayOdrRepository _todayOdrRepository;

        public ChangeAfterAutoCheckOrderInteractor(ITodayOdrRepository todayOdrRepository)
        {
            _todayOdrRepository = todayOdrRepository;
        }

        public ChangeAfterAutoCheckOrderOutputData Handle(ChangeAfterAutoCheckOrderInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new ChangeAfterAutoCheckOrderOutputData(ChangeAfterAutoCheckOrderStatus.InvalidHpId, new());
                }
                if (inputData.PtId <= 0)
                {
                    return new ChangeAfterAutoCheckOrderOutputData(ChangeAfterAutoCheckOrderStatus.InvalidPtId, new());
                }
                if (inputData.SinDate <= 0)
                {
                    return new ChangeAfterAutoCheckOrderOutputData(ChangeAfterAutoCheckOrderStatus.InvalidSinDate, new());
                }
                if (inputData.RaiinNo <= 0)
                {
                    return new ChangeAfterAutoCheckOrderOutputData(ChangeAfterAutoCheckOrderStatus.InvalidRaiinNo, new());
                }
                if (inputData.UserId <= 0)
                {
                    return new ChangeAfterAutoCheckOrderOutputData(ChangeAfterAutoCheckOrderStatus.InvalidUserId, new());
                }
                if (inputData.OdrInfs.Count == 0)
                {
                    return new ChangeAfterAutoCheckOrderOutputData(ChangeAfterAutoCheckOrderStatus.InvalidOdrInfs, new());
                }

                var result = _todayOdrRepository.ChangeAfterAutoCheckOrder(inputData.HpId, inputData.SinDate, inputData.UserId, inputData.RaiinNo, inputData.PtId, inputData.OdrInfs.Select(item =>
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
                    )).ToList(),
                    inputData.TargetItems.Select(t => new Tuple<int, string, int, int, TenItemModel, double>(t.Type, t.Message, t.OdrInfPosition, t.OdrInfDetailPosition, t.TenItemMst, t.Suryo)).ToList()
                    );

                return new ChangeAfterAutoCheckOrderOutputData(ChangeAfterAutoCheckOrderStatus.Successed, result.Select(o => new OdrInfItemInputData(
                        o.HpId,
                        o.RaiinNo,
                        o.RpNo,
                        o.RpEdaNo,
                        o.PtId,
                        o.SinDate,
                        o.HokenPid,
                        o.OdrKouiKbn,
                        o.RpName,
                        o.InoutKbn,
                        o.SikyuKbn,
                        o.SyohoSbt,
                        o.SanteiKbn,
                        o.TosekiKbn,
                        o.DaysCnt,
                        o.SortNo,
                        o.Id,
                        o.OrdInfDetails.Select(
                                od => new OdrInfDetailItemInputData(
                                        od.HpId,
                                        od.RaiinNo,
                                        od.RpNo,
                                        od.RpEdaNo,
                                        od.RowNo,
                                        od.PtId,
                                        od.SinDate,
                                        od.SinKouiKbn,
                                        od.ItemCd,
                                        od.ItemName,
                                        od.Suryo,
                                        od.UnitName,
                                        od.UnitSbt,
                                        od.TermVal,
                                        od.KohatuKbn,
                                        od.SyohoKbn,
                                        od.SyohoLimitKbn,
                                        od.DrugKbn,
                                        od.YohoKbn,
                                        od.Kokuji1,
                                        od.Kokuji2,
                                        od.IsNodspRece,
                                        od.IpnCd,
                                        od.IpnName,
                                        od.JissiKbn,
                                        od.JissiDate,
                                        od.JissiId,
                                        od.JissiMachine,
                                        od.ReqCd,
                                        od.Bunkatu,
                                        od.CmtName,
                                        od.CmtOpt,
                                        od.FontColor,
                                        od.CommentNewline
                                    )
                            ).ToList(),
                        o.IsDeleted
                    )).ToList()
                    );
            }
            finally
            {
                _todayOdrRepository.ReleaseResource();
            }
        }
    }
}
