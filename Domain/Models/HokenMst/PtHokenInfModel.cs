using Domain.Constant;
using Helper.Common;
using Helper.Extension;

namespace Domain.Models.HokenMst
{
    public class PtHokenInfModel
    {
        public PtHokenInfModel(int hpId, long ptId, int hokenId, int hokenKbn, string houbetu, int startDate, int endDate, int sinday, HokenMasterModel hokenMasterModel, List<PtHokenCheckModel> ptHokenCheckModels)
        {
            HpId = hpId;
            PtId = ptId;
            HokenId = hokenId;
            HokenKbn = hokenKbn;
            Houbetu = houbetu;
            StartDate = startDate;
            EndDate = endDate;
            Sinday = sinday;
            HokenMasterModel = hokenMasterModel;
            PtHokenCheckModels = ptHokenCheckModels;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int HokenId { get; private set; }

        public int HokenKbn { get; private set; }

        public string Houbetu { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public int Sinday { get; private set; }

        public HokenMasterModel HokenMasterModel { get; private set; }

        public List<PtHokenCheckModel> PtHokenCheckModels { get; private set; }

        public bool IsExpirated
        {
            get
            {
                return !(StartDate <= Sinday && EndDate >= Sinday);
            }
        }

        public bool IsHoken => IsShaho || IsKokuho;

        public bool IsShaho
        {
            // not nashi
            get => HokenKbn == 1 && Houbetu != HokenConstant.HOUBETU_NASHI;
        }

        public bool IsKokuho
        {
            get => HokenKbn == 2;
        }

        public bool IsReceKisaiOrNoHoken => IsReceKisai || IsNoHoken;

        public bool IsReceKisai
        {
            get
            {
                if (HokenMasterModel != null)
                {
                    return HokenMasterModel.ReceKisai == 3;
                }
                return false;
            }
        }

        public bool IsNoHoken
        {
            get
            {
                if (HokenMasterModel != null)
                {
                    return HokenMasterModel.HokenSbtKbn == 0;
                }
                return HokenKbn == 1 && Houbetu == HokenConstant.HOUBETU_NASHI;
            }
        }

        public bool IsRousai => HokenKbn == 11 || HokenKbn == 12 || HokenKbn == 13;

        public bool IsJibai => HokenKbn == 14;

        public bool IsJihi
        {
            get
            {
                if (HokenMasterModel != null)
                {
                    return HokenMasterModel.HokenSbtKbn == 8;
                }
                return HokenKbn == 0 && (Houbetu == HokenConstant.HOUBETU_JIHI_108 || Houbetu == HokenConstant.HOUBETU_JIHI_109);
            }
        }

        public bool HasDateConfirmed
        {
            get
            {
                // Jihi
                if (IsJihi)
                {
                    return true;
                }

                // nashi
                if (Houbetu == HokenConstant.HOUBETU_NASHI)
                {
                    return true;
                }
                if (PtHokenCheckModels == null) return false;
                if (PtHokenCheckModels.Count == 0)
                {
                    return false;
                }
                List<PtHokenCheckModel> isValidHokenChecks = PtHokenCheckModels
                    .Where(x => x.IsDeleted == 0)
                    .OrderByDescending(x => x.CheckDate)
                    .ToList();
                int SinYM = CIUtil.Copy(Sinday.AsString(), 1, 6).AsInteger();
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
