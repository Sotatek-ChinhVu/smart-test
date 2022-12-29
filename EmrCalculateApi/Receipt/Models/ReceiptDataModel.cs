using EmrCalculateApi.Ika.Models;
using EmrCalculateApi.Receipt.Constants;
using Helper.Constants;
using Helper.Common;
using EmrCalculateApi.Interface;

namespace EmrCalculateApi.Receipt.Models
{
    public class ReceiptDataModel
    {
        private const string ModuleName = ModuleNameConst.EmrCalculateReceipt;

        private ReceInfModel _receInfModel;
        private PtInfModel _ptInfModel;
        private HokenDataModel _hokenDataModel;
        private List<KohiDataModel> _kohiDataModels;
        private List<SyobyoDataModel> _syobyoDataModels;
        private List<SinMeiDataModel> _sinMeiDataModels;
        private List<SyojyoSyokiModel> _syojyoSyokiModels;
        private PtKyuseiModel _ptKyuseiModel;
        private RousaiReceiptModel _rousaiReceiptModel;
        private AfterCareReceiptModel _afterCareReceiptModel;
        private SyobyoKeikaModel _syobyoKeikaModel;
        private RecedenRirekiInfModel _recedenRirekiInfModel;
        private SikakuJyohoDataModel _sikakuJyohoModel;
        private List<JyusinbiDataModel> _jyusinbiDataModels;
        private MadoguchiFutanDataModel _madoguchiFutanDataModel;

        private int _outputYm;

        private int _receiptNo;
        private int _futanKbn;

        private int _recordYm;

        IEmrLogger _emrLogger;

        /// <summary>
        /// レセプト情報
        /// </summary>
        /// <param name="receInf">レセプト情報</param>
        /// <param name="ptInfModel">患者基本情報</param>
        /// <param name="hokenDataModel">保険情報</param>
        /// <param name="kohiDataModels">公費情報</param>
        /// <param name="syobyoDataModels">傷病情報</param>
        /// <param name="sinMeiDataModels">診療明細情報</param>
        /// <param name="syojyoSyokiModels">症状詳記</param>
        /// <param name="ptKyuseiModel">旧姓情報</param>
        /// <param name="rousaiReceiptModel">労災レセプト情報（労災のみ）</param>
        /// <param name="syobyoKeikaModel">傷病の経過（労災のみ）</param>
        /// <param name="recedenRirekiInfModel">返戻履歴情報</param>
        public ReceiptDataModel(
            ReceInfModel receInf, PtInfModel ptInfModel, HokenDataModel hokenDataModel, List<KohiDataModel> kohiDataModels, 
            List<SyobyoDataModel> syobyoDataModels, List<SinMeiDataModel> sinMeiDataModels, List<SyojyoSyokiModel> syojyoSyokiModels,
            PtKyuseiModel ptKyuseiModel, RousaiReceiptModel rousaiReceiptModel, SyobyoKeikaModel syobyoKeikaModel,
            RecedenRirekiInfModel recedenRirekiInfModel, int outputYm, AfterCareReceiptModel afterCareReceiptModel, 
            SikakuJyohoDataModel sikakuJyohoDataModel, List<JyusinbiDataModel> jyusinbiDataModels, MadoguchiFutanDataModel madoguchiFutanDataModel,
            IEmrLogger emrLogger)
        {
            _receInfModel = receInf;
            _ptInfModel = ptInfModel;
            _hokenDataModel = hokenDataModel;
            _kohiDataModels = kohiDataModels;
            _syobyoDataModels = syobyoDataModels;
            _sinMeiDataModels = sinMeiDataModels;
            _syojyoSyokiModels = syojyoSyokiModels;
            _ptKyuseiModel = ptKyuseiModel;
            _rousaiReceiptModel = rousaiReceiptModel;
            _syobyoKeikaModel = syobyoKeikaModel;
            _recedenRirekiInfModel = recedenRirekiInfModel;
            _outputYm = outputYm;
            _afterCareReceiptModel = afterCareReceiptModel;
            _sikakuJyohoModel = sikakuJyohoDataModel;
            _jyusinbiDataModels = jyusinbiDataModels;
            _madoguchiFutanDataModel = madoguchiFutanDataModel;

            _emrLogger = emrLogger;
        }

        /// <summary>
        /// レコード識別
        /// </summary>
        public string RecId
        {
            get { return "RE"; }
        }
        /// <summary>
        /// レセプト番号
        /// </summary>
        public int ReceiptNo
        {
            get { return _receiptNo; }
            set { _receiptNo = value; }
        }
        /// <summary>
        /// レセプト種別
        /// </summary>
        public string ReceiptSbt
        {
            get { return _receInfModel.ReceSbt ?? string.Empty; }
        }
        /// <summary>
        /// 診療年月
        /// </summary>
        public int SinYm
        {
            get { return _receInfModel.SinYm; }
        }
        /// <summary>
        /// 患者氏名
        /// </summary>
        public string PtName
        {
            get
            {
                string ret = _ptInfModel.Name ?? string.Empty;
                if(_ptKyuseiModel != null)
                {
                    ret = _ptKyuseiModel.Name;
                }

                return ret.Replace(" ", "　");
            }
        }
        /// <summary>
        /// 性別
        /// </summary>
        public int Sex
        {
            get { return _ptInfModel.Sex; }
        }
        /// <summary>
        /// 生年月日
        /// </summary>
        public int BirthDay
        {
            get { return _ptInfModel.Birthday; }
        }
        /// <summary>
        /// 給付割合
        /// </summary>
        public int KyufuRate
        {
            get { return 100 - _receInfModel.HokenRate; }
        }
        /// <summary>
        /// 負担区分
        /// </summary>
        public int FutanKbn
        {
            get { return _futanKbn; }
            set { _futanKbn = value; }
        }
        /// <summary>
        /// 特記事項
        /// </summary>
        public string Tokki
        {
            get{ return _receInfModel.Tokki ?? string.Empty; }
        }

        /// <summary>
        /// 一部負担金・食事療養費・生活療養費標準負担額区分
        /// </summary>
        public int IchibuKbn
        {
            get
            {
                int ret = 0;

                if(_receInfModel.KogakuOverKbn == 1)
                {
                    switch(_receInfModel.KogakuKbn )
                    {
                        case 4:
                            ret = 1;
                            break;
                        case 5:
                            ret = 3;
                            break;
                    }
                }

                return ret;
            }
        }
        /// <summary>
        /// カルテID
        /// </summary>
        public long KarteId
        {
            //get { return _receInfModel.PtId; }
            get { return _receInfModel.PtNum; }
        }
        
        /// <summary>
        /// 検索番号
        /// </summary>
        public string SearchNo
        {
            get
            {
                string ret = "";

                if(_receInfModel.SeikyuKbn == SeikyuKbnConst.OnlineHenrei)
                {
                    if(_recedenRirekiInfModel != null)
                    {
                        ret = _recedenRirekiInfModel.SearchNo;
                    }
                }

                return ret;
            }
        }
        /// <summary>
        /// 記録条件仕様年月
        /// </summary>
        public int RecordYm
        {
            get { return _recordYm; }
            set { _recordYm = value; }
        }
        /// <summary>
        /// 請求情報
        /// </summary>
        public string SeikyuInf
        {
            get { return string.Format("{0:D10}{1:D6}{2:D5}", _receInfModel.PtId, _receInfModel.SinYm, _receInfModel.HokenId); }
        }
        /// <summary>
        /// 患者カタカナ氏名
        /// </summary>
        public string PtKanaName
        {
            get
            {
                string ret = _ptInfModel.KanaName ?? string.Empty;

                if(_ptKyuseiModel != null)
                {
                    ret = _ptKyuseiModel.KanaName;
                }
                return ret;
            }
        }

        /// <summary>
        /// 患者の状態
        /// </summary>
        public string PtStatus
        {
            get { return _receInfModel.PtStatus ?? string.Empty; }
        }

        /// <summary>
        /// 傷病の経過
        /// </summary>
        public string SyobyoKeika
        {
            get
            {
                string ret = "";
                if (_syobyoKeikaModel != null)
                {
                    ret = _syobyoKeikaModel.Keika;
                }

                return ret;
            }
        }

        /// <summary>
        /// 労働局コード
        /// </summary>
        public string RoudouCd
        {
            get
            {
                string ret = "";
                if (_receInfModel != null && _receInfModel.PtHokenInf != null)
                {
                    ret = _receInfModel.PtHokenInf.RousaiRoudouCd ?? string.Empty;
                }
                return ret;
            }
        }
        /// <summary>
        /// 監督署コード
        /// </summary>
        public string KantokuCd
        {
            get
            {
                string ret = "";
                if (_receInfModel != null && _receInfModel.PtHokenInf != null)
                {
                    ret = _receInfModel.PtHokenInf.RousaiKantokuCd ?? string.Empty;
                }
                return ret;
            }
        }
        /// <summary>
        /// 円点レート
        /// </summary>
        public int EnTen
        {
            get
            {
                int ret = 0;

                if (_receInfModel != null && _receInfModel.HokenMst != null)
                {
                    ret = _receInfModel.HokenMst.EnTen;
                }

                return ret;
            }
        }

        /// <summary>
        /// REレコード
        /// </summary>
        public string RERecord
        {
            get
            {
                // 診療年月を取得
                int _getSinryoYm()
                {
                    int result = SinYm;
                    if(_outputYm < 202005)
                    {
                        result = CIUtil.SDateToWDate(SinYm * 100 + 1) / 100;
                    }
                    return result;
                }

                // 生年月日を取得
                int _getBirthday()
                {
                    int result = BirthDay;

                    if(_outputYm < 202005)
                    {
                        result = CIUtil.SDateToWDate(BirthDay);
                    }

                    return result;
                }

                string ret = "";

                // レコード識別情報
                ret = RecId;
                // レセプト番号
                ret += "," + ReceiptNo;
                // レセプト種別
                ret += "," + ReceiptSbt;

                // 診療年月
                //ret += "," + (CIUtil.SDateToWDate(SinYm * 100 + 1) / 100).ToString();
                ret += "," + (_getSinryoYm()).ToString();

                // 氏名
                string retText = "";
                string badText = "";
                string addName = CIUtil.ReplaceHiraDakuten(CIUtil.ToWide(PtName));

                if(CIUtil.IsUntilJISKanjiLevel2(addName, ref retText, ref badText) == false)
                {
                    // 第二水準ではない場合はカタカナ氏名
                    addName = CIUtil.ToWide(PtKanaName).Replace("゛", "");
                    
                    _emrLogger.WriteLogMsg(this, "RERecord", string.Format("PtName is include bad character PtId:{0} badCharacters:{1}", _ptInfModel.PtId,badText));
                }

                ret += "," + CIUtil.Copy(addName, 1, 20);

                // 性別
                ret += "," + Sex;
                // 生年月日
                ret += "," + (_getBirthday()).ToString();
                // 給付割合
                if (_receInfModel.HokenKbn == 2)
                {
                    //　国保の場合、出力
                    //if (_receInfModel.HokensyaNo.Length == 8 && _receInfModel.HokensyaNo.StartsWith("39"))
                    if(_receInfModel.Houbetu == "39")
                    {
                        // 後期高齢者は不要
                        ret += ",";
                    }
                    else
                    {
                        if (KyufuRate == 90 &&
                            ReceiptSbt.ToString().Substring(1, 1) == "1" &&
                           ReceiptSbt.ToString().Substring(3, 1) == "8")
                        {
                            // 前期高齢者2割の場合
                            ret += ",80";
                        }
                        else
                        {
                            ret += "," + CIUtil.ToStringIgnoreZero(KyufuRate);
                        }
                    }
                }
                else
                {
                    ret += ",";
                }
                // 入院年月日
                ret += ",";
                // 病棟区分
                ret += ",";
                // 一部負担金・食事療養費・生活療養費標準負担額区分
                ret += "," + CIUtil.ToStringIgnoreZero(IchibuKbn);
                // レセプト特記事項
                ret += "," + Tokki;
                // 病床数
                ret += ",";
                // カルテ番号等
                ret += "," + KarteId;
                // 割引点数単価
                ret += ",";
                // 予備
                ret += ",";
                // 予備
                ret += ",";
                // 予備
                ret += ",";
                // 検索番号
                ret += "," + SearchNo;
                // 記録条件仕様年月情報
                ret += "," + CIUtil.ToStringIgnoreZero(RecordYm);
                // 請求情報
                ret += "," + SeikyuInf;
                // 診療科１ 診療科名
                ret += ",";
                // 診療科１ 人体の部位等
                ret += ",";
                // 診療科１ 性別等
                ret += ",";
                // 診療科１ 医学的処置
                ret += ",";
                // 診療科１ 特定疾病
                ret += ",";
                // 診療科２ 診療科名
                ret += ",";
                // 診療科２ 人体の部位等
                ret += ",";
                // 診療科２ 性別等
                ret += ",";
                // 診療科２ 医学的処置
                ret += ",";
                // 診療科２ 特定疾病
                ret += ",";
                // 診療科３ 診療科名
                ret += ",";
                // 診療科３ 人体の部位等
                ret += ",";
                // 診療科３ 性別等
                ret += ",";
                // 診療科３ 医学的処置
                ret += ",";
                // 診療科３ 特定疾病
                ret += ",";
                // カタカナ（氏名）
                ret += "," + CIUtil.Copy(CIUtil.ToWide(CIUtil.GetKatakana(PtKanaName.Replace("　", "").Replace(" ", ""))).Replace("゛", ""), 1, 40);
                // 患者の状態
                ret += "," + PtStatus;

                return ret;
            }
        }

        /// <summary>
        /// REレコード
        /// </summary>
        public string RousaiRERecord
        {
            get
            {

                // 生年月日を取得
                int _getBirthday()
                {
                    int result = BirthDay;

                    if (_outputYm < 202005)
                    {
                        result = CIUtil.SDateToWDate(BirthDay);
                    }

                    return result;
                }

                string ret = "";

                // レコード識別情報
                ret = RecId;
                // レセプト番号
                ret += "," + ReceiptNo;
                // 予備
                ret += ",";

                // 予備
                ret += ",";
                // 氏名
                string retText = "";
                string badText = "";
                string addName = CIUtil.ReplaceHiraDakuten(CIUtil.ToWide(PtName));

                if (CIUtil.IsUntilJISKanjiLevel2(addName, ref retText, ref badText) == false)
                {
                    // 第二水準ではない場合はカタカナ氏名
                    addName = CIUtil.ToWide(CIUtil.GetKatakana(CIUtil.GetKatakana(PtKanaName))).Replace("゛", "");
                    _emrLogger.WriteLogMsg( this, "RERecord", string.Format("PtName is include bad character PtId:{0} badCharacters:{1}", _ptInfModel.PtId, badText));
                }
                
                ret += "," + CIUtil.Copy(addName, 1, 20);

                // 性別
                ret += "," + Sex;
                // 生年月日
                ret += "," + (_getBirthday()).ToString();
                
                // 予備
                ret += ",";
                
                // 入院年月日
                ret += ",";
                // 病棟区分
                ret += ",";
                // 予備
                ret += ",";
                // 予備
                ret += ",";
                // 病床数
                ret += ",";
                // カルテ番号等
                ret += "," + KarteId;
                // 予備
                ret += ",";
                // 予備
                ret += ",";
                // 予備
                ret += ",";
                // 予備
                ret += ",";
                // 電算処理受付番号
                ret += "," + SearchNo;
                // 記録条件仕様年月情報
                ret += "," + CIUtil.ToStringIgnoreZero(RecordYm);
                // 請求情報
                ret += "," + SeikyuInf;
                // 診療科１ 診療科名
                ret += ",";
                // 診療科１ 人体の部位等
                ret += ",";
                // 診療科１ 性別等
                ret += ",";
                // 診療科１ 医学的処置
                ret += ",";
                // 診療科１ 特定疾病
                ret += ",";
                // 診療科２ 診療科名
                ret += ",";
                // 診療科２ 人体の部位等
                ret += ",";
                // 診療科２ 性別等
                ret += ",";
                // 診療科２ 医学的処置
                ret += ",";
                // 診療科２ 特定疾病
                ret += ",";
                // 診療科３ 診療科名
                ret += ",";
                // 診療科３ 人体の部位等
                ret += ",";
                // 診療科３ 性別等
                ret += ",";
                // 診療科３ 医学的処置
                ret += ",";
                // 診療科３ 特定疾病
                ret += ",";

                if (_receInfModel.SeikyuYm >= 201805)
                {
                    // 予備
                    ret += ",";
                    // 患者の状態
                    ret += "," + PtStatus;
                }

                return ret;
            }
        }
        /// <summary>
        /// REレコード
        /// </summary>
        public string AfterCareRERecord
        {
            get
            {

                // 生年月日を取得
                int _getBirthday()
                {
                    int result = BirthDay;

                    if (_outputYm < 202005)
                    {
                        result = CIUtil.SDateToWDate(BirthDay);
                    }

                    return result;
                }

                string ret = "";

                // レコード識別情報
                ret = RecId;
                // レセプト番号
                ret += "," + ReceiptNo;
                // 予備
                ret += ",";

                // 予備
                ret += ",";
                // 氏名
                string retText = "";
                string badText = "";
                string addName = CIUtil.ReplaceHiraDakuten(CIUtil.ToWide(PtName));

                if (CIUtil.IsUntilJISKanjiLevel2(addName, ref retText, ref badText) == false)
                {
                    // 第二水準ではない場合はカタカナ氏名
                    addName = CIUtil.ToWide(CIUtil.GetKatakana(CIUtil.GetKatakana(PtKanaName))).Replace("゛", "");
                    _emrLogger.WriteLogMsg( this, "RERecord", string.Format("PtName is include bad character PtId:{0} badCharacters:{1}", _ptInfModel.PtId, badText));
                }

                ret += "," + CIUtil.Copy(addName, 1, 20);

                // 性別
                ret += "," + Sex;
                // 生年月日
                ret += "," + (_getBirthday()).ToString();

                // 予備３
                ret += ",";
                // 予備１１
                ret += ",";
                // 予備１２
                ret += ",";
                // 予備４
                ret += ",";
                // 予備５
                ret += ",";
                // 病床数
                ret += ",";
                // カルテ番号等
                ret += "," + KarteId;
                // 予備６
                ret += ",";
                // 予備７
                ret += ",";
                // 予備８
                ret += ",";
                // 予備９
                ret += ",";
                // 電算処理受付番号
                ret += "," + SearchNo;
                // 予備１３
                ret += ",";
                // 請求情報
                ret += "," + SeikyuInf;
                // 診療科１ 診療科名
                ret += ",";
                // 診療科１ 人体の部位等
                ret += ",";
                // 診療科１ 性別等
                ret += ",";
                // 診療科１ 医学的処置
                ret += ",";
                // 診療科１ 特定疾病
                ret += ",";
                // 診療科２ 診療科名
                ret += ",";
                // 診療科２ 人体の部位等
                ret += ",";
                // 診療科２ 性別等
                ret += ",";
                // 診療科２ 医学的処置
                ret += ",";
                // 診療科２ 特定疾病
                ret += ",";
                // 診療科３ 診療科名
                ret += ",";
                // 診療科３ 人体の部位等
                ret += ",";
                // 診療科３ 性別等
                ret += ",";
                // 診療科３ 医学的処置
                ret += ",";
                // 診療科３ 特定疾病
                ret += ",";
                // 予備１０
                ret += ",";
                // 患者の状態
                ret += "," + PtStatus;

                return ret;
            }
        }
        /// <summary>
        /// レセプトデータ
        /// </summary>
        public string RecedenRecord
        {
            get
            {
                string ret = "";

                // RE
                ret += RERecord;

                // HO
                if (_hokenDataModel != null && _hokenDataModel.HokenNo != 100)
                {
                    ret += "\r\n" + _hokenDataModel.HoRecord;
                }

                // KO
                if(_kohiDataModels != null && _kohiDataModels.Any())
                {
                    foreach(KohiDataModel kohiDataModel in _kohiDataModels)
                    {
                        ret += "\r\n" + kohiDataModel.KoRecord;
                    }
                }

                // SN
                if(_sikakuJyohoModel != null && _hokenDataModel != null && _hokenDataModel.HokenNo != 100)
                {
                    ret += "\r\n" + _sikakuJyohoModel.SNRecord;
                }

                // JD
                if (_jyusinbiDataModels != null && _jyusinbiDataModels.Any())
                {
                    foreach (JyusinbiDataModel jyusinbiDataModel in _jyusinbiDataModels)
                    {
                        ret += "\r\n" + jyusinbiDataModel.JDRecord;
                    }
                }

                // MF
                if(_madoguchiFutanDataModel != null)
                {
                    ret += "\r\n" + _madoguchiFutanDataModel.MFRecord;
                }

                // SY
                if(_syobyoDataModels != null && _syobyoDataModels.Any())
                {
                    foreach(SyobyoDataModel syobyoDataModel in _syobyoDataModels)
                    {
                        string buf = syobyoDataModel.SYRecord;
                        if (buf != "")
                        {
                            ret += "\r\n" + buf;
                        }
                    }
                }

                // SI, IY, TO, CO
                if(_sinMeiDataModels != null && _sinMeiDataModels.Any())
                {
                    foreach (SinMeiDataModel sinMeiDataModel in _sinMeiDataModels)
                    {
                        string buf = sinMeiDataModel.RecedenRecord;
                        if (buf != "")
                        {
                            ret += "\r\n" + buf;
                        }
                    }
                }

                // SJ
                if (_syojyoSyokiModels != null && _syojyoSyokiModels.Any())
                {
                    foreach (SyojyoSyokiModel syojyoSyoki in _syojyoSyokiModels)
                    {
                        string buf = syojyoSyoki.SJRecord();
                        if (buf != "")
                        {
                            ret += "\r\n" + buf;
                        }
                    }
                }

                if(_receInfModel.SeikyuKbn == SeikyuKbnConst.OnlineHenrei)
                {
                    // オンライン返戻の場合、履歴を付ける
                    if (_recedenRirekiInfModel != null)
                    {
                        ret += "\r\n" + _recedenRirekiInfModel.Rireki;

                        // 最後の改行をカットする
                        while (ret.EndsWith("\r\n"))
                        {
                            ret = ret.Substring(0, ret.Length - 2);
                        }
                    }
                }
                return ret;
            }
        }

        public string RousaiRecedenRecord
        {
            get
            {
                string ret = "";

                // RE
                ret += RousaiRERecord;

                //傷病の経過
                string keika = "";
                bool keikaOutput = false;

                if (_syobyoKeikaModel != null)
                {
                    keika = CIUtil.ToRecedenString(_syobyoKeikaModel.Keika);

                    if(keika.EndsWith("\r\n"))
                    {
                        keika = keika.Substring(0, keika.Length - 2);
                    }

                    string retText = "";
                    string badText = "";

                    if (CIUtil.IsUntilJISKanjiLevel2(keika, ref retText, ref badText) == false)
                    {
                        // 第二水準は除く
                        keika = retText;
                        _emrLogger.WriteLogMsg( this, "RERecord", string.Format("SyobyoKeika is include bad character PtId:{0} badCharacters:{1}", _ptInfModel.PtId, badText));
                    }
                }
                
                // RR
                if (_rousaiReceiptModel != null)
                {
                    if(keika.Length <= 50 && !keika.Contains("\r\n"))
                    {
                        _rousaiReceiptModel.SyobyoKeika = keika;
                        keikaOutput = true;
                    }
                    else
                    {
                        _rousaiReceiptModel.SyobyoKeika = "症状詳記に記載"; 
                    }

                    ret += "\r\n" + _rousaiReceiptModel.RRRecord;
                
                }

                // SY
                if (_syobyoDataModels != null && _syobyoDataModels.Any())
                {
                    foreach (SyobyoDataModel syobyoDataModel in _syobyoDataModels)
                    {
                        string buf = syobyoDataModel.RousaiSYRecord;
                        if (buf != "")
                        {
                            ret += "\r\n" + buf;
                        }
                    }
                }

                // RI, IY, TO, CO
                if (_sinMeiDataModels != null && _sinMeiDataModels.Any())
                {
                    foreach (SinMeiDataModel sinMeiDataModel in _sinMeiDataModels)
                    {
                        string buf = sinMeiDataModel.RousaiRecedenRecord;
                        if (buf != "")
                        {
                            ret += "\r\n" + buf;
                        }
                    }
                }

                // SJ
                if (keikaOutput == false)
                {
                    // 傷病の経過がある場合はSJに追加する
                    string syobyoSj = GetSyoubyoKeikaSJ(0, keika);
                    //const int conMaxLength = 1200;

                    //keika = "【傷病の経過】\r\n" + keika;
                    //string[] tmpLines = keika.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                    //List<string> lines = new List<string>();

                    //bool first = true;
                    //for(int i = 0; i < tmpLines.Count(); i++)
                    //{
                    //    if(first && tmpLines[i].Trim() == "")
                    //    {
                    //        // 先頭の空行はカット
                    //    }
                    //    else
                    //    {
                    //        first = false;
                    //        lines.Add(tmpLines[i]);
                    //    }
                    //}

                    //// 最後の空行はカット
                    //if (lines.Count() > 0)
                    //{
                    //    while (lines[lines.Count() - 1].Trim() == "")
                    //    {
                    //        lines.RemoveAt(lines.Count() - 1);
                    //    }
                    //}

                    //string syobyoSj = "";

                    //for (int i = 0; i < lines.Count(); i++)
                    //{
                    //    if (lines[i] == "")
                    //    {
                    //        // 改行だけの行
                    //        if (syobyoSj != "")
                    //        {
                    //            syobyoSj += "\r\n" + "SJ,,";
                    //        }
                    //        else
                    //        {
                    //            syobyoSj += "SJ,07,";
                    //        }
                    //    }
                    //    else
                    //    {
                    //        while (lines[i] != "")
                    //        {
                    //            string tmp = lines[i];
                    //            if (tmp.Length > conMaxLength)
                    //            {
                    //                tmp = lines[i].Substring(0, conMaxLength);
                    //            }
                    //            // コメント情報
                    //            if (syobyoSj != "")
                    //            {
                    //                syobyoSj += "\r\n" + "SJ,," + tmp;
                    //            }
                    //            else
                    //            {
                    //                syobyoSj += "SJ,07," + tmp;
                    //            }

                    //            lines[i] = CIUtil.Copy(lines[i], tmp.Length + 1, lines[i].Length - tmp.Length);
                    //        }
                    //    }
                    //}

                    if (syobyoSj != "")
                    {
                        ret += "\r\n" + syobyoSj;
                    }
                }

                if (_syojyoSyokiModels != null && _syojyoSyokiModels.Any())
                {
                    foreach (SyojyoSyokiModel syojyoSyoki in _syojyoSyokiModels)
                    {
                        string buf = syojyoSyoki.SJRecord();
                        if (buf != "")
                        {
                            ret += "\r\n" + buf;
                        }
                    }
                }

                return ret;
            }
        }
        /// <summary>
        /// GetSyoubyoKeikaSJ
        /// </summary>
        /// <param name="mode">1:アフターケア(詳記区分なし)</param>
        /// <param name="keika"></param>
        /// <returns></returns>
        private string GetSyoubyoKeikaSJ(int mode, string keika)
        {
            string syokiKbn = "07";
            if(mode == 1)
            {
                syokiKbn = "";
            }

            // 傷病の経過がある場合はSJに追加する
            const int conMaxLength = 1200;

            keika = "【傷病の経過】\r\n" + keika;
            string[] tmpLines = keika.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            List<string> lines = new List<string>();

            bool first = true;
            for (int i = 0; i < tmpLines.Count(); i++)
            {
                if (first && tmpLines[i].Trim() == "")
                {
                    // 先頭の空行はカット
                }
                else
                {
                    first = false;
                    lines.Add(tmpLines[i]);
                }
            }

            // 最後の空行はカット
            if (lines.Count() > 0)
            {
                while (lines[lines.Count() - 1].Trim() == "")
                {
                    lines.RemoveAt(lines.Count() - 1);
                }
            }

            string syobyoSj = "";

            for (int i = 0; i < lines.Count(); i++)
            {
                if (lines[i] == "")
                {
                    // 改行だけの行
                    if (syobyoSj != "")
                    {
                        syobyoSj += "\r\n" + "SJ,,";
                    }
                    else
                    {
                        syobyoSj += $"SJ,{syokiKbn},";
                    }
                }
                else
                {
                    while (lines[i] != "")
                    {
                        string tmp = lines[i];
                        if (tmp.Length > conMaxLength)
                        {
                            tmp = lines[i].Substring(0, conMaxLength);
                        }
                        // コメント情報
                        if (syobyoSj != "")
                        {
                            syobyoSj += "\r\n" + "SJ,," + tmp;
                        }
                        else
                        {
                            syobyoSj += $"SJ,{syokiKbn}," + tmp;
                        }

                        lines[i] = CIUtil.Copy(lines[i], tmp.Length + 1, lines[i].Length - tmp.Length);
                    }
                }
            }

            return syobyoSj;
        }

        public string AfterCareRecedenRecord
        {
            get
            {
                string ret = "";

                // RE
                ret += AfterCareRERecord;

                //傷病の経過
                string keika = "";
                bool keikaOutput = false;

                if (_syobyoKeikaModel != null)
                {
                    keika = CIUtil.ToRecedenString(_syobyoKeikaModel.Keika);

                    if (keika.EndsWith("\r\n"))
                    {
                        keika = keika.Substring(0, keika.Length - 2);
                    }

                    string retText = "";
                    string badText = "";

                    if (CIUtil.IsUntilJISKanjiLevel2(keika, ref retText, ref badText) == false)
                    {
                        // 第二水準は除く
                        keika = retText;
                        _emrLogger.WriteLogMsg( this, "RERecord", string.Format("SyobyoKeika is include bad character PtId:{0} badCharacters:{1}", _ptInfModel.PtId, badText));
                    }
                }

                // AR
                if (_afterCareReceiptModel != null)
                {
                    if (keika.Length <= 50 && !keika.Contains("\r\n"))
                    {
                        _afterCareReceiptModel.SyobyoKeika = keika;
                        keikaOutput = true;
                    }
                    else
                    {
                        _afterCareReceiptModel.SyobyoKeika = "症状詳記に記載";
                    }

                    ret += "\r\n" + _afterCareReceiptModel.ARRecord;

                }

                //// SY
                //if (_syobyoDataModels != null && _syobyoDataModels.Any())
                //{
                //    foreach (SyobyoDataModel syobyoDataModel in _syobyoDataModels)
                //    {
                //        string buf = syobyoDataModel.RousaiSYRecord;
                //        if (buf != "")
                //        {
                //            ret += "\r\n" + buf;
                //        }
                //    }
                //}

                // RI, IY, TO, CO
                if (_sinMeiDataModels != null && _sinMeiDataModels.Any())
                {
                    foreach (SinMeiDataModel sinMeiDataModel in _sinMeiDataModels)
                    {
                        string buf = sinMeiDataModel.AfterCareRecedenRecord;
                        if (buf != "")
                        {
                            ret += "\r\n" + buf;
                        }
                    }
                }

                // SJ
                if (keikaOutput == false)
                {
                    // 傷病の経過がある場合はSJに追加する
                    string syobyoSj = GetSyoubyoKeikaSJ(1, keika);

                    if (syobyoSj != "")
                    {
                        ret += "\r\n" + syobyoSj;
                    }
                }

                if (_syojyoSyokiModels != null && _syojyoSyokiModels.Any())
                {
                    foreach (SyojyoSyokiModel syojyoSyoki in _syojyoSyokiModels)
                    {
                        string buf = syojyoSyoki.SJRecord(1);
                        if (buf != "")
                        {
                            ret += "\r\n" + buf;
                        }
                    }
                }

                return ret;
            }
        }
        public int RecordCount
        {
            get
            {
                int ret = 0;
                if (_hokenDataModel != null && _hokenDataModel.HokenNo != 100)
                {
                    ret++;
                }

                if (_kohiDataModels != null && _kohiDataModels.Any())
                {
                    ret += _kohiDataModels.Count();
                }

                return ret;
            }
        }

        public int TotalTen
        {
            get
            {
                int ret = 0;
                ret = _receInfModel.Tensu;
                //if (_hokenDataModel != null && _hokenDataModel.HokenNo != 100)
                //{
                //    ret += _hokenDataModel.TotalTen ?? 0;
                //}
                //else if (_kohiDataModels != null && _kohiDataModels.Any())
                //{                    
                //    foreach (KohiDataModel kohiDataModel in _kohiDataModels)
                //    {
                //        ret += kohiDataModel.TotalTen ?? 0;
                //    }
                //}

                return ret;

            }
        }

        /// <summary>
        /// 労災用合計金額
        /// </summary>
        public int TotalKingaku
        {
            get
            {
                return _receInfModel.RousaiIFutan + _receInfModel.RousaiRoFutan;
            }
        }

        public int TotalKingakuAfter
        {
            get
            {
                return _afterCareReceiptModel.Gokei;
            }
        }
        /// <summary>
        /// 診療明細の数を返す
        /// </summary>
        public int SinMeiCount
        {
            get { return _sinMeiDataModels.Count(); }
        }

        public int SinDate
        {
            get
            {
                int ret = 0;
                if(_afterCareReceiptModel != null)
                {
                    ret = _afterCareReceiptModel.SinDate;
                }
                return ret;
            }
            set
            {
                if (_afterCareReceiptModel != null)
                {
                    _afterCareReceiptModel.SinDate = value;
                }
            }
        }
        public int? AfterSyokei
        {
            get
            {
                int? ret = 0;
                if (_afterCareReceiptModel != null)
                {
                    ret = _afterCareReceiptModel.Syokei;
                }
                return ret;
            }
            set
            {
                if (_afterCareReceiptModel != null)
                {
                    _afterCareReceiptModel.Syokei = value;
                }
            }
        }
        public int AfterSyokeiGaku_I
        {
            get
            {
                int ret = 0;
                if (_afterCareReceiptModel != null)
                {
                    ret = _afterCareReceiptModel.SyokeiGaku_I;
                }
                return ret;
            }
            set
            {
                if (_afterCareReceiptModel != null)
                {
                    _afterCareReceiptModel.SyokeiGaku_I = value;
                }
            }
        }
        public int AfterSyokeiGaku_RO
        {
            get
            {
                int ret = 0;
                if (_afterCareReceiptModel != null)
                {
                    ret = _afterCareReceiptModel.SyokeiGaku_RO;
                }
                return ret;
            }
            set
            {
                if (_afterCareReceiptModel != null)
                {
                    _afterCareReceiptModel.SyokeiGaku_RO = value;
                }
            }
        }
        public int KensaDate
        {
            get
            {
                int ret = 0;
                if (_afterCareReceiptModel != null)
                {
                    ret = _afterCareReceiptModel.KensaDate;
                }
                return ret;
            }
            set
            {
                if (_afterCareReceiptModel != null)
                {
                    _afterCareReceiptModel.KensaDate = value;
                }
            }
        }
        public int ZenkaiKensaDate
        {
            get
            {
                int ret = 0;
                if (_afterCareReceiptModel != null)
                {
                    ret = _afterCareReceiptModel.ZenkaiKensaDate;
                }
                return ret;
            }
            set
            {
                if (_afterCareReceiptModel != null)
                {
                    _afterCareReceiptModel.ZenkaiKensaDate = value;
                }
            }
        }
    }
}
