using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Futan.Models
{
    public class SyunoSeikyuModel
    {
        public SyunoSeikyu SyunoSeikyu { get; }

        public SyunoSeikyuModel(SyunoSeikyu syunoSeikyu)
        {
            SyunoSeikyu = syunoSeikyu;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return SyunoSeikyu.HpId; }
            set
            {
                if (SyunoSeikyu.HpId == value) return;
                SyunoSeikyu.HpId = value;
            }
        }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId
        {
            get { return SyunoSeikyu.PtId; }
            set
            {
                if (SyunoSeikyu.PtId == value) return;
                SyunoSeikyu.PtId = value;
            }
        }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        public int SinDate
        {
            get { return SyunoSeikyu.SinDate; }
            set
            {
                if (SyunoSeikyu.SinDate == value) return;
                SyunoSeikyu.SinDate = value;
            }
        }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        public long RaiinNo
        {
            get { return SyunoSeikyu.RaiinNo; }
            set
            {
                if (SyunoSeikyu.RaiinNo == value) return;
                SyunoSeikyu.RaiinNo = value;
            }
        }

        /// <summary>
        /// 入金区分
        /// 0:未精算 1:一部精算 2:免除 3:精算済
        /// </summary>
        public int NyukinKbn
        {
            get { return SyunoSeikyu.NyukinKbn; }
            set
            {
                if (SyunoSeikyu.NyukinKbn == value) return;
                SyunoSeikyu.NyukinKbn = value;
            }
        }

        /// <summary>
        /// 請求点数
        /// 診療点数（KAIKEI_INF.TENSU）
        /// </summary>
        public int SeikyuTensu
        {
            get { return SyunoSeikyu.SeikyuTensu; }
            set
            {
                if (SyunoSeikyu.SeikyuTensu == value) return;
                SyunoSeikyu.SeikyuTensu = value;
            }
        }

        /// <summary>
        /// 調整額
        ///     KAIKEI_INF.ADJUST_FUTAN
        /// </summary>
        public int AdjustFutan
        {
            get { return SyunoSeikyu.AdjustFutan; }
            set
            {
                if (SyunoSeikyu.AdjustFutan == value) return;
                SyunoSeikyu.AdjustFutan = value;
            }
        }

        /// <summary>
        /// 請求額
        /// 請求額 （KAIKEI_INF.TOTAL_PT_FUTAN）
        /// </summary>
        public int SeikyuGaku
        {
            get { return SyunoSeikyu.SeikyuGaku; }
            set
            {
                if (SyunoSeikyu.SeikyuGaku == value) return;
                SyunoSeikyu.SeikyuGaku = value;
            }
        }

        /// <summary>
        /// 請求詳細
        /// 診療明細（SIN_KOUI.DETAIL_DATA）
        /// </summary>
        public string SeikyuDetail
        {
            get { return SyunoSeikyu.SeikyuDetail ?? string.Empty; }
            set
            {
                if (SyunoSeikyu.SeikyuDetail == value) return;
                SyunoSeikyu.SeikyuDetail = value;
            }
        }

        /// <summary>
        /// 新請求点数
        ///     KAIKEI_INF.TENSU
        /// </summary>
        public int NewSeikyuTensu
        {
            get { return SyunoSeikyu.NewSeikyuTensu; }
            set
            {
                if (SyunoSeikyu.NewSeikyuTensu == value) return;
                SyunoSeikyu.NewSeikyuTensu = value;
            }
        }

        /// <summary>
        /// 新調整額
        ///     KAIKEI_INF.ADJUST_FUTAN
        /// </summary>
        public int NewAdjustFutan
        {
            get { return SyunoSeikyu.NewAdjustFutan; }
            set
            {
                if (SyunoSeikyu.NewAdjustFutan == value) return;
                SyunoSeikyu.NewAdjustFutan = value;
            }
        }

        /// <summary>
        /// 新請求額
        ///     KAIKEI_INF.TOTAL_PT_FUTAN
        /// </summary>
        public int NewSeikyuGaku
        {
            get { return SyunoSeikyu.NewSeikyuGaku; }
            set
            {
                if (SyunoSeikyu.NewSeikyuGaku == value) return;
                SyunoSeikyu.NewSeikyuGaku = value;
            }
        }

        /// <summary>
        /// 新請求詳細
        ///     SIN_KOUI.DETAIL_DATA
        /// </summary>
        public string NewSeikyuDetail
        {
            get { return SyunoSeikyu.NewSeikyuDetail ?? string.Empty; }
            set
            {
                if (SyunoSeikyu.NewSeikyuDetail == value) return;
                SyunoSeikyu.NewSeikyuDetail = value;
            }
        }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get { return SyunoSeikyu.CreateDate; }
            set
            {
                if (SyunoSeikyu.CreateDate == value) return;
                SyunoSeikyu.CreateDate = value;
            }
        }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        public int CreateId
        {
            get { return SyunoSeikyu.CreateId; }
            set
            {
                if (SyunoSeikyu.CreateId == value) return;
                SyunoSeikyu.CreateId = value;
            }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return SyunoSeikyu.CreateMachine ?? string.Empty; }
            set
            {
                if (SyunoSeikyu.CreateMachine == value) return;
                SyunoSeikyu.CreateMachine = value;
            }
        }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        public DateTime UpdateDate
        {
            get { return SyunoSeikyu.UpdateDate; }
            set
            {
                if (SyunoSeikyu.UpdateDate == value) return;
                SyunoSeikyu.UpdateDate = value;
            }
        }

        /// <summary>
        /// 更新者
        /// 
        /// </summary>
        public int UpdateId
        {
            get { return SyunoSeikyu.UpdateId; }
            set
            {
                if (SyunoSeikyu.UpdateId == value) return;
                SyunoSeikyu.UpdateId = value;
            }
        }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        public string UpdateMachine
        {
            get { return SyunoSeikyu.UpdateMachine ?? string.Empty; }
            set
            {
                if (SyunoSeikyu.UpdateMachine == value) return;
                SyunoSeikyu.UpdateMachine = value;
            }
        }
    }
}
