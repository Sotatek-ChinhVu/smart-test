using Entity.Tenant;
using EmrCalculateApi.Ika.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Receipt.Models
{
    public class SyoukiInfModel
    {
        public SyoukiInf SyoukiInf { get; } = null;

        public SyoukiInfModel(SyoukiInf syoukiInf)
        {
            SyoukiInf = syoukiInf;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return SyoukiInf.HpId; }
        }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId
        {
            get { return SyoukiInf.PtId; }
        }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        public int SinYm
        {
            get { return SyoukiInf.SinYm; }
        }

        /// <summary>
        /// 保険ID
        /// 
        /// </summary>
        public int HokenId
        {
            get { return SyoukiInf.HokenId; }
        }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        public int SeqNo
        {
            get { return SyoukiInf.SeqNo; }
        }

        /// <summary>
        /// 並び順
        /// 
        /// </summary>
        public int SortNo
        {
            get { return SyoukiInf.SortNo; }
        }

        /// <summary>
        /// 症状詳記区分
        /// SYOUKI_KBN_MST.SYOUKI_KBN
        /// </summary>
        public int SyoukiKbn
        {
            get { return SyoukiInf.SyoukiKbn; }
        }

        /// <summary>
        /// 症状詳記
        /// 
        /// </summary>
        public string Syouki
        {
            get { return SyoukiInf.Syouki ?? string.Empty; }
        }

        /// <summary>
        /// 削除区分
        /// 1: 削除
        /// </summary>
        public int IsDeleted
        {
            get { return SyoukiInf.IsDeleted; }
        }

    }

}
