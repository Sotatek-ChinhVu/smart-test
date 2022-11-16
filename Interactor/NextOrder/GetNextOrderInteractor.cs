using Domain.Models.Insurance;
using Domain.Models.NextOrder;
using UseCase.NextOrder.Get;

namespace Interactor.NextOrder
{
    public class GetNextOrderInteractor : IGetNextOrderInputPort
    {
        private readonly INextOrderRepository _nextOrderRepository;
        private readonly IInsuranceRepository _insuranceRepository;

        public GetNextOrderInteractor(INextOrderRepository nextOrderRepository, IInsuranceRepository insuranceRepository)
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

                var byomeis = _nextOrderRepository.GetByomeis(inputData.HpId, inputData.PtId, inputData.RsvkrtNo, inputData.Type);
                var orderInfs = _nextOrderRepository.GetOrderInfs(inputData.HpId, inputData.PtId, inputData.RsvkrtNo, inputData.SinDate, inputData.UserId);
                var karteInf = _nextOrderRepository.GetKarteInf(inputData.HpId, inputData.PtId, inputData.RsvkrtNo);

                var orderInfItems = orderInfs.Select(o => new RsvKrtOrderInfItem(o));

                var hokenOdrInfs = orderInfs?
               .GroupBy(odr => odr.HokenPid)
               .Select(grp => grp.FirstOrDefault())
               .ToList();
                if (byomeis.Count == 0 && karteInf.HpId == 0 && karteInf.PtId == 0 && karteInf.SeqNo == 0 && (hokenOdrInfs == null || hokenOdrInfs.Count == 0))
                {
                    return new GetNextOrderOutputData(new(), new(), new(), GetNextOrderStatus.NoData);
                }
                var byomeiItems = byomeis.Select(b => new RsvKrtByomeiItem(b)).ToList();

                var obj = new object();
                var tree = new GetNextOrderOutputData(new(), karteInf, byomeiItems, GetNextOrderStatus.Successed);
                if (hokenOdrInfs?.Count > 0)
                {
                    Parallel.ForEach(hokenOdrInfs.Select(h => h?.HokenPid), hokenId =>
                    {
                        var insuance = insurances.FirstOrDefault(i => i.HokenPid == hokenId);
                        var groupHoken = new GroupHokenItem(new List<GroupOdrItem>(), hokenId ?? 0, insuance?.HokenName ?? string.Empty);
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
                        if (!(groupOdrInfs == null || groupOdrInfs.Count() == 0))
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

                                var group = new GroupOdrItem(insuance?.HokenName ?? string.Empty, new List<RsvKrtOrderInfItem>(), hokenId ?? 0); ;
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
