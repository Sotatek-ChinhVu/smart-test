using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Ika.Models
{
    public class SanteiInfDetailModel
    {
        public SanteiInfDetail SanteiInfDetail { get; } = null;

        public SanteiInfDetailModel(SanteiInfDetail santeiInfDetail)
        {
            SanteiInfDetail = santeiInfDetail;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return SanteiInfDetail.HpId; }
        }

        /// <summary>
        /// 患者ID
        /// ○
        /// </summary>
        public long PtId
        {
            get { return SanteiInfDetail.PtId; }
        }

        /// <summary>
        /// 項目コード
        /// ○
        /// </summary>
        public string ItemCd
        {
            get { return SanteiInfDetail.ItemCd ?? string.Empty; }
        }

        /// <summary>
        /// 連番
        /// ○
        /// </summary>
        public int SeqNo
        {
            get { return SanteiInfDetail.SeqNo; }
        }

        /// <summary>
        /// 有効期限
        /// ○
        /// </summary>
        public int EndDate
        {
            get { return SanteiInfDetail.EndDate; }
        }

        /// <summary>
        /// 起算種別
        /// 1: 初回算定 2:発症 3:急性憎悪 4:治療開始 5:手術 6:初回診断
        /// </summary>
        public int KisanSbt
        {
            get { return SanteiInfDetail.KisanSbt; }
        }

        /// <summary>
        /// 起算日
        /// 
        /// </summary>
        public int KisanDate
        {
            get { return SanteiInfDetail.KisanDate; }
        }

        /// <summary>
        /// 病名
        /// 
        /// </summary>
        public string Byomei
        {
            get { return SanteiInfDetail.Byomei ?? string.Empty; }
        }

        /// <summary>
        /// 補足コメント
        /// 
        /// </summary>
        public string HosokuComment
        {
            get { return SanteiInfDetail.HosokuComment ?? string.Empty; }
        }

        /// <summary>
        /// コメント
        /// 
        /// </summary>
        public string Comment
        {
            get { return SanteiInfDetail.Comment ?? string.Empty; }
        }

    }

}
