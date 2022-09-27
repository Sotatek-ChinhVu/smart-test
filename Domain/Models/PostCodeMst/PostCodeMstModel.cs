namespace Domain.Models.PostCodeMst
{
    public class PostCodeMstModel
    {
        public PostCodeMstModel(long id, int hpId, string postCd, string prefKana, string cityKana, string postalTermKana, string prefName, string cityName, string banti, int isDeleted)
        {
            Id = id;
            HpId = hpId;
            PostCd = postCd;
            PrefKana = prefKana;
            CityKana = cityKana;
            PostalTermKana = postalTermKana;
            PrefName = prefName;
            CityName = cityName;
            Banti = banti;
            IsDeleted = isDeleted;
        }

        public long Id { get; set; }
        public int HpId { get; set; }
        public string PostCd { get; set; }
        public string PrefKana { get; set; }
        public string CityKana { get; set; }
        public string PostalTermKana { get; set; }
        public string PrefName { get; set; }
        public string CityName { get; set; }
        public string Banti { get; set; }
        public int IsDeleted { get; set; }
    }
}
