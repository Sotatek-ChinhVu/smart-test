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
    private const string SUSPECTED = "の疑い";
    private const string SUSPECTED_CD = "8002";
    private const string FREE_WORD = "0000999";
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
                mst.Id,
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

    public int SaveSuperSetDetail(int setCd, int userId, int hpId, SuperSetDetailModel superSetDetailModel)
    {
        int status = 0;
        if (!SaveSetByomei(setCd, userId, hpId, superSetDetailModel.SetByomeiList))
        {
            status = 1;
        }
        return status;
    }

    #region SaveSetByomei
    public bool SaveSetByomei(int setCd, int userId, int hpId, List<SetByomeiModel> setByomeiModels)
    {
        bool status = false;
        try
        {
            var listOldByomeis = _tenantDataContext.SetByomei.Where(mst => mst.SetCd == setCd && mst.HpId == hpId && mst.IsDeleted != 1).ToList();

            // Add new SetByomei
            var listAddNewByomeis = setByomeiModels.Where(model => model.Id == 0).Select(model => ConvertToSetByomeiEntity(setCd, userId, hpId, new SetByomei(), model)).ToList();
            if (listAddNewByomeis != null && listAddNewByomeis.Count > 0)
            {
                _tenantDataContext.AddRange(listAddNewByomeis);
            }

            // Update SetByomei
            foreach (var model in setByomeiModels.Where(model => model.Id != 0).ToList())
            {
                var mst = listOldByomeis.FirstOrDefault(mst => mst.Id == model.Id);
                if (mst != null)
                {
                    mst = ConvertToSetByomeiEntity(setCd, userId, hpId, mst, model) ?? new SetByomei();
                }
            }

            // Delete SetByomei
            var listByomeiDelete = listOldByomeis.Where(mst => !setByomeiModels.Select(model => model.Id).ToList().Contains(mst.Id)).ToList();
            foreach (var mst in listByomeiDelete)
            {
                mst.IsDeleted = 1;
                mst.UpdateDate = DateTime.UtcNow;
                mst.UpdateId = userId;
            }
            _tenantDataContext.SaveChanges();
            status = true;
            return status;
        }
        catch (Exception)
        {
            return status;
        }
    }

    private SetByomei ConvertToSetByomeiEntity(int setCd, int userId, int hpId, SetByomei mst, SetByomeiModel model)
    {
        mst.HpId = hpId;
        mst.SetCd = setCd;
        mst.SyobyoKbn = model.IsSyobyoKbn ? 1 : 0;
        mst.SikkanKbn = model.SikkanKbn;
        mst.NanbyoCd = model.NanByoCd;
        mst.IsNodspRece = model.IsDspRece ? 0 : 1;
        mst.IsNodspKarte = model.IsDspKarte ? 0 : 1;
        mst.HosokuCmt = model.ByomeiCmt ?? string.Empty;
        if (model.FullByomei.StartsWith("//"))
        {
            mst.Byomei = model.FullByomei.Substring(2) ?? string.Empty;
            mst.ByomeiCd = FREE_WORD;
        }
        else
        {
            mst.Byomei = model.FullByomei ?? string.Empty;
            mst.ByomeiCd = model.ByomeiCd ?? string.Empty;
        }

        var listPrefixSuffix = mst.ByomeiCd != FREE_WORD ? model.PrefixSuffixList : new();
        var itemSuspected = listPrefixSuffix.FirstOrDefault(item => item.Code.Equals(SUSPECTED_CD));

        if (itemSuspected != null)
        {
            listPrefixSuffix.Remove(itemSuspected);
        }
        mst.SyusyokuCd1 = listPrefixSuffix.Count > 0 ? listPrefixSuffix[0].Code : string.Empty;
        mst.SyusyokuCd2 = listPrefixSuffix.Count > 1 ? listPrefixSuffix[1].Code : string.Empty;
        mst.SyusyokuCd3 = listPrefixSuffix.Count > 2 ? listPrefixSuffix[2].Code : string.Empty;
        mst.SyusyokuCd4 = listPrefixSuffix.Count > 3 ? listPrefixSuffix[3].Code : string.Empty;
        mst.SyusyokuCd5 = listPrefixSuffix.Count > 4 ? listPrefixSuffix[4].Code : string.Empty;
        mst.SyusyokuCd6 = listPrefixSuffix.Count > 5 ? listPrefixSuffix[5].Code : string.Empty;
        mst.SyusyokuCd7 = listPrefixSuffix.Count > 6 ? listPrefixSuffix[6].Code : string.Empty;
        mst.SyusyokuCd8 = listPrefixSuffix.Count > 7 ? listPrefixSuffix[7].Code : string.Empty;
        mst.SyusyokuCd9 = listPrefixSuffix.Count > 8 ? listPrefixSuffix[8].Code : string.Empty;
        mst.SyusyokuCd10 = listPrefixSuffix.Count > 9 ? listPrefixSuffix[9].Code : string.Empty;
        mst.SyusyokuCd11 = listPrefixSuffix.Count > 10 ? listPrefixSuffix[10].Code : string.Empty;
        mst.SyusyokuCd12 = listPrefixSuffix.Count > 11 ? listPrefixSuffix[11].Code : string.Empty;
        mst.SyusyokuCd13 = listPrefixSuffix.Count > 12 ? listPrefixSuffix[12].Code : string.Empty;
        mst.SyusyokuCd14 = listPrefixSuffix.Count > 13 ? listPrefixSuffix[13].Code : string.Empty;
        mst.SyusyokuCd15 = listPrefixSuffix.Count > 14 ? listPrefixSuffix[14].Code : string.Empty;
        mst.SyusyokuCd16 = listPrefixSuffix.Count > 15 ? listPrefixSuffix[15].Code : string.Empty;
        mst.SyusyokuCd17 = listPrefixSuffix.Count > 16 ? listPrefixSuffix[16].Code : string.Empty;
        mst.SyusyokuCd18 = listPrefixSuffix.Count > 17 ? listPrefixSuffix[17].Code : string.Empty;
        mst.SyusyokuCd19 = listPrefixSuffix.Count > 18 ? listPrefixSuffix[18].Code : string.Empty;
        mst.SyusyokuCd20 = listPrefixSuffix.Count > 19 ? listPrefixSuffix[19].Code : string.Empty;
        mst.SyusyokuCd21 = listPrefixSuffix.Count > 20 ? listPrefixSuffix[20].Code : string.Empty;

        if (model.IsSuspected && mst.ByomeiCd != FREE_WORD && itemSuspected == null)
        {
            mst.SyusyokuCd21 = SUSPECTED_CD;
        }
        else if (!model.IsSuspected && mst.ByomeiCd != FREE_WORD)
        {
            mst.Byomei = mst.Byomei?.Replace(SUSPECTED, string.Empty);
        }

        if (model.Id == 0)
        {
            mst.CreateDate = DateTime.UtcNow;
            mst.CreateId = userId;
        }
        mst.UpdateDate = DateTime.UtcNow;
        mst.UpdateId = userId;
        return mst;
    }
    #endregion
}
