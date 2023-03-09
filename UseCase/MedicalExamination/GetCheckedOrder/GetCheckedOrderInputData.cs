using UseCase.Core.Sync.Core;
using UseCase.MedicalExamination.UpsertTodayOrd;

namespace UseCase.MedicalExamination.GetCheckedOrder
{
    public class GetCheckedOrderInputData : IInputData<GetCheckedOrderOutputData>
    {
        public GetCheckedOrderInputData(int hpId, int userId, int sinDate, int hokenId, int hokenPid, long ptId, int iBirthDay, long raiinNo, int syosaisinKbn, long oyaRaiinNo, int tantoId, int primaryDoctor, List<OdrInfItemInputData> odrInfItems, List<DiseaseItem> diseaseItems)
        {
            HpId = hpId;
            UserId = userId;
            SinDate = sinDate;
            HokenId = hokenId;
            HokenPid = hokenPid;
            PtId = ptId;
            IBirthDay = iBirthDay;
            RaiinNo = raiinNo;
            SyosaisinKbn = syosaisinKbn;
            OyaRaiinNo = oyaRaiinNo;
            TantoId = tantoId;
            PrimaryDoctor = primaryDoctor;
            OdrInfItems = odrInfItems;
            DiseaseItems = diseaseItems;
        }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public int SinDate { get; private set; }

        public int HokenId { get; private set; }

        public int HokenPid { get; private set; }

        public long PtId { get; private set; }

        public int IBirthDay { get; private set; }

        public long RaiinNo { get; private set; }

        public int SyosaisinKbn { get; private set; }

        public long OyaRaiinNo { get; private set; }

        public int TantoId { get; private set; }

        public int PrimaryDoctor { get; private set; }

        public List<OdrInfItemInputData> OdrInfItems { get; private set; }

        public List<DiseaseItem> DiseaseItems { get; private set; }
    }
}
