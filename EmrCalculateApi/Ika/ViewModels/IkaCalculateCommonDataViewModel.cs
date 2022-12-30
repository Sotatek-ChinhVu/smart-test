using Entity.Tenant;
using EmrCalculateApi.Ika.DB.Finder;
using EmrCalculateApi.Ika.DB.CommandHandler;
using EmrCalculateApi.Ika.Models;
using EmrCalculateApi.Ika.Constants;
using Helper.Constants;
using EmrCalculateApi.Utils;
using EmrCalculateApi.Interface;
using Domain.Constant;
using Helper.Common;
using EmrCalculateApi.Constants;
using Infrastructure.CommonDB;
using PostgreDataContext;
using Infrastructure.Interfaces;

namespace EmrCalculateApi.Ika.ViewModels
{
    // 共通データクラスを生成するための引数クラス
    public class IkaCalculateArgumentViewModel
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int hpId;
        /// <summary>
        /// 患者ID
        /// </summary>
        public long ptId;
        /// <summary>
        /// 診療日
        /// </summary>
        public int sinDate;
        /// <summary>
        /// 請求情報更新
        /// </summary>
        public int seikyuUp;
        /// <summary>
        /// 保険区分
        /// </summary>
        public int hokenKbn;
        /// <summary>
        /// 初再診
        /// </summary>
        public double syosai;
        /// <summary>
        /// 時間枠
        /// </summary>
        public double jikan;
        /// <summary>
        /// 計算モード
        /// 0:通常計算　1:連続計算　2:試算
        /// </summary>
        public int calcMode;
        /// <summary>
        /// レセプト状態情報のSTATUS_KBN=8を0に戻すかどうか
        /// </summary>
        public int clearReceChk;
        /// <summary>
        /// 点数マスタのキャッシュ
        /// </summary>
        public List<TenMstModel> cacheTenMst;
        /// <summary>
        /// 電子算定回数マスタのキャッシュ
        /// </summary>
        public List<DensiSanteiKaisuModel> cacheDensiSanteiKaisu;
        /// <summary>
        /// 項目グループマスタのキャッシュ
        /// </summary>
        public List<ItemGrpMstModel> cacheItemGrpMst;
        /// <summary>
        /// 医科計算ファインダー
        /// </summary>
        public IkaCalculateFinder ikaCalculateFinder;
        /// <summary>
        /// マスタファインダー
        /// </summary>
        public MasterFinder masterFinder;
        /// <summary>
        /// CommonBaseのマスタファインダー
        /// </summary>
        //public Emr.CommonBase.CommonMasters.DbAccess.MasterFinder commonMstFinder;
        /// <summary>
        /// 算定情報ファインダー
        /// </summary>
        public SanteiFinder santeiFinder;
        /// <summary>
        /// オーダーファインダー
        /// </summary>
        public OdrInfFinder odrInfFinder;
        /// <summary>
        /// DB保存処理ハンドラー
        /// </summary>
        public SaveIkaCalculateCommandHandler saveHandler;

        /// <summary>
        /// 来院情報
        /// </summary>
        public RaiinInfModel raiinInf;
        public List<RaiinInfModel> raiinInfs;

        /// <summary>
        /// 算定情報
        /// </summary>
        public List<SanteiInfDetailModel> santeiInfDtls;

        /// <summary>
        /// マスタ情報管理クラス
        /// </summary>
        public IkaCalculateCommonMasterViewModel mstCommon;

        /// <summary>
        /// CALC_STATUS.CREATE_MACHINEのプレフィックス
        /// </summary>
        public string preFix { get; set; } = "";
    }

    /// <summary>
    /// 計算に必要な共通データおよび共通メソッドクラス
    /// </summary>
    public class IkaCalculateCommonDataViewModel
    {
        private const string ModuleName = ModuleNameConst.EmrCalculateIka;

        /// <summary>
        /// 引数情報
        /// </summary>
        private IkaCalculateArgumentViewModel _arg;

        /// <summary>
        /// 算定ログ情報
        /// </summary>
        private List<CalcLogModel> _calcLogModels;

        #region Patient Data
        /// <summary>
        /// 患者情報
        /// </summary>
        private EmrCalculateApi.Ika.Models.PtInfModel _ptInf;
        /// <summary>
        /// 患者出産予定情報
        /// </summary>
        private List<PtPregnancyModel> _ptPregnancies;
        /// <summary>
        /// 算定情報詳細
        /// </summary>
        //private List<SanteiInfDetailModel> _santeiInfDtls;
        private List<PtHokenPatternModel> _ptHokenPatternModels;
        #endregion

        /// <summary>
        /// マスタ情報管理クラス
        /// </summary>
        private IkaCalculateCommonMasterViewModel _mstCommon;
        /// <summary>
        /// オーダー情報管理クラス
        /// </summary>
        private IkaCalculateCommonOdrDataViewModel _odrCommon;
        /// <summary>
        /// ワーク診療情報管理クラス
        /// </summary>
        private IkaCalculateCommonWrkDataViewModel _wrkCommon;
        /// <summary>
        /// 診療情報管理クラス
        /// </summary>
        private IkaCalculateCommonSinDataViewModel _sinCommon;
        /// <summary>
        /// 算定ログの連番
        /// </summary>
        private int _calcLogSeqNo;

        /// <summary>
        /// 初再診の保険組み合わせID
        /// </summary>
        private int _syosaiPid;
        /// <summary>
        /// 初再診の算定区分
        /// </summary>
        private int _syosaiSanteiKbn;

        /// <summary>
        /// 診療日が属する月の初日
        /// </summary>
        private int _sinFirstDateOfMonth;
        /// <summary>
        /// 診療日が属する月の最終日
        /// </summary>
        private int _sinLastDateOfMonth;
        /// <summary>
        /// 診療日が属する週の日曜日
        /// </summary>
        private int _sinFirstDateOfWeek;
        /// <summary>
        /// 診療日が属する週の土曜日
        /// </summary>
        private int _sinLastDateOfWeek;

        /// <summary>
        /// 当来院で使用されている保険組み合わせIDのリスト
        /// </summary>
        private List<int> _hokenPids;

        private Dictionary<(long, int), double> _syosaiList;
        private Dictionary<(long, int), int> _syosaiPidList;
        private Dictionary<(long, int), int> _syosaiHokenKbnList;
        private Dictionary<(long, int), int> _syosaiHokenIdList;
        private Dictionary<(long, int), int> _syosaiSanteiKbnList;
        private Dictionary<(long, int), double> _jikanList;

        private List<int> checkHokenKbn;
        private List<int> checkSanteiKbn;

        /// <summary>
        /// 初診項目のリスト
        /// </summary>
        private List<string> _syosinls =
            new List<string>
            {
                ItemCdConst.Syosin,
                ItemCdConst.SyosinCorona,
                ItemCdConst.SyosinJouhou,
                ItemCdConst.IgakuSyouniGairaiSyosinKofuAri,
                ItemCdConst.IgakuSyouniGairaiSyosinKofuNasi,
                ItemCdConst.IgakuSyouniKakaritukeSyosinKofuAri,
                ItemCdConst.IgakuSyouniKakaritukeSyosinKofuNasi,
                ItemCdConst.IgakuSyouniKakarituke1SyosinKofuAri,
                ItemCdConst.IgakuSyouniKakarituke1SyosinKofuNasi,
                ItemCdConst.IgakuSyouniKakarituke2SyosinKofuAri,
                ItemCdConst.IgakuSyouniKakarituke2SyosinKofuNasi
            };

        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="arg">IkaCalculateArgumentViewModel</param>
        public IkaCalculateCommonDataViewModel(IkaCalculateArgumentViewModel arg, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            const string conFncName = nameof(IkaCalculateCommonDataViewModel);

            _emrLogger = emrLogger;
            _systemConfigProvider = systemConfigProvider;
                        
            _emrLogger.WriteLogStart(this, conFncName, "");

            _arg = arg;

            // 初期化
            _calcLogSeqNo = 0;
            _syosaiPid = 0;

            //診療日が属する月の初日はここで求めておく
            _sinFirstDateOfMonth = arg.sinDate / 100 * 100 + 1;

            //診療日が属する月の最終日、診療日が属する週の日曜日と土曜日は必要時に取得
            _sinLastDateOfMonth = 0;
            _sinFirstDateOfWeek = 0;
            _sinLastDateOfWeek = 0;

            //算定ログ
            _calcLogModels = new List<CalcLogModel>();

            // マスタ管理クラス
            _mstCommon = new IkaCalculateCommonMasterViewModel(_arg.masterFinder, _arg.hpId, _arg.sinDate, _arg.cacheTenMst, _arg.cacheDensiSanteiKaisu, _arg.cacheItemGrpMst, _systemConfigProvider, _emrLogger);
            
            _hokenPids = new List<int>();

            _syosaiList = new Dictionary<(long, int), double>();
            _syosaiPidList = new Dictionary<(long, int), int>();
            _syosaiHokenKbnList = new Dictionary<(long, int), int>();
            _syosaiHokenIdList = new Dictionary<(long, int), int>();
            _syosaiSanteiKbnList = new Dictionary<(long, int), int>();
            _jikanList = new Dictionary<(long, int), double>();

            // 患者情報取得
            _ptInf = _arg.ikaCalculateFinder.FindPtInf(_arg.hpId, _arg.ptId, _arg.sinDate);
            // 患者保険組み合わせ情報取得            
            _ptHokenPatternModels = _arg.ikaCalculateFinder.FindPtHokenPattern(hpId, ptId, sinDate);

            //オーダー情報管理クラス
            if (_arg.calcMode != CalcModeConst.Trial)
            {
                _odrCommon = new IkaCalculateCommonOdrDataViewModel(_arg.odrInfFinder, _arg.masterFinder, _mstCommon, _arg.hpId, _arg.ptId, _arg.sinDate, _systemConfigProvider, _emrLogger);
                // 検査重複オーダー削除
                //DelKensa();

                //SortOdrDtl();
            }

            //ワーク情報管理クラス
            _wrkCommon = new IkaCalculateCommonWrkDataViewModel(_arg.santeiFinder, _mstCommon, _ptInf, _arg.hpId, _arg.ptId, _arg.sinDate, _systemConfigProvider);

            //算定情報管理クラス
            _sinCommon = new IkaCalculateCommonSinDataViewModel(_arg.santeiFinder, _mstCommon, _arg.hpId, _arg.ptId, _arg.sinDate, raiinNo, _arg.hokenKbn, _arg.calcMode, _systemConfigProvider, _emrLogger);

            checkHokenKbn = CalcUtils.GetCheckHokenKbns(_arg.hokenKbn, _systemConfigProvider.GetHokensyuHandling());
            checkSanteiKbn = CalcUtils.GetCheckSanteiKbns(_arg.hokenKbn, _systemConfigProvider.GetHokensyuHandling());                        
        }

        /// <summary>
        /// 点数マスタのキャッシュ
        /// </summary>
        public List<TenMstModel> CacheTenMst
        {
            get
            {
                return Mst?.CacheTenMst ?? null;
            }

        }

        /// <summary>
        /// 試算専用
        /// オーダーを計算用にコンバートする
        /// </summary>
        /// <param name="todayOdrInfModels"></param>
        //public void CreatOdrCommon(List<TodayOdrInfModel> todayOdrInfModels)
        //{
        //    _odrCommon = new IkaCalculateCommonOdrDataViewModel(
        //        todayOdrInfModels, _arg.raiinInf, _ptHokenPatternModels,
        //        _arg.odrInfFinder, _arg.masterFinder, _arg.ikaCalculateFinder,
        //        _mstCommon,
        //        _arg.hpId, _arg.ptId, _arg.sinDate);

        //    // 検査重複オーダー削除
        //    DelKensa();

        //    // 並び順を調整
        //    SortOdrDtl();
        //}

        /// <summary>
        /// 検査重複オーダー削除
        /// ※ログ出力したいため、あえて、IkaCalculateCommonOdrDataViewModelのコンストラクターでやらずに
        /// 　こちらで別に行うようにしている
        /// </summary>
        public void DelKensa()
        {
            HashSet<(int, long, string, string)> delItems = _odrCommon.DelSameKensa();

            if (_systemConfigProvider.GetCalcCheckKensaDuplicateLog() == 0)
            {
                foreach ((int sinDate, long raiinNo, string itemName, string baseName) delItem in delItems)
                {
                    if (string.IsNullOrEmpty(delItem.baseName))
                    {
                        AppendCalcLog(2, string.Format(FormatConst.DelKensa, delItem.itemName), delItem.sinDate, delItem.raiinNo);
                    }
                    else
                    {
                        AppendCalcLog(2, string.Format(FormatConst.DelKensaDiffBase, delItem.itemName, delItem.baseName), delItem.sinDate, delItem.raiinNo);
                    }
                }
            }
        }

        /// <summary>
        /// _odrCommon.odrDtllsの並び順を調整。項目に付随する選択式コメントが存在する場合はその項目の下に移動する
        /// </summary>
        public void SortOdrDtl()
        {
            DoSortOdrDtl(
                _odrCommon.odrDtlls.FindAll(p =>
                    //(p.OdrKouiKbn >= OdrKouiKbnConst.SyotiMin && p.OdrKouiKbn <= OdrKouiKbnConst.SyotiMax) ||
                    (p.OdrKouiKbn >= EmrCalculateApi.Constants.OdrKouiKbnConst.KensaMin && p.OdrKouiKbn <= OdrKouiKbnConst.KensaMax) //||
                                                                                                           //(p.OdrKouiKbn >= OdrKouiKbnConst.SyujyutuMin && p.OdrKouiKbn <= OdrKouiKbnConst.SyujyutuMax) ||
                                                                                                           //(p.OdrKouiKbn >= OdrKouiKbnConst.SonotaMin && p.OdrKouiKbn <= OdrKouiKbnConst.SonotaMax)
                    ));
        }
        /// <summary>
        /// _odrCommon.odrDtllsの並び順を調整。項目に付随する選択式コメントが存在する場合はその項目の下に移動する
        /// </summary>
        public void SortOdrDtlRaiin()
        {
            DoSortOdrDtl(
                _odrCommon.odrDtlls.FindAll(p =>
                    p.RaiinNo == raiinNo &&
                    (
                        //(p.OdrKouiKbn >= OdrKouiKbnConst.SyotiMin && p.OdrKouiKbn <= OdrKouiKbnConst.SyotiMax) ||
                        (p.OdrKouiKbn >= OdrKouiKbnConst.KensaMin && p.OdrKouiKbn <= OdrKouiKbnConst.KensaMax)  //||
                                                                                                                //(p.OdrKouiKbn >= OdrKouiKbnConst.SyujyutuMin && p.OdrKouiKbn <= OdrKouiKbnConst.SyujyutuMax) ||
                                                                                                                //(p.OdrKouiKbn >= OdrKouiKbnConst.SonotaMin && p.OdrKouiKbn <= OdrKouiKbnConst.SonotaMax)
                    )
                    ));
        }

        private void DoSortOdrDtl(List<OdrDtlTenModel> targetOdrDtls)
        {
            foreach (OdrDtlTenModel odrDtl in targetOdrDtls)
            {
                // 項目に付随する選択式コメントのコメントコードを取得する
                List<string> cmtCds =
                    _mstCommon.FindRecedenCmtSelectsByItemCd(odrDtl.ItemCd)?.Select(q => q.CommentCd).ToList() ?? null;
                if (cmtCds != null && cmtCds.Any() &&
                    _odrCommon.odrDtlls.Any(p =>
                        p.RaiinNo == odrDtl.RaiinNo && p.RpNo == odrDtl.RpNo && p.RpEdaNo == odrDtl.RpEdaNo && cmtCds.Contains(p.ItemCd)))
                {
                    // 取得した選択式コメント項目が当来院のオーダーに存在する場合

                    foreach (OdrDtlTenModel cmtOdrDtl in
                        _odrCommon.odrDtlls.FindAll(p =>
                            p.RaiinNo == odrDtl.RaiinNo && p.RpNo == odrDtl.RpNo && p.RpEdaNo == odrDtl.RpEdaNo && cmtCds.Contains(p.ItemCd)))
                    {
                        // 選択式コメント項目のRpNo等調整し、項目に付随するようにする
                        cmtOdrDtl.RowNo = odrDtl.RowNo;
                        cmtOdrDtl.RowSubNo =
                            (_odrCommon.odrDtlls.FindAll(p =>
                                p.RaiinNo == odrDtl.RaiinNo &&
                                p.RpNo == odrDtl.RpNo &&
                                p.RpEdaNo == odrDtl.RpEdaNo &&
                                p.RowNo == odrDtl.RowNo)?
                               .Max(p => p.RowSubNo) ?? 0) + 1;
                    }
                }
            }

            _odrCommon.odrDtlls =
                _odrCommon.odrDtlls
                .OrderBy(p => p.RaiinNo)
                .ThenBy(p => p.OdrKouiKbn)
                .ThenBy(p => p.SortNo)
                .ThenBy(p => p.RpNo)
                .ThenBy(p => p.RpEdaNo)
                .ThenBy(p => p.RowNo)
                .ThenBy(p => p.RowSubNo)
                .ToList();
        }
        /// <summary>
        /// 出産予定
        /// </summary>
        private List<PtPregnancyModel> PtPregnancies
        {
            get
            {
                if (_ptPregnancies == null)
                {
                    _ptPregnancies = _arg.ikaCalculateFinder.FindPtPregnancy(_arg.hpId, _arg.ptId, _arg.sinDate);
                }
                return _ptPregnancies;
            }
        }

        /// <summary>
        /// オーダー情報管理クラス
        /// </summary>
        public IkaCalculateCommonOdrDataViewModel Odr
        {
            get { return _odrCommon; }
        }

        /// <summary>
        /// ワーク診療情報管理クラス
        /// </summary>
        public IkaCalculateCommonWrkDataViewModel Wrk
        {
            get { return _wrkCommon; }
        }

        /// <summary>
        /// 診療情報管理クラス
        /// </summary>
        public IkaCalculateCommonSinDataViewModel Sin
        {
            get { return _sinCommon; }
        }

        /// <summary>
        /// マスタ情報管理クラス
        /// </summary>
        public IkaCalculateCommonMasterViewModel Mst
        {
            get { return _mstCommon; }
        }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int hpId
        {
            get { return _arg.hpId; }
        }

        /// <summary>
        /// 患者ID
        /// </summary>
        public long ptId
        {
            get { return _arg.ptId; }
        }

        /// <summary>
        /// 診療日
        /// </summary>
        public int sinDate
        {
            get { return _arg.sinDate; }
        }

        /// <summary>
        /// 来院番号
        /// </summary>
        public long raiinNo
        {
            get { return _arg.raiinInf != null ? _arg.raiinInf.RaiinNo : 0; }
        }

        /// <summary>
        /// 親来院番号
        /// </summary>
        public long oyaRaiinNo
        {
            get { return _arg.raiinInf != null ? _arg.raiinInf.OyaRaiinNo : 0; }
        }
        /// <summary>
        /// 診察開始時間
        /// </summary>
        public string sinStartTime
        {
            get
            {
                string ret = "";
                if (_arg.raiinInf != null)
                {
                    ret = _arg.raiinInf.SinStartTime;

                    if (ret.Length <= 4)
                    {
                        ret += "00";
                    }

                    if (ret.Length < 6)
                    {
                        ret.PadLeft(6, '0');
                    }
                }

                return ret;
            }
        }

        /// <summary>
        /// 保険区分
        /// 0:健保 1:労災 2:アフターケア 3:自賠 4:自費
        /// </summary>
        public int hokenKbn
        {
            get { return _arg.hokenKbn; }
            set
            {
                //if (_arg.hokenKbn == value) return;
                _arg.hokenKbn = value;

                checkHokenKbn.Clear();

                if (_systemConfigProvider.GetHokensyuHandling() == 0)
                {
                    // 同一に考える
                    if (_arg.hokenKbn <= 3)
                    {
                        checkHokenKbn.AddRange(new List<int> { 0, 1, 2, 3 });
                    }
                    else
                    {
                        checkHokenKbn.Add(_arg.hokenKbn);
                    }
                }
                else if (_systemConfigProvider.GetHokensyuHandling() == 1)
                {
                    // すべて同一に考える
                    checkHokenKbn.AddRange(new List<int> { 0, 1, 2, 3, 4 });
                }
                else
                {
                    // 別に考える
                    checkHokenKbn.Add(_arg.hokenKbn);
                }

                if (_arg.hokenKbn == 4)
                {
                    checkHokenKbn.Add(0);
                }

                checkSanteiKbn = CalcUtils.GetCheckSanteiKbns(_arg.hokenKbn, _systemConfigProvider.GetHokensyuHandling());

                Odr.HokenKbn = value;
                Wrk.HokenKbn = value;
                Sin.HokenKbn = value;
            }
        }

        /// <summary>
        /// 診療科名称
        /// </summary>
        public string kaName
        {
            get { return _arg.raiinInf.KaName; }
        }

        /// <summary>
        /// 初再診の種類
        /// </summary>
        public double syosai
        {
            get
            {
                double ret = 0;
                if (_syosaiList.ContainsKey((raiinNo, hokenKbn)))
                {
                    ret = _syosaiList[(raiinNo, hokenKbn)];
                }

                return ret;
            }
            set
            {
                if (_syosaiList.ContainsKey((raiinNo, hokenKbn)))
                {
                    // 更新
                    _syosaiList[(raiinNo, hokenKbn)] = value;
                }
                else
                {
                    _syosaiList.Add((raiinNo, hokenKbn), value);
                }
            }
        }

        /// <summary>
        /// 初再診項目のhokenPId
        /// </summary>
        public int syosaiPid
        {
            get
            {
                int ret = 0;
                if (_syosaiPidList.ContainsKey((raiinNo, hokenKbn)))
                {
                    ret = _syosaiPidList[(raiinNo, hokenKbn)];
                }

                return ret;
            }
            set
            {
                if (_syosaiPidList.ContainsKey((raiinNo, hokenKbn)))
                {
                    // 更新
                    _syosaiPidList[(raiinNo, hokenKbn)] = value;
                }
                else
                {
                    _syosaiPidList.Add((raiinNo, hokenKbn), value);
                }
            }
        }
        /// <summary>
        /// 初再診項目のhokenKbn
        /// </summary>
        public int syosaiHokenKbn
        {
            get
            {
                int ret = 0;
                if (_syosaiHokenKbnList.ContainsKey((raiinNo, hokenKbn)))
                {
                    ret = _syosaiHokenKbnList[(raiinNo, hokenKbn)];
                }

                return ret;
            }
            set
            {
                if (_syosaiHokenKbnList.ContainsKey((raiinNo, hokenKbn)))
                {
                    // 更新
                    _syosaiHokenKbnList[(raiinNo, hokenKbn)] = value;
                }
                else
                {
                    _syosaiHokenKbnList.Add((raiinNo, hokenKbn), value);
                }
            }
        }
        public int syosaiHokenId
        {
            get
            {
                int ret = 0;
                if (_syosaiHokenIdList.ContainsKey((raiinNo, hokenKbn)))
                {
                    ret = _syosaiHokenIdList[(raiinNo, hokenKbn)];
                }

                return ret;
            }
            set
            {
                if (_syosaiHokenIdList.ContainsKey((raiinNo, hokenKbn)))
                {
                    // 更新
                    _syosaiHokenIdList[(raiinNo, hokenKbn)] = value;
                }
                else
                {
                    _syosaiHokenIdList.Add((raiinNo, hokenKbn), value);
                }
            }
        }

        /// <summary>
        /// 初再診項目の算定区分
        /// </summary>
        public int syosaiSanteiKbn
        {
            get
            {
                int ret = 0;
                if (_syosaiSanteiKbnList.ContainsKey((raiinNo, hokenKbn)))
                {
                    ret = _syosaiSanteiKbnList[(raiinNo, hokenKbn)];
                }

                return ret;
            }
            set
            {
                if (_syosaiSanteiKbnList.ContainsKey((raiinNo, hokenKbn)))
                {
                    // 更新
                    _syosaiSanteiKbnList[(raiinNo, hokenKbn)] = value;
                }
                else
                {
                    _syosaiSanteiKbnList.Add((raiinNo, hokenKbn), value);
                }
            }
        }

        /// <summary>
        /// 時間枠
        /// </summary>
        public double jikan
        {
            get
            {
                double ret = 0;
                if (_jikanList.ContainsKey((raiinNo, hokenKbn)))
                {
                    ret = _jikanList[(raiinNo, hokenKbn)];
                }

                return ret;
            }
            set
            {
                if (_jikanList.ContainsKey((raiinNo, hokenKbn)))
                {
                    // 更新
                    _jikanList[(raiinNo, hokenKbn)] = value;
                }
                else
                {
                    _jikanList.Add((raiinNo, hokenKbn), value);
                }
            }
        }

        /// <summary>
        /// 計算モード
        /// </summary>
        public int calcMode
        {
            get { return _arg.calcMode; }
        }

        /// <summary>
        /// 出産予定情報
        /// </summary>
        public List<PtPregnancyModel> ptPregnancies
        {
            get { return PtPregnancies; }
        }

        /// <summary>
        /// 患者情報
        /// </summary>
        public EmrCalculateApi.Ika.Models.PtInfModel ptInf
        {
            get { return _ptInf; }
        }

        public List<PtHokenPatternModel> ptHokenPattern
        {
            get { return _ptHokenPatternModels; }
        }

        /// <summary>
        /// 算定情報詳細
        ///// </summary>
        //public List<SanteiInfDetailModel> santeiInfDtls
        //{
        //    get { return _santeiInfDtls; }
        //}

        /// <summary>
        /// 来院情報
        /// </summary>
        public RaiinInfModel raiinInf
        {
            get { return _arg.raiinInf; }
            set
            {
                _hokenPids = null;

                //if (_arg.raiinInf == value) return;
                _arg.raiinInf = value;

                if (Odr != null)
                {
                    Odr.HokenKbn = hokenKbn;
                    Odr.RaiinNo = raiinNo;
                }

                Wrk.HokenKbn = hokenKbn;
                Wrk.RaiinNo = raiinNo;
                Wrk.OyaRaiinNo = oyaRaiinNo;
                Wrk.SinStartTime = sinStartTime;

                Sin.RaiinNo = raiinNo;
            }
        }

        public List<RaiinInfModel> raiinInfs
        {
            get { return _arg.raiinInfs; }
        }

        /// <summary>
        /// マスタ検索
        /// </summary>
        public MasterFinder masterFinder
        {
            get { return _arg.masterFinder; }
        }

        /// <summary>
        /// 算定情報検索
        /// </summary>
        public SanteiFinder santeiFinder
        {
            get { return _arg.santeiFinder; }
        }

        /// <summary>
        /// 算定ログ
        /// </summary>
        public List<CalcLogModel> calcLogs
        {
            get { return _calcLogModels; }
        }

        /// <summary>
        /// 死亡日
        /// </summary>
        public int DeathDate
        {
            get { return _ptInf.DeathDate; }
        }

        #region データ操作

        #region データ操作 - クリア処理
        /// <summary>
        /// 作業データをクリアする
        /// </summary>
        public void ClearKeisanData()
        {
            Wrk.Clear();
            Sin.ClearKeisanData();

            ClearCalcLog();
        }

        /// <summary>
        /// 算定ログのみクリアする
        /// </summary>
        public void ClearCalcLog()
        {
            _calcLogSeqNo = 0;
            _calcLogModels.Clear();
        }

        #endregion

        /// <summary>
        /// 算定ログデータを追加する
        /// </summary>
        /// <param name="logSbt">
        /// ログ種別
        ///     1: 注意
        ///     2: 警告
        /// </param>
        /// <param name="logText">ログ</param>
        public void AppendCalcLog(int logSbt, string logText)
        {
            if (!_calcLogModels.Exists(c =>
                    c.HpId == _arg.hpId &&
                    c.PtId == _arg.ptId &&
                    c.SinDate == _arg.sinDate &&
                    c.RaiinNo == _arg.raiinInf.RaiinNo &&
                    c.LogSbt == logSbt &&
                    c.Text == logText)
                )
            {
                _calcLogSeqNo++;

                CalcLogModel calcLogModel = new CalcLogModel(new CalcLog());
                calcLogModel.HpId = _arg.hpId;
                calcLogModel.PtId = _arg.ptId;
                calcLogModel.SinDate = _arg.sinDate;
                calcLogModel.RaiinNo = _arg.raiinInf.RaiinNo;
                calcLogModel.SeqNo = _calcLogSeqNo;
                calcLogModel.LogSbt = logSbt;
                calcLogModel.Text = logText;
                _calcLogModels.Add(calcLogModel);
            }
        }
        public void AppendCalcLog(int logSbt, string logText, int sinDate, long raiinNo)
        {
            if (!_calcLogModels.Exists(c =>
                    c.HpId == _arg.hpId &&
                    c.PtId == _arg.ptId &&
                    c.SinDate == _arg.sinDate &&
                    c.RaiinNo == _arg.raiinInf.RaiinNo &&
                    c.LogSbt == logSbt &&
                    c.Text == logText)
                )
            {
                _calcLogSeqNo++;

                CalcLogModel calcLogModel = new CalcLogModel(new CalcLog());
                calcLogModel.HpId = _arg.hpId;
                calcLogModel.PtId = _arg.ptId;
                calcLogModel.SinDate = sinDate;
                calcLogModel.RaiinNo = raiinNo;
                calcLogModel.SeqNo = _calcLogSeqNo;
                calcLogModel.LogSbt = logSbt;
                calcLogModel.Text = logText;
                _calcLogModels.Add(calcLogModel);
            }
        }
        public void AppendCalcLog(int logSbt, string logText, string itemCd, string delItemCd, int delSbt, int isWarning, int termCnt, int termSbt, int hokenId)
        {
            //logSbt, logText, unqLogData.itemCd, unqLogData.delItemCd, unqLogData.delSbt, unqLogData.isWarning, unqLogData.termCnt, unqLogData.termSbt, unqLogData.hokenId
            if (!_calcLogModels.Exists(c =>
                    c.HpId == _arg.hpId &&
                    c.PtId == _arg.ptId &&
                    c.SinDate == _arg.sinDate &&
                    c.RaiinNo == _arg.raiinInf.RaiinNo &&
                    c.LogSbt == logSbt &&
                    c.Text == logText)
                )
            {
                _calcLogSeqNo++;

                CalcLogModel calcLogModel = new CalcLogModel(new CalcLog());
                calcLogModel.HpId = _arg.hpId;
                calcLogModel.PtId = _arg.ptId;
                calcLogModel.SinDate = sinDate;
                calcLogModel.RaiinNo = raiinNo;
                calcLogModel.SeqNo = _calcLogSeqNo;
                calcLogModel.LogSbt = logSbt;
                calcLogModel.Text = logText;
                calcLogModel.ItemCd = itemCd;
                calcLogModel.DelItemCd = delItemCd;
                calcLogModel.DelSbt = delSbt;
                calcLogModel.IsWarning = isWarning;
                calcLogModel.TermCnt = termCnt;
                calcLogModel.TermSbt = termSbt;
                calcLogModel.HokenId = hokenId;
                _calcLogModels.Add(calcLogModel);
            }
        }
        #endregion

        /// <summary>
        /// 点数マスタの内容を元に表示用コメントを取得する
        /// </summary>
        /// <param name="itemCd">コメントのITEM_CD</param>
        /// <param name="cmtOpt">コメント文</param>
        /// <param name="maskEdit">true: 不足桁をマスク文字で埋める</param>
        /// <returns></returns>
        public string GetCommentStr(string itemCd, ref string cmtOpt, bool maskEdit = false)
        {
            return _mstCommon.GetCommentStr(itemCd, ref cmtOpt, maskEdit = false);
        }

        #region 判定処理
        /// <summary>
        /// 健保の計算ロジックを通すか労災の計算ロジックを通すか判断する
        /// </summary>
        /// <returns>true: 労災 false: 労災以外</returns>
        public bool IsRosai
        {
            get
            {
                return (_arg.hokenKbn == HokenSyu.Rosai || _arg.hokenKbn == HokenSyu.After ||
                    (_arg.hokenKbn == HokenSyu.Jibai && _systemConfigProvider.GetJibaiJunkyo() == 1));
            }
        }
        /// <summary>
        /// 自賠労災準拠かどうか
        /// </summary>
        public bool IsJibaiRosai
        {
            get
            {
                return (_arg.hokenKbn == HokenSyu.Jibai && _systemConfigProvider.GetJibaiJunkyo() == 1);
            }
        }

        ///// <summary>
        ///// 診療行為(種別=S or R)またはコメントの場合trueを返す
        ///// </summary>
        ///// <param name="masterSbt">マスタ種別</param>
        ///// <param name="commentSkip">true: コメントの場合はスキップ(falseを返す)</param>
        ///// <returns>true: 診療行為(種別=S or R)またはコメント</returns>
        //public bool IsSorCommentItem(string masterSbt, bool commentSkip)
        //{
        //    return (IsSItem(masterSbt) ||
        //            (commentSkip == false && !IsYItem(masterSbt) && !IsTItem(masterSbt)));
        //}

        ///// <summary>
        ///// 薬剤(種別=Y)またはコメントの場合trueを返す
        ///// </summary>
        ///// <param name="masterSbt">マスタ種別</param>
        ///// <param name="commentSkip">true: コメントの場合はスキップ(falseを返す)</param>
        ///// <returns>true: 薬剤(種別=Y)またはコメント</returns>
        //public bool IsYorCommentItem(string masterSbt, bool commentSkip)
        //{
        //    return (IsYItem(masterSbt) ||
        //            (commentSkip == false && IsCItem(masterSbt)));

        //}

        ///// <summary>
        ///// 特材(種別=T or U)またはコメントの場合trueを返す
        ///// </summary>
        ///// <param name="masterSbt">マスタ種別</param>
        ///// <param name="commentSkip">true: コメントの場合はスキップ(falseを返す)</param>
        ///// <returns>true: 特材(種別=T or U)またはコメント</returns>
        //public bool IsTorCommentItem(string masterSbt, bool commentSkip)
        //{
        //    return (IsTItem(masterSbt) ||
        //            (commentSkip == false && IsCItem(masterSbt)));
        //}

        /// <summary>
        /// 診療行為(種別=S or R)の場合trueを返す
        /// </summary>
        public bool IsSItem(string masterSbt)
        {
            return (masterSbt == "S" || masterSbt == "R");
        }

        /// <summary>
        /// 薬剤(種別=Y)の場合trueを返す
        /// </summary>
        public bool IsYItem(string masterSbt)
        {
            return (masterSbt == "Y");
        }

        /// <summary>
        /// 特材(種別=T or U)の場合trueを返す
        /// </summary>
        public bool IsTItem(string masterSbt)
        {
            return (masterSbt == "T" || masterSbt == "U");
        }

        /// <summary>
        /// コメント（種別=C or D)の場合trueを返す
        /// </summary>
        /// <param name="masterSbt"></param>
        /// <returns></returns>
        public bool IsCItem(string masterSbt)
        {
            return (masterSbt == "C" || masterSbt == "D");
        }

        #region 年齢チェック

        /// <summary>
        /// 年齢（年）
        /// </summary>
        public int Age
        {
            get { return ptInf.Age; }
        }

        /// <summary>
        /// 年齢（月）
        /// </summary>
        public int AgeMonth
        {
            get { return ptInf.AgeMonth; }
        }

        /// <summary>
        /// 年齢（日）
        /// </summary>
        public int AgeDay
        {
            get { return ptInf.AgeDay; }
        }
        /// <summary>
        /// 生年月日
        /// </summary>
        public int BirthDay
        {
            get { return ptInf.Birthday; }
        }

        /// <summary>
        /// 幼児（6歳未満）かどうか判定
        /// </summary>
        /// <returns>true: 幼児（6歳未満）</returns>
        public bool IsYoJi
        {
            get { return ptInf.IsYoJi; }
        }

        /// <summary>
        /// 乳幼児（3歳未満）かどうか判定
        /// </summary>
        /// <returns>true: 乳幼児（3歳未満）</returns>
        public bool IsNyuyoJi
        {
            get { return ptInf.IsNyuyoJi; }
        }

        /// <summary>
        /// 乳児（1歳未満）かどうか判定
        /// </summary>
        /// <returns>true: 乳児（1歳未満）</returns>
        public bool IsNyuJi
        {
            get { return ptInf.IsNyuJi; }
        }

        /// <summary>
        /// 新生児（28日未満）かどうか判定
        /// </summary>
        /// <returns>true: 新生児（28日未満）</returns>
        public bool IsSinseiJi
        {
            get { return ptInf.IsSinseiJi; }
        }

        /// <summary>
        /// 就学しているかどうか
        /// false: 未就学
        /// </summary>
        public bool IsStudent
        {
            get { return ptInf.IsStudent; }
        }
        #endregion

        /// <summary>
        /// 妊婦かどうか判定
        /// 2018/04/01～2018/12/31
        /// </summary>
        public bool IsNinpu
        {
            get
            {
                if (_arg.sinDate >= 20180401 && _arg.sinDate <= 20181231)
                {
                    return PtPregnancies.Any();
                }
                return false;
            }
        }

        /// <summary>
        /// 診療日が休日（休日加算算定可）かどうか判定
        /// </summary>
        /// <returns>true: 休日（休日加算算定可）</returns>
        public bool IsHolidaySinDate
        {
            get
            {
                return IsHoliday(_arg.sinDate);
            }
        }

        /// <summary>
        /// 指定の日が休日(休日加算が算定可能)かどうか判定
        /// </summary>
        /// <param name="sinDate">診療日</param>
        /// <returns>true: 休日（休日加算算定可）</returns>
        public bool IsHoliday(int baseDate)
        {
            bool ret = false;

            string date = CIUtil.SDateToShowSDate(baseDate);
            DateTime dt = DateTime.Parse(date);
            //if (dt.DayOfWeek == DayOfWeek.Sunday)
            //{
            //    //日曜日
            //    ret = true;
            //}

            //if (!ret)
            //{
            //    int mmdd = baseDate % 10000;

            //    //年末年始（12/29～1/3）
            //    if (new List<int> {1229, 1230, 1231, 101, 102, 103 }.Contains(mmdd))
            //    {
            //        ret = true;
            //    }
            //}

            if (!ret)
            {
                if (_arg.masterFinder.IsKyujitu(_arg.hpId, baseDate))
                {
                    //休日
                    ret = true;
                }
            }

            return ret;
        }

        /// <summary>
        /// 受付時間が深夜の時間帯であるかどうかチェック
        /// 22:00～6:00
        /// </summary>
        /// <returns>true: 深夜時間帯である</returns>
        public bool IsSinyaTime
        {
            get { return (CIUtil.StrToIntDef(_arg.raiinInf.UketukeTime, 0) < 60000 || CIUtil.StrToIntDef(_arg.raiinInf.UketukeTime, 0) >= 220000); }
        }

        /// <summary>
        /// 指定のITEM_CDがコメント用のITEM_CDかどうかチェック
        /// (空文字　または　"8"で始まる9桁の値の場合、true
        /// </summary>
        /// <param name="itemCd"></param>
        /// <returns>true: コメント用項目コード</returns>
        public bool IsCommentItemCd(string itemCd)
        {
            return itemCd == "" ||
                        (itemCd.StartsWith("8") && itemCd.Length == 9);
        }

        #endregion

        #region 算定チェック

        /// <summary>
        /// 指定の項目が診療日に算定されているかチェック
        /// </summary>
        /// <param name="itemCd">チェックする項目</param>
        /// <returns>true: 算定されている</returns>
        public bool CheckSanteiDay(string itemCd)
        {
            return Sin.CheckSanteiSinday(itemCd);
        }

        /// <summary>
        /// 指定の項目が診療日に算定されているかチェック
        /// ※複数項目用
        /// </summary>
        /// <param name="itemCds">チェックする項目のリスト</param>
        /// <returns>true: 算定されている</returns>
        public bool CheckSanteiDay(List<string> itemCds)
        {
            return Sin.CheckSanteiSinday(itemCds);
        }

        /// <summary>
        /// 指定の項目が指定の期間に算定されているかチェック
        /// </summary>
        /// <param name="itemCd">チェックする項目</param>
        /// <param name="startDate">チェック開始日</param>
        /// <param name="endDate">チェック終了日</param>
        /// <returns>ture: 算定されている</returns>
        public bool CheckSanteiTerm(string itemCd, int startDate, int endDate)
        {
            if (startDate <= sinDate && sinDate <= endDate)
            {
                // 診療日を含む期間の場合、診療日分はリストから取得する
                if (calcMode != CalcModeConst.Trial)
                {
                    if (Wrk.ExistWrkSinKouiDetailByItemCd(itemCd, false))
                    {
                        return true;
                    }
                }
                else
                {
                    if (Sin.CheckSanteiSinday(itemCd))
                    {
                        return true;
                    }
                }
            }
            return _arg.santeiFinder.CheckSanteiTerm(_arg.hpId, _arg.ptId, startDate, endDate, sinDate, raiinNo, itemCd, hokenKbn);
        }

        /// <summary>
        /// 指定の項目が指定の期間に算定されているかチェック
        /// ※複数項目用
        /// </summary>
        /// <param name="itemCds">チェックする項目のリスト</param>
        /// <param name="startDate">チェック開始日</param>
        /// <param name="endDate">チェック終了日</param>
        /// <returns>ture: 算定されている</returns>
        public bool CheckSanteiTerm(List<string> itemCds, int startDate, int endDate)
        {
            if (startDate <= sinDate && sinDate <= endDate)
            {
                // 診療日を含む期間の場合、診療日分はリストから取得する
                if (calcMode != CalcModeConst.Trial)
                {
                    if (Wrk.ExistWrkSinKouiDetailByItemCd(itemCds, false))
                    {
                        return true;
                    }
                }
                else
                {
                    if (Sin.CheckSanteiSinday(itemCds))
                    {
                        return true;
                    }
                }
                //// 診療日を含む期間の場合、診療日分はリストから取得する
                //if (Sin.CheckSanteiSinday(itemCds))
                //{
                //    return true;
                //}
            }

            if (startDate / 100 == sinDate / 100 && endDate / 100 == sinDate / 100)
            {
                // 指定期間が当月内の場合は、取得済みの診療データからチェック
                return Sin.CheckSanteiTerm(itemCds, startDate, endDate);
            }
            else
            {
                // 指定期間が当月外の場合は、
                return _arg.santeiFinder.CheckSanteiTerm(_arg.hpId, _arg.ptId, startDate, endDate, sinDate, raiinNo, itemCds, hokenKbn);
            }
        }

        /// <summary>
        /// 指定の項目が指定期間に何回算定されているかを取得する
        /// </summary>
        /// <param name="startDate">カウント開始日</param>
        /// <param name="endDate">カウント終了日</param>
        /// <param name="itemCds">カウントする項目リスト</param>
        /// <returns>算定回数（数量×回数）</returns>
        public double SanteiCount(int startDate, int endDate, string itemCd)
        {
            double ret = 0;

            if (startDate <= sinDate && sinDate <= endDate)
            {
                ret += Sin.SanteiCountSinday(itemCd);
            }

            if (startDate / 100 == sinDate / 100 && endDate / 100 == sinDate / 100)
            {
                // 指定期間が当月内の場合は、取得済みの診療データからチェック
                return Sin.SanteiCountTerm(itemCd, startDate, endDate);
            }
            else
            {
                ret += _arg.santeiFinder.SanteiCount(_arg.hpId, _arg.ptId, startDate, endDate, sinDate, _arg.raiinInf.RaiinNo, itemCd, hokenKbn);
            }

            return ret;
        }

        /// <summary>
        /// 指定の項目が指定期間に何回算定されているかを取得する（複数指定）
        /// </summary>
        /// <param name="startDate">カウント開始日</param>
        /// <param name="endDate">カウント終了日</param>
        /// <param name="itemCds">カウントする項目リスト</param>
        /// <returns>算定回数（数量×回数）</returns>
        public double SanteiCount(int startDate, int endDate, List<string> itemCds)
        {
            double ret = 0;

            if (startDate <= sinDate && sinDate <= endDate)
            {
                ret += Sin.SanteiCountSinday(itemCds);
            }

            if (startDate / 100 == sinDate / 100 && endDate / 100 == sinDate / 100)
            {
                // 指定期間が当月内の場合は、取得済みの診療データからチェック
                return Sin.SanteiCountTerm(itemCds, startDate, endDate);
            }
            else
            {
                ret = _arg.santeiFinder.SanteiCount(_arg.hpId, _arg.ptId, startDate, endDate, sinDate, _arg.raiinInf.RaiinNo, itemCds, hokenKbn);
            }

            return ret;
        }

        /// <summary>
        /// 指定の包括区分の項目が指定期間に何回算定されているかを取得する
        /// </summary>
        /// <param name="startDate">カウント開始日</param>
        /// <param name="endDate">カウント終了日</param>
        /// <param name="hokatuKbn">カウントする包括区分</param>
        /// <returns>算定回数（数量×回数）</returns>
        public double SanteiCountByHokatuKbn(int startDate, int endDate, int hokatuKbn)
        {
            double ret = 0;

            if (startDate <= sinDate && sinDate <= endDate)
            {
                // 当日分（試算の場合は、当来院分）は、Wrk（計算中データ）から取得
                //ret += Sin.SanteiCountByHokatuKbnSinDay(hokatuKbn, hokenKbn);
                ret += Wrk.WrkCountByHokatuKbn(hokatuKbn);
            }

            int argSinDate = sinDate;
            if (calcMode == CalcModeConst.Trial)
            {
                // 試算の場合、同日来院分も取得する
                argSinDate = 0;
            }

            if (startDate / 100 == sinDate / 100 && endDate / 100 == sinDate / 100)
            {
                // 当月内の場合、ローカルキャッシュから
                ret += Sin.SanteiCountByHokatuKbnSinDay(startDate, endDate, argSinDate, raiinNo, hokatuKbn);
            }
            else
            {
                // 当月外の場合、DBから
                ret += _arg.santeiFinder.SanteiCountByHokatuKbn(hpId, ptId, startDate, endDate, argSinDate, raiinNo, hokatuKbn, hokenKbn);
            }

            return ret;
        }
        public double SanteiCountByHokatuKbnToday(int hokatuKbn)
        {
            double ret = 0;

            // 当日分（試算の場合は、当来院分）は、Wrk（計算中データ）から取得
            ret += Wrk.WrkCountByHokatuKbn(hokatuKbn);
            return ret;
        }
        public double SanteiCountByHokatuKbnTerm(int startDate, int endDate, int hokatuKbn)
        {
            double ret = 0;

            int argSinDate = sinDate;
            if (calcMode == CalcModeConst.Trial)
            {
                // 試算の場合、同日来院分も取得する
                argSinDate = 0;
            }

            if (startDate / 100 == sinDate / 100 && endDate / 100 == sinDate / 100)
            {
                // 当月内の場合、ローカルキャッシュから
                ret += Sin.SanteiCountByHokatuKbnSinDay(startDate, endDate, argSinDate, raiinNo, hokatuKbn);
            }
            else
            {
                // 当月外の場合、DBから
                ret += _arg.santeiFinder.SanteiCountByHokatuKbn(hpId, ptId, startDate, endDate, argSinDate, raiinNo, hokatuKbn, hokenKbn);
            }

            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="hokatuKbn"></param>
        /// <param name="cdKbn"></param>
        /// <param name="cdKbnno"></param>
        /// <param name="cdEdano"></param>
        /// <param name="cdKouno"></param>
        /// <returns></returns>
        public double SanteiCountByHokatuKbn(int startDate, int endDate, int hokatuKbn, string cdKbn, int cdKbnno, int cdEdano, int cdKouno)
        {
            double ret = 0;

            if (startDate <= sinDate && sinDate <= endDate)
            {
                // 当日分（試算の場合は、当来院分）は、Wrk（計算中データ）から取得
                //ret += Sin.SanteiCountByHokatuKbnSinDay(hokatuKbn, cdKbn, cdKbnno, cdEdano, cdKouno, hokenKbn);
                ret += Wrk.WrkCountByHokatuKbn(hokatuKbn, cdKbn, cdKbnno, cdEdano, cdKouno);
            }

            int argSinDate = sinDate;
            if (calcMode == CalcModeConst.Trial)
            {
                // 試算の場合、同日来院分も取得する
                argSinDate = 0;
            }


            if (startDate / 100 == sinDate / 100 && endDate / 100 == sinDate / 100)
            {
                // 当月内の場合、ローカルキャッシュから
                ret += Sin.SanteiCountByHokatuKbnSinDay(startDate, endDate, argSinDate, raiinNo, hokatuKbn, cdKbn, cdKbnno, cdEdano, cdKouno);
            }
            else
            {
                // 当月外の場合、DBから
                ret += _arg.santeiFinder.SanteiCountByHokatuKbn(hpId, ptId, startDate, endDate, sinDate, raiinNo, hokatuKbn, cdKbn, cdKbnno, cdEdano, cdKouno, hokenKbn);
            }

            return ret;
        }
        /// <summary>
        /// 当日分の算定回数
        /// </summary>
        /// <param name="sinDate"></param>
        /// <param name="hokatuKbn"></param>
        /// <param name="cdKbn"></param>
        /// <param name="cdKbnno"></param>
        /// <param name="cdEdano"></param>
        /// <param name="cdKouno"></param>
        /// <returns></returns>
        public double SanteiCountByHokatuKbnToday(int sinDate, int hokatuKbn, string cdKbn, int cdKbnno, int cdEdano, int cdKouno)
        {
            double ret = 0;

            // 当日分（試算の場合は、当来院分）は、Wrk（計算中データ）から取得
            ret += Wrk.WrkCountByHokatuKbn(hokatuKbn, cdKbn, cdKbnno, cdEdano, cdKouno);
            return ret;
        }
        /// <summary>
        /// 指定期間の算定回数、当日分除く
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="hokatuKbn"></param>
        /// <param name="cdKbn"></param>
        /// <param name="cdKbnno"></param>
        /// <param name="cdEdano"></param>
        /// <param name="cdKouno"></param>
        /// <returns></returns>
        public double SanteiCountByHokatuKbnTerm(int startDate, int endDate, int hokatuKbn, string cdKbn, int cdKbnno, int cdEdano, int cdKouno)
        {
            double ret = 0;

            int argSinDate = sinDate;
            if (calcMode == CalcModeConst.Trial)
            {
                // 試算の場合、同日来院分も取得する
                argSinDate = 0;
            }

            if (startDate / 100 == sinDate / 100 && endDate / 100 == sinDate / 100)
            {
                // 当月内の場合、ローカルキャッシュから
                ret += Sin.SanteiCountByHokatuKbnSinDay(startDate, endDate, argSinDate, raiinNo, hokatuKbn, cdKbn, cdKbnno, cdEdano, cdKouno);
            }
            else
            {
                // 当月外の場合、DBから
                ret += _arg.santeiFinder.SanteiCountByHokatuKbn(hpId, ptId, startDate, endDate, sinDate, raiinNo, hokatuKbn, cdKbn, cdKbnno, cdEdano, cdKouno, hokenKbn);
            }

            return ret;
        }
        // MAX_COUNT>1の場合は注意扱いする単位のコード
        // 142:2週, 143:2月, 144:3月, 145:4月, 146:6月, 147:12月, 148:5年
        List<int> CyuiUnitCd =
            new List<int>
            {
                    142, 143, 144, 145, 146, 147, 148
            };

        // ログメッセージのフォーマット
        List<string> calcLogFormat =
            new List<string>
            {
                    "",
                    FormatConst.MaybeNotSantei,
                    FormatConst.NotSantei
            };
        List<string> calcLogFormatAuto =
            new List<string>
            {
                    "",
                    FormatConst.MaybeNotSantei,
                    FormatConst.DontSantei
            };
        /// <summary>
        /// 指定の項目について算定回数マスタの設定に従い、
        /// 算定回数をチェックする
        /// </summary>
        /// <param name="itemCd">診療行為コード</param>
        /// <returns>
        ///     0:上限を超えない
        ///     1:上限を超える（注意）
        ///     2:上限を超える（警告）
        /// </returns>
        public int CheckSanteiKaisu(string itemCd, int santeiKbn, int isAuto, double konkaiSuryo = 0, bool nolog = false)
        {
            const string conFncName = nameof(CheckSanteiKaisu);

            List<DensiSanteiKaisuModel> densiSanteiKaisuModels =
                _mstCommon.FindDensiSanteiKaisu(_arg.sinDate, IsRosai, itemCd);

            int ret = 0;
            // ログ種別
            int logSbt = 0;
            // 期間
            string sTerm = "";
            // ログメッセージ
            string logText = "";
            // 項目名
            string itemName = "";

            // チェック期間と表記を取得する
            foreach (DensiSanteiKaisuModel densiSanteiKaisu in densiSanteiKaisuModels)
            {
                // チェック開始日
                int startDate = 0;
                // チェック終了日
                int endDate = sinDate;

                List<int> checkHokenKbnTmp = new List<int>();
                checkHokenKbnTmp.AddRange(checkHokenKbn);

                if (densiSanteiKaisu.TargetKbn == 1)
                {
                    // 健保のみ対象の場合はすべて対象
                }
                else if (densiSanteiKaisu.TargetKbn == 2)
                {
                    // 労災のみ対象の場合、健保は抜く
                    checkHokenKbnTmp.RemoveAll(p => new int[] { 0 }.Contains(p));
                }

                List<int> checkSanteiKbnTmp = new List<int>();
                checkSanteiKbnTmp.AddRange(checkSanteiKbn);

                if (santeiKbn == 2 && isAuto == 1)
                {
                    // 自費算定、自動算定の場合、自費も含める
                    checkSanteiKbnTmp.Add(2);
                }

                switch (densiSanteiKaisu.UnitCd)
                {
                    case 53:    //患者あたり
                        sTerm = "患者あたり";
                        break;
                    case 121:   //1日
                        startDate = sinDate;
                        sTerm = "1日";
                        break;
                    case 131:   //1月
                        startDate = sinDate / 100 * 100 + 1;
                        sTerm = "1月";
                        break;
                    case 138:   //1週
                        startDate = WeeksBefore(sinDate, 1);
                        sTerm = "1週";
                        break;
                    case 141:   //一連
                        sTerm = "一連";
                        break;
                    case 142:   //2週
                        startDate = WeeksBefore(sinDate, 2);
                        sTerm = "2週";
                        break;
                    case 143:   //2月
                        startDate = MonthsBefore(sinDate, 1);
                        sTerm = "2月";
                        break;
                    case 144:   //3月
                        startDate = MonthsBefore(sinDate, 2);
                        sTerm = "3月";
                        break;
                    case 145:   //4月
                        startDate = MonthsBefore(sinDate, 3);
                        sTerm = "4月";
                        break;
                    case 146:   //6月
                        startDate = MonthsBefore(sinDate, 5);
                        sTerm = "6月";
                        break;
                    case 147:   //12月
                        startDate = MonthsBefore(sinDate, 11);
                        sTerm = "12月";
                        break;
                    case 148:   //5年
                        startDate = YearsBefore(sinDate, 5);
                        sTerm = "5年";
                        break;
                    case 159:   //初診時
                        sTerm = "初診時";
                        break;
                    case 997:   //初診から1カ月（休日除く）
                        //if (Wrk.wrkSinKouiDetails.Any(p =>
                        //    _syosinls.Contains(p.ItemCd) && checkHokenKbnTmp.Contains(p.HokenKbn) && checkSanteiKbnTmp.Contains(Wrk.GetSanteiKbn(p.RaiinNo, p.RpNo))))
                        //{
                        //    // 初診関連項目を算定している場合、算定不可
                        //    endDate = 99999999;
                        //}
                        //else
                        //{
                            // 直近の初診日から１か月後を取得する（休日除く）
                            endDate = GetSyosinDate(sinDate, densiSanteiKaisu.TargetKbn, checkHokenKbnTmp, checkSanteiKbnTmp);
                            endDate = MonthsAfterExcludeHoliday(endDate, 1);
                        //}
                        break;
                    case 998:   //初診から1カ月
                        //if (calcMode != CalcModeConst.Trial && Wrk.wrkSinKouiDetails.Any(p =>
                        //    _syosinls.Contains(p.ItemCd) && checkHokenKbnTmp.Contains(p.HokenKbn) && checkSanteiKbnTmp.Contains(Wrk.GetSanteiKbn(p.RaiinNo, p.RpNo))))
                        //{
                        //    // 初診関連項目を算定している場合、算定不可
                        //    endDate = 99999999;
                        //}
                        //else
                        //{
                            // 直近の初診日から１か月後を取得する
                            endDate = GetSyosinDate(sinDate, densiSanteiKaisu.TargetKbn, checkHokenKbnTmp, checkSanteiKbnTmp);
                            endDate = MonthsAfter(endDate, 1);
                        //}
                        break;
                    case 999:   //カスタム
                        if (densiSanteiKaisu.TermSbt == 2)
                        {
                            //日
                            startDate = DaysBefore(sinDate, densiSanteiKaisu.TermCount);
                            sTerm = densiSanteiKaisu.TermCount.ToString() + "日";
                        }
                        else if (densiSanteiKaisu.TermSbt == 3)
                        {
                            //週
                            startDate = WeeksBefore(sinDate, densiSanteiKaisu.TermCount);
                            sTerm = densiSanteiKaisu.TermCount.ToString() + "週";
                        }
                        else if (densiSanteiKaisu.TermSbt == 4)
                        {
                            //月
                            startDate = MonthsBefore(sinDate, densiSanteiKaisu.TermCount);
                            sTerm = densiSanteiKaisu.TermCount.ToString() + "月";
                        }
                        else if (densiSanteiKaisu.TermSbt == 5)
                        {
                            //年
                            startDate = (sinDate / 10000 - (densiSanteiKaisu.TermCount - 1)) * 10000 + 101;
                            sTerm = densiSanteiKaisu.TermCount.ToString() + "年間";
                        }
                        break;
                    default:
                        startDate = -1;
                        break;
                }

                if (startDate >= 0)
                {
                    double count = 0;

                    if (new int[] { 997, 998 }.Contains(densiSanteiKaisu.UnitCd))
                    {
                        //初診から1カ月
                        if (endDate > sinDate)
                        {
                            //算定不可
                            if (densiSanteiKaisu.SpJyoken == 1)
                            {
                                // 注意
                                if (ret <= 1)
                                {
                                    ret = 1;
                                }
                                logSbt = 1;
                            }
                            else
                            {
                                ret = 2;
                                logSbt = 2;
                            }

                            if (nolog == false)
                            {
                                // 点数マスタから項目の名称取得
                                itemName = GetItemName(itemCd);

                                if (isAuto == 0)
                                {
                                    logText = string.Format(FormatConst.SanteiSyosin + calcLogFormat[logSbt], itemName);
                                }
                                else
                                {
                                    logText = string.Format(FormatConst.SanteiSyosin + calcLogFormatAuto[logSbt], itemName);
                                }

                                AppendCalcLog(logSbt, logText);
                            }
                        }
                    }
                    else if (densiSanteiKaisu.UnitCd == 159 &&
                        (new int[] { SyosaiConst.Syosin, SyosaiConst.Syosin2, SyosaiConst.SyosinCorona, SyosaiConst.SyosinJouhou, SyosaiConst.Syosin2Jouhou }.Contains((int)syosai) == false))
                    {
                        // 初診時なのに初診じゃない場合はエラー
                        //算定不可
                        if (densiSanteiKaisu.SpJyoken == 1)
                        {
                            // 注意
                            if (ret <= 1)
                            {
                                ret = 1;
                            }
                            logSbt = 1;
                        }
                        else
                        {
                            ret = 2;
                            logSbt = 2;
                        }

                        if (nolog == false)
                        {
                            // 点数マスタから項目の名称取得
                            itemName = GetItemName(itemCd);

                            if (isAuto == 0)
                            {
                                logText = string.Format(FormatConst.SanteiNotSyosin + calcLogFormat[logSbt], itemName);
                            }
                            else
                            {
                                logText = string.Format(FormatConst.SanteiNotSyosin + calcLogFormatAuto[logSbt], itemName);
                            }

                            AppendCalcLog(logSbt, logText);
                        }
                    }
                    else
                    {

                        //if(densiSanteiKaisu.TargetKbn == 1)
                        //{
                        //    checkHokenKbnTmp.RemoveAll(p => new int[] { 1, 2, 3}.Contains(p));
                        //}
                        //else if(densiSanteiKaisu.TargetKbn == 2)
                        //{
                        //    checkHokenKbnTmp.RemoveAll(p => new int[] { 0 }.Contains(p));
                        //}

                        List<string> itemCds = new List<string>();

                        List<ItemGrpMstModel> itemGrpMsts = new List<ItemGrpMstModel>();

                        if (densiSanteiKaisu.ItemGrpCd > 0)
                        {
                            // 項目グループの設定がある場合
                            itemGrpMsts = _mstCommon.FindItemGrpMst(sinDate, 1, densiSanteiKaisu.ItemGrpCd);
                        }

                        if (itemGrpMsts != null && itemGrpMsts.Any())
                        {
                            // 項目グループの設定がある場合
                            itemCds.AddRange(itemGrpMsts.Select(x => x.ItemCd));
                        }
                        else
                        {
                            itemCds.Add(itemCd);
                        }

                        bool suryoCount = IsSuryoCount(itemCd);

                        if (new int[] { 141, 159 }.Contains(densiSanteiKaisu.UnitCd) == false)
                        {
                            if (startDate / 100 == endDate / 100 && startDate / 100 == sinDate / 100)
                            {
                                // 診療月内の範囲の場合はリストから取得
                                count =
                                    _sinCommon.SanteiCountTerm(
                                        itemCds: itemCds,
                                        startDate: startDate,
                                        endDate: endDate,
                                        santeiKbns: checkSanteiKbnTmp,
                                        hokenKbns: checkHokenKbnTmp);
                            }
                            else
                            {
                                // 診療月外の範囲の場合はDBから取得
                                count =
                                    _arg.santeiFinder.SanteiCount(
                                        hpId: hpId,
                                        ptId: ptId,
                                        startDate: startDate,
                                        endDate: endDate,
                                        sinDate: sinDate,
                                        raiinNo: raiinNo,
                                        itemCds: itemCds,
                                        hokenKbn: hokenKbn,
                                        santeiKbns: checkSanteiKbnTmp,
                                        hokenKbns: checkHokenKbnTmp);
                            }

                            if (calcMode == CalcModeConst.Trial)
                            {
                                // 試算の場合、本日分は算定情報から取得
                                count += Sin.GetSanteiDaysSinDay(itemCds, checkSanteiKbnTmp, checkHokenKbnTmp).Any() ? 1 : 0;
                            }
                            // 今日の分を足す
                            //count += Sin.SanteiCountSinday(itemCd);
                            count += Wrk.WrkCountSinday(itemCds, checkSanteiKbnTmp, checkHokenKbnTmp, suryoCount);
                        }

                        // 今回の来院分をチェックする（今から算定する分、１回分があるので、それは省いておく）
                        count = count +
                                    ((from
                                            wrkDtl in Wrk.wrkSinKouiDetails
                                      where
                                          //wrkDtl.HokenKbn == hokenKbn &&
                                          checkHokenKbnTmp.Contains(wrkDtl.HokenKbn) &&
                                          checkSanteiKbnTmp.Contains(_wrkCommon.GetSanteiKbn(wrkDtl.RaiinNo, wrkDtl.RpNo)) &&
                                          wrkDtl.RaiinNo == raiinInf.RaiinNo &&
                                          itemCds.Contains(wrkDtl.ItemCd) &&
                                          wrkDtl.IsDeleted == DeleteStatus.None &&
                                          wrkDtl.FmtKbn != 10
                                      select
                                          (wrkDtl.Suryo <= 0 || suryoCount == false) ? 1 : wrkDtl.Suryo).Sum());
                        // 上限値を超えるかチェックする
                        if (densiSanteiKaisu.MaxCount <= count)
                        {
                            //if (densiSanteiKaisu.SpJyoken == 1 ||
                            //    (CyuiUnitCd.Contains(densiSanteiKaisu.UnitCd) && densiSanteiKaisu.MaxCount > 1) ||
                            //    (densiSanteiKaisu.UnitCd == 999 && (densiSanteiKaisu.TermCount > 1 && densiSanteiKaisu.MaxCount > 1))
                            //    )
                            if (densiSanteiKaisu.SpJyoken == 1)
                            {
                                // 注意
                                if (ret <= 1)
                                {
                                    ret = 1;
                                }
                                logSbt = 1;

                            }
                            else
                            {
                                ret = 2;
                                logSbt = 2;
                            }

                            if (nolog == false)
                            {
                                // 点数マスタから項目の名称取得
                                itemName = GetItemName(itemCd);

                                if (isAuto == 0)
                                {
                                    logText = string.Format(FormatConst.SanteiJyogen + calcLogFormat[logSbt], itemName, densiSanteiKaisu.MaxCount, sTerm);
                                }
                                else
                                {
                                    logText = string.Format(FormatConst.SanteiJyogen + calcLogFormatAuto[logSbt], itemName, densiSanteiKaisu.MaxCount, sTerm);
                                }

                                AppendCalcLog(logSbt, logText);
                            }

                        }
                        else if (densiSanteiKaisu.MaxCount < count + (konkaiSuryo > 0 && suryoCount == false ? 1 : konkaiSuryo))
                        {
                            // 今回分を足すと超えてしまう場合は注意（MaxCount = count + konkaiSuryoはセーフ）
                            ret = 1;
                            logSbt = 1;

                            if (nolog == false)
                            {
                                // 点数マスタから項目の名称取得
                                itemName = GetItemName(itemCd);

                                if (isAuto == 0)
                                {
                                    logText = string.Format(FormatConst.SanteiJyogen2 + calcLogFormat[logSbt], itemName, densiSanteiKaisu.MaxCount, sTerm);
                                }
                                else
                                {
                                    logText = string.Format(FormatConst.SanteiJyogen2 + calcLogFormatAuto[logSbt], itemName, densiSanteiKaisu.MaxCount, sTerm);
                                }

                                AppendCalcLog(logSbt, logText);
                            }
                        }
                    }
                }
            }

            return ret;

            #region Local Method
            string GetItemName(string AitemCd)
            {
                string retItemName = "";

                List<TenMstModel> tenMstModels = _mstCommon.GetTenMst(AitemCd);
                if (tenMstModels.Any())
                {
                    retItemName = tenMstModels[0].ReceName.Trim();
                }
                else
                {
                    // 点数マスタに登録がない場合は、診療行為コードを返す
                    retItemName = AitemCd;
                }

                return retItemName;
            }
            bool IsSuryoCount(string AitemCd)
            {
                bool isSuryoCount = true;

                List<TenMstModel> tenMstModels = _mstCommon.GetTenMst(AitemCd);
                if (tenMstModels.Any())
                {
                    isSuryoCount = !(new string[] { "ｍＬ", "ｃｍ２", "ｍ", "分" }.Contains(tenMstModels.First().ReceUnitName));
                }
                else
                {
                    // 点数マスタに登録がない場合は、診療行為コードを返す
                    isSuryoCount = false;
                }

                return isSuryoCount;
            }
            #endregion
        }

        /// <summary>
        /// 年齢チェック
        /// </summary>
        /// <param name="odrDtl"></param>
        /// <returns>
        ///     0:算定可能な年齢である
        ///     1:算定不可の年齢である（注意）
        ///     2:算定不可の年齢である（警告）
        /// </returns>
        public int CheckAge(OdrDtlTenModel odrDtl)
        {
            int result = 0;

            bool _checkValue(string val)
            {
                return (string.IsNullOrEmpty(val) == false &&
                   (new string[] { "AA", "B3", "BF", "BK", "B6", "MG" }.Contains(val) ||
                   CIUtil.StrToIntDef(val, 0) > 0));
            }

            if (odrDtl.TenMst != null && odrDtl.TenMst.AgeCheck != 2)
            {
                // 下限チェック
                if (_checkValue(odrDtl.TenMst.MinAge))
                {
                    if (CheckAgeMin(odrDtl.TenMst.MinAge) == false)
                    {
                        if (odrDtl.TenMst.AgeCheck == 1)
                        {
                            AppendCalcLog(2, string.Format(FormatConst.MaybeNotSanteiReason, odrDtl.ReceName, odrDtl.TenMst.MinAgeDsp + "未満の"));
                            result = 1;
                        }
                        else
                        {
                            AppendCalcLog(2, string.Format(FormatConst.NotSanteiReason, odrDtl.ReceName, odrDtl.TenMst.MinAgeDsp + "未満の"));
                            result = 2;
                        }
                    }
                }

                if (result == 0)
                {
                    // 上限チェック
                    if (_checkValue(odrDtl.TenMst.MaxAge))
                    {
                        if (CheckAgeMax(odrDtl.TenMst.MaxAge) == false)
                        {
                            if (odrDtl.TenMst.AgeCheck == 1)
                            {
                                AppendCalcLog(2, string.Format(FormatConst.MaybeNotSanteiReason, odrDtl.ReceName, odrDtl.TenMst.MaxAgeDsp + "以上の"));
                                result = 1;
                            }
                            else
                            {
                                AppendCalcLog(2, string.Format(FormatConst.NotSanteiReason, odrDtl.ReceName, odrDtl.TenMst.MaxAgeDsp + "以上の"));
                                result = 2;
                            }
                        }
                    }
                }
            }

            return result;
        }
        private bool CheckAgeMin(string MinAge)
        {
            bool ret = false;
            if (MinAge == "AA")
            {
                ret = (Age > 0 || AgeMonth > 0 || AgeDay >= 28);
            }
            else if (MinAge == "B3")
            {
                ret = ((Age >= 3 && AgeMonth > 0) || (Age == 3 && BirthDay / 100 < sinDate / 100));
            }
            else if (MinAge == "BF")
            {
                ret = ((Age >= 15 && AgeMonth > 0) || (Age == 15 && BirthDay / 100 < sinDate / 100));
            }
            else if (MinAge == "BK")
            {
                ret = ((Age >= 20 && AgeMonth > 0) || (Age == 20 && BirthDay / 100 < sinDate / 100));
            }
            else
            {
                ret = (CIUtil.StrToIntDef(MinAge, 999) <= Age);
            }

            return ret;
        }
        /// <summary>
        /// 年齢加算の上限チェック
        /// </summary>
        /// <param name="AgekasanMax">年齢加算上限</param>
        /// <returns>true: 年齢が、年齢加算の上限未満</returns>
        private bool CheckAgeMax(string MaxAge)
        {
            bool ret = false;
            if (MaxAge == "AA")
            {
                ret = (Age == 0 && AgeMonth == 0 && AgeDay < 28);
            }
            else if (MaxAge == "B3")
            {
                ret = (Age < 3 || (Age == 3 && BirthDay / 100 == sinDate / 100));
            }
            else if (MaxAge == "BF")
            {
                ret = (Age < 15 || (Age == 15 && BirthDay / 100 == sinDate / 100));
            }
            else if (MaxAge == "BK")
            {
                ret = (Age < 20 || (Age == 20 && BirthDay / 100 == sinDate / 100));
            }
            else if (MaxAge == "B6")
            {
                ret = (Age < 6 || (Age == 6 && BirthDay / 100 == sinDate / 100));
            }
            else if (MaxAge == "MG")
            {
                ret = CIUtil.IsStudent(BirthDay, sinDate) == false;
            }
            else
            {
                ret = (CIUtil.StrToIntDef(MaxAge, 999) > Age);
            }

            return ret;
        }
        /// <summary>
        /// 指定日以前の直近の初診日
        /// 初診の算定がない場合は、0を返す
        /// </summary>
        /// <param name="baseDate">基準日</param>
        /// <param name="targetKbn">0-健保・労災両方、1-健保のみ、2-労災のみ</param>
        /// <returns>直近の算定日(YYYYMMDD)</returns>
        public int GetSyosinDate(int baseDate, int targetKbn, List<int> checkHokenKbnTmp = null, List<int> checkSanteiKbnTmp = null)
        {
            int retDate = 0;
            int retSinDate = 0;

            List<int> checkHokenKbns = new List<int>();
            checkHokenKbns.AddRange(checkHokenKbn);

            if (checkHokenKbnTmp != null)
            {
                checkHokenKbns = checkHokenKbnTmp;
            }

            List<int> checkSanteiKbns = new List<int>();
            checkSanteiKbns.AddRange(checkSanteiKbn);

            if (checkSanteiKbnTmp != null)
            {
                checkSanteiKbns = checkSanteiKbnTmp;
            }

            //if(targetKbn == 1)
            //{
            //    checkHokenKbns.RemoveAll(p => new int[] { 1, 2, 3 }.Contains(p));
            //}
            //else if(targetKbn == 2)
            //{
            //    checkHokenKbns.RemoveAll(p => new int[] { 0 }.Contains(p));
            //}

            if (sinDate < baseDate)
            {
                //if (Sin.CheckSanteiSinday(_syosinls))
                //if (Wrk.ExistWrkSinKouiDetailByItemCd(_syosinls, false))
                if (Wrk.ExistWrkSinKouiDetailByItemCd(
                    itemCds: _syosinls,
                    onlyThisRaiin: false,
                    excludeSanteiGai: true,
                    sameHokenKbn: false,
                    hokenKbns: checkHokenKbns,
                    santeiKbns: checkSanteiKbns))
                {
                    retSinDate = sinDate;
                }
            }

            if (calcMode == CalcModeConst.Trial)
            {
                retDate =
                    _arg.santeiFinder.FindLastSanteiDate(
                        hpId: hpId,
                        ptId: ptId,
                        baseDate: baseDate,
                        sinDate: 0,
                        raiinNo: raiinNo,
                        itemCds: _syosinls,
                        hokenKbn: hokenKbn,
                        santeiKbn: 0,
                        hokenKbns: checkHokenKbns);
            }
            else
            {
                retDate =
                    _arg.santeiFinder.FindLastSanteiDate(
                        hpId: hpId,
                        ptId: ptId,
                        baseDate: baseDate,
                        sinDate: sinDate,
                        raiinNo: raiinNo,
                        itemCds: _syosinls,
                        hokenKbn: hokenKbn,
                        santeiKbn: 0,
                        hokenKbns: checkHokenKbns);
            }

            if (retDate < retSinDate)
            {
                retDate = retSinDate;
            }

            return retDate;
        }

        /// <summary>
        /// 指定日以前の直近の算定日
        /// 算定がない場合は、0を返す
        /// </summary>
        /// <param name="baseDate">基準日</param>
        /// <param name="itemCd">チェックしたい項目の診療行為コード</param>
        /// <returns>直近の算定日(YYYYMMDD)</returns>
        public int GetZenkaiDate(int baseDate, string itemCd)
        {
            int retDate = 0;
            int retSinDate = 0;

            if (sinDate <= baseDate)
            {
                //if (Sin.CheckSanteiSinday(itemCd))                
                if (Wrk.ExistWrkSinKouiDetailByItemCd(itemCd, false))
                {
                    retSinDate = sinDate;
                }
            }

            retDate = _arg.santeiFinder.FindLastSanteiDate(_arg.hpId, _arg.ptId, baseDate, sinDate, raiinNo, itemCd, hokenKbn);

            if (retDate < retSinDate)
            {
                retDate = retSinDate;
            }

            return retDate;
        }

        /// <summary>
        /// 指定日以前の直近の算定日（複数指定）
        /// 算定がない場合は、0を返す
        /// ※複数項目用
        /// </summary>
        /// <param name="baseDate">基準日</param>
        /// <param name="itemCds">チェックしたい項目の診療行為コードのリスト（複数指定）</param>
        /// <returns>直近の算定日(YYYYMMDD)</returns>
        public int GetZenkaiDate(int baseDate, List<string> itemCds)
        {
            int retDate = 0;
            int retSinDate = 0;

            if (sinDate <= baseDate)
            {
                //if (Sin.CheckSanteiSinday(itemCds))
                if (Wrk.ExistWrkSinKouiDetailByItemCd(itemCds, false))
                {
                    retSinDate = sinDate;
                }
            }

            retDate = _arg.santeiFinder.FindLastSanteiDate(_arg.hpId, _arg.ptId, baseDate, sinDate, raiinNo, itemCds, hokenKbn);

            if (retDate < retSinDate)
            {
                retDate = retSinDate;
            }

            return retDate;
        }

        /// <summary>
        /// 指定日以前の初回の算定日
        /// 算定がない場合は、0を返す
        /// </summary>
        /// <param name="baseDate">基準日</param>
        /// <param name="itemCd">初回算定日を取得したい項目の診療行為コード</param>
        /// <returns>初回の算定日</returns>
        public int GetSyokaiDate(int baseDate, string itemCd)
        {
            int retDate = _arg.santeiFinder.FindFirstSanteiDate(_arg.hpId, _arg.ptId, baseDate, raiinNo, itemCd, hokenKbn);

            if (retDate == 0 && sinDate <= baseDate)
            {
                if (Wrk.ExistWrkSinKouiDetailByItemCd(itemCd, false))
                {
                    retDate = sinDate;
                }
            }

            return retDate;
        }

        /// <summary>
        /// 指定日以前の初回の算定日（複数指定）
        /// 算定がない場合は、0を返す
        /// </summary>
        /// <param name="baseDate">基準日</param>
        /// <param name="itemCds">初回算定日を取得したい項目の診療行為コードのリスト（複数指定）</param>
        /// <returns>初回の算定日</returns>
        public int GetSyokaiDate(int baseDate, List<string> itemCds)
        {
            int retDate = _arg.santeiFinder.FindFirstSanteiDate(_arg.hpId, _arg.ptId, baseDate, raiinNo, itemCds, hokenKbn);

            if (retDate == 0 && sinDate <= baseDate)
            {
                if (Wrk.ExistWrkSinKouiDetailByItemCd(itemCds, false))
                {
                    retDate = sinDate;
                }
            }

            return retDate;
        }

        /// <summary>
        /// 指定項目の算定日リストを取得（同日来院分の情報は含まれないので注意する）
        /// </summary>
        /// <param name="startDate">チェック開始日</param>
        /// <param name="endDate">チェック終了日</param>
        /// <param name="itemCd">算定日を取得したい項目の診療行為コードのリスト（複数指定）</param>
        /// <returns>診療日+診療行為コードのリスト</returns>
        public List<SanteiDaysModel> GetSanteiDays(int startDate, int endDate, List<string> itemCds, int hokenKbn, bool excludeSinDate, bool excludeSanteiGai, List<int> santeiKbns = null)
        {
            List<SanteiDaysModel> ret = _arg.santeiFinder.GetSanteiDays(_arg.hpId, _arg.ptId, startDate, endDate, raiinNo, sinDate, itemCds, hokenKbn, excludeSanteiGai, santeiKbns);

            if (startDate <= sinDate && sinDate <= endDate)
            {
                //ret.AddRange(Sin.GetSanteiDaysSinDay(itemCds, hokenKbn));
                ret.AddRange(Sin.GetSanteiDaysSinDay(itemCds, santeiKbns: santeiKbns));
            }

            return ret;
        }
        /// <summary>
        /// 指定項目の算定日リストを取得（同日来院分の情報は含まれないので注意する）背反処理専用
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="itemCd"></param>
        /// <param name="hokenKbn"></param>
        /// <param name="excludeSinDate"></param>
        /// <param name="excludeSanteiGai"></param>
        /// <returns></returns>
        public List<SanteiDaysModel> GetSanteiDaysWithHaihan(int startDate, int endDate, string itemCd, int hokenKbn, bool excludeSinDate, bool excludeSanteiGai, List<int> santeiKbns = null)
        {
            return GetSanteiDaysWithHaihan(startDate, endDate, new List<string>() { itemCd }, hokenKbn, excludeSinDate, excludeSanteiGai, santeiKbns);
        }
        /// <summary>
        /// 指定項目の算定日リストを取得（同日来院分の情報は含まれないので注意する）背反処理専用
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="itemCds"></param>
        /// <param name="hokenKbn"></param>
        /// <param name="excludeSinDate"></param>
        /// <param name="excludeSanteiGai"></param>
        /// <returns></returns>
        public List<SanteiDaysModel> GetSanteiDaysWithHaihan(int startDate, int endDate, List<string> itemCds, int hokenKbn, bool excludeSinDate, bool excludeSanteiGai, List<int> santeiKbns = null)
        {
            List<SanteiDaysModel> ret = GetSanteiDays(startDate, endDate, itemCds, hokenKbn, excludeSinDate, excludeSanteiGai, santeiKbns);

            List<SanteiDaysModel> haihanSanteiDays = _arg.santeiFinder.GetSanteiDaysHaihan(_arg.hpId, _arg.ptId, startDate, endDate, raiinNo, sinDate, itemCds, hokenKbn, excludeSanteiGai);

            foreach (SanteiDaysModel santeiDay in haihanSanteiDays)
            {
                if (ret.Any(p => p.SinDate == santeiDay.SinDate && p.ItemCd == santeiDay.ItemCd) == false)
                {
                    ret.Add(santeiDay);
                }
            }

            if (startDate <= sinDate && sinDate <= endDate)
            {
                List<SanteiDaysModel> sinSanteiDays = Sin.GetSanteiDaysSinDayHaihan(itemCds);
                foreach (SanteiDaysModel santeiDay in sinSanteiDays)
                {
                    if (ret.Any(p => p.SinDate == santeiDay.SinDate && p.ItemCd == santeiDay.ItemCd && p.OdrItemCd == santeiDay.OdrItemCd) == false)
                    {
                        ret.Add(santeiDay);
                    }
                }
            }

            return ret;
        }
        public List<int> GetOdrDays(int startDate, int endDate, List<string> itemCds, int hokenKbn, bool excludeSanteiGai)
        {
            List<int> ret = _arg.odrInfFinder.GetOdrDays(_arg.hpId, _arg.ptId, startDate, endDate, itemCds, _arg.hokenKbn, excludeSanteiGai);
            return ret;
        }

        /// <summary>
        /// 指定項目の算定日リストを取得（同日来院分の情報は含まれないので注意する）
        /// </summary>
        /// <param name="startDate">チェック開始日</param>
        /// <param name="endDate">チェック終了日</param>
        /// <param name="itemCd">算定日を取得したい項目の診療行為コード</param>
        /// <returns>診療日+診療行為コードのリスト</returns>
        public List<SanteiDaysModel> GetSanteiDays(int startDate, int endDate, string itemCd, int hokenKbn, bool excludeSinDate, List<int> santeiKbns = null)
        {
            List<SanteiDaysModel> ret = _arg.santeiFinder.GetSanteiDays(_arg.hpId, _arg.ptId, startDate, endDate, raiinNo, sinDate, itemCd, hokenKbn, santeiKbns: santeiKbns);

            if (excludeSinDate == false && startDate <= sinDate && sinDate <= endDate)
            {
                ret.AddRange(Sin.GetSanteiDaysSinDay(itemCd, santeiKbns: santeiKbns));
            }

            return ret;
        }

        /// <summary>
        /// 診療日の指定項目の算定日リストを取得（複数指定）
        /// </summary>
        /// <param name="itemCds">算定日を取得したい項目の診療行為コードのリスト（複数指定）</param>
        /// <returns>診療日+診療行為コードのリスト</returns>
        public List<SanteiDaysModel> GetSanteiDaysSinDate(List<string> itemCds)
        {
            return Sin.GetSanteiDaysSinDay(itemCds);
        }

        /// <summary>
        /// 診療日の指定項目の算定日リストを取得
        /// </summary>
        /// <param name="itemCd">算定日を取得したい項目の診療行為コード</param>
        /// <returns>診療日+診療行為コードのリスト</returns>
        public List<SanteiDaysModel> GetSanteiDaysSinDate(string itemCd)
        {
            return Sin.GetSanteiDaysSinDay(itemCd);
        }

        /// <summary>
        /// 診療日の属する月の指定項目の算定日リストを取得（複数指定）
        /// </summary>
        /// <param name="itemCds">算定日を取得したい項目の診療行為コードのリスト（複数指定）</param>
        /// <returns>診療日+診療行為コードのリスト</returns>
        public List<SanteiDaysModel> GetSanteiDaysSinYm(List<string> itemCds)
        {
            return Sin.GetSanteiDaysSinYm(itemCds);
        }
        public double GetSanteiCountSinYm(List<string> itemCds, int endDate)
        {
            return Sin.GetSanteiCountSinYm(itemCds, endDate);
        }
        /// <summary>
        /// 診療日の属する月の指定項目の算定日リストを取得
        /// </summary>
        /// <param name="itemCd">算定日を取得したい項目の診療行為コード</param>
        /// <returns>診療日+診療行為コードのリスト</returns>
        public List<SanteiDaysModel> GetSanteiDaysSinYm(string itemCd)
        {
            return Sin.GetSanteiDaysSinYm(itemCd);
        }
        public List<SanteiDaysModel> GetSanteiDaysSinYmWithSanteiKbn(List<string> itemCds, bool addJihi = false)
        {
            List<SanteiDaysModel> results = new List<SanteiDaysModel>();

            results = Sin.GetSanteiDaysSinYmWithSanteiKbn(itemCds);

            if (addJihi)
            {
                // J自費、Z特材の算定日を取得
                List<SanteiDaysModel> jihiSanteidays = Sin.GetSanteiDaysInSinYMHaihan(SinFirstDateOfMonth, SinLastDateOfMonth, itemCds);

                foreach (SanteiDaysModel santeiDay in jihiSanteidays)
                {
                    if (results.Any(p => p.SinDate == santeiDay.SinDate && p.ItemCd == santeiDay.ItemCd) == false)
                    {
                        results.Add(santeiDay);
                    }
                }
            }

            return results;
        }
        public List<SanteiDaysModel> GetSanteiDaysSinYmWithHokenKbn(List<string> itemCds, bool addJihi = false)
        {
            List<SanteiDaysModel> results = new List<SanteiDaysModel>();

            results = Sin.GetSanteiDaysSinYmWithHokenKbn(itemCds);

            if (addJihi)
            {
                // J自費、Z特材の算定日を取得
                List<SanteiDaysModel> jihiSanteidays = Sin.GetSanteiDaysInSinYMHaihanWithHokenKbn(SinFirstDateOfMonth, SinLastDateOfMonth, itemCds);

                foreach (SanteiDaysModel santeiDay in jihiSanteidays)
                {
                    if (results.Any(p => p.SinDate == santeiDay.SinDate && p.ItemCd == santeiDay.ItemCd) == false)
                    {
                        results.Add(santeiDay);
                    }
                }
            }

            return results;
        }
        /// <summary>
        /// 診療日の属する月内の算定日リストを取得
        /// </summary>
        /// <param name="startDate">抽出開始日（診療月内を指定）</param>
        /// <param name="endDate">抽出終了日（診療月内を指定）</param>
        /// <param name="itemCds">算定日を取得したい項目の診療行為コード</param>
        /// <param name="santeiKbn">算定区分</param>
        /// <returns>診療日+診療行為コードのリスト</returns>
        public List<SanteiDaysModel> GetSanteiDaysInSinYM(int startDate, int endDate, List<string> itemCds, bool addJihi, int santeiKbn = 0)
        {
            // 通常の項目の算定日を取得
            List<SanteiDaysModel> results = Sin.GetSanteiDaysInSinYM(startDate, endDate, itemCds, santeiKbn);

            if (addJihi)
            {
                // J自費、Z特材の算定日を取得
                List<SanteiDaysModel> jihiSanteidays = Sin.GetSanteiDaysInSinYMHaihan(startDate, endDate, itemCds, santeiKbn);

                foreach (SanteiDaysModel santeiDay in jihiSanteidays)
                {
                    if (results.Any(p => p.SinDate == santeiDay.SinDate && p.ItemCd == santeiDay.ItemCd) == false)
                    {
                        results.Add(santeiDay);
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// 同月に1処方につき5種類を超える内服薬の投薬があるか
        /// </summary>
        /// <returns>true: ある</returns>
        public bool CheckNaifuku5Syu()
        {
            bool result = false;

            if (calcMode == CalcModeConst.Trial)
            {
                if (Odr.CheckNaifuku5Syu())
                {
                    // 当日オーダーで条件に当てはまる場合
                    result = true;
                }
                else
                {
                    // 当日分を除いてチェック
                    result = _arg.odrInfFinder.CheckNaifuku5Syu(_arg.hpId, _arg.ptId, _arg.sinDate, _arg.sinDate);
                }
            }
            else
            {
                if (_arg.odrInfFinder.CheckNaifuku5Syu(_arg.hpId, _arg.ptId, _arg.sinDate))
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// 同月に1処方につき抗うつ薬、抗精神病薬、抗不安薬又は睡眠薬を合わせて3種類を超える投薬があるか
        /// </summary>
        /// <returns>true: ある</returns>
        public bool CheckKouseisin()
        {
            bool result = false;

            if (calcMode == CalcModeConst.Trial)
            {
                if (Odr.CheckKouseisin())
                {
                    // 当日オーダーで条件に当てはまる場合
                    result = true;
                }
                else
                {
                    // 当日分を除いてチェック
                    return _arg.odrInfFinder.CheckKouseisin(_arg.hpId, _arg.ptId, _arg.sinDate, _arg.sinDate);
                }
            }
            else
            {
                return _arg.odrInfFinder.CheckKouseisin(_arg.hpId, _arg.ptId, _arg.sinDate);
            }

            return result;
        }

        /// <summary>
        /// 認知症地域包括診療料、および、認知症地域包括診療加算の算定要件チェック
        /// </summary>
        /// <param name="checkItemCd"></param>
        /// <param name="ItemName"></param>
        /// <returns></returns>
        public bool CheckSanteiNintiTiiki(string checkItemCd, string ItemName, int santeiKbn, int isAuto)
        {
            bool result = true;

            if (CheckSanteiKaisu(checkItemCd, santeiKbn, isAuto) == 2)
            {
                //算定上限を超える為、算定不可
                result = false;
            }
            else if (CheckNaifuku5Syu())
            {
                //同月に1処方につき5種類を超える内服薬の投薬がある場合は算定しない
                AppendCalcLog(2, string.Format("'{0}' は、同月に1処方につき5種類を超える内服薬の投薬があるため、算定できません。", ItemName));
                result = false;
            }
            else if (CheckKouseisin())
            {
                //同月に1処方につき抗うつ薬、抗精神病薬、抗不安薬又は睡眠薬を合わせて3種類を超えて投薬を行った場合は算定しない
                //ただし、抗うつ薬及び、抗精神病薬については、臨時に投薬した場合は種類数に含めない
                AppendCalcLog(2, string.Format("'{0}' は、同月に1処方につき抗うつ薬、抗精神病薬、抗不安薬又は睡眠薬を合わせて3種類を超えて投薬があるため、算定できません。", ItemName));
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 外来感染症対策向上加算等の加算対象項目チェック
        /// </summary>
        /// <param name="koui">1: 医学管理 2:在宅</param>
        /// <returns>true: 加算対象項目がある</returns>
        public bool ExistGairaiKansenTgt(int koui)
        {
            bool ret = false;

            //加算対象項目を取得
            List<string> lsTgt = Mst.GairaiKansenTgtList(koui);

            if (Wrk.ExistWrkSinKouiDetailByItemCd(lsTgt))
            {
                //算定されているか
                //小児科外来診療料等は自動算定のため算定項目もチェックする
                ret = true;
            }
            else if (Odr.ExistOdrDetailByItemCd(lsTgt))
            {
                //オーダーされているか？
                ret = true;
            }

            return ret;
        }

        #endregion

        #region 日数計算

        /// <summary>
        /// 診療日が属する月の初日
        /// </summary>
        public int SinFirstDateOfMonth
        {
            get { return _sinFirstDateOfMonth; }
        }

        /// <summary>
        /// 診療日が属する月の末日
        /// </summary>
        public int SinLastDateOfMonth
        {
            get
            {
                if (_sinLastDateOfMonth <= 0)
                {
                    _sinLastDateOfMonth = GetLastDateOfMonth(_arg.sinDate);
                }

                return _sinLastDateOfMonth;

            }
        }

        /// <summary>
        /// 診療日が属する週の日曜日
        /// </summary>
        public int SinFirstDateOfWeek
        {
            get
            {
                if (_sinFirstDateOfWeek == 0)
                {
                    _sinFirstDateOfWeek = GetFirstDateOfWeek(_arg.sinDate);
                }
                return _sinFirstDateOfWeek;
            }
        }

        /// <summary>
        /// 診療日が属する週の土曜日
        /// </summary>
        public int SinLastDateOfWeek
        {
            get
            {
                if (_sinLastDateOfWeek == 0)
                {
                    _sinLastDateOfWeek = GetLastDateOfWeek(_arg.sinDate);
                }
                return _sinLastDateOfWeek;
            }
        }

        ///<summary>
        ///指定の日数前の日付を取得する
        ///</summary>
        ///<param name="baseDate">基準日</param>
        ///<param name="term">日数</param>
        ///<returns>基準日の指定日数前の日付</returns>
        public int DaysBefore(int baseDate, int term)
        {
            return CIUtil.DaysBefore(baseDate, term);
        }

        ///<summary>
        ///指定の週数前の日曜日の日付を取得する
        ///</summary>
        ///<param name="baseDate">基準日</param>
        ///<param name="term">週数</param>
        ///<returns>基準日の指定週数前の週の日曜日の日付</returns>
        public int WeeksBefore(int baseDate, int term)
        {
            return CIUtil.WeeksBefore(baseDate, term);
        }

        /// <summary>
        /// 指定日が属する週の日曜日の日付を取得する
        /// </summary>
        /// <param name="baseDate">基準日</param>
        /// <returns>指定日が属する週の日曜日の日付</returns>
        public int GetFirstDateOfWeek(int baseDate)
        {
            return CIUtil.GetFirstDateOfWeek(baseDate);
        }

        /// <summary>
        /// 指定日が属する週の土曜日の日付を取得する
        /// </summary>
        /// <param name="baseDate">基準日</param>
        /// <returns>指定日が属する週の土曜日の日付</returns>
        public int GetLastDateOfWeek(int baseDate)
        {
            return CIUtil.GetLastDateOfWeek(baseDate);
        }

        /// <summary>
        /// 指定日が属する月の最終日を取得する
        /// </summary>
        /// <param name="baseDate">基準日</param>
        /// <returns></returns>
        public int GetLastDateOfMonth(int baseDate)
        {
            return CIUtil.GetLastDateOfMonth(baseDate);
        }

        ///<summary>
        ///指定の月数前の初日の日付を取得する
        ///</summary>
        ///<param name="baseDate">基準日</param>
        ///<param name="term">月数（1を指定すると前月、当月は0を指定する）</param>
        ///<returns>基準日の指定月数前の月の初日の日付</returns>
        public int MonthsBefore(int baseDate, int term)
        {
            return CIUtil.MonthsBefore(baseDate, term);
        }

        ///<summary>
        ///指定の月数後の日付を取得する
        ///</summary>
        ///<param name="baseDate">基準日</param>
        ///<param name="term">月数</param>
        ///<returns>基準日の指定月数後の日付</returns>
        public int MonthsAfter(int baseDate, int term)
        {
            int retDate = CIUtil.MonthsAfter(baseDate, term);

            DateTime? dt;
            DateTime dt1;

            dt = CIUtil.SDateToDateTime(retDate);
            if (dt != null)
            {
                dt1 = (DateTime)dt;

                if (dt1.Day < baseDate % 100)
                {
                    // 月数後の日が元の日より小さい場合は1日足す
                    dt1 = dt1.AddDays(1);
                }
                retDate = CIUtil.DateTimeToInt(dt1);
            }
            return retDate;
        }

        ///<summary>
        ///指定の月数後の日付を取得する
        ///休日の場合は、その前の休日以外の日付
        ///</summary>
        ///<param name="baseDate">基準日</param>
        ///<param name="term">月数</param>
        ///<returns>基準日の指定月数後の休日以外の日付</returns>
        public int MonthsAfterExcludeHoliday(int baseDate, int term)
        {
            int retDate = MonthsAfter(baseDate, term);

            DateTime? dt;
            DateTime dt1;

            dt = CIUtil.SDateToDateTime(retDate);
            if (dt != null)
            {
                dt1 = (DateTime)dt;
                int i = 1;

                while (IsHoliday(CIUtil.DateTimeToInt(dt1)) == true)
                {
                    // 休日の場合、1日前に移動
                    dt1 = dt1.AddDays(-1);
                    i++;
                    if (i > 31)
                    {
                        break;
                    }
                }
                retDate = CIUtil.DateTimeToInt(dt1);
            }
            return retDate;
        }

        ///<summary>
        ///指定の年数前の月の初日の日付を取得する
        ///</summary>
        ///<param name="baseDate">基準日</param>
        ///<param name="term">年数</param>
        ///<returns>基準日の指定年数前の月の初日の日付</returns>
        public int YearsBefore(int baseDate, int term)
        {
            return CIUtil.YearsBefore(baseDate, term);
        }
        #endregion

        /// <summary>
        /// 当該Rpの先頭項目の種類を返す
        /// </summary>
        /// <param name="filteredOdrDtl"></param>
        /// <returns>
        ///     0:診療行為
        ///     1:薬剤
        ///     2:特材
        ///     3:自費
        /// </returns>
        public int CheckFirstItemSbt(List<OdrDtlTenModel> filteredOdrDtl)
        {
            int index = filteredOdrDtl.FindIndex(p => new string[] { "S", "R", "Y", "T", "U" }.Contains(p.MasterSbt));
            int ret = 0;
            if (index >= 0)
            {
                if (filteredOdrDtl[index].TenMst != null && filteredOdrDtl[index].TenMst.JihiSbt > 0)
                {
                    ret = 3;
                }
                else if (filteredOdrDtl[index].MasterSbt == "Y")
                {
                    ret = 1;
                }
                else if (new string[] { "T", "U" }.Contains(filteredOdrDtl[index].MasterSbt))
                {
                    ret = 2;
                }
            }

            return ret;
        }

        /// <summary>
        /// 当該Rpの先頭項目の種類を返す（処置用）
        /// </summary>
        /// <param name="filteredOdrDtl"></param>
        /// <returns>
        ///     0:診療行為
        ///     1:薬剤
        ///     2:特材
        ///     3:自費
        ///     4:酸素
        /// </returns>
        public int CheckFirstItemSbtSyoti(List<OdrDtlTenModel> filteredOdrDtl)
        {
            int index = filteredOdrDtl.FindIndex(p => new string[] { "S", "R", "Y", "T", "U" }.Contains(p.MasterSbt));
            int ret = 0;
            if (index >= 0)
            {
                if (filteredOdrDtl[index].TenMst != null && filteredOdrDtl[index].TenMst.JihiSbt > 0)
                {
                    ret = 3;
                }
                else if (filteredOdrDtl[index].MasterSbt == "Y")
                {
                    ret = 1;
                }
                else if (new string[] { "T", "U" }.Contains(filteredOdrDtl[index].MasterSbt))
                {
                    if (filteredOdrDtl[index].IsSanso)
                    {
                        ret = 4;
                    }
                    else
                    {
                        ret = 2;
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// 指定の行為コードの自費項目の計算処理
        /// </summary>
        /// <param name="minKoui">行為コード下限</param>
        /// <param name="maxKoui">行為コード上限</param>
        /// <param name="kouiKbn">行為区分</param>
        /// <param name="sinId">診療ID</param>
        /// <param name="syukeisaki">集計先</param>
        /// <param name="cdKbn">コード区分</param>
        public void CalculateJihi(int minKoui, int maxKoui, int kouiKbn, int sinId, string syukeisaki, string cdKbn)
        {
            const string conFncName = nameof(CalculateJihi);

            List<OdrInfModel> filteredOdrInf;
            List<OdrDtlTenModel> filteredOdrDtl;

            // この行為に自費項目があるかチェック
            if (Odr.ExistJihiByKouiCd(minKoui, maxKoui))
            {
                // Rpを取得
                filteredOdrInf = Odr.FilterOdrInfByKouiKbnRange(minKoui, maxKoui);
                foreach (OdrInfModel odrInf in filteredOdrInf)
                {
                    // このRpに自費項目があるかチェック
                    if (Odr.ExistJihiByRp(odrInf.RpNo, odrInf.RpEdaNo))
                    {
                        // Rpと行為を準備しておく
                        Wrk.AppendNewWrkSinRpInf(kouiKbn, sinId, 2);
                        Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, syukeisaki, isNodspRece: 1, cdKbn: "JS");

                        // 詳細取得
                        filteredOdrDtl = Odr.FilterOdrDetailByRpNo(odrInf.RpNo, odrInf.RpEdaNo);

                        // このRpの最初のコメント以外の項目の種別
                        int firstItem = CheckFirstItemSbt(filteredOdrDtl);
                        // コメントレコードを読み飛ばすフラグ（コメント以外の項目に付随するコメントをくっつけるためのフラグ）
                        bool commentSkip = (firstItem != 3);
                        // 最初のコメント以外の項目であることを示すフラグ
                        bool firstSinryoKoui = true;

                        foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
                        {
                            if (odrDtl.IsJihi || (odrDtl.IsComment && commentSkip == false))
                            {
                                // 自費またはコメント

                                if (odrDtl.IsJihi)
                                {
                                    // 自費項目の場合、行為を追加するか判定
                                    if (firstSinryoKoui)
                                    {
                                        // 初回は追加不要
                                        firstSinryoKoui = false;
                                    }
                                    else
                                    {
                                        // 2回目以降は追加
                                        Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, syukeisaki, isNodspRece: NoDspConst.NoDsp, cdKbn: "JS");
                                    }
                                }

                                // 以降のコメントは付随させる
                                commentSkip = false;

                                // 項目追加
                                Wrk.AppendNewWrkSinKouiDetail(odrDtl, Odr.GetOdrCmt(odrDtl));

                                // 自費の場合、自費種別を行為にセットする
                                if (odrDtl.JihiSbt > 0 && Wrk.wrkSinKouis.Last().JihiSbt <= 0)
                                {
                                    Wrk.wrkSinKouis.Last().JihiSbt = odrDtl.JihiSbt;
                                }
                            }
                            else
                            {
                                // 自費、コメント以外の項目の場合、以降のコメントはこの項目に付随するコメントとみなして読み飛ばし
                                commentSkip = true;
                            }
                        }
                    }
                }

                Wrk.CommitWrkSinRpInf();
            }
        }


        /// <summary>
        /// CD_KBNを取得する
        /// 自費算定(SANTEI_KBN = 2)の場合は"JS"を返す
        /// </summary>
        /// <param name="santeiKbn">算定区分</param>
        /// <param name="Default">コード区分の初期値</param>
        /// <returns>コード区分</returns>
        public string GetCdKbn(int santeiKbn, string Default)
        {
            string cdKbn = Default;
            if (santeiKbn == SanteiKbnConst.Jihi)
            {
                cdKbn = "JS";
            }
            return cdKbn;
        }

        /// <summary>
        /// 自分以外の同一来院の来院リストを取得する
        /// </summary>
        /// <param name="raiinNo"></param>
        /// <returns></returns>
        public List<RaiinInfModel> GetDouituRaiin(long raiinNo, long oyaRaiinNo)
        {
            return raiinInfs.FindAll(p =>
                p.OyaRaiinNo == oyaRaiinNo &&
                p.RaiinNo != raiinNo);

        }

        /// <summary>
        /// マスタと紐づけられなかったオーダー項目をログに出力する
        /// </summary>
        public void CheckOdr()
        {
            // マスタがない
            List<OdrDtlTenModel> odrDtls = _odrCommon.odrDtlls.FindAll(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.RaiinNo == raiinNo &&
                p.TenMst == null &&
                p.ItemCd != null &&
                p.ItemCd != "" &&
                p.ItemCd.StartsWith("Y") == false
            ).Distinct().ToList();

            foreach (OdrDtlTenModel odrDtl in odrDtls)
            {
                if (!(odrDtl.ItemCd.StartsWith("Z") && odrDtl.InoutKbn == 1))
                {
                    // (Z特材 and 院外投薬)以外
                    AppendCalcLog(2, string.Format(FormatConst.NoMst, odrDtl.ItemName.Trim(), odrDtl.ItemCd));
                }
            }

            // 期限切れ
            odrDtls = _odrCommon.odrDtlls.FindAll(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.RaiinNo == raiinNo &&
                p.TenMst != null &&
                p.EndDate < sinDate &&
                p.ItemCd != null &&
                p.ItemCd != "" &&
                p.ItemCd.StartsWith("Y") == false
            ).Distinct().ToList();

            foreach (OdrDtlTenModel odrDtl in odrDtls)
            {
                if (!(odrDtl.ItemCd.StartsWith("Z") && odrDtl.InoutKbn == 1))
                {
                    // (Z特材 and 院外投薬)以外
                    AppendCalcLog(2, string.Format(FormatConst.OutTermMst, odrDtl.ItemName.Trim(), odrDtl.ItemCd, "~" + CIUtil.SDateToShowSDate(odrDtl.EndDate)));
                }
            }

            // 算定診療行為コードからマスタ検索できなかった項目
            odrDtls = _odrCommon.odrDtlls.FindAll(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.RaiinNo == raiinNo &&
                p.TenMst != null &&
                p.ItemCd != p.SanteiItemCd &&
                p.SanteiItemCd != ItemCdConst.NoSantei &&
                p.ItemCd != "" &&
                p.ItemCd.StartsWith("Y") == false
            ).Distinct().ToList();
            //odrDtls = _odrCommon.odrDtlls.FindAll(p =>
            //    p.HpId == hpId &&
            //    p.PtId == ptId &&
            //    p.RaiinNo == raiinNo &&
            //    ((p.TenMst != null && p.ItemCd != p.SanteiItemCd && p.ItemCd.Substring(1, 1) != "Y" && p.ItemCd.Substring(1, 1) != "Z") ||
            //     (p.TenMst == null && p.ItemCd.Substring(1, 1) != "Z")) &&
            //    p.SanteiItemCd != ItemCdConst.NoSantei &&
            //    p.ItemCd != ""                 
            //).Distinct().ToList();
            foreach (OdrDtlTenModel odrDtl in odrDtls)
            {
                if (odrDtl.ItemCd.StartsWith("Z"))
                {
                    if (odrDtl.InoutKbn != 1)
                    {
                        List<TenMstModel> tenMst = _mstCommon.GetTenMst(odrDtl.SanteiItemCd);
                        if (tenMst.Any() == false)
                        {
                            AppendCalcLog(2, string.Format(FormatConst.NoSanteiMst, odrDtl.ItemName.Trim(), odrDtl.ItemCd));
                        }
                    }
                }
                else
                {
                    AppendCalcLog(2, string.Format(FormatConst.NoSanteiMst, odrDtl.ItemName.Trim(), odrDtl.ItemCd));
                }
            }
        }

        /// <summary>
        /// 計算要求を追加する
        /// </summary>
        /// <param name="santeiDay">計算する診療日</param>
        /// <param name="seikyuUp">請求に反映するかどうか</param>
        /// <param name="calcMode">計算モード</param>
        public void AppendCalcStatusDays(HashSet<int> santeiDays, int seikyuUp, int calcMode)
        {
            List<CalcStatusModel> calcStatuses = new List<CalcStatusModel>();
            CalcStatusModel calcStatus = new CalcStatusModel(new CalcStatus());

            foreach (int santeiDay in santeiDays)
            {
                calcStatus.HpId = hpId;
                calcStatus.PtId = ptId;
                calcStatus.SinDate = santeiDay;
                calcStatus.SeikyuUp = seikyuUp;
                calcStatus.CalcMode = calcMode;
                calcStatus.UpdateStatus = UpdateStateConst.Add;

                calcStatuses.Add(calcStatus);
            }

            if (calcStatuses.Any())
            {
                _arg.saveHandler.AddCalcStatus(calcStatuses, _arg.preFix);
            }
        }

        public (int, int) GetPriorityPid(List<int> pids)
        {
            int retPid = 0;
            if (pids.Any())
            {
                retPid = pids.Last();
            }
            int retHid = 0;

            PtHokenPatternModel ptHokenPatternModel =
                _ptHokenPatternModels.Where(p => pids.Contains(p.HokenPid)).OrderBy(p => p.SortKey).FirstOrDefault();

            if (ptHokenPatternModel != null)
            {
                retPid = ptHokenPatternModel.HokenPid;
                retHid = ptHokenPatternModel.HokenId;
            }

            return (retPid, retHid);

        }
        public string GetSortKey(int hokenPid)
        {
            string ret = "";

            if (_ptHokenPatternModels.Any(p => p.HokenPid == hokenPid))
            {
                ret = _ptHokenPatternModels.First(p => p.HokenPid == hokenPid).SortKey;
            }

            return ret;

        }
        public bool IsBuntenKohi(int pid)
        {
            bool ret = false;

            int[] hokenSbtKbn = { 5, 6 };

            ret = _ptHokenPatternModels.Any(p =>
                p.HokenPid == pid &&
                (hokenSbtKbn.Contains(p.Kohi1HokenSbtKbn) ||
                  hokenSbtKbn.Contains(p.Kohi2HokenSbtKbn) ||
                  hokenSbtKbn.Contains(p.Kohi3HokenSbtKbn) ||
                  hokenSbtKbn.Contains(p.Kohi4HokenSbtKbn)));

            return ret;
        }

        public List<int> CheckHokenKbn
        {
            get { return checkHokenKbn; }
        }

        /// <summary>
        /// 算定状況チェック（ITEM_CD使用）（通常の場合はWRKから、試算の場合はSINから取得）
        /// </summary>
        /// <param name="itemCds"></param>
        /// <param name="onlyThisRaiin"></param>
        /// <param name="excludeSanteiGai"></param>
        /// <returns></returns>
        public bool ExistWrkOrSinKouiDetailByItemCd(List<string> itemCds, bool onlyThisRaiin = true, bool excludeSanteiGai = true, bool sameHokenSbt = false)
        {
            bool ret = false;

            if (calcMode != CalcModeConst.Trial)
            {
                ret = _wrkCommon.ExistWrkSinKouiDetailByItemCd(itemCds, onlyThisRaiin, excludeSanteiGai, sameHokenSbt);
            }
            else
            {
                ret = _sinCommon.ExistSinKouiDetailByItemCd(itemCds);
            }

            return ret;
        }
        public bool ExistTodayKouiDetailByItemCd(List<string> itemCds, bool onlyThisRaiin = true, bool excludeSanteiGai = true, bool sameHokenSbt = false, bool includeDelete = false)
        {
            bool ret = false;

            if (calcMode != CalcModeConst.Trial)
            {
                ret = _wrkCommon.ExistWrkSinKouiDetailByItemCd(itemCds, onlyThisRaiin, excludeSanteiGai, sameHokenSbt);
            }

            if (ret == false)
            {
                if (includeDelete == false)
                {
                    ret = _sinCommon.ExistSinKouiDetailByItemCd(itemCds);
                }
                else
                {
                    List<long> raiinNos = _wrkCommon.wrkSinRpInfs.GroupBy(p => p.RaiinNo).Select(p => p.Key).ToList();
                    ret = _sinCommon.ExistSinKouiDetailByItemCdIncludeDelete(itemCds, raiinNos);
                }
            }

            return ret;
        }
        /// <summary>
        /// 指定日以前に、指定の項目が最後にオーダーされた日を返す
        /// </summary>
        /// <param name="sinDate"></param>
        /// <param name="itemCd"></param>
        /// <returns></returns>
        public int GetZenkaiOdrDate(int baseDate, string itemCd)
        {
            return _arg.odrInfFinder.GetZenkaiOdrDate(hpId, ptId, baseDate, itemCd);

        }

        /// <summary>
        /// コロナに伴う電話再診かどうか判断
        /// 1. 電話再診（電話再診２科目）で、慢性疾患の診療（新型コロナウイルス感染症・診療報酬上臨時的取扱） がオーダーされている
        /// 2. 2020/04/01～2020/04/09で、電話再診（電話再診２科目）で、管理料（情報通信機器）がオーダーされている
        /// </summary>
        /// <returns></returns>
        public bool IsCoronaDenwaSaisin()
        {
            bool ret = false;
            List<string> itemCds = new List<string>
                {
                    // 特定疾患療養管理料（情報通信機器）
                    ItemCdConst.IgakuTokusituJyohoTusin,
                    // 小児科療養指導料（情報通信機器）
                    ItemCdConst.IgakuSyouniRyoyoJyohoTusin,
                    // てんかん指導料（情報通信機器）
                    ItemCdConst.IgakuTenkanJyohoTusin,
                    // 難病外来指導管理料（情報通信機器）
                    ItemCdConst.IgakuNanbyoJyohoTusin,
                    // 糖尿病透析予防指導管理料（情報通信機器）
                    ItemCdConst.IgakuTounyouJyohoTusin,
                    // 地域包括診療料（情報通信機器）
                    ItemCdConst.IgakuTiikiHoukatuJyohoTusin,
                    // 認知症地域包括診療料（情報通信機器）
                    ItemCdConst.IgakuNintiTiikiJyohoTusin,
                    // 生活習慣病管理料（情報通信機器）
                    ItemCdConst.IgakuSeikatuJyohoTusin
                };

            if (syosai == SyosaiConst.SaisinDenwa || syosai == SyosaiConst.SaisinDenwa2)
            {
                if (Odr.ExistOdrDetailByItemCd(ItemCdConst.IgakuManseiCorona))
                {
                    ret = true;
                }
                else if (sinDate >= 20200401 && sinDate <= 20200409)
                {
                    if (Odr.ExistOdrDetailByItemCd(itemCds))
                    {
                        ret = true;
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// 指定の診療行為コードが選択式コメントのものかチェック
        /// </summary>
        /// <param name="itemCd"></param>
        /// <returns></returns>
        public bool IsSelectComment(string itemCd)
        {
            return (
                itemCd.StartsWith("8") &&
                itemCd.Length == 9 &&
                itemCd != ItemCdConst.GazoDensibaitaiHozon &&
                Mst.RecedenCmtSelectExistByCommentCd(itemCd)
                );
        }

        public bool AppendNewWrkSinKouiDetailAgeKasan(OdrDtlTenModel odrDtl, List<OdrDtlTenModel> odrDtls)
        {
            bool ret = false;

            string kasanCd = Wrk.GetAgeKasanCd(odrDtl);

            if (CheckSanteiKaisu(kasanCd, odrDtl.SanteiKbn, 1) != 2)
            {
                ret = Wrk.AppendNewWrkSinKouiDetailAgeKasan(odrDtl, odrDtls);
            }

            return ret;
        }

        /// <summary>
        /// オーダー詳細から当該項目で算定可能な年齢加算項目を追加する（処置用）
        /// </summary>
        /// <param name="odrDtl">オーダー詳細</param>
        /// <returns>算定した加算の点数</returns>
        public double AppendNewWrkSinKouiDetailAgeKasanSyoti(OdrDtlTenModel odrDtl)
        {
            double retTen = 0;
            string itemCd = "";

            if (sinDate >= 20220401 &&
                Age < 6 &&
                odrDtl.JibiAgeKasan == 1 &&
                Mst.ExistAutoSantei(ItemCdConst.SyotiJibiNyuyojiKasan) &&
                Odr.ExistOdrDetailByItemCd(ItemCdConst.SyotiJibiNyuyojiKasanCancel) == false)
            {
                itemCd = ItemCdConst.SyotiJibiNyuyojiKasan;
            }

            if (itemCd == "")
            {
                for (int i = 1; i <= 4; i++)
                {
                    if (Wrk.IsValidAgeKasanConf(odrDtl.AgekasanMin(i), odrDtl.AgekasanMax(i), odrDtl.AgekasanCd(i)) == false)
                    {
                        break;
                    }
                    else if (CheckAgeMin(odrDtl.AgekasanMin(i)) && CheckAgeMax(odrDtl.AgekasanMax(i)))
                    {
                        itemCd = odrDtl.AgekasanCd(i);

                        break;
                    }
                }
            }

            if (itemCd == "")
            {
                ///     　1: ３歳未満乳幼児加算（処置）（１１０点）が算定できる診療行為
                ///     　2: ３歳未満乳幼児加算（処置）（５５点）が算定できる診療行為
                ///     　3: ６歳未満乳幼児加算（処置）（１１０点）が算定できる診療行為
                ///     　4: ６歳未満乳幼児加算（処置）（８３点）が算定できる診療行為
                ///     　5: ６歳未満乳幼児加算（処置）（５５点）が算定できる診療行為
                itemCd = Wrk.GetSyotiNyuyojiKasanItemCd(odrDtl.SyotiNyuyojiKbn);
            }

            if (itemCd != "")
            {
                if (CheckSanteiKaisu(itemCd, odrDtl.SanteiKbn, 1) != 2)
                {
                    Wrk.AppendNewWrkSinKouiDetail(itemCd, 1);
                    List<TenMstModel> tenMst = Mst.GetTenMst(itemCd);
                    if (tenMst.Any())
                    {
                        if (tenMst.First().IsSyotiJikangaiTarget) 
                        {
                            retTen = tenMst.First().Ten;
                        }
                    }
                }
            }

            return retTen;
        }
    }

}
