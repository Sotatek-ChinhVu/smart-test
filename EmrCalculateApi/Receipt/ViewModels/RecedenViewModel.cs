using EmrCalculateApi.Receipt.DB.Finder;
using EmrCalculateApi.Ika.DB.Finder;
using EmrCalculateApi.Ika.Models;
using Helper.Constants;
using EmrCalculateApi.Receipt.Models;
using EmrCalculateApi.Receipt.Constants;
using PostgreDataContext;
using Infrastructure.Interfaces;
using EmrCalculateApi.Interface;
using Helper.Common;
using Domain.Constant;
using EmrCalculateApi.Constants;

namespace EmrCalculateApi.Receipt.ViewModels
{
    public class RecedenViewModel: IDisposable
    {
        private const string ModuleName = ModuleNameConst.EmrCalculateReceipt;

        List<ReceiptDataModel> _recedenViewModels;
        PtFinder _ptFinder;
        HokenFinder _hokenFinder;
        ReceMasterFinder _receMasterFinder;
        DB.Finder.KaikeiFinder _kaikeiFinder;

        private SanteiFinder _santeiFinder;

        //private DBContextFactory _dbService;
        //public DBContextFactory DbService
        //{
        //    get => _dbService;
        //    set
        //    {
        //        if (_dbService != null)
        //        {
        //            __tenantDataContext.Dispose();
        //        }

        //        _dbService = value;
        //        _ptFinder = new PtFinder(value);
        //        _hokenFinder = new HokenFinder(value);
        //        _receMasterFinder = new ReceMasterFinder(value);
        //        _santeiFinder = new SanteiFinder(value);
        //        _kaikeiFinder = new DB.Finder.KaikeiFinder(value);
        //    }
        //}
        private readonly ITenantProvider _tenantProvider;
        private readonly TenantDataContext _tenantDataContext;
        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;
        public RecedenViewModel(ITenantProvider tenantProvider, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
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
        /// レセプト電算データ取得
        /// </summary>
        /// <param name="mode">
        /// 0:社保
        /// 1:国保
        /// 2:労災
        /// 3:アフターケア
        /// </param>
        /// <param name="sort">
        ///     0: 診療年月、患者番号
        ///     1: 診療年月、カナ氏名
        ///     2: 診療年月、保険者番号
        ///     3: 診療年月、レセ種別、カナ氏名
        /// </param>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="seikyuYM">請求月（システム用）</param>
        /// <param name="outputYM">請求月（記録用）</param>
        /// <param name="seikyuKbnMode">
        /// 0: 今回、月遅れ分のみ
        /// 1: オンライン返戻分のみ
        /// 2: 今回、月遅れ、オンライン返戻
        /// </param>
        /// <param name="kaId">診療科ID　0:未指定</param>
        /// <param name="tantoId">担当医ID　0:未指定</param>
        /// <param name="includeTester">true:テスト患者を含める</param>
        /// <param name="includeOutDrug">true:院外処方を含める</param>
        /// <returns>レセプト電算に記録する文字列（ファイル分リストで返す）</returns>
        public List<string> GetRecedenData(int mode, int sort, int hpId, int seikyuYM, int outputYM, int seikyuKbnMode, int kaId, int tantoId, bool includeTester, bool includeOutDrug)
        {
            const string conFncName = nameof(GetRecedenData);
            _emrLogger.WriteLogStart(this, conFncName, 
                $"mode:{mode} sort:{sort} hpId:{hpId} seikyuYM:{seikyuYM} outputYM:{outputYM} seikyuKbnMode:{seikyuKbnMode} kaId:{kaId} tantoId:{tantoId} IncludeTester{includeTester} IncludeOutDrug:{includeOutDrug}");

            string ret = "";
            List<string> retls = new List<string>();
            
            int repeatCount = 0;
            if(mode == 2)
            {
                repeatCount = 1;
            }

            try
            {

                // 病院情報取得
                HpInfModel hpInf = _receMasterFinder.FindHpInf(hpId, seikyuYM * 100 + 1);

                // 対象患者取得
                List<List<int>> seikyuKbns =
                    new List<List<int>>
                    {
                        new List<int>{ SeikyuKbnConst.Normal, SeikyuKbnConst.Tukiokure },
                        new List<int>{ SeikyuKbnConst.OnlineHenrei },
                        new List<int>{ SeikyuKbnConst.Normal, SeikyuKbnConst.Tukiokure, SeikyuKbnConst.OnlineHenrei }
                    };

                for (int i = 0; i <= repeatCount; i++)
                {
                    _recedenViewModels = new List<ReceiptDataModel>();

                    //int receiptNo = 0;
                    int getMode = mode;
                    int recordCount = 0;
                    int totalTen = 0;
                    int multiVolume = 0;

                    if(mode >= 3)
                    {
                        getMode = mode + 1;
                    }
                    else if (mode == 2 && i == 1)
                    {
                        // 労災レセ電で、ループ2回目の場合は2回目以降分を取得する
                        getMode = 3;
                    }
                    List<ReceInfModel> receInfModels = 
                        _ptFinder.FindReceInf(getMode, hpId, seikyuYM, kaId, tantoId, includeTester, 1, seikyuKbns[seikyuKbnMode])
                        //.OrderBy(p => p.SinYm)
                        //.ThenBy(p => p.PtId)
                        .ToList();

                    //List<SinRpInfModel> sinRpInfModels = _santeiFinder.FindSinRpInfDataNoTrack(hpId, seikyuYM);
                    //List<SinKouiModel> sinKouiModels = _santeiFinder.FindSinKouiDataNoTrack(hpId, seikyuYM);
                    //List<SinKouiDetailModel> sinKouiDetailModels = _santeiFinder.FindSinKouiDetailDataNoTrack(hpId, seikyuYM);
                    //List<SinKouiCountModel> sinKouiCountModels = _santeiFinder.FindSinKouiCountDataNoTrack(hpId, seikyuYM);
                    List<SinRpInfModel> sinRpInfModels = _santeiFinder.FindSinRpInfDataForRece(hpId, seikyuYM, mode, includeTester, seikyuKbns[seikyuKbnMode], kaId, tantoId);
                    List<SinKouiModel> sinKouiModels = _santeiFinder.FindSinKouiDataForRece(hpId, seikyuYM, mode, includeTester, seikyuKbns[seikyuKbnMode], kaId, tantoId);
                    List<SinKouiDetailModel> sinKouiDetailModels = _santeiFinder.FindSinKouiDetailDataForRece(hpId, seikyuYM, mode, includeTester, seikyuKbns[seikyuKbnMode], kaId, tantoId);
                    List<SinKouiCountModel> sinKouiCountModels = _santeiFinder.FindSinKouiCountDataForRece(hpId, seikyuYM, mode, includeTester, seikyuKbns[seikyuKbnMode], kaId, tantoId);

                    // ソート
                    if (mode == 2 && i == 0)
                    {
                        // 労災初回分は、労働局コード順
                        switch (sort)
                        {
                            case 0:
                                receInfModels =
                                    receInfModels
                                    .OrderBy(p=>p.RoudoukyokuCd)
                                    .ThenBy(p=>p.KantokusyoCd)
                                    .ThenBy(p => p.SinYm)
                                    .ThenBy(p => p.PtNum)
                                    .ThenBy(p => p.PtId)
                                    .ThenBy(p => p.HokenId)
                                    .ToList();
                                break;
                            case 1:
                                receInfModels =
                                    receInfModels
                                    .OrderBy(p => p.RoudoukyokuCd)
                                    .ThenBy(p => p.KantokusyoCd)
                                    .ThenBy(p => p.SinYm)
                                    .ThenBy(p => p.PtInf.KanaName)
                                    .ThenBy(p => p.PtNum)
                                    .ThenBy(p => p.PtId)
                                    .ThenBy(p => p.HokenId)
                                    .ToList();
                                break;
                            case 2:
                                receInfModels =
                                    receInfModels
                                    .OrderBy(p => p.SinYm)
                                    .ThenBy(p => p.HokensyaNo)
                                    .ThenBy(p => p.HokenId)
                                    .ToList();
                                break;
                            case 3:
                                receInfModels =
                                    receInfModels
                                    .OrderBy(p => p.RoudoukyokuCd)
                                    .ThenBy(p => p.KantokusyoCd)
                                    .ThenBy(p => p.SinYm)
                                    .ThenBy(p => p.HokenKbn)
                                    .ThenBy(p => p.ReceSbt)
                                    .ThenBy(p => p.PtInf.KanaName)
                                    .ThenBy(p => p.PtNum)
                                    .ThenBy(p => p.PtId)
                                    .ThenBy(p => p.HokenId)
                                    .ToList();
                                break;
                        }
                    }
                    else
                    {
                        switch (sort)
                        {
                            case 0:
                                receInfModels =
                                    receInfModels
                                    .OrderBy(p => p.SinYm)
                                    .ThenBy(p => p.PtNum)
                                    .ThenBy(p => p.PtId)
                                    .ThenBy(p => p.HokenId)
                                    .ToList();
                                break;
                            case 1:
                                receInfModels =
                                    receInfModels
                                    .OrderBy(p => p.SinYm)
                                    .ThenBy(p => p.PtInf.KanaName)
                                    .ThenBy(p => p.PtNum)
                                    .ThenBy(p => p.PtId)
                                    .ThenBy(p => p.HokenId)
                                    .ToList();
                                break;
                            case 2:
                                receInfModels =
                                    receInfModels
                                    .OrderBy(p => p.SinYm)
                                    .ThenBy(p => p.HokensyaNo)
                                    .ThenBy(p => p.HokenId)
                                    .ToList();
                                break;
                            case 3:
                                receInfModels =
                                    receInfModels
                                    .OrderBy(p => p.SinYm)
                                    .ThenBy(p => p.HokenKbn)
                                    .ThenBy(p => p.ReceSbt)
                                    .ThenBy(p => p.PtInf.KanaName)
                                    .ThenBy(p => p.PtNum)
                                    .ThenBy(p => p.PtId)
                                    .ThenBy(p => p.HokenId)
                                    .ToList();
                                break;
                        }
                    }

                    // ループ
                    
                    if (mode != 3)
                    {
                        ReceiptDataModel retRecedenView;

                        foreach (ReceInfModel receInfModel in receInfModels)
                        {
                            // レセプトデータを取得する

                            //if (receInfModel.SeikyuYm == receInfModel.SinYm)
                            //{
                            List<SinRpInfModel> filteredSinRpInfs = sinRpInfModels.FindAll(p => p.PtId == receInfModel.PtId && p.SinYm == receInfModel.SinYm);
                            List<SinKouiModel> filteredSinKouis = sinKouiModels.FindAll(p => p.PtId == receInfModel.PtId && p.SinYm == receInfModel.SinYm);
                            List<SinKouiDetailModel> filteredSinKouiDetails = sinKouiDetailModels.FindAll(p => p.PtId == receInfModel.PtId && p.SinYm == receInfModel.SinYm);
                            List<SinKouiCountModel> filteredSinKouiCounts = sinKouiCountModels.FindAll(p => p.PtId == receInfModel.PtId && p.SinYm == receInfModel.SinYm);

                            retRecedenView = GetReceiptData(
                                mode, includeOutDrug, outputYM, seikyuYM, hpInf.PrefNo, receInfModel, filteredSinRpInfs, filteredSinKouis, filteredSinKouiDetails, filteredSinKouiCounts);
                            //}
                            //else
                            //{
                            //    retRecedenView = GetReceiptData(mode, IncludeOutDrug, receInfModel, null, null, null, null);
                            //}

                            if (retRecedenView.SinMeiCount <= 0)
                            {
                                _emrLogger.WriteLogMsg(this, conFncName, string.Format("明細なし　ptId:{0} sinYM{1}", receInfModel.PtId, receInfModel.SinYm));
                            }
                            else
                            {
                                _recedenViewModels.Add(retRecedenView);
                            }
                        }
                    }
                    else
                    {
                        // アフターケア
                        List<ReceiptDataModel> retRecedenViews;

                        foreach (ReceInfModel receInfModel in receInfModels)
                        {
                            // レセプトデータを取得する
                            List<SinRpInfModel> filteredSinRpInfs = sinRpInfModels.FindAll(p => p.PtId == receInfModel.PtId && p.SinYm == receInfModel.SinYm);
                            List<SinKouiModel> filteredSinKouis = sinKouiModels.FindAll(p => p.PtId == receInfModel.PtId && p.SinYm == receInfModel.SinYm);
                            List<SinKouiDetailModel> filteredSinKouiDetails = sinKouiDetailModels.FindAll(p => p.PtId == receInfModel.PtId && p.SinYm == receInfModel.SinYm);
                            List<SinKouiCountModel> filteredSinKouiCounts = sinKouiCountModels.FindAll(p => p.PtId == receInfModel.PtId && p.SinYm == receInfModel.SinYm);

                            retRecedenViews = GetReceiptDataAfterCare(
                                mode, includeOutDrug, outputYM, receInfModel, filteredSinRpInfs, filteredSinKouis, filteredSinKouiDetails, filteredSinKouiCounts);

                            foreach (ReceiptDataModel retRecedenView in retRecedenViews)
                            {
                                if (retRecedenView.SinMeiCount <= 0)
                                {
                                    _emrLogger.WriteLogMsg(this, conFncName, string.Format("明細なし　ptId:{0} sinYM{1}", receInfModel.PtId, receInfModel.SinYm));
                                }
                                else
                                {
                                    _recedenViewModels.Add(retRecedenView);
                                }
                            }
                        }
                    }

                    // レセ電データ作成
                    if (_recedenViewModels.Any())
                    {
                        if (new int[] { 0, 1 }.Contains(mode))
                        {
                            int _getSeikyuYm()
                            {
                                int result = outputYM;
                                if(outputYM < 202005)
                                {
                                    result = CIUtil.SDateToWDate(outputYM * 100 + 1) / 100;
                                }
                                return result;
                            }

                            // IRレコード
                            ret += string.Format("IR,{0},{1:D2},1,{2:D7},,{3},{4},{5:D2},{6}",
                                mode + 1,
                                hpInf.PrefNo,
                                hpInf.HpCd,
                                CIUtil.Copy(CIUtil.ToWide(hpInf.ReceHpName), 1, 20),
                                //CIUtil.SDateToWDate(outputYM * 100 + 1) / 100,
                                _getSeikyuYm(),
                                multiVolume,
                                hpInf.Tel);

                            // レセプトデータ
                            int receiptNo = 0;
                            foreach (ReceiptDataModel receData in _recedenViewModels)
                            {
                                receiptNo++;
                                receData.ReceiptNo = receiptNo;

                                ret += "\r\n" + receData.RecedenRecord;

                                recordCount += receData.RecordCount;
                                totalTen += receData.TotalTen;
                            }

                            // GOレコード
                            ret += "\r\n" + string.Format("GO,{0},{1},{2:D2}", recordCount, totalTen, multiVolume == 0 ? 99 : multiVolume) + "\r\n";
                            retls.Add(ret);
                            ret = "";
                        }
                        else if(mode == 2)
                        {
                            // 労災

                            string roudouKyokuCd = "";
                            string kantokuSyoCd = "";
                            int enten = 0;
                            int totalKingaku = 0;
                            int receiptCount = 0;
                            int receiptNo = 0;
                            bool first = true;

                            // レセプトデータ
                            foreach (ReceiptDataModel receData in _recedenViewModels)
                            {
                                if (first == true || (i == 0 && (roudouKyokuCd != receData.RoudouCd || kantokuSyoCd != receData.KantokuCd)))
                                {
                                    first = false;

                                    // 労災初回分の場合、労働局コード or 監督署コードが異なれば別ファイルに保存する
                                    if (ret != "")
                                    {
                                        // RSレコード
                                        ret += "\r\n" +
                                            GetRSRecord(
                                                i,
                                                roudouKyokuCd, kantokuSyoCd,
                                                hpInf.RousaiHpCd, hpInf.Address1 + hpInf.Address2, hpInf.KaisetuName,
                                                enten, totalKingaku,
                                                receiptCount, multiVolume) + "\r\n";
                                        retls.Add(ret);
                                        ret = "";
                                    }

                                    int maxSinYm = CIUtil.SDateToWDate(outputYM * 100 + 1) / 100;
                                    if(i == 0)
                                    {
                                        if (_recedenViewModels.Any(p => p.RoudouCd == receData.RoudouCd))
                                        {
                                            maxSinYm = _recedenViewModels.Where(p => p.RoudouCd == receData.RoudouCd && p.KantokuCd == receData.KantokuCd).Max(p => p.SinYm);
                                        }
                                    }
                                    else
                                    {
                                        if (_recedenViewModels.Any())
                                        {
                                            maxSinYm = _recedenViewModels.Max(p => p.SinYm);
                                        }
                                    }
                                    
                                    if (outputYM < 202005)
                                    {
                                        maxSinYm = CIUtil.SDateToWDate(maxSinYm * 100 + 1) / 100;
                                    }
                                    
                                    // IRレコード
                                    ret += string.Format("IR,,{0:D2},1,{1:D7},,{2},{3},{4:D2},{5}",
                                        hpInf.PrefNo,
                                        hpInf.HpCd,
                                        CIUtil.Copy(CIUtil.ToWide(hpInf.ReceHpName), 1, 20),
                                        maxSinYm,
                                        multiVolume,
                                        hpInf.Tel);

                                    roudouKyokuCd = receData.RoudouCd;
                                    kantokuSyoCd = receData.KantokuCd;
                                    totalKingaku = 0;
                                    receiptCount = 0;
                                    receiptNo = 0;
                                }

                                receiptNo++;
                                receData.ReceiptNo = receiptNo;

                                ret += "\r\n" + receData.RousaiRecedenRecord;

                                enten = receData.EnTen;

                                receiptCount++;
                                totalKingaku += receData.TotalKingaku;
                            }

                            // RSレコード
                            ret += "\r\n" +
                                GetRSRecord(
                                    i,
                                    roudouKyokuCd, kantokuSyoCd,
                                    hpInf.RousaiHpCd, hpInf.Address1 + hpInf.Address2, hpInf.KaisetuName,
                                    enten, totalKingaku,
                                    receiptCount, multiVolume) + "\r\n";
                            retls.Add(ret);
                            ret = "";
                        }
                        else if (mode == 3)
                        {
                            // アフターケア
                            int enten = 0;
                            int totalKingaku = 0;
                            int receiptCount = 0;

                            int _getSeikyuYm()
                            {
                                int result = outputYM;
                                if (outputYM < 202005)
                                {
                                    result = CIUtil.SDateToWDate(outputYM * 100 + 1) / 100;
                                }
                                return result;
                            }

                            // 最大診療年月を取得
                            int maxSinYm = CIUtil.SDateToWDate(outputYM * 100 + 1) / 100;

                            if (_recedenViewModels.Any())
                            {
                                maxSinYm = _recedenViewModels.Max(p => p.SinYm);
                            }

                            if (outputYM < 202005)
                            {
                                maxSinYm = CIUtil.SDateToWDate(maxSinYm * 100 + 1) / 100;
                            }

                            // IRレコード
                            ret += string.Format("IR,,{0:D2},1,{1:D7},,{2},{3},,{4}",
                                hpInf.PrefNo,
                                hpInf.HpCd,
                                CIUtil.Copy(CIUtil.ToWide(hpInf.ReceHpName), 1, 20),
                                maxSinYm,
                                hpInf.Tel);

                            // レセプトデータ
                            int receiptNo = 0;
                            foreach (ReceiptDataModel receData in _recedenViewModels)
                            {
                                receiptNo++;
                                receData.ReceiptNo = receiptNo;

                                ret += "\r\n" + receData.AfterCareRecedenRecord;

                                recordCount += receData.RecordCount;
                                totalTen += receData.TotalTen;

                                enten = receData.EnTen;
                                receiptCount++;
                                totalKingaku += receData.TotalKingakuAfter;
                            }

                            // ASレコード
                            ret += "\r\n" +
                                GetASRecord(
                                    i,
                                    hpInf.RousaiHpCd, hpInf.Address1 + hpInf.Address2, hpInf.KaisetuName,
                                    enten, totalKingaku,
                                    receiptCount) + "\r\n";
                            retls.Add(ret);
                            ret = "";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _emrLogger.WriteLogError(this, conFncName, e);
            }

            _emrLogger.WriteLogEnd( this, conFncName, "");

            return retls;

            #region Local Method
            string GetRSRecord(int Amode, string ARoudouKyokuCd, string AKantokuSyoCd, string ArousaiHpCd, string Aaddress, string AkaisetuName, int AEnTen, int ATotalKingaku, int AReceiptCount, int AMultiVolume)
            {
                string rsRecord = "";
                // レコード識別情報
                // 病院・診療所の区分
                rsRecord = "RS,3";
                // 請求書提出年月日
                if (outputYM < 202005)
                {
                    // 和暦
                    rsRecord += "," + CIUtil.SDateToWDate(CIUtil.DateTimeToInt(DateTime.Now));
                }
                else
                {
                    // 西暦
                    rsRecord += "," + (CIUtil.DateTimeToInt(DateTime.Now));
                }
                // 都道府県労働局コード
                // 労働基準監督署コード
                if (Amode == 0)
                {
                    rsRecord += "," + ARoudouKyokuCd;
                    rsRecord += "," + AKantokuSyoCd;
                }
                else
                {
                    rsRecord += ",,";
                }
                // 指定病院等の番号
                rsRecord += "," + ArousaiHpCd.PadLeft(7, '0');
                // 郵便番号
                rsRecord += ",";
                // 医療機関所在地
                rsRecord += "," + CIUtil.ToWide(Aaddress);
                // 医療機関責任者氏名
                rsRecord += "," + AkaisetuName;
                // 労災診療費単価
                rsRecord += "," + (AEnTen * 100).ToString();
                // 請求金額
                rsRecord += "," + ATotalKingaku.ToString();
                // 内訳書添付枚数
                rsRecord += "," + AReceiptCount.ToString();
                // マルチボリューム識別情報
                rsRecord += "," + (AMultiVolume == 0 ? 99 : AMultiVolume).ToString();

                return rsRecord;
            }

            string GetASRecord(int Amode, string ArousaiHpCd, string Aaddress, string AkaisetuName, int AEnTen, int ATotalKingaku, int AReceiptCount)
            {
                string rsRecord = "";
                // レコード識別情報
                // 病院・診療所の区分
                rsRecord = "AS,3";
                // 請求書提出年月日
                rsRecord += "," + (CIUtil.DateTimeToInt(DateTime.Now));
                // 予備１
                rsRecord += ",";
                // 予備２
                rsRecord += ",";
                // 指定病院等の番号
                rsRecord += "," + ArousaiHpCd.PadLeft(7, '0');
                // 郵便番号
                rsRecord += ",";
                // 医療機関所在地
                rsRecord += "," + CIUtil.ToWide(Aaddress);
                // 医療機関責任者氏名
                rsRecord += "," + AkaisetuName;
                // 労災診療費単価
                rsRecord += "," + (AEnTen * 100).ToString();
                // 請求金額
                rsRecord += "," + ATotalKingaku.ToString();
                // 内訳書添付枚数
                rsRecord += "," + AReceiptCount.ToString();
                // 予備３
                rsRecord += ",";

                return rsRecord;
            }
            #endregion
        }

        /// <summary>
        /// レセプトデータを取得する
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
        public ReceiptDataModel GetReceiptData(
            int mode, bool includeOutDrug, int outputYm, int seikyuYm, int prefNo,
            ReceInfModel receInf, List<SinRpInfModel> sinRpInfs, List<SinKouiModel> sinKouis, List<SinKouiDetailModel> sinDtls, List<SinKouiCountModel> sinKouiCounts)
        {
            const string conFncName = nameof(GetReceiptData);

            int hpId = receInf.HpId;
            long ptId = receInf.PtId;
            int sinYm = receInf.SinYm;

            _emrLogger.WriteLogStart( this, conFncName, $"ptid:{ptId} sinYm:{sinYm}");

            // 基本情報
            //_emrLogger.WriteLogMsg( this, conFncName, "ptInf");

            PtInfModel ptInfModel = null;

            if (receInf.PtInf != null)
            {
                ptInfModel = new PtInfModel(receInf.PtInf, sinYm * 100 + 1);
            }
            else
            {
                ptInfModel = _ptFinder.FindPtInf(hpId, ptId, sinYm * 100 + 1);
            }

            // 保険情報
            //_emrLogger.WriteLogMsg( this, conFncName, "hokenData");

            HokenDataModel hokenDataModel = null;
            if(receInf.PtHokenInf != null)
            {
                hokenDataModel = new HokenDataModel(receInf.PtHokenInf);
            }
            else if (receInf.HokenId > 0)
            {
                hokenDataModel = _hokenFinder.FindHokenData(hpId, ptId, receInf.HokenId);
            }

            if(hokenDataModel != null)
            {
                hokenDataModel.JituNissu = receInf.HokenNissu;
                hokenDataModel.TotalTen = receInf.HokenReceTensu;
                hokenDataModel.FutanKingaku = receInf.HokenReceFutan;
            }

            // 公費情報
            //_emrLogger.WriteLogMsg( this, conFncName, "kohiData");
            List<KohiDataModel> kohiDataModels = new List<KohiDataModel>();

            // 公費ID
            //List<int> kohiIds = new List<int>();
            List<KohiDataModel> kohiDatas = _hokenFinder.FindKohiData(hpId, ptId, receInf.SinYm * 100 + 1);
            if (kohiDatas.Any())
            {
                for (int i = 1; i <= 4; i++)
                {
                    if (receInf.KohiReceKisai(i) == 1 && receInf.KohiId(i) > 0)
                    {
                        //kohiIds.Add(receInf.KohiId(i));

                        //kohiDataModels.Add(_hokenFinder.FindKohiData(hpId, ptId, receInf.KohiId(i)));
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

            //int index = 0;
            //foreach (int kohiId in kohiIds)
            //{
            //    index++;

            //    kohiDataModels.Add(_hokenFinder.FindKohiData(hpId, ptId, kohiId));
            //    kohiDataModels.Last().JituNissu = receInf.KohiNissu(index);
            //    kohiDataModels.Last().TotalTen = receInf.KohiReceTensu(index);
            //    kohiDataModels.Last().FutanKingaku = receInf.KohiReceFutan(index);
            //    kohiDataModels.Last().IchibuFutan = receInf.KohiReceKyufu(index);
            //}

            // 傷病名情報            
            //_emrLogger.WriteLogMsg( this, conFncName, "syobyoData");
            List<SyobyoDataModel> syobyoDataModels = _ptFinder.FindSyobyoData(hpId, ptId, sinYm, receInf.HokenId, outputYm);

            // 診療明細情報
            SinMeiViewModel sinMeiDataModels = null;

            //_emrLogger.WriteLogMsg( this, conFncName, "sinMei");
            if (sinRpInfs != null)
            {
                sinMeiDataModels = new SinMeiViewModel(SinMeiMode.Receden, includeOutDrug, hpId, ptId, sinYm, seikyuYm, receInf, null, ptInfModel, receInf.Tokki, sinRpInfs, sinKouis, sinDtls, sinKouiCounts, _tenantProvider, _systemConfigProvider, _emrLogger);
            }
            else
            {
                // 今(2020/07/23)のところ、こちらを通ることはないはず
                sinMeiDataModels = new SinMeiViewModel(SinMeiMode.Receden, includeOutDrug, hpId, ptId, sinYm, receInf, ptInfModel, receInf.Tokki, _tenantProvider, _systemConfigProvider, _emrLogger);
            }

            // 症状詳記情報
            //_emrLogger.WriteLogMsg( this, conFncName, "syouki");
            List<SyojyoSyokiModel> syojyoSyokiModels = _ptFinder.FindSyoukiInf(hpId, ptId, sinYm, receInf.HokenId);

            // 旧姓
            //_emrLogger.WriteLogMsg( this, conFncName, "kyusei");
            PtKyuseiModel ptKyuseiModel = _ptFinder.FindPtKyusei(hpId, ptId, sinMeiDataModels.LastSinDate);

            // 返戻
            //_emrLogger.WriteLogMsg( this, conFncName, "henrei");
            RecedenRirekiInfModel recedenRirekiInfModel = null;

            if(receInf.SeikyuKbn == SeikyuKbnConst.OnlineHenrei)
            {
                // オンライン返戻
                //_emrLogger.WriteLogMsg( this, conFncName, "online");
                recedenRirekiInfModel = _ptFinder.FindRecedenRirekiInf(hpId, ptId, sinYm, receInf.PreHokenId);
            }

            // 資格情報
            SikakuJyohoDataModel sikakuJyohoDataModel = null;
            if(receInf.SinYm >= 202109)
            {
                // 2021/09から

                if (hokenDataModel != null && string.IsNullOrEmpty(hokenDataModel.EdaNo) == false)
                {
                    sikakuJyohoDataModel = new SikakuJyohoDataModel()
                    {
                        EdaNo = hokenDataModel.EdaNo
                    };
                    
                }
            }

            // 受診日
            List<JyusinbiDataModel> jyusinbiDataModels = new List<JyusinbiDataModel>();
            if (receInf.SinYm >= 202109)
            {
                // 2021/09から
                jyusinbiDataModels = _receMasterFinder.FindReceInfJd(hpId, ptId, seikyuYm, sinYm, receInf.HokenId);
            }

            // 窓口負担
            MadoguchiFutanDataModel madoguchiFutanDataModel = null;
            if (receInf.SinYm >= 202109)
            {
                // 2021/09から
                madoguchiFutanDataModel = new MadoguchiFutanDataModel();
                if(receInf.KogakuFutan == 0)
                {
                    madoguchiFutanDataModel.MadoguchiFutanKbn = 0;
                    //京都府福祉 特殊処理
                    if (prefNo == PrefCode.Kyoto &&
                        receInf.HokenKbn == HokenKbn.Syaho &&
                        receInf.TokkiContains("01"))
                    {
                        var wrkKohis = kohiDatas.Where(k =>
                            k.PtKohi.HokenId == receInf.Kohi1Id ||
                            k.PtKohi.HokenId == receInf.Kohi2Id ||
                            k.PtKohi.HokenId == receInf.Kohi3Id ||
                            k.PtKohi.HokenId == receInf.Kohi4Id
                        ).ToList();

                        if (wrkKohis.Any(p => new string[] { "43", "44", "45" }.Contains(p.PtKohi.Houbetu)))
                        {
                            madoguchiFutanDataModel.MadoguchiFutanKbn = 1;
                        }
                    }
                }
                else if(receInf.KogakuFutan >= 1)
                {
                    if(receInf.IsTasukai == 0)
                    {
                        madoguchiFutanDataModel.MadoguchiFutanKbn = 1;
                    }
                    else
                    {
                        madoguchiFutanDataModel.MadoguchiFutanKbn = 2;
                    }
                }
            }

            RousaiReceiptModel rousaiReceiptModel = null;
            AfterCareReceiptModel afterCareReceiptModel = null;
            SyobyoKeikaModel syobyoKeikaModel = null;

            if (new int[] { 2, 3 }.Contains(mode))
            {
                // 労災レセプト情報
                //_emrLogger.WriteLogMsg( this, conFncName, "rosai");
                rousaiReceiptModel =
                    new RousaiReceiptModel(hokenDataModel.PtHokenInf, _ptFinder.FindPtRousaiTenki(hpId, ptId, sinYm, receInf.HokenId), ptInfModel.PtInf, (ptKyuseiModel != null ? ptKyuseiModel.PtKyusei: null), receInf.RousaiCount, outputYm);

                if(rousaiReceiptModel != null)
                {
                    rousaiReceiptModel.JituNissu = receInf.HokenNissu;
                    rousaiReceiptModel.Syokei = receInf.RousaiIFutan / receInf.HokenMst.EnTen;
                    rousaiReceiptModel.SyokeiGaku_I = receInf.RousaiIFutan;
                    rousaiReceiptModel.SyokeiGaku_RO = receInf.RousaiRoFutan;

                    int ryoyoStartDate = hokenDataModel.PtHokenInf.RyoyoStartDate;

                    if(ryoyoStartDate > 0)
                    {
                        if(ryoyoStartDate / 100 < sinYm)
                        {
                            // 療養開始日が診療月以前の場合は、診療月1日
                            ryoyoStartDate = sinYm * 100 + 1;
                        }
                        else if(ryoyoStartDate / 100 > sinYm)
                        {
                            ryoyoStartDate = sinMeiDataModels.FirstSyosinDate;
                        }
                    }
                    else
                    {
                        // 未設定の場合、初診算定日
                        ryoyoStartDate = sinMeiDataModels.FirstSyosinDate;
                    }
                    
                    if(ryoyoStartDate == 0)
                    {
                        // 上の処理で取得できないときは診療月1日
                        ryoyoStartDate = sinYm * 100 + 1;
                    }

                    rousaiReceiptModel.RyoyoStartDate = ryoyoStartDate;
                                        
                    int ryoyoEndDate = hokenDataModel.PtHokenInf.RyoyoEndDate;

                    if (rousaiReceiptModel.Tenki == 3)
                    {
                        // 転帰が継続の場合
                        if(ryoyoEndDate / 100 != sinYm)
                        {
                            // 療養終了日が診療月ではない、または、設定なし(0)なら、診療月末日を設定
                            ryoyoEndDate = CIUtil.GetLastDateOfMonth(sinYm * 100 + 1);
                        }
                    }
                    else
                    {
                        //// 転帰が継続以外の場合
                        //if(ryoyoEndDate / 100 > sinYm)
                        //{
                        //    // 療養終了日が診療月翌月以降の場合、診療月末日を設定
                        //    ryoyoEndDate = CIUtil.GetLastDateOfMonth(sinYm * 100 + 1);
                        //}
                        //else if(ryoyoEndDate / 100 < sinYm)
                        //{
                            //// 療養終了日が診療前月以前の場合、最終診療日を設定
                            ryoyoEndDate = sinMeiDataModels.LastSinDate;
                        //}
                    }

                    if (ryoyoEndDate == 0)
                    {
                        // 上記で設定できないケースの場合、診療月末を設定
                        ryoyoEndDate = CIUtil.GetLastDateOfMonth(sinYm * 100 + 1);
                    }
                    else if(ryoyoEndDate > hokenDataModel.PtHokenInf.RyoyoEndDate && hokenDataModel.PtHokenInf.RyoyoEndDate > 0)
                    {
                        ryoyoEndDate = hokenDataModel.PtHokenInf.RyoyoEndDate;
                    }

                    rousaiReceiptModel.RyoyoEndDate = ryoyoEndDate;

                }

                // 傷病の経過
                //_emrLogger.WriteLogMsg( this, conFncName, "syobyo");
                syobyoKeikaModel = _ptFinder.FindSyobyoKeika(hpId, ptId, sinYm, receInf.HokenId);
                                
            }

            //_emrLogger.WriteLogMsg( this, conFncName, "recedenViewModel");
            ReceiptDataModel recedenViewModel = 
                new ReceiptDataModel(
                    receInf, ptInfModel, hokenDataModel, kohiDataModels, syobyoDataModels, sinMeiDataModels.SinMei,
                    syojyoSyokiModels, ptKyuseiModel, rousaiReceiptModel, syobyoKeikaModel, recedenRirekiInfModel, outputYm, afterCareReceiptModel,
                    sikakuJyohoDataModel, jyusinbiDataModels, madoguchiFutanDataModel, _emrLogger);

            _emrLogger.WriteLogEnd( this, conFncName, "");

            return recedenViewModel;
        }

        /// <summary>
        /// レセプトデータを取得する（アフターケア用）
        /// </summary>
        /// <param name="mode">
        ///     0:社保
        ///     1:国保
        ///     2:労災(初回分)
        ///     3:労災(2回目以降分)
        /// </param>
        /// <param name="includeOutDrug">1: 院外処方含む</param>
        /// <param name="outputYm">請求年月(yyyyMM)</param>
        /// <param name="receInf">レセプト情報</param>
        /// <param name="sinRpInfs"></param>
        /// <param name="sinKouis"></param>
        /// <param name="sinDtls"></param>
        /// <param name="sinKouiCounts"></param>
        /// <returns></returns>
        public List<ReceiptDataModel> GetReceiptDataAfterCare(
            int mode, bool includeOutDrug, int outputYm,
            ReceInfModel receInf, List<SinRpInfModel> sinRpInfs, List<SinKouiModel> sinKouis, List<SinKouiDetailModel> sinDtls, List<SinKouiCountModel> sinKouiCounts)
        {
            const string conFncName = nameof(GetReceiptDataAfterCare);

            int hpId = receInf.HpId;
            long ptId = receInf.PtId;
            int sinYm = receInf.SinYm;
            List<ReceiptDataModel> results = new List<ReceiptDataModel>();

            _emrLogger.WriteLogStart( this, conFncName, $"ptId:{ptId} sinYm:{sinYm}");

            // 基本情報
            //_emrLogger.WriteLogMsg( this, conFncName, "ptInf");

            PtInfModel ptInfModel = null;

            if (receInf.PtInf != null)
            {
                ptInfModel = new PtInfModel(receInf.PtInf, sinYm * 100 + 1);
            }
            else
            {
                ptInfModel = _ptFinder.FindPtInf(hpId, ptId, sinYm * 100 + 1);
            }

            // 保険情報
            //_emrLogger.WriteLogMsg( this, conFncName, "hokenData");

            HokenDataModel hokenDataModel = null;
            if (receInf.PtHokenInf != null)
            {
                hokenDataModel = new HokenDataModel(receInf.PtHokenInf);
            }
            else if (receInf.HokenId > 0)
            {
                hokenDataModel = _hokenFinder.FindHokenData(hpId, ptId, receInf.HokenId);
            }

            if (hokenDataModel != null)
            {
                hokenDataModel.JituNissu = receInf.HokenNissu;
                hokenDataModel.TotalTen = receInf.HokenReceTensu;
                hokenDataModel.FutanKingaku = receInf.HokenReceFutan;
            }

            // 公費情報
            //_emrLogger.WriteLogMsg( this, conFncName, "kohiData");
            List<KohiDataModel> kohiDataModels = new List<KohiDataModel>();

            // 公費ID
            //List<int> kohiIds = new List<int>();
            List<KohiDataModel> kohiDatas = _hokenFinder.FindKohiData(hpId, ptId, receInf.SinYm * 100 + 1);
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

            // 傷病名情報            
            //_emrLogger.WriteLogMsg( this, conFncName, "syobyoData");
            List<SyobyoDataModel> syobyoDataModels = _ptFinder.FindSyobyoData(hpId, ptId, sinYm, receInf.HokenId, outputYm);

            // 会計情報取得
            List<(int sinDate, List<long> raiinNos, int SyokeiGaku_I, int SyokeiGaku_RO, int Syokei)> kaikeiDayInfs =
                GetKaikeiDayInfs(ptId, sinYm, receInf);

            // 会計日ごとに診療明細情報を取得する
            foreach ((int sinDate, List<long> raiinNos, int SyokeiGaku_I, int SyokeiGaku_RO, int Syokei) kaikeiDayInf in kaikeiDayInfs)
            {
                // 診療明細情報
                SinMeiViewModel sinMeiDataModels = null;
                
                //_emrLogger.WriteLogMsg( this, conFncName, "sinMei");
                if (sinRpInfs != null)
                {
                    sinMeiDataModels = new SinMeiViewModel(SinMeiMode.RecedenAfter, includeOutDrug, hpId, ptId, kaikeiDayInf.sinDate, kaikeiDayInf.raiinNos, receInf, ptInfModel, receInf.Tokki, sinRpInfs, sinKouis, sinDtls, sinKouiCounts, _tenantProvider, _systemConfigProvider, _emrLogger);
                }
                else
                {
                    sinMeiDataModels = new SinMeiViewModel(SinMeiMode.RecedenAfter, includeOutDrug, hpId, ptId, kaikeiDayInf.sinDate, kaikeiDayInf.raiinNos, receInf, ptInfModel, receInf.Tokki, _tenantProvider, _systemConfigProvider, _emrLogger);
                }

                // 旧姓
                //_emrLogger.WriteLogMsg( this, conFncName, "kyusei");
                PtKyuseiModel ptKyuseiModel = _ptFinder.FindPtKyusei(hpId, ptId, kaikeiDayInf.sinDate);

                // 労災レセプト情報
                AfterCareReceiptModel afterCareReceiptModel = null;
                SyobyoKeikaModel syobyoKeikaModel = null;

                // 労災レセプト情報
                afterCareReceiptModel = 
                    new AfterCareReceiptModel(hokenDataModel.PtHokenInf, _ptFinder.FindPtRousaiTenki(hpId, ptId, sinYm, receInf.HokenId), ptInfModel.PtInf, receInf.RousaiCount, outputYm);
                syobyoKeikaModel = 
                    _ptFinder.FindSyobyoKeikaForAfterCare(hpId, ptId, kaikeiDayInf.sinDate, receInf.HokenId);

                // 症状詳記情報
                //_emrLogger.WriteLogMsg( this, conFncName, "syouki");
                List<SyojyoSyokiModel> syojyoSyokiModels = _ptFinder.FindSyoukiInf(hpId, ptId, sinYm, receInf.HokenId);

                RecedenRirekiInfModel recedenRirekiInfModel = null;
                if (receInf.SeikyuKbn == SeikyuKbnConst.OnlineHenrei)
                {
                    // オンライン返戻
                    //_emrLogger.WriteLogMsg( this, conFncName, "online");
                    recedenRirekiInfModel = _ptFinder.FindRecedenRirekiInf(hpId, ptId, sinYm, receInf.PreHokenId);
                }

                //レセプト電算モデルを作成する
                //_emrLogger.WriteLogMsg( this, conFncName, "recedenViewModel");
                ReceiptDataModel recedenViewModel =
                    new ReceiptDataModel(
                        receInf, ptInfModel, hokenDataModel, kohiDataModels, syobyoDataModels, sinMeiDataModels.SinMei,
                        syojyoSyokiModels, ptKyuseiModel, null, syobyoKeikaModel, recedenRirekiInfModel, outputYm, afterCareReceiptModel,
                        null, null, null, _emrLogger);

                // 診療日（アフターケア用）
                recedenViewModel.SinDate = kaikeiDayInf.sinDate;
                // 小計（アフターケア用）
                recedenViewModel.AfterSyokei = kaikeiDayInf.Syokei;
                // 小計イ（アフターケア用）
                recedenViewModel.AfterSyokeiGaku_I = kaikeiDayInf.SyokeiGaku_I;
                // 小計ロ（アフターケア用）
                recedenViewModel.AfterSyokeiGaku_RO = kaikeiDayInf.SyokeiGaku_RO;
                // 検査日（アフターケア用）
                recedenViewModel.KensaDate = 0;
                if (_ptFinder.ZenkaiKensaDate(hpId, ptId, kaikeiDayInf.sinDate, receInf.HokenId) == kaikeiDayInf.sinDate)
                {
                    recedenViewModel.KensaDate = kaikeiDayInf.sinDate;
                }
                // 前回検査日（アフターケア用）
                recedenViewModel.ZenkaiKensaDate = _ptFinder.ZenkaiKensaDate(hpId, ptId, kaikeiDayInf.sinDate - 1, receInf.HokenId);

                results.Add(recedenViewModel);
            }

            return results;
        }
        /// <summary>
        /// 会計日のリストを取得する
        /// </summary>
        /// <param name="ptId"></param>
        /// <param name="sinYm"></param>
        /// <param name="receInf"></param>
        /// <returns></returns>
        private List<(int sinDate, List<long> raiinNos, int SyokeiGaku_I, int SyokeiGaku_RO, int Syokei)>
            GetKaikeiDayInfs(long ptId, int sinYm, ReceInfModel receInf)
        {
            List<KaikeiDetailModel> kaikeiDtls = _kaikeiFinder.FindKaikeiDetail(ptId, sinYm, receInf.HokenId);

            List<(int sinDate, List<long> raiinNos, int SyokeiGaku_I, int SyokeiGaku_RO, int Syokei)> kaikeiDayInfs =
                new List<(int sinDate, List<long> raiinNos, int TotalI, int TotalRo, int Syokei)>();

            int tmpSinDate = 0;
            List<long> tmpRaiinNos = new List<long>();
            int tmpSyokeiGaku_I = 0;
            int tmpSyokeiGaku_RO = 0;
            int tmpSyokei = 0;

            foreach (KaikeiDetailModel kaikeiDtl in kaikeiDtls)
            {
                if (tmpSinDate != kaikeiDtl.SinDate)
                {
                    if (tmpSinDate > 0)
                    {
                        kaikeiDayInfs.Add((tmpSinDate, new List<long>(tmpRaiinNos), tmpSyokeiGaku_I, tmpSyokeiGaku_RO, tmpSyokei));
                    }

                    tmpSinDate = kaikeiDtl.SinDate;
                    tmpRaiinNos.Clear();
                    tmpRaiinNos.Add(kaikeiDtl.RaiinNo);
                    tmpSyokeiGaku_I = kaikeiDtl.RousaiIFutan;
                    tmpSyokeiGaku_RO = kaikeiDtl.RousaiRoFutan;
                    tmpSyokei = kaikeiDtl.RousaiIFutan / receInf.HokenMst.EnTen;
                }
                else
                {
                    tmpRaiinNos.Add(kaikeiDtl.RaiinNo);
                    tmpSyokeiGaku_I += kaikeiDtl.RousaiIFutan;
                    tmpSyokeiGaku_RO += kaikeiDtl.RousaiRoFutan;
                    tmpSyokei += kaikeiDtl.RousaiIFutan / receInf.HokenMst.EnTen;
                }
            }

            if (tmpSinDate > 0)
            {
                kaikeiDayInfs.Add((tmpSinDate, new List<long>(tmpRaiinNos), tmpSyokeiGaku_I, tmpSyokeiGaku_RO, tmpSyokei));
            }

            return kaikeiDayInfs;
        }

        public void Dispose()
        {
            _tenantDataContext.Dispose();
        }
    }

}
