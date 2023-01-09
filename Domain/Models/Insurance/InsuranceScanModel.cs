namespace Domain.Models.Insurance
{
    public class InsuranceScanModel
    {
        public InsuranceScanModel(int hpId, long ptId, int hokenGrp, int hokenId, string fileName, int isDeleted)
        {
            HpId = hpId;
            PtId = ptId;
            HokenGrp = hokenGrp;
            HokenId = hokenId;
            FileName = fileName;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int HokenGrp { get; private set; }

        public int HokenId { get; private set; }

        public string FileName { get; private set; }

        public int IsDeleted { get; private set; }
    }
}
