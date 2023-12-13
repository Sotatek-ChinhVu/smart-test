using Entity.Tenant;

namespace Reporting.Accounting.Model
{
    public class CoSyunoNyukinModel
    {
        public SyunoNyukin SyunoNyukin { get; }
        public PaymentMethodMst PaymentMethod { get; }

        public CoSyunoNyukinModel(SyunoNyukin syunoNyukin, PaymentMethodMst payMethod)
        {
            SyunoNyukin = syunoNyukin;
            PaymentMethod = payMethod;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return SyunoNyukin.HpId; }
        }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId
        {
            get { return SyunoNyukin.PtId; }
        }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        public int SinDate
        {
            get { return SyunoNyukin.SinDate; }
        }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        public long RaiinNo
        {
            get { return SyunoNyukin.RaiinNo; }
        }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        public long SeqNo
        {
            get { return SyunoNyukin.SeqNo; }
        }

        /// <summary>
        /// 入金順番
        /// 同一来院に対して分割入金した場合の入金の順番
        /// </summary>
        public int SortNo
        {
            get { return SyunoNyukin.SortNo; }
        }

        /// <summary>
        /// 調整額
        /// 
        /// </summary>
        public int AdjustFutan
        {
            get { return SyunoNyukin.AdjustFutan; }
        }

        /// <summary>
        /// 入金額
        /// 
        /// </summary>
        public int NyukinGaku
        {
            get { return SyunoNyukin.NyukinGaku; }
        }

        /// <summary>
        /// 支払方法コード
        /// PAYMENT_METHOD_MST.PAYMENT_METHOD_CD
        /// </summary>
        public int PaymentMethodCd
        {
            get { return SyunoNyukin.PaymentMethodCd; }
        }
        public string PayMehod
        {
            get { return PaymentMethod != null ? PaymentMethod.PayName ?? string.Empty : string.Empty; }
        }
        /// <summary>
        /// 入金日
        /// 
        /// </summary>
        public int NyukinDate
        {
            get { return SyunoNyukin.NyukinDate; }
        }

        /// <summary>
        /// 受付種別
        /// UKETUKE_SBT_MST.KBN_ID（入金時の受付種別）
        /// </summary>
        public int UketukeSbt
        {
            get { return SyunoNyukin.UketukeSbt; }
        }

        /// <summary>
        /// 入金コメント
        /// 
        /// </summary>
        public string NyukinCmt
        {
            get { return SyunoNyukin.NyukinCmt ?? string.Empty; }
        }

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        public int IsDeleted
        {
            get { return SyunoNyukin.IsDeleted; }
        }
    }
}
