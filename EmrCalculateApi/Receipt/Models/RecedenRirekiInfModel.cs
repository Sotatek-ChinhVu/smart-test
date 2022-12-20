using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Receipt.Models
{
    public class RecedenRirekiInfModel
    {
        public RecedenRirekiInf RecedenRirekiInf { get; } = null;

        public RecedenRirekiInfModel(RecedenRirekiInf recedenRirekiInf)
        {
            RecedenRirekiInf = recedenRirekiInf;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return RecedenRirekiInf.HpId; }
        }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId
        {
            get { return RecedenRirekiInf.PtId; }
        }

        /// <summary>
        /// 保険ID
        /// 
        /// </summary>
        public int HokenId
        {
            get { return RecedenRirekiInf.HokenId; }
        }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        public int SinYm
        {
            get { return RecedenRirekiInf.SinYm; }
        }

        /// <summary>
        /// 連番
        /// 同一HP_ID, PT_ID, HOKEN_ID, SIN_YM内での連番
        /// </summary>
        public int SeqNo
        {
            get { return RecedenRirekiInf.SeqNo; }
        }

        /// <summary>
        /// 検索番号
        /// 
        /// </summary>
        public string SearchNo
        {
            get { return RecedenRirekiInf.SearchNo; }
        }

        /// <summary>
        /// 履歴管理情報
        /// 
        /// </summary>
        public string Rireki
        {
            get { return RecedenRirekiInf.Rireki; }
        }

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        public int IsDeleted
        {
            get { return RecedenRirekiInf.IsDeleted; }
        }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get { return RecedenRirekiInf.CreateDate; }
        }

        /// <summary>
        /// 作成者ID
        /// 
        /// </summary>
        public int CreateId
        {
            get { return RecedenRirekiInf.CreateId; }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return RecedenRirekiInf.CreateMachine; }
        }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        public DateTime UpdateDate
        {
            get { return RecedenRirekiInf.UpdateDate; }
        }

        /// <summary>
        /// 更新者ID
        /// 
        /// </summary>
        public int UpdateId
        {
            get { return RecedenRirekiInf.UpdateId; }
        }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        public string UpdateMachine
        {
            get { return RecedenRirekiInf.UpdateMachine; }
        }


    }

}
