using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmrCalculateApi.Ika.Models;

namespace EmrCalculateApi.Receipt.Models
{
    public class ReceSinKouiCountModel
    {
        //public SinKouiCount SinKouiCount { get; } = null;

        /// <summary>
        /// 更新情報
        ///     0: 変更なし
        ///     1: 追加
        ///     2: 削除
        /// </summary>
        private int _updateState = 0;
        private long _keyNo = 0;

        private int _hpId;
        private long _ptId;
        private int _sinYm;
        private int _sinDay;
        private long _raiinNo;
        private int _rpNo;
        private int _seqNo;
        private int _count;

        public ReceSinKouiCountModel(SinKouiCountModel sinKouiCount)
        {
            _hpId = sinKouiCount.HpId;
            _ptId = sinKouiCount.PtId;
            _sinYm = sinKouiCount.SinYm;
            _sinDay = sinKouiCount.SinDay;
            _raiinNo = sinKouiCount.RaiinNo;
            _rpNo = sinKouiCount.RpNo;
            _seqNo = sinKouiCount.SeqNo;
            _count = sinKouiCount.Count;
            AdjCount = _count;
            _updateState = sinKouiCount.UpdateState;
            _keyNo = sinKouiCount.KeyNo;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get => _hpId;
        }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId
        {
            get => _ptId;
        }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        public int SinYm
        {
            get => _sinYm;
        }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        public int SinDay
        {
            get => _sinDay;
        }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        public long RaiinNo
        {
            get => _raiinNo;
        }

        /// <summary>
        /// 剤番号
        /// SEQUENCE SIN_KOUI.RP_NO
        /// </summary>
        public int RpNo
        {
            get => _rpNo;
            set
            {
                _rpNo = value;
            }
        }

        /// <summary>
        /// 連番
        /// SIN_KOUI.SEQ_NO
        /// </summary>
        public int SeqNo
        {
            get => _seqNo;
            set
            {
                _seqNo = value;
            }
        }

        /// <summary>
        /// 回数
        /// 来院ごとの回数
        /// </summary>
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
            }
        }
        public int AdjCount { get; set; } = 0;

        /// <summary>
        /// 更新情報
        ///     0: 変更なし
        ///     1: 追加
        ///     2: 削除
        /// </summary>
        public int UpdateState
        {
            get { return _updateState; }
            set { _updateState = value; }
        }

        /// <summary>
        /// キー番号
        /// </summary>
        public long KeyNo
        {
            get { return _keyNo; }
            set { _keyNo = value; }
        }
    }
}
