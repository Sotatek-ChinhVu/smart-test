namespace Domain.Models.SetKbn
{
    public class SetKbnMst
    {
        public SetKbnMst(int hpId, int setKbn, int setKbnEdaNo, string setKbnName, int kaCd, int docCd, int isDeleted, int generationId)
        {
            HpId = hpId;
            SetKbn = setKbn;
            SetKbnEdaNo = setKbnEdaNo;
            SetKbnName = setKbnName;
            KaCd = kaCd;
            DocCd = docCd;
            IsDeleted = isDeleted;
            GenerationId = generationId;
        }

        public int HpId { get; set; }
        public int SetKbn { get; set; }
        public int SetKbnEdaNo { get; set; }
        public string SetKbnName { get; set; }
        public int KaCd { get; set; }
        public int DocCd { get; set; }
        public int IsDeleted { get; set; }
        public int GenerationId { get; set; }
    }
}
