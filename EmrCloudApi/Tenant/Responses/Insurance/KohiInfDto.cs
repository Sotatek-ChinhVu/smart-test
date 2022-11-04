using Domain.Models.Insurance;

namespace EmrCloudApi.Tenant.Responses.Insurance
{
    public class KohiInfDto
    {
        public KohiInfDto(KohiInfModel kohiInfModel)
        {
            FutansyaNo = kohiInfModel.FutansyaNo;
            JyukyusyaNo = kohiInfModel.JyukyusyaNo;
            HokenId = kohiInfModel.HokenId;
            StartDate = kohiInfModel.StartDate;
            EndDate = kohiInfModel.EndDate;
            ConfirmDate = kohiInfModel.ConfirmDate;
            Rate = kohiInfModel.Rate;
            GendoGaku = kohiInfModel.GendoGaku;
            SikakuDate = kohiInfModel.SikakuDate;
            KofuDate = kohiInfModel.KofuDate;
            TokusyuNo = kohiInfModel.TokusyuNo;
            HokenSbtKbn = kohiInfModel.HokenSbtKbn;
            Houbetu = kohiInfModel.Houbetu;
            HokenMstModel = new HokenMstDto(kohiInfModel.HokenMstModel);
            HokenNo = kohiInfModel.HokenNo;
            HokenEdaNo = kohiInfModel.HokenEdaNo;
            PrefNo = kohiInfModel.PrefNo;
            SinDate = kohiInfModel.SinDate;
            ConfirmDateList = kohiInfModel.ConfirmDateList.Select(c => new ConfirmDateDto(c)).ToList();
            IsHaveKohiMst = kohiInfModel.IsHaveKohiMst;
            IsDeleted = kohiInfModel.IsDeleted;
            IsAddNew = kohiInfModel.IsAddNew;
            SeqNo = kohiInfModel.SeqNo;
        }

        public List<ConfirmDateDto> ConfirmDateList { get; private set; }

        public HokenMstDto HokenMstModel { get; private set; }

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

        public int HokenNo { get; private set; }

        public int HokenEdaNo { get; private set; }

        public int PrefNo { get; private set; }

        public int SinDate { get; private set; }

        public bool IsHaveKohiMst { get; private set; }

        public int IsDeleted { get; private set; }

        public bool IsAddNew { get; private set; }

        public long SeqNo { get; private set; }

        public bool IsEmptyModel => HokenId == 0;

        public bool IsExpirated
        {
            get
            {
                return !(StartDate <= SinDate && EndDate >= SinDate);
            }
        }
    }
}
