namespace UseCase.Accounting.GetAccountingSystemConf
{
    public class AccountingConfigDto
    {
        public AccountingConfigDto()
        {
        }

        public AccountingConfigDto(bool isVisiblePrintDrgLabel, bool isCheckedPrintDrgLabel, bool isVisiblePrintOutDrg, bool isCheckedPrintOutDrg, bool isCheckedPrintReceipt, bool isCheckedPrintDetail, bool isVisiblePrintDrgLabelSize, bool isVisiblePrintDrgInf, bool isCheckedPrintDrgInf, bool isVisiblePrintDrgNote, bool isCheckedPrintDrgNote)
        {
            IsVisiblePrintDrgLabel = isVisiblePrintDrgLabel;
            IsCheckedPrintDrgLabel = isCheckedPrintDrgLabel;
            IsVisiblePrintOutDrg = isVisiblePrintOutDrg;
            IsCheckedPrintOutDrg = isCheckedPrintOutDrg;
            IsCheckedPrintReceipt = isCheckedPrintReceipt;
            IsCheckedPrintDetail = isCheckedPrintDetail;
            IsVisiblePrintDrgLabelSize = isVisiblePrintDrgLabelSize;
            IsVisiblePrintDrgInf = isVisiblePrintDrgInf;
            IsCheckedPrintDrgInf = isCheckedPrintDrgInf;
            IsVisiblePrintDrgNote = isVisiblePrintDrgNote;
            IsCheckedPrintDrgNote = isCheckedPrintDrgNote;
        }

        public bool IsVisiblePrintDrgLabel { get; private set; }
        public bool IsCheckedPrintDrgLabel { get; private set; }
        public bool IsVisiblePrintOutDrg { get; private set; }
        public bool IsCheckedPrintOutDrg { get; private set; }
        public bool IsCheckedPrintReceipt { get; private set; }
        public bool IsCheckedPrintDetail { get; private set; }
        public bool IsVisiblePrintDrgLabelSize { get; private set; }
        public bool IsVisiblePrintDrgInf { get; private set; }
        public bool IsCheckedPrintDrgInf { get; private set; }
        public bool IsVisiblePrintDrgNote { get; private set; }
        public bool IsCheckedPrintDrgNote { get; private set; }
    }
}
