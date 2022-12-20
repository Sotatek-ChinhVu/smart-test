using Domain.Models.Insurance;
using Domain.Models.OrdInfs;
using UseCase.OrdInfs.GetListTrees;

namespace Interactor.OrdInfs
{
    public class GetOrdInfListTreeInteractor : IGetOrdInfListTreeInputPort
    {
        private readonly IOrdInfRepository _ordInfRepository;
        private readonly IInsuranceRepository _insuranceRepository;
        public GetOrdInfListTreeInteractor(IOrdInfRepository ordInfRepository, IInsuranceRepository insuranceRepository)
        {
            _ordInfRepository = ordInfRepository;
            _insuranceRepository = insuranceRepository;
        }

        public GetOrdInfListTreeOutputData Handle(GetOrdInfListTreeInputData inputData)
        {
            if (inputData.RaiinNo <= 0)
            {
                return new GetOrdInfListTreeOutputData(new List<GroupHokenItem>(), GetOrdInfListTreeStatus.InvalidRaiinNo);
            }
            if (inputData.HpId <= 0)
            {
                return new GetOrdInfListTreeOutputData(new List<GroupHokenItem>(), GetOrdInfListTreeStatus.InvalidHpId);
            }
            if (inputData.PtId <= 0)
            {
                return new GetOrdInfListTreeOutputData(new List<GroupHokenItem>(), GetOrdInfListTreeStatus.InvalidPtId);
            }
            if (inputData.SinDate <= 0)
            {
                return new GetOrdInfListTreeOutputData(new List<GroupHokenItem>(), GetOrdInfListTreeStatus.InvalidSinDate);
            }
            var allOdrInfs = _ordInfRepository
                    .GetList(inputData.HpId, inputData.PtId, inputData.UserId, inputData.RaiinNo, inputData.SinDate, inputData.IsDeleted)
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
                        )).OrderBy(odrDetail => odrDetail.RpNo)
                        .ThenBy(odrDetail => odrDetail.RpEdaNo)
                        .ThenBy(odrDetail => odrDetail.RowNo)
                        .ToList(),
                         o.CreateDate,
                         o.CreateId,
                         o.CreateName
                        ))
                    .OrderBy(odr => odr.OdrKouiKbn)
                    .ThenBy(odr => odr.RpNo)
                    .ThenBy(odr => odr.RpEdaNo)
                    .ThenBy(odr => odr.SortNo)
                    .ToList();

            var hokenOdrInfs = allOdrInfs
                .GroupBy(odr => odr.HokenPid)
                .Select(grp => grp.FirstOrDefault())
                .ToList();
            var insurances = _insuranceRepository.GetListHokenPattern(inputData.HpId, inputData.PtId, false).ToList();

            if (hokenOdrInfs == null || hokenOdrInfs.Count == 0)
            {
                return new GetOrdInfListTreeOutputData(new List<GroupHokenItem>(), GetOrdInfListTreeStatus.NoData);
            }

            var tree = new GetOrdInfListTreeOutputData(new List<GroupHokenItem>(), GetOrdInfListTreeStatus.Successed);

            var obj = new object();
            Parallel.ForEach(hokenOdrInfs.Select(h => h?.HokenPid), hokenId =>
            {

                var insuance = insurances.FirstOrDefault(i => i.HokenPid == hokenId);
                var groupHoken = new GroupHokenItem(new List<GroupOdrItem>(), hokenId, insuance?.HokenName ?? string.Empty);
                // Find By Group
                var groupOdrInfs = allOdrInfs.Where(odr => odr.HokenPid == hokenId)
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

            return tree;
        }
    }
}
