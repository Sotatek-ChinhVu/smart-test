using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.TodayOdr;
using UseCase.MedicalExamination.ConvertItem;
using OdrInfDetailItemOfTodayOrder = UseCase.OrdInfs.GetListTrees.OdrInfDetailItem;
using OdrInfItemOfTodayOrder = UseCase.OrdInfs.GetListTrees.OdrInfItem;

namespace Interactor.MedicalExamination
{
    public class ConvertItemInteractor : IConvertItemInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;
        private readonly ITodayOdrRepository _todayOrderRepository;

        public ConvertItemInteractor(IMstItemRepository mstItemRepository, ITodayOdrRepository todayOrderRepository)
        {
            _mstItemRepository = mstItemRepository;
            _todayOrderRepository = todayOrderRepository;
        }

        public ConvertItemOutputData Handle(ConvertItemInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new ConvertItemOutputData(ConvertItemStatus.InValidHpId, new());
                }
                if (inputData.UserId <= 0)
                {
                    return new ConvertItemOutputData(ConvertItemStatus.InValidUserId, new());
                }
                if (inputData.RaiinNo <= 0)
                {
                    return new ConvertItemOutputData(ConvertItemStatus.InValidRaiinNo, new());
                }
                if (inputData.PtId <= 0)
                {
                    return new ConvertItemOutputData(ConvertItemStatus.InValidPtId, new());
                }

                if (inputData.SinDate <= 0)
                {
                    return new ConvertItemOutputData(ConvertItemStatus.InValidSinDate, new());
                }

                if (inputData.ExpiredItems.Count == 0 || inputData.OdrInfItems.Count == 0)
                {
                    return new ConvertItemOutputData(ConvertItemStatus.InputNotData, new());
                }
                Dictionary<string, TenItemModel> filterExpiredItems = new();
                Dictionary<string, List<TenItemModel>> filterSaveHistoryItems = new();
                foreach (var expiredItem in inputData.ExpiredItems)
                {
                    var tenItem = expiredItem.Value.FirstOrDefault(e => e.ModeStatus != -1) ?? new();
                    var tenItemHistories = expiredItem.Value.Where(e => e.ModeStatus == 0 || e.ModeStatus == -1).ToList();
                    if (tenItem != null && !string.IsNullOrEmpty(tenItem.ItemCd))
                    {
                        filterExpiredItems.Add(expiredItem.Key, tenItem);
                    }

                    if (tenItemHistories?.Count > 0)
                    {
                        filterSaveHistoryItems.Add(expiredItem.Key, tenItemHistories);
                    }
                }

                bool saveHistory = true;
                if (filterSaveHistoryItems.Count > 0)
                    saveHistory = _mstItemRepository.ExceConversionItem(inputData.HpId, inputData.UserId, filterSaveHistoryItems);

                var conversionItem = _todayOrderRepository.ConvertConversionItemToOrderInfModel(inputData.HpId, inputData.RaiinNo, inputData.PtId, inputData.SinDate, inputData.OdrInfItems.Select(item =>
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
                        0,
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
                    )).ToList(), filterExpiredItems);

                var conversionItemOuput = conversionItem.Select(
                    r => new OdrInfItemOfTodayOrder(
                        r.HpId,
                        r.RaiinNo,
                        r.RpNo,
                        r.RpEdaNo,
                        r.PtId,
                        r.SinDate,
                        r.HokenPid,
                        r.OdrKouiKbn,
                        r.RpName,
                        r.InoutKbn,
                        r.SikyuKbn,
                        r.SyohoSbt,
                        r.SanteiKbn,
                        r.TosekiKbn,
                        r.DaysCnt,
                        r.SortNo,
                        r.Id,
                        r.GroupKoui.Value,
                        r.OrdInfDetails.Select(
                             od => new OdrInfDetailItemOfTodayOrder(
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
                                 )
                            ).ToList(),
                           r.CreateDate,
                           r.CreateId,
                           r.CreateName
                        )).ToList();

                if (!saveHistory)
                {
                    return new ConvertItemOutputData(ConvertItemStatus.Failed, new());
                }

                return new ConvertItemOutputData(ConvertItemStatus.Successed, conversionItemOuput);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }


    }
}
