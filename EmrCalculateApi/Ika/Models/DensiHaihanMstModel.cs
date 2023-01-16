using Entity.Tenant;

namespace EmrCalculateApi.Ika.Models
{
    public class DensiHaihanMstModel 
    {
        private int _hpId;
        private int _startDate;
        private int _endDate;
        private string _itemCd1;
        private string _itemCd2;
        private int _haihanKbn;
        private int _spJyoken;
        private int _incAfter;
        private int _termCnt;
        private int _termSbt;
        private int _targetKbn;
        private int _mstSbt;

        public DensiHaihanMstModel()
        {
            _hpId = 0;
            _startDate = 0;
            _endDate = 0;
            _itemCd1 = "";
            _itemCd2 = "";
            _haihanKbn = 0;
            _spJyoken = 0;
            _incAfter = 0;
            _termCnt = 0;
            _termSbt = 0;
            _targetKbn = 0;
            _mstSbt = 0;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return _hpId; }
            set { _hpId = value; }
        }

        /// <summary>
        /// 適用開始日
        /// </summary>
        public int StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        /// <summary>
        /// 適用終了日
        /// </summary>
        public int EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        /// <summary>
        /// 項目コード１
        /// 
        /// </summary>
        public string ItemCd1
        {
            get { return _itemCd1 ?? string.Empty; }
            set { _itemCd1 = value; }
        }

        /// <summary>
        /// 項目コード２
        /// 
        /// </summary>
        public string ItemCd2
        {
            get { return _itemCd2 ?? string.Empty; }
            set { _itemCd2 = value; }
        }

        /// <summary>
        /// 背反区分
        /// "背反の条件を表す。 
        /// 1: 診療行為コード①を算定する。 
        /// 2: 診療行為コード②を算定する。 
        /// 3: 何れか一方を算定する。"
        /// </summary>
        public int HaihanKbn
        {
            get { return _haihanKbn; }
            set { _haihanKbn = value; }
        }

        /// <summary>
        /// 特例条件
        /// "背反条件に特別な条件がある場合に設定する 
        /// 0: 条件なし
        /// 1: 条件あり "
        /// </summary>
        public int SpJyoken
        {
            get { return _spJyoken; }
            set { _spJyoken = value; }
        }

        /// <summary>
        /// 診療日以降確認有無
        /// 1:診療日以降～月末(週末)までもチェック対象にする
        /// </summary>
        public int IncAfter
        {
            get { return _incAfter; }
            set { _incAfter = value; }
        }

        /// <summary>
        /// チェック期間数
        /// </summary>
        public int TermCnt
        {
            get { return _termCnt; }
            set { _termCnt = value; }
        }

        /// <summary>
        /// チェック期間種別
        /// 0:未指定 1:来院 2:日 3:暦週 4:暦月 5:週 6:月 9:患者あたり
        /// </summary>
        public int termSbt
        {
            get { return _termSbt; }
            set { _termSbt = value; }
        }

        /// <summary>
        /// 対象区分
        /// </summary>
        public int TargetKbn
        {
            get { return _targetKbn; }
            set { _targetKbn = value; }
        }

        /// <summary>
        /// マスタの種類
        /// </summary>
        public int mstSbt
        {
            get { return _mstSbt; }
            set { _mstSbt = value; }
        }
    }

}
