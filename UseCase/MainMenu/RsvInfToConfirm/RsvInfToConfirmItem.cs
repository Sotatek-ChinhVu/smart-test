namespace UseCase.MainMenu.RsvInfToConfirm
{
    public class RsvInfToConfirmItem
    {
        public RsvInfToConfirmItem(string ptName, int hpId, int sinDate, long raiinNo, long ptId, long ptNum, int birthday, int tantoId, int kaId, string ptNumDisplay, int age, List<HokenInfItem> ptHokenInfs)
        {
            PtName = ptName;
            HpId = hpId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            PtId = ptId;
            PtNum = ptNum;
            Birthday = birthday;
            TantoId = tantoId;
            KaId = kaId;
            PtNumDisplay = ptNumDisplay;
            Age = age;
            PtHokenInfs = ptHokenInfs;
        }

        public string PtName { get; private set; }

        public int HpId { get; private set; }

        public int SinDate { get; private set; }

        public long RaiinNo { get; private set; }

        public long PtId { get; private set; }

        public long PtNum { get; private set; }

        public int Birthday { get; private set; }

        public int TantoId { get; private set; }

        public int KaId { get; private set; }

        public string PtNumDisplay { get; private set; }

        public int Age { get; private set; }

        public List<HokenInfItem> PtHokenInfs { get; private set; }

    }

    public class HokenInfItem
    {
        public HokenInfItem(long ptId, int hokenId, long seqNo, int hokenNo, int hokenEdaNo, int hokenKbn, string hokensyaNo, string kigo, string bango, string edaNo, int honkeKbn, int kogakuKbn)
        {
            PtId = ptId;
            HokenId = hokenId;
            SeqNo = seqNo;
            HokenNo = hokenNo;
            HokenEdaNo = hokenEdaNo;
            HokenKbn = hokenKbn;
            HokensyaNo = hokensyaNo;
            Kigo = kigo;
            Bango = bango;
            EdaNo = edaNo;
            HonkeKbn = honkeKbn;
            KogakuKbn = kogakuKbn;
        }

        public long PtId { get; private set; }

        public int HokenId { get; private set; }

        public long SeqNo { get; private set; }

        public int HokenNo { get; private set; }

        public int HokenEdaNo { get; private set; }

        public int HokenKbn { get; private set; }

        public string HokensyaNo { get; private set; }

        public string Kigo { get; private set; }

        public string Bango { get; private set; }

        public string EdaNo { get; private set; }

        public int HonkeKbn { get; private set; }

        public int KogakuKbn { get; private set; }

        private string HokenKbnName
        {
            get
            {
                if (HokenKbn > 2 || HokenKbn < 0)
                {
                    return string.Empty;
                }
                else if (HokenKbn == 0)
                {
                    return "自費";
                }
                else if (HokenKbn == 1)
                {
                    return "社保";
                }
                else
                {
                    return "国保";
                }
            }
        }

        public string PtHokenName
        {
            get
            {
                if (PtId == 0)
                {
                    return string.Empty;
                }
                return HokenId + "  " + HokenKbnName + " " + HokenNo + " " + Kigo + " ・ " + Bango;
            }
        }
    }
}
