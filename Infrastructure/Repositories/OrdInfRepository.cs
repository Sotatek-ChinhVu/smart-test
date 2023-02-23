using Domain.Models.MstItem;
using Domain.Models.OrdInf;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class OrdInfRepository : RepositoryBase, IOrdInfRepository
    {
        public OrdInfRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public void Create(OrdInfModel ord)
        {
            throw new NotImplementedException();
        }

        public void Delete(int ordId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OrdInfModel> GetList(int hpId, long ptId, int userId, long raiinNo, int sinDate, bool isDeleted)
        {
            var allOdrInf = NoTrackingDataContext.OdrInfs.Where(odr => odr.HpId == hpId && odr.PtId == ptId && odr.RaiinNo == raiinNo && odr.SinDate == sinDate && odr.OdrKouiKbn != 10 && (isDeleted || odr.IsDeleted == 0))?.ToList();
            var rpNos = allOdrInf?.Select(o => o.RpNo);
            var allOdrInfDetails = NoTrackingDataContext.OdrInfDetails.Where(o => o.HpId == hpId && o.PtId == ptId && o.SinDate == sinDate && o.RaiinNo == raiinNo && o.ItemCd != ItemCdConst.JikanKihon && o.ItemCd != ItemCdConst.SyosaiKihon && (rpNos != null && rpNos.Contains(o.RpNo)))?.ToList();

            var result = ConvertEntityToListOrdInfModel(allOdrInf, allOdrInfDetails, hpId, sinDate, sinDate, userId, false);
            return result;
        }

        public IEnumerable<OrdInfModel> GetList(long ptId, int hpId, int userId, int deleteCondition, List<long> raiinNos)
        {
            var allOdrInf = NoTrackingDataContext.OdrInfs.Where(odr => odr.PtId == ptId && odr.HpId == hpId && odr.OdrKouiKbn != 10 && raiinNos.Contains(odr.RaiinNo))?.ToList();
            var rpNo = allOdrInf?.Select(o => o.RpNo);
            var rpEdNo = allOdrInf?.Select(o => o.RpEdaNo);
            var allOdrInfDetails = NoTrackingDataContext.OdrInfDetails.Where(o => o.PtId == ptId && o.HpId == hpId && (rpNo != null && rpNo.Contains(o.RpNo)) && (rpEdNo != null && rpEdNo.Contains(o.RpEdaNo)))?.ToList();

            if (deleteCondition == 0)
            {
                allOdrInf = allOdrInf?.Where(r => r.IsDeleted == DeleteTypes.None).ToList();
            }
            else if (deleteCondition == 1)
            {
                allOdrInf = allOdrInf?.Where(r => r.IsDeleted == DeleteTypes.None || r.IsDeleted == DeleteTypes.Deleted).ToList();
            }
            else
            {
                allOdrInf = allOdrInf?.Where(r => r.IsDeleted == DeleteTypes.None || r.IsDeleted == DeleteTypes.Deleted || r.IsDeleted == DeleteTypes.Confirm).ToList();
            }

            var sindateMin = allOdrInfDetails?.Count() > 0 ? allOdrInfDetails.Min(o => o.SinDate) : 0;
            var sindateMax = allOdrInfDetails?.Count() > 0 ? allOdrInfDetails.Max(o => o.SinDate) : 0;

            return ConvertEntityToListOrdInfModel(allOdrInf, allOdrInfDetails, hpId, sindateMin, sindateMax, userId);
        }

        public List<OrdInfModel> GetList(int hpId, long ptId, int sinYm, int hokenId)
        {
            List<int> hokenPIdList = NoTrackingDataContext.SinKouis.Where(item => item.HpId == hpId
                                                                                  && item.PtId == ptId
                                                                                  && item.SinYm == sinYm
                                                                                  && item.HokenId == hokenId
                                                                                  && item.IsNodspRece == 0
                                                                                  && item.IsDeleted == DeleteTypes.None)
                                                                   .Select(item => item.HokenPid)
                                                                   .Distinct()
                                                                   .ToList();

            var odrInfList = NoTrackingDataContext.OdrInfs.Where(item => item.HpId == hpId
                                                                         && item.PtId == ptId
                                                                         && item.SinDate / 100 == sinYm
                                                                         && hokenPIdList.Contains(item.HokenPid)
                                                                         && item.IsDeleted == DeleteTypes.None
                                                                         && item.OdrKouiKbn != 10)
                                                          .ToList();

            var raiinNoList = odrInfList.Select(item => item.RaiinNo).Distinct().ToList();
            var rpNoList = odrInfList.Select(item => item.RpNo).Distinct().ToList();
            var rpEdaNoList = odrInfList.Select(item => item.RpEdaNo).Distinct().ToList();
            var sinDateList = odrInfList.Select(item => item.SinDate).Distinct().ToList();

            var odrInfDetailList = NoTrackingDataContext.OdrInfDetails.Where(item => item.HpId == hpId
                                                                                     && item.PtId == ptId
                                                                                     && raiinNoList.Contains(item.RaiinNo)
                                                                                     && rpNoList.Contains(item.RpNo)
                                                                                     && rpEdaNoList.Contains(item.RpEdaNo)
                                                                                     && sinDateList.Contains(item.SinDate))
                                                                      .ToList();

            var sindateMin = odrInfDetailList.Any() ? odrInfDetailList.Min(o => o.SinDate) : 0;
            var sindateMax = odrInfDetailList.Any() ? odrInfDetailList.Max(o => o.SinDate) : 0;

            return ConvertEntityToListOrdInfModel(odrInfList, odrInfDetailList, hpId, sindateMin, sindateMax, 0);
        }

        public OrdInfModel GetHeaderInfo(int hpId, long ptId, long raiinNo, int sinDate)
        {

            var odrInf = NoTrackingDataContext.OdrInfs.FirstOrDefault(odr => odr.HpId == hpId && odr.PtId == ptId && odr.RaiinNo == raiinNo && odr.SinDate == sinDate && odr.OdrKouiKbn == 10 && odr.IsDeleted == 0) ?? new OdrInf();

            var allOdrInfDetails = NoTrackingDataContext.OdrInfDetails.Where(o => o.HpId == hpId && o.PtId == ptId && o.SinDate == sinDate && o.RaiinNo == raiinNo && o.RpNo == odrInf.RpNo && odrInf.RpEdaNo == o.RpEdaNo)?.ToList();

            var odrInfModel = ConvertToModel(odrInf);
            var odrInfDetailModels = allOdrInfDetails?.Select(od => ConvertToDetailModel(od)).ToList();
            odrInfModel.ChangeOdrDetail(odrInfDetailModels ?? new List<OrdInfDetailModel>());

            return odrInfModel;
        }

        public IEnumerable<OrdInfModel> GetListToCheckValidate(long ptId, int hpId, List<long> raiinNos)
        {
            var allOdrInf = NoTrackingDataContext.OdrInfs.Where(odr => odr.PtId == ptId && odr.HpId == hpId && odr.OdrKouiKbn != 10 && raiinNos.Contains(odr.RaiinNo) && odr.IsDeleted == 0)?.ToList();


            return allOdrInf?.Select(o => ConvertToModel(o)) ?? new List<OrdInfModel>();
        }

        public bool CheckExistOrder(int hpId, long ptId, long raiinNo, int sinDate, long rpNo, long rpEdaNo)
        {
            var check = NoTrackingDataContext.OdrInfs.Any(o => o.RpNo == rpNo && o.RpEdaNo == rpEdaNo && o.HpId == hpId && o.PtId == ptId && o.SinDate == sinDate && o.RaiinNo == raiinNo && o.IsDeleted == 0);
            return check;
        }

        public bool CheckIsGetYakkaPrice(int hpId, TenItemModel tenMst, int sinDate)
        {
            if (tenMst == null) return false;
            var ipnKasanExclude = NoTrackingDataContext.ipnKasanExcludes.Where(u => u.HpId == hpId && u.IpnNameCd == tenMst.IpnNameCd && u.StartDate <= sinDate && u.EndDate >= sinDate).FirstOrDefault();

            var ipnKasanExcludeItem = NoTrackingDataContext.ipnKasanExcludeItems.Where(u => u.HpId == hpId && u.ItemCd == tenMst.ItemCd && u.StartDate <= sinDate && u.EndDate >= sinDate).FirstOrDefault();
            return ipnKasanExclude == null && ipnKasanExcludeItem == null;
        }

        public List<Tuple<string, string, bool>> CheckIsGetYakkaPrices(int hpId, List<TenItemModel> tenMsts, int sinDate)
        {
            var result = new List<Tuple<string, string, bool>>();
            if (!(tenMsts?.Count > 0)) return result;
            foreach (var tenMst in tenMsts)
            {
                var ipnKasanExclude = NoTrackingDataContext.ipnKasanExcludes.Where(u => u.HpId == hpId && u.IpnNameCd == tenMst.IpnNameCd && u.StartDate <= sinDate && u.EndDate >= sinDate).FirstOrDefault();

                var ipnKasanExcludeItem = NoTrackingDataContext.ipnKasanExcludeItems.Where(u => u.HpId == hpId && u.ItemCd == tenMst.ItemCd && u.StartDate <= sinDate && u.EndDate >= sinDate).FirstOrDefault();

                result.Add(Tuple.Create(tenMst.IpnNameCd, tenMst.ItemCd, ipnKasanExclude == null && ipnKasanExcludeItem == null));
            }

            return result;
        }

        public IpnMinYakkaMstModel FindIpnMinYakkaMst(int hpId, string ipnNameCd, int sinDate)
        {
            var yakkaMst = NoTrackingDataContext.IpnMinYakkaMsts.Where(p =>
                  p.HpId == hpId &&
                  p.StartDate <= sinDate &&
                  p.EndDate >= sinDate &&
                  p.IpnNameCd == ipnNameCd)
              .FirstOrDefault();

            if (yakkaMst != null)
            {
                return new IpnMinYakkaMstModel(
                        yakkaMst.Id,
                        yakkaMst.HpId,
                        yakkaMst.IpnNameCd,
                        yakkaMst.StartDate,
                        yakkaMst.EndDate,
                        yakkaMst.Yakka,
                        yakkaMst.SeqNo,
                        yakkaMst.IsDeleted
                    );
            }

            return new IpnMinYakkaMstModel(0, 0, string.Empty, 0, 0, 0, 0, 0);
        }

        public List<IpnMinYakkaMstModel> GetCheckIpnMinYakkaMsts(int hpId, int sinDate, List<string> ipnNameCds)
        {
            var yakkaMsts = NoTrackingDataContext.IpnMinYakkaMsts.Where(p =>
               p.HpId == hpId &&
               p.StartDate <= sinDate &&
               p.EndDate >= sinDate &&
               ipnNameCds.Contains(p.IpnNameCd)).ToList();

            return !(yakkaMsts?.Count > 0) ? new List<IpnMinYakkaMstModel>() : yakkaMsts.Select(yakkaMst => new IpnMinYakkaMstModel(
                    yakkaMst.Id,
                    yakkaMst.HpId,
                    yakkaMst.IpnNameCd,
                    yakkaMst.StartDate,
                    yakkaMst.EndDate,
                    yakkaMst.Yakka,
                    yakkaMst.SeqNo,
                    yakkaMst.IsDeleted
                )).ToList();
        }

        public long GetMaxRpNo(int hpId, long ptId, long raiinNo, int sinDate)
        {
            var odrList = NoTrackingDataContext.OdrInfs
            .Where(odr => odr.HpId == hpId && odr.PtId == ptId && odr.RaiinNo == raiinNo && odr.SinDate == sinDate);

            if (odrList.Any())
            {
                return odrList.Max(odr => odr.RpNo);
            }

            return 0;
        }

        public int GetSinDate(long ptId, int hpId, int searchType, int sinDate, List<long> listRaiiNoSameSinDate, string searchText)
        {
            if (searchType == 1)
                return NoTrackingDataContext.OdrInfDetails.OrderBy(od => od.SinDate).LastOrDefault(od => od.HpId == hpId && od.PtId == ptId && (od.ItemName != null && od.ItemName.Contains(searchText)) && od.SinDate <= sinDate && !listRaiiNoSameSinDate.Contains(od.RaiinNo))?.SinDate ?? -1;
            else
                return NoTrackingDataContext.OdrInfDetails.OrderBy(od => od.SinDate).FirstOrDefault(od => od.HpId == hpId && od.PtId == ptId && (od.ItemName != null && od.ItemName.Contains(searchText)) && od.SinDate >= sinDate && !listRaiiNoSameSinDate.Contains(od.RaiinNo))?.SinDate ?? -1;
        }

        private List<OrdInfModel> ConvertEntityToListOrdInfModel(List<OdrInf>? allOdrInf, List<OdrInfDetail>? allOdrInfDetails, int hpId, int sinDateMin, int sinDateMax, int userId, bool isHistory = true)
        {
            var result = new List<OrdInfModel>();

            if (!(allOdrInf?.Count > 0))
            {
                return result;
            }

            var itemCds = allOdrInfDetails?.Select(od => od.ItemCd ?? string.Empty);
            var ipnCds = allOdrInfDetails?.Select(od => od.IpnCd ?? string.Empty);
            var sinKouiKbns = allOdrInfDetails?.Select(od => od.SinKouiKbn);
            var tenMsts = NoTrackingDataContext.TenMsts.Where(t => t.HpId == hpId && (t.StartDate <= sinDateMin && t.EndDate >= sinDateMax) && (itemCds != null && itemCds.Contains(t.ItemCd))).ToList();
            var kensaMsts = NoTrackingDataContext.KensaMsts.Where(t => t.HpId == hpId).ToList();
            var yakkas = NoTrackingDataContext.IpnMinYakkaMsts.Where(t => t.HpId == hpId && (t.StartDate <= sinDateMax && t.EndDate >= sinDateMax) && (ipnCds != null && ipnCds.Contains(t.IpnNameCd))).ToList();
            var ipnKasanExcludes = NoTrackingDataContext.ipnKasanExcludes.Where(t => t.HpId == hpId && (t.StartDate <= sinDateMin && t.EndDate >= sinDateMax)).ToList();
            var ipnKasanExcludeItems = NoTrackingDataContext.ipnKasanExcludeItems.Where(t => t.HpId == hpId && (t.StartDate <= sinDateMin && t.EndDate >= sinDateMax)).ToList();
            var ipnNameMsts = isHistory ? null : NoTrackingDataContext.IpnNameMsts.Where(ipn => (ipnCds != null && ipnCds.Contains(ipn.IpnNameCd)) && ipn.HpId == hpId && ipn.StartDate <= sinDateMin && ipn.EndDate >= sinDateMax).ToList();
            var ipnKansanMsts = isHistory ? null : NoTrackingDataContext.IpnKasanMsts.Where(ipn => (ipnCds != null && ipnCds.Contains(ipn.IpnNameCd)) && ipn.HpId == hpId && ipn.StartDate <= sinDateMin && ipn.IsDeleted == 0).ToList();
            var listYohoSets = isHistory ? null : NoTrackingDataContext.YohoSetMsts.Where(y => y.HpId == hpId && y.IsDeleted == 0 && y.UserId == userId).ToList();
            var itemCdYohos = isHistory ? null : listYohoSets?.Select(od => od.ItemCd ?? string.Empty);

            var tenMstYohos = isHistory ? null : NoTrackingDataContext.TenMsts.Where(t => t.HpId == hpId && t.IsNosearch == 0 && t.StartDate <= sinDateMin && t.EndDate >= sinDateMax && (sinKouiKbns != null && sinKouiKbns.Contains(t.SinKouiKbn)) && (itemCdYohos != null && itemCdYohos.Contains(t.ItemCd))).ToList();

            var checkKensaIrai = NoTrackingDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == 2019 && p.GrpEdaNo == 0);
            var kensaIrai = checkKensaIrai?.Val ?? 0;
            var checkKensaIraiCondition = NoTrackingDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == 2019 && p.GrpEdaNo == 1);
            var kensaIraiCondition = checkKensaIraiCondition?.Val ?? 0;

            var odrInfs = from odrInf in allOdrInf
                          join user in NoTrackingDataContext.UserMsts.Where(u => u.HpId == hpId)
                          on odrInf.CreateId equals user.UserId into odrUsers
                          from odrUser in odrUsers.DefaultIfEmpty()
                          select ConvertToModel(odrInf, odrUser?.Sname ?? string.Empty);
            var obj = new object();
            Parallel.ForEach(odrInfs, rpOdrInf =>
            {

                var odrDetailModels = new List<OrdInfDetailModel>();

                var odrInfDetails = allOdrInfDetails?.Where(detail => detail.RpNo == rpOdrInf.RpNo && detail.RpEdaNo == rpOdrInf.RpEdaNo)
                    .ToList();

                if (odrInfDetails?.Count > 0)
                {
                    var usage = odrInfDetails.FirstOrDefault(d => d.YohoKbn == 1 || d.ItemCd == ItemCdConst.TouyakuChozaiNaiTon || d.ItemCd == ItemCdConst.TouyakuChozaiGai);

                    var objDetail = new object();
                    Parallel.For(0, odrInfDetails.Count, index =>
                    {
                        var odrInfDetail = odrInfDetails[index];

                        var tenMst = tenMsts.FirstOrDefault(t => t.ItemCd == odrInfDetail.ItemCd);
                        var ten = tenMst?.Ten ?? 0;
                        if (tenMst != null && string.IsNullOrEmpty(odrInfDetail.IpnCd)) odrInfDetail.IpnCd = tenMst.IpnNameCd;

                        var kensaMst = tenMst == null ? null : kensaMsts.FirstOrDefault(k => k.KensaItemCd == tenMst.KensaItemCd && k.KensaItemSeqNo == tenMst.KensaItemSeqNo);

                        var ipnNameMst = ipnNameMsts?.FirstOrDefault(ipn => ipn.IpnNameCd == odrInfDetail.IpnCd);
                        if (tenMst != null && string.IsNullOrEmpty(odrInfDetail.IpnCd) && ipnNameMst != null) odrInfDetail.IpnName = ipnNameMst.IpnName;

                        var kasan = ipnKansanMsts?.FirstOrDefault(ipn => ipn.IpnNameCd == odrInfDetail.IpnCd);

                        var alternationIndex = index % 2;
                        var bunkatuKoui = 0;
                        if (odrInfDetail.ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu)
                        {
                            bunkatuKoui = usage?.SinKouiKbn ?? 0;
                        }

                        var yakka = yakkas.FirstOrDefault(p => p.IpnNameCd == odrInfDetail.IpnCd)?.Yakka ?? 0;

                        var isGetPriceInYakka = IsGetPriceInYakka(tenMst, ipnKasanExcludes, ipnKasanExcludeItems);

                        int kensaGaichu = GetKensaGaichu(odrInfDetail, tenMst, rpOdrInf.InoutKbn, rpOdrInf.OdrKouiKbn, kensaMst, (int)kensaIraiCondition, (int)kensaIrai);
                        var odrInfDetailModel = ConvertToDetailModel(odrInfDetail, yakka, ten, isGetPriceInYakka, kensaGaichu, bunkatuKoui, rpOdrInf.InoutKbn, alternationIndex, tenMst?.OdrTermVal ?? 0, tenMst?.CnvTermVal ?? 0, tenMst?.YjCd ?? string.Empty, tenMst?.MasterSbt ?? string.Empty, isHistory ? new List<YohoSetMstModel>() : GetListYohoSetMstModelByUserID(listYohoSets ?? new List<YohoSetMst>(), tenMstYohos?.Where(t => t.SinKouiKbn == odrInfDetail.SinKouiKbn)?.ToList() ?? new List<TenMst>()), kasan?.Kasan1 ?? 0, kasan?.Kasan2 ?? 0, tenMst?.CnvUnitName ?? string.Empty, tenMst?.OdrUnitName ?? string.Empty, kensaMst?.CenterItemCd1 ?? string.Empty, kensaMst?.CenterItemCd2 ?? string.Empty);
                        lock (objDetail)
                        {
                            odrDetailModels.Add(odrInfDetailModel);
                        }
                    });
                    lock (obj)
                    {
                        rpOdrInf.OrdInfDetails.AddRange(odrDetailModels);
                        result.Add(rpOdrInf);
                    }
                }
            });

            return result;
        }

        private static OrdInfModel ConvertToModel(OdrInf ordInf, string createName = "", string updateName = "")
        {
            return new OrdInfModel(ordInf.HpId,
                        ordInf.RaiinNo,
                        ordInf.RpNo,
                        ordInf.RpEdaNo,
                        ordInf.PtId,
                        ordInf.SinDate,
                        ordInf.HokenPid,
                        ordInf.OdrKouiKbn,
                        ordInf.RpName ?? string.Empty,
                        ordInf.InoutKbn,
                        ordInf.SikyuKbn,
                        ordInf.SyohoSbt,
                        ordInf.SanteiKbn,
                        ordInf.TosekiKbn,
                        ordInf.DaysCnt,
                        ordInf.SortNo,
                        ordInf.IsDeleted,
                        ordInf.Id,
                        new List<OrdInfDetailModel>(),
                        ordInf.CreateDate,
                        ordInf.CreateId,
                        createName,
                        ordInf.UpdateDate,
                        ordInf.UpdateId,
                        updateName
                   );
        }

        private OrdInfDetailModel ConvertToDetailModel(OdrInfDetail ordInfDetail, double yakka = 0, double ten = 0, bool isGetPriceInYakka = false, int kensaGaichu = 0, int bunkatuKoui = 0, int inOutKbn = 0, int alternationIndex = 0, double odrTermVal = 0, double cnvTermVal = 0, string yjCd = "", string masterSbt = "", List<YohoSetMstModel>? yohoSets = null, int kasan1 = 0, int kasan2 = 0, string cnvUnitName = "", string odrUnitName = "", string centerItemCd1 = "", string centerItemCd2 = "")
        {
            return new OrdInfDetailModel(
                            ordInfDetail.HpId,
                            ordInfDetail.RaiinNo,
                            ordInfDetail.RpNo,
                            ordInfDetail.RpEdaNo,
                            ordInfDetail.RowNo,
                            ordInfDetail.PtId,
                            ordInfDetail.SinDate,
                            ordInfDetail.SinKouiKbn,
                            ordInfDetail.ItemCd ?? string.Empty,
                            ordInfDetail.ItemName ?? string.Empty,
                            ordInfDetail.Suryo,
                            ordInfDetail.UnitName ?? string.Empty,
                            ordInfDetail.UnitSBT,
                            ordInfDetail.TermVal,
                            ordInfDetail.KohatuKbn,
                            ordInfDetail.SyohoKbn,
                            ordInfDetail.SyohoLimitKbn,
                            ordInfDetail.DrugKbn,
                            ordInfDetail.YohoKbn,
                            ordInfDetail.Kokuji1 ?? string.Empty,
                            ordInfDetail.Kokiji2 ?? string.Empty,
                            ordInfDetail.IsNodspRece,
                            ordInfDetail.IpnCd ?? string.Empty,
                            ordInfDetail.IpnName ?? string.Empty,
                            ordInfDetail.JissiKbn,
                            ordInfDetail.JissiDate ?? DateTime.MinValue,
                            ordInfDetail.JissiId,
                            ordInfDetail.JissiMachine ?? string.Empty,
                            ordInfDetail.ReqCd ?? string.Empty,
                            ordInfDetail.Bunkatu ?? string.Empty,
                            ordInfDetail.CmtName ?? string.Empty,
                            ordInfDetail.CmtOpt ?? string.Empty,
                            ordInfDetail.FontColor ?? string.Empty,
                            ordInfDetail.CommentNewline,
                            masterSbt,
                            inOutKbn,
                            yakka,
                            isGetPriceInYakka,
                            0,
                            0,
                            ten,
                            bunkatuKoui,
                            alternationIndex,
                            kensaGaichu,
                            odrTermVal,
                            cnvTermVal,
                            yjCd,
                            yohoSets ?? new List<YohoSetMstModel>(),
                            kasan1,
                            kasan2,
                            cnvUnitName,
                            odrUnitName,
                            centerItemCd1,
                            centerItemCd2,
                            ordInfDetail.CmtOpt ?? string.Empty
                );
        }

        private static bool IsGetPriceInYakka(TenMst? tenMst, List<IpnKasanExclude> ipnKasanExcludes, List<IpnKasanExcludeItem> ipnKasanExcludeItems)
        {
            if (tenMst == null) return false;

            var ipnKasanExclude = ipnKasanExcludes.FirstOrDefault(u => u.IpnNameCd == tenMst.IpnNameCd);

            var ipnKasanExcludeItem = ipnKasanExcludeItems.FirstOrDefault(u => u.ItemCd == tenMst.ItemCd);

            return ipnKasanExclude == null && ipnKasanExcludeItem == null;
        }

        private static int GetKensaGaichu(OdrInfDetail? odrInfDetail, TenMst? tenMst, int inOutKbn, int odrKouiKbn, KensaMst? kensaMst, int kensaIraiCondition, int kensaIrai)
        {
            if (string.IsNullOrEmpty(odrInfDetail?.ItemCd) &&
                   string.IsNullOrEmpty(odrInfDetail?.ItemName?.Trim()) &&
                   odrInfDetail?.SinKouiKbn == 0)
            {
                return KensaGaichuTextConst.NONE;
            }

            if (odrInfDetail?.SinKouiKbn == 61 || odrInfDetail?.SinKouiKbn == 64)
            {
                bool kensaCondition;
                if (kensaIraiCondition == 0)
                {
                    kensaCondition = (odrInfDetail.SinKouiKbn == 61 || odrInfDetail.SinKouiKbn == 64) && odrInfDetail.Kokuji1 != "7" && odrInfDetail.Kokuji1 != "9";
                }
                else
                {
                    kensaCondition = odrInfDetail.SinKouiKbn == 61 && odrInfDetail.Kokuji1 != "7" && odrInfDetail.Kokuji1 != "9" && (tenMst == null ? 0 : tenMst.HandanGrpKbn) != 6;
                }

                if (kensaCondition && inOutKbn == 1)
                {
                    int kensaSetting = kensaIrai;
                    if (kensaMst == null)
                    {
                        if (kensaSetting > 0)
                        {
                            return KensaGaichuTextConst.GAICHU_NONE;
                        }
                    }
                    else if (string.IsNullOrEmpty(kensaMst.CenterItemCd1)
                        && string.IsNullOrEmpty(kensaMst.CenterItemCd2) && kensaSetting > 1)
                    {
                        return KensaGaichuTextConst.GAICHU_NOT_SET;
                    }
                }
            }

            if (!string.IsNullOrEmpty(odrInfDetail?.ItemName) && string.IsNullOrEmpty(odrInfDetail.ItemCd))
            {
                if (inOutKbn == 1 && (odrKouiKbn >= 20 && odrKouiKbn <= 23) || odrKouiKbn == 28)
                {
                    if (odrInfDetail.IsNodspRece == 0)
                    {
                        return KensaGaichuTextConst.IS_DISPLAY_RECE_ON;
                    }
                }
                else
                {
                    if (odrInfDetail.IsNodspRece == 1)
                    {
                        return KensaGaichuTextConst.IS_DISPLAY_RECE_OFF;
                    }
                }
            }
            return KensaGaichuTextConst.NONE;
        }

        private static List<YohoSetMstModel> GetListYohoSetMstModelByUserID(List<YohoSetMst> listYohoSetMst, List<TenMst> listTenMst)
        {
            var query = from yoho in listYohoSetMst
                        join ten in listTenMst on yoho.ItemCd?.Trim() equals ten.ItemCd.Trim()
                        select new
                        {
                            Yoho = yoho,
                            ItemName = ten.Name,
                            ten.YohoKbn
                        };

            return query.OrderBy(u => u.Yoho.SortNo).AsEnumerable().Select(u => new YohoSetMstModel(u.ItemName, u.YohoKbn, u.Yoho?.SetId ?? 0, u.Yoho?.UserId ?? 0, u.Yoho?.ItemCd ?? string.Empty)).ToList();
        }

        public IEnumerable<ApproveInfModel> GetApproveInf(int hpId, long ptId, bool isDeleted, List<long> raiinNos)
        {
            var result = NoTrackingDataContext.ApprovalInfs.Where(a => a.HpId == hpId && a.PtId == ptId && (isDeleted || a.IsDeleted == 0) && raiinNos.Contains(a.RaiinNo));
            return result.Select(
                    r => new ApproveInfModel(
                            r.Id,
                            r.HpId,
                            r.PtId,
                            r.SinDate,
                            r.RaiinNo,
                            r.SeqNo,
                            r.IsDeleted,
                            GetDisplayApproveInf(r.UpdateId, r.UpdateDate)
                        )
                );
        }

        private string GetDisplayApproveInf(int updateId, DateTime? updateDate)
        {
            string result = string.Empty;
            string info = string.Empty;

            string docName = NoTrackingDataContext.UserMsts.FirstOrDefault(u => u.Id == updateId)?.Sname ?? string.Empty;
            if (!string.IsNullOrEmpty(docName))
            {
                info += docName;
            }

            string approvalDateTime = string.Empty;
            if (updateDate != null && updateDate.Value != DateTime.MinValue)
            {
                approvalDateTime = " " + updateDate.Value.ToString("yyyy/MM/dd HH:mm");
            }

            info += approvalDateTime;

            if (!string.IsNullOrEmpty(info))
            {
                result += "（承認: " + info + "）";
            }

            return result;
        }

        public List<Tuple<string, string>> GetIpnMst(int hpId, int sinDateMin, int sinDateMax, List<string> ipnCds)
        {
            var ipnNameMsts = NoTrackingDataContext.IpnNameMsts.Where(ipn => (ipnCds != null && ipnCds.Contains(ipn.IpnNameCd)) && ipn.HpId == hpId && ipn.StartDate <= sinDateMin && ipn.EndDate >= sinDateMax).Select(i => new Tuple<string, string>(i.IpnNameCd, i.IpnName ?? string.Empty)).ToList();

            return ipnNameMsts;
        }

        public OrdInfModel Read(int ordId)
        {
            throw new NotImplementedException();
        }

        public void Update(OrdInfModel ord)
        {
            throw new NotImplementedException();
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
