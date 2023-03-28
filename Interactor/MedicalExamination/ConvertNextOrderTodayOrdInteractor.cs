using Domain.Models.MstItem;
using Domain.Models.TodayOdr;
using Interactor.NextOrder;
using UseCase.MedicalExamination.ConvertNextOrderToTodayOdr;
using UseCase.OrdInfs.GetListTrees;

namespace Interactor.MedicalExamination
{
    public class ConvertNextOrderTodayOrdInteractor : IConvertNextOrderToTodayOrdInputPort
    {
        private readonly ITodayOdrRepository _todayOdrRepository;
        private readonly IMstItemRepository _mstItemRepository;

        public ConvertNextOrderTodayOrdInteractor(ITodayOdrRepository todayOdrRepository, IMstItemRepository mstItemRepository)
        {
            _todayOdrRepository = todayOdrRepository;
            _mstItemRepository = mstItemRepository;
        }

        public ConvertNextOrderToTodayOrdOutputData Handle(ConvertNextOrderToTodayOrdInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new ConvertNextOrderToTodayOrdOutputData(ConvertNextOrderToTodayOrdStatus.InvalidHpId, new());
                }
                if (inputData.SinDate <= 0)
                {
                    return new ConvertNextOrderToTodayOrdOutputData(ConvertNextOrderToTodayOrdStatus.InvalidSinDate, new());
                }
                if (inputData.RaiinNo < 0)
                {
                    return new ConvertNextOrderToTodayOrdOutputData(ConvertNextOrderToTodayOrdStatus.InvalidRaiinNo, new());
                }
                if (inputData.UserId < 0)
                {
                    return new ConvertNextOrderToTodayOrdOutputData(ConvertNextOrderToTodayOrdStatus.InvalidUserId, new());
                }
                if (inputData.PtId < 0)
                {
                    return new ConvertNextOrderToTodayOrdOutputData(ConvertNextOrderToTodayOrdStatus.InvalidPtId, new());
                }
                if (inputData.RsvkrtOrderInfItems.Count == 0)
                {
                    return new ConvertNextOrderToTodayOrdOutputData(ConvertNextOrderToTodayOrdStatus.InvalidOrderInfs, new());
                }

                var ipnCds = new List<Tuple<string, string>>();
                foreach (var orderInfItem in inputData.RsvkrtOrderInfItems)
                {
                    ipnCds.AddRange(_mstItemRepository.GetCheckIpnCds(orderInfItem.RsvKrtOrderInfDetailItems.Select(od => od.IpnCd.Trim()).ToList()));
                }

                var result = _todayOdrRepository.FromNextOrderToTodayOrder(inputData.HpId, inputData.SinDate, inputData.RaiinNo, inputData.UserId, inputData.RsvkrtOrderInfItems.Select(rsv => NextOrderCommon.ConvertRsvkrtOrderInfToModel(inputData.HpId, inputData.PtId, ipnCds, rsv)).ToList());

                return new ConvertNextOrderToTodayOrdOutputData(ConvertNextOrderToTodayOrdStatus.Successed, result
                    .Select(o => new OdrInfItem(
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
                        o.GroupKoui.Value,
                        o.OrdInfDetails.Select(od => new OdrInfDetailItem(
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
                            od.CommentNewline,
                            od.Yakka,
                            od.IsGetPriceInYakka,
                            od.Ten,
                            od.BunkatuKoui,
                            od.AlternationIndex,
                            od.KensaGaichu,
                            od.OdrTermVal,
                            od.CnvTermVal,
                            od.YjCd,
                            od.MasterSbt,
                            od.YohoSets,
                            od.Kasan1,
                            od.Kasan2,
                            od.CnvUnitName,
                            od.OdrUnitName,
                            od.HasCmtName,
                            od.CenterItemCd1,
                            od.CenterItemCd2
                        )).ToList(),
                         o.CreateDate,
                         o.CreateId,
                         o.CreateName,
                         o.IsDeleted,
                         false
                        )).ToList());
            }
            finally
            {
                _todayOdrRepository.ReleaseResource();
            }
        }
    }
}
