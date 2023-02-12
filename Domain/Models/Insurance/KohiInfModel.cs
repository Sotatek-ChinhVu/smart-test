using Domain.Models.HokenMst;
using Domain.Models.InsuranceMst;
using Helper.Common;
using Helper.Extension;
using System.Text.Json.Serialization;

namespace Domain.Models.Insurance
{
    public class KohiInfModel
    {
        public KohiInfModel(string futansyaNo, string jyukyusyaNo, int hokenId, int startDate, int endDate, int confirmDate, int rate, int gendoGaku, int sikakuDate, int kofuDate, string tokusyuNo, int hokenSbtKbn, string houbetu, int hokenNo, int hokenEdaNo, int prefNo, HokenMstModel hokenMstModel, int sinDate, List<ConfirmDateModel> confirmDateList, bool isHaveKohiMst, int isDeleted, bool isAddNew, long seqNo)
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
            SeqNo = seqNo;
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
        public KohiInfModel(List<ConfirmDateModel> confirmDateList, string futansyaNo, string jyukyusyaNo, int hokenId, int startDate, int endDate, int confirmDate, int rate, int gendoGaku, int sikakuDate, int kofuDate, string tokusyuNo, int hokenSbtKbn, string houbetu, HokenMstModel hokenMstModel, int hokenNo, int hokenEdaNo, int prefNo, int sinDate, bool isHaveKohiMst, int isDeleted, long seqNo, bool isAddNew)
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
            SeqNo = seqNo;
            IsAddNew = isAddNew;
        }

        public KohiInfModel(int hokenId, int prefNo, int hokenNo, int hokenEdaNo, string futansyaNo, int startDate, int endDate, int sinDate, HokenMstModel hokenMstModel, List<PtHokenCheckModel> ptHokenCheckModels)
        {
            HokenId = hokenId;
            PrefNo = prefNo;
            HokenNo = hokenNo;
            HokenEdaNo = hokenEdaNo;
            FutansyaNo = futansyaNo;
            StartDate = startDate;
            EndDate = endDate;
            SinDate = sinDate;
            HokenMstModel = hokenMstModel;
            PtHokenCheckModels = ptHokenCheckModels;
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
        public long SeqNo { get; private set; }

        public List<PtHokenCheckModel> PtHokenCheckModels { get; private set; }

        public bool IsEmptyModel => HokenId == 0;

        public bool IsExpirated
        {
            get
            {
                return !(StartDate <= SinDate && EndDate >= SinDate);
            }
        }

        public bool IsAddNew { get; private set; }

        public int IsLimitList { get => HokenMstModel.IsLimitList; }

        public int CalcSpKbn { get => HokenMstModel.CalcSpKbn; }

        public string PrefNoMst { get => HokenMstModel.PrefactureName; }

        public bool HasDateConfirmed
        {
            get
            {
                if (PtHokenCheckModels == null) return false;
                if (PtHokenCheckModels.Count == 0)
                {
                    return false;
                }
                List<PtHokenCheckModel> isValidHokenChecks = PtHokenCheckModels
                    .Where(x => x.IsDeleted == 0)
                    .OrderByDescending(x => x.CheckDate)
                    .ToList();
                int SinYM = CIUtil.Copy(SinDate.AsString(), 1, 6).AsInteger();
                foreach (PtHokenCheckModel ptHokenCheck in isValidHokenChecks)
                {
                    int currentConfirmYM = CIUtil.Copy(CIUtil.DateTimeToInt(ptHokenCheck.CheckDate).AsString(), 1, 6).AsInteger();
                    if (currentConfirmYM == SinYM)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public int LastDateConfirmed
        {
            get
            {
                if (PtHokenCheckModels == null || PtHokenCheckModels.Count <= 0) return 0;

                return CIUtil
                    .Copy(
                        CIUtil.DateTimeToInt(PtHokenCheckModels.OrderByDescending(item => item.CheckDate).First().CheckDate)
                            .AsString(), 1, 8).AsInteger();
            }
        }
    }
}
