using Domain.Models.TodayOdr;
using UseCase.MedicalExamination.AddAutoItem;
using UseCase.OrdInfs.GetListTrees;

namespace Interactor.MedicalExamination
{
    public class AddAutoItemInteractor : IAddAutoItemInputPort
    {
        private readonly ITodayOdrRepository _todayOdrRepository;
        public AddAutoItemInteractor(ITodayOdrRepository todayOdrRepository)
        {
            _todayOdrRepository = todayOdrRepository;
        }

        public AddAutoItemOutputData Handle(AddAutoItemInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new AddAutoItemOutputData(AddAutoItemStatus.InvalidHpId, new());
                }
                if (inputData.UserId <= 0)
                {
                    return new AddAutoItemOutputData(AddAutoItemStatus.InvalidUserId, new());
                }
                if (inputData.SinDate < 0)
                {
                    return new AddAutoItemOutputData(AddAutoItemStatus.InvalidSinDate, new());
                }
                if (inputData.OrderInfItems.Count == 0)
                {
                    return new AddAutoItemOutputData(AddAutoItemStatus.InvalidAddedAutoItem, new());
                }

                var result = _todayOdrRepository.AutoAddOrders(inputData.HpId, inputData.UserId, inputData.SinDate, inputData.OrderInfItems.Select(c => new Tuple<int, int, string, int, int>(c.OrderPosition, c.OrderDetailPosition, c.RpName, c.InOutKbn, c.OdrKouiKbn)).ToList(), inputData.AddedOrderInfs.Select(o => new Tuple<int, int, string, long>(o.OrderDetailPosition, o.OrderDetailPosition, o.ItemCd, o.Id)).ToList());


                return new AddAutoItemOutputData(AddAutoItemStatus.Successed, result.Select(r => new OdrInfItem(
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
                             od => new OdrInfDetailItem(
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
                                 od.OdrUnitName
                                 )
                            ).ToList(),
                           r.CreateDate,
                           r.CreateId,
                           r.CreateName
                        )).ToList());
            }
            catch
            {
                return new AddAutoItemOutputData(AddAutoItemStatus.Failed, new());
            }
        }
    }
}
