namespace Domain.Models.Insurance
{
    public class InsuranceScanModel
    {
        public InsuranceScanModel(int hpId, long ptId, int hokenGrp, int hokenId, string oldImage, string fileName)
        {
            HpId = hpId;
            PtId = ptId;
            HokenGrp = hokenGrp;
            HokenId = hokenId;
            OldImage = oldImage;
            FileName = fileName;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int HokenGrp { get; private set; }

        public int HokenId { get; private set; }

        public string OldImage { get; private set; } = string.Empty;

        public string FileName { get; private set; } = string.Empty;
    }
}
