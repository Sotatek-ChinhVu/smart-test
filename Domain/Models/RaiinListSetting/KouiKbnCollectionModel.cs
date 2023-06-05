namespace Domain.Models.RaiinListSetting
{
    public class KouiKbnCollectionModel
    {
        public KouiKbnCollectionModel(RaiinListKouiModel iKanModel, RaiinListKouiModel zaitakuModel, RaiinListKouiModel naifukuModel, RaiinListKouiModel tonpukuModel, RaiinListKouiModel gaiyoModel, RaiinListKouiModel hikaKinchuModel, RaiinListKouiModel jochuModel, RaiinListKouiModel tentekiModel, RaiinListKouiModel tachuModel, RaiinListKouiModel jikochuModel, RaiinListKouiModel shochiModel, RaiinListKouiModel shujutsuModel, RaiinListKouiModel masuiModel, RaiinListKouiModel kentaiModel, RaiinListKouiModel seitaiModel, RaiinListKouiModel sonohokaModel, RaiinListKouiModel gazoModel, RaiinListKouiModel rihaModel, RaiinListKouiModel seishinModel, RaiinListKouiModel hoshaModel, RaiinListKouiModel byoriModel, RaiinListKouiModel jihiModel)
        {
            IKanModel = iKanModel;
            ZaitakuModel = zaitakuModel;
            NaifukuModel = naifukuModel;
            TonpukuModel = tonpukuModel;
            GaiyoModel = gaiyoModel;
            HikaKinchuModel = hikaKinchuModel;
            JochuModel = jochuModel;
            TentekiModel = tentekiModel;
            TachuModel = tachuModel;
            JikochuModel = jikochuModel;
            ShochiModel = shochiModel;
            ShujutsuModel = shujutsuModel;
            MasuiModel = masuiModel;
            KentaiModel = kentaiModel;
            SeitaiModel = seitaiModel;
            SonohokaModel = sonohokaModel;
            GazoModel = gazoModel;
            RihaModel = rihaModel;
            SeishinModel = seishinModel;
            HoshaModel = hoshaModel;
            ByoriModel = byoriModel;
            JihiModel = jihiModel;
        }

        public RaiinListKouiModel IKanModel { get; private set; }

        public RaiinListKouiModel ZaitakuModel { get; private set; }

        public RaiinListKouiModel NaifukuModel { get; private set; }

        public RaiinListKouiModel TonpukuModel { get; private set; }

        public RaiinListKouiModel GaiyoModel { get; private set; }

        public RaiinListKouiModel HikaKinchuModel { get; private set; }

        public RaiinListKouiModel JochuModel { get; private set; }

        public RaiinListKouiModel TentekiModel { get; private set; }

        public RaiinListKouiModel TachuModel { get; private set; }

        public RaiinListKouiModel JikochuModel { get; private set; }

        public RaiinListKouiModel ShochiModel { get; private set; }

        public RaiinListKouiModel ShujutsuModel { get; private set; }

        public RaiinListKouiModel MasuiModel { get; private set; }

        public RaiinListKouiModel KentaiModel { get; private set; }

        public RaiinListKouiModel SeitaiModel { get; private set; }

        public RaiinListKouiModel SonohokaModel { get; private set; }

        public RaiinListKouiModel GazoModel { get; private set; }

        public RaiinListKouiModel RihaModel { get; private set; }

        public RaiinListKouiModel SeishinModel { get; private set; }

        public RaiinListKouiModel HoshaModel { get; private set; }

        public RaiinListKouiModel ByoriModel { get; private set; }

        public RaiinListKouiModel JihiModel { get; private set; }

        public bool Dosage
        {
            get => (NaifukuModel != null && NaifukuModel.IsChecked) && (TonpukuModel != null && TonpukuModel.IsChecked) && (GaiyoModel != null && GaiyoModel.IsChecked);
        }

        public bool Injection
        {
            get => (HikaKinchuModel != null && HikaKinchuModel.IsChecked) && (JochuModel != null && JochuModel.IsChecked) && (TentekiModel != null && TentekiModel.IsChecked) && (TachuModel != null && TachuModel.IsChecked);
        }

        public bool Kensa
        {
            get => (KentaiModel != null && KentaiModel.IsChecked) && (SeitaiModel != null && SeitaiModel.IsChecked) && (SonohokaModel != null && SonohokaModel.IsChecked);
        }
    }
}
