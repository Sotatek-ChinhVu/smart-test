namespace Reporting.Kensalrai.Model
{
    public class CoKensaIraiPrintDataModel
    {
        public CoKensaIraiPrintDataModel()
        {
            ItemDatas = new List<(string kensaItemCd, string CenterItemCd, string KensaKanaName, string Yoki)>();
        }

        public int SinDate { get; set; } = 0;
        public long IraiCd { get; set; } = 0;
        public long RaiinNo { get; set; } = 0;
        public long PtNum { get; set; } = 0;
        public string PtName { get; set; } = "";
        public int Age { get; set; } = 0;
        public string Sex { get; set; } = "";
        public string Sikyu { get; set; } = "";
        public string Toseki { get; set; } = "";
        public string KaName { get; set; } = "";
        public string TantoName { get; set; } = "";
        public string TantoKanaName { get; set; } = "";
        public string Height { get; set; } = "";
        public string Weight { get; set; } = "";

        public int SeqNo { get; set; } = 0;

        public List<(string kensaItemCd, string CenterItemCd, string KensaKanaName, string Yoki)>
            ItemDatas
        { get; set; }
    }
}
