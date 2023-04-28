using CommonChecker.Types;
using CommonCheckers.OrderRealtimeChecker.Enums;
using Domain.Models.Diseases;
using Domain.Models.Family;
using SpecialNoteFull = Domain.Models.SpecialNote.SpecialNoteModel;

namespace CommonCheckers.OrderRealtimeChecker.Models
{
    public class UnitCheckerForOrderListResult<TOdrInf, TOdrDetail>
        where TOdrInf : class, IOdrInfoModel<TOdrDetail>
        where TOdrDetail : class, IOdrInfoDetailModel
    {
        public int Sinday { get; private set; }

        public long PtId { get; private set; }

        public ActionResultType ActionType { get; set; } = ActionResultType.OK;

        public bool IsError => ErrorOrderList != null && ErrorOrderList.Count > 0;

        public List<TOdrInf> ErrorOrderList { get; set; }

        public object ErrorInfo { get; set; }

        public RealtimeCheckerType CheckerType { get; private set; }

        public List<TOdrInf> CheckingOrderList { get; private set; }

        public SpecialNoteFull SpecialNoteModel { get; private set; }
        public List<PtDiseaseModel> PtDiseaseModels { get; private set; }
        public List<FamilyModel> FamilyModels { get; private set; }
        public bool IsDataOfDb { get; private set; }

        public UnitCheckerForOrderListResult(RealtimeCheckerType checkerType, List<TOdrInf> checkingOrderList, int sinday, long ptId, SpecialNoteFull specialNoteModel, List<PtDiseaseModel> ptDiseaseModels, List<FamilyModel> familyModels, bool isDataOfDb)
        {
            CheckerType = checkerType;
            CheckingOrderList = checkingOrderList;
            Sinday = sinday;
            PtId = ptId;
            ErrorOrderList = new List<TOdrInf>();
            ErrorInfo = string.Empty;
            SpecialNoteModel = specialNoteModel;
            PtDiseaseModels = ptDiseaseModels;
            FamilyModels = familyModels;
            IsDataOfDb = isDataOfDb;
        }
    }
}
