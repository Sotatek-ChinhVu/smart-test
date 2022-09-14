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

    public int SaveSuperSetDetail(int setCd, int userId, SuperSetDetailModel superSetDetailModel)
    {
        int status = 0;
        if (!SaveSetByomei(setCd, userId, superSetDetailModel.SetByomeiList))
        {
            status = 1;
        }
        return status;
    }

    #region SaveSetByomei
    public bool SaveSetByomei(int setCd, int userId, List<SetByomeiModel> setByomeiModels)
    {
        bool status = false;
        try
        {
            var listOldByomeis = _tenantDataContext.SetByomei.Where(mst => mst.SetCd == setCd && mst.IsDeleted != 1).ToList();

            // Add new SetByomei
            var listAddNewByomeis = setByomeiModels.Where(model => model.Id == 0).Select(model => ConvertToNewSetByomeiEntity(setCd, userId, new SetByomei(), model)).ToList();
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
                    mst = ConvertToNewSetByomeiEntity(setCd, userId, mst, model) ?? new SetByomei();
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

    private SetByomei ConvertToNewSetByomeiEntity(int setCd, int userId, SetByomei mst, SetByomeiModel model)
    {
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
        mst.SyusyokuCd1 = model.PrefixSuffixList.Count > 0 ? model.PrefixSuffixList[0].Code : string.Empty;
        mst.SyusyokuCd2 = model.PrefixSuffixList.Count > 1 ? model.PrefixSuffixList[1].Code : string.Empty;
        mst.SyusyokuCd3 = model.PrefixSuffixList.Count > 2 ? model.PrefixSuffixList[2].Code : string.Empty;
        mst.SyusyokuCd4 = model.PrefixSuffixList.Count > 3 ? model.PrefixSuffixList[3].Code : string.Empty;
        mst.SyusyokuCd5 = model.PrefixSuffixList.Count > 4 ? model.PrefixSuffixList[4].Code : string.Empty;
        mst.SyusyokuCd6 = model.PrefixSuffixList.Count > 5 ? model.PrefixSuffixList[5].Code : string.Empty;
        mst.SyusyokuCd7 = model.PrefixSuffixList.Count > 6 ? model.PrefixSuffixList[6].Code : string.Empty;
        mst.SyusyokuCd8 = model.PrefixSuffixList.Count > 7 ? model.PrefixSuffixList[7].Code : string.Empty;
        mst.SyusyokuCd9 = model.PrefixSuffixList.Count > 8 ? model.PrefixSuffixList[8].Code : string.Empty;
        mst.SyusyokuCd10 = model.PrefixSuffixList.Count > 9 ? model.PrefixSuffixList[9].Code : string.Empty;
        mst.SyusyokuCd11 = model.PrefixSuffixList.Count > 10 ? model.PrefixSuffixList[10].Code : string.Empty;
        mst.SyusyokuCd12 = model.PrefixSuffixList.Count > 11 ? model.PrefixSuffixList[11].Code : string.Empty;
        mst.SyusyokuCd13 = model.PrefixSuffixList.Count > 12 ? model.PrefixSuffixList[12].Code : string.Empty;
        mst.SyusyokuCd14 = model.PrefixSuffixList.Count > 13 ? model.PrefixSuffixList[13].Code : string.Empty;
        mst.SyusyokuCd15 = model.PrefixSuffixList.Count > 14 ? model.PrefixSuffixList[14].Code : string.Empty;
        mst.SyusyokuCd16 = model.PrefixSuffixList.Count > 15 ? model.PrefixSuffixList[15].Code : string.Empty;
        mst.SyusyokuCd17 = model.PrefixSuffixList.Count > 16 ? model.PrefixSuffixList[16].Code : string.Empty;
        mst.SyusyokuCd18 = model.PrefixSuffixList.Count > 17 ? model.PrefixSuffixList[17].Code : string.Empty;
        mst.SyusyokuCd19 = model.PrefixSuffixList.Count > 18 ? model.PrefixSuffixList[18].Code : string.Empty;
        mst.SyusyokuCd20 = model.PrefixSuffixList.Count > 19 ? model.PrefixSuffixList[19].Code : string.Empty;
        mst.SyusyokuCd21 = model.PrefixSuffixList.Count > 20 ? model.PrefixSuffixList[20].Code : string.Empty;
        if (model.IsSuspected)
        {
            mst.SyusyokuCd21 = SUSPECTED_CD;
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
