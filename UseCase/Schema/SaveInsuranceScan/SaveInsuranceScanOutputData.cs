using UseCase.Core.Sync.Core;

namespace Schema.Insurance.SaveInsuranceScan
{
    public class SaveInsuranceScanOutputData : IOutputData
    {
        public SaveInsuranceScanOutputData(string urlImage, SaveInsuranceScanStatus status)
        {
            UrlImage = urlImage;
            Status = status;
        }

        public string UrlImage { get; private set; }

        public SaveInsuranceScanStatus Status { get; private set; }

    }
}
