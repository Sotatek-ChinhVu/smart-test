using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Ika.Models
{
    public class PtHokenPatternModel
    {
        public PtHokenPattern PtHokenPattern { get; } = null;

        private string _kohi1Houbetu;
        private int _kohi1HokenSbtKbn;
        private int _kohi1HokenNo;
        private int _kohi1HokenEdaNo;
        /// <summary>
        /// 公費１の優先順位
        /// </summary>
        private string _kohi1PriorityNo;

        private string _kohi2Houbetu;
        private int _kohi2HokenSbtKbn;
        private int _kohi2HokenNo;
        private int _kohi2HokenEdaNo;
        /// <summary>
        /// 公費２の優先順位
        /// </summary>
        private string _kohi2PriorityNo;

        private string _kohi3Houbetu;
        private int _kohi3HokenSbtKbn;
        private int _kohi3HokenNo;
        private int _kohi3HokenEdaNo;
        /// <summary>
        /// 公費３の優先順位
        /// </summary>
        private string _kohi3PriorityNo;

        private string _kohi4Houbetu;
        private int _kohi4HokenSbtKbn;
        private int _kohi4HokenNo;
        private int _kohi4HokenEdaNo;
        /// <summary>
        /// 公費４の優先順位
        /// </summary>
        private string _kohi4PriorityNo;

        public PtHokenPatternModel(PtHokenPattern ptHokenPattern, 
            string kohi1Houbetu = "", int kohi1HokenSbtKbn = 0, int kohi1HokenNo = 0, int kohi1HokenEdaNo = 0, string kohi1PriorityNo = "",
            string kohi2Houbetu = "", int kohi2HokenSbtKbn = 0, int kohi2HokenNo = 0, int kohi2HokenEdaNo = 0, string kohi2PriorityNo = "",
            string kohi3Houbetu = "", int kohi3HokenSbtKbn = 0, int kohi3HokenNo = 0, int kohi3HokenEdaNo = 0, string kohi3PriorityNo = "",
            string kohi4Houbetu = "", int kohi4HokenSbtKbn = 0, int kohi4HokenNo = 0, int kohi4HokenEdaNo = 0, string kohi4PriorityNo = "")
        {
            PtHokenPattern = ptHokenPattern;
            _kohi1Houbetu = kohi1Houbetu;
            _kohi1HokenSbtKbn = kohi1HokenSbtKbn;
            _kohi1HokenNo = kohi1HokenNo;
            _kohi1HokenEdaNo = kohi1HokenEdaNo;
            _kohi1PriorityNo = kohi1PriorityNo;
            _kohi2Houbetu = kohi2Houbetu;
            _kohi2HokenSbtKbn = kohi2HokenSbtKbn;
            _kohi2HokenNo = kohi2HokenNo;
            _kohi2HokenEdaNo = kohi2HokenEdaNo;
            _kohi2PriorityNo = kohi2PriorityNo;
            _kohi3Houbetu = kohi3Houbetu;
            _kohi3HokenSbtKbn = kohi3HokenSbtKbn;
            _kohi3HokenNo = kohi3HokenNo;
            _kohi3HokenEdaNo = kohi3HokenEdaNo;
            _kohi3PriorityNo = kohi3PriorityNo;
            _kohi4Houbetu = kohi4Houbetu;
            _kohi4HokenSbtKbn = kohi4HokenSbtKbn;
            _kohi4HokenNo = kohi4HokenNo;
            _kohi4HokenEdaNo = kohi4HokenEdaNo;
            _kohi4PriorityNo = kohi4PriorityNo;
        }

        /// <summary>
        /// 患者保険組合せ
        ///  保険メモもしくは、組合せの変更があった場合は履歴管理する。                  
        ///  適用開始日・適用終了日の変更は各テーブルで履歴管理されるため、このテーブルは更新者情報の更新のみとする。                  
        /// </summary>
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId
        {
            get { return PtHokenPattern.HpId; }
        }

        /// <summary>
        /// 患者ID
        ///  患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId
        {
            get { return PtHokenPattern.PtId; }
        }

        /// <summary>
        /// 保険ID
        ///  患者別に保険情報を識別するための固有の番号
        /// </summary>
        public int HokenPid
        {
            get { return PtHokenPattern.HokenPid; }
        }

        /// <summary>
        /// 連番
        /// </summary>
        public long SeqNo
        {
            get { return PtHokenPattern.SeqNo; }
        }

        /// <summary>
        /// 保険区分
        ///  0:自費
        ///  1:社保
        ///  2:国保
        ///  11:労災(短期給付)
        ///  12:労災(傷病年金)
        ///  13:アフターケア
        ///  14:自賠責
        /// </summary>
        public int HokenKbn
        {
            get { return PtHokenPattern.HokenKbn; }
        }

        /// <summary>
        /// 保険種別コード
        ///  0:      下記以外
        ///  11..14: 社保単独～４併
        ///  21..24: 国保単独～４併
        ///  31..34: 後期単独～４併
        ///  41..44: 退職単独～４併
        ///  51..54: 公費単独～４併
        /// </summary>
        public int HokenSbtCd
        {
            get { return PtHokenPattern.HokenSbtCd; }
        }

        /// <summary>
        /// 主保険 保険ID
        /// </summary>
        public int HokenId
        {
            get { return PtHokenPattern.HokenId; }
        }

        /// <summary>
        /// 公費１ 保険ID
        /// </summary>
        public int Kohi1Id
        {
            get { return PtHokenPattern.Kohi1Id; }
        }

        /// <summary>
        /// 公費２ 保険ID
        /// </summary>
        public int Kohi2Id
        {
            get { return PtHokenPattern.Kohi2Id; }
        }

        /// <summary>
        /// 公費３ 保険ID
        /// </summary>
        public int Kohi3Id
        {
            get { return PtHokenPattern.Kohi3Id; }
        }

        /// <summary>
        /// 公費４ 保険ID
        /// </summary>
        public int Kohi4Id
        {
            get { return PtHokenPattern.Kohi4Id; }
        }

        /// <summary>
        /// 保険メモ
        /// </summary>
        public string HokenMemo
        {
            get { return PtHokenPattern.HokenMemo ?? string.Empty; }
        }

        /// <summary>
        /// 適用開始日
        ///  主保険の適用開始日(主保険を持たない場合は公費１ or 労災)          
        /// </summary>
        public int StartDate
        {
            get { return PtHokenPattern.StartDate; }
        }

        /// <summary>
        /// 適用終了日
        ///  主保険の適用終了日(主保険を持たない場合は公費１ or 労災)          
        /// </summary>
        public int EndDate
        {
            get { return PtHokenPattern.EndDate; }
        }

        /// <summary>
        /// 削除区分
        ///  1:削除
        /// </summary>
        public int IsDeleted
        {
            get { return PtHokenPattern.IsDeleted; }
        }

        ///// <summary>
        ///// 作成日時 
        ///// </summary>
        //public DateTime CreateDate
        //{
        //    get { return PtHokenPattern.CreateDate; }
        //}

        ///// <summary>
        ///// 作成者  
        ///// </summary>
        //public int CreateId
        //{
        //    get { return PtHokenPattern.CreateId; }
        //}

        ///// <summary>
        ///// 作成端末   
        ///// </summary>
        //public string CreateMachine
        //{
        //    get { return PtHokenPattern.CreateMachine; }
        //}

        ///// <summary>
        ///// 更新日時   
        ///// </summary>
        //public DateTime UpdateDate
        //{
        //    get { return PtHokenPattern.UpdateDate; }
        //}

        ///// <summary>
        ///// 更新者   
        ///// </summary>
        //public int UpdateId
        //{
        //    get { return PtHokenPattern.UpdateId; }
        //}

        ///// <summary>
        ///// 更新端末   
        ///// </summary>
        //public string UpdateMachine
        //{
        //    get { return PtHokenPattern.UpdateMachine; }
        //}

        public string Kohi1Houbetu
        {
            get { return _kohi1Houbetu ?? string.Empty; }
        }
        public int Kohi1HokenSbtKbn
        {
            get { return _kohi1HokenSbtKbn; }
        }
        public int Kohi1HokenNo
        {
            get { return _kohi1HokenNo; }
        }
        public int Kohi1HokenEdaNo
        {
            get { return _kohi1HokenEdaNo; }
        }
        /// <summary>
        /// 公費１の優先順位
        /// </summary>
        public string Kohi1PriorityNo
        {
            get { return _kohi1PriorityNo; }
        }

        public string Kohi2Houbetu
        {
            get { return _kohi2Houbetu ?? string.Empty; }
        }
        public int Kohi2HokenSbtKbn
        {
            get { return _kohi2HokenSbtKbn; }
        }
        public int Kohi2HokenNo
        {
            get { return _kohi2HokenNo; }
        }
        public int Kohi2HokenEdaNo
        {
            get { return _kohi2HokenEdaNo; }
        }

        /// <summary>
        /// 公費２の優先順位
        /// </summary>
        public string Kohi2PriorityNo
        {
            get { return _kohi2PriorityNo; }
        }

        public string Kohi3Houbetu
        {
            get { return _kohi3Houbetu ?? string.Empty; }
        }
        public int Kohi3HokenSbtKbn
        {
            get { return _kohi3HokenSbtKbn; }
        }
        public int Kohi3HokenNo
        {
            get { return _kohi3HokenNo; }
        }
        public int Kohi3HokenEdaNo
        {
            get { return _kohi3HokenEdaNo; }
        }


        /// <summary>
        /// 公費３の優先順位
        /// </summary>
        public string Kohi3PriorityNo
        {
            get { return _kohi3PriorityNo; }
        }

        public string Kohi4Houbetu
        {
            get { return _kohi4Houbetu ?? string.Empty; }
        }

        public int Kohi4HokenSbtKbn
        {
            get { return _kohi4HokenSbtKbn; }
        }
        public int Kohi4HokenNo
        {
            get { return _kohi4HokenNo; }
        }
        public int Kohi4HokenEdaNo
        {
            get { return _kohi4HokenEdaNo; }
        }


        /// <summary>
        /// 公費４の優先順位
        /// </summary>
        public string Kohi4PriorityNo
        {
            get { return _kohi4PriorityNo; }
        }

        /// <summary>
        /// 計算順番
        ///  公費優先順位
        /// </summary>
        public string SortKey
        {
            get
            {
                return String.Format(
                "{0}{1}{2}{3}{4}{5}{6}{7}{8:D4}0",
                Kohi1PriorityNo?.PadLeft(5, '9') ?? "99999", Kohi1Houbetu?.PadLeft(3, '0') ?? "999",
                Kohi2PriorityNo?.PadLeft(5, '9') ?? "99999", Kohi2Houbetu?.PadLeft(3, '0') ?? "999",
                Kohi3PriorityNo?.PadLeft(5, '9') ?? "99999", Kohi3Houbetu?.PadLeft(3, '0') ?? "999",
                Kohi4PriorityNo?.PadLeft(5, '9') ?? "99999", Kohi4Houbetu?.PadLeft(3, '0') ?? "999",
                HokenPid);
            }
        }

        public int GetHokenSbtKbn(int kohiId)
        {
            int ret = 0;

            if(Kohi1Id == kohiId)
            {
                ret = Kohi1HokenSbtKbn;
            }
            else if (Kohi2Id == kohiId)
            {
                ret = Kohi2HokenSbtKbn;
            }
            else if (Kohi3Id == kohiId)
            {
                ret = Kohi3HokenSbtKbn;
            }
            else if (Kohi4Id == kohiId)
            {
                ret = Kohi4HokenSbtKbn;
            }

            return ret;
        }
    }

}
