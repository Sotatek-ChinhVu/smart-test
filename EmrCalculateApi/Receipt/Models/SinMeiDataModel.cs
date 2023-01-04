using EmrCalculateApi.Interface;
using Helper.Common;
using Helper.Constants;

namespace EmrCalculateApi.Receipt.Models
{
    public class SinMeiDataModel
    {
        private const string ModuleName = ModuleNameConst.EmrCalculateReceipt;

        private long _ptId;
        private string _recId;
        private int _sinId;
        private string _futanKbn;
        private string _itemCd;
        private string _odrItemCd;
        private string _itemName;
        private string _commentData;
        private double _suryo;
        private double _ten;
        private double _kingaku;
        private double _totalTen;
        private double _totalKingaku;
        private int _count;
        private int _unitCd;
        private string _unitName;
        private double _price;
        private string _tokuzaiName;
        private string _productName;
        private string _comment1;
        private string _commentCd1;
        private string _commentData1;
        private string _comment2;
        private string _commentCd2;
        private string _commentData2;
        private string _comment3;
        private string _commentCd3;
        private string _commentData3;
        private int _day1;
        private int _day2;
        private int _day3;
        private int _day4;
        private int _day5;
        private int _day6;
        private int _day7;
        private int _day8;
        private int _day9;
        private int _day10;
        private int _day11;
        private int _day12;
        private int _day13;
        private int _day14;
        private int _day15;
        private int _day16;
        private int _day17;
        private int _day18;
        private int _day19;
        private int _day20;
        private int _day21;
        private int _day22;
        private int _day23;
        private int _day24;
        private int _day25;
        private int _day26;
        private int _day27;
        private int _day28;
        private int _day29;
        private int _day30;
        private int _day31;
        private int _hokenPid;
        private int _rpNo;
        private int _seqNo;
        private int _rowNo;
        private string _cdKbn;
        private int _jihiSbt;
        private int _futanS;
        private int _futanK1;
        private int _futanK2;
        private int _futanK3;
        private int _futanK4;

        private int _lastRowKbn;
        private int _santeiKbn;
        private int _inOutKbn;
        private int _kizamiId;
        private int _tenId;
        private int _kazeiKbn;
        private int _taxRate;
        private string _syukeiSaki;

        private int _entenKbn;

        ISystemConfigProvider _systemConfigProvider;
        IEmrLogger _emrLogger;

        public SinMeiDataModel(ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            _systemConfigProvider = systemConfigProvider;
            _emrLogger = emrLogger;
        }

        /// <summary>
        /// 患者番号
        /// </summary>
        public long PtId
        {
            get { return _ptId; }
            set { _ptId = value; }
        }

        ///<summary>
        ///レコード識別情報
        ///</summary>
        public string RecId
        {
            get { return _recId; }
            set { _recId = value; }
        }

        /// <summary>
        /// 診療識別
        /// </summary>
        public int SinId
        {
            get { return _sinId; }
            set { _sinId = value; }
        }

        public int SinIdOrg { get; set; } = 0;
        /// <summary>
        /// 負担区分
        /// </summary>
        public string FutanKbn
        {
            get { return _futanKbn == "0" ? "" : _futanKbn; }
            set { _futanKbn = value; }
        }

        public int FutanSortNo
        {
            get
            {
                int ret = 0;

                switch (FutanKbn)
                {
                    case "1": ret = 1; break;
                    case "5": ret = 2; break;
                    case "6": ret = 3; break;
                    case "B": ret = 4; break;
                    case "C": ret = 5; break;

                    case "2": ret = 101; break;
                    case "3": ret = 102; break;
                    case "E": ret = 103; break;
                    case "G": ret = 104; break;
                    case "7": ret = 105; break;
                    case "H": ret = 106; break;
                    case "I": ret = 107; break;
                    case "J": ret = 108; break;
                    case "K": ret = 109; break;

                    case "4": ret = 201; break;
                    case "M": ret = 202; break;
                    case "N": ret = 203; break;
                    case "O": ret = 204; break;
                    case "P": ret = 205; break;
                    case "Q": ret = 206; break;
                    case "R": ret = 207; break;
                    case "S": ret = 208; break;
                    case "T": ret = 209; break;
                    case "U": ret = 210; break;

                    case "V": ret = 301; break;
                    case "W": ret = 302; break;
                    case "X": ret = 303; break;
                    case "Y": ret = 304; break;
                    case "Z": ret = 305; break;

                    case "9": ret = 401; break;
                }

                return ret;
            }
        }

        /// <summary>
        /// 診療行為コード
        /// </summary>
        public string ItemCd
        {
            get { return _itemCd; }
            set { _itemCd = value; }
        }

        /// <summary>
        /// オーダー診療行為コード
        /// </summary>
        public string OdrItemCd
        {
            get { return _odrItemCd; }
            set { _odrItemCd = value; }
        }
        /// <summary>
        /// 診療行為名称
        /// </summary>
        public string ItemName
        {
            get { return _itemName; }
            set { _itemName = value; }
        }

        /// <summary>
        /// 文字データ
        /// </summary>
        public string CommentData
        {
            get { return _commentData; }
            set { _commentData = value; }
        }

        /// <summary>
        /// 数量データ（使用量）
        /// </summary>
        public double Suryo
        {
            get { return _suryo; }
            set { _suryo = value; }
        }
        public string SuryoDsp
        {
            get 
            {
                string ret = "";

                if(string.IsNullOrEmpty(UnitName) == false)
                {
                    ret = Suryo.ToString();
                }
                return ret; 
            }
            
        }
        /// <summary>
        /// 点数
        /// </summary>
        public double TotalTen
        {
            get { return _totalTen; }
            set { _totalTen = value; }
        }

        /// <summary>
        /// 金額
        /// </summary>
        public double TotalKingaku
        {
            get { return _totalKingaku; }
            set { _totalKingaku = value; }
        }
        /// <summary>
        /// 点数
        /// </summary>
        public double Ten
        {
            get { return _ten; }
            set { _ten = value; }
        }

        /// <summary>
        /// 金額
        /// </summary>
        public double Kingaku
        {
            get { return _kingaku; }
            set { _kingaku = value; }
        }

        /// <summary>
        /// 回数
        /// </summary>
        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }
        /// <summary>
        /// 点数回数
        /// </summary>
        public string TenKai
        {
            get
            {
                string ret = "";

                if (_lastRowKbn == 1)
                {
                    if (_kingaku != 0)
                    {
                        ret = string.Format("{0,8}x{1,3}", _kingaku, _count);
                    }
                    else if (_ten != 0 || InOutKbn == 1 || DspZeroTenKai)
                    {
                        // 0以外 or 院外処方
                        ret = string.Format("{0,8}x{1,3}", _ten, _count);
                    }
                }
                return ret;
            }
        }
        /// <summary>
        /// 点数が0でも点数回数を表示するかどうか
        /// </summary>
        public bool DspZeroTenKai { get; set; } = false;

        /// <summary>
        /// 単位コード
        /// </summary>
        public int UnitCd
        {
            get { return _unitCd; }
            set { _unitCd = value; }
        }
        /// <summary>
        /// 単位名称
        /// </summary>
        public string UnitName
        {
            get { return _unitName; }
            set { _unitName = value; }
        }
        /// <summary>
        /// 単価
        /// </summary>
        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }
        /// <summary>
        /// 特定器材名称
        /// </summary>
        public string TokuzaiName
        {
            get { return _tokuzaiName; }
            set { _tokuzaiName = value; }
        }
        /// <summary>
        /// 商品名及び規格又はサイズ
        /// </summary>
        public string ProductName
        {
            get { return _productName; }
            set { _productName = value; }
        }
        /// <summary>
        /// コメント１
        /// </summary>
        public string Comment1
        {
            get { return _comment1; }
            set { _comment1 = value; }
        }
        /// <summary>
        /// コメントコード１
        /// </summary>
        public string CommentCd1
        {
            get { return _commentCd1; }
            set { _commentCd1 = value; }
        }
        /// <summary>
        /// コメントデータ１
        /// </summary>
        public string CommentData1
        {
            get { return _commentData1; }
            set { _commentData1 = value; }
        }
        /// <summary>
        /// コメント２
        /// </summary>
        public string Comment2
        {
            get { return _comment2; }
            set { _comment2 = value; }
        }
        /// <summary>
        /// コメントコード２
        /// </summary>
        public string CommentCd2
        {
            get { return _commentCd2; }
            set { _commentCd2 = value; }
        }
        /// <summary>
        /// コメントデータ２
        /// </summary>
        public string CommentData2
        {
            get { return _commentData2; }
            set { _commentData2 = value; }
        }
        /// <summary>
        /// コメント３
        /// </summary>
        public string Comment3
        {
            get { return _comment3; }
            set { _comment3 = value; }
        }
        /// <summary>
        /// コメントコード３
        /// </summary>
        public string CommentCd3
        {
            get { return _commentCd3; }
            set { _commentCd3 = value; }
        }
        /// <summary>
        /// コメントデータ３
        /// </summary>
        public string CommentData3
        {
            get { return _commentData3; }
            set { _commentData3 = value; }
        }
        ///<summary>
        ///１日の情報
        ///</summary>
        public int Day1
        {
            get { return _day1; }
            set { _day1 = value; }
        }
        ///<summary>
        ///２日の情報
        ///</summary>
        public int Day2
        {
            get { return _day2; }
            set { _day2 = value; }
        }
        ///<summary>
        ///３日の情報
        ///</summary>
        public int Day3
        {
            get { return _day3; }
            set { _day3 = value; }
        }
        ///<summary>
        ///４日の情報
        ///</summary>
        public int Day4
        {
            get { return _day4; }
            set { _day4 = value; }
        }
        ///<summary>
        ///５日の情報
        ///</summary>
        public int Day5
        {
            get { return _day5; }
            set { _day5 = value; }
        }
        ///<summary>
        ///６日の情報
        ///</summary>
        public int Day6
        {
            get { return _day6; }
            set { _day6 = value; }
        }
        ///<summary>
        ///７日の情報
        /// </summary>
        public int Day7
        {
            get { return _day7; }
            set { _day7 = value; }
        }
        ///<summary>
        ///８日の情報
        /// </summary>
        public int Day8
        {
            get { return _day8; }
            set { _day8 = value; }
        }
        ///<summary>
        ///９日の情報
        /// </summary>
        public int Day9
        {
            get { return _day9; }
            set { _day9 = value; }
        }
        ///<summary>
        ///１０日の情報
        /// </summary>
        public int Day10
        {
            get { return _day10; }
            set { _day10 = value; }
        }
        ///<summary>
        ///１１日の情報
        /// </summary>
        public int Day11
        {
            get { return _day11; }
            set { _day11 = value; }
        }
        ///<summary>
        ///１２日の情報
        /// </summary>
        public int Day12
        {
            get { return _day12; }
            set { _day12 = value; }
        }
        ///<summary>
        ///１３日の情報
        /// </summary>
        public int Day13
        {
            get { return _day13; }
            set { _day13 = value; }
        }
        ///<summary>
        ///１４日の情報
        /// </summary>
        public int Day14
        {
            get { return _day14; }
            set { _day14 = value; }
        }
        ///<summary>
        ///１５日の情報
        /// </summary>
        public int Day15
        {
            get { return _day15; }
            set { _day15 = value; }
        }
        ///<summary>
        ///１６日の情報
        /// </summary>
        public int Day16
        {
            get { return _day16; }
            set { _day16 = value; }
        }
        ///<summary>
        ///１７日の情報
        /// </summary>
        public int Day17
        {
            get { return _day17; }
            set { _day17 = value; }
        }
        ///<summary>
        ///１８日の情報
        /// </summary>
        public int Day18
        {
            get { return _day18; }
            set { _day18 = value; }
        }
        ///<summary>
        ///１９日の情報
        /// </summary>
        public int Day19
        {
            get { return _day19; }
            set { _day19 = value; }
        }
        ///<summary>
        ///２０日の情報
        /// </summary>
        public int Day20
        {
            get { return _day20; }
            set { _day20 = value; }
        }
        ///<summary>
        ///２１日の情報
        /// </summary>
        public int Day21
        {
            get { return _day21; }
            set { _day21 = value; }
        }
        ///<summary>
        ///２２日の情報
        /// </summary>
        public int Day22
        {
            get { return _day22; }
            set { _day22 = value; }
        }
        ///<summary>
        ///２３日の情報
        /// </summary>
        public int Day23
        {
            get { return _day23; }
            set { _day23 = value; }
        }
        ///<summary>
        ///２４日の情報
        /// </summary>
        public int Day24
        {
            get { return _day24; }
            set { _day24 = value; }
        }
        ///<summary>
        ///２５日の情報
        /// </summary>
        public int Day25
        {
            get { return _day25; }
            set { _day25 = value; }
        }
        ///<summary>
        ///２６日の情報
        /// </summary>
        public int Day26
        {
            get { return _day26; }
            set { _day26 = value; }
        }
        ///<summary>
        ///２７日の情報
        /// </summary>
        public int Day27
        {
            get { return _day27; }
            set { _day27 = value; }
        }
        ///<summary>
        ///２８日の情報
        /// </summary>
        public int Day28
        {
            get { return _day28; }
            set { _day28 = value; }
        }
        ///<summary>
        ///２９日の情報
        /// </summary>
        public int Day29
        {
            get { return _day29; }
            set { _day29 = value; }
        }
        ///<summary>
        ///３０日の情報
        /// </summary>
        public int Day30
        {
            get { return _day30; }
            set { _day30 = value; }
        }
        ///<summary>
        ///３１日の情報
        /// </summary>
        public int Day31
        {
            get { return _day31; }
            set { _day31 = value; }
        }

        public int Day(int index)
        {
            switch (index)
            {
                case 1: return Day1;
                case 2: return Day2;
                case 3: return Day3;
                case 4: return Day4;
                case 5: return Day5;
                case 6: return Day6;
                case 7: return Day7;
                case 8: return Day8;
                case 9: return Day9;
                case 10: return Day10;
                case 11: return Day11;
                case 12: return Day12;
                case 13: return Day13;
                case 14: return Day14;
                case 15: return Day15;
                case 16: return Day16;
                case 17: return Day17;
                case 18: return Day18;
                case 19: return Day19;
                case 20: return Day20;
                case 21: return Day21;
                case 22: return Day22;
                case 23: return Day23;
                case 24: return Day24;
                case 25: return Day25;
                case 26: return Day26;
                case 27: return Day27;
                case 28: return Day28;
                case 29: return Day29;
                case 30: return Day30;
                case 31: return Day31;
            }
            return 0;
        }

        public int Day1Add { get; set; } = 0;
        public int Day2Add { get; set; } = 0;
        public int Day3Add { get; set; } = 0;
        public int Day4Add { get; set; } = 0;
        public int Day5Add { get; set; } = 0;
        public int Day6Add { get; set; } = 0;
        public int Day7Add { get; set; } = 0;
        public int Day8Add { get; set; } = 0;
        public int Day9Add { get; set; } = 0;
        public int Day10Add { get; set; } = 0;
        public int Day11Add { get; set; } = 0;
        public int Day12Add { get; set; } = 0;
        public int Day13Add { get; set; } = 0;
        public int Day14Add { get; set; } = 0;
        public int Day15Add { get; set; } = 0;
        public int Day16Add { get; set; } = 0;
        public int Day17Add { get; set; } = 0;
        public int Day18Add { get; set; } = 0;
        public int Day19Add { get; set; } = 0;
        public int Day20Add { get; set; } = 0;
        public int Day21Add { get; set; } = 0;
        public int Day22Add { get; set; } = 0;
        public int Day23Add { get; set; } = 0;
        public int Day24Add { get; set; } = 0;
        public int Day25Add { get; set; } = 0;
        public int Day26Add { get; set; } = 0;
        public int Day27Add { get; set; } = 0;
        public int Day28Add { get; set; } = 0;
        public int Day29Add { get; set; } = 0;
        public int Day30Add { get; set; } = 0;
        public int Day31Add { get; set; } = 0;

        public int DayAdd(int index)
        {
            switch (index)
            {
                case 1: return Day1Add;
                case 2: return Day2Add;
                case 3: return Day3Add;
                case 4: return Day4Add;
                case 5: return Day5Add;
                case 6: return Day6Add;
                case 7: return Day7Add;
                case 8: return Day8Add;
                case 9: return Day9Add;
                case 10: return Day10Add;
                case 11: return Day11Add;
                case 12: return Day12Add;
                case 13: return Day13Add;
                case 14: return Day14Add;
                case 15: return Day15Add;
                case 16: return Day16Add;
                case 17: return Day17Add;
                case 18: return Day18Add;
                case 19: return Day19Add;
                case 20: return Day20Add;
                case 21: return Day21Add;
                case 22: return Day22Add;
                case 23: return Day23Add;
                case 24: return Day24Add;
                case 25: return Day25Add;
                case 26: return Day26Add;
                case 27: return Day27Add;
                case 28: return Day28Add;
                case 29: return Day29Add;
                case 30: return Day30Add;
                case 31: return Day31Add;
            }
            return 0;
        }
        /// <summary>
        /// 保険組み合わせID
        /// </summary>
        public int HokenPid
        {
            get { return _hokenPid; }
            set { _hokenPid = value; }
        }

        /// <summary>
        /// RP_NO
        /// </summary>
        public int RpNo
        {
            get { return _rpNo; }
            set { _rpNo = value; }
        }
        /// <summary>
        /// SEQ_NO
        /// </summary>
        public int SeqNo
        {
            get { return _seqNo; }
            set { _seqNo = value; }
        }
        /// <summary>
        /// ROW_NO
        /// </summary>
        public int RowNo
        {
            get { return _rowNo; }
            set { _rowNo = value; }
        }
        /// <summary>
        /// コード区分
        /// </summary>
        public string CdKbn
        {
            get { return _cdKbn; }
            set { _cdKbn = value; }
        }
        /// <summary>
        /// 自費種別
        /// </summary>
        public int JihiSbt
        {
            get { return _jihiSbt; }
            set { _jihiSbt = value; }
        }
        /// <summary>
        /// 主保険負担
        /// 1: 負担あり
        /// </summary>
        public int FutanS
        {
            get { return _futanS; }
            set { _futanS = value; }
        }
        /// <summary>
        /// 公１負担
        /// 1: 負担あり
        /// </summary>
        public int FutanK1
        {
            get { return _futanK1; }
            set { _futanK1 = value; }
        }
        /// <summary>
        /// 公２負担
        /// 2: 負担あり
        /// </summary>
        public int FutanK2
        {
            get { return _futanK2; }
            set { _futanK2 = value; }
        }
        /// <summary>
        /// 公３負担
        /// 3: 負担あり
        /// </summary>
        public int FutanK3
        {
            get { return _futanK3; }
            set { _futanK3 = value; }
        }
        /// <summary>
        /// 公４負担
        /// 4: 負担あり
        /// </summary>
        public int FutanK4
        {
            get { return _futanK4; }
            set { _futanK4 = value; }
        }
        /// <summary>
        /// 行区分
        /// </summary>
        public int LastRowKbn
        {
            get { return _lastRowKbn; }
            set { _lastRowKbn = value; }
        }
        /// <summary>
        /// 算定区分
        /// </summary>
        public int SanteiKbn
        {
            get { return _santeiKbn; }
            set { _santeiKbn = value; }
        }

        /// <summary>
        /// 院外処方区分
        ///     0: 院内
        ///     1: 院外
        /// </summary>
        public int InOutKbn
        {
            get { return _inOutKbn; }
            set { _inOutKbn = value; }
        }

        public int KizamiId
        {
            get { return _kizamiId; }
            set { _kizamiId = value; }
        }

        public int TenId
        {
            get { return _tenId; }
            set { _tenId = value; }
        }
        /// <summary>
        /// 課税区分
        ///     0 - 非課税
        ///     1 - 外税（通常税率）
        ///     2 - 外税（軽減税率）
        ///     3 - 内税（通常税率）
        ///     4 - 内税（軽減税率）
        /// </summary>
        public int KazeiKbn
        {
            get { return _kazeiKbn; }
            set { _kazeiKbn = value; }
        }
        /// <summary>
        /// SYSTEM_GENERATION_CONF検索用
        /// </summary>
        public int ZeiKigoEdaNo
        {
            get
            {
                int ret = 0;

                switch(KazeiKbn)
                {
                    case 0: ret = 2; break;
                    case 1: ret = 0; break;
                    case 2: ret = 1; break;
                    case 3: ret = 0; break;
                    case 4: ret = 1; break;
                }
                return ret;
            }
        }
        /// <summary>
        /// 0-非課税・外税 1-内税
        /// </summary>
        public int ZeiInOut
        {
            get
            {
                int ret = 0;

                switch (KazeiKbn)
                {
                    case 0: ret = 0; break;
                    case 1: ret = 0; break;
                    case 2: ret = 0; break;
                    case 3: ret = 1; break;
                    case 4: ret = 1; break;
                }
                return ret;
            }
        }

        public int TaxRate
        {
            get { return _taxRate; }
            set { _taxRate = value; }
        }

        public string RecedenRecord
        {
            get
            {
                string ret = "";

                switch(RecId)
                {
                    case "SI":
                            // SIレコード
                            ret = GetSIRecord();
                        break;
                    case "IY":
                            // IYレコード
                            ret = GetIYRecord();
                        break;
                    case "TO":
                            // TOレコード
                            ret = GetTORecord();
                        break;
                    case "CO":
                            // COレコード
                            ret = GetCORecord();
                        break;
                }

                return ret;
            }
        }

        public string RousaiRecedenRecord
        {
            get
            {
                string ret = "";

                switch (RecId)
                {
                    case "SI":
                        // RIレコード
                        ret = GetRIRecord();
                        break;
                    case "IY":
                        // IYレコード
                        ret = GetIYRecord(1);
                        break;
                    case "TO":
                        // TOレコード
                        ret = GetTORecord(1);
                        break;
                    case "CO":
                        // COレコード
                        ret = GetCORecord(1);
                        break;
                }

                return ret;
            }
        }
        public string AfterCareRecedenRecord
        {
            get
            {
                string ret = "";

                switch (RecId)
                {
                    case "SI":
                        // RIレコード
                        ret = GetRIRecord(2);
                        break;
                    case "IY":
                        // IYレコード
                        ret = GetIYRecord(2);
                        break;
                    case "TO":
                        // TOレコード
                        ret = GetTORecord(2);
                        break;
                    case "CO":
                        // COレコード
                        ret = GetCORecord(2);
                        break;
                }

                return ret;
            }
        }
        private string GetSIRecord()
        {
            string ret = "";

            // 共通部
            ret = GetCommon();
            // 数量
            ret += "," + GetSuryo();
            // 合計点数
            ret += "," + GetTen();
            // 回数
            ret += "," + GetCount();
            // コメント情報
            ret += "," + GetCommentData();
            // 日付情報
            ret += "," + GetDays();

            return ret;
        }

        private string GetRIRecord(int mode = 0)
        {
            string ret = "";

            // 共通部
            // レコード識別
            ret += "RI";

            // 診療識別
            if ((RowNo == 1 && SinId > 0) || SinId == 1 || SinId == 99)
            {
                string sinId = $",{SinId:D2}";
                if (InOutKbn == 1)
                {
                    // 院外処方の場合
                    if (_systemConfigProvider.GetReceiptOutDrgSinId() == 1)
                    {
                        sinId = $"Z{SinId % 10}";
                    }
                }
                ret += sinId;
            }
            else
            {
                ret += ",";
            }
             // 診療行為コード
            ret += "," + ItemCd;

            // 数量
            ret += "," + GetSuryo();
            // 合計点数
            ret += "," + GetTen();
            // 合計金額
            ret += "," + GetKingaku();
            // 回数
            ret += "," + GetCount();
            // コメント情報
            ret += "," + GetCommentData();
            // 日付情報
            if (mode == 2)
            {
                ret += "," + GetEmptyDays();
            }
            else
            {
                ret += "," + GetDays();
            }

            return ret;
        }

        private string GetIYRecord(int mode = 0)
        {
            string ret = "";

            // 共通部
            ret = GetCommon(mode);

            // 数量
            ret += "," + GetSuryo();
            // 合計点数
            ret += "," + GetTen();
            // 回数
            ret += "," + GetCount();
            // コメント情報
            ret += "," + GetCommentData();
            // 日付情報
            if (mode == 2)
            {
                ret += "," + GetEmptyDays();
            }
            else
            {
                ret += "," + GetDays();
            }

            return ret;
        }

        private string GetTORecord(int mode = 0)
        {
            string ret = "";

            // 共通部
            ret = GetCommon(mode);

            // 数量
            ret += "," + GetSuryo();
            // 合計点数
            ret += "," + GetTenTO();
            // 回数
            ret += "," + GetCount();
            // 単位コード
            if (UnitCd > 0)
            {
                ret += "," + string.Format("{0:D3}", UnitCd);
            }
            else
            {
                ret += ",";
            }
            // 単価
            ret += "," + CIUtil.ToStringIgnoreZero(Price);
            // 特定器材名称
            if (mode == 0)
            {
                ret += "," + CIUtil.ToWide(TokuzaiName);
            }
            else
            {
                ret += ",";
            }
            // 製品名
            ret += "," + CIUtil.ToWide(ProductName);
            // コメント情報
            ret += "," + GetCommentData();
            // 日付情報
            if (mode == 2)
            {
                ret += "," + GetEmptyDays();
            }
            else
            {
                ret += "," + GetDays();
            }

            return ret;
        }

        private string GetCORecord(int mode = 0)
        {
            const int conMaxLength = 38;

            string ret = "";

            // 共通部
            string commonData = GetCommon(mode);
            string commentData = CIUtil.ToRecedenString(CommentData);
            string refText = "";
            string badText = "";
            if (ItemCd.StartsWith("82") && ItemCd.Length == 9)
            {
                // 定型文
                ret += commonData + ",";
            }
            else
            {
                if (CIUtil.IsUntilJISKanjiLevel2(commentData, ref refText, ref badText) == false)
                {
                    _emrLogger.WriteLogMsg( this, "GetCORecord",
                        string.Format("CommentData is include bad charcter PtId:{0} Comment:{1} badCharcters:{1}",
                            _ptId, commentData, badText));

                    commentData = refText;

                }

                if (commentData != "")
                {
                    string[] tmpLines = commentData.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                    List<string> lines = new List<string>();

                    bool first = true;
                    for (int i = 0; i < tmpLines.Count(); i++)
                    {
                        if (first && tmpLines[i].Trim() == "" && tmpLines.Count() >= 2)
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
                        while (lines[lines.Count() - 1].Trim() == "" && lines.Count() >= 2)
                        {
                            lines.RemoveAt(lines.Count() - 1);
                        }
                    }

                    for (int i = 0; i < lines.Count(); i++)
                    {
                        while (lines[i] != "")
                        {
                            string tmp = lines[i];
                            if (tmp.Length > conMaxLength)
                            {
                                tmp = lines[i].Substring(0, conMaxLength);
                            }

                            if (ItemCd == ItemCdConst.CommentFree && tmp.Trim() == "")
                            {
                                // フリーコメントで、空、または、スペースのみの場合は、記録しない
                            }
                            else
                            { 
                                // コメント情報
                                if (ret != "")
                                {
                                    ret += "\r\n";
                                }
                                ret += commonData + "," + tmp;

                                if (SinId != 1 && SinId != 99)
                                {
                                    // 2行目以降は診療識別を記録しない
                                    commonData = GetCommon2(mode);
                                }
                            }

                            lines[i] = CIUtil.Copy(lines[i], tmp.Length + 1, lines[i].Length - tmp.Length);
                        }
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// レコード共通部分の文字列を取得
        /// </summary>
        /// <returns></returns>
        private string GetCommon(int mode = 0)
        {
            string ret = "";

            // レコード識別
            ret += RecId;

            // 診療識別
            if ((RowNo == 1 && SinId > 0) || SinId == 1 || SinId == 99)
            {
                string sinId = $",{SinId:D2}";
                if (InOutKbn == 1)
                {
                    // 院外処方の場合
                    if (_systemConfigProvider.GetReceiptOutDrgSinId() == 1)
                    {
                        sinId = $"Z{SinId % 10}";
                    }
                }
                ret += sinId;
            }
            else
            {
                ret += ",";
            }
            // 負担区分
            if (mode == 0)
            {
                ret += "," + FutanKbn;
            }
            else
            {
                ret += ",";
            }

            // 診療行為コード
            ret += "," + ItemCd;
            
            return ret;
        }

        /// <summary>
        /// レコード共通部分の文字列を取得（コメント折り返し用）
        /// </summary>
        /// <returns></returns>
        private string GetCommon2(int mode = 0)
        {
            string ret = "";

            // レコード識別
            ret += RecId;
            // 診療識別
            ret += ",";
            // 負担区分
            if (mode == 0)
            {
                ret += "," + FutanKbn;
            }
            else
            {
                ret += ",";
            }
            // 診療行為コード
            ret += "," + ItemCd;

            return ret;
        }

        private string GetSuryo()
        {
            string ret = "";
            if (
                ((RecId == "SI" || RecId == "RI") && KizamiId == 1) ||
                 (RecId == "IY" && TenId == 1) ||
                 (RecId == "TO" && new int[] { 1, 2, 4, 9 }.Contains(TenId)))
            {
                ret = CIUtil.ToStringIgnoreZero(Suryo);
            }

            return ret;
        }

        /// <summary>
        /// 合計点数を取得
        /// </summary>
        /// <returns></returns>
        private string GetTen()
        {
            string retTen = "";

            if (LastRowKbn == 1)
            {                
                retTen += CIUtil.ToStringIgnoreZero(Math.Abs(Ten));
            }
            return retTen;
        }
        /// <summary>
        /// 特定器材の場合の合計点数、酸素補正率の場合は0でも0を記録する
        /// </summary>
        /// <returns></returns>
        private string GetTenTO()
        {
            string retTen = "";

            if (LastRowKbn == 1)
            {
                if (ItemCd == ItemCdConst.SansoHoseiRitu)
                {
                    // 酸素補正率は0も記録する
                    retTen += Math.Abs(Ten).ToString();
                }
                else
                {
                    retTen += CIUtil.ToStringIgnoreZero(Math.Abs(Ten));
                }
            }
            return retTen;
        }
        /// <summary>
        /// 合計金額を取得
        /// </summary>
        /// <returns></returns>
        private string GetKingaku()
        {
            string retKingaku = "";

            if (LastRowKbn == 1)
            {
                retKingaku += CIUtil.ToStringIgnoreZero(Math.Abs(Kingaku));
            }
            return retKingaku;
        }

        /// <summary>
        /// 回数を取得
        /// </summary>
        /// <returns></returns>
        private string GetCount()
        {
            string retCount = "";

            //if(LastRowKbn == 1)
            //{
                retCount = Count.ToString();
            //}
            return retCount;
        }
        
        /// <summary>
        /// コメント情報を取得
        /// </summary>
        /// <returns></returns>
        private string GetCommentData()
        {
            string ret = "";

            ret += CommentCd1;
            ret += "," + CIUtil.ToWide(CommentData1);
            ret += "," + CommentCd2;
            ret += "," + CIUtil.ToWide(CommentData2);
            ret += "," + CommentCd3;
            ret += "," + CIUtil.ToWide(CommentData3);

            return ret;
        }

        /// <summary>
        /// 日付情報取得
        /// </summary>
        /// <returns></returns>
        private string GetDays()
        {
            string ret = "";

            ret += CIUtil.ToStringIgnoreZero(Day1);
            ret += "," + CIUtil.ToStringIgnoreZero(Day2);
            ret += "," + CIUtil.ToStringIgnoreZero(Day3);
            ret += "," + CIUtil.ToStringIgnoreZero(Day4);
            ret += "," + CIUtil.ToStringIgnoreZero(Day5);
            ret += "," + CIUtil.ToStringIgnoreZero(Day6);
            ret += "," + CIUtil.ToStringIgnoreZero(Day7);
            ret += "," + CIUtil.ToStringIgnoreZero(Day8);
            ret += "," + CIUtil.ToStringIgnoreZero(Day9);
            ret += "," + CIUtil.ToStringIgnoreZero(Day10);
            ret += "," + CIUtil.ToStringIgnoreZero(Day11);
            ret += "," + CIUtil.ToStringIgnoreZero(Day12);
            ret += "," + CIUtil.ToStringIgnoreZero(Day13);
            ret += "," + CIUtil.ToStringIgnoreZero(Day14);
            ret += "," + CIUtil.ToStringIgnoreZero(Day15);
            ret += "," + CIUtil.ToStringIgnoreZero(Day16);
            ret += "," + CIUtil.ToStringIgnoreZero(Day17);
            ret += "," + CIUtil.ToStringIgnoreZero(Day18);
            ret += "," + CIUtil.ToStringIgnoreZero(Day19);
            ret += "," + CIUtil.ToStringIgnoreZero(Day20);
            ret += "," + CIUtil.ToStringIgnoreZero(Day21);
            ret += "," + CIUtil.ToStringIgnoreZero(Day22);
            ret += "," + CIUtil.ToStringIgnoreZero(Day23);
            ret += "," + CIUtil.ToStringIgnoreZero(Day24);
            ret += "," + CIUtil.ToStringIgnoreZero(Day25);
            ret += "," + CIUtil.ToStringIgnoreZero(Day26);
            ret += "," + CIUtil.ToStringIgnoreZero(Day27);
            ret += "," + CIUtil.ToStringIgnoreZero(Day28);
            ret += "," + CIUtil.ToStringIgnoreZero(Day29);
            ret += "," + CIUtil.ToStringIgnoreZero(Day30);
            ret += "," + CIUtil.ToStringIgnoreZero(Day31);

            return ret;
        }
        private string GetEmptyDays()
        {
            string ret = "";

            ret += "";
            ret += ",";
            ret += ",";
            ret += ",";
            ret += ",";
            ret += ",";
            ret += ",";
            ret += ",";
            ret += ",";
            ret += ",";
            ret += ",";
            ret += ",";
            ret += ",";
            ret += ",";
            ret += ",";
            ret += ",";
            ret += ",";
            ret += ",";
            ret += ",";
            ret += ",";
            ret += ",";
            ret += ",";
            ret += ",";
            ret += ",";
            ret += ",";
            ret += ",";
            ret += ",";
            ret += ",";
            ret += ",";
            ret += ",";
            ret += ",";

            return ret;
        }
        /// <summary>
        /// 診療行為のソートキー
        /// [診区が1, 99(レセコメント）の場合はそのまま、それ以外の場合は10] * 100 + 負担区分ソート順(分点の場合は公費Index、それ以外の場合は0)
        /// </summary>
        public int ReceSortKey
        {
            get
            {
                int ret = 0;

                if(new int[] { 1, 99 }.Contains(SinId))
                {
                    ret = SinId;
                }
                else
                {
                    ret = 10;
                }

                ret = ret * 100;

                ret += getFutanSortNo(FutanKbn);

                return ret;
            }
        }
        public int FutanSortKey
        {
            get
            {
                int ret = 0;

                if (new int[] { 1, 99 }.Contains(SinId))
                {
                    ret = SinId;
                }
                else
                {
                    ret = 10;
                }

                ret = ret * 1000;

                ret += FutanSortNo;

                return ret;
            }
        }
        private int getFutanSortNo(string futanKbn)
        {
            // 分点公費の場合、応じたソート順を返す
            // 分点公費ではない場合は0
            if (this.FutanK1 > 1) return 1;
            if (this.FutanK2 > 1) return 2;
            if (this.FutanK3 > 1) return 3;
            if (this.FutanK4 > 1) return 4;
            return 0;
        }

        public string SyukeiSaki
        {
            get
            {
                return _syukeiSaki;
            }
            set
            {
                _syukeiSaki = value;
            }
        }
        /// <summary>
        /// 円点区分
        /// </summary>
        public int EnTenKbn
        {
            get
            {
                return _entenKbn;
            }
            set
            {
                _entenKbn = value;
            }
        }

        /// 薬剤区分
        ///     当該医薬品の薬剤区分を表す。
        ///       0: 薬剤以外
        ///     　1: 内用薬
        ///     　3: その他
        ///     　4: 注射薬
        ///     　6: 外用薬
        ///     　8: 歯科用薬剤
        ///     （削）9: 歯科特定薬剤
        ///     ※レセプト電算マスターの項目「剤型」を収容する。
        public int DrugKbn { get; set; } = 0;
        
        /// <summary>
        /// SIN_KOUI.RP_NO
        /// </summary>
        public int SinRpNo { get; set; }
        /// <summary>
        /// SIN_KOUI.SEQ_NO
        /// </summary>
        public int SinSeqNo { get; set; }

        public bool IsComment
        {
            get
            {
                bool ret = false;

                if(string.IsNullOrEmpty(OdrItemCd) || 
                    (OdrItemCd.StartsWith("8") && OdrItemCd.Length == 9))
                {
                    ret = true;
                }

                return ret;
            }
        }

    }
}
