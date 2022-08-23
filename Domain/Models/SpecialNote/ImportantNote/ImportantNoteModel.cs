namespace Domain.Models.SpecialNote.ImportantNote
{
    public class ImportantNoteModel
    {
        public ImportantNoteModel(List<PtAlrgyFoodModel> alrgyFoodItems, List<PtAlrgyElseModel> alrgyElseItems, List<PtAlrgyDrugModel> alrgyDrugItems, List<PtKioRekiModel> kioRekiItems, List<PtInfectionModel> infectionsItems, List<PtOtherDrugModel> otherDrugItems, List<PtOtcDrugModel> otcDrugItems, List<PtSuppleModel> suppleItems)
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

        public ImportantNoteModel()
        {
            AlrgyFoodItems = new List<PtAlrgyFoodModel>();
            AlrgyElseItems = new List<PtAlrgyElseModel>();
            AlrgyDrugItems = new List<PtAlrgyDrugModel>();
            KioRekiItems = new List<PtKioRekiModel>();
            InfectionsItems = new List<PtInfectionModel>();
            OtherDrugItems = new List<PtOtherDrugModel>();
            OtcDrugItems = new List<PtOtcDrugModel>();
            SuppleItems = new List<PtSuppleModel>();
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
