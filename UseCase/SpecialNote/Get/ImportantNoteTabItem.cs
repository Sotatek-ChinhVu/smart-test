using Domain.Models.PtAlrgyDrug;
using Domain.Models.PtAlrgyElse;
using Domain.Models.PtAlrgyFood;
using Domain.Models.PtInfection;
using Domain.Models.PtKioReki;
using Domain.Models.PtOtcDrug;
using Domain.Models.PtOtherDrug;
using Domain.Models.PtSupple;

namespace UseCase.SpecialNote.Get
{
    public class ImportantNoteTabItem
    {
        public ImportantNoteTabItem(List<PtAlrgyFoodModel> alrgyFoodItems, List<PtAlrgyElseModel> alrgyElseItems, List<PtAlrgyDrugModel> alrgyDrugItems, List<PtKioRekiModel> kioRekiItems, List<PtInfectionModel> infectionsItems, List<PtOtherDrugModel> otherDrugItems, List<PtOtcDrugModel> otcDrugItems, List<PtSuppleModel> suppleItems)
        {
            AlrgyFoodItems = alrgyFoodItems;
            AlrgyElseItems = alrgyElseItems;
            AlrgyDrugItems = alrgyDrugItems;
            KioRekiItems = kioRekiItems;
            InfectionsItems = infectionsItems;
            OtherDrugItems = otherDrugItems;
            OtcDrugItems = otcDrugItems;
            SuppleItems = suppleItems;
        }

        //Aglrgy
        public List<PtAlrgyFoodModel> AlrgyFoodItems { get; private set; }
        public List<PtAlrgyElseModel> AlrgyElseItems { get; private set; }
        public List<PtAlrgyDrugModel> AlrgyDrugItems { get; private set; }

        //Pathological
        public List<PtKioRekiModel> KioRekiItems { get; private set; }
        public List<PtInfectionModel> InfectionsItems { get; private set; }

        //Interaction PtOtherDrugItem
        public List<PtOtherDrugModel> OtherDrugItems { get; private set; }
        public List<PtOtcDrugModel> OtcDrugItems { get; private set; }
        public List<PtSuppleModel> SuppleItems { get; private set; }
    }
}
