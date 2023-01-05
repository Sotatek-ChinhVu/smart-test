using UseCase.Core.Sync.Core;
using UseCase.Diseases.Upsert;
using UseCase.MedicalExamination.UpsertTodayOrd;

namespace UseCase.MedicalExamination.GetCheckedOrder
{
    public class GetCheckedOrderInputData : IInputData<GetCheckedOrderOutputData>
    {
        public GetCheckedOrderInputData(int hpId, int userId, int sinDate, int hokenId, long ptId, int iBirthDay, long raiinNo, int syosaisinKbn, long oyaRaiinNo, int primaryDoctor, int tantoId, List<OdrInfItemInputData> odrInfItemInputDatas, List<UpsertPtDiseaseListInputItem> ptDiseaseListInputItems)
        {
            HpId = hpId;
            UserId = userId;
            SinDate = sinDate;
            HokenId = hokenId;
            PtId = ptId;
            IBirthDay = iBirthDay;
            RaiinNo = raiinNo;
            SyosaisinKbn = syosaisinKbn;
            OyaRaiinNo = oyaRaiinNo;
            PrimaryDoctor = primaryDoctor;
            TantoId = tantoId;
            OdrInfItemInputDatas = odrInfItemInputDatas;
            PtDiseaseListInputItems = ptDiseaseListInputItems;
        }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public int SinDate { get; private set; }

        public int HokenId { get; private set; }

        public long PtId { get; private set; }

        public int IBirthDay { get; private set; }

        public long RaiinNo { get; private set; }

        public int SyosaisinKbn { get; private set; }

        public long OyaRaiinNo { get; private set; }

        public int PrimaryDoctor { get; private set; }

        public int TantoId { get; private set; }

        public List<OdrInfItemInputData> OdrInfItemInputDatas { get; private set; }

        public List<UpsertPtDiseaseListInputItem> PtDiseaseListInputItems { get; private set; }
    }
}
