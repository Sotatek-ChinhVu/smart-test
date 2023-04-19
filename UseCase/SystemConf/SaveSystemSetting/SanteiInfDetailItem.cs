using Helper.Constants;

namespace UseCase.SystemConf.SaveSystemSetting
{
    public class SanteiInfDetailItem
    {
        public SanteiInfDetailItem(long id, long ptId, string itemCd, int endDate, int kisanSbt, int kisanDate, string byomei, string hosokuComment, string comment, bool isDeleted, ModelStatus autoSanteiMstModelStatus)
        {
            Id = id;
            PtId = ptId;
            ItemCd = itemCd;
            EndDate = endDate;
            KisanSbt = kisanSbt;
            KisanDate = kisanDate;
            Byomei = byomei;
            HosokuComment = hosokuComment;
            Comment = comment;
            IsDeleted = isDeleted;
            AutoSanteiMstModelStatus = autoSanteiMstModelStatus;
        }

        public long Id { get; private set; }

        public long PtId { get; private set; }

        public string ItemCd { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public int KisanSbt { get; private set; }

        public int KisanDate { get; private set; }

        public string Byomei { get; private set; }

        public string HosokuComment { get; private set; }

        public string Comment { get; private set; }

        public bool IsDeleted { get; private set; }

        public ModelStatus AutoSanteiMstModelStatus { get; private set; }

        public bool CheckDefaultValue()
        {
            return StartDate == 0 && EndDate == 99999999 && Id == 0;
        }
    }
}
