using Entity.Tenant;
using EmrCalculateApi.Ika.DB.Finder;
using EmrCalculateApi.Ika.DB.CommandHandler;
using EmrCalculateApi.Ika.Models;
using EmrCalculateApi.Ika.Constants;
using Helper.Constants;
using EmrCalculateApi.Utils;
using Infrastructure.Interfaces;
using Helper.Common;
using Domain.Constant;
using EmrCalculateApi.Constants;
using EmrCalculateApi.Interface;

namespace EmrCalculateApi.Ika.ViewModels
{
    public class IkaCalculateCommonWrkDataViewModel
    {
        SanteiFinder _santeiFinder;

        private List<WrkSinRpInfModel> _wrkSinRpInfs;
        private List<WrkSinKouiModel> _wrkSinKouis;
        private List<WrkSinKouiDetailModel> _wrkSinKouiDetails;
        private List<WrkSinKouiDetailDelModel> _wrkSinKouiDetailDels;

        private IkaCalculateCommonMasterViewModel _tenMstCommon;
        private PtInfModel _ptInf;

        private int _hpId;
        private long _ptId;
        private int _sinDate;
        private long _raiinNo;
        private long _oyaRaiinNo;
        private int _hokenKbn;

        private int _wrkRpNo;
        private int _wrkSeqNo;
        private int _wrkRowNo;

        private int _wrkDtlDelItemSeqNo;
        private string _wrkDtlDelItemCd;
        private string _kokuji1;
        private string _kokuji2;
        private string _tyuCd;
        private string _tyuSeq;
        private int _tusokuAge;
        private int _itemSeqNo;
        private int _itemEdaNo;

        private int _syosaiPid;

        private List<int> checkHokenKbn;
        private List<int> checkSanteiKbn;

        private List<SanteiInfDetailModel> _santeiInfDtls;
        ISystemConfigProvider _systemConfigProvider;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="santeiFinder"></param>
        /// <param name="tenMstCommon"></param>
        /// <param name="ptInf"></param>
        /// <param name="hpId"></param>
        /// <param name="ptId"></param>
        /// <param name="sinDate"></param>
        public IkaCalculateCommonWrkDataViewModel(SanteiFinder santeiFinder, IkaCalculateCommonMasterViewModel tenMstCommon, PtInfModel ptInf, int hpId, long ptId, int sinDate, 
            ISystemConfigProvider systemConfigProvider)
        {
            _santeiFinder = santeiFinder;

            _wrkSinRpInfs = new List<WrkSinRpInfModel>();
            _wrkSinKouis = new List<WrkSinKouiModel>();
            _wrkSinKouiDetails = new List<WrkSinKouiDetailModel>();
            _wrkSinKouiDetailDels = new List<WrkSinKouiDetailDelModel>();
            _wrkSinRpInfs = new List<WrkSinRpInfModel>();

            _tenMstCommon = tenMstCommon;
            _ptInf = ptInf;

            _hpId = hpId;
            _ptId = ptId;
            _sinDate = sinDate;

            _santeiInfDtls = _santeiFinder.FindSanteiInfDetail(_hpId, _ptId, _sinDate);

            checkHokenKbn = new List<int>();
            checkSanteiKbn = new List<int>();

            _systemConfigProvider = systemConfigProvider;
        }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId
        {
            get { return _hpId; }
        }

        /// <summary>
        /// 患者ID
        /// </summary>
        public long PtId
        {
            get { return _ptId; }
        }

        /// <summary>
        /// 診療日
        /// </summary>
        public int SinDate
        {
            get { return _sinDate; }
        }

        /// <summary>
        /// 来院番号
        /// </summary>
        public long RaiinNo
        {
            get { return _raiinNo; }
            set { _raiinNo = value; }
        }
        /// <summary>
        /// 親来院番号
        /// </summary>
        public long OyaRaiinNo
        {
            get { return _oyaRaiinNo; }
            set { _oyaRaiinNo = value; }
        }
        /// <summary>
        /// 診察開始時間
        /// </summary>
        public string SinStartTime { get; set; }

        /// <summary>
        /// 保険区分
        /// </summary>
        public int HokenKbn
        {
            get { return _hokenKbn; }
            set
            {
                _hokenKbn = value;

                checkHokenKbn = CalcUtils.GetCheckHokenKbns(_hokenKbn, _systemConfigProvider.GetHokensyuHandling());
                checkSanteiKbn = CalcUtils.GetCheckSanteiKbns(_hokenKbn, _systemConfigProvider.GetHokensyuHandling());
            }
        }

        /// <summary>
        /// 初再診の保険組み合わせID
        /// </summary>
        public int SyosaiPid
        {
            get { return _syosaiPid; }
            set { _syosaiPid = value; }
        }

        /// <summary>
        /// 現在のRpNo
        /// </summary>
        public int RpNo
        {
            get { return _wrkRpNo; }
        }

        /// <summary>
        /// 現在のSeqNo
        /// </summary>
        public int SeqNo
        {
            get { return _wrkSeqNo; }
        }

        /// <summary>
        /// ワーク診療Rp情報
        /// </summary>
        public List<WrkSinRpInfModel> wrkSinRpInfs
        {
            get { return _wrkSinRpInfs; }
        }

        /// <summary>
        /// ワーク診療行為情報
        /// </summary>
        public List<WrkSinKouiModel> wrkSinKouis
        {
            get { return _wrkSinKouis; }
        }

        /// <summary>
        /// ワーク診療行為情報詳細
        /// </summary>
        public List<WrkSinKouiDetailModel> wrkSinKouiDetails
        {
            get { return _wrkSinKouiDetails; }
        }

        /// <summary>
        /// ワーク診療行為情報詳細削除
        /// </summary>
        public List<WrkSinKouiDetailDelModel> wrkSinKouiDetailDels
        {
            get { return _wrkSinKouiDetailDels; }
        }

        /// <summary>
        /// ローカル変数の初期化
        /// </summary>
        public void Clear()
        {
            _wrkRpNo = 0;
            _wrkSeqNo = 0;
            _wrkRowNo = 0;

            _wrkSinRpInfs.Clear();
            _wrkSinKouis.Clear();
            _wrkSinKouiDetails.Clear();
            _wrkSinKouiDetailDels.Clear();

        }

        #region データ操作
        #region データ操作 - ワークRp情報関連
        /// <summary>
        /// ワークRp情報を生成する
        /// </summary>
        /// <param name="hokenPid">医療機関識別ID</param>
        /// <param name="sinKouiKbn">診療行為区分</param>
        /// <param name="sinId">診療識別</param>
        /// <param name="santeiKbn">算定区分　1:自費</param>
        /// <returns></returns>
        public WrkSinRpInfModel GetWrkSinRpInf(int sinKouiKbn, int sinId, int santeiKbn)
        {
            WrkSinRpInfModel wrkSinRpInfModel = new WrkSinRpInfModel(new WrkSinRpInf());

            wrkSinRpInfModel.HpId = _hpId;
            wrkSinRpInfModel.PtId = _ptId;
            wrkSinRpInfModel.SinDate = _sinDate;
            wrkSinRpInfModel.RaiinNo = _raiinNo;
            wrkSinRpInfModel.HokenKbn = _hokenKbn;
            wrkSinRpInfModel.SinKouiKbn = sinKouiKbn;
            wrkSinRpInfModel.SinId = sinId;
            wrkSinRpInfModel.SanteiKbn = santeiKbn;

            return wrkSinRpInfModel;
        }

        /// <summary>
        /// ワークRp情報を追加する
        /// 現在、追加中のワークRp情報がある場合、付随する診療行為が存在するかチェックし、
        /// なければ追加を取り消して、新たに追加する
        /// </summary>
        /// <param name="wrkSinRpInfModel">ワークRp情報</param>
        public void AppendWrkSinRpInf(WrkSinRpInfModel wrkSinRpInfModel)
        {
            if (_wrkSinRpInfs.Any())
            {
                CommitWrkSinRpInf();
            }

            _wrkRpNo++;
            _wrkSeqNo = 0;
            _wrkRowNo = 0;

            wrkSinRpInfModel.RpNo = _wrkRpNo;
            _wrkSinRpInfs.Add(wrkSinRpInfModel);

        }

        /// <summary>
        /// ワークRp情報を生成し、追加する
        /// </summary>
        /// <param name="sinKouiKbn">診療行為区分</param>
        /// <param name="sinId">診療識別</param>
        /// <param name="santeiKbn">算定区分　1:自費</param>
        public void AppendNewWrkSinRpInf(int sinKouiKbn, int sinId, int santeiKbn)
        {
            AppendWrkSinRpInf(GetWrkSinRpInf(sinKouiKbn, sinId, santeiKbn));
        }

        /// <summary>
        /// ワークRp情報を確定する
        /// もし、付随するワーク診療行為がない場合は、追加を取り消す
        /// </summary>
        public void CommitWrkSinRpInf()
        {
            CommitWrkSinKoui();

            if (_wrkSinKouis.Any() == false || _wrkSinKouis.Any(p => p.RpNo == _wrkRpNo) == false)
            {
                _wrkSinRpInfs.RemoveAll(p => p.RpNo == _wrkRpNo);
                _wrkRpNo--;
            }
            else if (_wrkSinKouis.Any())
            {
                // CdNoを採番
                List<WrkSinKouiDetailModel> wrkDtls = _wrkSinKouiDetails.FindAll(p => p.RpNo == _wrkRpNo);

                if (wrkDtls.Any())
                {
                    string cdNo = "";

                    for (int i = 0; i <= 1; i++)
                    {
                        List<WrkSinKouiDetailModel> filteredWrkDtls = new List<WrkSinKouiDetailModel>();

                        if (i == 0)
                        {
                            filteredWrkDtls =
                                wrkDtls.FindAll(p => new string[] { "1", "3", "5" }.Contains(p.Kokuji2));
                        }
                        else
                        {
                            filteredWrkDtls =
                                wrkDtls.FindAll(p => !(new string[] { "1", "3", "5" }.Contains(p.Kokuji2)));
                        }

                        foreach (WrkSinKouiDetailModel wrkDtl in filteredWrkDtls)
                        {
                            List<TenMstModel> tenMst = new List<TenMstModel>();

                            if (wrkDtl.TenMst != null)
                            {
                                //tenMst.Add(wrkDtl.TenMst);
                                if (wrkDtl.TenMst.CdKbn != "" && wrkDtl.TenMst.CdKbn != "-" && wrkDtl.TenMst.CdKbn != "*")
                                {
                                    string tmpCdNo =
                                    wrkDtl.TenMst.CdKbn +
                                        String.Format("{0:D3}{1:D2}{2:D2}",
                                            wrkDtl.TenMst.CdKbnno, wrkDtl.TenMst.CdEdano, wrkDtl.TenMst.CdKouno);

                                    if (string.IsNullOrEmpty(cdNo) || string.Compare(cdNo, tmpCdNo) == 1)
                                    {
                                        cdNo = tmpCdNo;
                                    }
                                }
                            }
                            else
                            {
                                tenMst = _tenMstCommon.GetTenMst(wrkDtl.ItemCd);

                                if (tenMst.First().CdKbn != "" && tenMst.First().CdKbn != "-" && tenMst.First().CdKbn != "*")
                                {
                                    string tmpCdNo =
                                    tenMst.First().CdKbn +
                                    String.Format("{0:D3}{1:D2}{2:D2}",
                                        tenMst.First().CdKbnno, tenMst.First().CdEdano, tenMst.First().CdKouno);

                                    if (string.IsNullOrEmpty(cdNo) || string.Compare(cdNo, tmpCdNo) == 1)
                                    {
                                        cdNo = tmpCdNo;
                                    }
                                }
                            }

                            _wrkSinRpInfs.Last().CdNo = cdNo;
                        }

                        if (string.IsNullOrEmpty(_wrkSinRpInfs.Last().CdNo) == false)
                        {
                            break;
                        }
                    }
                    //foreach (WrkSinKouiDetailModel wrkDtl in wrkDtls)
                    //{
                    //    List<TenMstModel> tenMst = new List<TenMstModel>();

                    //    if (wrkDtl.TenMst != null)
                    //    {
                    //        tenMst.Add(wrkDtl.TenMst);
                    //    }
                    //    else
                    //    {
                    //        tenMst = _tenMstCommon.GetTenMst(wrkDtl.ItemCd);
                    //    }

                    //    if (tenMst.Any())
                    //    {
                    //        if (tenMst.First().CdKbn != "" && tenMst.First().CdKbn != "-" && tenMst.First().CdKbn != "*")
                    //        {
                    //            _wrkSinRpInfs.Last().CdNo =
                    //                tenMst.First().CdKbn +
                    //                String.Format("{0:D3}{1:D2}{2:D2}",
                    //                    tenMst.First().CdKbnno, tenMst.First().CdEdano, tenMst.First().CdKouno);
                    //            break;
                    //        }
                    //    }
                    //}
                }

                if (_wrkSinRpInfs.Last().CdNo == "")
                {
                    List<WrkSinKouiModel> wrkSins = _wrkSinKouis.FindAll(p => p.RpNo == _wrkRpNo);

                    if (wrkSins.Any())
                    {
                        if (wrkSins.First().CdKbn == "F" && new int[] { 21, 22, 23 }.Contains(_wrkSinRpInfs.Last().SinId) &&
                            _wrkSinKouiDetails.Any(p => p.RpNo == _wrkRpNo && p.ItemCd.StartsWith("6")))
                        {
                            // 投薬で薬剤を含むRpの場合は、一番上にくるように
                            _wrkSinRpInfs.Last().CdNo = wrkSins.First().CdKbn + "0000000";
                        }
                        else if (_wrkSinKouiDetails.Any(p => p.RpNo == _wrkRpNo && p.ItemCd.StartsWith("6")))
                        {
                            // 薬剤を含む場合
                            _wrkSinRpInfs.Last().CdNo = wrkSins.First().CdKbn + "9999997";
                        }
                        else if (_wrkSinKouiDetails.Any(p => p.RpNo == _wrkRpNo && p.ItemCd.StartsWith("7")))
                        {
                            // 特材を含む場合
                            _wrkSinRpInfs.Last().CdNo = wrkSins.First().CdKbn + "9999998";
                        }
                        else
                        {
                            _wrkSinRpInfs.Last().CdNo = wrkSins.First().CdKbn + "9999999";
                        }
                    }

                    if (_wrkSinRpInfs.Last().CdNo == "")
                    {
                        // コードが確定できない場合、とりあえず、一番下になるようにしておく
                        _wrkSinRpInfs.Last().CdNo = "Z9999999";
                    }
                }

                // 削除分しかない場合
                if (_wrkSinKouis.Any(p => p.RpNo == _wrkRpNo && p.IsDeleted == 0) == false)
                {
                    _wrkSinRpInfs.Last().IsDeleted = DeleteStatus.DeleteFlag;
                }
            }
        }

        #endregion

        #region データ操作 - ワーク診療行為関連
        /// <summary>
        /// ワーク診療行為を生成する
        /// </summary>
        /// <param name="syukeiSaki">集計先</param>
        /// <param name="count">回数</param>
        /// <param name="inoutKbn">院内院外</param>
        /// <param name="isNodspRece">レセ非表示フラグ</param>
        /// <param name="isNodspPaperRece">紙レセ非表示フラグ</param>
        /// <returns></returns>
        public WrkSinKouiModel GetWrkSinKoui(int hokenPid, int hokenId, string syukeiSaki, int count = 1, int inoutKbn = 0, int isNodspRece = 0, int isNodspPaperRece = 0, int hokatuKensa = 0, string cdKbn = "", int jihiSbt = 0, long odrRpNo = 0)
        {
            WrkSinKouiModel wrkSinKouiModel = new WrkSinKouiModel(new WrkSinKoui());

            wrkSinKouiModel.HpId = _hpId;
            wrkSinKouiModel.PtId = _ptId;
            wrkSinKouiModel.SinDate = _sinDate;
            wrkSinKouiModel.RaiinNo = _raiinNo;
            wrkSinKouiModel.HokenKbn = _hokenKbn;
            //wrkSinKouiModel.RecId = recId;
            wrkSinKouiModel.HokenPid = hokenPid;
            wrkSinKouiModel.HokenId = hokenId;
            wrkSinKouiModel.SyukeiSaki = syukeiSaki;
            wrkSinKouiModel.Count = (count == 0 ? 1 : count);
            wrkSinKouiModel.IsNodspRece = isNodspRece;
            wrkSinKouiModel.IsNodspPaperRece = isNodspPaperRece;
            wrkSinKouiModel.InoutKbn = inoutKbn;
            wrkSinKouiModel.HokatuKensa = hokatuKensa;
            wrkSinKouiModel.CdKbn = cdKbn;
            wrkSinKouiModel.JihiSbt = jihiSbt;
            wrkSinKouiModel.OdrRpNo = odrRpNo;

            return wrkSinKouiModel;
        }

        /// <summary>
        /// ワーク診療行為を追加する
        /// 現在、追加中のワーク診療行為がある場合、付随する詳細が存在するかチェックし、
        /// なければ追加を取り消して、新たに追加する
        /// </summary>
        /// <param name="wrkSinKouiModel">ワーク診療行為</param>
        public void AppendWrkSinKoui(WrkSinKouiModel wrkSinKouiModel)
        {
            if (_wrkSeqNo > 0)
            {
                CommitWrkSinKoui();
            }

            _wrkSeqNo++;
            _wrkRowNo = 0;

            _kokuji1 = "";
            _kokuji2 = "";
            _tyuCd = "";
            _tyuSeq = "";
            _tusokuAge = 0;
            _itemSeqNo = 0;
            _itemEdaNo = 0;

            wrkSinKouiModel.RpNo = _wrkRpNo;
            wrkSinKouiModel.SeqNo = _wrkSeqNo;
            _wrkSinKouis.Add(wrkSinKouiModel);

            if (wrkSinKouiModel.HokenPid < 0)
            {
                // 初期値として、初再診の保険組み合わせIDをセットしておく
                wrkSinKouiModel.HokenPid = _syosaiPid;
            }
        }

        /// <summary>
        /// ワーク診療行為を生成し、追加する
        /// </summary>
        /// <param name="syukeiSaki">集計先</param>
        /// <param name="count">回数</param>
        /// <param name="inoutKbn">院内院外</param>
        /// <param name="isNodspRece">レセ非表示フラグ</param>
        /// <param name="isNodspPaperRece">紙レセ非表示フラグ</param>
        public void AppendNewWrkSinKoui(int hokenPid, int hokenId, string syukeiSaki, int count = 1, int inoutKbn = 0, int isNodspRece = 0, int isNodspPaperRece = 0, int hokatuKensa = 0, string cdKbn = "", int jihiSbt = 0, long odrRpNo = 0)
        {
            AppendWrkSinKoui(GetWrkSinKoui(hokenPid, hokenId, syukeiSaki, count, inoutKbn, isNodspRece, isNodspPaperRece, hokatuKensa, cdKbn, jihiSbt, odrRpNo));
        }

        /// <summary>
        /// ワーク診療行為の集計先変更または追加する
        /// </summary>
        /// <param name="hokenPid"></param>
        /// <param name="Syukeisaki"></param>
        /// <param name="cdKbn"></param>
        /// <param name="AfirstSinryoKoui">ture: 追加（falseに変えて返す）</param>
        public void AppendOrUpdateKoui(int hokenPid, int hokenId, string Syukeisaki, string cdKbn, ref bool AfirstSinryoKoui, long odrRpNo = 0)
        {
            if (AfirstSinryoKoui == true && _wrkSinKouis.Any() &&
                (_wrkSinKouiDetails.Any(p => p.RpNo == _wrkSinKouis.Last().RpNo && p.SeqNo == _wrkSinKouis.Last().SeqNo) == false))
            {
                AfirstSinryoKoui = false;
                _wrkSinKouis.Last().SyukeiSaki = Syukeisaki;
            }
            else
            {
                // 行為を追加する
                AppendNewWrkSinKoui(hokenPid, hokenId, Syukeisaki, cdKbn: cdKbn, odrRpNo: odrRpNo);
            }
        }

        /// <summary>
        /// ワーク診療行為を確定する
        /// もし、付随するワーク診療行為詳細がない場合は、追加を取り消す
        /// </summary>
        public void CommitWrkSinKoui()
        {
            if (_wrkSinKouiDetails.Any() == false || _wrkSinKouiDetails.Any(p => p.RpNo == _wrkRpNo && p.SeqNo == _wrkSeqNo) == false)
            {
                //付随する詳細がない場合、ワーク診療行為を削除する
                _wrkSinKouis.RemoveAll(p => p.RpNo == _wrkRpNo && p.SeqNo == _wrkSeqNo);
                _wrkSeqNo--;
            }
            else if (_wrkSinKouiDetails.Any())
            {
                // 検査コメント項目とコメントのみのRpの場合、検査コメント項目をフリーコメントに変換
                List<WrkSinKouiDetailModel> filteredWrkSinKouiDtls = _wrkSinKouiDetails.FindAll(p => p.RpNo == _wrkRpNo && p.SeqNo == _wrkSeqNo);
                bool kensaCmtOnly =
                    (filteredWrkSinKouiDtls.Any(p => p.TenMst != null && p.TenMst.KensaCmt == 1)) &&
                    (filteredWrkSinKouiDtls.Any(p => p.TenMst != null && p.TenMst.KensaCmt != 1 && p.RecId != "CO") == false);
                foreach (WrkSinKouiDetailModel wrkSinDtl in filteredWrkSinKouiDtls)
                {
                    if (kensaCmtOnly && wrkSinDtl.TenMst != null && wrkSinDtl.TenMst.KensaCmt == 1)
                    {
                        wrkSinDtl.ItemCd = ItemCdConst.CommentFree;
                        wrkSinDtl.RecId = "CO";
                        wrkSinDtl.CmtOpt = wrkSinDtl.ItemName;
                        wrkSinDtl.TenMst = _tenMstCommon.GetTenMst(ItemCdConst.CommentFree)?.First() ?? null;
                    }
                }

                // レセ非表示区分チェック
                if (_wrkSinKouiDetails.Any(p =>
                    p.RpNo == _wrkRpNo &&
                    p.SeqNo == _wrkSeqNo &&
                    new int[] { 0, 2 }.Contains(p.IsNodspRece)) == false)
                {
                    _wrkSinKouis.Last().IsNodspRece = 1;
                }

                if (_wrkSinKouiDetails.Any(p =>
                        p.RpNo == _wrkRpNo &&
                        p.SeqNo == _wrkSeqNo &&
                        p.IsNodspPaperRece == 0) == false)
                {
                    _wrkSinKouis.Last().IsNodspPaperRece = 1;
                }

                // 削除フラグチェック
                if (_wrkSinKouiDetails.Any(p =>
                    p.RpNo == _wrkRpNo &&
                    p.SeqNo == _wrkSeqNo &&
                    p.IsDeleted == 0) == false)
                {
                    _wrkSinKouis.Last().IsDeleted = 1;
                }

                // RecId
                if (_wrkSinKouiDetails.Any(p =>
                        p.RpNo == _wrkSinKouis.Last().RpNo &&
                        p.SeqNo == _wrkSinKouis.Last().SeqNo &&
                        p.IsNodspRece == 0 &&
                        p.RecId == "SI"))
                {
                    _wrkSinKouis.Last().RecId = "SI";
                }
                else if (_wrkSinKouis.Last().CdKbn == "J" &&
                    _wrkSinKouiDetails.Any(p =>
                        p.RpNo == _wrkSinKouis.Last().RpNo &&
                        p.SeqNo == _wrkSinKouis.Last().SeqNo &&
                        p.TenMst != null &&
                        new int[] { 2, 3, 4, 5 }.Contains(p.TenMst.SansoKbn)))
                {
                    // 処置酸素は手技にまとめる
                    _wrkSinKouis.Last().RecId = "SI";
                }
                //else if (_wrkSinRpInfs.Any(p => 
                //            p.RpNo == _wrkSinKouis.Last().RpNo && 
                //            (p.SinId == 31 || p.SinId == 32)))
                //{
                //    // 静脈注射、皮下筋肉内注射行為は手技薬剤特材をひとつのRpにまとめる
                //    _wrkSinKouis.Last().RecId = "SI";
                //}
                else if (_wrkSinKouiDetails.Any(p =>
                            p.RpNo == _wrkSinKouis.Last().RpNo &&
                            p.SeqNo == _wrkSinKouis.Last().SeqNo &&
                            p.RecId == "IY"))
                {
                    _wrkSinKouis.Last().RecId = "IY";
                }
                else if (_wrkSinKouiDetails.Any(p =>
                            p.RpNo == _wrkSinKouis.Last().RpNo &&
                            p.SeqNo == _wrkSinKouis.Last().SeqNo &&
                            p.IsNodspRece == 0 &&
                            p.RecId == "CO") &&
                        _wrkSinKouiDetails.Any(p =>
                            p.RpNo == _wrkSinKouis.Last().RpNo &&
                            p.SeqNo == _wrkSinKouis.Last().SeqNo &&
                            p.IsNodspRece == 0 &&
                            p.RecId != "CO") == false &&
                        _wrkSinKouiDetails.Any(p =>
                            p.RpNo == _wrkSinKouis.Last().RpNo &&
                            p.SeqNo == _wrkSinKouis.Last().SeqNo &&
                            p.IsNodspRece != 0 &&
                            p.RecId == "SI") &&
                         _wrkSinKouiDetails.Any(p =>
                            p.RpNo == _wrkSinKouis.Last().RpNo &&
                            p.SeqNo == _wrkSinKouis.Last().SeqNo &&
                            p.IsNodspRece != 0 &&
                            p.RecId != "SI") == false)
                {
                    // IS_NODSP_RECE=0でCO以外の項目がなく、
                    // IS_NODSP_RECE<>0でSI以外の項目がない場合
                    // つまり、COのみが表示項目で、SIのみが非表示項目の場合はSI扱い
                    _wrkSinKouis.Last().RecId = "SI";
                }
                else
                {
                    _wrkSinKouis.Last().RecId = "TO";
                }
            }
        }

        #endregion

        #region データ操作 - ワーク診療行為詳細関連
        /// <summary>
        /// ワーク診療行為詳細の1レコード分のデータを返す
        /// 自動発生項目用
        /// </summary>
        /// <param name="itemCd">診療行為コード</param>
        /// <param name="autoAdd">1:自動追加</param>
        /// <param name="itemName">診療行為名称</param>
        /// <param name="suryo">数量</param>
        /// <param name="suryo2">数量2</param>
        /// <param name="fmtKbn">書式区分</param>
        /// <param name="itemSbt">項目種別</param>
        /// <param name="cmtOpt">コメント文</param>
        /// <param name="cmtCd1">コメントコード１</param>
        /// <param name="cmtOpt1">コメント文１</param>
        /// <param name="cmtCd2">コメントコード２</param>
        /// <param name="cmtOpt2">コメント文２</param>
        /// <param name="cmtCd3">コメントコード３</param>
        /// <param name="cmtOpt3">コメント文３</param>
        /// <param name="isNodspRece">
        /// レセプト非表示区分
        /// 1:非表示
        /// 2:電算のみ非表示
        /// </param>
        /// <param name="isNodspPaperRece">紙レセ非表示区分　1:非表示</param>
        /// <param name="isNodspRyosyu">領収証非表示区分　1:非表示</param>
        /// <returns>ワーク診療行為詳細の1レコード分のデータ</returns>
        public WrkSinKouiDetailModel GetWrkSinKouiDetail
            (string itemCd, int autoAdd = 0, string itemName = "", double suryo = 1, double suryo2 = 0, int fmtKbn = 0, int itemSbt = -1, string cmtOpt = "",
            string cmtCd1 = "", string cmtOpt1 = "",
            string cmtCd2 = "", string cmtOpt2 = "",
            string cmtCd3 = "", string cmtOpt3 = "",
            int isNodspRece = -1, int isNodspPaperRece = -1, int isNodspRyosyu = -1,
            int isDeleted = 0, string baseItemCd = "", int baseSeqNo = 0)
        {
            WrkSinKouiDetailModel wrkSinKouiDetailModel = new WrkSinKouiDetailModel(new WrkSinKouiDetail());
            wrkSinKouiDetailModel.HpId = _hpId;
            wrkSinKouiDetailModel.PtId = _ptId;
            wrkSinKouiDetailModel.SinDate = _sinDate;
            wrkSinKouiDetailModel.RaiinNo = _raiinNo;
            wrkSinKouiDetailModel.OyaRaiinNo = _oyaRaiinNo;
            wrkSinKouiDetailModel.SinStartTime = SinStartTime;
            wrkSinKouiDetailModel.HokenKbn = _hokenKbn;
            wrkSinKouiDetailModel.Suryo = suryo;
            wrkSinKouiDetailModel.Suryo2 = suryo2;
            wrkSinKouiDetailModel.FmtKbn = fmtKbn;
            wrkSinKouiDetailModel.ItemSbt = 0;
            wrkSinKouiDetailModel.IsNodspRece =
                isNodspRece < 0 ? 0 : isNodspRece;
            wrkSinKouiDetailModel.IsNodspPaperRece =
                isNodspPaperRece < 0 ? 0 : isNodspPaperRece;
            wrkSinKouiDetailModel.IsNodspRyosyu =
                isNodspRyosyu < 0 ? 0 : isNodspRyosyu;

            wrkSinKouiDetailModel.TenId = 0;
            wrkSinKouiDetailModel.Ten = 0;
            wrkSinKouiDetailModel.CdKbn = "";
            wrkSinKouiDetailModel.CdKbnno = 0;
            wrkSinKouiDetailModel.CdEdano = 0;
            wrkSinKouiDetailModel.CdKouno = 0;

            if (itemCd == "")
            {
                itemCd = ItemCdConst.CommentFree;
            }

            List<TenMstModel> tenMstModel = _tenMstCommon.GetTenMst(itemCd);

            wrkSinKouiDetailModel.OdrItemCd = itemCd;

            if (tenMstModel.Any())
            {
                wrkSinKouiDetailModel.TenMst = tenMstModel.First();

                wrkSinKouiDetailModel.ItemCd = itemCd;

                if (itemName == "")
                {
                    wrkSinKouiDetailModel.ItemName = tenMstModel[0].ReceName;
                }
                else
                {
                    wrkSinKouiDetailModel.ItemName = itemName;
                }
                wrkSinKouiDetailModel.UnitCd = CIUtil.StrToIntDef(wrkSinKouiDetailModel.TenMst.ReceUnitCd, 0);
                wrkSinKouiDetailModel.UnitName = wrkSinKouiDetailModel.TenMst.ReceUnitName;
                wrkSinKouiDetailModel.TenId = wrkSinKouiDetailModel.TenMst.TenId;
                wrkSinKouiDetailModel.Ten = wrkSinKouiDetailModel.TenMst.Ten;
                wrkSinKouiDetailModel.CdKbn = wrkSinKouiDetailModel.TenMst.CdKbn;
                wrkSinKouiDetailModel.CdKbnno = wrkSinKouiDetailModel.TenMst.CdKbnno;
                wrkSinKouiDetailModel.CdEdano = wrkSinKouiDetailModel.TenMst.CdEdano;
                wrkSinKouiDetailModel.CdKouno = wrkSinKouiDetailModel.TenMst.CdKouno;
                wrkSinKouiDetailModel.Kokuji1 = wrkSinKouiDetailModel.TenMst.Kokuji1;
                wrkSinKouiDetailModel.Kokuji2 = wrkSinKouiDetailModel.TenMst.Kokuji2;
                wrkSinKouiDetailModel.TyuCd = wrkSinKouiDetailModel.TenMst.TyuCd;
                if (wrkSinKouiDetailModel.IsZeroTyuCd)
                {
                    wrkSinKouiDetailModel.TyuCd = "9999";
                }
                wrkSinKouiDetailModel.TyuSeq = wrkSinKouiDetailModel.TenMst.TyuSeq;
                wrkSinKouiDetailModel.IsNodspRece =
                    (isNodspRece < 0 ? wrkSinKouiDetailModel.TenMst.IsNodspRece : isNodspRece);
                wrkSinKouiDetailModel.IsNodspPaperRece =
                    (isNodspPaperRece < 0 ? wrkSinKouiDetailModel.TenMst.IsNodspPaperRece : isNodspPaperRece);
                wrkSinKouiDetailModel.IsNodspRyosyu =
                    (isNodspRyosyu < 0 ? wrkSinKouiDetailModel.TenMst.IsNodspRyosyu : isNodspRyosyu);
                if (wrkSinKouiDetailModel.TenMst.MasterSbt == "C" || wrkSinKouiDetailModel.TenMst.MasterSbt == "D")
                {
                    wrkSinKouiDetailModel.ItemSbt = 1;
                }

                wrkSinKouiDetailModel.RecId = MstSbtToRecId(wrkSinKouiDetailModel.TenMst.MasterSbt, wrkSinKouiDetailModel.TenMst.SinKouiKbn, wrkSinKouiDetailModel.ItemCd);
            }

            if (itemSbt >= 0)
            {
                wrkSinKouiDetailModel.ItemSbt = itemSbt;
            }

            wrkSinKouiDetailModel.ItemSeqNo = 0;
            wrkSinKouiDetailModel.ItemEdaNo = 0;

            wrkSinKouiDetailModel.IsAutoAdd = autoAdd;   //自動追加
            wrkSinKouiDetailModel.CmtOpt = cmtOpt;
            wrkSinKouiDetailModel.CmtCd1 = cmtCd1;
            wrkSinKouiDetailModel.CmtOpt1 = cmtOpt1;
            wrkSinKouiDetailModel.CmtCd2 = cmtCd2;
            wrkSinKouiDetailModel.CmtOpt2 = cmtOpt2;
            wrkSinKouiDetailModel.CmtCd3 = cmtCd3;
            wrkSinKouiDetailModel.CmtOpt3 = cmtOpt3;

            wrkSinKouiDetailModel.IsDeleted = isDeleted;
            wrkSinKouiDetailModel.BaseItemCd = baseItemCd;
            wrkSinKouiDetailModel.BaseSeqNo = baseSeqNo;

            return wrkSinKouiDetailModel;
        }

        /// <summary>
        /// ワーク診療行為詳細の1レコード分のデータを返す
        /// オーダー詳細の内容を元に生成
        /// </summary>
        /// <param name="odrDtl">オーダー詳細</param>
        /// <param name="itemCd">診療行為コード</param>
        /// <param name="autoAdd">1:自動追加</param>
        /// <param name="suryo2">数量２</param>
        /// <param name="fmtKbn">書式区分</param>
        /// <param name="itemSbt">項目種別</param>
        /// <returns>ワーク診療行為詳細の1レコード分のデータ</returns>
        public WrkSinKouiDetailModel GetWrkSinKouiDetail
            (OdrDtlTenModel odrDtl, List<OdrInfCmtModel> odrCmts, int autoAdd = 0, double suryo2 = 0, int fmtKbn = 0, int itemSbt = -1,
            int isNodspRece = -1, int isNodspPaperRece = -1, int isNodspRyosyu = -1, int isDeleted = 0, string baseItemCd = "", int baseSeqNo = 0)
        {
            WrkSinKouiDetailModel wrkSinKouiDetailModel = new WrkSinKouiDetailModel(new WrkSinKouiDetail());

            if (odrDtl.TenMst != null)
            {
                //wrkSinKouiDetailModel.TenMst = new TenMstModel(odrDtl.TenMst);
                wrkSinKouiDetailModel.TenMst = odrDtl.TenMst;
            }

            wrkSinKouiDetailModel.HpId = _hpId;
            wrkSinKouiDetailModel.PtId = _ptId;
            wrkSinKouiDetailModel.SinDate = _sinDate;
            wrkSinKouiDetailModel.RaiinNo = _raiinNo;
            wrkSinKouiDetailModel.OyaRaiinNo = _oyaRaiinNo;
            wrkSinKouiDetailModel.SinStartTime = SinStartTime;
            wrkSinKouiDetailModel.HokenKbn = _hokenKbn;
            wrkSinKouiDetailModel.Suryo = odrDtl.CalcedSuryo;
            wrkSinKouiDetailModel.Suryo2 = suryo2;
            wrkSinKouiDetailModel.FmtKbn = fmtKbn;
            wrkSinKouiDetailModel.IsDummy = odrDtl.IsDummy;

            if (itemSbt < 0)
            {
                if (odrDtl.IsComment)
                {
                    // コメントの場合
                    wrkSinKouiDetailModel.ItemSbt = 1;
                }
                else
                {
                    wrkSinKouiDetailModel.ItemSbt = 0;
                }
            }
            else
            {
                wrkSinKouiDetailModel.ItemSbt = itemSbt;
            }

            wrkSinKouiDetailModel.OdrItemCd = odrDtl.OdrItemCd;

            if (odrDtl.ItemCd != "" && odrDtl.SanteiItemCd != "")
            {
                //bool changeFree = false;
                //if(odrDtl.ItemCd.StartsWith("W"))
                //{
                //    // 特処コメントはフリーコメントに置き換える
                //    wrkSinKouiDetailModel.ItemCd = ItemCdConst.CommentFree;
                //    changeFree = true;
                //}
                //else 
                if (odrDtl.OdrKouiKbn != OdrKouiKbnConst.Jihi && odrDtl.SanteiItemCd == ItemCdConst.NoSantei)
                {
                    // 自費項目以外で算定診療行為コードが9999999999

                    //if (odrDtl.IsNodspRece == 1 || (odrDtl.TenMst != null && odrDtl.TenMst.IsNodspRece == 1))
                    //{
                    //    // レセプトに表示しない項目の場合は、オーダー時のままの項目コード
                    wrkSinKouiDetailModel.ItemCd = odrDtl.OdrItemCd;
                    //}
                    //else
                    //{
                    //    // レセプトに表示する必要がある場合、フリーコメントに置き換える
                    //    wrkSinKouiDetailModel.ItemCd = ItemCdConst.CommentFree;
                    //    changeFree = true;
                    //}
                }
                else if (odrDtl.JihiSbt > 0 && odrDtl.SanteiItemCd == ItemCdConst.NoSantei)
                {
                    // 自費項目の場合、オーダー時のままの項目コード
                    wrkSinKouiDetailModel.ItemCd = odrDtl.OdrItemCd;
                }
                else if (odrDtl.MasterSbt == "S" && odrDtl.SyukeiSaki == "A18" && odrDtl.OdrItemCd.StartsWith("Z"))
                {
                    // 労災レセプト点数欄記載用ダミー項目の場合、オーダー時のままの項目コード
                    wrkSinKouiDetailModel.ItemCd = odrDtl.OdrItemCd;
                }
                else
                {
                    // 通常は算定診療行為コードを設定する
                    wrkSinKouiDetailModel.ItemCd = odrDtl.SanteiItemCd;
                }

                //if (changeFree)
                //{
                //    // フリーコメントに変換した場合、通常のフリーコメント処理と異なり、レセコメントを優先して設定
                //    wrkSinKouiDetailModel.ItemName = odrDtl.ReceName != "" ? odrDtl.ReceName : odrDtl.ItemName;
                //    wrkSinKouiDetailModel.CmtOpt = wrkSinKouiDetailModel.ItemName;
                //}
                //else 
                if (odrDtl.MasterSbt == "C" || odrDtl.MasterSbt == "D")
                {
                    //コメント項目の場合

                    string cmt = "";
                    string cmtOpt = odrDtl.CmtOpt;

                    if (odrDtl.ItemCd.StartsWith("852"))
                    {
                        // パターン852で5桁未満の場合、前ゼロ5桁にそろえる
                        if (cmtOpt.Length < 5)
                        {
                            cmtOpt = cmtOpt.PadLeft(5, '０');
                        }
                    }

                    if (odrDtl.ItemCd == ItemCdConst.GazoDensibaitaiHozon && cmtOpt == "")
                    {
                        // 電子媒体保存撮影
                        cmtOpt = wrkSinKouiDetailModel.Suryo.ToString();
                    }
                    cmt = _tenMstCommon.GetCommentStr(odrDtl.ItemCd, ref cmtOpt);
                    wrkSinKouiDetailModel.ItemName = cmt;
                    wrkSinKouiDetailModel.CmtOpt = cmtOpt;
                }
                else if (odrDtl.IsTItem)
                {
                    // 特材の場合、オーダー時の名称を使用
                    wrkSinKouiDetailModel.ItemName = odrDtl.ReceName.Trim();
                    if (string.IsNullOrEmpty(wrkSinKouiDetailModel.ItemName))
                    {
                        wrkSinKouiDetailModel.ItemName = odrDtl.ItemName;
                    }
                }
                //else if ( wrkSinKouiDetailModel.ItemCd == ItemCdConst.CommentFree)
                //{
                //    // フリーコメントの場合（MasterSbtがコメント以外、通常はchangeFree=trueになっているはずだが念のため）
                //    wrkSinKouiDetailModel.ItemName = odrDtl.ItemName;
                //    wrkSinKouiDetailModel.CmtOpt = odrDtl.ItemName;
                //}
                else
                {
                    // 通常はレセ名称を設定する
                    wrkSinKouiDetailModel.ItemName = odrDtl.ReceName;
                    wrkSinKouiDetailModel.CmtOpt = odrDtl.CmtOpt;
                }
            }
            else
            {
                //フリーコメント
                wrkSinKouiDetailModel.ItemCd = ItemCdConst.CommentFree;
                wrkSinKouiDetailModel.ItemName = odrDtl.ItemName;
                wrkSinKouiDetailModel.CmtOpt = odrDtl.ItemName;
            }

            if (odrDtl.MasterSbt == "C" || odrDtl.MasterSbt == "D" || wrkSinKouiDetailModel.ItemCd == ItemCdConst.CommentFree)
            {
                //コメント項目の場合、項目識別を1に設定
                wrkSinKouiDetailModel.ItemSbt = 1;
            }

            wrkSinKouiDetailModel.RecId = MstSbtToRecId(odrDtl.MasterSbt, odrDtl.SinKouiKbn, wrkSinKouiDetailModel.ItemCd);

            wrkSinKouiDetailModel.UnitCd = CIUtil.StrToIntDef(odrDtl.ReceUnitCd, 0);
            wrkSinKouiDetailModel.UnitName = odrDtl.ReceUnitName;
            wrkSinKouiDetailModel.TenId = odrDtl.TenId;
            wrkSinKouiDetailModel.Ten = odrDtl.Ten;
            wrkSinKouiDetailModel.CdKbn = odrDtl.CdKbn;
            wrkSinKouiDetailModel.CdKbnno = odrDtl.CdKbnno;
            wrkSinKouiDetailModel.CdEdano = odrDtl.CdEdano;
            wrkSinKouiDetailModel.CdKouno = odrDtl.CdKouno;

            wrkSinKouiDetailModel.Kokuji1 = odrDtl.Kokuji1;
            wrkSinKouiDetailModel.Kokuji2 = odrDtl.Kokuji2;
            wrkSinKouiDetailModel.TyuCd = odrDtl.TyuCd;
            if (wrkSinKouiDetailModel.IsZeroTyuCd)
            {
                wrkSinKouiDetailModel.TyuCd = "9999";
            }
            wrkSinKouiDetailModel.TyuSeq = odrDtl.TyuSeq;

            wrkSinKouiDetailModel.IsNodspRece = 0;
            if (odrDtl.IsComment)
            {
                // オーダーからレセ非表示設定を受け継ぐのはコメントのみ
                wrkSinKouiDetailModel.IsNodspRece = odrDtl.IsNodspRece;
            }

            if ((odrDtl.IsComment == false && odrDtl.TenMst == null) || (odrDtl.TenMst != null && odrDtl.TenMst.IsNodspRece == 1))
            {
                // マスタにない項目の場合レセ非表示
                wrkSinKouiDetailModel.IsNodspRece = 1;
            }

            if (isNodspRece >= 0)
            {
                wrkSinKouiDetailModel.IsNodspRece = isNodspRece;
            }

            wrkSinKouiDetailModel.IsNodspPaperRece = odrDtl.IsNodspPaperRece;
            if (isNodspPaperRece >= 0)
            {
                wrkSinKouiDetailModel.IsNodspPaperRece = isNodspPaperRece;
            }

            wrkSinKouiDetailModel.IsNodspRyosyu = odrDtl.IsNodspRyosyu;
            if (isNodspRyosyu >= 0)
            {
                wrkSinKouiDetailModel.IsNodspRyosyu = isNodspRyosyu;
            }

            wrkSinKouiDetailModel.IsAutoAdd = autoAdd;   //自動追加

            wrkSinKouiDetailModel.CmtCd1 = "";
            wrkSinKouiDetailModel.CmtOpt1 = "";
            wrkSinKouiDetailModel.CmtCd2 = "";
            wrkSinKouiDetailModel.CmtOpt2 = "";
            wrkSinKouiDetailModel.CmtCd3 = "";
            wrkSinKouiDetailModel.CmtOpt3 = "";

            wrkSinKouiDetailModel.IsDeleted = isDeleted;

            wrkSinKouiDetailModel.BaseItemCd = baseItemCd;
            wrkSinKouiDetailModel.BaseSeqNo = baseSeqNo;

            // コメント処理
            string retCmtOpt = "";
            string retCmt = "";
            //List<OdrInfCmtModel> odrCmts = FilterOdrInfComment(odrDtl.RpNo, odrDtl.RpEdaNo, odrDtl.RowNo);

            foreach (OdrInfCmtModel odrCmt in odrCmts.OrderBy(p => p.SortNo))
            {
                retCmtOpt = odrCmt.CmtOpt;
                retCmt = _tenMstCommon.GetCommentStr(odrCmt.CmtCd, ref retCmtOpt);

                wrkSinKouiDetailModel.AddComment(retCmt, odrCmt.CmtCd, retCmtOpt);
            }

            return wrkSinKouiDetailModel;
        }

        /// <summary>
        /// マスタ種別からレコード識別を取得する
        /// </summary>
        /// <param name="masterSbt">マスタ種別</param>
        /// <param name="sinKouiKbn">診療行為区分</param>
        /// <returns>レコード識別</returns>
        private string MstSbtToRecId(string masterSbt, int sinKouiKbn, string itemCd)
        {
            string ret = "";

            if (itemCd == ItemCdConst.CommentFree)
            {
                ret = ReceRecId.Comment;
            }
            else if (masterSbt == "S" || masterSbt == "R")
            {
                ret = ReceRecId.Sinryo;
            }
            else if (masterSbt == "Y")
            {
                ret = ReceRecId.Yakuzai;
            }
            else if (masterSbt == "T" || masterSbt == "U")
            {
                ret = ReceRecId.Tokutei;
            }
            else if (sinKouiKbn == 96)
            {
                ret = ReceRecId.Jihi;
            }
            else
            {
                ret = ReceRecId.Comment;
            }

            return ret;
        }

        /// <summary>
        /// ワーク診療行為詳細（コメント）を生成する
        /// </summary>
        /// <param name="itemCd">診療行為コード</param>
        /// <param name="cmtOpt">コメント分</param>
        /// <param name="autoAdd">自動発生項目　1:自動発生項目</param>
        /// <param name="isNodspRece">
        /// レセプト非表示区分
        /// 1:非表示
        /// 2:電算のみ非表示
        /// </param>
        /// <param name="isNodspPaperRece">紙レセ非表示区分　1:非表示</param>
        /// <param name="isNodspRyosyu">領収証非表示区分　1:非表示</param>
        public WrkSinKouiDetailModel GetWrkSinKouiDetailCommentRecord
            (string itemCd, string cmtOpt, int autoAdd = 0, int fmtKbn = 0,
             int isNodspRece = -1, int isNodspPaperRece = -1, int isNodspRyosyu = -1,
             string baseItemCd = "", int baseSeqNo = 0)
        {
            string retCmt = "";
            string retCmtOpt = "";

            retCmtOpt = cmtOpt;

            retCmt = _tenMstCommon.GetCommentStr(itemCd, ref retCmtOpt);

            return GetWrkSinKouiDetail
                     (itemCd, autoAdd, itemName: retCmt, cmtOpt: retCmtOpt, fmtKbn: fmtKbn, itemSbt: 1,
                      isNodspRece: isNodspRece, isNodspPaperRece: isNodspPaperRece, isNodspRyosyu: isNodspRyosyu,
                      baseItemCd: baseItemCd, baseSeqNo: baseSeqNo);
        }

        /// <summary>
        /// ワーク診療行為詳細を追加する
        /// ROW_NOは自動インクリメント
        /// </summary>
        /// <param name="wrkSinKouiDetailModel">ワーク診療行為詳細</param>
        public void AppendWrkSinKouiDetail(WrkSinKouiDetailModel wrkSinKouiDetailModel)
        {
            _wrkRowNo++;

            wrkSinKouiDetailModel.RpNo = _wrkRpNo;
            wrkSinKouiDetailModel.SeqNo = _wrkSeqNo;
            wrkSinKouiDetailModel.RowNo = _wrkRowNo;

            if (wrkSinKouiDetailModel.ItemSbt == 0)
            {
                if (_itemSeqNo > 0 && _kokuji1 == "")
                {
                    // _itemSeqNo > 0 ・・・Rp先頭の項目ではない
                    // _kokuji1 == "" ・・・この項目より前に診療行為項目はない
                    // この場合、前にコメントがあるような状況なので、_itemSeqNoは既に採番済みのはず
                    _itemEdaNo++;
                }
                else
                {
                    // 診療行為項目が来た場合、枝番は1に戻す
                    _itemSeqNo++;
                    _itemEdaNo = 1;
                }
            }
            else
            {
                if (_itemSeqNo == 0)
                {
                    // この項目より前に診療行為項目はない→この後の診療行為項目に付随するコメントとみなす
                    _itemSeqNo++;
                }
                _itemEdaNo++;
            }


            if (wrkSinKouiDetailModel.ItemSbt == 0 && (wrkSinKouiDetailModel.TenMst?.BuiKbn ?? 0) == 0)
            {
                // コメントでも部位でもない場合

                //if (_itemSeqNo > 0 && _kokuji1 == "")
                //{
                //    // _itemSeqNo > 0 ・・・Rp先頭の項目ではない
                //    // _kokuji1 == "" ・・・この項目より前に診療行為項目はない
                //    // この場合、前にコメントがあるような状況なので、_itemSeqNoは既に採番済みのはず
                //    _itemEdaNo++;
                //}
                //else
                //{
                //    // 診療行為項目が来た場合、枝番は1に戻す
                //    _itemSeqNo++;
                //    _itemEdaNo = 1;
                //}

                //コメント以外の場合、キーを記憶
                _kokuji1 = wrkSinKouiDetailModel.Kokuji1;
                _kokuji2 = wrkSinKouiDetailModel.Kokuji2;
                _tyuCd = wrkSinKouiDetailModel.TyuCd;
                _tyuSeq = wrkSinKouiDetailModel.TyuSeq;
                _tusokuAge = wrkSinKouiDetailModel.TusokuAge;

                // 上の項目チェック
                for (int i = _wrkSinKouiDetails.Count - 1; i >= 0; i--)
                {
                    // 上の項目がコメントか部位で、kokuji1が未設定なら更新
                    if (_wrkSinKouiDetails[i].RpNo == _wrkRpNo &&
                       _wrkSinKouiDetails[i].SeqNo == _wrkSeqNo &&
                       (_wrkSinKouiDetails[i].ItemSbt == 1 || (_wrkSinKouiDetails[i].TenMst?.BuiKbn ?? 0) > 0) &&
                       _wrkSinKouiDetails[i].Kokuji1 == "")
                    {
                        _wrkSinKouiDetails[i].Kokuji1 = _kokuji1;
                        _wrkSinKouiDetails[i].Kokuji2 = _kokuji2;
                        _wrkSinKouiDetails[i].TyuCd = _tyuCd;
                        _wrkSinKouiDetails[i].TyuSeq = _tyuSeq;
                        _wrkSinKouiDetails[i].TusokuAge = _tusokuAge;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                //コメントの場合、直前のキーを引き継ぐ
                wrkSinKouiDetailModel.Kokuji1 = _kokuji1;
                wrkSinKouiDetailModel.Kokuji2 = _kokuji2;
                wrkSinKouiDetailModel.TyuCd = _tyuCd;
                wrkSinKouiDetailModel.TyuSeq = _tyuSeq;
                wrkSinKouiDetailModel.TusokuAge = _tusokuAge;

                //if (_itemSeqNo == 0)
                //{
                //    // この項目より前に診療行為項目はない→この後の診療行為項目に付随するコメントとみなす
                //    _itemSeqNo++;
                //}
                //_itemEdaNo++;
            }

            wrkSinKouiDetailModel.ItemSeqNo = _itemSeqNo;
            wrkSinKouiDetailModel.ItemEdaNo = _itemEdaNo;

            _wrkSinKouiDetails.Add(wrkSinKouiDetailModel);

        }

        /// <summary>
        /// ワーク診療行為詳細を指定のRpに追加する
        /// </summary>
        /// <param name="rpNo">追加先RpNo</param>
        /// <param name="seqNo">追加先SeqNo</param>
        /// <param name="wrkSinKouiDetailModel">追加するワーク診療行為詳細</param>
        public void InsertWrkSinKouiDetail(int rpNo, int seqNo, WrkSinKouiDetailModel wrkSinKouiDetailModel)
        {
            wrkSinKouiDetailModel.RpNo = rpNo;
            wrkSinKouiDetailModel.SeqNo = seqNo;

            int maxRowNo = 0;
            if (_wrkSinKouiDetails.Any(p =>
                     p.RaiinNo == _raiinNo &&
                     p.HokenKbn == _hokenKbn &&
                     p.RpNo == rpNo &&
                     p.SeqNo == seqNo))
            {
                maxRowNo =
                    _wrkSinKouiDetails.Where(p =>
                        p.RaiinNo == _raiinNo &&
                        p.HokenKbn == _hokenKbn &&
                        p.RpNo == rpNo &&
                        p.SeqNo == seqNo)?.Max(p => p.RowNo) ?? 0;
            }

            wrkSinKouiDetailModel.RowNo = maxRowNo + 1;

            wrkSinKouiDetailModel.ItemSeqNo =
            _wrkSinKouiDetails.Where(p =>
                p.RpNo == rpNo &&
                p.SeqNo == seqNo)?.Max(p => p.ItemSeqNo) + 1 ?? 1;
            if (wrkSinKouiDetailModel.ItemSbt == 0)
            {

            }
            else
            {
                List<WrkSinKouiDetailModel> maxWrkDtls =
                    _wrkSinKouiDetails.FindAll(p =>
                        p.RaiinNo == _raiinNo &&
                        p.HokenKbn == _hokenKbn &&
                        p.IsDeleted == DeleteStatus.None &&
                        p.RpNo == rpNo &&
                        p.SeqNo == seqNo &&
                        p.RowNo <= maxRowNo &&
                        p.ItemSbt == 0)
                        .OrderByDescending(p => p.RowNo)
                        .ToList();
                if (maxWrkDtls != null && maxWrkDtls.Any())
                {
                    wrkSinKouiDetailModel.Kokuji1 = maxWrkDtls.First().Kokuji1;
                    wrkSinKouiDetailModel.Kokuji2 = maxWrkDtls.First().Kokuji2;
                    wrkSinKouiDetailModel.TyuCd = maxWrkDtls.First().TyuCd;
                    if (wrkSinKouiDetailModel.IsZeroTyuCd)
                    {
                        wrkSinKouiDetailModel.TyuCd = "9999";
                    }

                    wrkSinKouiDetailModel.TyuSeq = maxWrkDtls.First().TyuSeq;
                }
            }


            _wrkSinKouiDetails.Add(wrkSinKouiDetailModel);

        }

        /// <summary>
        /// ワーク診療行為詳細を生成し、追加する
        /// 自動発生項目用
        /// </summary>
        /// <param name="itemCd">診療行為コード</param>
        /// <param name="autoAdd">1:自動追加</param>
        /// <param name="suryo">数量</param>
        /// <param name="suryo2">数量2</param>
        /// <param name="fmtKbn">書式区分</param>
        /// <param name="itemSbt">項目種別</param>
        /// <param name="cmtOpt">コメント文</param>
        /// <param name="cmtCd1">コメントコード１</param>
        /// <param name="cmtOpt1">コメント文１</param>
        /// <param name="cmtCd2">コメントコード２</param>
        /// <param name="cmtOpt2">コメント文２</param>
        /// <param name="cmtCd3">コメントコード３</param>
        /// <param name="cmtOpt3">コメント文３</param>
        /// <param name="isNodspRece">
        /// レセプト非表示区分
        /// 1:非表示
        /// 2:電算のみ非表示
        /// </param>
        /// <param name="isNodspPaperRece">紙レセ非表示区分　1:非表示</param>
        /// <param name="isNodspRyosyu">領収証非表示区分　1:非表示</param>
        public void AppendNewWrkSinKouiDetail
            (string itemCd, int autoAdd = 0, string itemName = "", double suryo = 1, double suryo2 = 0, int fmtKbn = 0, int itemSbt = -1, string cmtOpt = "",
            string cmtCd1 = "", string cmtOpt1 = "",
            string cmtCd2 = "", string cmtOpt2 = "",
            string cmtCd3 = "", string cmtOpt3 = "",
            int isNodspRece = -1, int isNodspPaperRece = -1, int isNodspRyosyu = -1,
            int isDeleted = 0, string baseItemCd = "", int baseSeqNo = 0)
        {
            AppendWrkSinKouiDetail(
                GetWrkSinKouiDetail
                    (itemCd, autoAdd: autoAdd, itemName: itemName,
                     suryo: suryo, suryo2: suryo2, fmtKbn: fmtKbn, itemSbt: itemSbt,
                     cmtOpt: cmtOpt,
                     cmtCd1: cmtCd1, cmtOpt1: cmtOpt1,
                     cmtCd2: cmtCd2, cmtOpt2: cmtOpt2,
                     cmtCd3: cmtCd3, cmtOpt3: cmtOpt3,
                     isNodspRece: isNodspRece, isNodspPaperRece: isNodspPaperRece, isNodspRyosyu: isNodspRyosyu,
                     isDeleted: isDeleted, baseItemCd: baseItemCd, baseSeqNo: baseSeqNo));
        }

        public void InsertNewWrkSinKouiDetail
            (int rpNo, int seqNo, string itemCd, int autoAdd = 0, string itemName = "", double suryo = 1, double suryo2 = 0, int fmtKbn = 0, int itemSbt = -1, string cmtOpt = "",
            string cmtCd1 = "", string cmtOpt1 = "",
            string cmtCd2 = "", string cmtOpt2 = "",
            string cmtCd3 = "", string cmtOpt3 = "",
            int isNodspRece = -1, int isNodspPaperRece = -1, int isNodspRyosyu = -1,
            int isDeleted = 0)
        {
            InsertWrkSinKouiDetail(
                rpNo,
                seqNo,
                GetWrkSinKouiDetail
                    (itemCd, autoAdd: autoAdd, itemName: itemName,
                     suryo: suryo, suryo2: suryo2, fmtKbn: fmtKbn, itemSbt: itemSbt,
                     cmtOpt: cmtOpt,
                     cmtCd1: cmtCd1, cmtOpt1: cmtOpt1,
                     cmtCd2: cmtCd2, cmtOpt2: cmtOpt2,
                     cmtCd3: cmtCd3, cmtOpt3: cmtOpt3,
                     isNodspRece: isNodspRece, isNodspPaperRece: isNodspPaperRece, isNodspRyosyu: isNodspRyosyu,
                     isDeleted: isDeleted));
        }

        /// <summary>
        /// ワーク診療行為詳細を生成し、追加する
        /// オーダー詳細の内容を元に生成
        /// </summary>
        /// <param name="odrDtl">オーダー詳細</param>
        /// <param name="itemCd">診療行為コード</param>
        /// <param name="autoAdd">1:自動追加</param>
        /// <param name="suryo2">数量２</param>
        /// <param name="fmtKbn">書式区分</param>
        /// <param name="itemSbt">項目種別</param>
        public void AppendNewWrkSinKouiDetail(
            OdrDtlTenModel odrDtl, List<OdrInfCmtModel> odrCmts, int autoAdd = 0, double suryo2 = 0, int fmtKbn = 0, int itemSbt = -1, int isDeleted = 0,
            int isNodspRece = -1, int isNodspPaperRece = -1, int isNodspRyosyu = -1, string baseItemCd = "", int baseSeqNo = 0)
        {
            if (odrDtl.OdrItemCd.StartsWith("Z"))
            {
                //ユーザー設定特材
                fmtKbn = 20;
            }
            AppendWrkSinKouiDetail(
                GetWrkSinKouiDetail(
                    odrDtl: odrDtl, odrCmts: odrCmts, autoAdd: autoAdd, suryo2: suryo2, fmtKbn: fmtKbn, itemSbt: itemSbt,
                    isNodspRece: isNodspRece, isNodspPaperRece: isNodspPaperRece, isNodspRyosyu: isNodspRyosyu, isDeleted: isDeleted, baseItemCd: baseItemCd, baseSeqNo: baseSeqNo));
        }

        /// <summary>
        /// オーダー詳細から当該項目で算定可能な年齢加算項目を追加する
        /// </summary>
        /// <param name="odrDtl">オーダー詳細</param>
        public bool AppendNewWrkSinKouiDetailAgeKasan(OdrDtlTenModel odrDtl)
        {
            bool ret = false;

            string kasanCd = GetAgeKasanCd(odrDtl);

            if (kasanCd != "")
            {
                if (wrkSinKouiDetails.Any(p =>
                     p.RpNo == _wrkRpNo &&
                     p.SeqNo == _wrkSeqNo &&
                     p.ItemCd == kasanCd) == false)
                {
                    AppendNewWrkSinKouiDetail(kasanCd, 1);
                    ret = true;
                }
            }

            return ret;
        }

        /// <summary>
        /// オーダー詳細から当該項目で算定可能な年齢加算項目を追加する（手オーダーチェックつき）
        /// </summary>
        /// <param name="odrDtl"></param>
        /// <param name="odrDtls"></param>
        /// <returns></returns>
        public bool AppendNewWrkSinKouiDetailAgeKasan(OdrDtlTenModel odrDtl, List<OdrDtlTenModel> odrDtls)
        {
            bool ret = false;

            string kasanCd = GetAgeKasanCd(odrDtl);

            if (kasanCd != "")
            {
                if (odrDtls.Any(p =>
                     p.RpNo == odrDtl.RpNo &&
                     p.RpEdaNo == odrDtl.RpEdaNo &&
                     p.ItemCd == kasanCd) == false)
                {
                    AppendNewWrkSinKouiDetail(kasanCd, 1);
                    ret = true;
                }
            }

            return ret;
        }

        // 設定されていても無視する加算項目のリスト
        List<string> ExcludeKasanItemCd =
            new List<string>
            {
                    ItemCdConst.SonotaTuuinSeisin20Kasan,   // 通院・在宅精神療法（２０歳未満）加算
                    ItemCdConst.SonotaJidoSisyunkiKasan16,  // 児童思春期精神科専門管理加算（１６歳未満）（２年以内）
                    ItemCdConst.SonotaJidoSisyunkiKasan16_Sonota,  // 児童思春期精神科専門管理加算（１６歳未満）（（１）以外）
                    ItemCdConst.SonotaJidoSisyunkiKasan20,  // 児童思春期精神科専門管理加算（２０歳未満）
                    ItemCdConst.SonotaSikkanbetu,           // 疾患別等専門プログラム加算（精神科ショート・ケア、小規模なもの）
                    ItemCdConst.GazoSyoniTinseiMRI          // 小児鎮静下ＭＲＩ撮影加算
            };

        public string GetAgeKasanCd(OdrDtlTenModel odrDtl)
        {
            string ret = "";

            for (int i = 1; i <= 4; i++)
            {
                if (IsValidAgeKasanConf(odrDtl.AgekasanMin(i), odrDtl.AgekasanMax(i), odrDtl.AgekasanCd(i)) == false)
                {
                    break;
                }
                else if (ExcludeKasanItemCd.Contains(odrDtl.AgekasanCd(i)))
                {
                    break;
                }
                else if (CheckAgeMin(odrDtl.AgekasanMin(i)) && CheckAgeMax(odrDtl.AgekasanMax(i)))
                {
                    ret = odrDtl.AgekasanCd(i);
                    break;
                }
            }

            return ret;
        }

        /// <summary>
        /// 診療行為コードから当該項目で算定可能な年齢加算項目を追加する
        /// </summary>
        /// <param name="itemCd">診療行為コード</param>
        public void AppendNewWrkSinKouiDetailAgeKasan(string itemCd)
        {
            TenMstModel tenMst = _tenMstCommon.GetTenMst(itemCd).FirstOrDefault();

            if (tenMst != null)
            {
                for (int i = 1; i <= 4; i++)
                {
                    if (IsValidAgeKasanConf(tenMst.AgekasanMin(i), tenMst.AgekasanMax(i), tenMst.AgekasanCd(i)) == false)
                    {
                        // 無効な設定
                        break;
                    }
                    else if (CheckAgeMin(tenMst.AgekasanMin(i)) && CheckAgeMax(tenMst.AgekasanMax(i)))
                    {
                        AppendNewWrkSinKouiDetail(tenMst.AgekasanCd(i), 1);
                        break;
                    }
                }
            }
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

            for (int i = 1; i <= 4; i++)
            {
                if (IsValidAgeKasanConf(odrDtl.AgekasanMin(i), odrDtl.AgekasanMax(i), odrDtl.AgekasanCd(i)) == false)
                {
                    break;
                }
                else if (CheckAgeMin(odrDtl.AgekasanMin(i)) && CheckAgeMax(odrDtl.AgekasanMax(i)))
                {
                    itemCd = odrDtl.AgekasanCd(i);

                    break;
                }
            }

            if (itemCd == "")
            {
                ///     　1: ３歳未満乳幼児加算（処置）（１１０点）が算定できる診療行為
                ///     　2: ３歳未満乳幼児加算（処置）（５５点）が算定できる診療行為
                ///     　3: ６歳未満乳幼児加算（処置）（１１０点）が算定できる診療行為
                ///     　4: ６歳未満乳幼児加算（処置）（８３点）が算定できる診療行為
                ///     　5: ６歳未満乳幼児加算（処置）（５５点）が算定できる診療行為
                itemCd = GetSyotiNyuyojiKasanItemCd(odrDtl.SyotiNyuyojiKbn);
            }

            if (itemCd != "")
            {
                AppendNewWrkSinKouiDetail(itemCd, 1);
                List<TenMstModel> tenMst = _tenMstCommon.GetTenMst(itemCd);
                if (tenMst.Any())
                {
                    retTen = tenMst.First().Ten;
                }
            }

            return retTen;
        }

        /// <summary>
        /// 処置乳幼児区分から該当する加算項目の診療行為コードを返す
        /// </summary>
        /// <param name="SyotiNyuyojiKbn"></param>
        /// <returns></returns>
        public string GetSyotiNyuyojiKasanItemCd(int SyotiNyuyojiKbn)
        {
            string itemCd = "";

            switch (SyotiNyuyojiKbn)
            {
                case 1:
                    if (_ptInf.IsNyuyoJi)
                    {
                        itemCd = ItemCdConst.SyotiNyuYojiKasan1;
                    }
                    break;
                case 2:
                    if (_ptInf.IsNyuyoJi)
                    {
                        itemCd = ItemCdConst.SyotiNyuYojiKasan2;
                    }
                    break;
                case 3:
                    if (_ptInf.IsYoJi)
                    {
                        itemCd = ItemCdConst.SyotiYojiKasan1;
                    }
                    break;
                case 4:
                    if (_ptInf.IsYoJi)
                    {
                        itemCd = ItemCdConst.SyotiYojiKasan2;
                    }
                    break;
                case 5:
                    if (_ptInf.IsYoJi)
                    {
                        itemCd = ItemCdConst.SyotiYojiKasan3;
                    }
                    break;
            }

            return itemCd;
        }

        /// <summary>
        /// 年齢加算の下限チェック
        /// </summary>
        /// <param name="AgekasanMin">年齢加算下限</param>
        /// <returns>true: 年齢が、年齢加算の下限以上</returns>
        private bool CheckAgeMin(string AgekasanMin)
        {
            bool ret = false;
            if (AgekasanMin == "AA")
            {
                ret = (_ptInf.Age > 0 || _ptInf.AgeMonth > 0 || _ptInf.AgeDay >= 28);
            }
            else if (AgekasanMin == "B3")
            {
                ret = ((_ptInf.Age >= 3 && _ptInf.AgeMonth > 0) || (_ptInf.Age == 3 && _ptInf.Birthday / 100 < _sinDate / 100));
            }
            else if (AgekasanMin == "BF")
            {
                ret = ((_ptInf.Age >= 15 && _ptInf.AgeMonth > 0) || (_ptInf.Age == 15 && _ptInf.Birthday / 100 < _sinDate / 100));
            }
            else if (AgekasanMin == "BK")
            {
                ret = ((_ptInf.Age >= 20 && _ptInf.AgeMonth > 0) || (_ptInf.Age == 20 && _ptInf.Birthday / 100 < _sinDate / 100));
            }
            else if (AgekasanMin == "B6")
            {
                ret = ((_ptInf.Age >= 6 && _ptInf.AgeMonth > 0) || (_ptInf.Age == 6 && _ptInf.Birthday / 100 < _sinDate / 100));
            }
            else if (AgekasanMin == "MG")
            {
                ret = CIUtil.IsStudent(_ptInf.Birthday, _sinDate);
            }
            else
            {
                ret = (CIUtil.StrToIntDef(AgekasanMin, 999) <= _ptInf.Age);
            }

            return ret;
        }

        /// <summary>
        /// 年齢加算の設定が有効かチェック
        /// </summary>
        /// <param name="AgeKasanVal"></param>
        /// <returns>true: 有効な設定</returns>
        public bool IsValidAgeKasanConf(string ageMin, string ageMax, string ageCd)
        {
            bool ret = true;

            if (ageCd == "0" || ageCd == "")
            {
                ret = false;
            }
            else if (new string[] { "AA", "B3", "BF", "BK", "B6", "MG" }.Contains(ageMin))
            {
                // 有効
            }
            else if (new string[] { "AA", "B3", "BF", "BK", "B6", "MG" }.Contains(ageMax))
            {
                // 有効
            }
            else if ((ageMin == "0" || ageMin == "") &&
                    (ageMax == "0" || ageMax == ""))
            {
                ret = false;
            }

            return ret;
        }

        /// <summary>
        /// 年齢加算の上限チェック
        /// </summary>
        /// <param name="AgekasanMax">年齢加算上限</param>
        /// <returns>true: 年齢が、年齢加算の上限未満</returns>
        private bool CheckAgeMax(string AgekasanMax)
        {
            bool ret = false;
            if (AgekasanMax == "AA")
            {
                ret = (_ptInf.Age == 0 && _ptInf.AgeMonth == 0 && _ptInf.AgeDay < 28);
            }
            else if (AgekasanMax == "B3")
            {
                ret = (_ptInf.Age < 3 || (_ptInf.Age == 3 && _ptInf.Birthday / 100 == _sinDate / 100));
            }
            else if (AgekasanMax == "BF")
            {
                ret = (_ptInf.Age < 15 || (_ptInf.Age == 15 && _ptInf.Birthday / 100 == _sinDate / 100));
            }
            else if (AgekasanMax == "BK")
            {
                ret = (_ptInf.Age < 20 || (_ptInf.Age == 20 && _ptInf.Birthday / 100 == _sinDate / 100));
            }
            else if (AgekasanMax == "B6")
            {
                ret = (_ptInf.Age < 6 || (_ptInf.Age == 6 && _ptInf.Birthday / 100 == _sinDate / 100));
            }
            else if (AgekasanMax == "MG")
            {
                ret = CIUtil.IsStudent(_ptInf.Birthday, _sinDate) == false;
            }
            else
            {
                ret = (CIUtil.StrToIntDef(AgekasanMax, 999) > _ptInf.Age);
            }

            return ret;
        }

        List<string> ignoreSyokaiSanteiItems =
            new List<string>
            {
                            ItemCdConst.IgakuSyouniCounseling1,
                            ItemCdConst.IgakuSyouniCounseling2,
                            ItemCdConst.IgakuNico2_4,
                            ItemCdConst.IgakuNico5, ItemCdConst.IgakuZensoku1_2
            };

        /// <summary>
        /// 体外受精・顕微授精管理料 
        /// </summary>
        List<string> taigaiKenbiJuseis =
            new List<string>
            {
                ItemCdConst.SyujyutuTaigaiJusei,
                ItemCdConst.SyujyutuKenbiJusei1,
                ItemCdConst.SyujyutuKenbiJusei2_5,
                ItemCdConst.SyujyutuKenbiJusei6_9,
                ItemCdConst.SyujyutuKenbiJusei10,
                ItemCdConst.SyujyutuJuseiDouji1,
                ItemCdConst.SyujyutuJuseiDouji2_5,
                ItemCdConst.SyujyutuJuseiDouji6_9,
                ItemCdConst.SyujyutuJuseiDouji10
            };

        /// <summary>
        /// 受精卵・胚培養管理料
        /// </summary>
        List<string> juseiranHaiBaiyos =
            new List<string>
            {
                ItemCdConst.SyujyutuJuseiranHaiBaiyo1,
                ItemCdConst.SyujyutuJuseiranHaiBaiyo2_5,
                ItemCdConst.SyujyutuJuseiranHaiBaiyo6_9,
                ItemCdConst.SyujyutuJuseiranHaiBaiyo10
            };

        /// <summary>
        /// 胚凍結保存管理料（胚凍結保存管理料（導入時））
        /// </summary>
        List<string> haiHozonDonyus =
            new List<string>
            {
                ItemCdConst.SyujyutuHaiHozonDonyu1,
                ItemCdConst.SyujyutuHaiHozonDonyu2_5,
                ItemCdConst.SyujyutuHaiHozonDonyu6_9,
                ItemCdConst.SyujyutuHaiHozonDonyu10
            };

        /// <summary>
        /// オーダー詳細から当該項目に設定されているコメントを追加する
        /// </summary>
        /// <param name="odrDtl"></param>
        public void AppendNewWrkSinKouiDetailComment(OdrDtlTenModel odrDtl, List<OdrDtlTenModel> odrDtls)
        {
            int retDate = 0;
            int syokaiDate = 0;
            string comment;
            bool addSanteiComment = false;  // 算定情報からコメント追加したかどうか。追加済みの場合、TEN_MSTの設定によるコメント追加は行わない

            bool _existsItemCd(string AitemCd)
            {
                return odrDtls.Any(p =>
                                     p.RpNo == odrDtl.RpNo &&
                                     p.RpEdaNo == odrDtl.RpEdaNo &&
                                     p.ItemCd == AitemCd);
            }

            if (_santeiInfDtls.Any(p => p.ItemCd == odrDtl.ItemCd))
            {
                // 算定情報の登録がある場合
                SanteiInfDetailModel syokaiFirst =
                    _santeiInfDtls.FindAll(p => p.ItemCd == odrDtl.ItemCd && p.KisanSbt == 1 && p.KisanDate <= _sinDate && p.EndDate >= _sinDate).OrderBy(p => p.KisanDate).FirstOrDefault();

                // 初回分は起算日が一番小さいものを採用する
                if (syokaiFirst != null)
                {
                    // 初回算定
                    if (ignoreSyokaiSanteiItems.Contains(odrDtl.ItemCd) ||
                        new int[] { 2, 3, 4, 5, 10 }.Contains(odrDtl.CmtKbn))
                    {
                        // 初回日 or 前回日を出力する指定のある項目は、後で処理する
                        // ここでは設定値を取得するだけにしておく
                        syokaiDate = syokaiFirst.KisanDate;
                    }
                    else if (_systemConfigProvider.GetCalcAutoComment() != 1 ||
                        (_systemConfigProvider.GetCalcAutoComment() == 1 && odrDtl.OdrKouiKbn >= 80 && odrDtl.OdrKouiKbn <= 89))
                    {
                        RecedenCmtSelectModel recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.Syokai);
                        
                        if (recedenCmtSelect == null)
                        {
                            recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.Hassyo);
                        }
                        if (recedenCmtSelect == null)
                        {
                            recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.ChiryoKaisi);
                        }
                        if (recedenCmtSelect == null)
                        {
                            recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.HassyoOrChiryo);
                        }

                        if (recedenCmtSelect != null)
                        {
                            string itemCd = recedenCmtSelect.CommentCd;
                            if (itemCd.StartsWith("820") == false ||
                                (itemCd.StartsWith("820") && syokaiFirst.KisanDate == _sinDate))
                            {
                                if (_existsItemCd(itemCd) == false)
                                {
                                    string cmtOpt = CIUtil.ToWide(CIUtil.SDateToWDate(syokaiFirst.KisanDate).ToString());
                                    AppendNewWrkSinKouiDetailCommentRecord(itemCd: itemCd, cmtOpt: cmtOpt, baseItemCd: odrDtl.ItemCd);
                                    addSanteiComment = true;
                                }
                            }
                        }
                        else
                        {
                            string itemCd = ItemCdConst.CommentFree;
                            string cmtOpt = "初回算定　" + CIUtil.ToWide(CIUtil.SDateToShowWDate2(syokaiFirst.KisanDate).Replace(" ", ""));

                            AppendNewWrkSinKouiDetailCommentRecord(itemCd: itemCd, cmtOpt: cmtOpt, baseItemCd: odrDtl.ItemCd);
                            addSanteiComment = true;
                        }
                    }

                    AppendByomeiComment(syokaiFirst, false);

                }

                if (_systemConfigProvider.GetCalcAutoComment() != 1 ||
                        (_systemConfigProvider.GetCalcAutoComment() == 1 && (odrDtl.OdrKouiKbn < 800 || odrDtl.OdrKouiKbn > 899)))
                {
                    // 初回以外は該当するものすべて出力
                    int kisanDate = -1;
                    int kisanSbt = -1;
                    bool bByomeiOnly = false;
                    foreach (SanteiInfDetailModel santeiInfDtl in
                        _santeiInfDtls.FindAll(p =>
                            p.ItemCd == odrDtl.ItemCd &&
                            p.KisanSbt != 1 &&
                            p.KisanDate <= _sinDate
                            && p.EndDate >= _sinDate)
                        .OrderByDescending(p => p.KisanDate).ThenBy(p => p.KisanSbt))
                    {
                        string itemCd = "";
                        string cmtOpt = "";
                        bool bAppend = false;

                        if (santeiInfDtl.KisanDate != kisanDate || santeiInfDtl.KisanSbt != kisanSbt)
                        {
                            cmtOpt = CIUtil.ToWide(CIUtil.SDateToShowWDate2(santeiInfDtl.KisanDate).Replace(" ", ""));
                            kisanDate = santeiInfDtl.KisanDate;
                            kisanSbt = santeiInfDtl.KisanSbt;


                            if (santeiInfDtl.KisanSbt == 2)
                            {
                                // 発症日
                                RecedenCmtSelectModel recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.Hassyo);
                                if (recedenCmtSelect == null)
                                {
                                    recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.HassyoOrChiryo);
                                }

                                if (recedenCmtSelect != null)
                                {
                                    itemCd = recedenCmtSelect.CommentCd;
                                    if (_existsItemCd(itemCd) == false)
                                    {
                                        cmtOpt = CIUtil.ToWide(CIUtil.SDateToWDate(santeiInfDtl.KisanDate).ToString());
                                    }
                                    else
                                    {
                                        itemCd = "";
                                    }
                                }
                                else
                                {
                                    //itemCd = ItemCdConst.CommentHassyo;
                                    itemCd = ItemCdConst.CommentFree;
                                    cmtOpt = "発症　" + cmtOpt;
                                }
                            }
                            else if (santeiInfDtl.KisanSbt == 3)
                            {
                                // 急性増悪
                                RecedenCmtSelectModel recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.KyuseiZouaku);

                                if (recedenCmtSelect != null)
                                {
                                    itemCd = recedenCmtSelect.CommentCd;
                                    if (_existsItemCd(itemCd) == false)
                                    {
                                        cmtOpt = CIUtil.ToWide(CIUtil.SDateToWDate(santeiInfDtl.KisanDate).ToString());
                                    }
                                    else
                                    {
                                        itemCd = "";
                                    }
                                }
                                else
                                {
                                    //itemCd = ItemCdConst.CommentKyuseizoaku;
                                    itemCd = ItemCdConst.CommentFree;
                                    cmtOpt = "急性増悪　" + cmtOpt;
                                }
                            }
                            else if (santeiInfDtl.KisanSbt == 4)
                            {
                                // 治療開始日
                                RecedenCmtSelectModel recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.ChiryoKaisi);
                                if (recedenCmtSelect == null)
                                {
                                    recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.HassyoOrChiryo);
                                }

                                if (recedenCmtSelect != null)
                                {
                                    itemCd = recedenCmtSelect.CommentCd;
                                    if (_existsItemCd(itemCd) == false)
                                    {
                                        cmtOpt = CIUtil.ToWide(CIUtil.SDateToWDate(santeiInfDtl.KisanDate).ToString());
                                    }
                                    else
                                    {
                                        itemCd = "";
                                    }
                                }
                                else
                                {
                                    //itemCd = ItemCdConst.CommentChiryo;
                                    itemCd = ItemCdConst.CommentFree;
                                    cmtOpt = "治療開始　" + cmtOpt;
                                }
                            }
                            else if (santeiInfDtl.KisanSbt == 5)
                            {
                                // 手術日
                                RecedenCmtSelectModel recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.Syujyutu);

                                if (recedenCmtSelect != null)
                                {
                                    itemCd = recedenCmtSelect.CommentCd;
                                    if (_existsItemCd(itemCd) == false)
                                    {
                                        cmtOpt = CIUtil.ToWide(CIUtil.SDateToWDate(santeiInfDtl.KisanDate).ToString());
                                    }
                                    else
                                    {
                                        itemCd = "";
                                    }
                                }
                                else
                                {
                                    itemCd = ItemCdConst.CommentFree;
                                    cmtOpt = "手術　" + cmtOpt;
                                }
                            }
                            else if (santeiInfDtl.KisanSbt == 6)
                            {
                                // 初回診断
                                RecedenCmtSelectModel recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.SyokaiSindan);

                                if (recedenCmtSelect != null)
                                {
                                    itemCd = recedenCmtSelect.CommentCd;
                                    if (_existsItemCd(itemCd) == false)
                                    {
                                        cmtOpt = CIUtil.ToWide(CIUtil.SDateToWDate(santeiInfDtl.KisanDate).ToString());
                                    }
                                    else
                                    {
                                        itemCd = "";
                                    }
                                }
                                else
                                {
                                    itemCd = ItemCdConst.CommentFree;
                                    cmtOpt = "初回診断　" + cmtOpt;
                                }
                            }

                            if (itemCd != "")
                            {
                                AppendNewWrkSinKouiDetailCommentRecord(itemCd: itemCd, cmtOpt: cmtOpt, baseItemCd: odrDtl.ItemCd);
                                addSanteiComment = true;
                            }
                        }
                        else
                        {
                            if (bByomeiOnly && santeiInfDtl.Byomei != "" && santeiInfDtl.Comment == "")
                            {
                                bAppend = true;
                            }
                        }

                        AppendByomeiComment(santeiInfDtl, bAppend);

                        if (santeiInfDtl.Byomei != "" && santeiInfDtl.Comment == "")
                        {
                            bByomeiOnly = true;
                        }

                    }
                }
            }

            if (odrDtl.ItemCd == ItemCdConst.IgakuSyouniCounseling1)
            {
                // 小児特定疾患カウンセリング料（１回目）の初回算定日を取得
                retDate = _santeiFinder.FindFirstSanteiDate(_hpId, _ptId, _sinDate, _raiinNo, ItemCdConst.IgakuSyouniCounseling1, _hokenKbn);

                if (retDate == 0 && odrDtl.ItemCd == ItemCdConst.IgakuSyouniCounseling1)
                {
                    // 過去に小児特定疾患カウンセリング料の算定が見当たらない場合、本日が初回と判断し、診療日をセット
                    retDate = _sinDate;
                }

                if (retDate > 0)
                {
                    RecedenCmtSelectModel recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.Syokai);

                    if (recedenCmtSelect != null)
                    {
                        if (_existsItemCd(recedenCmtSelect.CommentCd) == false)
                        {
                            AppendNewWrkSinKouiDetailCommentRecord(
                            itemCd: recedenCmtSelect.CommentCd,
                            cmtOpt: CIUtil.ToWide(CIUtil.SDateToWDate(retDate).ToString()),
                            autoAdd: 1,
                            baseItemCd: odrDtl.ItemCd);
                        }
                    }
                    else
                    {
                        string itemCd = ItemCdConst.CommentSyouniCounseling;

                        if (retDate >= 20190501)
                        {
                            // 令和
                            itemCd = ItemCdConst.CommentSyouniCounselingReiwa;
                        }
                        AppendNewWrkSinKouiDetailCommentRecord(
                            itemCd: itemCd,
                            cmtOpt: CIUtil.ToWide(CIUtil.SDateToWDate(retDate).ToString().Substring(1, 6)),
                            autoAdd: 1,
                            baseItemCd: odrDtl.ItemCd);
                    }
                }
            }
            else if (odrDtl.ItemCd == ItemCdConst.IgakuSyouniCounseling2 || odrDtl.ItemCd == ItemCdConst.IgakuSyouniCounselingKounin)
            {
                // 小児特定疾患カウンセリング料（２回目）または小児特定疾患カウンセリング料（公認心理師）の場合、小児特定疾患カウンセリング料（１回目）の初回算定日を取得
                retDate = _santeiFinder.FindFirstSanteiDate(_hpId, _ptId, _sinDate, _raiinNo, ItemCdConst.IgakuSyouniCounseling1, _hokenKbn);

                if (retDate > 0)
                {
                    RecedenCmtSelectModel recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.Syokai);

                    if (recedenCmtSelect != null)
                    {
                        if (_existsItemCd(recedenCmtSelect.CommentCd) == false)
                        {
                            AppendNewWrkSinKouiDetailCommentRecord(
                            itemCd: recedenCmtSelect.CommentCd,
                            cmtOpt: CIUtil.ToWide(CIUtil.SDateToWDate(retDate).ToString()),
                            autoAdd: 1,
                            baseItemCd: odrDtl.ItemCd);
                        }
                    }
                }
            }
            else if (new string[] { ItemCdConst.IgakuNico1, ItemCdConst.IgakuNico1Rinsyojyo }.Contains(odrDtl.ItemCd))
            {
                // ニコチン依存症管理料（初回）またはニコチン依存症管理料１（初回）（診療報酬上臨時的取扱）の場合、診療日を記載
                RecedenCmtSelectModel recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.Syokai);

                if (recedenCmtSelect != null)
                {
                    if (_existsItemCd(recedenCmtSelect.CommentCd) == false)
                    {
                        AppendNewWrkSinKouiDetailCommentRecord(recedenCmtSelect.CommentCd, CIUtil.ToWide(CIUtil.SDateToWDate(_sinDate).ToString()), autoAdd: 1);
                    }
                }
                else
                {
                    AppendNewWrkSinKouiDetailCommentRecord(ItemCdConst.CommentSyokaiSantei, CIUtil.ToWide(CIUtil.SDateToWDate(_sinDate).ToString().Substring(3, 4)), autoAdd: 1);
                }
            }
            else if (new string[] { ItemCdConst.IgakuNico2_4, ItemCdConst.IgakuNico5, ItemCdConst.IgakuNico5Rinsyojyo, ItemCdConst.IgakuNico2_4Tusin }.Contains(odrDtl.ItemCd))
            {
                // ニコチン依存症管理料　2回目以降、２~４回（情報通信機器）の場合、ニコチン依存症管理料（初回）の日付を取得
                retDate = _santeiFinder.FindLastSanteiDate(_hpId, _ptId, _sinDate, _sinDate, _raiinNo, new List<string> { ItemCdConst.IgakuNico1, ItemCdConst.IgakuNico1Rinsyojyo }, _hokenKbn);
                if (retDate > 0)
                {
                    RecedenCmtSelectModel recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.Syokai);

                    if (recedenCmtSelect != null)
                    {
                        if (_existsItemCd(recedenCmtSelect.CommentCd) == false)
                        {
                            AppendNewWrkSinKouiDetailCommentRecord(recedenCmtSelect.CommentCd, CIUtil.ToWide(CIUtil.SDateToWDate(retDate).ToString()), autoAdd: 1);
                        }
                    }
                    else
                    {
                        AppendNewWrkSinKouiDetailCommentRecord(
                            itemCd: ItemCdConst.CommentSyokaiSantei,
                            cmtOpt: CIUtil.ToWide(CIUtil.SDateToWDate(retDate).ToString().Substring(3, 4)),
                            autoAdd: 1,
                            baseItemCd: odrDtl.ItemCd);
                    }
                }
            }
            else if (odrDtl.ItemCd == ItemCdConst.IgakuJyudoZensoku1)
            {
                // 重度喘息患者治療管理加算（１月目）の場合、診療日を記載
                RecedenCmtSelectModel recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.Syokai);

                if (recedenCmtSelect != null)
                {
                    if (_existsItemCd(recedenCmtSelect.CommentCd) == false)
                    {
                        AppendNewWrkSinKouiDetailCommentRecord(recedenCmtSelect.CommentCd, CIUtil.ToWide(CIUtil.SDateToWDate(_sinDate).ToString()), autoAdd: 1);
                    }
                }
                else
                {
                    AppendNewWrkSinKouiDetailCommentRecord(ItemCdConst.CommentSyokaiSantei, CIUtil.ToWide(CIUtil.SDateToWDate(_sinDate).ToString().Substring(3, 4)), autoAdd: 1);
                }
            }
            else if (odrDtl.ItemCd == ItemCdConst.IgakuJyudoZensoku2_6)
            {
                // 重度喘息患者治療管理加算（２月目以降６月目まで）の場合、喘息治療管理料１（１月目）の日付を取得
                retDate = _santeiFinder.FindLastSanteiDate(_hpId, _ptId, _sinDate, _sinDate, _raiinNo, ItemCdConst.IgakuZensoku1_1, _hokenKbn);
                if (retDate > 0)
                {
                    RecedenCmtSelectModel recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.Syokai);

                    if (recedenCmtSelect != null)
                    {
                        if (_existsItemCd(recedenCmtSelect.CommentCd) == false)
                        {
                            AppendNewWrkSinKouiDetailCommentRecord(recedenCmtSelect.CommentCd, CIUtil.ToWide(CIUtil.SDateToWDate(retDate).ToString()), autoAdd: 1);
                        }
                    }
                    //else
                    //{
                    //    AppendNewWrkSinKouiDetailCommentRecord(
                    //        itemCd: ItemCdConst.CommentSyokaiSantei,
                    //        cmtOpt: CIUtil.ToWide(CIUtil.SDateToWDate(retDate).ToString().Substring(3, 4)),
                    //        autoAdd: 1,
                    //        baseItemCd: odrDtl.ItemCd);
                    //}
                }
            }
            else if (odrDtl.ItemCd == ItemCdConst.IgakuBienMeneki1)
            {
                // アレルギー性鼻炎免疫療法治療管理料（１月目）の場合、診療日を記載
                RecedenCmtSelectModel recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.Syokai);

                if (recedenCmtSelect != null)
                {
                    if (_existsItemCd(recedenCmtSelect.CommentCd) == false)
                    {
                        AppendNewWrkSinKouiDetailCommentRecord(recedenCmtSelect.CommentCd, CIUtil.ToWide(CIUtil.SDateToWDate(_sinDate).ToString()), autoAdd: 1);
                    }
                }
                else
                {
                    AppendNewWrkSinKouiDetailCommentRecord(ItemCdConst.CommentSyokaiSantei, CIUtil.ToWide(CIUtil.SDateToWDate(_sinDate).ToString().Substring(3, 4)), autoAdd: 1);
                }
            }
            else if (odrDtl.ItemCd == ItemCdConst.IgakuBienMeneki2)
            {
                // アレルギー性鼻炎免疫療法治療管理料（２月目以降）の場合、アレルギー性鼻炎免疫療法治療管理料（１月目）の日付を取得
                retDate = _santeiFinder.FindLastSanteiDate(_hpId, _ptId, _sinDate, _sinDate, _raiinNo, ItemCdConst.IgakuBienMeneki1, _hokenKbn);
                if (retDate > 0)
                {
                    RecedenCmtSelectModel recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.Syokai);

                    if (recedenCmtSelect != null)
                    {
                        if (_existsItemCd(recedenCmtSelect.CommentCd) == false)
                        {
                            AppendNewWrkSinKouiDetailCommentRecord(recedenCmtSelect.CommentCd, CIUtil.ToWide(CIUtil.SDateToWDate(retDate).ToString()), autoAdd: 1);
                        }
                    }
                    else
                    {
                        AppendNewWrkSinKouiDetailCommentRecord(
                            itemCd: ItemCdConst.CommentSyokaiSantei,
                            cmtOpt: CIUtil.ToWide(CIUtil.SDateToWDate(retDate).ToString().Substring(3, 4)),
                            autoAdd: 1,
                            baseItemCd: odrDtl.ItemCd);
                    }
                }
            }
            else if (taigaiKenbiJuseis.Contains(odrDtl.ItemCd))
            {
                // 体外受精・顕微授精管理料、体外受精及び顕微授精同時実施管理料の場合、9種類の項目のうち初回の日付を取得
                retDate = _santeiFinder.FindFirstSanteiDate(_hpId, _ptId, _sinDate, _raiinNo, taigaiKenbiJuseis, _hokenKbn);

                if (retDate <= 0)
                {
                    retDate = _sinDate;
                }

                RecedenCmtSelectModel recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.Syokai);

                if (recedenCmtSelect != null)
                {
                    if (_existsItemCd(recedenCmtSelect.CommentCd) == false)
                    {
                        AppendNewWrkSinKouiDetailCommentRecord(recedenCmtSelect.CommentCd, CIUtil.ToWide(CIUtil.SDateToWDate(retDate).ToString()), autoAdd: 1);
                    }
                }
                else
                {
                    AppendNewWrkSinKouiDetailCommentRecord(
                        itemCd: ItemCdConst.CommentSyokaiSantei,
                        cmtOpt: CIUtil.ToWide(CIUtil.SDateToWDate(retDate).ToString().Substring(3, 4)),
                        autoAdd: 1,
                        baseItemCd: odrDtl.ItemCd);
                }
            }
            else if (juseiranHaiBaiyos.Contains(odrDtl.ItemCd))
            {
                // 受精卵・胚培養管理料の場合、4種類の項目のうち初回の日付を取得
                retDate = _santeiFinder.FindFirstSanteiDate(_hpId, _ptId, _sinDate, _raiinNo, juseiranHaiBaiyos, _hokenKbn);

                if (retDate <= 0)
                {
                    retDate = _sinDate;
                }

                RecedenCmtSelectModel recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.Syokai);

                if (recedenCmtSelect != null)
                {
                    if (_existsItemCd(recedenCmtSelect.CommentCd) == false)
                    {
                        AppendNewWrkSinKouiDetailCommentRecord(recedenCmtSelect.CommentCd, CIUtil.ToWide(CIUtil.SDateToWDate(retDate).ToString()), autoAdd: 1);
                    }
                }
                else
                {
                    AppendNewWrkSinKouiDetailCommentRecord(
                        itemCd: ItemCdConst.CommentSyokaiSantei,
                        cmtOpt: CIUtil.ToWide(CIUtil.SDateToWDate(retDate).ToString().Substring(3, 4)),
                        autoAdd: 1,
                        baseItemCd: odrDtl.ItemCd);
                }
            }
            else if (haiHozonDonyus.Contains(odrDtl.ItemCd))
            {
                // 胚凍結保存管理料（胚凍結保存管理料（導入時））の場合、4種類の項目のうち初回の日付を取得
                retDate = _santeiFinder.FindFirstSanteiDate(_hpId, _ptId, _sinDate, _raiinNo, haiHozonDonyus, _hokenKbn);

                if (retDate <= 0)
                {
                    retDate = _sinDate;
                }

                RecedenCmtSelectModel recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.Syokai);

                if (recedenCmtSelect != null)
                {
                    if (_existsItemCd(recedenCmtSelect.CommentCd) == false)
                    {
                        AppendNewWrkSinKouiDetailCommentRecord(recedenCmtSelect.CommentCd, CIUtil.ToWide(CIUtil.SDateToWDate(retDate).ToString()), autoAdd: 1);
                    }
                }
                else
                {
                    AppendNewWrkSinKouiDetailCommentRecord(
                        itemCd: ItemCdConst.CommentSyokaiSantei,
                        cmtOpt: CIUtil.ToWide(CIUtil.SDateToWDate(retDate).ToString().Substring(3, 4)),
                        autoAdd: 1,
                        baseItemCd: odrDtl.ItemCd);
                }
            }
            else if (addSanteiComment == false)
            {
                switch (odrDtl.CmtKbn)
                {
                    case 1:     // 実施日
                        {
                            RecedenCmtSelectModel recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.JissiBi);

                            if (recedenCmtSelect != null)
                            {
                                if (_existsItemCd(recedenCmtSelect.CommentCd) == false)
                                {
                                    string cmtOpt = _getCmtOptDate(recedenCmtSelect.CommentCd, _sinDate);

                                    AppendNewWrkSinKouiDetailCommentRecord(recedenCmtSelect.CommentCd, cmtOpt, autoAdd: 1);
                                }
                            }
                            else
                            {
                                AppendNewWrkSinKouiDetailCommentRecord(
                                itemCd: ItemCdConst.CommentFree,
                                cmtOpt: CIUtil.SDateToShowWDate2(_sinDate).Replace(" ", "") + "実施",
                                autoAdd: 1,
                                baseItemCd: odrDtl.ItemCd);
                            }
                        }
                        break;
                    case 2:     // 前回実施日
                        retDate = _santeiFinder.FindLastSanteiDate(_hpId, _ptId, _sinDate, _sinDate, _raiinNo, odrDtl.ItemCd, _hokenKbn);
                        if (retDate > 0)
                        {
                            RecedenCmtSelectModel recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.Zenkai);

                            if (recedenCmtSelect != null)
                            {
                                if (_existsItemCd(recedenCmtSelect.CommentCd) == false)
                                {
                                    string cmtOpt = _getCmtOptDate(recedenCmtSelect.CommentCd, retDate);

                                    AppendNewWrkSinKouiDetailCommentRecord(recedenCmtSelect.CommentCd, cmtOpt, autoAdd: 1);
                                }
                            }
                            else
                            {
                                //AppendNewWrkSinKouiDetailCommentRecord(ItemCdConst.CommentZenkaiJissi, CIUtil.ToWide((retDate % 10000).ToString()), autoAdd: 1);
                                AppendNewWrkSinKouiDetailCommentRecord(
                                    itemCd: ItemCdConst.CommentFree,
                                    cmtOpt: "前回実施　" + CIUtil.ToWide(CIUtil.SDateToShowWDate2(retDate).Replace(" ", "")),
                                    autoAdd: 1,
                                    baseItemCd: odrDtl.ItemCd);
                            }
                        }
                        break;
                    case 3:     // 初回実施日
                        {
                            if (syokaiDate <= 0)
                            {
                                syokaiDate = _santeiFinder.FindFirstSanteiDate(_hpId, _ptId, _sinDate, _raiinNo, odrDtl.ItemCd, _hokenKbn);

                                if (syokaiDate <= 0)
                                {
                                    syokaiDate = _sinDate;
                                }
                            }

                            RecedenCmtSelectModel recedenCmtSelect = null;

                            List<int> cmtSbts =
                                new List<int> {
                                    CmtSbtConst.Syokai,
                                    CmtSbtConst.Hassyo,
                                    CmtSbtConst.ChiryoKaisi,
                                    CmtSbtConst.HassyoOrChiryo
                                };

                            foreach (int cmtsbt in cmtSbts)
                            {
                                recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, cmtsbt);

                                if (recedenCmtSelect != null)
                                {
                                    break;
                                }
                            }

                            if (recedenCmtSelect != null && recedenCmtSelect.CommentCd.StartsWith("820"))
                            {
                                // 820の場合、本当に初回の時だけ出力する
                                if (_existsItemCd(recedenCmtSelect.CommentCd) == false)
                                {
                                    if (syokaiDate == _sinDate)
                                    {
                                        AppendNewWrkSinKouiDetailCommentRecord(recedenCmtSelect.CommentCd, "", autoAdd: 1);
                                    }
                                }
                            }
                            else if (syokaiDate > 0)
                            {
                                if (recedenCmtSelect != null)
                                {
                                    if (_existsItemCd(recedenCmtSelect.CommentCd) == false)
                                    {
                                        string cmtOpt = _getCmtOptDate(recedenCmtSelect.CommentCd, syokaiDate);
                                        AppendNewWrkSinKouiDetailCommentRecord(recedenCmtSelect.CommentCd, cmtOpt, autoAdd: 1);
                                    }
                                }
                                else
                                {
                                    //AppendNewWrkSinKouiDetailCommentRecord(ItemCdConst.CommentSyokaiJissi, CIUtil.ToWide((retDate % 10000).ToString()), autoAdd: 1);
                                    AppendNewWrkSinKouiDetailCommentRecord(
                                    itemCd: ItemCdConst.CommentFree,
                                    cmtOpt: "初回実施　" + CIUtil.ToWide(CIUtil.SDateToShowWDate2(syokaiDate).Replace(" ", "")),
                                    autoAdd: 1,
                                    baseItemCd: odrDtl.ItemCd);
                                }
                            }
                            //else
                            //{
                            //    //AppendNewWrkSinKouiDetailCommentRecord(ItemCdConst.CommentSyokaiJissi, CIUtil.ToWide((_sinDate % 10000).ToString()), autoAdd: 1);
                            //    AppendNewWrkSinKouiDetailCommentRecord(
                            //        itemCd: ItemCdConst.CommentFree, 
                            //        cmtOpt: "初回実施　" + CIUtil.ToWide(CIUtil.SDateToShowWDate2(_sinDate).Replace(" ", "")), 
                            //        autoAdd: 1,
                            //        baseItemCd: odrDtl.ItemCd);
                            //}

                            break;
                        }
                    case 4:     // 前回算定日 or 初回算定日
                        {
                            retDate = _santeiFinder.FindLastSanteiDate(_hpId, _ptId, _sinDate, _sinDate, _raiinNo, odrDtl.ItemCd, _hokenKbn);

                            RecedenCmtSelectModel recedenCmtSelectZenkai = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.Zenkai);
                            RecedenCmtSelectModel recedenCmtSelectSyokai = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.Syokai);

                            if ((recedenCmtSelectZenkai != null && _existsItemCd(recedenCmtSelectZenkai.CommentCd)) ||
                                (recedenCmtSelectSyokai != null && _existsItemCd(recedenCmtSelectSyokai.CommentCd)))
                            {
                                // 前回日 or 初回日のコメントが手入力されている場合は何もしない
                            }
                            else if (retDate > 0 &&
                                (
                                    (syokaiDate < retDate) ||
                                    (syokaiDate == retDate && syokaiDate != _sinDate))
                                )
                            {
                                // 前回日が取得できて、
                                // 初回日 < 前回日 or
                                // 初回日 == 前回日 and 初回日 != 診療日
                                //RecedenCmtSelectModel recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.Zenkai);

                                if (recedenCmtSelectZenkai != null)
                                {
                                    if (_existsItemCd(recedenCmtSelectZenkai.CommentCd) == false)
                                    {
                                        string cmtOpt = _getCmtOptDate(recedenCmtSelectZenkai.CommentCd, retDate);
                                        AppendNewWrkSinKouiDetailCommentRecord(recedenCmtSelectZenkai.CommentCd, cmtOpt, autoAdd: 1);
                                    }
                                }
                                else
                                {
                                    //AppendNewWrkSinKouiDetailCommentRecord(ItemCdConst.CommentZenkaiJissi, CIUtil.ToWide((retDate % 10000).ToString()), autoAdd: 1);
                                    AppendNewWrkSinKouiDetailCommentRecord(
                                    itemCd: ItemCdConst.CommentFree,
                                    cmtOpt: "前回実施　" + CIUtil.ToWide(CIUtil.SDateToShowWDate2(retDate).Replace(" ", "")),
                                    autoAdd: 1,
                                    baseItemCd: odrDtl.ItemCd);
                                }
                            }
                            else
                            {
                                if (syokaiDate <= 0)
                                {
                                    syokaiDate = _sinDate;
                                }

                                //RecedenCmtSelectModel recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.Syokai);

                                if (recedenCmtSelectSyokai != null)
                                {
                                    if (_existsItemCd(recedenCmtSelectSyokai.CommentCd) == false)
                                    {
                                        if (recedenCmtSelectSyokai.CommentCd.StartsWith("820"))
                                        {
                                            if (syokaiDate == _sinDate)
                                            {
                                                // 820の場合、本当に初回の場合のみ出力
                                                AppendNewWrkSinKouiDetailCommentRecord(recedenCmtSelectSyokai.CommentCd, "", autoAdd: 1);
                                            }
                                        }
                                        else
                                        {
                                            string cmtOpt = _getCmtOptDate(recedenCmtSelectSyokai.CommentCd, syokaiDate);
                                            AppendNewWrkSinKouiDetailCommentRecord(recedenCmtSelectSyokai.CommentCd, cmtOpt, autoAdd: 1);
                                        }
                                    }
                                }
                                else
                                {
                                    //AppendNewWrkSinKouiDetailCommentRecord(ItemCdConst.CommentSyokaiJissi, CIUtil.ToWide((_sinDate % 10000).ToString()), autoAdd: 1);
                                    AppendNewWrkSinKouiDetailCommentRecord(
                                    itemCd: ItemCdConst.CommentFree,
                                    cmtOpt: "初回実施　" + CIUtil.ToWide(CIUtil.SDateToShowWDate2(syokaiDate).Replace(" ", "")),
                                    autoAdd: 1,
                                    baseItemCd: odrDtl.ItemCd);
                                }
                            }
                        }
                        break;
                    case 5:     // 初回算定日
                        {
                            if (syokaiDate <= 0)
                            {
                                string itemcd = odrDtl.ItemCd;
                                if (itemcd == ItemCdConst.IgakuTokuYaku4)
                                {
                                    // 特定薬剤治療管理料１（第４月目以降）の場合は、特定薬剤治療管理料１を初回日とする
                                    itemcd = ItemCdConst.IgakuTokuYaku;
                                }
                                syokaiDate = _santeiFinder.FindFirstSanteiDate(_hpId, _ptId, _sinDate, _raiinNo, itemcd, _hokenKbn);

                                if (syokaiDate <= 0)
                                {
                                    syokaiDate = _sinDate;
                                }
                            }

                            RecedenCmtSelectModel recedenCmtSelect = null;

                            List<int> cmtSbts =
                                new List<int> {
                                    CmtSbtConst.Syokai,
                                    CmtSbtConst.Hassyo,
                                    CmtSbtConst.ChiryoKaisi,
                                    CmtSbtConst.HassyoOrChiryo
                                };

                            foreach (int cmtsbt in cmtSbts)
                            {
                                recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, cmtsbt);

                                if (recedenCmtSelect != null)
                                {
                                    break;
                                }
                            }

                            if (recedenCmtSelect != null && recedenCmtSelect.CommentCd.StartsWith("820"))
                            {
                                // 820の場合、本当に初回の場合のみ出力
                                if (_existsItemCd(recedenCmtSelect.CommentCd) == false)
                                {
                                    if (syokaiDate == _sinDate)
                                    {
                                        AppendNewWrkSinKouiDetailCommentRecord(recedenCmtSelect.CommentCd, "", autoAdd: 1);
                                    }
                                }
                            }
                            else if (syokaiDate > 0)
                            {
                                if (recedenCmtSelect != null)
                                {
                                    if (_existsItemCd(recedenCmtSelect.CommentCd) == false)
                                    {
                                        string cmtOpt = _getCmtOptDate(recedenCmtSelect.CommentCd, syokaiDate);
                                        AppendNewWrkSinKouiDetailCommentRecord(recedenCmtSelect.CommentCd, cmtOpt, autoAdd: 1);
                                    }
                                }
                                else
                                {
                                    //AppendNewWrkSinKouiDetailCommentRecord(ItemCdConst.CommentSyokaiJissi, CIUtil.ToWide((retDate % 10000).ToString()), autoAdd: 1);
                                    AppendNewWrkSinKouiDetailCommentRecord(
                                    itemCd: ItemCdConst.CommentFree,
                                    cmtOpt: "初回算定　" + CIUtil.ToWide(CIUtil.SDateToShowWDate2(syokaiDate).Replace(" ", "")),
                                    autoAdd: 1,
                                    baseItemCd: odrDtl.ItemCd);
                                }
                            }
                            //else
                            //{
                            //    //AppendNewWrkSinKouiDetailCommentRecord(ItemCdConst.CommentSyokaiJissi, CIUtil.ToWide((_sinDate % 10000).ToString()), autoAdd: 1);
                            //    AppendNewWrkSinKouiDetailCommentRecord(
                            //        itemCd: ItemCdConst.CommentFree, 
                            //        cmtOpt: "初回算定　" + CIUtil.ToWide(CIUtil.SDateToShowWDate2(_sinDate).Replace(" ", "")), 
                            //        autoAdd: 1,
                            //        baseItemCd: odrDtl.ItemCd);
                            //}
                            break;
                        }
                    case 6:     // 実施日（列挙）
                                //comment = GetSanteiDaysComment(_sinDate / 100 * 100 + 1, _sinDate / 100 * 100 + 31, odrDtl.ItemCd);

                        //if(comment != "")
                        //{
                        //    AppendNewWrkSinKouiDetailCommentRecord(ItemCdConst.CommentFree, comment);
                        //}

                        {
                            RecedenCmtSelectModel recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.JissiBi);

                            if (recedenCmtSelect == null || _existsItemCd(recedenCmtSelect.CommentCd) == false)
                            {
                                AppendNewWrkSinKouiDetail(
                                    itemCd: ItemCdConst.CommentJissiRekkyoDummy,
                                    autoAdd: 1,
                                    suryo: 0,
                                    cmtOpt: odrDtl.ItemCd,
                                    baseItemCd: odrDtl.ItemCd);
                            }
                        }
                        break;
                    case 7:     // 実施日（列挙、前月末・翌月頭含む）
                                //int startDate;
                                //int endDate;

                        //string totalComment = "";

                        //// 前月末週
                        //startDate = IkaCalculateUtilViewModel.GetFirstDateOfWeek(_sinDate / 100 * 100 + 1);

                        //if (startDate / 100 < _sinDate / 100)
                        //{
                        //    endDate = IkaCalculateUtilViewModel.GetLastDateOfMonth(startDate);
                        //    comment = GetSanteiDaysComment(startDate, endDate, odrDtl.ItemCd);
                        //    totalComment += comment;
                        //}

                        //// 当月
                        //comment = GetSanteiDaysComment(_sinDate / 100 * 100 + 1, _sinDate / 100 * 100 + 31, odrDtl.ItemCd);
                        //totalComment += comment;

                        //// 翌月初週
                        //endDate = IkaCalculateUtilViewModel.GetLastDateOfWeek(IkaCalculateUtilViewModel.GetLastDateOfMonth(_sinDate));

                        //if (endDate / 100 < _sinDate / 100)
                        //{
                        //    startDate = endDate / 100 * 100 + 1;
                        //    comment = GetSanteiDaysComment(startDate, endDate, odrDtl.ItemCd);
                        //    totalComment += comment;
                        //}

                        //if(totalComment != "")
                        //{
                        //    AppendNewWrkSinKouiDetailCommentRecord(ItemCdConst.CommentFree, comment);
                        //}

                        {
                            RecedenCmtSelectModel recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.JissiBi);

                            if (recedenCmtSelect == null || _existsItemCd(recedenCmtSelect.CommentCd) == false)
                            {
                                AppendNewWrkSinKouiDetail(
                                    itemCd: ItemCdConst.CommentJissiRekkyoZengoDummy,
                                    autoAdd: 1,
                                    suryo: 0,
                                    cmtOpt: odrDtl.ItemCd,
                                    baseItemCd: odrDtl.ItemCd);
                            }
                        }
                        break;
                    case 8:     // 実施日（列挙、項目名あり）
                        {
                            RecedenCmtSelectModel recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.JissiBi);

                            if (recedenCmtSelect == null || _existsItemCd(recedenCmtSelect.CommentCd) == false)
                            {
                                AppendNewWrkSinKouiDetail(
                                itemCd: ItemCdConst.CommentJissiRekkyoItemNameDummy,
                                autoAdd: 1,
                                cmtOpt: odrDtl.ItemCd,
                                baseItemCd: odrDtl.ItemCd);
                            }
                        }
                        break;
                    //case 9:     // 実施日数
                    //    AppendNewWrkSinKouiDetail(ItemCdConst.CommentJissiNissuDummy, autoAdd: 1, cmtOpt: odrDtl.ItemCd);
                    //    break;
                    case 10:    // 前回日 or 初回日（項目名あり）
                        {
                            retDate = _santeiFinder.FindLastSanteiDate(_hpId, _ptId, _sinDate, _sinDate, _raiinNo, odrDtl.ItemCd, _hokenKbn);

                            RecedenCmtSelectModel recedenCmtSelectZenkai = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.Zenkai);
                            RecedenCmtSelectModel recedenCmtSelectSyokai = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.Syokai);

                            if ((recedenCmtSelectZenkai != null && _existsItemCd(recedenCmtSelectZenkai.CommentCd)) ||
                                (recedenCmtSelectSyokai != null && _existsItemCd(recedenCmtSelectSyokai.CommentCd)))
                            {
                                // 前回日 or 初回日のコメントが手入力されている場合は何もしない
                            }
                            else if (retDate > 0 &&
                                (
                                    (syokaiDate < retDate) ||
                                    (syokaiDate == retDate && syokaiDate != _sinDate))
                                )
                            {
                                // 前回日が取得できて、
                                // 初回日 < 前回日 or
                                // 初回日 == 前回日 and 初回日 != 診療日                           
                                RecedenCmtSelectModel recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.Zenkai);

                                if (recedenCmtSelect != null)
                                {
                                    if (_existsItemCd(recedenCmtSelect.CommentCd) == false)
                                    {
                                        string cmtOpt = _getCmtOptDate(recedenCmtSelect.CommentCd, retDate);
                                        AppendNewWrkSinKouiDetailCommentRecord(recedenCmtSelect.CommentCd, cmtOpt, autoAdd: 1);
                                    }
                                }
                                else
                                {
                                    //AppendNewWrkSinKouiDetailCommentRecord(ItemCdConst.CommentZenkaiJissi, CIUtil.ToWide((retDate % 10000).ToString()), autoAdd: 1);
                                    AppendNewWrkSinKouiDetailCommentRecord(
                                    itemCd: ItemCdConst.CommentFree,
                                    cmtOpt: string.Format("({0}：前回実施 {1})", odrDtl.ReceName, CIUtil.ToWide(CIUtil.SDateToShowWDate2(retDate).Replace(" ", ""))),
                                    autoAdd: 1,
                                    baseItemCd: odrDtl.ItemCd);
                                }
                            }
                            else
                            {
                                if (syokaiDate <= 0)
                                {
                                    syokaiDate = _sinDate;
                                }

                                RecedenCmtSelectModel recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.Syokai);

                                if (recedenCmtSelect != null)
                                {
                                    if (_existsItemCd(recedenCmtSelect.CommentCd) == false)
                                    {
                                        if (recedenCmtSelect.CommentCd.StartsWith("820"))
                                        {
                                            if (syokaiDate == _sinDate)
                                            {
                                                // 820の場合、本当に初回の場合のみ出力
                                                AppendNewWrkSinKouiDetailCommentRecord(recedenCmtSelect.CommentCd, "", autoAdd: 1);
                                            }
                                        }
                                        else
                                        {
                                            string cmtOpt = _getCmtOptDate(recedenCmtSelect.CommentCd, syokaiDate);
                                            AppendNewWrkSinKouiDetailCommentRecord(recedenCmtSelect.CommentCd, cmtOpt, autoAdd: 1);
                                        }
                                    }
                                }
                                else
                                {
                                    //AppendNewWrkSinKouiDetailCommentRecord(ItemCdConst.CommentSyokaiJissi, CIUtil.ToWide((_sinDate % 10000).ToString()), autoAdd: 1);
                                    AppendNewWrkSinKouiDetailCommentRecord(
                                    itemCd: ItemCdConst.CommentFree,
                                    cmtOpt: string.Format("({0}：初回実施 {1})", odrDtl.ReceName, CIUtil.ToWide(CIUtil.SDateToShowWDate2(syokaiDate).Replace(" ", ""))),
                                    autoAdd: 1,
                                    baseItemCd: odrDtl.ItemCd);
                                }
                            }
                        }
                        break;
                    case 11:     // 数量コメント
                        if (string.IsNullOrEmpty(odrDtl.UnitName) == false && odrDtl.Suryo > 0)
                        {
                            RecedenCmtSelectModel recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(odrDtl.ItemCd, CmtSbtConst.SinryoJikan);

                            if (recedenCmtSelect != null)
                            {
                                if (_existsItemCd(recedenCmtSelect.CommentCd) == false)
                                {
                                    AppendNewWrkSinKouiDetailCommentRecord(recedenCmtSelect.CommentCd, CIUtil.ToWide(odrDtl.Suryo.ToString()), autoAdd: 1);
                                }
                            }
                        }
                        break;
                }
            }

            #region Local Method
            // 算定日列挙コメントを取得する
            string GetSanteiDaysComment(int startDate, int endDate, string itemCd)
            {
                List<SanteiDaysModel> santeiDays;
                string ret = "";

                santeiDays =
                    _santeiFinder.GetSanteiDays(_hpId, _ptId, startDate, endDate, _raiinNo, _sinDate, odrDtl.ItemCd, _hokenKbn);
                santeiDays.Add(new SanteiDaysModel(_sinDate, odrDtl.ItemCd));   // 今日の分を追加
                IEnumerable<SanteiDaysModel> ieSanteiDays = santeiDays.Distinct();

                if (ieSanteiDays != null && ieSanteiDays.Any())
                {
                    foreach (SanteiDaysModel day in santeiDays)
                    {
                        if (ret != "")
                        {
                            ret += ",";
                        }
                        ret += (day.SinDate % 100).ToString();
                    }
                    ret = "（" + ret + "日）";
                }

                return ret;
            }

            void AppendByomeiComment(SanteiInfDetailModel AsanteiInfDtl, bool append)
            {
                string itemCd = "";
                string cmtOpt = "";

                // 病名
                if (AsanteiInfDtl.Byomei != "")
                {
                    RecedenCmtSelectModel recedenCmtSelect = _tenMstCommon.FindRecedenCmtSelect(AsanteiInfDtl.ItemCd, CmtSbtConst.Sikkan);
                    if (recedenCmtSelect != null)
                    {
                        itemCd = recedenCmtSelect.CommentCd;

                        cmtOpt = AsanteiInfDtl.Byomei;
                        if (AsanteiInfDtl.HosokuComment != "")
                        {
                            cmtOpt += "（" + AsanteiInfDtl.HosokuComment + "）";
                        }

                        if (cmtOpt != "")
                        {
                            AppendNewWrkSinKouiDetailCommentRecord(itemCd, cmtOpt);
                        }
                    }
                    else
                    {
                        itemCd = ItemCdConst.CommentFree;
                        if (append == false)
                        {
                            cmtOpt = "疾患名 " + AsanteiInfDtl.Byomei;
                            if (AsanteiInfDtl.HosokuComment != "")
                            {
                                cmtOpt += "（" + AsanteiInfDtl.HosokuComment + "）";
                            }

                            if (cmtOpt != "")
                            {
                                AppendNewWrkSinKouiDetailCommentRecord(itemCd, cmtOpt);
                            }
                        }
                        else
                        {
                            cmtOpt = ", " + AsanteiInfDtl.Byomei;
                            if (AsanteiInfDtl.HosokuComment != "")
                            {
                                cmtOpt += "（" + AsanteiInfDtl.HosokuComment + "）";
                            }

                            if (cmtOpt != "")
                            {
                                string retCmt = "";
                                string retCmtOpt = "";

                                retCmtOpt = cmtOpt;

                                retCmt = _tenMstCommon.GetCommentStr(itemCd, ref retCmtOpt);

                                wrkSinKouiDetails.Last().ItemName += retCmt;
                                wrkSinKouiDetails.Last().CmtOpt += retCmtOpt;
                            }
                        }
                    }
                }

                // コメント
                if (AsanteiInfDtl.Comment != "")
                {
                    itemCd = ItemCdConst.CommentFree;
                    cmtOpt = AsanteiInfDtl.Comment;
                    AppendNewWrkSinKouiDetailCommentRecord(itemCd, cmtOpt);
                }
            }

            string _getCmtOptDate(string AitemCd, int Adate)
            {
                string ret = CIUtil.ToWide(CIUtil.SDateToWDate(Adate).ToString());

                List<TenMstModel> tenMsts = _tenMstCommon.GetTenMst(AitemCd);

                if (tenMsts != null && tenMsts.Any())
                {
                    if (tenMsts.First().Name.Contains("年月日") == false && tenMsts.First().Name.Contains("年月"))
                    {
                        ret = CIUtil.ToWide(CIUtil.Copy(CIUtil.SDateToWDate(Adate).ToString(), 1, 5));
                    }
                }

                return ret;
            }
            #endregion
        }

        /// <summary>
        /// ワーク診療行為詳細（コメント）を生成し、追加する
        /// </summary>
        /// <param name="itemCd">診療行為コード</param>
        /// <param name="cmtOpt">コメント文</param>
        /// <param name="autoAdd">1:自動発生</param>
        /// <param name="fmtKbn">書式区分</param>
        public void AppendNewWrkSinKouiDetailCommentRecord(string itemCd, string cmtOpt, int autoAdd = 1, int fmtKbn = 0, string baseItemCd = "", int isnodspRyosyu = -1, int baseSeqNo = 0)
        {
            AppendWrkSinKouiDetail(GetWrkSinKouiDetailCommentRecord(itemCd: itemCd, cmtOpt: cmtOpt, autoAdd: autoAdd, fmtKbn: fmtKbn, baseItemCd: baseItemCd, isNodspRyosyu: isnodspRyosyu, baseSeqNo: baseSeqNo));
        }

        /// <summary>
        /// ワーク診療行為詳細（コメント）を生成し、指定のRpに追加する
        /// </summary>
        /// <param name="rpNo">追加先RpNo</param>
        /// <param name="seqNo">追加先SeqNo</param>
        /// <param name="itemCd">診療行為コード</param>
        /// <param name="cmtOpt">コメント文</param>
        /// <param name="autoAdd">1:自動発生</param>
        /// <param name="fmtKbn">書式区分</param>
        public void InsertNewWrkSinKouiDetailCommentRecord(int rpNo, int seqNo, string itemCd, string cmtOpt, int autoAdd = 1, int fmtKbn = 0)
        {
            InsertWrkSinKouiDetail(rpNo, seqNo, GetWrkSinKouiDetailCommentRecord(itemCd, cmtOpt, autoAdd, fmtKbn));
        }

        /// <summary>
        /// 指定の診療行為コードを持つレコードが存在するかチェックする
        /// </summary>
        /// <param name="itemCd">チェックしたい診療行為コード</param>
        /// <returns>true: 存在する</returns>
        public bool ExistWrkSinKouiDetailByItemCd(string itemCd, bool onlyThisRaiin = true)
        {
            if (onlyThisRaiin)
            {
                return _wrkSinKouiDetails.Any(p =>
                    p.HpId == _hpId &&
                    p.PtId == _ptId &&
                    p.RaiinNo == _raiinNo &&
                    //p.HokenKbn == _hokenKbn && 
                    checkHokenKbn.Contains(p.HokenKbn) &&
                    checkSanteiKbn.Contains(GetSanteiKbn(p.RaiinNo, p.RpNo)) &&
                    p.ItemCd == itemCd &&
                    p.IsDeleted == DeleteStatus.None);
            }
            else
            {
                return _wrkSinKouiDetails.Any(p =>
                    p.HpId == _hpId &&
                    p.PtId == _ptId &&
                    //p.RaiinNo == _raiinNo &&
                    //p.HokenKbn == _hokenKbn && 
                    checkHokenKbn.Contains(p.HokenKbn) &&
                    checkSanteiKbn.Contains(GetSanteiKbn(p.RaiinNo, p.RpNo)) &&
                    p.ItemCd == itemCd &&
                    p.IsDeleted == DeleteStatus.None);
            }
        }

        public bool ExistWrkSinKouiDetailByItemCd(List<string> itemCds, bool onlyThisRaiin = true, bool excludeSanteiGai = true, bool sameHokenKbn = false, List<int> hokenKbns = null, List<int> santeiKbns = null)
        {
            bool ret = false;

            List<int> checkHokenKbns = checkHokenKbn;

            if (hokenKbns != null)
            {
                checkHokenKbns = hokenKbns;
            }
            else if (sameHokenKbn)
            {
                checkHokenKbns = new List<int> { _hokenKbn };
            }

            List<int> checkSanteiKbns = checkSanteiKbn;

            if (santeiKbns != null)
            {
                checkSanteiKbns = santeiKbns;
            }

            List<WrkSinKouiDetailModel> wrkDtls = new List<WrkSinKouiDetailModel>();

            if (onlyThisRaiin)
            {
                wrkDtls = _wrkSinKouiDetails.FindAll(p =>
                    p.HpId == _hpId &&
                    p.PtId == _ptId &&
                    p.RaiinNo == _raiinNo &&
                    checkHokenKbns.Contains(p.HokenKbn) &&
                    checkSanteiKbns.Contains(GetSanteiKbn(p.RaiinNo, p.RpNo)) &&
                    itemCds.Contains(p.ItemCd) &&
                    p.IsDeleted == DeleteStatus.None);
            }
            else
            {
                wrkDtls = _wrkSinKouiDetails.FindAll(p =>
                    p.HpId == _hpId &&
                    p.PtId == _ptId &&
                    checkHokenKbns.Contains(p.HokenKbn) &&
                    checkSanteiKbns.Contains(GetSanteiKbn(p.RaiinNo, p.RpNo)) &&
                    itemCds.Contains(p.ItemCd) &&
                    p.IsDeleted == DeleteStatus.None);
            }

            if (excludeSanteiGai)
            {
                foreach (WrkSinKouiDetailModel wrkDtl in wrkDtls)
                {
                    if (_wrkSinRpInfs.Any(p =>
                         p.RaiinNo == wrkDtl.RaiinNo &&
                         p.RpNo == wrkDtl.RpNo &&
                         p.SanteiKbn == SanteiKbnConst.Santei &&
                         p.IsDeleted == DeleteStatus.None))
                    {
                        ret = true;
                        break;
                    }
                }
            }
            else if (wrkDtls.Any())
            {
                ret = true;
            }

            return ret;
        }

        public bool ExistWrkSinKouiDetailByItemCdRpNo(List<string> itemCds, long rpno, bool onlyThisRaiin = true, bool excludeSanteiGai = true, bool sameHokenKbn = false, List<int> hokenKbns = null, List<int> santeiKbns = null)
        {
            bool ret = false;

            List<int> checkHokenKbns = checkHokenKbn;

            if (hokenKbns != null)
            {
                checkHokenKbns = hokenKbns;
            }
            else if (sameHokenKbn)
            {
                checkHokenKbns = new List<int> { _hokenKbn };
            }

            List<int> checkSanteiKbns = checkSanteiKbn;

            if (santeiKbns != null)
            {
                checkSanteiKbns = santeiKbns;
            }

            List<WrkSinKouiDetailModel> wrkDtls = new List<WrkSinKouiDetailModel>();

            if (onlyThisRaiin)
            {
                wrkDtls = _wrkSinKouiDetails.FindAll(p =>
                    p.HpId == _hpId &&
                    p.PtId == _ptId &&
                    p.RaiinNo == _raiinNo &&
                    checkHokenKbns.Contains(p.HokenKbn) &&
                    checkSanteiKbns.Contains(GetSanteiKbn(p.RaiinNo, p.RpNo)) &&
                    itemCds.Contains(p.ItemCd) &&
                    p.RpNo < rpno &&
                    p.IsDeleted == DeleteStatus.None);
            }
            else
            {
                wrkDtls = _wrkSinKouiDetails.FindAll(p =>
                    p.HpId == _hpId &&
                    p.PtId == _ptId &&
                    checkHokenKbns.Contains(p.HokenKbn) &&
                    checkSanteiKbns.Contains(GetSanteiKbn(p.RaiinNo, p.RpNo)) &&
                    itemCds.Contains(p.ItemCd) &&
                    ((p.RaiinNo != _raiinNo && string.Compare(p.SinStartTime, SinStartTime) < 0) || p.RpNo < rpno) &&
                    p.IsDeleted == DeleteStatus.None);
            }

            if (excludeSanteiGai)
            {
                foreach (WrkSinKouiDetailModel wrkDtl in wrkDtls)
                {
                    if (_wrkSinRpInfs.Any(p =>
                         p.RaiinNo == wrkDtl.RaiinNo &&
                         p.RpNo == wrkDtl.RpNo &&
                         p.SanteiKbn == SanteiKbnConst.Santei &&
                         p.IsDeleted == DeleteStatus.None))
                    {
                        ret = true;
                        break;
                    }
                }
            }
            else if (wrkDtls.Any())
            {
                ret = true;
            }

            return ret;
        }
        public bool ExistWrkSinKouiDetailByItemCdExcludeThisRaiin(List<string> itemCds)
        {
            bool ret = false;

            List<int> checkHokenKbns = checkHokenKbn;

            List<WrkSinKouiDetailModel> wrkDtls = new List<WrkSinKouiDetailModel>();

            wrkDtls = _wrkSinKouiDetails.FindAll(p =>
                p.HpId == _hpId &&
                p.PtId == _ptId &&
                p.RaiinNo != _raiinNo &&
                checkHokenKbns.Contains(p.HokenKbn) &&
                itemCds.Contains(p.ItemCd) &&
                p.IsDeleted == DeleteStatus.None);

            if (wrkDtls.Any())
            {
                ret = true;
            }

            return ret;
        }
        public bool ExistWrkSinKouiDetailBySyukeiSaki(string syukeiSaki, bool onlyThisRaiin = true)
        {
            if (onlyThisRaiin)
            {
                return _wrkSinKouis.Any(p =>
                    p.HpId == _hpId &&
                    p.PtId == _ptId &&
                    p.RaiinNo == _raiinNo &&
                    //p.HokenKbn == _hokenKbn && 
                    checkHokenKbn.Contains(p.HokenKbn) &&
                    checkSanteiKbn.Contains(GetSanteiKbn(p.RaiinNo, p.RpNo)) &&
                    p.SyukeiSaki == syukeiSaki &&
                    p.IsDeleted == DeleteStatus.None);
            }
            else
            {
                return _wrkSinKouis.Any(p =>
                    p.HpId == _hpId &&
                    p.PtId == _ptId &&
                    p.RaiinNo == _raiinNo &&
                    //p.HokenKbn == _hokenKbn && 
                    checkHokenKbn.Contains(p.HokenKbn) &&
                    checkSanteiKbn.Contains(GetSanteiKbn(p.RaiinNo, p.RpNo)) &&
                    p.SyukeiSaki == syukeiSaki &&
                    p.IsDeleted == DeleteStatus.None);
            }
        }
        public bool ExistWrkSinKouiDetailBySyukeiSaki(List<string> syukeiSakis, bool onlyThisRaiin = true)
        {
            if (onlyThisRaiin)
            {
                return _wrkSinKouis.Any(p =>
                    p.HpId == _hpId &&
                    p.PtId == _ptId &&
                    p.RaiinNo == _raiinNo &&
                    //p.HokenKbn == _hokenKbn && 
                    checkHokenKbn.Contains(p.HokenKbn) &&
                    checkSanteiKbn.Contains(GetSanteiKbn(p.RaiinNo, p.RpNo)) &&
                    syukeiSakis.Contains(p.SyukeiSaki) &&
                    p.IsDeleted == DeleteStatus.None);
            }
            else
            {
                return _wrkSinKouis.Any(p =>
                    p.HpId == _hpId &&
                    p.PtId == _ptId &&
                    p.RaiinNo == _raiinNo &&
                    //p.HokenKbn == _hokenKbn && 
                    checkHokenKbn.Contains(p.HokenKbn) &&
                    checkSanteiKbn.Contains(GetSanteiKbn(p.RaiinNo, p.RpNo)) &&
                    syukeiSakis.Contains(p.SyukeiSaki) &&
                    p.IsDeleted == DeleteStatus.None);
            }
        }
        public List<WrkSinKouiModel> GetWrkSinKouiDetailBySyukeiSaki(List<string> syukeiSakis, bool onlyThisRaiin = true)
        {
            if (onlyThisRaiin)
            {
                return _wrkSinKouis.FindAll(p =>
                    p.HpId == _hpId &&
                    p.PtId == _ptId &&
                    p.RaiinNo == _raiinNo &&
                    //p.HokenKbn == _hokenKbn && 
                    checkHokenKbn.Contains(p.HokenKbn) &&
                    checkSanteiKbn.Contains(GetSanteiKbn(p.RaiinNo, p.RpNo)) &&
                    syukeiSakis.Contains(p.SyukeiSaki) &&
                    p.IsDeleted == DeleteStatus.None);
            }
            else
            {
                return _wrkSinKouis.FindAll(p =>
                    p.HpId == _hpId &&
                    p.PtId == _ptId &&
                    p.RaiinNo == _raiinNo &&
                    //p.HokenKbn == _hokenKbn && 
                    checkHokenKbn.Contains(p.HokenKbn) &&
                    checkSanteiKbn.Contains(GetSanteiKbn(p.RaiinNo, p.RpNo)) &&
                    syukeiSakis.Contains(p.SyukeiSaki) &&
                    p.IsDeleted == DeleteStatus.None);
            }
        }
        /// <summary>
        /// ワーク診療行為を探す
        /// </summary>
        /// <param name="target">0: 診療日すべて 1:この来院のみ 2:この保険のみ</param>
        /// <returns></returns>
        public List<WrkSinKouiDetailModel> FindWrkSinKouiDetail(int target)
        {
            List<WrkSinKouiDetailModel> ret = new List<WrkSinKouiDetailModel>();

            switch (target)
            {
                case FindWrkDtlMode.SinDateAll:
                    ret = FindWrkSinKouiDetailSinDate();
                    break;
                case FindWrkDtlMode.RaiinOnly:
                    ret = FindWrkSinKouiDetailRaiin();
                    break;
                case FindWrkDtlMode.HokenOnly:
                    ret = FindWrkSinKouiDetailHoken();
                    break;

            }
            return ret;
        }

        /// <summary>
        /// 現在計算中の診療日のワーク診療行為詳細を探す
        /// </summary>
        /// <returns></returns>
        public List<WrkSinKouiDetailModel> FindWrkSinKouiDetailSinDate()
        {
            return _wrkSinKouiDetails.FindAll(p =>
                p.HpId == _hpId &&
                p.PtId == _ptId &&
                p.SinDate == _sinDate &&
                p.IsDeleted == DeleteStatus.None);
        }

        /// <summary>
        /// 現在計算中の来院のワーク診療行為詳細を返す
        /// </summary>
        /// <returns></returns>
        public List<WrkSinKouiDetailModel> FindWrkSinKouiDetailRaiin()
        {
            return _wrkSinKouiDetails.FindAll(p =>
                p.HpId == _hpId &&
                p.PtId == _ptId &&
                p.RaiinNo == _raiinNo &&
                p.IsDeleted == DeleteStatus.None);
        }

        /// <summary>
        /// 計算中の来院、保険のワーク診療行為詳細を返す
        /// </summary>
        /// <returns></returns>
        public List<WrkSinKouiDetailModel> FindWrkSinKouiDetailHoken()
        {
            return _wrkSinKouiDetails.FindAll(p =>
                p.HpId == _hpId &&
                p.PtId == _ptId &&
                p.RaiinNo == _raiinNo &&
                p.HokenKbn == _hokenKbn &&
                p.IsDeleted == DeleteStatus.None);
        }

        /// <summary>
        /// 指定の診療行為コードを持つレコードを返す
        /// </summary>
        /// <param name="target">0: 診療日すべて 1:この来院のみ 2:この保険のみ</param>
        /// <param name="itemCd">取得したい診療行為コード</param>
        /// <returns>指定の診療行為コードを持つレコード</returns>
        public List<WrkSinKouiDetailModel> FindWrkSinKouiDetailByItemCd(int target, string itemCd)
        {
            List<WrkSinKouiDetailModel> ret = FindWrkSinKouiDetail(target);
            return ret.FindAll(p => p.ItemCd == itemCd);
        }

        public List<WrkSinKouiDetailModel> FindWrkSinKouiDetailByItemCd(int target, List<string> itemCds)
        {
            List<WrkSinKouiDetailModel> ret = FindWrkSinKouiDetail(target);
            return ret.FindAll(p => itemCds.Contains(p.ItemCd));
        }

        /// <summary>
        /// 指定のRpのレコードを返す
        /// </summary>
        /// <param name="rpNo">RP_NO</param>
        /// <param name="seqNo">SEQ_NO</param>
        /// <returns></returns>
        public List<WrkSinKouiDetailModel> FindWrkSinKouiDetailByRp(int rpNo, int seqNo)
        {
            return _wrkSinKouiDetails.FindAll(p =>
                p.HpId == _hpId &&
                p.PtId == _ptId &&
                p.RaiinNo == _raiinNo &&
                p.HokenKbn == _hokenKbn &&
                p.RpNo == rpNo &&
                p.SeqNo == (seqNo == 0 ? p.SeqNo : seqNo) &&
                p.IsDeleted == DeleteStatus.None);
        }

        /// <summary>
        /// 指定の診療行為コードを持つレコードを削除する
        /// </summary>
        /// <param name="itemCds">削除したい診療行為コード</param>
        public void RemoveWrkSinKouiDetailByItemCd(List<string> itemCds)
        {
            _wrkSinKouiDetails.RemoveAll(p =>
                p.HpId == _hpId &&
                p.PtId == _ptId &&
                p.RaiinNo == _raiinNo &&
                p.HokenKbn == _hokenKbn &&
                itemCds.Contains(p.ItemCd));
        }
        #endregion

        #region データ操作 - ワーク診療行為詳細削除関連

        /// <summary>
        /// ワーク診療行為詳細削除を生成する
        /// </summary>
        /// <param name="hokenKbn">保険の区分</param>
        /// <param name="rpNo">RP_NO</param>
        /// <param name="seqNo">SEQ_NO</param>
        /// <param name="rowNo">ROW_NO</param>
        /// <param name="itemCd">算定不可となる診療行為のコード</param>
        /// <param name="delItemCd">診療行為コードを算定不可の原因となる診療行為のコード</param>
        /// <param name="santeiDate">削除項目の算定日</param>
        /// <param name="delSbt">
        /// 削除種別
        ///     0:包括 1:背反
        /// </param>
        /// <param name="isWarning">
        /// 警告
        ///     0:削除 1:警告 2:どちらか1つ 3:どちらか1つの可能性
        /// </param>
        /// <param name="termCnt">
        /// チェック期間数
        ///     TERM_SBTと組み合わせて使用
        ///     ※TERM_SBT in (1,4)のときのみ有効
        ///     例）2日の場合、TERM_CNT=2, TERM_SBT=1と登録
        /// </param>
        /// <param name="termSbt">
        /// チェック期間種別
        ///     0:未指定 1:来院 2:日 3:暦週 4:暦月 5:週 6:月 9:患者あたり
        /// </param>
        /// <returns>ワーク診療行為詳細削除情報</returns>
        public WrkSinKouiDetailDelModel GetWrkSinKouiDetailDel
            (int hokenKbn, int rpNo, int seqNo, int rowNo, string itemCd, string delItemCd, int santeiDate, int delSbt, int isWarning, int termCnt, int termSbt, int isAutoAdd, List<string> delItemCds = null, int hokenId = 0)
        {
            WrkSinKouiDetailDelModel wrkDtlDel =
                new WrkSinKouiDetailDelModel(new WrkSinKouiDetailDel());
            wrkDtlDel.HpId = _hpId;
            wrkDtlDel.PtId = _ptId;
            wrkDtlDel.SinDate = _sinDate;
            wrkDtlDel.RaiinNo = _raiinNo;
            wrkDtlDel.HokenKbn = hokenKbn;
            wrkDtlDel.RpNo = rpNo;
            wrkDtlDel.SeqNo = seqNo;
            wrkDtlDel.RowNo = rowNo;
            wrkDtlDel.ItemCd = itemCd;
            wrkDtlDel.DelItemCds = delItemCds;
            wrkDtlDel.HokenId = hokenId;

            //if(_wrkDtlDelItemCd != itemCd)
            //{
            //    _wrkDtlDelItemCd = itemCd;
            //    _wrkDtlDelItemSeqNo = 0;
            //}

            //_wrkDtlDelItemSeqNo++;

            var maxItemSeqNo =
                wrkSinKouiDetailDels.Where(p =>
                   p.HpId == _hpId &&
                   p.RaiinNo == _raiinNo &&
                   p.HokenKbn == hokenKbn &&
                   p.RpNo == rpNo &&
                   p.SeqNo == seqNo &&
                   p.RowNo == rowNo).DefaultIfEmpty()
                    .Max(p =>
                        p?.ItemSeqNo);

            if (maxItemSeqNo != null)
            {
                wrkDtlDel.ItemSeqNo = (maxItemSeqNo ?? 0) + 1;
            }
            else
            {
                wrkDtlDel.ItemSeqNo = 1;
            }
            //wrkDtlDel.ItemSeqNo = _wrkDtlDelItemSeqNo;
            wrkDtlDel.DelItemCd = delItemCd;
            wrkDtlDel.SanteiDate = santeiDate;
            wrkDtlDel.DelSbt = delSbt;
            wrkDtlDel.IsWarning = isWarning;
            wrkDtlDel.TermCnt = termCnt;
            wrkDtlDel.TermSbt = termSbt;
            wrkDtlDel.IsAutoAdd = isAutoAdd;

            return wrkDtlDel;
        }

        /// <summary>
        /// ワーク診療行為詳細削除を追加する
        /// </summary>
        /// <param name="wrkSinKouiDetailDelModel">ワーク診療行為詳細削除のオブジェクト</param>
        public void AppendWrkSinKouiDetailDel(WrkSinKouiDetailDelModel wrkSinKouiDetailDelModel)
        {
            wrkSinKouiDetailDels.Add(wrkSinKouiDetailDelModel);
        }

        /// <summary>
        /// ワーク診療行為詳細削除を生成して追加する
        /// </summary>
        /// <param name="hokenKbn">保険の区分</param>
        /// <param name="rpNo">RP_NO</param>
        /// <param name="seqNo">SEQ_NO</param>
        /// <param name="rowNo">ROW_NO</param>
        /// <param name="itemCd">算定不可となる診療行為のコード</param>
        /// <param name="delItemCd">診療行為コードを算定不可の原因となる診療行為のコード</param>
        /// <param name="delSbt">
        /// 削除種別
        ///     0:包括 1:背反
        /// </param>
        /// <param name="isWarning">
        /// 警告
        ///     0:削除 1:警告 2:どちらか１つ 3:どちらか1つの可能性
        /// </param>
        /// <param name="termCnt">
        /// チェック期間数
        ///     TERM_SBTと組み合わせて使用
        ///     ※TERM_SBT in (1,4)のときのみ有効
        ///     例）2日の場合、TERM_CNT=2, TERM_SBT=1と登録
        /// </param>
        /// <param name="termSbt">
        /// チェック期間種別
        ///     0:未指定 1:来院 2:日 3:暦週 4:暦月 5:週 6:月 9:患者あたり
        /// </param>
        public void AppendNewWrkSinKouiDetailDel
            (int hokenKbn, int rpNo, int seqNo, int rowNo, string itemCd, string delItemCd, int santeiDate, int delSbt, int isWarning, int termCnt, int termSbt, int isAutoAdd, List<string> delItemCds = null, int hokenId = 0)
        {
            wrkSinKouiDetailDels.Add
                (GetWrkSinKouiDetailDel
                    (hokenKbn, rpNo, seqNo, rowNo, itemCd, delItemCd, santeiDate, delSbt, isWarning, termCnt, termSbt, isAutoAdd, delItemCds, hokenId));
        }

        /// <summary>
        /// ワーク診療行為詳細削除情報を削除する
        /// </summary>
        /// <param name="delItemCd">他の項目を削除する診療行為コード</param>
        /// <param name="termCnt">期間数</param>
        /// <param name="termSbt">期間種別</param>
        public void RemoveWrkSinKOuiDetailDel(string delItemCd, int termCnt, int termSbt)
        {
            _wrkSinKouiDetailDels.RemoveAll(p =>
                p.HpId == _hpId &&
                p.RaiinNo == _raiinNo &&
                p.HokenKbn == _hokenKbn &&
                p.DelItemCd == delItemCd &&
                p.TermCnt == termCnt &&
                p.TermSbt == termSbt);
        }

        #endregion
        #endregion

        /// <summary>
        /// 指定のRp番号の算定区分を返す
        /// </summary>
        /// <param name="rpNo"></param>
        /// <returns></returns>
        public int GetSanteiKbn(long raiinno, int rpNo)
        {
            //int ret = 0;

            int ret = _wrkSinRpInfs.FindAll(p =>
                p.RaiinNo == raiinno &&
                p.HokenKbn == _hokenKbn &&
                p.RpNo == rpNo).FirstOrDefault()?.SanteiKbn ?? 0;

            //if(wrkSinRpInfs.Any())
            //{
            //    ret = wrkSinRpInfs.First().SanteiKbn;
            //}

            return ret;
        }
        public string GetSyukeiSaki(long raiinno, int rpNo, int seqNo)
        {
            //int ret = 0;

            string ret = _wrkSinKouis.FindAll(p =>
                p.RaiinNo == raiinno &&
                p.HokenKbn == _hokenKbn &&
                p.RpNo == rpNo &&
                p.SeqNo == seqNo).FirstOrDefault()?.SyukeiSaki ?? "";

            return ret;
        }
        public string GetCdNo(long raiinno, int rpNo)
        {
            string ret = _wrkSinRpInfs.FindAll(p =>
                p.RaiinNo == raiinno &&
                p.HokenKbn == _hokenKbn &&
                p.RpNo == rpNo).FirstOrDefault()?.CdNo ?? "";

            return ret;
        }
        /// <summary>
        /// WrkDetailDelに存在するかどうかチェックする
        /// </summary>
        /// <param name="rpNo"></param>
        /// <param name="itemCd"></param>
        /// <returns>true: 存在する</returns>
        public bool ExistWrkSinKouiDetailDel(int rpNo, string itemCd)
        {
            //bool ret = false;
            return (_wrkSinKouiDetailDels.Any(p =>
                    p.RaiinNo == _raiinNo &&
                    p.HokenKbn == _hokenKbn &&
                    p.RpNo == rpNo &&
                    p.ItemCd == itemCd &&
                    p.IsWarning == 0));
            //{
            //    ret = true;
            //}
            //return ret;
        }
        public bool ExistWrkSinKouiDetailDel(WrkSinKouiDetailModel wrkDtl)
        {
            //bool ret = false;
            return (_wrkSinKouiDetailDels.Any(p =>
                    p.RaiinNo == _raiinNo &&
                    p.HokenKbn == _hokenKbn &&
                    p.RpNo == wrkDtl.RpNo &&
                    p.SeqNo == wrkDtl.SeqNo &&
                    p.RowNo == wrkDtl.RowNo &&
                    p.IsWarning == 0));
            //{
            //    ret = true;
            //}
            //return ret;
        }
        /// <summary>
        /// 指定の診療日の、指定の診療行為の、算定回数（複数指定）
        /// </summary>
        /// <param name="itemCds">診療行為コード（複数指定）</param>
        /// <param name="santeiKbn">算定区分</param>
        /// <returns>算定回数</returns>
        /// 
        public double WrkCountSinday(string itemCd, List<int> santeiKbns = null, bool isSuryoCount = true)
        {
            List<string> itemCds = new List<string> { itemCd };

            return WrkCountSinday(itemCds, santeiKbns, null, isSuryoCount);
        }

        public double WrkCountSinday(List<string> itemCds, List<int> santeiKbns = null, List<int> hokenKbns = null, bool isSuryoCount = true)
        {
            const string conFncName = nameof(WrkCountSinday);

            int sinYm = SinDate / 100;

            List<int> checkHokenKbns = new List<int>();
            checkHokenKbns.AddRange(checkHokenKbn);

            if (hokenKbns != null)
            {
                checkHokenKbns = hokenKbns;
            }

            List<int> checkSanteiKbns = new List<int>();

            if (santeiKbns == null)
            {
                checkSanteiKbns = checkSanteiKbn;
            }
            else
            {
                checkSanteiKbns = santeiKbns;
            }

            var wrkSinKouis = _wrkSinKouis.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                o.SinDate == SinDate &&
                o.RaiinNo != RaiinNo &&
                o.IsDeleted == DeleteStatus.None);
            if (wrkSinKouis.Any() == false) { return 0; }

            var wrkSinKouiDetails = _wrkSinKouiDetails.FindAll(p =>
                p.HpId == HpId &&
                p.PtId == PtId &&
                p.SinDate == SinDate &&
                itemCds.Contains(p.ItemCd) &&
                p.IsDeleted == DeleteStatus.None &&
                p.FmtKbn != 10
                );
            if (wrkSinKouiDetails.Any() == false) { return 0; }

            var wrkSinRpInfs = _wrkSinRpInfs.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                o.SinDate == SinDate &&
                //o.HokenKbn != 4 &&    自費分も含めるらしい
                checkHokenKbns.Contains(o.HokenKbn) &&
                //o.SanteiKbn == santeiKbn &&
                checkSanteiKbns.Contains(o.SanteiKbn) &&
                o.IsDeleted == DeleteStatus.None
            );

            var joinQuery = (
                from wrkSinKouiDetail in wrkSinKouiDetails
                join wrkSinKoui in wrkSinKouis on
                    new { wrkSinKouiDetail.HpId, wrkSinKouiDetail.PtId, wrkSinKouiDetail.RaiinNo, wrkSinKouiDetail.RpNo, wrkSinKouiDetail.SeqNo } equals
                    new { wrkSinKoui.HpId, wrkSinKoui.PtId, wrkSinKoui.RaiinNo, wrkSinKoui.RpNo, wrkSinKoui.SeqNo }
                join wrkSinRp in wrkSinRpInfs on
                    new { wrkSinKouiDetail.HpId, wrkSinKouiDetail.PtId, wrkSinKouiDetail.RaiinNo, wrkSinKouiDetail.RpNo } equals
                    new { wrkSinRp.HpId, wrkSinRp.PtId, wrkSinRp.RaiinNo, wrkSinRp.RpNo }
                where
                    wrkSinKouiDetail.HpId == HpId &&
                    wrkSinKouiDetail.PtId == PtId &&
                    wrkSinKouiDetail.SinDate == SinDate &&
                    itemCds.Contains(wrkSinKouiDetail.ItemCd) &&
                    wrkSinKoui.SinDate == SinDate &&
                    wrkSinKoui.RaiinNo != RaiinNo
                group new { wrkSinKouiDetail, wrkSinKoui } by new { wrkSinKoui.HpId } into A
                select new { sum = A.Sum(a => (double)a.wrkSinKoui.Count * ((a.wrkSinKouiDetail.Suryo <= 0 || isSuryoCount == false) ? 1 : a.wrkSinKouiDetail.Suryo)) }
            );

            var result = joinQuery.ToList();
            if (result.Any())
            {
                return result.FirstOrDefault().sum;
            }
            else
            {
                return 0;
            }

        }

        public double WrkCountSindayIncThisRaiin(string itemCd, int santeiKbn = 0)
        {
            List<string> itemCds = new List<string> { itemCd };

            return WrkCountSindayIncThisRaiin(itemCds);
        }

        public double WrkCountSindayIncThisRaiin(List<string> itemCds, int santeiKbn = 0)
        {
            const string conFncName = nameof(WrkCountSindayIncThisRaiin);

            int sinYm = SinDate / 100;

            var wrkSinRpInfs = _wrkSinRpInfs.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                o.SinDate == SinDate &&
                //o.HokenKbn != 4 &&    自費分も含めるらしい
                checkHokenKbn.Contains(o.HokenKbn) &&
                //o.SanteiKbn == santeiKbn &&
                checkSanteiKbn.Contains(o.SanteiKbn) &&
                o.IsDeleted == DeleteStatus.None
            );
            var wrkSinKouis = _wrkSinKouis.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                o.SinDate == SinDate &&
                o.IsDeleted == DeleteStatus.None);
            var wrkSinKouiDetails = _wrkSinKouiDetails.FindAll(p =>
                p.HpId == HpId &&
                p.PtId == PtId &&
                p.SinDate == SinDate &&
                itemCds.Contains(p.ItemCd) &&
                p.IsDeleted == DeleteStatus.None
                );

            var joinQuery = (
                from wrkSinKouiDetail in wrkSinKouiDetails
                join wrkSinKoui in wrkSinKouis on
                    new { wrkSinKouiDetail.HpId, wrkSinKouiDetail.PtId, wrkSinKouiDetail.RaiinNo, wrkSinKouiDetail.RpNo, wrkSinKouiDetail.SeqNo } equals
                    new { wrkSinKoui.HpId, wrkSinKoui.PtId, wrkSinKoui.RaiinNo, wrkSinKoui.RpNo, wrkSinKoui.SeqNo }
                join wrkSinRp in wrkSinRpInfs on
                    new { wrkSinKouiDetail.HpId, wrkSinKouiDetail.PtId, wrkSinKouiDetail.RaiinNo, wrkSinKouiDetail.RpNo } equals
                    new { wrkSinRp.HpId, wrkSinRp.PtId, wrkSinRp.RaiinNo, wrkSinRp.RpNo }
                where
                    wrkSinKouiDetail.HpId == HpId &&
                    wrkSinKouiDetail.PtId == PtId &&
                    wrkSinKouiDetail.SinDate == SinDate &&
                    itemCds.Contains(wrkSinKouiDetail.ItemCd) &&
                    wrkSinKoui.SinDate == SinDate
                group new { wrkSinKouiDetail, wrkSinKoui } by new { wrkSinKoui.HpId } into A
                select new { sum = A.Sum(a => (double)a.wrkSinKoui.Count * (a.wrkSinKouiDetail.Suryo <= 0 ? 1 : a.wrkSinKouiDetail.Suryo)) }
            );

            var result = joinQuery.ToList();
            if (result.Any())
            {
                return result.FirstOrDefault().sum;
            }
            else
            {
                return 0;
            }

        }

        /// <summary>
        /// 診療日の指定の包括区分の項目の算定回数を取得する
        /// </summary>
        /// <param name="hokatuKbn">包括区分</param>
        /// <returns>診療日の指定の包括区分の項目の算定回数</returns>
        public double WrkCountByHokatuKbn(int hokatuKbn)
        {
            int sinYm = SinDate / 100;

            var wrkSinRpInfs = _wrkSinRpInfs.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                o.SinDate == SinDate &&
                o.RaiinNo != RaiinNo &&
                //o.HokenKbn != 4 &&    自費分も含めるらしい
                checkHokenKbn.Contains(o.HokenKbn) &&
                //o.SanteiKbn == santeiKbn &&
                checkSanteiKbn.Contains(o.SanteiKbn) &&
                o.IsDeleted == DeleteStatus.None
            );
            var wrkSinKouis = _wrkSinKouis.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                o.SinDate == SinDate &&
                o.RaiinNo != RaiinNo &&
                o.IsDeleted == DeleteStatus.None);
            var wrkSinKouiDetails = _wrkSinKouiDetails.FindAll(p =>
                p.HpId == HpId &&
                p.PtId == PtId &&
                p.SinDate == SinDate &&
                p.RaiinNo != RaiinNo &&
                p.HokatuKbn == hokatuKbn &&
                p.IsDeleted == DeleteStatus.None
                );


            var joinQuery = (
                from wrkSinKouiDetail in wrkSinKouiDetails
                join wrkSinKoui in wrkSinKouis on
                    new { wrkSinKouiDetail.HpId, wrkSinKouiDetail.PtId, wrkSinKouiDetail.RaiinNo, wrkSinKouiDetail.RpNo, wrkSinKouiDetail.SeqNo } equals
                    new { wrkSinKoui.HpId, wrkSinKoui.PtId, wrkSinKoui.RaiinNo, wrkSinKoui.RpNo, wrkSinKoui.SeqNo }
                join wrkSinRp in wrkSinRpInfs on
                    new { wrkSinKouiDetail.HpId, wrkSinKouiDetail.PtId, wrkSinKouiDetail.RaiinNo, wrkSinKouiDetail.RpNo } equals
                    new { wrkSinRp.HpId, wrkSinRp.PtId, wrkSinRp.RaiinNo, wrkSinRp.RpNo }
                where
                    wrkSinKouiDetail.HpId == HpId &&
                    wrkSinKouiDetail.PtId == PtId &&
                    wrkSinKouiDetail.SinDate == SinDate &&
                    wrkSinKouiDetail.HokatuKbn == hokatuKbn &&
                    wrkSinKoui.SinDate == SinDate
                group new { wrkSinKouiDetail, wrkSinKoui } by new { wrkSinKoui.HpId } into A
                select new { sum = A.Sum(a => (double)a.wrkSinKoui.Count * (a.wrkSinKouiDetail.Suryo <= 0 ? 1 : a.wrkSinKouiDetail.Suryo)) }
                );

            var result = joinQuery.ToList();
            if (result.Any())
            {
                return result.FirstOrDefault().sum;
            }
            else
            {
                return 0;
            }
        }

        public double WrkCountByHokatuKbn(int hokatuKbn, string cdKbn, int cdKbnno, int cdEdano, int cdKouno)
        {
            int sinYm = SinDate / 100;

            var wrkSinRpInfs = _wrkSinRpInfs.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                o.SinDate == SinDate &&
                o.RaiinNo != RaiinNo &&
                //o.HokenKbn != 4 &&    自費分も含めるらしい
                checkHokenKbn.Contains(o.HokenKbn) &&
                //o.SanteiKbn == santeiKbn &&
                checkSanteiKbn.Contains(o.SanteiKbn) &&
                o.IsDeleted == DeleteStatus.None
            );
            var wrkSinKouis = _wrkSinKouis.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                o.SinDate == SinDate &&
                o.RaiinNo != RaiinNo &&
                o.IsDeleted == DeleteStatus.None);
            var wrkSinKouiDetails = _wrkSinKouiDetails.FindAll(p =>
                p.HpId == HpId &&
                p.PtId == PtId &&
                p.SinDate == SinDate &&
                p.RaiinNo != RaiinNo &&
                p.HokatuKbn == hokatuKbn &&
                p.CdKbn == cdKbn &&
                p.CdKbnno == cdKbnno &&
                p.CdKouno == cdKouno &&
                p.IsDeleted == DeleteStatus.None
                );


            var joinQuery = (
                from wrkSinKouiDetail in wrkSinKouiDetails
                join wrkSinKoui in wrkSinKouis on
                    new { wrkSinKouiDetail.HpId, wrkSinKouiDetail.PtId, wrkSinKouiDetail.RaiinNo, wrkSinKouiDetail.RpNo, wrkSinKouiDetail.SeqNo } equals
                    new { wrkSinKoui.HpId, wrkSinKoui.PtId, wrkSinKoui.RaiinNo, wrkSinKoui.RpNo, wrkSinKoui.SeqNo }
                join wrkSinRp in wrkSinRpInfs on
                    new { wrkSinKouiDetail.HpId, wrkSinKouiDetail.PtId, wrkSinKouiDetail.RaiinNo, wrkSinKouiDetail.RpNo } equals
                    new { wrkSinRp.HpId, wrkSinRp.PtId, wrkSinRp.RaiinNo, wrkSinRp.RpNo }
                where
                    wrkSinKouiDetail.HpId == HpId &&
                    wrkSinKouiDetail.PtId == PtId &&
                    wrkSinKouiDetail.SinDate == SinDate &&
                    wrkSinKouiDetail.HokatuKbn == hokatuKbn &&
                    wrkSinKoui.SinDate == SinDate
                group new { wrkSinKouiDetail, wrkSinKoui } by new { wrkSinKoui.HpId } into A
                select new { sum = A.Sum(a => (double)a.wrkSinKoui.Count * (a.wrkSinKouiDetail.Suryo <= 0 ? 1 : a.wrkSinKouiDetail.Suryo)) }
                );

            var result = joinQuery.ToList();
            if (result.Any())
            {
                return result.FirstOrDefault().sum;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 検査で使用しているHOKEN_PIDのリスト
        /// </summary>
        public List<int> GetKensaHokenPidList(int hokatuKensa, int santeiKbn)
        {
            List<int> _hokenPids = null;

            if (_hokenPids == null)
            {
                _hokenPids = new List<int>();

                // 検査で使用しているHOKEN_PID
                var rps = wrkSinRpInfs.FindAll(p =>
                                p.HpId == _hpId &&
                                p.PtId == _ptId &&
                                p.RaiinNo == _raiinNo &&
                                //p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                                p.SanteiKbn == santeiKbn &&
                                p.SinId >= OdrKouiKbnConst.KensaMin &&
                                p.SinId <= OdrKouiKbnConst.KensaMax &&
                                p.IsDeleted == DeleteStatus.None);

                var kouis = wrkSinKouis.FindAll(p =>
                                p.HpId == _hpId &&
                                p.PtId == _ptId &&
                                p.RaiinNo == _raiinNo &&
                                checkHokenKbn.Contains(p.HokenKbn) &&
                                p.HokatuKensa == hokatuKensa &&
                                p.IsDeleted == DeleteStatus.None
                                );

                var join = (

                    from rp in rps
                    join koui in kouis on
                        new { rp.HpId, rp.PtId, rp.SinDate, rp.RaiinNo, rp.RpNo } equals
                        new { koui.HpId, koui.PtId, koui.SinDate, koui.RaiinNo, koui.RpNo }
                    group new { koui } by new { koui.HokenPid } into A
                    select new
                    {
                        A.Key.HokenPid
                    }
                    ).ToList();

                join?.ForEach(p =>
                    {
                        _hokenPids.Add(p.HokenPid);
                    }
                );
            }
            return _hokenPids;
        }

        public void Reload()
        {
            foreach (WrkSinRpInfModel wrkSinRpInf in _wrkSinRpInfs)
            {
                try
                {
                    _santeiFinder.WrkSinRpInfReload(wrkSinRpInf.WrkSinRpInf);
                }
                catch
                { }
            }

            foreach (WrkSinKouiModel wrkSinKoui in _wrkSinKouis)
            {
                try
                {
                    _santeiFinder.WrkSinKouiReload(wrkSinKoui.WrkSinKoui);
                }
                catch
                { }
            }

            foreach (WrkSinKouiDetailModel wrkSinDtl in _wrkSinKouiDetails)
            {
                try
                {
                    _santeiFinder.WrkSinKouiDetailReload(wrkSinDtl.WrkSinKouiDetail);
                }
                catch
                { }
            }

            foreach (WrkSinKouiDetailDelModel wrkSinDtlDel in _wrkSinKouiDetailDels)
            {
                try
                {
                    _santeiFinder.WrkSinKouiDetailDelReload(wrkSinDtlDel.WrkSinKouiDetailDel);
                }
                catch
                { }
            }
        }

        public double GetSyoteiTen(long rpno, long seqno, string kokuji1)
        {
            double ret = 0;

            for (int i = _wrkSinKouiDetails.Count - 1; i >= 0; i--)
            {
                if (_wrkSinKouiDetails[i].RpNo != rpno || _wrkSinKouiDetails[i].SeqNo != seqno)
                {
                    break;
                }
                else
                {
                    if (new string[] { "1", "3", "5" }.Contains(_wrkSinKouiDetails[i].Kokuji1))
                    {
                        // 加算項目ではない場合、所定点数に加える（ただし、KOKUJI1=1の場合、加算のこともあるのでKOKUJI2もチェック）

                        ret += _wrkSinKouiDetails[i].Ten;
                    }
                    else if (kokuji1 == "9" &&
                            (_wrkSinKouiDetails[i].TusokuTargetKbn == 0 && new string[] { "7", "A" }.Contains(_wrkSinKouiDetails[i].Kokuji1)))
                    {
                        // 通則加算の場合、注加算含む（手術だけかもしれない？）
                        if (!((_wrkSinKouiDetails[i].Kokuji1 == "9") &&
                            (_wrkSinKouiDetails[i].TenId == 5 || _wrkSinKouiDetails[i].TenId == 6)))
                        {
                            // でも、率加算は除く
                            ret += _wrkSinKouiDetails[i].Ten;
                        }
                    }
                    else if (_wrkSinKouiDetails[i].MasterSbt == "Y" || _wrkSinKouiDetails[i].MasterSbt == "T")
                    {
                        // 薬剤、特材が間にあったら抜ける
                        break;
                    }
                }
            }

            return ret;
        }

        public int GetWrkKouiHokenId(int rpno, int seqno)
        {
            int ret = 0;

            if(_wrkSinKouis.Any(p=>
                p.HpId == HpId &&
                p.PtId == PtId &&
                p.SinDate == SinDate &&
                p.RaiinNo == RaiinNo &&
                p.RpNo == rpno &&
                p.SeqNo == seqno &&
                p.IsDeleted == DeleteStatus.None))
            {
                ret = _wrkSinKouis.Find(p =>
                    p.HpId == HpId &&
                    p.PtId == PtId &&
                    p.SinDate == SinDate &&
                    p.RaiinNo == RaiinNo &&
                    p.RpNo == rpno &&
                    p.SeqNo == seqno &&
                    p.IsDeleted == DeleteStatus.None).HokenId;
            }

            return ret;
        }
    }
}
