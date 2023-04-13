using Domain.Types;

namespace UseCase.MedicalExamination.GetContainerMst
{
    public class KensaPrinterItem
    {
        public KensaPrinterItem(string itemCd, string containerName, long containerCd, bool isChecked, int kensaLabel, string selectedPrinterName, Thickness textBoxBorderThickness, Thickness comboboxBorderThickness, string itemName, int inoutKbn, int odrKouiKbn)
        {
            ItemCd = itemCd;
            ContainerName = containerName;
            ContainerCd = containerCd;
            IsChecked = isChecked;
            KensaLabel = kensaLabel;
            SelectedPrinterName = selectedPrinterName;
            TextBoxBorderThickness = textBoxBorderThickness;
            ComboboxBorderThickness = comboboxBorderThickness;
            ItemName = itemName;
            InoutKbn = inoutKbn;
            OdrKouiKbn = odrKouiKbn;
        }

        public string ItemCd { get; private set; }

        public string ContainerName { get; private set; }

        public long ContainerCd { get; private set; }

        public bool IsChecked { get; private set; }

        public int KensaLabel { get; private set; }

        public string SelectedPrinterName { get; private set; }

        public Thickness TextBoxBorderThickness { get; private set; }

        public Thickness ComboboxBorderThickness { get; private set; }

        public string ItemName { get; private set; }

        public int InoutKbn { get; private set; }

        public int OdrKouiKbn { get; private set; }
    }
}
