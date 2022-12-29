using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Models.MstItem;
using Domain.Models.NextOrder;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.RaiinKbn;
using Domain.Models.RaiinKubunMst;
using Domain.Models.TodayOdr;
using Entity.Tenant;
using Helper.Enum;
using UseCase.MedicalExamination.CheckedItemName;
using UseCase.MedicalExamination.InitKbnSetting;

namespace Interactor.MedicalExamination
{
    public class InitKbnSettingInteractor : IInitKbnSettingInputPort
    {
        private readonly ITodayOdrRepository _todayOdrRepository;
        private readonly IMstItemRepository _mstItemRepository;
        private readonly IRaiinKubunMstRepository _raiinKubunMstRepository;
        private readonly IRaiinKbnRepository _raiinKbnRepository;
        private readonly INextOrderRepository _nextOrderRepository;

        public InitKbnSettingInteractor(ITodayOdrRepository todayOdrRepository, IMstItemRepository mstItemRepository, IRaiinKubunMstRepository raiinKubunMstRepository, IRaiinKbnRepository raiinKbnRepository, INextOrderRepository nextOrderRepository)
        {
            _todayOdrRepository = todayOdrRepository;
            _mstItemRepository = mstItemRepository;
            _raiinKubunMstRepository = raiinKubunMstRepository;
            _raiinKbnRepository = raiinKbnRepository;
            _nextOrderRepository = nextOrderRepository;
        }

        public InitKbnSettingOutputData Handle(InitKbnSettingInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new InitKbnSettingOutputData(InitKbnSettingStatus.InvalidHpId, new());
                }
                if (inputData.WindowType < 0 || (byte)inputData.WindowType > 20)
                {
                    return new InitKbnSettingOutputData(InitKbnSettingStatus.InvalidWindowType, new());
                }
                if (inputData.FrameId < 0)
                {
                    return new InitKbnSettingOutputData(InitKbnSettingStatus.InvalidFrameId, new());
                }
                if (inputData.PtId <= 0)
                {
                    return new InitKbnSettingOutputData(InitKbnSettingStatus.InvalidPtId, new());
                }
                if (inputData.RaiinNo <= 0)
                {
                    return new InitKbnSettingOutputData(InitKbnSettingStatus.InvalidRaiinNo, new());
                }
                if (inputData.SinDate <= 0)
                {
                    return new InitKbnSettingOutputData(InitKbnSettingStatus.InvalidSinDate, new());
                }

                var raiinKbnModels = _raiinKbnRepository.GetRaiinKbns(inputData.HpId, inputData.PtId, inputData.RaiinNo, inputData.SinDate);
                if (raiinKbnModels?.Count > 0)
                {
                    var raiinKouiKbns = _raiinKubunMstRepository.GetRaiinKouiKbns(inputData.HpId);
                    var raiinKbnItemCds = _raiinKubunMstRepository.GetRaiinKbnItems(inputData.HpId);
                    if (inputData.WindowType == WindowType.ReceptionInInsertMode)
                    {
                        if (inputData.IsEnableLoadDefaultVal) _nextOrderRepository.InitDefaultByNextOrder();
                    }
                    else if (inputData.WindowType == WindowType.MedicalExamination)
                    {
                        if (isEnableLoadDefaultVal) InitDefaultByTodayOrder();
                    }
                    else if (inputData.WindowType == WindowType.Booking)
                    {
                        if (isEnableLoadDefaultVal) InitDefaultByRsv(frameID);
                    }
                }

                var checkedName = _todayOdrRepository.CheckNameChanged(inputData.OdrInfs.Select(item =>
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

                return new CheckedItemNameOutputData(CheckedItemNameStatus.Successed, checkedName);
            }
            catch
            {
                return new CheckedItemNameOutputData(CheckedItemNameStatus.Failed, new());
            }
            finally
            {
                _todayOdrRepository.ReleaseResource();
            }
        }
    }
}
