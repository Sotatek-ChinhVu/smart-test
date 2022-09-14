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

    public SuperSetDetailModel GetSuperSetDetail(int hpId, int setCd)
    {
        return new SuperSetDetailModel(
                GetSetByomeiList(hpId, setCd),
                GetSetKarteInfModel(hpId, setCd)
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

    public bool SaveSetByomei(List<SetByomeiModel> setByomeis)
    {
        bool status = false;
        try
        {
            var listAddNews = setByomeis.Where(byomei => byomei.Id == 0).ToList();
            var listUpdates = _tenantDataContext.
                setByomeis.Where(byomei => !listAddNews.Contains(byomei)).ToList();
            if (listAddNews != null && listAddNews.Count > 0)
            {
                _tenantDataContext.AddRange(listAddNews);
            }
            _tenantDataContext.UpdateRange(listUpdates);
            _tenantDataContext.SaveChanges();
            return status;
        }
        catch (Exception)
        {
            return status;
        }
    }
}
