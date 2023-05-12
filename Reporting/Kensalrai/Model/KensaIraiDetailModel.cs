using Entity.Tenant;

namespace Reporting.Kensalrai.Model
{
    public class KensaIraiDetailModel
    {
        public KensaMst KensaMst { get; } = null;

        public TenMst TenMst { get; } = null;

        public KensaIraiDetailModel(bool isSelected, long rpNo, long rpEdaNo, int rowNo, int seqNo, KensaMst kensaMst)
        {
            IsSelected = isSelected;
            RpNo = rpNo;
            RpEdaNo = rpEdaNo;
            RowNo = rowNo;
            SeqNo = seqNo;
            KensaMst = kensaMst;
        }

        public KensaIraiDetailModel(bool isSelected, long rpNo, long rpEdaNo, int rowNo, int seqNo, TenMst tenMst, KensaMst kensaMst)
        {
            IsSelected = isSelected;
            RpNo = rpNo;
            RpEdaNo = rpEdaNo;
            RowNo = rowNo;
            SeqNo = seqNo;
            TenMst = tenMst;
            KensaMst = kensaMst == null ? new KensaMst() : kensaMst;
        }

        public string TenKensaItemCd
        {
            get => TenMst?.KensaItemCd;
        }

        public string ItemCd
        {
            get => TenMst.ItemCd;
        }

        public string ItemName
        {
            get => TenMst.Name;
        }

        public string KanaName1
        {
            get => TenMst.KanaName1;
        }

        public string CenterCd
        {
            get => KensaMst.CenterCd;
        }

        public string KensaItemCd
        {
            get { return KensaMst.KensaItemCd; }
        }
        public string CenterItemCd
        {
            get { return KensaMst.CenterItemCd1; }
        }
        public string KensaKana
        {
            get { return KensaMst.KensaKana; }
        }
        public string KensaName
        {
            get { return KensaMst.KensaName; }
        }
        public long ContainerCd
        {
            get { return KensaMst.ContainerCd; }
        }
        /// <summary>
        /// 容器名
        /// </summary>
        public string ContainerName { get; set; } = "";
        /// <summary>
        /// 選択されているかどうか
        /// </summary>
        public bool IsSelected { get; set; } = true;
        /// <summary>
        /// RpNo（ODR_INF_DETAILとのリレーション用）
        /// </summary>
        public long RpNo { get; set; } = 0;
        /// <summary>
        /// RpEdaNo（ODR_INF_DETAILとのリレーション用）
        /// </summary>
        public long RpEdaNo { get; set; } = 0;
        /// <summary>
        /// RowNo（ODR_INF_DETAILとのリレーション用）
        /// </summary>
        public int RowNo { get; set; } = 0;
        /// <summary>
        /// シーケンス。ソートに使用する番号
        /// </summary>
        public int SeqNo { get; set; } = 0;

        public bool CheckDefaultValue()
        {
            return string.IsNullOrEmpty(ItemCd) && string.IsNullOrEmpty(ItemName) && string.IsNullOrEmpty(KanaName1);
        }
    }
}
