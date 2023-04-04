namespace Domain.Models.Receipt.ReceiptCreation
{
    public class ReceInfValidateModel
    {
        public ReceInfValidateModel(long ptId, long ptNum, int sinYm, int isTester, int hokenKbn, int isPaperRece, int rousaiSaigaiKbn, int hokenId, int rousaiSyobyoDate)
        {
            PtId = ptId;
            PtNum = ptNum;
            SinYm = sinYm;
            IsTester = isTester;
            HokenKbn = hokenKbn;
            IsPaperRece = isPaperRece;
            RousaiSaigaiKbn = rousaiSaigaiKbn;
            HokenId = hokenId;
            RousaiSyobyoDate = rousaiSyobyoDate;
        }

        public long PtId { get; private set; }

        public long PtNum { get; private set; }

        public int SinYm { get; private set; }

        public int IsTester { get; private set; }

        public int HokenKbn { get; private set; }

        public int IsPaperRece { get; private set; }

        public int RousaiSaigaiKbn { get; private set; }

        public int HokenId { get; private set; }

        public int RousaiSyobyoDate { get; private set; }
    }
}
