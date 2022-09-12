using Domain.Models.SuperSetDetail;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System.Text;

namespace Infrastructure.Repositories;

public class SuperSetDetailRepository : ISuperSetDetailRepository
{
    private readonly TenantNoTrackingDataContext _tenantNoTrackingDataContext;
    private readonly TenantDataContext _tenantDataContext;
    public SuperSetDetailRepository(ITenantProvider tenantProvider)
    {
        _tenantNoTrackingDataContext = tenantProvider.GetNoTrackingDataContext();
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
    }

    public SuperSetDetailModel GetSuperSetDetail(int hpId, int setCd, int sindate)
    {
        return new SuperSetDetailModel(
                GetSetByomeiList(hpId, setCd),
                GetSetKarteInfModel(hpId, setCd),
                GetSetOrdInfModel(hpId, setCd, sindate)
            );
    }

    #region GetSetByomeiList
    private List<SetByomeiModel> GetSetByomeiList(int hpId, int setCd)
    {
        var listByomeis = _tenantNoTrackingDataContext.SetByomei.Where(odr => odr.HpId == hpId && odr.SetCd == setCd && odr.IsDeleted != 1).ToList();

        // get list ByomeiMst
        List<string> codeLists = new();
        foreach (var item in listByomeis)
        {
            codeLists.AddRange(GetCodeLists(item));
        }
        var byomeiMstList = _tenantNoTrackingDataContext.ByomeiMsts.Where(b => codeLists.Contains(b.ByomeiCd)).ToList();

        var listSetByomeiModels = listByomeis.Select(mst => ConvertSetByomeiModel(mst, byomeiMstList)).ToList();
        return listSetByomeiModels;
    }

    private SetByomeiModel ConvertSetByomeiModel(SetByomei mst, List<ByomeiMst> byomeiMstList)
    {
        bool isSyobyoKbn = mst.SyobyoKbn == 1;
        int sikkanKbn = mst.SikkanKbn;
        int nanByoCd = mst.NanbyoCd;
        string fullByomei = mst.Byomei ?? string.Empty;
        bool isDspRece = mst.IsNodspRece == 0;
        bool isDspKarte = mst.IsNodspKarte == 0;
        string byomeiCmt = mst.HosokuCmt ?? string.Empty;
        string byomeiCd = mst.ByomeiCd ?? string.Empty;
        var codeLists = GetCodeLists(mst);
        //prefix and suffix
        var prefixSuffixList = codeLists?.Select(code => new PrefixSuffixModel(code, byomeiMstList.FirstOrDefault(item => item.ByomeiCd.Equals(code))?.Byomei ?? string.Empty)).ToList();
        bool isSuspected = false;
        if (codeLists != null)
        {
            isSuspected = codeLists.Any(c => c == "8002");
        }
        return new SetByomeiModel(
                isSyobyoKbn,
                sikkanKbn,
                nanByoCd,
                fullByomei,
                isSuspected,
                isDspRece,
                isDspKarte,
                byomeiCmt,
                byomeiCd,
                prefixSuffixList ?? new List<PrefixSuffixModel>()
            );
    }

    private List<string> GetCodeLists(SetByomei mst)
    {
        var codeLists = new List<string>()
            {
                mst.SyusyokuCd1 ?? string.Empty,
                mst.SyusyokuCd2 ?? string.Empty,
                mst.SyusyokuCd3 ?? string.Empty,
                mst.SyusyokuCd4 ?? string.Empty,
                mst.SyusyokuCd5 ?? string.Empty,
                mst.SyusyokuCd6 ?? string.Empty,
                mst.SyusyokuCd7 ?? string.Empty,
                mst.SyusyokuCd8 ?? string.Empty,
                mst.SyusyokuCd9 ?? string.Empty,
                mst.SyusyokuCd10 ?? string.Empty,
                mst.SyusyokuCd11 ?? string.Empty,
                mst.SyusyokuCd12 ?? string.Empty,
                mst.SyusyokuCd13 ?? string.Empty,
                mst.SyusyokuCd14 ?? string.Empty,
                mst.SyusyokuCd15 ?? string.Empty,
                mst.SyusyokuCd16 ?? string.Empty,
                mst.SyusyokuCd17 ?? string.Empty,
                mst.SyusyokuCd18 ?? string.Empty,
                mst.SyusyokuCd19 ?? string.Empty,
                mst.SyusyokuCd20 ?? string.Empty,
                mst.SyusyokuCd21 ?? string.Empty
            };
        return codeLists?.Where(c => c != string.Empty).ToList() ?? new List<string>();
    }

    #endregion

    #region GetSetKarteInfModelList
    private SetKarteInfModel GetSetKarteInfModel(int hpId, int setCd)
    {
        var setKarteInf = _tenantNoTrackingDataContext.SetKarteInf.FirstOrDefault(odr => odr.HpId == hpId && odr.SetCd == setCd && odr.IsDeleted != 1) ?? new SetKarteInf();
        return new SetKarteInfModel(
                setKarteInf.HpId,
                setKarteInf.SetCd,
                setKarteInf.RichText == null ? string.Empty : Encoding.UTF8.GetString(setKarteInf.RichText)
            );
    }

    #endregion

    #region  
    private List<SetOrdInfModel> GetSetOrdInfModel(int hpId, int setCd, int sindate)
    {
        var allSetOdrInfs = _tenantNoTrackingDataContext.SetOdrInf.Where(order => order.HpId == hpId && order.SetCd == setCd && order.IsDeleted != 1)?.ToList();
        var allSetOdrInfDetails = _tenantNoTrackingDataContext.SetOdrInfDetail.Where(detail => detail.HpId == hpId && detail.SetCd == setCd)?.ToList();
        var result = new List<SetOrdInfModel>();

        var itemCds = allSetOdrInfDetails?.Select(detail => detail.ItemCd);
        var ipnCds = allSetOdrInfDetails?.Select(detail => detail.IpnCd);
        var tenMsts = _tenantDataContext.TenMsts.Where(t => t.HpId == hpId && t.StartDate <= sindate && t.EndDate >= sindate && (itemCds != null && itemCds.Contains(t.ItemCd))).ToList();
        var kensaMsts = _tenantDataContext.KensaMsts.Where(kensa => kensa.HpId == hpId && kensa.IsDelete != 1).ToList();
        var yakkas = _tenantDataContext.IpnMinYakkaMsts.Where(t => t.HpId == hpId && (t.StartDate <= sinDateMax && t.EndDate >= sinDateMax) && (ipnCds != null && ipnCds.Contains(t.IpnNameCd))).ToList();
        var ipnKasanExcludes = _tenantDataContext.ipnKasanExcludes.Where(t => t.HpId == hpId && (t.StartDate <= sinDateMin && t.EndDate >= sinDateMax)).ToList();
        var ipnKasanExcludeItems = _tenantDataContext.ipnKasanExcludeItems.Where(t => t.HpId == hpId && (t.StartDate <= sinDateMin && t.EndDate >= sinDateMax)).ToList();

        var checkKensaIrai = _tenantDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == 2019 && p.GrpEdaNo == 0);
        var kensaIrai = checkKensaIrai?.Val ?? 0;
        var checkKensaIraiCondition = _tenantDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == 2019 && p.GrpEdaNo == 1);
        var kensaIraiCondition = checkKensaIraiCondition?.Val ?? 0;



        return result;
    }
    #endregion
}
