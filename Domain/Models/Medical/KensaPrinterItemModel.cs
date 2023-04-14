using Domain.Types;
using System.Collections.ObjectModel;

namespace Domain.Models.Medical
{
    public class KensaPrinterItemModel
    {
        public KensaPrinterItemModel(string itemCd, string containerName, long containerCd, bool isChecked, int kensaLabel, string selectedPrinterName, Thickness textBoxBorderThickness, Thickness comboboxBorderThickness, string itemName, int inoutKbn, int odrKouiKbn)
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

        public KensaPrinterItemModel(string itemCd, string itemName, string containerName, int kensaLabel, long containerCd = 0, bool isChecked = false)
        {
            ItemCd = itemCd;
            ItemName = itemName;
            ContainerName = containerName;
            KensaLabel = kensaLabel;
            ContainerCd = containerCd;
            IsChecked = isChecked;
            SelectedPrinterName = string.Empty;
            TextBoxBorderThickness = new();
            ComboboxBorderThickness = new();
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

        public KensaPrinterItemModel ChangeKensaLabel(int kensaLabel)
        {
            KensaLabel = kensaLabel;
            return this;
        }

        public KensaPrinterItemModel ChangeTextBoxBorderThickness(Thickness textBoxBorderThickness)
        {
            TextBoxBorderThickness = textBoxBorderThickness;
            return this;
        }

        public KensaPrinterItemModel ChangeComboboxBorderThickness(Thickness comboboxBorderThickness)
        {
            ComboboxBorderThickness = comboboxBorderThickness;
            return this;
        }

        public KensaPrinterItemModel ChangeInoutKbnOdrKouiKbn(int inoutKbn, int odrKouiKbn)
        {
            InoutKbn = inoutKbn;
            OdrKouiKbn = odrKouiKbn;
            return this;
        }
    }
}
