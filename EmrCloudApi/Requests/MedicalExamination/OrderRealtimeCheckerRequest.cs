using CommonChecker.Models.OrdInf;
using CommonCheckers.OrderRealtimeChecker.Models;
using Domain.Models.Diseases;
using UseCase.Family;
using UseCase.MedicalExamination.SaveMedical;

namespace EmrCloudApi.Requests.MedicalExamination
{
    public class OrderRealtimeCheckerRequest
    {
        public OrderRealtimeCheckerRequest(long ptId, int hpId, int sinDay, List<OrdInfoModel> currentListOdr, List<OrdInfoModel> listCheckingOrder, SpecialNoteItem specialNoteItem, List<PtDiseaseModel> ptDiseaseModels, List<FamilyItem> familyItems, bool isDataOfDb, RealTimeCheckerCondition realTimeCheckerCondition)
        {
            PtId = ptId;
            HpId = hpId;
            SinDay = sinDay;
            CurrentListOdr = currentListOdr;
            ListCheckingOrder = listCheckingOrder;
            SpecialNoteItem = specialNoteItem;
            PtDiseaseModels = ptDiseaseModels;
            FamilyItems = familyItems;
            IsDataOfDb = isDataOfDb;
            RealTimeCheckerCondition = realTimeCheckerCondition;
        }

        public long PtId { get; set; }

        public int HpId { get; set; }

        public int SinDay { get; set; }

        public List<OrdInfoModel> CurrentListOdr { get; set; }
        public List<OrdInfoModel> ListCheckingOrder { get; set; }
        public SpecialNoteItem SpecialNoteItem { get; set; }
        public List<PtDiseaseModel> PtDiseaseModels { get; set; }
        public List<FamilyItem> FamilyItems { get; private set; }
        public RealTimeCheckerCondition RealTimeCheckerCondition { get; private set; }
        public bool IsDataOfDb { get; set; }
    }
}
