using Domain.Models.TodayOdr;
using UseCase.MedicalExamination.ConvertFromHistoryTodayOrder;
using UseCase.OrdInfs.GetListTrees;

namespace Interactor.MedicalExamination
{
    public class ConvertFromHistoryToTodayOdrInteractor : IConvertFromHistoryTodayOrderInputPort
    {
        private readonly ITodayOdrRepository _todayOdrRepository;

        public ConvertFromHistoryToTodayOdrInteractor(ITodayOdrRepository todayOdrRepository)
        {
            _todayOdrRepository = todayOdrRepository;
        }

        public ConvertFromHistoryTodayOrderOutputData Handle(ConvertFromHistoryTodayOrderInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new ConvertFromHistoryTodayOrderOutputData(ConvertFromHistoryTodayOrderStatus.InvalidHpId, new());
                }
                if (inputData.PtId <= 0)
                {
                    return new ConvertFromHistoryTodayOrderOutputData(ConvertFromHistoryTodayOrderStatus.InvalidPtId, new());
                }
                if (inputData.SinDate <= 0)
                {
                    return new ConvertFromHistoryTodayOrderOutputData(ConvertFromHistoryTodayOrderStatus.InvalidSinDate, new());
                }
                if (inputData.RaiinNo <= 0)
                {
                    return new ConvertFromHistoryTodayOrderOutputData(ConvertFromHistoryTodayOrderStatus.InvalidRaiinNo, new());
                }
                if (inputData.UserId <= 0)
                {
                    return new ConvertFromHistoryTodayOrderOutputData(ConvertFromHistoryTodayOrderStatus.InvalidUserId, new());
                }
                if (inputData.HistoryOdrInfModels.Count <= 0)
                {
                    return new ConvertFromHistoryTodayOrderOutputData(ConvertFromHistoryTodayOrderStatus.InputNoData, new());
                }
                var result = _todayOdrRepository.FromHistory(inputData.HpId, inputData.SinDate, inputData.RaiinNo, inputData.UserId, inputData.PtId, inputData.HistoryOdrInfModels);

                return new ConvertFromHistoryTodayOrderOutputData(ConvertFromHistoryTodayOrderStatus.Successed, result.Select(o => new OdrInfItem(
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
                        )).OrderBy(odrDetail => odrDetail.RpNo)
                        .ThenBy(odrDetail => odrDetail.RpEdaNo)
                        .ThenBy(odrDetail => odrDetail.RowNo)
                        .ToList(),
                         o.CreateDate,
                         o.CreateId,
                         o.CreateName
                        )).ToList());
            }
            finally
            {
                _todayOdrRepository.ReleaseResource();
            }
        }
    }
}
