using UseCase.MedicalExamination.GetHistory;

namespace DevExpress.Models
{
    public class Karte2ExportModel
    {
        public int HpId { get; set; }

        public long PtId { get; set; }

        public string KanaName { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Sex { get; set; } = string.Empty;

        public string Birthday { get; set; } = string.Empty;

        public string CurrentTime { get; set; } = string.Empty;

        public string StartDate { get; set; } = string.Empty;

        public string EndDate { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public List<HistoryKarteOdrRaiinItem> HistoryKarteOdrRaiinItems { get; set; } = new List<HistoryKarteOdrRaiinItem>();
    }

}

