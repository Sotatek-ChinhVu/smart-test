using UseCase.Core.Sync.Core;

namespace Schema.Insurance.SaveInsuranceScan
{
    public class SaveInsuranceScanOutputData : IOutputData
    {
        public SaveInsuranceScanOutputData(SaveInsuranceScanStatus status, IEnumerable<string> urlCreateds)
        {
            Status = status;
            UrlCreateds = urlCreateds;
        }

        public SaveInsuranceScanStatus Status { get; private set; }

        public IEnumerable<string> UrlCreateds { get;private set; }

    }
}
