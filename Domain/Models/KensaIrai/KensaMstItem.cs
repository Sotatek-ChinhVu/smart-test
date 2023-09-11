namespace Domain.Models.KensaIrai
{
    public class KensaMstItem
    {
        public string KensaItemCd { get; set; }

        public int KensaItemSeqNo { get; set; }

        public string CenterCd { get; set; } = string.Empty;

        public string KensaName { get; set; } = string.Empty;

        public string KensaKana { get; set; } = string.Empty;

        public string Unit { get; set; } = string.Empty;

        public int MaterialCd { get; set; }

        public int ContainerCd { get; set; }

        public string MaleStd { get; set; } = string.Empty;

        public string MaleStdLow { get; set; } = string.Empty;

        public string MaleStdHigh { get; set; } = string.Empty;

        public string FemaleStd { get; set; } = string.Empty;

        public string FemaleStdLow { get; set; } = string.Empty;

        public string FemaleStdHigh { get; set; } = string.Empty;

        public string Formula { get; set; } = string.Empty;

        public int Digit { get; set; }

        public string OyaItemCd { get; set; } = string.Empty;

        public int OyaItemSeqNo { get; set; }

        public long SortNo { get; set; }

        public string CenterItemCd1 { get; set; } = string.Empty;

        public string CenterItemCd2 { get; set; } = string.Empty;

        public int IsDeleted { get; set; }
    }
}
