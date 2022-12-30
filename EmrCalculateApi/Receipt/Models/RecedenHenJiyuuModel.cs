using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Receipt.Models
{
    public class RecedenHenJiyuuModel
    {
        public RecedenHenJiyuu RecedenHenJiyuu { get; } = null;

        public RecedenHenJiyuuModel(RecedenHenJiyuu recedenHenJiyuu)
        {
            RecedenHenJiyuu = recedenHenJiyuu;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return RecedenHenJiyuu.HpId; }
        }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId
        {
            get { return RecedenHenJiyuu.PtId; }
        }

        /// <summary>
        /// 保険ID
        /// 
        /// </summary>
        public int HokenId
        {
            get { return RecedenHenJiyuu.HokenId; }
        }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        public int SinYm
        {
            get { return RecedenHenJiyuu.SinYm; }
        }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        public int SeqNo
        {
            get { return RecedenHenJiyuu.SeqNo; }
        }

        /// <summary>
        /// 返戻事由コード
        /// 0
        /// </summary>
        public string HenreiJiyuuCd
        {
            get { return RecedenHenJiyuu.HenreiJiyuuCd; }
        }

        /// <summary>
        /// 返戻事由
        /// 
        /// </summary>
        public string HenreiJiyuu
        {
            get { return RecedenHenJiyuu.HenreiJiyuu; }
        }

        /// <summary>
        /// 補足情報
        /// 
        /// </summary>
        public string Hosoku
        {
            get { return RecedenHenJiyuu.Hosoku; }
        }

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        public int IsDeleted
        {
            get { return RecedenHenJiyuu.IsDeleted; }
        }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get { return RecedenHenJiyuu.CreateDate; }
        }

        /// <summary>
        /// 作成者ID
        /// 
        /// </summary>
        public int CreateId
        {
            get { return RecedenHenJiyuu.CreateId; }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return RecedenHenJiyuu.CreateMachine; }
        }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        public DateTime UpdateDate
        {
            get { return RecedenHenJiyuu.UpdateDate; }
        }

        /// <summary>
        /// 更新者ID
        /// 
        /// </summary>
        public int UpdateId
        {
            get { return RecedenHenJiyuu.UpdateId; }
        }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        public string UpdateMachine
        {
            get { return RecedenHenJiyuu.UpdateMachine; }
        }


    }

}
