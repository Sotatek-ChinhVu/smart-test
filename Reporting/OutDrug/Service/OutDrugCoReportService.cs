using Helper.Common;
using Helper.Constants;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Reporting.CommonMasters.Config;
using Reporting.CommonMasters.Constants;
using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.OutDrug.DB;
using Reporting.OutDrug.Model;
using System.Text;
using System.Text.Json;
using Reporting.OutDrug.Model.Output;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.OutDrug.Utils;
using HokenSbtKbn = Reporting.CommonMasters.Constants.HokenSbtKbn;

namespace Reporting.OutDrug.Service;

public class OutDrugCoReportService : IOutDrugCoReportService
{
    private List<CoOutDrugModel> _coModels { get; set; }
    private CoOutDrugModel _coModel;
    private readonly ISystemConfig _systemConfig;
    private readonly ITenantProvider _tenantProvider;
    private readonly IReadRseReportFileService _readRseReportFileService;
    /// <summary>
    /// 印刷処理繰り返し回数
    /// </summary>
    private int _repeatKai;

    /// <summary>
    /// 印刷タイプ（どの帳票を印刷するか？）
    /// 0: 処方箋
    /// 1: 処方箋QR用
    /// 2: 分割指示
    /// </summary>
    private OutDrugPrintOutType _printoutType { get; set; }

    /// <summary>
    /// 処方箋項目欄印字データ
    /// </summary>
    private List<PrintOutData> _dataList;

    /// <summary>
    /// 処方箋備考欄印字データ
    /// </summary>
    private List<string> _bikoList { get; set; }

    public OutDrugCoReportService(ISystemConfig systemConfig, ITenantProvider tenantProvider, IReadRseReportFileService readRseReportFileService)
    {
        _systemConfig = systemConfig;
        _tenantProvider = tenantProvider;
        _readRseReportFileService = readRseReportFileService;
        _bikoList = new();
        _dataList = new();
        _coModels = new();
        _coModel = new();
    }

    // CoReport Formから読み取ったオブジェクトの情報
    #region Form Object Property

    /// <summary>
    /// 項目リストの幅
    /// </summary>
    private int _dataCharCount;
    /// <summary>
    /// 数量リストの幅
    /// </summary>
    private int _suryoCharCount;
    /// <summary>
    /// 単位リストの幅
    /// </summary>
    private int _unitCharCount;
    /// <summary>
    /// 回数リストの幅
    /// </summary>
    private int _kaisuCharCount;
    /// <summary>
    /// 用法単位リストの幅
    /// </summary>
    private int _yohoUnitCharCount;
    /// <summary>
    /// 項目リストの行数
    /// </summary>
    private int _dataRowCount;

    /// <summary>
    /// 備考欄狭いほうの幅
    /// </summary>
    private int _bikoShortCharCount;
    /// <summary>
    /// 備考欄狭いほうの行数
    /// </summary>
    private int _bikoShortRowCount;
    /// <summary>
    /// 備考欄広い方の幅
    /// </summary>
    private int _bikoLongCharCount;
    /// <summary>
    /// 備考欄広い方の行数
    /// </summary>
    private int _bikoLongRowCount;
    #endregion

    public CoOutDrugReportingOutputData GetOutDrugReportingData(int hpId, long ptId, int sinDate, long raiinNo)
    {
        try
        {
            List<CoOutDrugReportingOutputItem> result = new();
            using (var noTrackingDataContext = _tenantProvider.GetNoTrackingDataContext())
            {
                var finder = new CoOutDrugFinder(_tenantProvider);
                _coModels = GetData(finder, hpId, ptId, sinDate, raiinNo);

                string printDataId = string.Empty;

                int repeatMax = 1;
                if (_systemConfig.SyohosenHikae() == 1)
                {
                    // 控えを印刷する場合、2回実行する
                    repeatMax = 2;
                }

                if (_coModels.Count == 0)
                {
                    return new CoOutDrugReportingOutputData()
                    {
                        Data = result,
                        RepeatMax = repeatMax,
                        SyohosenRefillStrikeLine = _systemConfig.SyohosenRefillStrikeLine(),
                        SyohosenQRKbn = _systemConfig.SyohosenQRKbn()
                    };
                }

                if (_coModels != null && _coModels.Count != 0)
                {
                    foreach (CoOutDrugModel coModelData in _coModels)
                    {
                        _coModel = coModelData;

                        if (_coModel.PrintData?.PrintDataID != printDataId)
                        {
                            // 印刷対象が変わったらページリセット
                            printDataId = _coModel.PrintData?.PrintDataID ?? string.Empty;
                        }

                        if (_coModel.PrintData?.PrintType == OutDrugPrintOutType.Syohosen)
                        {
                            string formfile = "fmOutDrug.rse";
                            if (coModelData.PrintData?.SinDate >= 20220401)
                            {
                                formfile = "fmOutDrug_202204.rse";
                            }

                            // フォーム情報取得
                            GetFormParam(formfile);

                            // 印字する項目のリスト生成
                            MakeDataList();

                            // 印字する備考のリスト生成
                            MakeBikoList();

                            // 印刷タイプを処方箋にセット
                            _printoutType = OutDrugPrintOutType.Syohosen;
                        }
                        else if (_coModel.PrintData?.PrintType == OutDrugPrintOutType.Bunkatu)
                        {
                            // 分割指示別紙
                            _printoutType = OutDrugPrintOutType.Bunkatu;
                        }

                        Dictionary<string, string> singleFieldData = UpdateFormHeader();
                        var outputItem = new CoOutDrugReportingOutputItem()
                        {
                            SingleFieldData = singleFieldData,
                            PrintOutData = _dataList,
                            BikoList = _bikoList,
                            PrintoutType = (int)_printoutType,
                            PrintDataId = printDataId,
                            PrintDataPrintType = (int)(_coModel.PrintData?.PrintType ?? 0),
                            PrintDataSinDate = _coModel.PrintData?.SinDate ?? 0,
                            PrintDataBunkatuMax = _coModel.PrintData?.BunkatuMax ?? 0,
                            PrintDataHonKeKbn = _coModel.PrintData?.HonKeKbn ?? 0,
                            PrintDataRefillCount = _coModel.PrintData?.RefillCount ?? 0,
                            PrintDataSex = _coModel.PrintData?.Sex ?? 0,
                            QRData = _coModel.QRData(),
                            PtNum = _coModel.PrintData?.PtNum ?? 0,
                            PtName = _coModel.PrintData?.PtName ?? string.Empty,
                            HpFaxNo = _coModel.PrintData?.HpFaxNo ?? string.Empty,
                            HpOtherContacts = _coModel.PrintData?.HpOtherContacts ?? string.Empty,
                            HpTel = _coModel.PrintData?.HpTel ?? string.Empty,
                        };
                        result.Add(outputItem);
                    }
                }

                return new CoOutDrugReportingOutputData()
                {
                    Data = result,
                    RepeatMax = repeatMax,
                    SyohosenRefillStrikeLine = _systemConfig.SyohosenRefillStrikeLine(),
                    SyohosenQRKbn = _systemConfig.SyohosenQRKbn()
                };
            }
        }
        finally
        {
            _systemConfig.ReleaseResource();
            _tenantProvider.DisposeDataContext();
        }
    }

    /// <summary>
    /// 印刷する内容を取得する
    /// </summary>
    /// <returns></returns>
    private List<CoOutDrugModel> GetData(CoOutDrugFinder finder, int hpId, long ptId, int sinDate, long raiinNo)
    {
        // 戻り値
        List<CoOutDrugModel> outDrugModels = new();

        List<CoOdrInfModel> odrInfs = finder.FindOdrInfData(hpId, ptId, sinDate, raiinNo);
        List<CoOdrInfDetailModel> odrInfDtls = finder.FindOdrInfDetailData(hpId, ptId, sinDate, raiinNo);

        foreach (CoOdrInfModel odr in odrInfs)
        {
            if (odrInfDtls.Any(p => p.RpNo == odr.RpNo && p.RpEdaNo == odr.RpEdaNo))
            {
                odr.Refill = (int)odrInfDtls.Find(p => p.RpNo == odr.RpNo && p.RpEdaNo == odr.RpEdaNo).Suryo;
            }
        }

        List<CoOdrInfModel> filteredOdrInfs = new();
        List<CoOdrInfDetailModel> filteredOdrInfDtls = new();

        // 医療機関情報取得
        CoHpInfModel hpInf = finder.FindHpInf(hpId, sinDate);
        // 患者情報
        CoPtInfModel ptInf = finder.FindPtInf(hpId, ptId, sinDate);

        // QRコードのバージョン
        string version = QRVersion.Jahis5;
        switch (_systemConfig.SyohosenQRVersion())
        {
            case 1:
                version = QRVersion.Jahis7;
                break;
            case 2:
                version = sinDate >= KaiseiDate.d20220401 ? QRVersion.Jahis8 : QRVersion.Jahis7;
                break;
            case 3:
                version = sinDate >= KaiseiDate.d20221001 ? QRVersion.Jahis9 : QRVersion.Jahis8;
                break;
        }

        // 保険の種類ごとに処理する
        for (int i = 0; i <= 4; i++)
        {
            if (i != 4)
            {
                List<int> hokenSyu = new();
                switch (i)
                {
                    case 0: // 健保
                        hokenSyu = new List<int> { 0 };
                        break;
                    case 1: // 労災
                        hokenSyu = new List<int> { 1, 2 };
                        break;
                    case 2: // 自賠
                        hokenSyu = new List<int> { 3 };
                        break;
                    case 3: // 自費
                        hokenSyu = new List<int> { 4 };
                        break;
                }

                // 自費算定以外の場合、自費算定分除く（自費分点分は自費保険の方に印字する）
                filteredOdrInfs = odrInfs.FindAll(p => hokenSyu.Contains(p.HokenSyu) && p.SanteiKbn != 2);
            }
            else
            {
                // 自費算定の場合、保険関係なく、自費分点分を含める
                filteredOdrInfs = odrInfs.FindAll(p => p.SanteiKbn == 2);
            }

            if (filteredOdrInfs.Any())
            {

                // 保険IDの種類（自費算定を除いて、ODR_INFのHOKEN_IDをグループ化）
                List<int> hokenIds = filteredOdrInfs.Where(p => p.SanteiKbn != 2).GroupBy(p => p.HokenId).Select(p => p.Key).ToList();

                if (i == 4)
                {
                    // 自費算定の場合、保険IDは関係ないので、ダミーで1つ追加しておく
                    hokenIds.Add(0);
                }

                // 保険IDごとのループ
                foreach (int hokenId in hokenIds)
                {
                    // 保険PIDの種類（保険IDを持つ組み合わせIDを探す）
                    var groupbyHokenPids = odrInfs.Where(p => p.HokenId == hokenId).GroupBy(p => p.HokenPid).ToList();

                    List<int> hokenPids = new List<int>();

                    foreach (var groupbyHokenPid in groupbyHokenPids)
                    {
                        hokenPids.Add(groupbyHokenPid.Key);
                    }

                    // 改めてオーダー取り直し
                    if (i != 4)
                    {
                        // 自費算定以外
                        filteredOdrInfs = odrInfs.FindAll(p => hokenPids.Contains(p.HokenPid) && p.SanteiKbn != 2);
                        filteredOdrInfDtls = odrInfDtls.FindAll(p => hokenPids.Contains(p.HokenPId) && p.SanteiKbn != 2);
                    }
                    else
                    {
                        // 自費算定
                        filteredOdrInfs = odrInfs.FindAll(p => p.SanteiKbn == 2);
                        filteredOdrInfDtls = odrInfDtls.FindAll(p => p.SanteiKbn == 2);
                    }

                    // オーダーデータがなければ次へ
                    if (!filteredOdrInfs.Any())
                    {
                        continue;
                    }

                    // リフィル回数
                    List<(int count, List<(long rpno, long rpedano)> rp)> refills = new List<(int, List<(long, long)>)>();

                    foreach (CoOdrInfModel odr in filteredOdrInfs)
                    {
                        List<CoOdrInfDetailModel> dtlrefill =
                            filteredOdrInfDtls.FindAll(p =>
                                p.RpNo == odr.RpNo &&
                                p.RpEdaNo == odr.RpEdaNo &&
                                p.ItemCd == ItemCdConst.Con_Refill);
                        if (dtlrefill != null && dtlrefill.Any())
                        {
                            if (!refills.Any(p => p.count == dtlrefill.First().Suryo))
                            {
                                refills.Add(((int)dtlrefill.First().Suryo, new List<(long rpno, long rpedano)>()));
                            }

                            refills.Find(p => p.count == (int)dtlrefill.First().Suryo).rp.Add((odr.RpNo, odr.RpEdaNo));
                        }
                        else
                        {
                            if (!refills.Any(p => p.count == 0))
                            {
                                refills.Add((0, new List<(long rpno, long rpedano)>()));
                            }

                            refills.Find(p => p.count == 0).rp.Add((odr.RpNo, odr.RpEdaNo));
                        }
                    }

                    // 残薬確認・情報提供のチェック（取得したodrInfと紐づかないので別にチェック）
                    foreach (CoOdrInfDetailModel dtl in filteredOdrInfDtls.FindAll(p => new string[] { ItemCdConst.ZanGigi, ItemCdConst.ZanTeiKyo }.Contains(p.ItemCd)))
                    {
                        List<CoOdrInfDetailModel> dtlrefill =
                            filteredOdrInfDtls.FindAll(p =>
                                p.RpNo == dtl.RpNo &&
                                p.RpEdaNo == dtl.RpEdaNo &&
                                p.ItemCd == ItemCdConst.Con_Refill);
                        if (dtlrefill != null && dtlrefill.Any())
                        {
                            if (!refills.Any(p => p.count == dtlrefill.First().Suryo))
                            {
                                refills.Add(((int)dtlrefill.First().Suryo, new List<(long rpno, long rpedano)>()));
                            }

                            refills.Find(p => p.count == (int)dtlrefill.First().Suryo).rp.Add((dtl.RpNo, dtl.RpEdaNo));
                        }
                        else
                        {
                            if (!refills.Any(p => p.count == 0))
                            {
                                refills.Add((0, new List<(long rpno, long rpedano)>()));
                            }

                            refills.Find(p => p.count == 0).rp.Add((dtl.RpNo, dtl.RpEdaNo));
                        }
                    }

                    foreach ((int count, List<(long rpno, long rpedano)> rp) refill in refills.OrderBy(p => p.count))
                    {
                        bool _isRefillRp(long Arpno, long Arpedano)
                        {
                            bool ret = false;

                            foreach ((long rpno, long rpedano) in refill.rp)
                            {
                                if (Arpno == rpno && Arpedano == rpedano)
                                {
                                    ret = true;
                                    break;
                                }
                            }

                            return ret;
                        }

                        filteredOdrInfs = odrInfs.FindAll(p => _isRefillRp(p.RpNo, p.RpEdaNo));

                        filteredOdrInfDtls = odrInfDtls.FindAll(p => _isRefillRp(p.RpNo, p.RpEdaNo));

                        // 最大分割回数
                        int bunkatuMax = filteredOdrInfDtls.Max(p => p.BunkatuCount);
                        if (bunkatuMax <= 0) bunkatuMax = 1;

                        // PT_HOKEN
                        CoPtHokenInfModel ptHoken = finder.FindPtHoken(hpId, ptId, hokenId, sinDate);

                        // PT_KOHI
                        // この患者が当該診療で使用しているすべての公費
                        List<CoPtKohiModel> ptKohis = new List<CoPtKohiModel>();
                        // 処方箋の公費欄に印字する公費
                        List<CoPtKohiModel> filteredPtKohis = new List<CoPtKohiModel>();


                        if (i == 0)
                        {
                            #region 健保の場合は、公費の種類を調べ、オーダーに公費使用状況を設定する
                            // KohiIDの種類
                            HashSet<int> kohiIds = new HashSet<int>();
                            foreach (CoOdrInfModel odrInf in filteredOdrInfs)
                            {
                                for (int j = 1; j <= 4; j++)
                                {
                                    if (odrInf.KohiId(j) > 0) kohiIds.Add(odrInf.KohiId(j));
                                }
                            }

                            if (kohiIds.Any())
                            {
                                ptKohis = finder.FindPtKohi(hpId, ptId, sinDate, kohiIds);
                                // priority順に並べ替え
                                // 公費（5.生保、6.分点、7.一般) で、負担者番号があるものに絞り込み
                                // 他に処方箋未記載の公費があれば、ここでフィルタする
                                filteredPtKohis =
                                    ptKohis.FindAll(p =>
                                        new int[] { 5, 6, 7 }.Contains(p.HokenSbtKbn) &&
                                        (p.FutansyaNo != string.Empty || p.JyukyusyaNo != string.Empty)
                                        )
                                    .OrderBy(p => p.SortKey)
                                    .ToList();
                            }

                            #region オーダーの公費使用状況を設定

                            // HOKEN_PIDに対する公費使用状況のマスタ
                            List<(int pid, int kohi1, int kohi2, int kohi3, int kohi4)>
                                futans = new List<(int pid, int kohi1, int kohi2, int kohi3, int kohi4)>();

                            foreach (CoOdrInfModel odrInf in filteredOdrInfs)
                            {
                                if (futans.Any(p => p.pid == odrInf.HokenPid))
                                {
                                    // PIDがマスタに存在する場合
                                    (int pid, int kohi1, int kohi2, int kohi3, int kohi4) futan =
                                        futans.Find(p => p.pid == odrInf.HokenPid);
                                    odrInf.Kohi1Futan = futan.kohi1;
                                    odrInf.Kohi2Futan = futan.kohi2;
                                    odrInf.Kohi3Futan = futan.kohi3;
                                    odrInf.KohiSpFutan = futan.kohi4;
                                }
                                else
                                {
                                    // PIDがマスタに存在しない場合
                                    if (odrInf.PtHokenPattern.Kohi1Id > 0)
                                    {
                                        int[] kohis = new int[4];
                                        for (int j = 1; j <= 4; j++)
                                        {
                                            int odrKohiId = odrInf.KohiId(j);
                                            for (int k = 0; k < filteredPtKohis.Count; k++)
                                            {
                                                if (filteredPtKohis[k].HokenId == odrKohiId)
                                                {
                                                    if (new int[] { HokenSbtKbn.Seiho, HokenSbtKbn.Bunten, HokenSbtKbn.Ippan }.Contains(filteredPtKohis[k].HokenMst.HokenSbtKbn))
                                                    {
                                                        // 分点公費の場合は2を設定
                                                        kohis[k] = 2;
                                                    }
                                                    else
                                                    {
                                                        kohis[k] = 1;
                                                    }
                                                }
                                            }
                                        }

                                        odrInf.Kohi1Futan = kohis[0];
                                        odrInf.Kohi2Futan = kohis[1];
                                        odrInf.Kohi3Futan = kohis[2];
                                        odrInf.KohiSpFutan = kohis[3];

                                        futans.Add((odrInf.HokenPid, kohis[0], kohis[1], kohis[2], kohis[3]));
                                    }
                                }
                            }
                            #endregion
                            #endregion
                        }

                        // ソート
                        if (i == 0)
                        {
                            // 健保の場合
                            if (_systemConfig.SyohosenKouiDivide() == 1)
                            {
                                // 行為を分けて出力
                                filteredOdrInfs =
                                    filteredOdrInfs
                                        .OrderBy(p => p.KohiSortKey)
                                        .ThenBy(p => p.OdrKouiKbn)
                                        .ThenBy(p => p.SyohoSbt)
                                        .ThenBy(p => p.SikyuKbn)
                                        .ThenBy(p => p.TosekiKbn)
                                        .ThenBy(p => p.SanteiKbn)
                                        .ThenBy(p => p.SortNo)
                                        .ThenBy(p => p.RpNo)
                                        .ThenBy(p => p.RpEdaNo)
                                        .ToList();
                            }
                            else
                            {
                                filteredOdrInfs =
                                    filteredOdrInfs
                                        .OrderBy(p => p.KohiSortKey)
                                        .ThenBy(p => p.SyohoSbt)
                                        .ThenBy(p => p.SikyuKbn)
                                        .ThenBy(p => p.TosekiKbn)
                                        .ThenBy(p => p.SanteiKbn)
                                        .ThenBy(p => p.SortNo)
                                        .ThenBy(p => p.OdrKouiKbn)
                                        .ThenBy(p => p.RpNo)
                                        .ThenBy(p => p.RpEdaNo)
                                        .ToList();
                            }
                        }
                        else
                        {
                            // 健保以外の場合
                            if (_systemConfig.SyohosenKouiDivide() == 1)
                            {
                                // 行為を分けて出力
                                filteredOdrInfs =
                                    filteredOdrInfs
                                        .OrderBy(p => p.HokenPid)
                                        .ThenBy(p => p.OdrKouiKbn)
                                        .ThenBy(p => p.SyohoSbt)
                                        .ThenBy(p => p.SikyuKbn)
                                        .ThenBy(p => p.TosekiKbn)
                                        .ThenBy(p => p.SanteiKbn)
                                        .ThenBy(p => p.SortNo)
                                        .ThenBy(p => p.RpNo)
                                        .ThenBy(p => p.RpEdaNo)
                                        .ToList();
                            }
                            else
                            {
                                filteredOdrInfs =
                                    filteredOdrInfs
                                        .OrderBy(p => p.HokenPid)
                                        .ThenBy(p => p.SyohoSbt)
                                        .ThenBy(p => p.SikyuKbn)
                                        .ThenBy(p => p.TosekiKbn)
                                        .ThenBy(p => p.SanteiKbn)
                                        .ThenBy(p => p.SortNo)
                                        .ThenBy(p => p.OdrKouiKbn)
                                        .ThenBy(p => p.RpNo)
                                        .ThenBy(p => p.RpEdaNo)
                                        .ToList();
                            }
                        }

                        // 来院情報取得
                        CoRaiinInfModel raiinInf = finder.FindRaiinInf(hpId, ptId, sinDate, raiinNo);

                        // 分割回数分繰り返し
                        for (int bunkatuKai = 1; bunkatuKai <= bunkatuMax; bunkatuKai++)
                        {
                            // 印字用データを作成
                            CoOutDrugPrintData printData = new CoOutDrugPrintData(OutDrugPrintOutType.Syohosen, sinDate, ptInf, ptHoken, filteredPtKohis, hpInf, raiinInf, bunkatuMax, bunkatuKai, refill.count);

                            // QRのデータを作成
                            CoOutDrugQRData qrData = new CoOutDrugQRData(version, sinDate, hpInf, raiinInf, ptInf, ptHoken, filteredPtKohis, bunkatuMax, bunkatuKai, refill.count);

                            #region 残薬
                            qrData.QR062 = null;
                            if (filteredOdrInfDtls.Any(p => p.ItemCd == @"@ZANGIGI"))
                            {
                                // 疑義照会
                                qrData.QR062 = new CoOutDrugQR062(version, 1);
                                printData.ZanyakuGigi = true;
                            }
                            else if (filteredOdrInfDtls.Any(p => p.ItemCd == @"@ZANTEIKYO"))
                            {
                                // 情報提供
                                qrData.QR062 = new CoOutDrugQR062(version, 2);
                                printData.ZanyakuTeikyo = true;
                            }
                            #endregion

                            #region 備考
                            qrData.QR081 = new CoOutDrugQR081(version);

                            List<string> bikos = new List<string>();
                            string biko = string.Empty;

                            #region 処方箋備考欄
                            foreach (CoOdrInfModel odrInf in filteredOdrInfs.FindAll(p => p.OdrKouiKbn == 101))
                            {
                                List<CoOdrInfDetailModel> BikoOdrInfDtls =
                                    odrInfDtls.FindAll(p =>
                                        p.RpNo == odrInf.RpNo && p.RpEdaNo == odrInf.RpEdaNo && p.ItemCd != ItemCdConst.Con_Refill);

                                if (!BikoOdrInfDtls.Any()) continue;

                                foreach (CoOdrInfDetailModel odrDtl in BikoOdrInfDtls)
                                {
                                    biko += odrDtl.ItemName;
                                    if (odrDtl.CommentNewLine == 1)
                                    {
                                        bikos.Add(biko);
                                        biko = string.Empty;
                                    }
                                }

                                if (biko != string.Empty)
                                {
                                    bikos.Add(biko);
                                    biko = string.Empty;
                                }
                            }
                            #endregion

                            #region 第３第４公費
                            if (filteredPtKohis.Count >= 3)
                            {
                                for (int kohiIndex = 2; kohiIndex < filteredPtKohis.Count; kohiIndex++)
                                {
                                    bikos.Add(
                                        $"公費{CIUtil.ToWide((kohiIndex + 1).ToString())}　" +
                                        $"負担者番号：{filteredPtKohis[kohiIndex].PtKohi.FutansyaNo}　" +
                                        $"受給者番号：{filteredPtKohis[kohiIndex].PtKohi.JyukyusyaNo}"
                                        );
                                }
                            }
                            #endregion

                            #region 麻薬
                            if (filteredOdrInfDtls.Any(p => p.MadokuKbn == 1))
                            {
                                biko = OutDrugUtil.AppendStr(biko, $"患者住所：{ptInf.HomeAddress1 + ptInf.HomeAddress2} 麻薬施用者免許証番号：{raiinInf.MayakuLicenseNo}");
                            }
                            #endregion

                            #region ニコチネル
                            if (filteredOdrInfDtls.Any(p => ItemCdConst.nicotineruls.Contains(p.ItemCd)))
                            {
                                biko = OutDrugUtil.AppendStr(biko, "ニコチン依存症管理料の算定に伴う処方");
                            }
                            #endregion

                            #region 地域包括
                            if (_systemConfig.SyohosenChiikiHoukatu() == 1)
                            {
                                #region sub method
                                void _addBikoSanteiOdrItem(List<string> itemCds, string kigo)
                                {
                                    bool santei = false;
                                    santei = finder.CheckSanteiTerm(hpId, ptId, sinDate / 100 * 100 + 1, sinDate, itemCds);
                                    if (!santei)
                                    {
                                        santei = finder.CheckOdrTerm(hpId, ptId, sinDate, sinDate, itemCds);
                                    }

                                    if (santei)
                                    {
                                        biko = OutDrugUtil.AppendStr(biko, kigo);
                                    }
                                }
                                void _addBikoOdrItem(List<string> itemCds, string kigo)
                                {
                                    bool santei = false;

                                    santei = finder.CheckOdrTerm(hpId, ptId, sinDate, sinDate, itemCds);

                                    if (santei)
                                    {
                                        biko = OutDrugUtil.AppendStr(biko, kigo);
                                    }
                                }
                                #endregion

                                _addBikoSanteiOdrItem(new List<string> { ItemCdConst.IgakuTiikiHoukatu1 }, "地域包括診療料１算定");
                                _addBikoSanteiOdrItem(new List<string> { ItemCdConst.IgakuTiikiHoukatu2 }, "地域包括診療料２算定");
                                _addBikoSanteiOdrItem(new List<string> { ItemCdConst.IgakuNintiTiikiHoukatu1 }, "認知症地域包括診療料１算定");
                                _addBikoSanteiOdrItem(new List<string> { ItemCdConst.IgakuNintiTiikiHoukatu2 }, "認知症地域包括診療料２算定");

                                _addBikoOdrItem(new List<string> { ItemCdConst.SaisinTiikiHoukatu1 }, "地域包括診療料加算１算定");
                                _addBikoOdrItem(new List<string> { ItemCdConst.SaisinTiikiHoukatu2 }, "地域包括診療料加算２算定");
                                _addBikoOdrItem(new List<string> { ItemCdConst.SaisinNintiTiikiHoukatu1 }, "認知症地域包括診療料加算１算定");
                                _addBikoOdrItem(new List<string> { ItemCdConst.SaisinNintiTiikiHoukatu2 }, "認知症地域包括診療料加算２算定");
                            }
                            #endregion

                            #region 情報通信
                            if (finder.CheckOdrRaiin(hpId, ptId, raiinNo, new List<string> { ItemCdConst.Con_Jouhou }))
                            {
                                biko = OutDrugUtil.AppendStr(biko, "情報通信");
                            }
                            #endregion

                            #region 高齢者
                            if (i == 0 && (CIUtil.AgeChk(ptInf.Birthday, sinDate, 70) || ptHoken.IsKouki) && ptHoken.HokenSbtKbn == HokenSbtKbn.Hoken)
                            {
                                int hokenRate = printData.GetHokenRate(ptHoken.Rate, ptHoken.HokenSbtKbn, ptHoken.KogakuKbn, ptHoken.Houbetu);

                                if (hokenRate >= 30)
                                {
                                    biko = OutDrugUtil.AppendStr(biko, "（高７）");
                                }
                                else
                                {
                                    if (ptHoken.IsKouki && sinDate >= KaiseiDate.d20221001)
                                    {
                                        if (ptHoken.KogakuKbn == 41)
                                        {
                                            biko = OutDrugUtil.AppendStr(biko, "（高８）");
                                        }
                                        else
                                        {
                                            biko = OutDrugUtil.AppendStr(biko, "（高９）");
                                        }
                                    }
                                    else
                                    {
                                        biko = OutDrugUtil.AppendStr(biko, "（高一）");
                                    }
                                }
                            }
                            #endregion

                            #region 未就学
                            if (!CIUtil.IsStudent(ptInf.Birthday, sinDate) && i == 0)
                            {
                                biko = OutDrugUtil.AppendStr(biko, "（６歳就学前）");
                            }
                            #endregion

                            #region マル長
                            int marucyo = finder.ExistMarucyo(hpId, ptId, sinDate, hokenId);

                            if ((ptKohis.Any(p => p.HokenSbtKbn == 2 && p.HokenEdaNo == 0)) || (marucyo == 1))
                            {
                                biko = OutDrugUtil.AppendStr(biko, "（長）");
                            }
                            else if ((ptKohis.Any(p => p.HokenSbtKbn == 2 && p.HokenEdaNo == 1)) || (marucyo == 2))
                            {
                                biko = OutDrugUtil.AppendStr(biko, "（長２）");
                            }
                            #endregion

                            #region 都道府県別処理

                            // 作業用
                            string tmp = string.Empty;

                            switch (hpInf.PrefNo)
                            {
                                case PrefCode.Gifu:
                                    #region 岐阜
                                    tmp = string.Empty;
                                    for (int j = ptKohis.Count - 1; j >= 0; j--)
                                    {
                                        tmp += ptKohis[j].TokusyuNo;
                                    }
                                    if (tmp != string.Empty)
                                    {
                                        biko = OutDrugUtil.AppendStr(biko, tmp);
                                    }
                                    #endregion
                                    break;
                                case PrefCode.Shizuoka:
                                    #region 静岡
                                    tmp = string.Empty;
                                    for (int j = ptKohis.Count - 1; j >= 0; j--)
                                    {
                                        if (ptKohis[j].TokusyuNo != string.Empty)
                                        {
                                            tmp = ptKohis[j].TokusyuNo;
                                        }
                                    }
                                    if (tmp != string.Empty)
                                    {
                                        biko = OutDrugUtil.AppendStr(biko, tmp);
                                    }
                                    #endregion
                                    break;
                                case PrefCode.Aichi:
                                    #region 愛知
                                    if (ptKohis.Any(p => p.FutansyaNo == "99000000"))
                                    {
                                        biko = OutDrugUtil.AppendStr(biko, "マル特");
                                    }

                                    tmp = string.Empty;
                                    for (int j = ptKohis.Count - 1; j >= 0; j--)
                                    {
                                        tmp += ptKohis[j].TokusyuNo;
                                    }
                                    if (tmp != string.Empty)
                                    {
                                        biko = OutDrugUtil.AppendStr(biko, tmp);
                                    }
                                    #endregion
                                    break;
                                case PrefCode.Kyoto:
                                    #region 京都
                                    if (ptKohis.Any(p => p.HokenNo == 127 && p.PrefNo == PrefCode.Kyoto))
                                    {
                                        // 京都重障老人
                                        biko = OutDrugUtil.AppendStr(biko, "(老)重障老人");
                                    }
                                    #endregion
                                    break;
                                case PrefCode.Osaka:
                                    #region 大阪
                                    List<CoPtKohiModel> ptKohi = ptKohis.FindAll(p => new int[] { 198, 298 }.Contains(p.HokenNo));
                                    if (ptKohi.Any())
                                    {
                                        biko = OutDrugUtil.AppendStr(biko, $"（　）主病{ptKohi.First().TokusyuNo}");
                                    }
                                    #endregion
                                    break;
                                case PrefCode.Hyogo:
                                    #region 兵庫

                                    #region sub method
                                    void _addBikoMonthLimit(List<string> houbetus, string kigo)
                                    {

                                        ptKohi = ptKohis.FindAll(p => houbetus.Contains(CIUtil.Copy(p.FutansyaNo, 1, 2)));
                                        if (ptKohi.Any())
                                        {
                                            if (ptKohi.First().DayLimitFutan > 0)
                                            {
                                                biko = OutDrugUtil.AppendStr(biko, $"{kigo}{ptKohi.First().DayLimitFutan}円");
                                            }
                                            else
                                            {
                                                biko = OutDrugUtil.AppendStr(biko, $"{kigo}{ptKohi.First().MonthLimitFutan}円");
                                            }
                                        }
                                    }
                                    void _addBikoRateLimit(List<string> houbetus, string kigo)
                                    {
                                        ptKohi = ptKohis.FindAll(p => houbetus.Contains(CIUtil.Copy(p.FutansyaNo, 1, 2)));
                                        if (ptKohi.Any())
                                        {
                                            tmp = kigo;

                                            if (ptKohi.First().FutanRate > 0)
                                            {
                                                tmp += $"{ptKohi.First().FutanRate / 10}割";
                                            }
                                            if (ptKohi.First().DayLimitFutan > 0)
                                            {
                                                tmp += $"{ptKohi.First().DayLimitFutan}円";
                                            }
                                            else if (ptKohi.First().MonthLimitFutan > 0)
                                            {
                                                tmp += $"{ptKohi.First().MonthLimitFutan}円";
                                            }
                                            biko = OutDrugUtil.AppendStr(biko, tmp);
                                        }
                                    }
                                    #endregion

                                    _addBikoMonthLimit(new List<string> { "43", "44", "82", "83" }, "（障）");
                                    _addBikoMonthLimit(new List<string> { "58", "59", "68", "69" }, "（高）");
                                    _addBikoMonthLimit(new List<string> { "80", "81" }, "（乳）");
                                    _addBikoMonthLimit(new List<string> { "84", "85" }, "（母）");

                                    _addBikoRateLimit(new List<string> { "41", "42" }, "（老）");
                                    _addBikoRateLimit(new List<string> { "47", "48" }, "（こども）");

                                    #endregion
                                    break;
                                case PrefCode.Kumamoto:
                                    #region 熊本
                                    for (int j = ptKohis.Count - 1; j >= 0; j--)
                                    {
                                        if (ptKohis[j].HokenNo == 141)
                                        {
                                            biko = OutDrugUtil.AppendStr(biko, $"子ども：{ptKohis[j].JyukyusyaNo}"); ;
                                        }
                                        else if (ptKohis[j].HokenNo == 142)
                                        {
                                            biko = OutDrugUtil.AppendStr(biko, $"重障：{ptKohis[j].JyukyusyaNo}"); ;
                                        }
                                        else if (ptKohis[j].HokenNo == 143)
                                        {
                                            biko = OutDrugUtil.AppendStr(biko, $"ひとり親：{ptKohis[j].JyukyusyaNo}"); ;
                                        }
                                    }
                                    #endregion
                                    break;
                            }

                            #endregion

                            if (biko != string.Empty)
                            {
                                bikos.Add(biko);
                            }

                            // 備考レコード生成（1レコード100byteまで）
                            foreach (string bikoLine in bikos)
                            {
                                string line = bikoLine;
                                while (line != string.Empty)
                                {
                                    tmp = line;
                                    if (CIUtil.LenB(line) > 100)
                                    {
                                        tmp = CIUtil.CiCopyStrWidth(line, 1, 100);
                                    }

                                    qrData.QR081.Add(tmp);

                                    line = CIUtil.CiCopyStrWidth(line, CIUtil.LenB(tmp) + 1, CIUtil.LenB(line) - CIUtil.LenB(tmp));
                                }
                            }

                            // 紙用（紙の分はあとで適当な幅で分割する処理を行う）
                            printData.Biko = bikos;
                            #endregion

                            qrData.QRRps = new List<CoOutDrugQRRp>();
                            printData.RpInfs = new List<CoOutDrugPrintDataRpInf>();

                            int rpNo = 1;
                            foreach (CoOdrInfModel odrInf in filteredOdrInfs.FindAll(p => p.OdrKouiKbn != 100 && p.OdrKouiKbn != 101))
                            {
                                // Detail絞り込み
                                filteredOdrInfDtls = odrInfDtls.FindAll(p => p.RpNo == odrInf.RpNo && p.RpEdaNo == odrInf.RpEdaNo);

                                // Detailがない場合は次へ
                                if (!filteredOdrInfDtls.Any()) continue;

                                if (bunkatuKai > 1 && !filteredOdrInfDtls.Any(p => p.BunkatuCount >= bunkatuKai))
                                {
                                    // 分割２回目以降の場合、分割２回目以降の指示があるRp以外無視
                                    continue;
                                }

                                // QRデータ初期化
                                CoOutDrugQR101 qr101 = null;
                                CoOutDrugQR102 qr102 = null;

                                // 用法
                                CoOutDrugQR111 qr111 = new CoOutDrugQR111(version, rpNo, "　");

                                // 用法補足
                                CoOutDrugQR181 qr181 = new CoOutDrugQR181(version, rpNo);
                                // 薬剤情報
                                List<CoOutDrugQRDrug> qrDrugs = new List<CoOutDrugQRDrug>();

                                // 紙用薬剤情報

                                // 剤型区分
                                int zaikeiKbn = GetZaikeiKbn(odrInf.OdrKouiKbn);
                                // 分割指示有無
                                bool bunkatu = false;
                                // 総調剤数量
                                int totalSuryo = 1;
                                // 調剤数量
                                int cyozaiSuryo = 1;

                                totalSuryo = odrInf.DaysCnt;
                                cyozaiSuryo = odrInf.DaysCnt;

                                if (new int[] { 1, 2 }.Contains(zaikeiKbn))
                                {
                                    // 分割調剤の項目がある場合、そちらから日数回数を取得する
                                    List<CoOdrInfDetailModel> bunkatuOdrDtl = filteredOdrInfDtls.FindAll(p => p.ItemCd == @"@BUNKATU");
                                    if (bunkatuOdrDtl.Any())
                                    {
                                        bunkatu = true;
                                        cyozaiSuryo = CIUtil.StrToIntDef(bunkatuOdrDtl.First().BunkatuKai(bunkatuKai), 1);
                                    }
                                }

                                qr101 = new CoOutDrugQR101(version, rpNo, zaikeiKbn, totalSuryo);

                                if (bunkatuMax > 1)
                                {
                                    // 分割回数が２回以上の場合、102レコードを作成
                                    qr102 = new CoOutDrugQR102(version, rpNo, cyozaiSuryo, totalSuryo);
                                }

                                // 紙用Rpデータ
                                printData.RpInfs.Add(new CoOutDrugPrintDataRpInf(rpNo, odrInf.KohiFutan));

                                // 臨時処方
                                if (_systemConfig.SyohosenRinjiKisai() == 1)
                                {
                                    if (odrInf.SyohoSbt == 1)
                                    {
                                        printData.RpInfs.Last().AddDrugInf(ItemTypeConst.NoAstComment, string.Empty, "【臨時処方】", 0, string.Empty);
                                    }
                                }

                                int seqNo = 1;

                                foreach (CoOdrInfDetailModel odrDtl in filteredOdrInfDtls)
                                {
                                    if (odrDtl.ItemCd == ItemCdConst.ChusyaJikocyu)
                                    {
                                        // 「自己注射」手技項目は印字しない
                                        continue;
                                    }
                                    else if (odrDtl.ItemCd == ItemCdConst.Con_Refill)
                                    {
                                        // リフィル処方は印字しない
                                        continue;
                                    }
                                    else if (odrDtl.DrugKbn > 0 || odrDtl.IsTokuzai)
                                    {
                                        // 薬剤または特材
                                        CoOutDrugQR201 qr201 = null;
                                        CoOutDrugQR211 qr211 = null;
                                        CoOutDrugQR231 qr231 = null;
                                        CoOutDrugQR281 qr281 = new CoOutDrugQR281(version, rpNo, seqNo);

                                        int infKbn = 0;
                                        int cdSbt = 0;
                                        string itemCd = string.Empty;
                                        string drugName = string.Empty;
                                        string printDrugName = string.Empty;
                                        double yoryo = 0;
                                        int rikika = 0;
                                        string unitName = string.Empty;

                                        if (odrDtl.DrugKbn > 0)
                                        {
                                            // 薬剤
                                            infKbn = 1;

                                            if (odrDtl.SanteiItemCd == "777770000")
                                            {
                                                infKbn = 2; // 医療材料
                                                cdSbt = 2;  // ﾚｾﾌﾟﾄ電算ｺｰﾄﾞ
                                                itemCd = odrDtl.ItemCd;
                                                drugName = odrDtl.ItemName;
                                            }
                                            else if (odrDtl.SanteiItemCd == "9999999999")
                                            {
                                                infKbn = 3; // 非保険薬
                                                cdSbt = 1; // ｺｰﾄﾞなし
                                                itemCd = string.Empty;
                                                drugName = odrDtl.ItemName;
                                            }
                                            else if (odrDtl.SyohoKbn != 3)
                                            {
                                                // 一般名処方以外
                                                cdSbt = 2;
                                                itemCd = odrDtl.ItemCd;

                                                printDrugName = odrDtl.ReceName;
                                                if (printDrugName == string.Empty)
                                                {
                                                    printDrugName = odrDtl.ItemName;
                                                }
                                            }
                                            else
                                            {
                                                // 一般名処方
                                                cdSbt = 7;
                                                itemCd = odrDtl.IpnCd + "ZZZ";
                                                drugName = odrDtl.IpnName;
                                                if (string.IsNullOrEmpty(drugName))
                                                {
                                                    drugName = odrDtl.ReceName;
                                                    if (drugName == string.Empty)
                                                    {
                                                        drugName = odrDtl.ItemName;
                                                    }
                                                    drugName = $"一般名登録なし（{drugName}）";
                                                }
                                            }

                                            yoryo = odrDtl.Suryo;
                                            if (_systemConfig.SyohosenTani() == 1)
                                            {
                                                // レセ単位で出力する場合
                                                if (odrDtl.TermVal > 0)
                                                {
                                                    // 単位を変換
                                                    yoryo = Math.Round(odrDtl.Suryo * odrDtl.TermVal, 5, MidpointRounding.AwayFromZero);
                                                }

                                                rikika = 1;
                                                unitName = odrDtl.TenMst.ReceUnitName;
                                            }
                                            else
                                            {
                                                if (odrDtl.TermVal != 0 && odrDtl.TermVal != 1)
                                                {
                                                    // 力価単位
                                                    rikika = 2;
                                                    // 係数レコード
                                                    qr211 = new CoOutDrugQR211(version, rpNo, seqNo, Math.Round(odrDtl.TermVal, 5, MidpointRounding.AwayFromZero));
                                                }
                                                else
                                                {
                                                    rikika = 1;
                                                }

                                                unitName = odrDtl.UnitName;
                                            }

                                            qr201 = new CoOutDrugQR201(version, rpNo, seqNo, infKbn, cdSbt, itemCd, drugName, yoryo, rikika, unitName);

                                            // 一般名処方、後発品変更可否チェック
                                            string henkoMark = string.Empty;
                                            int yakuhinHosokuKbn = 0;
                                            string yakuhinHosokuInf = string.Empty;
                                            int yakuhinHosokuKbn2 = 0;
                                            string yakuhinHosokuInf2 = string.Empty;
                                            string printHosokuInf = string.Empty;

                                            if (odrDtl.SyohoKbn == 1)
                                            {
                                                yakuhinHosokuKbn = 3;
                                                yakuhinHosokuInf = "後発品変更不可";
                                                henkoMark = "×";
                                            }
                                            else if (new int[] { 2, 3 }.Contains(odrDtl.SyohoKbn))
                                            {
                                                if (odrDtl.SyohoLimitKbn == 1)
                                                {
                                                    yakuhinHosokuKbn = 4;
                                                    yakuhinHosokuInf = "剤型変更不可";
                                                    printHosokuInf = "（剤型変更不可）";
                                                }
                                                else if (odrDtl.SyohoLimitKbn == 2)
                                                {
                                                    yakuhinHosokuKbn = 5;
                                                    yakuhinHosokuInf = "含有規格変更不可";
                                                    printHosokuInf = "（含有規格変更不可）";

                                                }
                                                else if (odrDtl.SyohoLimitKbn == 3)
                                                {
                                                    yakuhinHosokuKbn = 4;
                                                    yakuhinHosokuInf = "剤型変更不可";
                                                    yakuhinHosokuKbn2 = 5;
                                                    yakuhinHosokuInf2 = "含有規格変更不可";
                                                    printHosokuInf = "（含有規格及び、剤型変更不可）";
                                                }
                                            }

                                            if (printDrugName == string.Empty) printDrugName = drugName;

                                            printData.RpInfs.Last().AddDrugInf(ItemTypeConst.Item, henkoMark, printDrugName, yoryo, unitName);

                                            if (yakuhinHosokuKbn > 0)
                                            {
                                                qr281.Add(yakuhinHosokuKbn, yakuhinHosokuInf);
                                            }
                                            if (yakuhinHosokuKbn2 > 0)
                                            {
                                                qr281.Add(yakuhinHosokuKbn2, yakuhinHosokuInf2);
                                            }

                                            if (printHosokuInf != string.Empty)
                                            {
                                                printData.RpInfs.Last().AddDrugInf(ItemTypeConst.NoAstComment, string.Empty, printHosokuInf, 0, string.Empty);
                                            }

                                            // 公費負担
                                            if (qrData.IsHeiyo)
                                            {
                                                qr231 = new CoOutDrugQR231(
                                                    version,
                                                    rpNo, seqNo,
                                                    (odrInf.Kohi1Futan > 0 ? 1 : 0),
                                                    (odrInf.Kohi2Futan > 0 ? 1 : 0),
                                                    (odrInf.Kohi3Futan > 0 ? 1 : 0),
                                                    (odrInf.KohiSpFutan > 0 ? 1 : 0));
                                            }

                                        }
                                        else if (odrDtl.IsTokuzai)
                                        {
                                            // 特材
                                            infKbn = 2; // 医療材料
                                            cdSbt = 2; // ﾚｾﾌﾟﾄ電算ｺｰﾄﾞ
                                            itemCd = odrDtl.SanteiItemCd;

                                            printDrugName = odrDtl.ReceName;
                                            if (printDrugName == string.Empty)
                                            {
                                                printDrugName = odrDtl.ItemName;
                                            }

                                            if (itemCd == "777770000")
                                            {
                                                drugName = odrDtl.ItemName;
                                            }
                                            else if (itemCd == "9999999999")
                                            {
                                                infKbn = 0; // 不明
                                                cdSbt = 1; // ｺｰﾄﾞなし
                                                itemCd = string.Empty;
                                                drugName = odrDtl.ItemName;
                                            }

                                            yoryo = odrDtl.Suryo;
                                            rikika = 1;
                                            unitName = odrDtl.UnitName;

                                            qr201 = new CoOutDrugQR201(version, rpNo, seqNo, infKbn, cdSbt, itemCd, drugName, yoryo, rikika, unitName);

                                            printData.RpInfs.Last().AddDrugInf(ItemTypeConst.Item, string.Empty, printDrugName, yoryo, unitName);
                                        }
                                        qrDrugs.Add(new CoOutDrugQRDrug(qr201, qr211, qr231, qr281));
                                        seqNo++;
                                    }
                                    else
                                    {
                                        // その他
                                        if (odrDtl.ItemCd == @"@BUNKATU")
                                        {
                                            // 分割
                                            string comment = string.Empty;
                                            if (odrInf.OdrKouiKbn == 21)
                                            {
                                                // 内服の場合は日数
                                                comment = CIUtil.ToWide($"(総投与日数{totalSuryo}日)");
                                            }
                                            else
                                            {
                                                // 頓服、外用は回数
                                                comment = CIUtil.ToWide($"(総投与回数{totalSuryo}回)");
                                            }
                                            qr181.Add(CIUtil.ToWide(comment));
                                        }
                                        else if (odrDtl.YohoKbn == 1)
                                        {
                                            // 用法
                                            string comment = CIUtil.ToWide(odrDtl.ItemName);

                                            qr111.YohoName = comment;
                                            printData.RpInfs.Last().AddDrugInf(ItemTypeConst.Yoho, string.Empty, comment, cyozaiSuryo, odrDtl.UnitName);

                                            // 紙用、分割レコード、用法の次に来るようにここで生成
                                            if (bunkatu && bunkatuMax > 1)
                                            {
                                                if (odrInf.OdrKouiKbn == 21)
                                                {
                                                    // 内服の場合は日数
                                                    comment = CIUtil.ToWide($"(総投与日数{totalSuryo}日)");
                                                }
                                                else
                                                {
                                                    // 頓服、外用は回数
                                                    comment = CIUtil.ToWide($"(総投与回数{totalSuryo}回)");
                                                }
                                                printData.RpInfs.Last().AddDrugInf(ItemTypeConst.Bunkatu, string.Empty, comment, 0, string.Empty);
                                            }

                                        }
                                        else
                                        {
                                            // 補助用法、コメント
                                            string comment = CIUtil.ToWide(odrDtl.ItemName);
                                            qr181.Add(comment);

                                            if (odrDtl.YohoKbn == 2)
                                            {
                                                // 第二用法
                                                printData.RpInfs.Last().AddDrugInf(ItemTypeConst.Hojyo, string.Empty, comment, odrDtl.Suryo, odrDtl.UnitName);
                                            }
                                            else
                                            {
                                                // その他
                                                printData.RpInfs.Last().AddDrugInf(ItemTypeConst.Comment, string.Empty, comment, 0, string.Empty);
                                            }
                                        }
                                    }
                                }

                                qrData.QRRps.Add(new CoOutDrugQRRp(qr101, qr102, qr111, qr181, qrDrugs));


                                rpNo++;
                            }

                            // 処方コメント
                            foreach (CoOdrInfModel odrInf in filteredOdrInfs.FindAll(p => p.OdrKouiKbn == 100))
                            {
                                if (bunkatuKai > 1 && !filteredOdrInfDtls.Any(p => p.BunkatuCount >= bunkatuKai))
                                {
                                    // 分割２回目以降の場合、分割２回目以降の指示があるRp以外無視
                                    continue;
                                }

                                printData.RpInfs.Add(new CoOutDrugPrintDataRpInf(rpNo, 999));

                                filteredOdrInfDtls = odrInfDtls.FindAll(p => p.RpNo == odrInf.RpNo && p.RpEdaNo == odrInf.RpEdaNo);

                                if (!filteredOdrInfDtls.Any()) continue;

                                string syohoComment = string.Empty;
                                foreach (CoOdrInfDetailModel odrDtl in filteredOdrInfDtls.FindAll(p => p.ItemCd != ItemCdConst.Con_Refill))
                                {
                                    syohoComment += odrDtl.ItemName;
                                    if (odrDtl.CommentNewLine == 1)
                                    {
                                        // 改行
                                        printData.RpInfs.Last().AddDrugInf(ItemTypeConst.Comment, string.Empty, syohoComment, 0, string.Empty);
                                        syohoComment = string.Empty;
                                    }
                                }

                                if (!string.IsNullOrEmpty(syohoComment))
                                {
                                    printData.RpInfs.Last().AddDrugInf(ItemTypeConst.Comment, string.Empty, syohoComment, 0, string.Empty);
                                    syohoComment = string.Empty;
                                }
                            }

                            outDrugModels.Add(new CoOutDrugModel(printData, qrData));
                        }

                        if (bunkatuMax > 1)
                        {
                            // 分割指示別紙用
                            CoOutDrugPrintData printData = new CoOutDrugPrintData(OutDrugPrintOutType.Bunkatu, sinDate, ptInf, ptHoken, filteredPtKohis, hpInf, raiinInf, bunkatuMax, 0, 0);
                            outDrugModels.Add(new CoOutDrugModel(printData, null));
                        }
                        // 紙用データ作成
                    }
                }
            }
        }

        return outDrugModels;
    }

    /// <summary>
    /// 処方箋のリスト
    /// </summary>
    /// <param name="coModel"></param>
    private void MakeDataList()
    {
        #region sub method

        // リストに追加（先頭行に変更マーク）
        List<PrintOutData> _addList(string str, int maxLength, string henkouMark)
        {
            List<PrintOutData> addPrintOutData = new List<PrintOutData>();

            bool firstAdd = true;
            string line = str;

            while (line != string.Empty)
            {
                string tmp = line;
                if (CIUtil.LenB(line) > maxLength)
                {
                    // 文字列が最大幅より広い場合、カット
                    tmp = CIUtil.CiCopyStrWidth(line, 1, maxLength);
                }

                PrintOutData printOutData = new PrintOutData();
                printOutData.Data = tmp;
                addPrintOutData.Add(printOutData);

                if (firstAdd)
                {
                    // 初回追加時は、変更マークをセット
                    firstAdd = false;
                    addPrintOutData.Last().HenkoMark = henkouMark;
                }

                // 今回出力分の文字列を削除
                line = CIUtil.CiCopyStrWidth(line, CIUtil.LenB(tmp) + 1, CIUtil.LenB(line) - CIUtil.LenB(tmp));
            }

            return addPrintOutData;
        }

        // リストに追加（先頭行にアスタリスク）
        List<PrintOutData> _addListComment(string str, int maxLength)
        {
            List<PrintOutData> addPrintOutData = new List<PrintOutData>();

            // 先頭にアスタリスク
            string line = "* " + str;

            while (line != string.Empty)
            {
                string tmp = line;
                if (CIUtil.LenB(line) > maxLength)
                {
                    // 文字列が最大幅より広い場合、カット
                    tmp = CIUtil.CiCopyStrWidth(line, 1, maxLength);
                }

                PrintOutData printOutData = new();
                printOutData.Data = tmp;
                addPrintOutData.Add(printOutData);

                // 今回出力分の文字列を削除
                line = CIUtil.CiCopyStrWidth(line, CIUtil.LenB(tmp) + 1, CIUtil.LenB(line) - CIUtil.LenB(tmp));

                // インデント
                if (line != string.Empty)
                {
                    line = "  " + line;
                }
            }

            return addPrintOutData;
        }

        string _getKohiName(CoPtKohiModel ptKohi)
        {
            string kohiName = ptKohi.HokenMst.HokenName;

            int equalCount = (_dataCharCount - CIUtil.LenB(kohiName) - 18);
            int leftCount = equalCount / 2;
            int rightCount = equalCount / 2;
            if (equalCount % 2 == 1)
            {
                rightCount++;
            }
            kohiName = $"{new string('=', leftCount)}  以下、{kohiName}　適用分  {new string('=', rightCount)}";

            return kohiName;
        }

        // maxLengthで指定した幅の中央にstrが来るよう、前をスペースで埋めた文字列を返す
        string _addHalfSpace(string str, int maxLength)
        {
            string add = new string(' ', (maxLength - CIUtil.LenB(str)) / 2);
            int cut = 4;
            if (add.Length < cut)
            {
                cut = add.Length;
            }

            string ret = add + str;

            ret = CIUtil.Copy(ret, cut, ret.Length - cut + 1);
            return ret;
        }

        // このページの印字済み行数
        // 既に追加した行数 % 1ページの最大行数
        int _getPrintedLineCount()
        {
            return _dataList.Count % _dataRowCount;
        }

        // このページに印字可能な残り行数
        // 1ページの最大行数 - (既に追加した行数 % 1ページの最大行数)
        int _getRemainingLineCount()
        {
            return _dataRowCount - _getPrintedLineCount();
        }
        #endregion

        _dataList = new List<PrintOutData>();

        int kohiFutan = -1;

        for (int i = 0; i < _coModel.PrintData?.RpInfs.Count; i++)
        {
            CoOutDrugPrintDataRpInf rpInf = _coModel.PrintData.RpInfs[i];
            List<PrintOutData> addPrintOutData = new();

            // 分点チェック
            if (kohiFutan < 0)
            {
                kohiFutan = rpInf.KohiFutan;

            }
            else if (rpInf.KohiFutan == 999)
            {
                // 処方箋コメントの場合、区切り線を出力
                addPrintOutData.AddRange(_addList(new string('-', _dataCharCount), _dataCharCount, string.Empty));
            }
            else if (rpInf.KohiFutan != kohiFutan)
            {
                // 適用する公費が変わったとき、公費名称を印字する
                CoPtKohiModel ptKohi = _coModel.PrintData?.PtKohi(rpInf.KohiFutan - 1) ?? new();
                if (ptKohi != null)
                {
                    addPrintOutData.AddRange(_addList(_getKohiName(ptKohi), _dataCharCount, string.Empty));
                    addPrintOutData.Last().RpInf = "====";
                }

                kohiFutan = rpInf.KohiFutan;
            }

            // 項目を印字する
            foreach (CoOutDrugPrintDataDrugInf drugInf in rpInf.DrugInfs)
            {

                if (drugInf.ItemType == ItemTypeConst.Item)
                {
                    // 薬剤等
                    addPrintOutData.AddRange(_addList(drugInf.Data, _dataCharCount - _suryoCharCount - _unitCharCount, drugInf.HenkouMark));

                    if (addPrintOutData.Any())
                    {
                        addPrintOutData.Last().Suryo = drugInf.Suryo.ToString();
                        addPrintOutData.Last().UnitName = drugInf.UnitName;
                    }
                }
                else if (new int[] { ItemTypeConst.Yoho, ItemTypeConst.Hojyo }.Contains(drugInf.ItemType))
                {
                    // 用法
                    addPrintOutData.AddRange(_addListComment(drugInf.Data, _dataCharCount - _kaisuCharCount - _yohoUnitCharCount));
                    if (addPrintOutData.Any())
                    {
                        if (!string.IsNullOrEmpty(drugInf.UnitName.Trim()))
                        {
                            addPrintOutData.Last().Kaisu = drugInf.Suryo.ToString();
                        }
                        addPrintOutData.Last().YohoUnit = drugInf.UnitName;
                    }
                }
                else if (drugInf.ItemType == ItemTypeConst.Bunkatu)
                {
                    // 分割
                    addPrintOutData.AddRange(_addList(drugInf.Data, _dataCharCount, string.Empty));
                    if (addPrintOutData.Any())
                    {
                        addPrintOutData.Last().Data = string.Empty;
                        addPrintOutData.Last().Bunkatu = drugInf.Data.ToString();
                    }
                }
                else if (drugInf.ItemType == ItemTypeConst.NoAstComment)
                {
                    // コメント（アスタリスクなし）
                    addPrintOutData.AddRange(_addList(drugInf.Data, _dataCharCount, string.Empty));
                }
                else
                {
                    // コメント
                    addPrintOutData.AddRange(_addListComment(drugInf.Data, _dataCharCount));
                }
            }

            if ((addPrintOutData.Count + _getPrintedLineCount()) > _dataRowCount + (i + 1 == _coModel.PrintData?.RpInfs.Count ? 0 : -1))
            {
                // 追加する行数 + このページの印字済み行数 > 1ページの最大行数(最終Rpの場合は0, その他は-1する）
                // つまり、このRpのデータを追加すると、ページの行数を超えてしまう場合、
                // 区切りの文字と残り行を埋める空行を追加する
                // このRpのデータは次ページに印字する

                string addComment = string.Empty;

                if (_getRemainingLineCount() >= 1)
                {
                    addComment = _addHalfSpace("---- 次頁あり ----", _dataCharCount);
                }

                if (addComment != string.Empty)
                {
                    _dataList.Add(new PrintOutData());
                    _dataList.Last().Data = addComment;
                }

                // 追加する行数を決定する
                int addRowCount = _getRemainingLineCount();
                if (addRowCount % _dataRowCount != 0)
                {
                    for (int j = 0; j < addRowCount; j++)
                    {
                        // 空行で埋める
                        _dataList.Add(new PrintOutData());
                    }
                }
            }

            if (rpInf.KohiFutan != 999)
            {
                // 処方コメント以外の場合、Rp番号を追加する
                for (int j = 0; j < addPrintOutData.Count; j++)
                {
                    if (addPrintOutData[j].RpInf != "====")
                    {
                        // RpInf が、"===="の場合、公費の区切り行なので無視

                        addPrintOutData[j].RpInf = $"{rpInf.RpNo})";
                        break;
                    }
                }
            }

            _dataList.AddRange(addPrintOutData);

            if (_dataRowCount - (_dataList.Count % _dataRowCount) > 1)
            {
                // 1行空ける
                _dataList.Add(new PrintOutData());
            }
        }

        if (_getRemainingLineCount() >= 1)
        {
            // 残り行数が1行以上の場合、コメント追加
            _dataList.Add(new PrintOutData());
            _dataList.Last().Data = _addHalfSpace("---- 以下余白 ----", _dataCharCount);
        }
        else if (_getRemainingLineCount() == 0)
        {
            // 残り行数が0行の場合
            if (_dataList.Last().IsClearData)
            {
                // 最終行が空行の場合、コメントをセット
                _dataList.Last().Data = _addHalfSpace("---- 以下余白 ----", _dataCharCount);
            }
            else
            {
                // 空行ではない場合は次ページに
                _dataList.Add(new PrintOutData());
                _dataList.Last().Data = _addHalfSpace("---- 以下余白 ----", _dataCharCount);
            }
        }
    }/// <summary>

     /// 剤型区分の取得
     /// </summary>
     /// <param name="odrKouiKbn">オーダー行為区分</param>
     /// <returns>剤型区分</returns>
    private int GetZaikeiKbn(int odrKouiKbn)
    {
        int ret = 0;
        if (odrKouiKbn == 21)
        {
            // 内服
            ret = 1;
        }
        else if (odrKouiKbn == 22)
        {
            // 頓服
            ret = 2;
        }
        else if (odrKouiKbn == 23)
        {
            // 外用
            ret = 3;
        }
        else if (odrKouiKbn >= 30 && odrKouiKbn <= 39)
        {
            // 注射
            ret = 5;
        }
        else
        {
            // その他
            ret = 9;
        }

        return ret;
    }

    /// <summary>
    /// 備考欄のリスト
    /// </summary>
    /// <param name="coModel"></param>
    private void MakeBikoList()
    {
        _bikoList = new List<string>();

        // 行に応じた文字幅をセット
        // ※処方箋の備考欄は、狭い幅の行　->　広い幅の行という風に、行が増えると幅が広がるようになっていて、一定ではない
        List<int> maxLengths = new List<int>();

        // 幅が狭い方のリストの幅を、リストの行数分セット
        for (int i = 0; i < _bikoShortRowCount; i++)
        {
            maxLengths.Add(_bikoShortCharCount);
        }
        // 幅が広い方のリストの幅を、リストの行数分セット
        for (int i = 0; i < _bikoLongRowCount; i++)
        {
            maxLengths.Add(_bikoLongCharCount);
        }

        // maxLengthsの何番目を使うか？のインデックス
        int lengthsIndex = 0;

        // 備考欄データ作成処理
        foreach (string biko in _coModel.PrintData?.Biko ?? new())
        {
            string line = biko;
            while (line != string.Empty)
            {
                string tmp = line;
                if (CIUtil.LenB(line) > maxLengths[lengthsIndex])
                {
                    tmp = CIUtil.CiCopyStrWidth(line, 1, maxLengths[lengthsIndex]);
                }

                _bikoList.Add(tmp);

                line = CIUtil.CiCopyStrWidth(line, CIUtil.LenB(tmp) + 1, CIUtil.LenB(line) - CIUtil.LenB(tmp));

                lengthsIndex++;
                if (lengthsIndex >= maxLengths.Count)
                {
                    lengthsIndex = 0;
                }
            }
        }
    }

    private void GetFormParam(string formfile)
    {
        List<ObjectCalculate> fieldInputList = new()
        {
            new ObjectCalculate("lsData", (int)CalculateTypeEnum.GetFormatLength),
            new ObjectCalculate("lsSuryo", (int)CalculateTypeEnum.GetFormatLength),
            new ObjectCalculate("lsUnitName", (int)CalculateTypeEnum.GetFormatLength),
            new ObjectCalculate("lsKaisu", (int)CalculateTypeEnum.GetFormatLength),
            new ObjectCalculate("lsYohoUnitName", (int)CalculateTypeEnum.GetFormatLength),
            new ObjectCalculate("lsData", (int)CalculateTypeEnum.GetListRowCount),
            new ObjectCalculate("lsBikoShort", (int)CalculateTypeEnum.GetFormatLength),
            new ObjectCalculate("lsBikoShort", (int)CalculateTypeEnum.GetListRowCount),
            new ObjectCalculate("lsBikoLong", (int)CalculateTypeEnum.GetFormatLength),
            new ObjectCalculate("lsBikoLong", (int)CalculateTypeEnum.GetListRowCount)
        };

        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.OutDrug, formfile, fieldInputList);

        var javaOutputData = _readRseReportFileService.ReadFileRse(data);
        UpdateParamLocal(javaOutputData.responses ?? new());
    }

    private void UpdateParamLocal(List<ObjectCalculateResponse> result)
    {
        foreach (var item in result)
        {
            switch (item.typeInt)
            {
                case (int)CalculateTypeEnum.GetListRowCount:
                    switch (item.listName)
                    {
                        case "lsData":
                            _dataRowCount = item.result;
                            break;
                        case "lsBikoShort":
                            _bikoShortRowCount = item.result;
                            break;
                        case "lsBikoLong":
                            _bikoLongRowCount = item.result;
                            break;
                    }
                    break;
                case (int)CalculateTypeEnum.GetFormatLength:
                    switch (item.listName)
                    {
                        case "lsData":
                            _dataCharCount = item.result;
                            break;
                        case "lsSuryo":
                            _suryoCharCount = item.result;
                            break;
                        case "lsUnitName":
                            _unitCharCount = item.result;
                            break;
                        case "lsKaisu":
                            _kaisuCharCount = item.result;
                            break;
                        case "lsYohoUnitName":
                            _yohoUnitCharCount = item.result;
                            break;
                        case "lsBikoShort":
                            _bikoShortCharCount = item.result;
                            break;
                        case "lsBikoLong":
                            _bikoLongCharCount = item.result;
                            break;
                    }
                    break;
            }
        }
    }

    private Dictionary<string, string> UpdateFormHeader()
    {
        Dictionary<string, string> result = new();
        #region sub method
        // 分割
        string _getBunkatu()
        {
            string ret = string.Empty;

            if (_coModel.PrintData?.BunkatuMax > 1)
            {
                ret = $"分割指示に係る処方箋　{_coModel.PrintData.BunkatuMax}分割の{_coModel.PrintData.BunkatuKai}回目";
            }
            return ret;
        }
        // 注意
        string _getCyui()
        {
            string ret = string.Empty;

            if (_coModel.PrintData?.BunkatuMax <= 1)
            {
                ret = "（この処方箋は、どの保険薬局でも有効です。）";
            }
            return ret;
        }


        // 残薬確認のチェック
        string _getZanyakuCheck(bool check)
        {
            if (check)
            {
                return "×";
            }
            else
            {
                return string.Empty;
            }
        }

        string _getRefillCount(int count)
        {
            if (count > 0)
            {
                return count.ToString();
            }
            else
            {
                return _systemConfig.SyohosenRefillZero();
            }
        }

        // 負担率
        string _getFutanRate()
        {
            string ret = string.Empty;

            if (_systemConfig.SyohosenFutanRate() != 0)
            {
                if (_coModel.PrintData?.KohiCount > 0 && _systemConfig.SyohosenFutanRate() == 2)
                {
                    // 公費併用時印字しない設定の場合
                }
                else
                {
                    int? futanRate = _coModel.PrintData?.FutanRate;

                    if (futanRate != null)
                    {
                        ret = $"{futanRate / 10}割";
                    }
                }
            }

            return ret;
        }

        // 変更不可の薬剤がある場合、保険医署名
        string _getDoctorName()
        {
            string ret = string.Empty;

            if (_coModel.PrintData?.HenkoFuka ?? false)
            {
                ret = _coModel.PrintData?.DoctorName ?? string.Empty;
            }

            return ret;
        }
        #endregion

        // 分割
        result.Add("dfBunkatu", _getBunkatu());
        // 注意
        result.Add("dfCyui", _getCyui());
        // 医療機関住所
        result.Add("txHpAddress", _coModel.PrintData?.HpAddress ?? string.Empty);
        // 医療機関名称
        result.Add("dfHpName", _coModel.PrintData?.HpName ?? string.Empty);
        // 医療機関電話番号
        result.Add("dfHpTel", _coModel.PrintData?.HpTel ?? string.Empty);
        // 保険医師名
        result.Add("dfKaisetuName", _coModel.PrintData?.DoctorName ?? string.Empty);
        // 受付順番
        result.Add("dfUketukeNo", _coModel.PrintData?.UketukeNo.ToString() ?? string.Empty);
        // 都道府県番号
        result.Add("dfPrefNo", $"{_coModel.PrintData?.PrefNo,2}");
        // 点数表番号
        result.Add("dfTensuHyoNo", 1.ToString() ?? string.Empty);
        // 医療機関コード
        result.Add("dfHpCd", _coModel.PrintData?.HpCd.PadLeft(7, '0') ?? string.Empty);
        // 患者カナ氏名
        result.Add("dfPtKanaName", _coModel.PrintData?.PtKanaName ?? string.Empty);
        // 患者漢字氏名
        result.Add("dfPtKanjiName", _coModel.PrintData?.PtName ?? string.Empty);
        // 生年月日
        result.Add("dfBirthDay", _coModel.PrintData?.Birthday ?? string.Empty);
        // 年齢
        result.Add("dfAge", _coModel.PrintData?.Age.ToString() ?? string.Empty);
        // 交付年月日
        result.Add("dfKofuDay", _coModel.PrintData?.KofuDate ?? string.Empty);
        // 保険者番号
        result.Add("dfHokensyaNo", _coModel.PrintData?.HokensyaNo.PadLeft(8, ' ') ?? string.Empty);
        // 記号番号
        result.Add("dfKigoBango", _coModel.PrintData?.KigoBango ?? string.Empty);
        // 枝番
        result.Add("dfEdaban", _coModel.PrintData?.EdaNo ?? string.Empty);
        // 負担率
        result.Add("dfRate", _getFutanRate());
        // 公費１負担者番号
        result.Add("dfFutansyaNoK1", _coModel.PrintData?.KohiFutansyaNo(0).PadLeft(8, ' ') ?? string.Empty);
        // 公費１受給者番号
        result.Add("dfJyukyusyaNoK1", _coModel.PrintData?.KohiJyukyusyaNo(0).PadLeft(7, ' ') ?? string.Empty);
        // 公費２負担者番号
        result.Add("dfFutansyaNoK2", _coModel.PrintData?.KohiFutansyaNo(1).PadLeft(8, ' ') ?? string.Empty);
        // 公費２受給者番号
        result.Add("dfJyukyusyaNoK2", _coModel.PrintData?.KohiJyukyusyaNo(1).PadLeft(7, ' ') ?? string.Empty);
        // 残薬確認ー疑義
        result.Add("dfCheckGigi", _getZanyakuCheck(_coModel.PrintData?.ZanyakuGigi ?? false));
        // 残薬確認ー情報提供
        result.Add("dfCheckTeikyo", _getZanyakuCheck(_coModel.PrintData?.ZanyakuTeikyo ?? false));
        // リフィル回数
        result.Add("dfRefill", _getRefillCount(_coModel.PrintData?.RefillCount ?? 0));
        // 患者番号
        result.Add("dfPtNum", _coModel.PrintData?.PtNum.ToString() ?? string.Empty);
        // 変更不可の薬剤がある場合、署名
        result.Add("dfDoctorName", _getDoctorName());

        return result;
    }
}
