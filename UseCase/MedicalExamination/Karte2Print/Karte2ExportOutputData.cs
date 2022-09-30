using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.Karte2Print
{
    public class Karte2ExportOutputData : IOutputData
    {
        public Karte2ExportOutputData(string url, Karte2PrintStatus status)
        {
            Url = url;
            Status = status;
        }

        public Karte2ExportOutputData(Karte2PrintStatus status)
        {
            Status = status;
            Url = String.Empty;
        }

        public string Url { get; private set; }
        public Karte2PrintStatus Status { get; private set; }
    }
}
