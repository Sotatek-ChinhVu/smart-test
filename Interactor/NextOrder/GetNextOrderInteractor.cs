using Domain.Models.Insurance;
using Domain.Models.NextOrder;
using Microsoft.EntityFrameworkCore;
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

                var insurances = _insuranceRepository.GetListHokenPattern(inputData.HpId, inputData.PtId, false).ToList();
                var nextOrder = _nextOrderRepository.Get(inputData.HpId, inputData.PtId, inputData.RsvkrtNo, inputData.Type);
                var orderInfItems = nextOrder.orderInfs.Select(
                        o => new RsvKrtOrderInfItem(
                                o.HpId,
                                o.PtId,
                                o.RsvkrtNo,
                                o.RpNo,
                                o.RpEdaNo,
                                o.Id,
                                o.HokenPid,
                                o.OdrKouiKbn,
                                o.RpName,
                                o.RpName,
                                o.InoutKbn,
                                o.SikyuKbn,
                                o.SyohoSbt,
                                o.SanteiKbn,
                                o.TosekiKbn,
                                o.DaysCnt,
                                o.IsDeleted,
                                o.SortNo,
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

                var obj = new object();
                Parallel.ForEach(hokenOdrInfs.Select(h => h?.HokenPid), hokenId =>
                {

                    var insuance = insurances.FirstOrDefault(i => i.HokenPid == hokenId);
                    var groupHoken = new GroupHokenItem(new List<GroupOdrItem>(), hokenId, insuance?.HokenName ?? string.Empty);
                    // Find By Group
                    var groupOdrInfs = nextOrder.orderInfs?.Where(odr => odr.HokenPid == hokenId)
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
                            var rpOdrInfs = allOdrInfs.Where(odrInf => odrInf.HokenPid == hokenId
                                                    && odrInf.GroupOdrKouiKbn == groupOdrInf?.GroupOdrKouiKbn
                                                    && odrInf.InoutKbn == groupOdrInf?.InoutKbn
                                                    && odrInf.SyohoSbt == groupOdrInf?.SyohoSbt
                                                    && odrInf.SikyuKbn == groupOdrInf?.SikyuKbn
                                                    && odrInf.TosekiKbn == groupOdrInf?.TosekiKbn
                                                    && odrInf.SanteiKbn == groupOdrInf?.SanteiKbn)
                                                .ToList();

                            var group = new GroupOdrItem("Hoken title", new List<OdrInfItem>(), hokenId);
                            lock (objGroupOdrInf)
                            {
                                group.OdrInfs.AddRange(rpOdrInfs);
                                groupHoken.GroupOdrItems.Add(group);
                            }
                        });
                    }
                    lock (obj)
                    {
                        tree.GroupHokens.Add(groupHoken);
                    }
                });

                return new GetHeaderInfOutputData(reception.SyosaisinKbn, reception.JikanKbn, reception.HokenPid, reception.SanteiKbn, reception.TantoId, reception.KaId, reception.UketukeTime, reception.SinStartTime, reception.SinEndTime, raiinTag.TagNo, odrInf, GetHeaderInfStatus.Successed);
            }
            catch
            {
                return new GetNextOrderOutputData(new(), new(), new(), GetNextOrderStatus.Failed);
            }
        }
    }
}
