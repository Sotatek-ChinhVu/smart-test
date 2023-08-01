namespace Reporting.KensaLabel.Model
{
    public class KensaPrinterModel
    {
        public KensaPrinterModel(string itemCd, string containerName, long containerCd, int count, string printerName, int inoutKbn, int odrKouiKbn)
        {
            ItemCd = itemCd;
            ContainerName = containerName;
            ContainerCd = containerCd;
            Count = count;
            PrinterName = printerName;
            InoutKbn = inoutKbn;
            OdrKouiKbn = odrKouiKbn;
        }

        public string ItemCd { get; set; }

        public string ContainerName { get; set; }

        public long ContainerCd { get; set; }

        public int Count { get; set; }

        public string PrinterName { get; set; }

        public int InoutKbn { get; set; }

        public int OdrKouiKbn { get; set; }
    }
}
