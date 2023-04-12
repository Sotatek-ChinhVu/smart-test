using Helper.Constants;

namespace UseCase.SystemConf.SaveSystemSetting
{
    public class SystemGenerationConfItem
    {
        public SystemGenerationConfItem(long id, int hpId, int grpCd, int grpEdaNo, int startDate, int endDate, int val, string param, string biko, ModelStatus systemGenerationConfStatus)
        {
            Id = id;
            HpId = hpId;
            GrpCd = grpCd;
            GrpEdaNo = grpEdaNo;
            StartDate = startDate;
            EndDate = endDate;
            Val = val;
            Param = param;
            Biko = biko;
            SystemGenerationConfStatus = systemGenerationConfStatus;
        }

        public long Id { get; private set; }
        public int HpId { get; private set; }
        public int GrpCd { get; private set; }
        public int GrpEdaNo { get; private set; }
        public int StartDate { get; private set; }
        public int EndDate { get; private set; }
        public int Val { get; private set; }
        public string Param { get; private set; }
        public string Biko { get; private set; }
        public ModelStatus SystemGenerationConfStatus { get; private set; }

        public bool CheckDefaultValue()
        {
            return StartDate == 0 && (EndDate == 99999999 || EndDate == 0) && Val < 0 && string.IsNullOrEmpty(Param);
        }
    }
}
