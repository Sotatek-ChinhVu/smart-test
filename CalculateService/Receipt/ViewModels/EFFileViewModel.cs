using CalculateService.Receipt.DB.Finder;
using CalculateService.Ika.DB.Finder;
using Helper.Constants;
using CalculateService.Receipt.Models;
using CalculateService.Receipt.Constants;
using CalculateService.Interface;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace CalculateService.Receipt.ViewModels;

public class EFFileViewModel : IDisposable
{
    int hpId = Session.HospitalID;

    PtFinder _ptFinder;
    HokenFinder _hokenFinder;
    ReceMasterFinder _receMasterFinder;
    DB.Finder.KaikeiFinder _kaikeiFinder;

    private readonly ITenantProvider _tenantProvider;
    private readonly TenantDataContext _tenantDataContext;
    private readonly ISystemConfigProvider _systemConfigProvider;
    private readonly IEmrLogger _emrLogger;
    private SanteiFinder _santeiFinder;

    public EFFileViewModel(ITenantProvider tenantProvider, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
    {
        _tenantProvider = tenantProvider;
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
        _systemConfigProvider = systemConfigProvider;
        _emrLogger = emrLogger;

        _ptFinder = new PtFinder(_tenantDataContext, _systemConfigProvider, _emrLogger);
        _hokenFinder = new HokenFinder(_tenantDataContext, _systemConfigProvider, _emrLogger);
        _receMasterFinder = new ReceMasterFinder(_tenantDataContext, _systemConfigProvider, _emrLogger);
        _kaikeiFinder = new KaikeiFinder(_tenantDataContext, _systemConfigProvider, _emrLogger);
        _santeiFinder = new SanteiFinder(_tenantDataContext, _systemConfigProvider, _emrLogger);
    }

    /// <summary>
    /// Eファイルを作成する
    /// </summary>
    /// <returns></returns>
    public string GetEFileData(int sinYm, bool includeTester)
    {
        const string conFncName = nameof(GetEFileData);
        _emrLogger.WriteLogStart(this, conFncName,
            $"sinYm:{sinYm}");

        // 対象を取得
        // RECE_INF HOKEN_KBN in (1,2) 診療月ベースで取得
        List<ReceInfModel> receInfModels =
            _ptFinder.FindEFReceInf(hpId, sinYm, includeTester);

        if (receInfModels != null)
        {
            receInfModels = receInfModels.OrderBy(p => p.PtNum).ThenBy(p => p.HokenId).ToList();
        }

        List<Ika.Models.SinRpInfModel> sinRpInfModels = _santeiFinder.FindSinRpInfDataForEF(hpId, sinYm, includeTester);
        List<Ika.Models.SinKouiModel> sinKouiModels = _santeiFinder.FindSinKouiDataForEF(hpId, sinYm, includeTester);
        List<Ika.Models.SinKouiDetailModel> sinKouiDetailModels = _santeiFinder.FindSinKouiDetailDataForEF(hpId, sinYm, includeTester);
        List<Ika.Models.SinKouiCountModel> sinKouiCountModels = _santeiFinder.FindSinKouiCountDataForEF(hpId, sinYm, includeTester);

        List<EFileDataModel> _eFileViewModels = new List<EFileDataModel>();
        List<EFRaiinInfModel> raiinInfs =
            _receMasterFinder.FindRaiinDatas(hpId, sinYm);
        foreach (ReceInfModel receInfModel in receInfModels)
        {
            // EFファイル用データを取得する
            if (raiinInfs.Any(p => p.PtId == receInfModel.PtId))
            {
                List<Ika.Models.SinRpInfModel> filteredSinRpInfs = sinRpInfModels.FindAll(p => p.PtId == receInfModel.PtId && p.SinYm == receInfModel.SinYm);
                List<Ika.Models.SinKouiModel> filteredSinKouis = sinKouiModels.FindAll(p => p.PtId == receInfModel.PtId && p.SinYm == receInfModel.SinYm);
                List<Ika.Models.SinKouiDetailModel> filteredSinKouiDetails = sinKouiDetailModels.FindAll(p => p.PtId == receInfModel.PtId && p.SinYm == receInfModel.SinYm);
                List<Ika.Models.SinKouiCountModel> filteredSinKouiCounts = sinKouiCountModels.FindAll(p => p.PtId == receInfModel.PtId && p.SinYm == receInfModel.SinYm);
                EFileDataModel retEFileView = GetEData(
                   receInfModel, filteredSinRpInfs, filteredSinKouis, filteredSinKouiDetails, filteredSinKouiCounts, raiinInfs);

                _eFileViewModels.Add(retEFileView);
            }
        }

        string ret = string.Empty;
        foreach (EFileDataModel eFile in _eFileViewModels)
        {
            if (string.IsNullOrEmpty(eFile.EFileData) == false)
            {
                if (string.IsNullOrEmpty(ret) == false)
                {
                    ret += "\r\n";
                }
                ret += eFile.EFileData;
            }
        }
        // 改行がなければ支援ツールで最終行が読み込まれない
        ret += "\r\n";

        return ret;
    }
    /// <summary>
    /// Fファイルを作成する
    /// </summary>
    /// <returns></returns>
    public string GetFFileData(int sinYm, bool includeTester)
    {
        const string conFncName = nameof(GetFFileData);
        _emrLogger.WriteLogStart(this, conFncName,
            $"sinYm:{sinYm}");

        // 対象を取得
        // RECE_INF HOKEN_KBN in (1,2) 診療月ベースで取得
        List<ReceInfModel> receInfModels =
            _ptFinder.FindEFReceInf(hpId, sinYm, includeTester);

        if (receInfModels != null)
        {
            receInfModels = receInfModels.OrderBy(p => p.PtNum).ThenBy(p => p.HokenId).ToList();
        }

        List<Ika.Models.SinRpInfModel> sinRpInfModels = _santeiFinder.FindSinRpInfDataForEF(hpId, sinYm, includeTester);
        List<Ika.Models.SinKouiModel> sinKouiModels = _santeiFinder.FindSinKouiDataForEF(hpId, sinYm, includeTester);
        List<Ika.Models.SinKouiDetailModel> sinKouiDetailModels = _santeiFinder.FindSinKouiDetailDataForEF(hpId, sinYm, includeTester);
        List<Ika.Models.SinKouiCountModel> sinKouiCountModels = _santeiFinder.FindSinKouiCountDataForEF(hpId, sinYm, includeTester);

        List<FFileDataModel> _fFileViewModels = new List<FFileDataModel>();
        List<EFRaiinInfModel> raiinInfs =
            _receMasterFinder.FindRaiinDatas(hpId, sinYm);
        foreach (ReceInfModel receInfModel in receInfModels)
        {
            // EFファイル用データを取得する
            if (raiinInfs.Any(p => p.PtId == receInfModel.PtId))
            {
                List<Ika.Models.SinRpInfModel> filteredSinRpInfs = sinRpInfModels.FindAll(p => p.PtId == receInfModel.PtId && p.SinYm == receInfModel.SinYm);
                List<Ika.Models.SinKouiModel> filteredSinKouis = sinKouiModels.FindAll(p => p.PtId == receInfModel.PtId && p.SinYm == receInfModel.SinYm);
                List<Ika.Models.SinKouiDetailModel> filteredSinKouiDetails = sinKouiDetailModels.FindAll(p => p.PtId == receInfModel.PtId && p.SinYm == receInfModel.SinYm);
                List<Ika.Models.SinKouiCountModel> filteredSinKouiCounts = sinKouiCountModels.FindAll(p => p.PtId == receInfModel.PtId && p.SinYm == receInfModel.SinYm);
                FFileDataModel retFFileView = GetFData(
                   receInfModel, filteredSinRpInfs, filteredSinKouis, filteredSinKouiDetails, filteredSinKouiCounts);

                _fFileViewModels.Add(retFFileView);
            }
        }

        string ret = string.Empty;
        foreach (FFileDataModel fFile in _fFileViewModels)
        {
            if (string.IsNullOrEmpty(fFile.FFileData) == false)
            {
                if (string.IsNullOrEmpty(ret) == false)
                {
                    ret += "\r\n";
                }
                ret += fFile.FFileData;
            }
        }
        // 改行がなければ支援ツールで最終行が読み込まれない
        ret += "\r\n";

        return ret;
    }
    /// <summary>
    /// EFファイルを作成する
    /// </summary>
    /// <returns></returns>
    public string GetEFFileData(int sinYm, bool includeTester)
    {
        const string conFncName = nameof(GetEFFileData);
        _emrLogger.WriteLogStart(this, conFncName,
            $"sinYm:{sinYm}");

        // 対象を取得
        // RECE_INF HOKEN_KBN in (1,2) 診療月ベースで取得
        List<ReceInfModel> receInfModels =
            _ptFinder.FindEFReceInf(hpId, sinYm, includeTester);

        if (receInfModels != null)
        {
            receInfModels = receInfModels.OrderBy(p => p.PtNum).ThenBy(p => p.HokenId).ToList();
        }

        List<Ika.Models.SinRpInfModel> sinRpInfModels = _santeiFinder.FindSinRpInfDataForEF(hpId, sinYm, includeTester);
        List<Ika.Models.SinKouiModel> sinKouiModels = _santeiFinder.FindSinKouiDataForEF(hpId, sinYm, includeTester);
        List<Ika.Models.SinKouiDetailModel> sinKouiDetailModels = _santeiFinder.FindSinKouiDetailDataForEF(hpId, sinYm, includeTester);
        List<Ika.Models.SinKouiCountModel> sinKouiCountModels = _santeiFinder.FindSinKouiCountDataForEF(hpId, sinYm, includeTester);

        List<EFFileDataModel> _efFileViewModels = new List<EFFileDataModel>();
        List<EFRaiinInfModel> raiinInfs =
            _receMasterFinder.FindRaiinDatas(hpId, sinYm);

        foreach (ReceInfModel receInfModel in receInfModels)
        {
            // EFファイル用データを取得する
            if (raiinInfs.Any(p => p.PtId == receInfModel.PtId))
            {
                List<Ika.Models.SinRpInfModel> filteredSinRpInfs = sinRpInfModels.FindAll(p => p.PtId == receInfModel.PtId && p.SinYm == receInfModel.SinYm);
                List<Ika.Models.SinKouiModel> filteredSinKouis = sinKouiModels.FindAll(p => p.PtId == receInfModel.PtId && p.SinYm == receInfModel.SinYm);
                List<Ika.Models.SinKouiDetailModel> filteredSinKouiDetails = sinKouiDetailModels.FindAll(p => p.PtId == receInfModel.PtId && p.SinYm == receInfModel.SinYm);
                List<Ika.Models.SinKouiCountModel> filteredSinKouiCounts = sinKouiCountModels.FindAll(p => p.PtId == receInfModel.PtId && p.SinYm == receInfModel.SinYm);
                EFFileDataModel retEFFileView = GetEFData(
                   receInfModel, filteredSinRpInfs, filteredSinKouis, filteredSinKouiDetails, filteredSinKouiCounts, raiinInfs);

                _efFileViewModels.Add(retEFFileView);
            }
        }

        string ret = string.Empty;
        foreach (EFFileDataModel fFile in _efFileViewModels)
        {
            if (string.IsNullOrEmpty(fFile.EFFileData) == false)
            {
                if (string.IsNullOrEmpty(ret) == false)
                {
                    ret += "\r\n";
                }
                ret += fFile.EFFileData;
            }
        }
        // 改行がなければ支援ツールで最終行が読み込まれない
        ret += "\r\n";

        return ret;
    }

    /// <summary>
    /// EFileデータを取得する
    /// </summary>
    /// <param name="mode">
    ///     0:社保
    ///     1:国保
    ///     2:労災(初回分)
    ///     3:労災(2回目以降分)
    ///     4:アフターケア
    /// </param>
    /// <param name="includeOutDrug">1: 院外処方含む</param>
    /// <param name="outputYm">請求年月(yyyyMM)</param>
    /// <param name="receInf">レセプト情報</param>
    /// <param name="sinRpInfs"></param>
    /// <param name="sinKouis"></param>
    /// <param name="sinDtls"></param>
    /// <param name="sinKouiCounts"></param>
    /// <returns></returns>
    public EFileDataModel GetEData(
        ReceInfModel receInf,
        List<Ika.Models.SinRpInfModel> sinRpInfs, List<Ika.Models.SinKouiModel> sinKouis, List<Ika.Models.SinKouiDetailModel> sinDtls, List<Ika.Models.SinKouiCountModel> sinKouiCounts, List<EFRaiinInfModel> raiinInfs)
    {
        const string conFncName = nameof(GetEData);

        int hpId = receInf.HpId;
        long ptId = receInf.PtId;
        int sinYm = receInf.SinYm;

        _emrLogger.WriteLogStart(this, conFncName, $"ptid:{ptId} sinYm:{sinYm}");

        // 基本情報
        Ika.Models.PtInfModel ptInfModel = FindKihon(receInf);

        // 保険情報
        HokenDataModel hokenDataModel = FindHoken(receInf);

        // 公費情報
        List<KohiDataModel> kohiDataModels = FindKohi(receInf);

        // 傷病名情報            
        List<SyobyoDataModel> syobyoDataModels = _ptFinder.FindSyobyoData(hpId, ptId, sinYm, receInf.HokenId, sinYm);

        // 診療明細情報
        SinMeiViewModel sinMeiDataModels = FindSinMei(receInf, ptInfModel, sinRpInfs, sinKouis, sinDtls, sinKouiCounts);

        // 施設コード
        string sisetuCd = _receMasterFinder.GetSisetuCd(hpId, sinYm * 100 + 31);

        // Eファイルデータ取得
        EFileDataModel efileViewModel = new EFileDataModel(sisetuCd, receInf, ptInfModel, hokenDataModel, kohiDataModels, syobyoDataModels, sinMeiDataModels.SinMei, raiinInfs, _receMasterFinder);

        _emrLogger.WriteLogEnd(this, conFncName, "");

        return efileViewModel;
    }

    /// <summary>
    /// FFileデータを取得する
    /// </summary>
    /// <param name="mode">
    ///     0:社保
    ///     1:国保
    ///     2:労災(初回分)
    ///     3:労災(2回目以降分)
    ///     4:アフターケア
    /// </param>
    /// <param name="includeOutDrug">1: 院外処方含む</param>
    /// <param name="outputYm">請求年月(yyyyMM)</param>
    /// <param name="receInf">レセプト情報</param>
    /// <param name="sinRpInfs"></param>
    /// <param name="sinKouis"></param>
    /// <param name="sinDtls"></param>
    /// <param name="sinKouiCounts"></param>
    /// <returns></returns>
    public FFileDataModel GetFData(
        ReceInfModel receInf,
        List<Ika.Models.SinRpInfModel> sinRpInfs, List<Ika.Models.SinKouiModel> sinKouis, List<Ika.Models.SinKouiDetailModel> sinDtls, List<Ika.Models.SinKouiCountModel> sinKouiCounts)
    {
        const string conFncName = nameof(GetFData);

        int hpId = receInf.HpId;
        long ptId = receInf.PtId;
        int sinYm = receInf.SinYm;

        _emrLogger.WriteLogStart(this, conFncName, $"ptid:{ptId} sinYm:{sinYm}");

        // 基本情報
        Ika.Models.PtInfModel ptInfModel = FindKihon(receInf);

        // 保険情報
        HokenDataModel hokenDataModel = FindHoken(receInf);

        // 公費情報
        List<KohiDataModel> kohiDataModels = FindKohi(receInf);

        // 傷病名情報            
        List<SyobyoDataModel> syobyoDataModels = _ptFinder.FindSyobyoData(hpId, ptId, sinYm, receInf.HokenId, sinYm);

        // 診療明細情報
        SinMeiViewModel sinMeiDataModels = FindSinMei(receInf, ptInfModel, sinRpInfs, sinKouis, sinDtls, sinKouiCounts);

        // 施設コード
        string sisetuCd = _receMasterFinder.GetSisetuCd(hpId, sinYm * 100 + 31);

        // Fファイルデータ取得
        FFileDataModel ffileViewModel = new FFileDataModel(sisetuCd, receInf, ptInfModel, hokenDataModel, kohiDataModels, syobyoDataModels, sinMeiDataModels.SinMei, _receMasterFinder);

        _emrLogger.WriteLogEnd(this, conFncName, "");

        return ffileViewModel;
    }

    /// <summary>
    /// FFileデータを取得する
    /// </summary>
    /// <param name="mode">
    ///     0:社保
    ///     1:国保
    ///     2:労災(初回分)
    ///     3:労災(2回目以降分)
    ///     4:アフターケア
    /// </param>
    /// <param name="includeOutDrug">1: 院外処方含む</param>
    /// <param name="outputYm">請求年月(yyyyMM)</param>
    /// <param name="receInf">レセプト情報</param>
    /// <param name="sinRpInfs"></param>
    /// <param name="sinKouis"></param>
    /// <param name="sinDtls"></param>
    /// <param name="sinKouiCounts"></param>
    /// <returns></returns>
    public EFFileDataModel GetEFData(
        ReceInfModel receInf,
        List<Ika.Models.SinRpInfModel> sinRpInfs, List<Ika.Models.SinKouiModel> sinKouis, List<Ika.Models.SinKouiDetailModel> sinDtls, List<Ika.Models.SinKouiCountModel> sinKouiCounts, List<EFRaiinInfModel> raiinInfs)
    {
        const string conFncName = nameof(GetEFData);

        int hpId = receInf.HpId;
        long ptId = receInf.PtId;
        int sinYm = receInf.SinYm;

        _emrLogger.WriteLogStart(this, conFncName, $"ptid:{ptId} sinYm:{sinYm}");

        // 基本情報
        Ika.Models.PtInfModel ptInfModel = FindKihon(receInf);

        // 保険情報
        HokenDataModel hokenDataModel = FindHoken(receInf);

        // 公費情報
        List<KohiDataModel> kohiDataModels = FindKohi(receInf);

        // 傷病名情報            
        List<SyobyoDataModel> syobyoDataModels = _ptFinder.FindSyobyoData(hpId, ptId, sinYm, receInf.HokenId, sinYm);

        // 診療明細情報
        SinMeiViewModel sinMeiDataModels = FindSinMei(receInf, ptInfModel, sinRpInfs, sinKouis, sinDtls, sinKouiCounts);

        // 施設コード
        string sisetuCd = _receMasterFinder.GetSisetuCd(hpId, sinYm * 100 + 31);

        // EFファイルデータ取得
        EFFileDataModel effileViewModel = new EFFileDataModel(sisetuCd, receInf, ptInfModel, hokenDataModel, kohiDataModels, syobyoDataModels, sinMeiDataModels.SinMei, raiinInfs, _receMasterFinder);

        _emrLogger.WriteLogEnd(this, conFncName, "");

        return effileViewModel;
    }

    /// <summary>
    /// 基本情報取得
    /// </summary>
    /// <param name="receInf"></param>
    /// <returns></returns>
    private Ika.Models.PtInfModel FindKihon(ReceInfModel receInf)
    {
        Ika.Models.PtInfModel ptInfModel = null;

        if (receInf.PtInf != null)
        {
            ptInfModel = new Ika.Models.PtInfModel(receInf.PtInf, receInf.SinYm * 100 + 1);
        }
        else
        {
            ptInfModel = _ptFinder.FindPtInf(receInf.HpId, receInf.PtId, receInf.SinYm * 100 + 1);
        }

        return ptInfModel;
    }

    /// <summary>
    /// 保険情報取得
    /// </summary>
    /// <param name="receInf"></param>
    /// <returns></returns>
    private HokenDataModel FindHoken(ReceInfModel receInf)
    {
        HokenDataModel hokenDataModel = null;
        if (receInf.PtHokenInf != null)
        {
            hokenDataModel = new HokenDataModel(receInf.PtHokenInf);
        }
        else if (receInf.HokenId > 0)
        {
            hokenDataModel = _hokenFinder.FindHokenData(receInf.HpId, receInf.PtId, receInf.HokenId);
        }

        if (hokenDataModel != null)
        {
            hokenDataModel.JituNissu = receInf.HokenNissu;
            hokenDataModel.TotalTen = receInf.HokenReceTensu;
            hokenDataModel.FutanKingaku = receInf.HokenReceFutan;
        }

        return hokenDataModel;
    }

    /// <summary>
    /// 公費情報取得
    /// </summary>
    /// <param name="receInf"></param>
    /// <returns></returns>
    private List<KohiDataModel> FindKohi(ReceInfModel receInf)
    {
        List<KohiDataModel> kohiDataModels = new List<KohiDataModel>();

        // 公費ID
        List<KohiDataModel> kohiDatas = _hokenFinder.FindKohiData(receInf.HpId, receInf.PtId, receInf.SinYm * 100 + 1);
        if (kohiDatas.Any())
        {
            for (int i = 1; i <= 4; i++)
            {
                if (receInf.KohiReceKisai(i) == 1 && receInf.KohiId(i) > 0)
                {
                    if (kohiDatas.Any(p => p.PtKohi.HokenId == receInf.KohiId(i)))
                    {
                        kohiDataModels.Add(kohiDatas.Find(p => p.PtKohi.HokenId == receInf.KohiId(i)));
                        kohiDataModels.Last().JituNissu = receInf.KohiNissu(i);
                        kohiDataModels.Last().Tensu = receInf.KohiTensu(i);
                        kohiDataModels.Last().ReceTen = receInf.KohiReceTensu(i);
                        kohiDataModels.Last().ReceFutan = receInf.KohiReceFutan(i);
                        kohiDataModels.Last().ReceKyufu = receInf.KohiReceKyufu(i);
                        kohiDataModels.Last().Futan = receInf.KohiFutan(i);
                        kohiDataModels.Last().Futan10en = receInf.KohiFutan10en(i);
                    }
                }
            }
        }

        return kohiDataModels;
    }
    private SinMeiViewModel FindSinMei(
        ReceInfModel receInf, Ika.Models.PtInfModel ptInfModel, List<Ika.Models.SinRpInfModel> sinRpInfs, List<Ika.Models.SinKouiModel> sinKouis, List<Ika.Models.SinKouiDetailModel> sinDtls, List<Ika.Models.SinKouiCountModel> sinKouiCounts)
    {
        SinMeiViewModel sinMeiDataModels = null;

        //Log.WriteLogTrc(ModuleName, this, conFncName, "sinMei");
        if (sinRpInfs != null)
        {
            sinMeiDataModels = new SinMeiViewModel(SinMeiMode.EFFile, true, receInf.HpId, receInf.PtId, receInf.SinYm, receInf.SinYm, receInf, null, ptInfModel, receInf.Tokki, sinRpInfs, sinKouis, sinDtls, sinKouiCounts,
                                   _tenantProvider,
                                   _systemConfigProvider,
                                   _emrLogger);
        }
        else
        {
            // 今(2020/07/23)のところ、こちらを通ることはないはず
            sinMeiDataModels = new SinMeiViewModel(SinMeiMode.EFFile, true, receInf.HpId, receInf.PtId, receInf.SinYm, receInf, ptInfModel, receInf.Tokki,
                                   _tenantProvider,
                                   _systemConfigProvider,
                                   _emrLogger);
        }

        return sinMeiDataModels;
    }

    public void Dispose()
    {
        _tenantDataContext.Dispose();
    }
}
