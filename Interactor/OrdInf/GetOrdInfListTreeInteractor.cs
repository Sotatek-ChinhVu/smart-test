using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using UseCase.OrdInfs.GetListTrees;

namespace Interactor.OrdInfs
{
    public class GetOrdInfListTreeInteractor : IGetOrdInfListTreeInputPort
    {
        private readonly IOrdInfRepository _ordInfRepository;
        private readonly IOrdInfDetailRepository _ordInfDetailRepository;
        public GetOrdInfListTreeInteractor(IOrdInfRepository ordInfRepository, IOrdInfDetailRepository ordInfDetailRepository)
        {
            _ordInfRepository = ordInfRepository;
            _ordInfDetailRepository = ordInfDetailRepository;
        }

        public GetOrdInfListTreeOutputData Handle(GetOrdInfListTreeInputData inputData)
        {
            if (inputData.RaiinNo.Value <= 0)
            {
                return new GetOrdInfListTreeOutputData(new List<GroupHokenItem>(), GetOrdInfListTreeStatus.InvalidRaiinNo);
            }
            if (inputData.HpId.Value <= 0)
            {
                return new GetOrdInfListTreeOutputData(new List<GroupHokenItem>(), GetOrdInfListTreeStatus.InvalidHpId);
            }
            if (inputData.PtId.Value <= 0)
            {
                return new GetOrdInfListTreeOutputData(new List<GroupHokenItem>(), GetOrdInfListTreeStatus.InvalidPtId);
            }
            if (inputData.SinDate.Value <= 0)
            {
                return new GetOrdInfListTreeOutputData(new List<GroupHokenItem>(), GetOrdInfListTreeStatus.InvalidSinDate);
            }

            var allOdrInfDetails = _ordInfDetailRepository
                .GetList(inputData.PtId, inputData.RaiinNo, inputData.SinDate)
                .Select(od => new OdrInfDetailItem(
                    od.HpId.Value,
                    od.RaiinNo.Value,
                    od.RpNo.Value,
                    od.RpEdaNo.Value,
                    od.RowNo.Value,
                    od.PtId.Value,
                    od.SinDate.Value,
                    od.SinKouiKbn.Value,
                    od.ItemCd.Value,
                    od.ItemName.Value,
                    od.Suryo,
                    od.UnitName.Value,
                    od.UnitSbt.Value,
                    od.TermVal,
                    od.KohatuKbn.Value,
                    od.SyohoKbn,
                    od.SyohoLimitKbn,
                    od.DrugKbn,
                    od.YohoKbn,
                    od.Kokuji1.Value,
                    od.Kokuji2.Value,
                    od.IsNodspRece,
                    od.IpnCd.Value,
                    od.IpnName.Value,
                    od.JissiKbn,
                    od.JissiDate,
                    od.JissiId,
                    od.JissiMachine,
                    od.ReqCd.Value,
                    od.Bunkatu.Value,
                    od.CmtName.Value,
                    od.CmtName.Value,
                    od.FontColor,
                    od.CommentNewline
                ))
               .OrderBy(odrDetail => odrDetail.RpNo)
               .ThenBy(odrDetail => odrDetail.RpEdaNo)
               .ThenBy(odrDetail => odrDetail.RowNo)
               .ToList();

            var allOdrInfs = _ordInfRepository
                    .GetList(inputData.PtId, inputData.RaiinNo, inputData.SinDate)
                    .Select(o => new OdrInfItem(
                        o.HpId.Value,
                        o.RaiinNo.Value,
                        o.RpNo.Value,
                        o.RpEdaNo.Value,
                        o.PtId.Value,
                        o.SinDate.Value,
                        o.HokenPid.Value,
                        o.OdrKouiKbn.Value,
                        o.RpName.Value,
                        o.InoutKbn.Value,
                        o.SikyuKbn.Value,
                        o.SyohoSbt.Value,
                        o.SanteiKbn.Value,
                        o.TosekiKbn.Value,
                        o.DaysCnt,
                        o.SortNo,
                        o.Id.Value,
                        o.GroupKoui.Value,
                        allOdrInfDetails
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

            var dem = 1;

            if (hokenOdrInfs?.Count > 0)
            {
                var tree = new GetOrdInfListTreeOutputData(new List<GroupHokenItem>(), GetOrdInfListTreeStatus.Successed);
                foreach (var hokenId in hokenOdrInfs.Select(h => h?.HokenPid))
                {
                    var groupHoken = new GroupHokenItem(new List<GroupOdrItem>(), hokenId, "Hoken title " + dem);
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
                    if (groupOdrInfs?.Count > 0)
                    {
                        foreach (var groupOdrInf in groupOdrInfs)
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


                            foreach (OdrInfItem rpOdrInf in rpOdrInfs)
                            {
                                // Find OdrInfDetail
                                var odrInfDetails = allOdrInfDetails
                                    .Where(detail => detail.RpNo == rpOdrInf.RpNo && detail.RpEdaNo == rpOdrInf.RpEdaNo)
                                    .ToList();
                                var odrModel = new OdrInfItem(
                                    rpOdrInf.HpId,
                                    rpOdrInf.RaiinNo,
                                    rpOdrInf.RpNo,
                                    rpOdrInf.RpEdaNo,
                                    rpOdrInf.PtId,
                                    rpOdrInf.SinDate,
                                    rpOdrInf.HokenPid,
                                    rpOdrInf.OdrKouiKbn,
                                    rpOdrInf.RpName,
                                    rpOdrInf.InoutKbn,
                                    rpOdrInf.SikyuKbn,
                                    rpOdrInf.SyohoSbt,
                                    rpOdrInf.SanteiKbn,
                                    rpOdrInf.TosekiKbn,
                                    rpOdrInf.DaysCnt,
                                    rpOdrInf.SortNo,
                                    rpOdrInf.Id,
                                    rpOdrInf.GroupOdrKouiKbn,
                                    odrInfDetails
                                    );
                                group.OdrInfs.Add(odrModel);
                            }
                            groupHoken.GroupOdrItems.Add(group);
                        }
                    }
                    tree.GroupHokens.Add(groupHoken);
                    dem++;
                }

                return tree;
            }

            return new GetOrdInfListTreeOutputData(new List<GroupHokenItem>(), GetOrdInfListTreeStatus.NoData);
        }
    }
}
