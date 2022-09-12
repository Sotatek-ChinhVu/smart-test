namespace Domain.Models.SystemGenerationConf
{
    public class SystemGenerationConfModel
    {
        public SystemGenerationConfModel(long id, int hpId, int grpCd, int grpEdaNo, int startDate, int endDate, int val, string param, string biko)
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
    }
}
