using System.Text.Json.Serialization;

namespace Domain.Models.Insurance
{
    public class KohiInfModel
    {
        public KohiInfModel(string futansyaNo, string jyukyusyaNo, int hokenId, int startDate, int endDate, int confirmDate, int rate, int gendoGaku, int sikakuDate, int kofuDate, string tokusyuNo, int hokenSbtKbn, string houbetu, int hokenNo, int hokenEdaNo, int prefNo, HokenMstModel hokenMstModel, int sinDate, List<ConfirmDateModel> confirmDateList, bool isHaveKohiMst, int isDeleted, bool isAddNew)
        {
            FutansyaNo = futansyaNo;
            JyukyusyaNo = jyukyusyaNo;
            HokenId = hokenId;
            StartDate = startDate;
            EndDate = endDate;
            ConfirmDate = confirmDate;
            Rate = rate;
            GendoGaku = gendoGaku;
            SikakuDate = sikakuDate;
            KofuDate = kofuDate;
            TokusyuNo = tokusyuNo;
            HokenSbtKbn = hokenSbtKbn;
            Houbetu = houbetu;
            HokenMstModel = hokenMstModel;
            HokenNo = hokenNo;
            HokenEdaNo = hokenEdaNo;
            PrefNo = prefNo;
            SinDate = sinDate;
            ConfirmDateList = confirmDateList;
            IsHaveKohiMst = isHaveKohiMst;
            IsDeleted = isDeleted;
            IsAddNew = isAddNew;
        }

        public KohiInfModel(int hokenId)
        {
            FutansyaNo = string.Empty;
            JyukyusyaNo = string.Empty;
            HokenId = hokenId;
            TokusyuNo = string.Empty;
            Houbetu = string.Empty;
            HokenMstModel = new HokenMstModel();
            ConfirmDateList = new List<ConfirmDateModel>();
            IsHaveKohiMst = false;
        }

        [JsonConstructor]
        public KohiInfModel(List<ConfirmDateModel> confirmDateList, string futansyaNo, string jyukyusyaNo, int hokenId, int startDate, int endDate, int confirmDate, int rate, int gendoGaku, int sikakuDate, int kofuDate, string tokusyuNo, int hokenSbtKbn, string houbetu, HokenMstModel hokenMstModel, int hokenNo, int hokenEdaNo, int prefNo, int sinDate, bool isHaveKohiMst, int isDeleted)
        {
            ConfirmDateList = confirmDateList;
            FutansyaNo = futansyaNo;
            JyukyusyaNo = jyukyusyaNo;
            HokenId = hokenId;
            StartDate = startDate;
            EndDate = endDate;
            ConfirmDate = confirmDate;
            Rate = rate;
            GendoGaku = gendoGaku;
            SikakuDate = sikakuDate;
            KofuDate = kofuDate;
            TokusyuNo = tokusyuNo;
            HokenSbtKbn = hokenSbtKbn;
            Houbetu = houbetu;
            HokenMstModel = hokenMstModel;
            HokenNo = hokenNo;
            HokenEdaNo = hokenEdaNo;
            PrefNo = prefNo;
            SinDate = sinDate;
            IsHaveKohiMst = isHaveKohiMst;
            IsDeleted = isDeleted;
        }

        public List<ConfirmDateModel> ConfirmDateList { get; private set; }

        public string FutansyaNo { get; private set; }

        public string JyukyusyaNo { get; private set; }

        public int HokenId { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public int ConfirmDate { get; private set; }

        public int Rate { get; private set; }

        public int GendoGaku { get; private set; }

        public int SikakuDate { get; private set; }

        public int KofuDate { get; private set; }

        public string TokusyuNo { get; private set; }

        public int HokenSbtKbn { get; private set; }

        public string Houbetu { get; private set; }

        public HokenMstModel HokenMstModel { get; private set; }

        public int HokenNo { get; private set; }

        public int HokenEdaNo { get; private set; }

        public int PrefNo { get; private set; }

        public int SinDate { get; private set; }

        public bool IsHaveKohiMst { get; private set; }

        public int IsDeleted { get; private set; }

        public bool IsEmptyModel => HokenId == 0;

        public bool IsExpirated
        {
            get
            {
                return !(StartDate <= SinDate && EndDate >= SinDate);
            }
        }

        public bool IsAddNew { get; private set; }
    }
}
