namespace EmrCloudApi.Requests.RaiinListSetting
{
    public class RaiinListMstDto
    {
        public RaiinListMstDto(int grpId, string grpName, int sortNo, int isDeleted, List<RaiinListDetailDto> raiinListDetailsList)
        {
            GrpId = grpId;
            GrpName = grpName;
            SortNo = sortNo;
            IsDeleted = isDeleted;
            RaiinListDetailsList = raiinListDetailsList;
        }

        public int GrpId { get; private set; }

        public string GrpName { get; private set; }

        public int SortNo { get; private set; }

        public int IsDeleted { get; private set; }

        public List<RaiinListDetailDto> RaiinListDetailsList { get; private set; }
    }

    public class RaiinListDetailDto
    {
        public RaiinListDetailDto(int grpId, int kbnCd, int sortNo, string kbnName, string colorCd, int isDeleted, bool isOnlySwapSortNo, List<RaiinListDocDto> raiinListDoc, List<RaiinListItemDto> raiinListItem, List<RaiinListFileDto> raiinListFile, KouiKbnCollectionDto kouCollection)
        {
            GrpId = grpId;
            KbnCd = kbnCd;
            SortNo = sortNo;
            KbnName = kbnName;
            ColorCd = colorCd;
            IsDeleted = isDeleted;
            IsOnlySwapSortNo = isOnlySwapSortNo;
            RaiinListDoc = raiinListDoc;
            RaiinListItem = raiinListItem;
            RaiinListFile = raiinListFile;
            KouCollection = kouCollection;
        }

        public int GrpId { get; private set; }

        public int KbnCd { get; private set; }

        public int SortNo { get; private set; }

        public string KbnName { get; private set; }

        public string ColorCd { get; private set; }

        public int IsDeleted { get; private set; }

        public bool IsOnlySwapSortNo { get; private set; }

        public List<RaiinListDocDto> RaiinListDoc { get; private set; }

        public List<RaiinListItemDto> RaiinListItem { get; private set; }

        public List<RaiinListFileDto> RaiinListFile { get; private set; }

        public KouiKbnCollectionDto KouCollection { get; private set; }
    }


    public class RaiinListDocDto
    {
        public RaiinListDocDto(int hpId, int grpId, int kbnCd, long seqNo, int categoryCd, string categoryName, int isDeleted)
        {
            HpId = hpId;
            GrpId = grpId;
            KbnCd = kbnCd;
            SeqNo = seqNo;
            CategoryCd = categoryCd;
            CategoryName = categoryName;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }

        public int GrpId { get; private set; }

        public int KbnCd { get; private set; }

        public long SeqNo { get; private set; }

        public int CategoryCd { get; private set; }

        /// <summary>
        /// Docategory model
        /// </summary>
        public string CategoryName { get; private set; }

        public int IsDeleted { get; private set; }
    }

    public class RaiinListItemDto
    {
        public RaiinListItemDto(int hpId, int grpId, int kbnCd, string itemCd, long seqNo, string inputName, int isExclude, bool isAddNew, int isDeleted)
        {
            HpId = hpId;
            GrpId = grpId;
            KbnCd = kbnCd;
            ItemCd = itemCd;
            SeqNo = seqNo;
            InputName = inputName;
            IsExclude = isExclude;
            IsAddNew = isAddNew;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }

        public int GrpId { get; private set; }

        public int KbnCd { get; private set; }

        public string ItemCd { get; private set; }

        public long SeqNo { get; private set; }

        public string InputName { get; private set; }

        public int IsExclude { get; private set; }

        public bool IsAddNew { get; private set; }

        public int IsDeleted { get; private set; }
    }

    public class RaiinListFileDto
    {
        public RaiinListFileDto(int hpId, int grpId, int kbnCd, int categoryCd, string categoryName, long seqNo, int isDeleted)
        {
            HpId = hpId;
            GrpId = grpId;
            KbnCd = kbnCd;
            CategoryCd = categoryCd;
            CategoryName = categoryName;
            SeqNo = seqNo;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }

        public int GrpId { get; private set; }

        public int KbnCd { get; private set; }

        public int CategoryCd { get; private set; }

        /// <summary>
        /// FilingCategoryModel?.CategoryName
        /// </summary>
        public string CategoryName { get; private set; }


        public long SeqNo { get; private set; }

        public int IsDeleted { get; private set; }
    }

    public class KouiKbnCollectionDto
    {
        public KouiKbnCollectionDto(RaiinListKouiDto iKanModel, RaiinListKouiDto zaitakuModel, RaiinListKouiDto naifukuModel, RaiinListKouiDto tonpukuModel, RaiinListKouiDto gaiyoModel, RaiinListKouiDto hikaKinchuModel, RaiinListKouiDto jochuModel, RaiinListKouiDto tentekiModel, RaiinListKouiDto tachuModel, RaiinListKouiDto jikochuModel, RaiinListKouiDto shochiModel, RaiinListKouiDto shujutsuModel, RaiinListKouiDto masuiModel, RaiinListKouiDto kentaiModel, RaiinListKouiDto seitaiModel, RaiinListKouiDto sonohokaModel, RaiinListKouiDto gazoModel, RaiinListKouiDto rihaModel, RaiinListKouiDto seishinModel, RaiinListKouiDto hoshaModel, RaiinListKouiDto byoriModel, RaiinListKouiDto jihiModel)
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

        public RaiinListKouiDto IKanModel { get; private set; }

        public RaiinListKouiDto ZaitakuModel { get; private set; }

        public RaiinListKouiDto NaifukuModel { get; private set; }

        public RaiinListKouiDto TonpukuModel { get; private set; }

        public RaiinListKouiDto GaiyoModel { get; private set; }

        public RaiinListKouiDto HikaKinchuModel { get; private set; }

        public RaiinListKouiDto JochuModel { get; private set; }

        public RaiinListKouiDto TentekiModel { get; private set; }

        public RaiinListKouiDto TachuModel { get; private set; }

        public RaiinListKouiDto JikochuModel { get; private set; }

        public RaiinListKouiDto ShochiModel { get; private set; }

        public RaiinListKouiDto ShujutsuModel { get; private set; }

        public RaiinListKouiDto MasuiModel { get; private set; }

        public RaiinListKouiDto KentaiModel { get; private set; }

        public RaiinListKouiDto SeitaiModel { get; private set; }

        public RaiinListKouiDto SonohokaModel { get; private set; }

        public RaiinListKouiDto GazoModel { get; private set; }

        public RaiinListKouiDto RihaModel { get; private set; }

        public RaiinListKouiDto SeishinModel { get; private set; }

        public RaiinListKouiDto HoshaModel { get; private set; }

        public RaiinListKouiDto ByoriModel { get; private set; }

        public RaiinListKouiDto JihiModel { get; private set; }
    }

    public class RaiinListKouiDto
    {
        public RaiinListKouiDto(int hpId, int grpId, int kbnCd, long seqNo, int kouiKbnId, int isDeleted)
        {
            HpId = hpId;
            GrpId = grpId;
            KbnCd = kbnCd;
            SeqNo = seqNo;
            KouiKbnId = kouiKbnId;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }

        public int GrpId { get; private set; }

        public int KbnCd { get; private set; }

        public long SeqNo { get; private set; }

        public int KouiKbnId { get; private set; }

        public int IsDeleted { get; private set; }
    }
}
