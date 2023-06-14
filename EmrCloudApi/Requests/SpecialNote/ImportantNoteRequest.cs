using Domain.Models.SpecialNote.ImportantNote;

namespace EmrCloudApi.Requests.SpecialNote
{
    public class ImportantNoteRequest
    {
        public List<PtAlrgyFoodRequest> AlrgyFoodItems { get; set; } = new List<PtAlrgyFoodRequest>();
        public List<PtAlrgyElseRequest> AlrgyElseItems { get; set; } = new List<PtAlrgyElseRequest>();
        public List<PtAlrgyDrugRequest> AlrgyDrugItems { get; set; } = new List<PtAlrgyDrugRequest>();

        //Pathological
        public List<PtKioRekiRequest> KioRekiItems { get; set; } = new List<PtKioRekiRequest>();
        public List<PtInfectionRequest> InfectionsItems { get; set; } = new List<PtInfectionRequest>();

        //Interaction PtOtherDrugItem
        public List<PtOtherDrugRequest> OtherDrugItems { get; set; } = new List<PtOtherDrugRequest>();
        public List<PtOtcDrugRequest> OtcDrugItems { get; set; } = new List<PtOtcDrugRequest>();
        public List<PtSuppleRequest> SuppleItems { get; set; } = new List<PtSuppleRequest>();
        public ImportantNoteModel Map(int hpId)
        {
            return new ImportantNoteModel(AlrgyFoodItems.Select(x => x.Map(hpId)).ToList(),
                AlrgyElseItems.Select(x => x.Map(hpId)).ToList(),
                AlrgyDrugItems.Select(x => x.Map(hpId)).ToList(),
                KioRekiItems.Select(x => x.Map(hpId)).ToList(),
                InfectionsItems.Select(x => x.Map(hpId)).ToList(),
                OtherDrugItems.Select(x => x.Map(hpId)).ToList(),
                OtcDrugItems.Select(x => x.Map(hpId)).ToList(),
                SuppleItems.Select(x => x.Map(hpId)).ToList());
        }
    }
    public class PtAlrgyFoodRequest
    {
        public int HpId { get; set; }

        public long PtId { get; set; }

        public int SeqNo { get; set; }

        public int SortNo { get; set; }

        public string FoodName { get; set; } = String.Empty;

        public string AlrgyKbn { get; set; } = String.Empty;

        public int StartDate { get; set; }

        public int EndDate { get; set; }

        public string Cmt { get; set; } = String.Empty;

        public int IsDeleted { get; set; }
        public PtAlrgyFoodModel Map(int hpId)
        {
            return new PtAlrgyFoodModel(hpId, PtId, SeqNo, SortNo, AlrgyKbn, StartDate, EndDate, Cmt, IsDeleted, FoodName);
        }
    }
    public class PtAlrgyElseRequest
    {
        public int HpId { get; set; }

        public long PtId { get; set; }

        public int SeqNo { get; set; }

        public int SortNo { get; set; }

        public string AlrgyName { get; set; } = String.Empty;

        public int StartDate { get; set; }

        public int EndDate { get; set; }

        public string Cmt { get; set; } = String.Empty;

        public int IsDeleted { get; set; }
        public PtAlrgyElseModel Map(int hpId)
        {
            return new PtAlrgyElseModel(hpId, PtId, SeqNo, SortNo, AlrgyName, StartDate, EndDate, Cmt, IsDeleted);
        }
    }
    public class PtAlrgyDrugRequest
    {
        public int HpId { get; set; }

        public long PtId { get; set; }

        public int SeqNo { get; set; }

        public int SortNo { get; set; }

        public string ItemCd { get; set; } = String.Empty;

        public string DrugName { get; set; } = String.Empty;

        public int StartDate { get; set; }

        public int EndDate { get; set; }

        public string Cmt { get; set; } = String.Empty;

        public int IsDeleted { get; set; }
        public PtAlrgyDrugModel Map(int hpId)
        {
            return new PtAlrgyDrugModel(hpId, PtId, SeqNo, SortNo, ItemCd, DrugName, StartDate, EndDate, Cmt, IsDeleted);
        }
    }
    public class PtKioRekiRequest
    {
        public int HpId { get; set; }

        public long PtId { get; set; }

        public int SeqNo { get; set; }

        public int SortNo { get; set; }

        public string ByomeiCd { get; set; } = String.Empty;

        public string ByotaiCd { get; set; } = String.Empty;

        public string Byomei { get; set; } = String.Empty;

        public int StartDate { get; set; }

        public string Cmt { get; set; } = String.Empty;

        public int IsDeleted { get; set; }
        public PtKioRekiModel Map(int hpId)
        {
            return new PtKioRekiModel(hpId, PtId, SeqNo, SortNo, ByomeiCd, ByotaiCd, Byomei, StartDate, Cmt, IsDeleted);
        }
    }
    public class PtInfectionRequest
    {
        public int HpId { get; set; }

        public long PtId { get; set; }

        public long SeqNo { get; set; }

        public int SortNo { get; set; }

        public string ByomeiCd { get; set; } = String.Empty;

        public string ByotaiCd { get; set; } = String.Empty;

        public string Byomei { get; set; } = String.Empty;

        public int StartDate { get; set; }

        public string Cmt { get; set; } = String.Empty;

        public int IsDeleted { get; set; }
        public PtInfectionModel Map(int hpId)
        {
            return new PtInfectionModel(hpId, PtId, SeqNo, SortNo, ByomeiCd, ByotaiCd, Byomei, StartDate, Cmt, IsDeleted);
        }
    }
    public class PtOtherDrugRequest
    {
        public int HpId { get; set; }

        public long PtId { get; set; }

        public long SeqNo { get; set; }

        public int SortNo { get; set; }

        public string ItemCd { get; set; } = String.Empty;

        public string DrugName { get; set; } = String.Empty;

        public int StartDate { get; set; }

        public int EndDate { get; set; }

        public string Cmt { get; set; } = String.Empty;

        public int IsDeleted { get; set; }
        public PtOtherDrugModel Map(int hpId)
        {
            return new PtOtherDrugModel(hpId, PtId, SeqNo, SortNo, ItemCd, DrugName, StartDate, EndDate, Cmt, IsDeleted);
        }
    }
    public class PtOtcDrugRequest
    {
        public int HpId { get; set; }

        public long PtId { get; set; }

        public long SeqNo { get; set; }

        public int SortNo { get; set; }

        public int SerialNum { get; set; }

        public string TradeName { get; set; } = String.Empty;

        public int StartDate { get; set; }

        public int EndDate { get; set; }

        public string Cmt { get; set; } = String.Empty;

        public int IsDeleted { get; set; }
        public PtOtcDrugModel Map(int hpId)
        {
            return new PtOtcDrugModel(hpId, PtId, SeqNo, SortNo, SerialNum, TradeName, StartDate, EndDate, Cmt, IsDeleted);
        }
    }
    public class PtSuppleRequest
    {
        public int HpId { get; set; }

        public long PtId { get; set; }

        public int SeqNo { get; set; }

        public int SortNo { get; set; }

        public string IndexCd { get; set; } = String.Empty;

        public string IndexWord { get; set; } = String.Empty;

        public int StartDate { get; set; }

        public int EndDate { get; set; }

        public string Cmt { get; set; } = String.Empty;

        public int IsDeleted { get; set; }
        public PtSuppleModel Map(int hpId)
        {
            return new PtSuppleModel(hpId, PtId, SeqNo, SortNo, IndexCd, IndexWord, StartDate, EndDate, Cmt, IsDeleted);
        }
    }
}
