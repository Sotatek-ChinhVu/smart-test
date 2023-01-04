using EmrCloudApi.Responses.Document.Dto;

namespace EmrCloudApi.Requests.Insurance
{
    public class HokenKohiDto
    {
        public HokenKohiDto(int hpId, long ptId, int hokenId, long seqNo, int prefNo, int hokenNo, int hokenEdaNo, string futansyaNo, string jyukyusyaNo, int hokenSbtKbn, string houbetu, string tokusyuNo, int sikakuDate, int kofuDate, int startDate, int endDate, int rate, int gendoGaku, int isDeleted, List<ConfirmDateDto> confirmDates, bool isAddNew)
        {
            HpId = hpId;
            PtId = ptId;
            HokenId = hokenId;
            SeqNo = seqNo;
            PrefNo = prefNo;
            HokenNo = hokenNo;
            HokenEdaNo = hokenEdaNo;
            FutansyaNo = futansyaNo;
            JyukyusyaNo = jyukyusyaNo;
            HokenSbtKbn = hokenSbtKbn;
            Houbetu = houbetu;
            TokusyuNo = tokusyuNo;
            SikakuDate = sikakuDate;
            KofuDate = kofuDate;
            StartDate = startDate;
            EndDate = endDate;
            Rate = rate;
            GendoGaku = gendoGaku;
            IsDeleted = isDeleted;
            ConfirmDates = confirmDates;
            IsAddNew = isAddNew;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int HokenId { get; private set; }

        public long SeqNo { get; private set; }

        public int PrefNo { get; private set; }

        public int HokenNo { get; private set; }

        public int HokenEdaNo { get; private set; }

        public string FutansyaNo { get; private set; } = string.Empty;

        public string JyukyusyaNo { get; private set; } = string.Empty;

        public int HokenSbtKbn { get; private set; }

        public string Houbetu { get; private set; } = string.Empty;

        public string TokusyuNo { get; private set; } = string.Empty;

        public int SikakuDate { get; private set; }

        public int KofuDate { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public int Rate { get; private set; }
        public int GendoGaku { get; private set; }

        public int IsDeleted { get; private set; }

        public List<ConfirmDateDto> ConfirmDates { get; private set; } = new List<ConfirmDateDto>();

        public bool IsAddNew { get; private set; }
    }
}