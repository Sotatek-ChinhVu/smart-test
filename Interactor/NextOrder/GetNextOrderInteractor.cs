using Domain.Models.Insurance;
using Domain.Models.NextOrder;
using UseCase.NextOrder.Get;

namespace Interactor.NextOrder
{
    public class GetNextOrderInteractor : IGetNextOrderInputPort
    {
        private readonly INextOrder _nextOrderRepository;
        private readonly IInsuranceRepository _insuranceRepository;

        public GetNextOrderInteractor(INextOrder nextOrderRepository, IInsuranceRepository insuranceRepository)
        {
            _nextOrderRepository = nextOrderRepository;
            _insuranceRepository = insuranceRepository;
        }

        public GetNextOrderOutputData Handle(GetNextOrderInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new GetNextOrderOutputData(new(), new(), new(), GetNextOrderStatus.InvalidHpId);
                }
                if (inputData.PtId <= 0)
                {
                    return new GetNextOrderOutputData(new(), new(), new(), GetNextOrderStatus.InvalidPtId);

                }
                if (inputData.RsvkrtNo <= 0)
                {
                    return new GetNextOrderOutputData(new(), new(), new(), GetNextOrderStatus.InvalidRsvkrtNo);
                }
                if (inputData.SinDate <= 0)
                {
                    return new GetNextOrderOutputData(new(), new(), new(), GetNextOrderStatus.InvalidSinDate);
                }
                if (inputData.UserId <= 0)
                {
                    return new GetNextOrderOutputData(new(), new(), new(), GetNextOrderStatus.InvalidUserId);
                }

                var insurances = _insuranceRepository.GetListHokenPattern(inputData.HpId, inputData.PtId, false).ToList();
                var nextOrder = _nextOrderRepository.Get(inputData.HpId, inputData.PtId, inputData.RsvkrtNo, inputData.UserId, inputData.SinDate, inputData.Type);
                var orderInfItems = nextOrder.orderInfs.Select(
                        o => new RsvKrtOrderInfItem(
                                o.HpId,
                                o.PtId,
                                o.RsvDate,
                                o.RsvkrtNo,
                                o.RpNo,
                                o.RpEdaNo,
                                o.Id,
                                o.HokenPid,
                                o.OdrKouiKbn,
                                o.RpName,
                                o.InoutKbn,
                                o.SikyuKbn,
                                o.SyohoSbt,
                                o.SanteiKbn,
                                o.TosekiKbn,
                                o.DaysCnt,
                                o.IsDeleted,
                                o.SortNo,
                                o.GroupKoui.Value,
                                o.CreateDate,
                                o.CreateId,
                                o.CreateName,
                                o.OrderInfDetailModels.Select(od =>
                                    new RsvKrtOrderInfDetailItem(
                                      od.HpId,
                                      od.PtId,
                                      od.RsvkrtNo,
                                      od.RpNo,
                                      od.RpEdaNo,
                                      od.RowNo,
                                      od.RsvDate,
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
                                      od.Bunkatu,
                                      od.CmtName,
                                      od.CmtOpt,
                                      od.FontColor,
                                      od.CommentNewline,
                                      od.MasterSbt,
                                      od.InOutKbn,
                                      od.Yakka,
                                      od.IsGetPriceInYakka,
                                      od.Ten,
                                      od.BunkatuKoui,
                                      od.AlternationIndex,
                                      od.KensaGaichu,
                                      od.RefillSetting,
                                      od.CmtCol1,
                                      od.OdrTermVal,
                                      od.CnvTermVal,
                                      od.YjCd,
                                      od.YohoSets,
                                      od.Kasan1,
                                      od.Kasan2
                                    )).ToList()
                            )
                    );

                var hokenOdrInfs = nextOrder.orderInfs?
               .GroupBy(odr => odr.HokenPid)
               .Select(grp => grp.FirstOrDefault())
               .ToList();
                if (nextOrder.byomeis.Count == 0 && nextOrder.karteInf.HpId == 0 && nextOrder.karteInf.PtId == 0 && nextOrder.karteInf.SeqNo == 0 && (hokenOdrInfs == null || hokenOdrInfs.Count == 0))
                {
                    return new GetNextOrderOutputData(new(), new(), new(), GetNextOrderStatus.NoData);
                }
                var byomeiItems = nextOrder.byomeis.Select(b => new RsvKrtByomeiItem(
                        b.Id,
                        b.HpId,
                        b.PtId,
                        b.RsvkrtNo,
                        b.SeqNo,
                        b.ByomeiCd,
                        b.Byomei,
                        b.SyobyoKbn,
                        b.SikkanKbn,
                        b.NanbyoCd,
                        b.HosokuCmt,
                        b.IsNodspRece,
                        b.IsNodspKarte,
                        b.IsDeleted,
                        b.Icd10,
                        b.Icd102013,
                        b.Icd1012013,
                        b.Icd1022013,
                        b.PrefixSuffixList
                    )).ToList();

                var obj = new object();
                var tree = new GetNextOrderOutputData(new(), nextOrder.karteInf, byomeiItems, GetNextOrderStatus.Successed);
                if (hokenOdrInfs?.Count > 0)
                {
                    Parallel.ForEach(hokenOdrInfs.Select(h => h?.HokenPid), hokenId =>
                    {
                        var insuance = insurances.FirstOrDefault(i => i.HokenPid == hokenId);
                        var groupHoken = new GroupHokenItem(new List<GroupOdrItem>(), hokenId, insuance?.HokenName ?? string.Empty);
                        // Find By Group
                        var groupOdrInfs = orderInfItems?.Where(odr => odr.HokenPid == hokenId)
                            .GroupBy(odr => new
                            {
                                odr.HokenPid,
                                odr.GroupOdrKouiKbn,
                                odr.InoutKbn,
                                odr.SyohoSbt,
                                odr.SikyuKbn,
                                odr.TosekiKbn,
                                odr.SanteiKbn
                            })
                            .Select(grp => grp.FirstOrDefault())
                            .ToList();
                        if (!(groupOdrInfs == null || groupOdrInfs.Count == 0))
                        {
                            var objGroupOdrInf = new object();
                            Parallel.ForEach(groupOdrInfs, groupOdrInf =>
                            {
                                var odrInfs = orderInfItems?.Where(odrInf => odrInf.HokenPid == hokenId
                                                        && odrInf.GroupOdrKouiKbn == groupOdrInf?.GroupOdrKouiKbn
                                                        && odrInf.InoutKbn == groupOdrInf?.InoutKbn
                                                        && odrInf.SyohoSbt == groupOdrInf?.SyohoSbt
                                                        && odrInf.SikyuKbn == groupOdrInf?.SikyuKbn
                                                        && odrInf.TosekiKbn == groupOdrInf?.TosekiKbn
                                                        && odrInf.SanteiKbn == groupOdrInf?.SanteiKbn)
                                                    .ToList();

                                var group = new GroupOdrItem(insuance?.HokenName ?? string.Empty, new List<RsvKrtOrderInfItem>(), hokenId); ;
                                lock (objGroupOdrInf)
                                {
                                    if (odrInfs?.Count > 0)
                                        group.OdrInfs.AddRange(odrInfs);
                                    groupHoken.GroupOdrItems.Add(group);
                                }
                            });
                        }
                        lock (obj)
                        {
                            tree.GroupHokenItems.Add(groupHoken);
                        }
                    });
                }

                return tree;
            }
            catch
            {
                return new GetNextOrderOutputData(new(), new(), new(), GetNextOrderStatus.Failed);
            }
        }
    }
}
