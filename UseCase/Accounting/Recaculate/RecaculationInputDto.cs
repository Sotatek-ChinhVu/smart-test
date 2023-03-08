namespace UseCase.Accounting.Recaculate
{
    public class RecaculationInputDto
    {
        public RecaculationInputDto(int hpId, long ptId, int sinDate, int seikyuUp, string prefix)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            SeikyuUp = seikyuUp;
            Prefix = prefix;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public int SeikyuUp { get; private set; }
        public string Prefix { get; private set; }
    }
}
