using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Receipt.Models
{
    public class SyobyoKeikaModel
    {
        public SyobyoKeika SyobyoKeika { get; } = null;

        public SyobyoKeikaModel(SyobyoKeika syobyoKeika)
        {
            SyobyoKeika = syobyoKeika;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return SyobyoKeika.HpId; }
        }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId
        {
            get { return SyobyoKeika.PtId; }
        }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        public int SinYm
        {
            get { return SyobyoKeika.SinYm; }
        }

        /// <summary>
        /// 診療日
        /// アフターケアの場合に使用
        /// </summary>
        public int SinDay
        {
            get { return SyobyoKeika.SinDay; }
        }

        /// <summary>
        /// 保険ID
        /// 
        /// </summary>
        public int HokenId
        {
            get { return SyobyoKeika.HokenId; }
        }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        public int SeqNo
        {
            get { return SyobyoKeika.SeqNo; }
        }

        /// <summary>
        /// 傷病の経過
        /// 
        /// </summary>
        public string Keika
        {
            get { return SyobyoKeika.Keika ?? string.Empty; }
        }

        /// <summary>
        /// 削除区分
        /// 1: 削除
        /// </summary>
        public int IsDeleted
        {
            get { return SyobyoKeika.IsDeleted; }
        }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get { return SyobyoKeika.CreateDate; }
        }

        /// <summary>
        /// 作成者ID
        /// 
        /// </summary>
        public int CreateId
        {
            get { return SyobyoKeika.CreateId; }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return SyobyoKeika.CreateMachine ?? string.Empty; }
        }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        public DateTime UpdateDate
        {
            get { return SyobyoKeika.UpdateDate; }
        }

        /// <summary>
        /// 更新者ID
        /// 
        /// </summary>
        public int UpdateId
        {
            get { return SyobyoKeika.UpdateId; }
        }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        public string UpdateMachine
        {
            get { return SyobyoKeika.UpdateMachine ?? string.Empty; }
        }


    }

}
