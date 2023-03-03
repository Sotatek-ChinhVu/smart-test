namespace Domain.Models.Insurance
{
    public class InsuranceScanModel
    {
        public InsuranceScanModel(int hpId, long ptId, long seqNo, int hokenGrp, int hokenId, string fileName, Stream file, int isDeleted)
        {
            HpId = hpId;
            PtId = ptId;
            SeqNo = seqNo;
            HokenGrp = hokenGrp;
            HokenId = hokenId;
            FileName = fileName;
            FileNameDisplay = string.Empty;
            File = file;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public long SeqNo { get; private set; }

        public int HokenGrp { get; private set; }

        public int HokenId { get; private set; }

        public string FileName { get; private set; }

        public string FileNameDisplay { get; private set; }

        public Stream File { get; private set; }

        public int IsDeleted { get; private set; }

        public void SetDisplayImage(string prefix) => FileNameDisplay = prefix + FileName;
    }
}
